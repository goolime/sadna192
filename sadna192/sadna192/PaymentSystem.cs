namespace sadna192
{
    public interface I_PaymentSystem
    {
        bool Connect();
        bool check_payment(string payment);
        void pay(double total, string payment);
    }
}
