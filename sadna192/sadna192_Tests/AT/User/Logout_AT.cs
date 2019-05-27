using Microsoft.VisualStudio.TestTools.UnitTesting;
using sadna192;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static sadna192_Tests.Stubs;

namespace sadna192.Tests.AcceptanceTests
{
    [TestClass()]
    public class Logout_AT
    {
        private static I_ServiceLayer serviceLayer ;
        private I_User_ServiceLayer userServiceLayer ;

        [TestInitialize]
        public void Init()
        {
            serviceLayer = new ServiceLayer();
            try
            {
                serviceLayer.Create_ServiceLayer(new Stub_deliverySystem(), new Stub_paymentSystem(), "admin", "1234Abcd");
            }
            catch (Exception) { }
            userServiceLayer = serviceLayer.Connect();
            try
            {
                userServiceLayer.Register("logoutuser", "1221Abcd");
            }
            catch (Exception) { }
            try
            {
                userServiceLayer.Login("logoutuser", "1221Abcd");
                    if (userServiceLayer.Open_Store("Grocery"))
                        userServiceLayer.Add_Product_Store("Grocery", "milk", "food", 4.5, 10, null, null);
            }
            catch (Exception) { }
        }

        [TestMethod()]
        public void Logout_happyTest1()
        {
            Assert.IsTrue(userServiceLayer.Logout());
            Assert.ThrowsException<Exception>(() => { userServiceLayer.Open_Store("mall"); }, "only subscribe users can open stores");
        }

        [TestMethod()]
        public void Logout_happyTest2()
        {
            I_User_ServiceLayer userServiceLayer2 = serviceLayer.Connect();
            if (userServiceLayer2.Register("logoutuser2", "2345Abcd"))
            { 
                userServiceLayer2.Login("logoutuser2", "2345Abcd");                
                List <ProductInStore> products = userServiceLayer2.GlobalSearch("milk", null,null, -1, -1 , -1 , -1);
                userServiceLayer2.Add_To_ShopingBasket(products[0] , 2);
                List<KeyValuePair<ProductInStore, int>> user2_cart = userServiceLayer2.Watch_Cart();

                Assert.IsTrue(userServiceLayer2.Logout());
                Assert.ThrowsException<Exception>(()=> { userServiceLayer2.Watch_Cart(); });

                userServiceLayer2.Login("logoutuser2", "2345Abcd");
                Assert.AreEqual(userServiceLayer2.Watch_Cart().Count, user2_cart.Count);
            }      
        }

        [TestMethod()]
        public void Logout_sadTest()
        {
            I_User_ServiceLayer userServiceLayer3 = serviceLayer.Connect();
            Assert.ThrowsException<Exception>(() => { userServiceLayer3.Logout(); }, "visitors user cannot perfoem logout");
        }

        [TestCleanup]
        public void TestClean()
        {
            serviceLayer.CleanUpSystem();
            serviceLayer = null;
        }
    }
}