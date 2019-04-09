using System;
using sadna192;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sadna192_Tests
{
    class Stubs
    {
        public class stub_deliverySystem : I_DeliverySystem
        {
            int index = 0;
            public virtual void canclePackage(string code)
            {

            }

            public virtual bool check_address(string address)
            {
                return true;
            }

            public virtual bool Connect()
            {
                return true;
            }

            public virtual string sendPackage(string address)
            {
                string ans = "test " + index;
                index++;
                return ans;
            }
        }

        public class stub_paymentSystem : I_PaymentSystem
        {
            public virtual bool check_payment(string payment)
            {
                return true;
            }

            public virtual bool Connect()
            {
                return true;
            }

            public virtual void pay(double total, string payment)
            {

            }
        }
    }
}
