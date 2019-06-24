using System;
using sadna192;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApplication1
{
    class Stubs
    {
        public class Stub_deliverySystem : I_DeliverySystem
        {
            int index = 0;
            public virtual Task canclePackage(string code)
            {
                return new Task(() => { });

            }

            public virtual bool check_address(string address)
            {
                return true;
            }

            public virtual Task<bool> Connect()
            {
                Task<bool> t = new Task<bool>(() => true);
                t.Start();
                return t;
            }

            public virtual Task<string> sendPackage(string address, string name, string city, string country, string zip)
            {
                string ans = "test " + index;
                index++;
                Task<string> t = new Task<string>(() => ans);
                t.Start();
                return t;
            }
        }

        public class Stub_paymentSystem : I_PaymentSystem
        {
            public virtual bool check_payment(string payment)
            {
                return true;
            }

            public virtual Task<bool> Connect()
            {
                Task<bool> t = new Task<bool>(() => true);
                t.Start();
                return t;
            }

            public virtual void pay(double total, string card_number, int month, int year, string holder, string ccv, string id)
            {

            }
        }
    }
}
