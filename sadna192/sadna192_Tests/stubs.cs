using System;
using sadna192;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sadna192_Tests
{
    public class Stubs
    {
        public class Stub_deliverySystem : I_DeliverySystem
        {
            int index = 0;
            bool isConnect = false; 

            public Stub_deliverySystem()
            {
                isConnect = true; 
            }

            public virtual void canclePackage(string code)
            {

            }

            public virtual bool check_address(string address)
            {
                return true;
            }

            public virtual bool Connect()
            {
                return isConnect;
            }

            public virtual string sendPackage(string address)
            {
                string ans = "test " + index;
                index++;
                return ans;
            }
        }

        public class Stub_paymentSystem : I_PaymentSystem
        {
            bool isConnect = false;

            public Stub_paymentSystem()
            {
                isConnect = true; 
            }

            public virtual bool check_payment(string payment)
            {
                return true;
            }

            public virtual bool Connect()
            {
                return isConnect;
            }

            public virtual void pay(double total, string payment)
            {

            }
        }
    }
}
