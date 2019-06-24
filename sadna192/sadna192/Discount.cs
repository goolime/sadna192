using System;
using System.Collections.Generic;

namespace sadna192
{
    public interface Discount
    {
        double calculate(ProductInStore p, UserState u);
        Discount Copy();
    }
    public class noDiscount : Discount
    {
        public double calculate(ProductInStore p, UserState u)
        {
            return 1;
        }

        public Discount Copy()
        {
            return new noDiscount();
        }
    }

    public abstract class multipleDiscount : Discount
    {
        internal List<Discount> discount;

        public List<Discount> getDiscount()
        {
            List<Discount> ans = new List<Discount>();
            foreach (Discount d in this.discount) ans.Add(d);
            return ans;
        }

        public abstract double calculate(ProductInStore p, UserState u);
        public abstract Discount Copy();
    }

    public class AndDiscount : multipleDiscount
    {
        public AndDiscount(List<Discount> l) : base()
        {
            this.discount = l;
        }
        public override double calculate(ProductInStore p, UserState u)
        {
            double ans = 1;
            foreach(Discount d in this.discount)
            {
                ans = ans * d.calculate(p, u);
            }
            return ans;
        }

        public override Discount Copy()
        {
            List<Discount> tmp = new List<Discount>();

            foreach (Discount d in base.discount)
            {
                tmp.Add(d.Copy());
            }

            return new AndDiscount(tmp);
        }
    }

    public class XOrDiscount : multipleDiscount
    {
        public XOrDiscount(List<Discount> l) : base()
        {
            ServiceLayer SL = new ServiceLayer();
            this.discount = l;
        }
        public override double calculate(ProductInStore p, UserState u)
        {
            double ans = 1;
            foreach (Discount d in this.discount)
            {
                double tmp = d.calculate(p, u);
                if (tmp < ans) ans = tmp;
            }
            return ans;
        }
        public override Discount Copy()
        {
            List<Discount> tmp = new List<Discount>();

            foreach (Discount d in base.discount)
            {
                tmp.Add(d.Copy());
            }

            return new XOrDiscount(tmp);
        }
    }

    public class IncludeStoreDiscount : Discount
    {
        public double calculate(ProductInStore p, UserState u)
        {
            return p.getStore().GetDiscount().calculate(p, u);
        }

        public Discount Copy()
        {
            return new IncludeStoreDiscount();
        }
    }

    public class ProductAmountDiscount : Discount
    {
        string product;
        int amount;
        double discount;

        public ProductAmountDiscount(string product, int i,double discount)
        {
            this.amount = i;
            this.product = product;
            this.discount = discount;
        }

        public double calculate(ProductInStore p, UserState u)
        {
            if (u.numOfItemsInCart(p.getStore().getName(), this.product) >= this.amount) return this.discount;
            else return 1;
        }

        public Discount Copy()
        {
            return new ProductAmountDiscount("" + this.product, this.amount, this.discount);
        }
    }

    public class ProductAmountInBasketDiscount : Discount
    {
        int amount;
        double discount;

        public ProductAmountInBasketDiscount(int i, double discount)
        {
            this.amount = i;
            this.discount = discount;
        }

        public double calculate(ProductInStore p, UserState u)
        {
            if (u.numOfItemsInCart(p.getStore().getName()) >= this.amount) return this.discount;
            else return 1;
        }

        public Discount Copy()
        {
            return new ProductAmountInBasketDiscount(this.amount, this.discount);
        }
    }

    public class TimeDiscount : Discount
    {
        DateTime from, to;
        Discount discount;

        public TimeDiscount(DateTime from, DateTime to, Discount discount)
        {
            this.from = from;
            this.to = to;
            this.discount = discount;
        }

        public double calculate(ProductInStore p, UserState u)
        {
            if (from<DateTime.Now && DateTime.Now<to) return discount.calculate(p,u);
            else return 1;
        }

        public Discount Copy()
        {
            return new TimeDiscount(new DateTime(this.from.Ticks), new DateTime(this.to.Ticks), this.discount);
        }
    }

    public class fixDiscount:Discount
    {
        double discount;

        public fixDiscount(double discount)
        {
            this.discount = discount;
        }

        public double calculate(ProductInStore p, UserState u)
        {
            return this.discount;
        }

        public Discount Copy()
        {
            return new fixDiscount(this.discount);
        }
    }
}



       