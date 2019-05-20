using System;
using System.Collections.Generic;

namespace sadna192
{
    public interface Discount
    {
        double calculate(ProductInStore p, UserState u);
    }
    public class noDiscount : Discount
    {
        public double calculate(ProductInStore p, UserState u)
        {
            return 1;
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
    }

    public class IncludeStoreDiscount : Discount
    {
        public double calculate(ProductInStore p, UserState u)
        {
            return p.getStore().GetDiscount().calculate(p, u);
        }
    }

}



       