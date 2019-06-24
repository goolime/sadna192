using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace sadna192
{
    public abstract class Policy
    {
        public int PolicyID { get; set; }
        public abstract bool check(ProductInStore p, UserState u);
        public abstract bool immidiate();
        public abstract Policy Copy();
    }

    public class regularPolicy: Policy
    {

    public regularPolicy() {}
    public override bool check(ProductInStore p, UserState u)
        {
            return true;
        }

        public override Policy Copy()
        {
            return new regularPolicy();
        }

        public override bool immidiate()
        {
            return false;
        }

        public static regularPolicy creteRegularPolicy()
        {
            regularPolicy rgp = new regularPolicy();
            if (!DBAccess.SaveToDB(rgp))
                DBAccess.DBerror("could not save regular policy to DB ");
            return rgp;
        }
    }

    public class immidiatePolicy: Policy
    {

    public immidiatePolicy() { }

    public override bool check(ProductInStore p, UserState u)
        {
            return true;
        }

        public override Policy Copy()
        {
            return new immidiatePolicy();
        }

        public override bool immidiate()
        {
            return true; 
        }
    }

    public abstract class MultiplePolicy : Policy
    {

    public MultiplePolicy() { }

    public List<Policy> Policies { get; set; }

        public List<Policy> getPolicies()
        {
            List<Policy> ans = new List<Policy>();
            foreach (Policy p in this.Policies) ans.Add(p);
            return ans;
        }
    }

    public class OrPolicy : MultiplePolicy
    {
    public OrPolicy() { }

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
    public AndPolicy() { }

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
    public XOrPolicy() { }

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
    public IncludeStorePolicy() { }

    public override bool  check(ProductInStore p, UserState u)
        {
            return p.getStore().GetPolicy().check(p, u);
        }

        public override Policy Copy()
        {
            return new IncludeStorePolicy();
        }

        public override bool immidiate()
        {
            return false;
        }
    }

    public class MamberPolicy : Policy
    {
    public MamberPolicy() { }

    public override bool check(ProductInStore p, UserState u)
        {
            return u.isMember();
        }

        public override Policy Copy()
        {
            return new MamberPolicy();
        }

        public override bool immidiate()
        {
            return false;
        }
    }
    
    public class MinimumInCart:Policy
    {
        [Column("min")]
        public int min { get; set; }
        public MinimumInCart() { }

    
        public MinimumInCart(int i)
        {
            this.min = i;
        }

        public override bool check(ProductInStore p, UserState u)
        {
            return u.numOfItemsInCart(p.getStore().getName()) >= this.min;
        }

        public override Policy Copy()
        {
            return new MinimumInCart(this.min);
        }

        public override bool immidiate()
        {
            return false;
        }
    }

    public class MinimumProductInCart : Policy
    {
        [Column("min")]
        public int min { get; set; }

        public MinimumProductInCart() { }

    public MinimumProductInCart(int i)
        {
            this.min = i;
        }


        public override bool check(ProductInStore p, UserState u)
        {
            return u.numOfItemsInCart(p.getStore().getName(),p.getName()) >= this.min;
        }

        public override Policy Copy()
        {
            return new MaximumProductInCart(this.min);
        }

        public override bool immidiate()
        {
            return false;
        }
    }

    public class MaximumInCart : Policy
    {
        [Column("max")]
        public int max { get; set; }

        public MaximumInCart() { }

        public MaximumInCart(int i)
        {
            this.max = i;
        }

        public override bool check(ProductInStore p, UserState u)
        {
            return u.numOfItemsInCart(p.getStore().getName()) <= this.max;
        }

        public override Policy Copy()
        {
            return new MaximumInCart(this.max);
        }

        public override bool immidiate()
        {
            return false;
        }
    }

    public class MaximumProductInCart : Policy
    {
        [Column("max")]
        public int max { get; set; }

        public MaximumProductInCart() { }

    public MaximumProductInCart(int i)
        {
            this.max = i;
        }

        public override bool check(ProductInStore p, UserState u)
        {
            return u.numOfItemsInCart(p.getStore().getName(), p.getName()) <= this.max;
        }

        public override Policy Copy()
        {
            return new MaximumProductInCart(this.max);
        }

        public override bool immidiate()
        {
            return false;
        }
    }

    public class TimePolicy:Policy
    {
        public DateTime from { get; set; }
        public DateTime to { get; set; }
        public TimePolicy() { }

    public TimePolicy(DateTime from, DateTime to)
        {
            this.from = from;
            this.to = to;
        }

        public override bool check(ProductInStore p, UserState u)
        {
            DateTime now = DateTime.Now;
            return this.from < now && now < this.to;
        }

        public override Policy Copy()
        {
            return new TimePolicy(new DateTime(this.from.Ticks), new DateTime(this.to.Ticks));
        }

        public override bool immidiate()
        {
            return false;
        }
    }
}