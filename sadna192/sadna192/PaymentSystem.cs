using System.Threading.Tasks;

namespace sadna192
{
    public interface I_PaymentSystem
    {
        Task<bool> Connect();
        bool check_payment(string payment);
         void pay(double total, string card_number, int month, int year, string holder, string ccv, string id);
    }
}
