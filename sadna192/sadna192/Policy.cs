using System;
using System.Collections.Generic;

namespace sadna192
{
    public interface Policy
    {
        bool check(ProductInStore p, UserState u);
        bool immidiate();
        Policy Copy();
    }

    public class regularPolicy: Policy
    {
        public bool check(ProductInStore p, UserState u)
        {
            return true;
        }

        public Policy Copy()
        {
            return new regularPolicy();
        }

        public bool immidiate()
        {
            return false;
        }
    }

    public class immidiatePolicy: Policy
    {
        public bool check(ProductInStore p, UserState u)
        {
            return true;
        }

        public Policy Copy()
        {
            return new immidiatePolicy();
        }

        public bool immidiate()
        {
            return true; 
        }
    }

    public abstract class MultiplePolicy : Policy
    {
        internal List<Policy> Policies;

        public List<Policy> getPolicies()
        {
            List<Policy> ans = new List<Policy>();
            foreach (Policy p in this.Policies) ans.Add(p);
            return ans;
        }
        public abstract bool check(ProductInStore p, UserState u);
        public abstract bool immidiate();
        public abstract Policy Copy();
    }

    public class OrPolicy : MultiplePolicy
    {
        public OrPolicy(List<Policy> l):base()
        {
            this.Policies = l;
        }
        public override bool check(ProductInStore p, UserState u)
        {
            foreach (Policy poli in this.Policies)
            {
                if (poli.check(p, u)) return true;
            }
            return false;
        }

        public override Policy Copy()
        {
            List<Policy> tmp = new List<Policy>();
            foreach (Policy p in base.Policies)
            {
                tmp.Add(p.Copy());
            }
            return new OrPolicy(tmp);
        }

        public override bool immidiate()
        {
            foreach (Policy poli in this.Policies)
            {
                if (poli.immidiate()) return true;
            }
            return false;
        }
    }

    public class AndPolicy : MultiplePolicy
    {
        public AndPolicy(List<Policy> l) : base()
        {
            this.Policies = l;
        }
        public override bool check(ProductInStore p, UserState u)
        {
            foreach (Policy poli in this.Policies)
            {
                if (!poli.check(p, u)) return false;
            }
            return true;
        }
        public override Policy Copy()
        {
            List<Policy> tmp = new List<Policy>();
            foreach (Policy p in base.Policies)
            {
                tmp.Add(p.Copy());
            }
            return new AndPolicy(tmp);
        }

        public override bool immidiate()
        {
            foreach (Policy poli in this.Policies)
            {
                if (!poli.immidiate()) return false;
            }
            return true;
        }
    }

    public class XOrPolicy : MultiplePolicy
    {
        public XOrPolicy(List<Policy> l) : base()
        {
            this.Policies = l;
        }
        public override bool check(ProductInStore p, UserState u)
        {
            bool ans = false;
            foreach (Policy poli in this.Policies)
            {
                if (poli.check(p, u))
                {
                    if (ans) return false;
                    else ans = true;
                }

            }
            return ans;
        }

        public override Policy Copy()
        {
            List<Policy> tmp = new List<Policy>();
            foreach (Policy p in base.Policies)
            {
                tmp.Add(p.Copy());
            }
            return new XOrPolicy(tmp);
        }

        public override bool immidiate()
        {
            bool ans = false;
            foreach (Policy poli in this.Policies)
            {
                if (poli.immidiate())
                {
                    if (ans) return false;
                    else ans = true;
                }
            }
            return ans;
        }
    }

    public class IncludeStorePolicy : Policy
    {
        public bool check(ProductInStore p, UserState u)
        {
            return p.getStore().GetPolicy().check(p, u);
        }

        public Policy Copy()
        {
            return new IncludeStorePolicy();
        }

        public bool immidiate()
        {
            return false;
        }
    }

    public class MamberPolicy : Policy
    {
        public bool check(ProductInStore p, UserState u)
        {
            return u.isMember();
        }

        public Policy Copy()
        {
            return new MamberPolicy();
        }

        public bool immidiate()
        {
            return false;
        }
    }
    
    public class MinimumInCart:Policy
    {
        int min;
        public MinimumInCart(int i)
        {
            this.min = i;
        }

        public bool check(ProductInStore p, UserState u)
        {
            return u.numOfItemsInCart(p.getStore().getName()) >= this.min;
        }

        public Policy Copy()
        {
            return new MinimumInCart(this.min);
        }

        public bool immidiate()
        {
            return false;
        }
    }

    public class MinimumProductInCart : Policy
    {
        int min;
        public MinimumProductInCart(int i)
        {
            this.min = i;
        }


        public bool check(ProductInStore p, UserState u)
        {
            return u.numOfItemsInCart(p.getStore().getName(),p.getName()) >= this.min;
        }

        public Policy Copy()
        {
            return new MaximumProductInCart(this.min);
        }

        public bool immidiate()
        {
            return false;
        }
    }

    public class MaximumInCart : Policy
    {
        int max;
        public MaximumInCart(int i)
        {
            this.max = i;
        }

        public bool check(ProductInStore p, UserState u)
        {
            return u.numOfItemsInCart(p.getStore().getName()) <= this.max;
        }

        public Policy Copy()
        {
            return new MaximumInCart(this.max);
        }

        public bool immidiate()
        {
            return false;
        }
    }

    public class MaximumProductInCart : Policy
    {
        int max;
        public MaximumProductInCart(int i)
        {
            this.max = i;
        }

        public bool check(ProductInStore p, UserState u)
        {
            return u.numOfItemsInCart(p.getStore().getName(), p.getName()) <= this.max;
        }

        public Policy Copy()
        {
            return new MaximumProductInCart(this.max);
        }

        public bool immidiate()
        {
            return false;
        }
    }

    public class TimePolicy:Policy
    {
        DateTime from;
        DateTime to;
        public TimePolicy(DateTime from, DateTime to)
        {
            this.from = from;
            this.to = to;
        }

        public bool check(ProductInStore p, UserState u)
        {
            DateTime now = DateTime.Now;
            return this.from < now && now < this.to;
        }

        public Policy Copy()
        {
            return new TimePolicy(new DateTime(this.from.Ticks), new DateTime(this.to.Ticks));
        }

        public bool immidiate()
        {
            return false;
        }
    }
}