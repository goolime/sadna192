using System;
using System.Collections.Generic;

namespace sadna192
{
    public interface Policy
    {

        bool Check(ProductInStore p, UserState u);
        bool Immidiate();
    }

    public abstract class DecoratorPolicy
    {
        public Policy Policy { get; set; }
        public DecoratorPolicy()
        {
        }
        public abstract bool MyCheck(ProductInStore p, UserState u);
        public bool Immidiate()
        {
            return Policy.Immidiate();
        }

        public void AttachPolicy(Policy p)
        {
            Policy = p;
        }

        public bool Check(ProductInStore p, UserState u)
        {
            return MyCheck(p, u) && Policy.Check(p, u);
        }
    }

    public class RegularPolicy: Policy
    {

        public bool Check(ProductInStore p, UserState u)
        {
            return true;
        }

        public bool Immidiate()
        {
            return false;
        }
    }

    public class ImmidiatePolicy: Policy
    {
        public bool Check(ProductInStore p, UserState u)
        {
            return true;
        }
       
        public bool Immidiate()
        {
            return true; 
        }
    }

    public class BasicRegularDecoratorPolicy : DecoratorPolicy
    {
        public BasicRegularDecoratorPolicy()
        {
            Policy = new RegularPolicy();
        }

        public override bool MyCheck(ProductInStore p, UserState u)
        {
            return Policy.Check(p, u);
        }
    }

    public class BasicImmidiateDecoratorPolicy : DecoratorPolicy
    {
        public BasicImmidiateDecoratorPolicy()
        {
            Policy = new ImmidiatePolicy();
        }

        public override bool MyCheck(ProductInStore p, UserState u)
        {
            return Policy.Check(p, u);
        }
    }

    public abstract class MultiplePolicy : DecoratorPolicy
    {
        internal List<DecoratorPolicy> Policies;

        public MultiplePolicy()
        {
        }

        public List<DecoratorPolicy> GetPolicies()
        {
            List<DecoratorPolicy> ans = new List<DecoratorPolicy>();
            foreach (DecoratorPolicy p in this.Policies) ans.Add(p);
            return ans;
        }
    }

    public class OrPolicy : MultiplePolicy
    {
        public OrPolicy(List<DecoratorPolicy> l,Policy p)
        {
            this.Policies = l;
        }
        public override bool MyCheck(ProductInStore p, UserState u)
        {
            foreach (DecoratorPolicy poli in this.Policies)
            {
                if (poli.MyCheck(p, u)) return true;
            }
            return false;
        }

    }

    public class AndPolicy : MultiplePolicy
    {
        public AndPolicy(List<DecoratorPolicy> l)
        {
            this.Policies = l;
        }
        public override bool MyCheck(ProductInStore p, UserState u)
        {
            foreach (DecoratorPolicy poli in this.Policies)
            {
                if (!poli.MyCheck(p, u)) return false;
            }
            return true;
        }
    }

    public class XOrPolicy : MultiplePolicy
    {
        public XOrPolicy(List<DecoratorPolicy> l,Policy p) 
        {
            this.Policies = l;
        }
        public override bool MyCheck(ProductInStore p, UserState u)
        {
            bool ans = false;
            foreach (DecoratorPolicy poli in this.Policies)
            {
                if (poli.MyCheck(p, u))
                {
                    if (ans) return false;
                    else ans = true;
                }

            }
            return ans;
        }
    }

    public class IncludeStorePolicy : DecoratorPolicy
    {
        public override bool MyCheck(ProductInStore p, UserState u)
        {
            return p.getStore().GetPolicy().Check(p, u);
        }
    }

    public class MamberPolicy : DecoratorPolicy
    {
        public override bool MyCheck(ProductInStore p, UserState u)
        {
            return u.isMember();
        }

    }
    
    public class MinimumInCart:DecoratorPolicy
    {
        int min;
        public MinimumInCart(int i)
        {
            this.min = i;
        }

        public override bool MyCheck(ProductInStore p, UserState u)
        {
            return u.numOfItemsInCart(p.getStore().getName()) >= this.min;
        }
    }

    public class MinimumProductInCart : DecoratorPolicy
    {
        int min;
        public MinimumProductInCart(int i)
        {
            this.min = i;
        }

        public override bool MyCheck(ProductInStore p, UserState u)
        {
            return u.numOfItemsInCart(p.getStore().getName(),p.getName()) >= this.min;
        }
    }

    public class MaximumInCart : DecoratorPolicy
    {
        int max;
        public MaximumInCart(int i)
        {
            this.max = i;
        }

        public override bool MyCheck(ProductInStore p, UserState u)
        {
            return u.numOfItemsInCart(p.getStore().getName()) <= this.max;
        }

    }

    public class MaximumProductInCart : DecoratorPolicy
    {
        int max;
        public MaximumProductInCart(int i)
        {
            this.max = i;
        }

        public override bool MyCheck(ProductInStore p, UserState u)
        {
            return u.numOfItemsInCart(p.getStore().getName(), p.getName()) <= this.max;
        }
    }

    public class TimePolicy:DecoratorPolicy
    {
        DateTime from;
        DateTime to;
        public TimePolicy(DateTime from, DateTime to)
        {
            this.from = from;
            this.to = to;
        }

        public override bool MyCheck(ProductInStore p, UserState u)
        {
            DateTime now = DateTime.Now;
            return this.from < now && now < this.to;
        }
    }

    public class ProductPolicy : DecoratorPolicy
    {
        public string Product { get; set; }

        public override bool MyCheck(ProductInStore p, UserState u)
        {
            return Product == p.getName();
        }
    }

    public class MaxProductPolicy : DecoratorPolicy
    {
        public string Product { get; set; }
        public int Value { get; set; }

        public override bool MyCheck(ProductInStore p, UserState u)
        {
            return p.getName()!=Product || u.numOfItemsInCart(p.getStore().getName(), p.getName()) <= this.Value;
        }
    }

    public class MinProductPolicy : DecoratorPolicy
    {
        public string Product { get; set; }
        public int Value { get; set; }

        public override bool MyCheck(ProductInStore p, UserState u)
        {
            return p.getName() != Product || u.numOfItemsInCart(p.getStore().getName(), p.getName()) >= this.Value;
        }
    }
}