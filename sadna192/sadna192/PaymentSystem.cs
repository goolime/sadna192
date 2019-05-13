namespace sadna192
{
    public interface I_PaymentSystem
    {
        bool Connect();
        bool check_payment(string payment);
        
        /// <summary>
        ///  return the amount of money that couldn't be charged.
        /// </summary>
        /// <param name="total"></param> the amount to pay.
        /// <param name="payment"></param> the payment system.
        /// <returns></returns>
        int pay(double total, string payment);
    }
}
