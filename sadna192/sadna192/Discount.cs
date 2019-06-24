using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace sadna192
{
    public abstract class Discount
    {
        public int DiscountID { get; set; }
        public abstract double calculate(ProductInStore p, UserState u);
        public Discount(){}
        Discount Copy();
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
    public class AndDiscount : multipleDiscount
    {
        public AndDiscount(List<Discount> l) : base()
        {
            ServiceLayer SL = new ServiceLayer();
            this.discount = l;
        }
        public override double calculate(ProductInStore p, UserState u)
        {
            return 1;
        }
    }

        public static noDiscount creteNoDiscount()
        {
            noDiscount nod = new noDiscount();
            if (!DBAccess.SaveToDB(nod))
                DBAccess.DBerror("could not save noDiscount to DB ");
            return nod;

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
    }

    public class IncludeStoreDiscount : Discount
    {

        public IncludeStoreDiscount()
        {

        }
        public override double calculate(ProductInStore p, UserState u)
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
        public ProductAmountDiscount()
        {

        }
        public string product { get; set; }
        [Column("amount")]
        public int amount { get; set; }
        [Column("discount")]
        public double discount { get; set; }

        public ProductAmountDiscount(string product, int i, double discount)
        {
            this.amount = i;
            this.product = product;
            this.discount = discount;
        }

        public override double calculate(ProductInStore p, UserState u)
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

        public ProductAmountInBasketDiscount()
        {

        }

        [Column("amount")]
        public int amount { get; set; }
        [Column("discount")]
        public double discount { get; set; }

        public ProductAmountInBasketDiscount(int i, double discount)
        {
            this.amount = i;
            this.discount = discount;
        }

        public override double calculate(ProductInStore p, UserState u)
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

        public TimeDiscount()
        {

        }

        [Column("discount")]
        public double discount { get; set; }
        DateTime from, to;
        double discount;

        public TimeDiscount(DateTime from, DateTime to, Discount discount)
        {
            this.from = from;
            this.to = to;
            this.discount = discount;
        }

        public override double calculate(ProductInStore p, UserState u)
        {
            if (from < DateTime.Now && DateTime.Now < to) return this.discount;
            else return 1;
        }

        public Discount Copy()
        {
            return new TimeDiscount(new DateTime(this.from.Ticks), new DateTime(this.to.Ticks), this.discount);
        }
    }

    public class fixDiscount : Discount
    {

        public fixDiscount()
        {

        } 

        [Column("discount")]
        public double discount { get; set; }

        public fixDiscount(double discount)
        {
            this.discount = discount;
        }

        public override double calculate(ProductInStore p, UserState u)
        {
            return this.discount;
        }

        public Discount Copy()
        {
            return new fixDiscount(this.discount);
        }
    }

    public abstract class multipleDiscount : Discount
    {

        public multipleDiscount()
        {
        }
        public List<Discount> discount { get; set; }

        public List<Discount> getDiscount()
        {
            List<Discount> ans = new List<Discount>();
            foreach (Discount d in this.discount) ans.Add(d);
            return ans;
        }

       // public abstract double calculate(ProductInStore p, UserState u);
    }

    public class AndDiscount : multipleDiscount
    {
        public AndDiscount()
        {
        }

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
    }

    public class XOrDiscount : multipleDiscount
    {
        public XOrDiscount()
        {
        }

        public XOrDiscount(List<Discount> l) : base()
        {
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
    }
}



       