using System.Threading.Tasks;

namespace sadna192
{
    public interface I_DeliverySystem
    {
        Task<bool> Connect();
        bool check_address(string address);
        Task<string> sendPackage(string address, string name, string city, string country, string zip);
        Task canclePackage(string code);
    }
}
