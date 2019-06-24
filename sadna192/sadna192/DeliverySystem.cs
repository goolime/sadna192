namespace sadna192
{
    public interface I_DeliverySystem
    {
        bool Connect();
        bool check_address(string address);
        string sendPackage(string address);
        void canclePackage(string code);
    }
}
