using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sadna192
{
    public interface I_DeliverySystem
    {
        bool Connect();
        bool check_address(string address);
    }
}
