using Microsoft.VisualStudio.TestTools.UnitTesting;
using sadna192;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static sadna192_Tests.Stubs;

namespace sadna192.Tests
{
    [TestClass()]
    public class Logout_AT
    {
        private static I_ServiceLayer serviceLayer ;
        private I_User_ServiceLayer userServiceLayer ;

        [TestInitialize]
        public void init()
        {
            serviceLayer = new ServiceLayer();
            serviceLayer.Create_ServiceLayer(new stub_deliverySystem(), new stub_paymentSystem(), "admin", "1234");
            userServiceLayer = serviceLayer.Connect();
            if (userServiceLayer.Register("logout_user", "1221"))
                if (userServiceLayer.Login("logout_user", "1221"))
                    if (userServiceLayer.Open_Store("Grocery"))
                         userServiceLayer.Add_Product_Store("Grocery", "milk", "food", 4.5 ,10 , null, null);
        }

        [TestMethod()]
        public void Logout_happy_test1()
        {
            Assert.IsTrue(userServiceLayer.Logout());
            Assert.ThrowsException<Exception>(() => { userServiceLayer.Open_Store("mall"); }, "only subscribe users can open stores");
        }

        [TestMethod()]
        public void Logout_happy_test2()
        {
            I_User_ServiceLayer userServiceLayer2 = serviceLayer.Connect();
            if (userServiceLayer2.Register("logout_user2", "2345"))
            { 
                userServiceLayer2.Login("logout_user2", "2345");                
                List <ProductInStore> products = userServiceLayer2.GlobalSearch("milk", "food",null, 0, 10 , 0 , 0);
                userServiceLayer2.Add_To_ShopingBasket(products[0] , 2);
                List<KeyValuePair<ProductInStore, int>> user2_cart = userServiceLayer2.Watch_Cart();

                userServiceLayer2.Logout();
                Assert.AreNotSame(userServiceLayer2.Watch_Cart() , user2_cart);

                userServiceLayer2.Login("logout_user2", "2345");
                Assert.AreSame(userServiceLayer2.Watch_Cart(), user2_cart);
            }      
        }

        [TestMethod()]
        public void Logout_sad_test()
        {
            I_User_ServiceLayer userServiceLayer3 = serviceLayer.Connect();
            Assert.ThrowsException<Exception>(() => { userServiceLayer3.Logout(); }, "visitors user cannot perfoem logout");
            Assert.ThrowsException<Exception>(() => { userServiceLayer3.Open_Store("mall"); }, "only subscribe users can open stores");
        }


    }
}