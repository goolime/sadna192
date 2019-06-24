using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Net.Http;

namespace WebApplication1
{
    public class herokuapp_wsep_192
    {
        public class paymentSystem : sadna192.I_PaymentSystem
        {
            private static readonly HttpClient client = new HttpClient();
            public bool check_payment(string payment)
            {
                return true;
            }


            public async Task<bool> Connect()
            {
                var values = new Dictionary<string, string>
                {
                    { "action_type", "handshake" }
                };

                var content = new FormUrlEncodedContent(values);

                var response = await client.PostAsync("https://cs-bgu-wsep.herokuapp.com/", content);

                //var responseString = await response.Content.ReadAsStringAsync();
                return response.StatusCode == System.Net.HttpStatusCode.OK;
            }



            public async void pay(double total, string card_number,int month,int year,string holder,string ccv,string id)
            {
                var values = new Dictionary<string, string>
                {
                 { "action_type", "pay" },
                 { "card_number", card_number},
                 { "month", month+"" },
                 { "year", year+"" },
                 { "holder", holder },
                 { "ccv", ccv },
                 { "id", id }                };

                var content = new FormUrlEncodedContent(values);

                var response = await client.PostAsync("https://cs-bgu-wsep.herokuapp.com/", content);

                var responseString = await response.Content.ReadAsStringAsync();
                if (responseString == "-1")
                {
                    throw new sadna192.Sadna192Exception("could not use external system to pay","paymentSystem","pay");
                }
            }
        }

        public class deliverySystem : sadna192.I_DeliverySystem
        {
            private static readonly HttpClient client = new HttpClient();

            public async Task canclePackage(string code)
            {
                var values = new Dictionary<string, string>
                {
                 { "action_type", "cancel_supply" },
                 { "transaction_id", code }
                };

                var content = new FormUrlEncodedContent(values);

                var response = await client.PostAsync("https://cs-bgu-wsep.herokuapp.com/", content);

                var responseString = await response.Content.ReadAsStringAsync();
                if (responseString == "-1")
                {
                    throw new sadna192.Sadna192Exception("could not use external system to cancel delivery", "deliverySystem", "canclePackage");
                }
            }

            public bool check_address(string address)
            {
                return true;
            }

            public async Task<bool> Connect()
            {
                var values = new Dictionary<string, string>
                {
                    { "action_type", "handshake" }
                };

                var content = new FormUrlEncodedContent(values);

                var response = await client.PostAsync("https://cs-bgu-wsep.herokuapp.com/", content);

                //var responseString = await response.Content.ReadAsStringAsync();
                return response.StatusCode == System.Net.HttpStatusCode.OK;
            }

            public async Task<string> sendPackage(string address,string name,string city,string country,string zip)
            {
                var values = new Dictionary<string, string>
                {
                  { "action_type", "supply" },
                  { "name",name },
                  { "address", address },
                  { "city", city},
                  { "country",country },
                  { "zip", zip},                };

                var content = new FormUrlEncodedContent(values);

                var response = await client.PostAsync("https://cs-bgu-wsep.herokuapp.com/", content);

                var responseString = await response.Content.ReadAsStringAsync();
                if (responseString == "-1")
                {
                    throw new sadna192.Sadna192Exception("could not use external system to pay", "paymentSystem", "pay");
                }
                return responseString;
            }
        }
        
    }

}
