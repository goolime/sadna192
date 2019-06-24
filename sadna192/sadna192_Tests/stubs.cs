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
                Task<bool> t = new Task<bool>(() => isConnect);
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
            bool isConnect = false;

            public Stub_paymentSystem()
            {
                isConnect = true; 
            }

            public virtual bool check_payment(string payment)
            {
                return true;
            }

            public virtual Task<bool> Connect()
            {
                Task<bool> t = new Task<bool>(() => isConnect);
                    t.Start();
                return t;
            }

            public virtual void pay(double total, string card_number, int month, int year, string holder, string ccv, string id)
            {

            }
        }

        public class Stub_Alerter : sadna192.Alerter
        {
            public bool AlertUser(string messege)
            {
                return true;
            }

            public List<string> LastNotifications()
            {
                return new List<string>();
            }
        }
    }
}
