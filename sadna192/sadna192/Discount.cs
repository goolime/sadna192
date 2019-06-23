using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace sadna192
{
    public abstract class Discount
    {
        public int DiscountID { get; set; }
        public abstract double calculate(ProductInStore p, UserState u);

        public Discount()
        {
            
        }
    }
    public class noDiscount : Discount
    {
        public noDiscount()
        {

        }
        public override double calculate(ProductInStore p, UserState u)
        {
            return 1;
        }

        public static noDiscount creteNoDiscount()
        {
            noDiscount nod = new noDiscount();
            if (!DBAccess.SaveToDB(nod))
                DBAccess.DBerror("could not save noDiscount to DB ");
            return nod;

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
    }

    public class TimeDiscount : Discount
    {

        public TimeDiscount()
        {

        }

        [Column("discount")]
        public double discount { get; set; }
        DateTime from, to;

        public TimeDiscount(DateTime from, DateTime to, double discount)
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



       