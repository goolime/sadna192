using System;

namespace sadna192
{
    public interface Discount
    {
        double calculate(int amount, double price);
    }
    public class noDiscount:Discount
    {
        public double calculate(int amount,double price)
        {
            return 0;
        }
    }
}