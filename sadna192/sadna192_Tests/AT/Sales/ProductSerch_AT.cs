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
    public class ProductSearch_AT
    {
        private static I_ServiceLayer serviceLayer;
        private I_User_ServiceLayer userServiceLayer1;
        private I_User_ServiceLayer userServiceLayer2;
        private I_User_ServiceLayer userServiceLayer3;

        [TestInitialize]
        public void Init()
        {
            serviceLayer = new ServiceLayer();
            try
            {
                serviceLayer.Create_ServiceLayer(new Stub_deliverySystem(), new Stub_paymentSystem(), "admin1", "1234Asdf");
            }
            catch (Exception) { }
            userServiceLayer1 = serviceLayer.Connect(new Stub_Alerter());
            userServiceLayer2 = serviceLayer.Connect(new Stub_Alerter());
            userServiceLayer3 = serviceLayer.Connect(new Stub_Alerter());

            try
            {
                if (userServiceLayer1.Register("productSearchuser", "1221Abcd"))
                    if (userServiceLayer1.Login("productSearchuser", "1221Abcd"))
                        if (userServiceLayer1.Open_Store("Grocery"))
                            userServiceLayer1.Add_Product_Store("Grocery", "milk", "food", 4.5, 10, null, null);


                if (userServiceLayer2.Register("productSearchuser2", "9876Abcd"))
                    if (userServiceLayer2.Login("productSearchuser2", "9876Abcd"))
                        if (userServiceLayer2.Open_Store("handm") && userServiceLayer2.Open_Store("Market"))
                        {
                            userServiceLayer2.Add_Product_Store("handm", "dress", "clothes", 29.90, 5, null, null);
                            userServiceLayer2.Add_Product_Store("Market", "milk", "food", 3, 6, null, null);
                        }


                if (userServiceLayer3.Register("productSearchuser3", "4455Abcd"))
                    userServiceLayer3.Login("productSearchuser3", "4455Abcd");
            }
            catch (Exception) { }
        }

        [TestMethod()]
        public void Product_Search_by_Category_happyTest()
        {
            List<ProductInStore> foodSearch = userServiceLayer3.GlobalSearch(null, "food", null, -1, -1, -1, -1);
            Assert.AreEqual(foodSearch.Count, 2);
        }

        [TestMethod()]
        public void Product_Search_by_price_happyTest()
        {
            List<ProductInStore> minPriceSearch = userServiceLayer3.GlobalSearch(null, null, null, 5, -1, -1, -1);
            Assert.AreEqual(minPriceSearch.Count, 1);
            List<ProductInStore> maxPriceSearch = userServiceLayer3.GlobalSearch(null, null, null, -1, 5, -1, -1);
            Assert.AreEqual(maxPriceSearch.Count, 2);
        }

        [TestMethod()]
        public void Product_search_by_name_happyTest()
        {
            List<ProductInStore> milkSearch = userServiceLayer3.GlobalSearch("milk", null, null, -1, -1, -1, -1);
            Assert.AreEqual(milkSearch.Count, 2);
           
        }


        [TestMethod()]
        public void Product_search_sadTest()
        {       
            Assert.ThrowsException<Exception>(() => { userServiceLayer3.GlobalSearch("", "food", null, 0, -2, 0, 0); }, "searching with wrong type of product name");
            Assert.ThrowsException<Exception>(() => { userServiceLayer3.GlobalSearch("milk‚Î", null, null, 0, 20, 0, 0); }, "searching with wrong type of catagory");
            Assert.ThrowsException<Exception>(() => { userServiceLayer3.GlobalSearch("milk", null, null, -2, 0, 0, 0); }, "searching with wrong type of keywords list");
            Assert.ThrowsException<Exception>(() => { userServiceLayer3.GlobalSearch("milk", "food", null, 40, 20, 0, 0); }, "searching with min price > max price");
            Assert.ThrowsException<Exception>(() => { userServiceLayer3.GlobalSearch("milk", "food", null, -2, 20, 0, 0); }, "searching with wrong type of min price");
            Assert.ThrowsException<Exception>(() => { userServiceLayer3.GlobalSearch("milk", "food", null, 0, -5, 0, 0); }, "searching with wrong type of max price");
            Assert.ThrowsException<Exception>(() => { userServiceLayer3.GlobalSearch("milk", "food", null, 0, 20, -0.5, 0); }, "searching with wrong type of store rank");
            Assert.ThrowsException<Exception>(() => { userServiceLayer3.GlobalSearch("milk", "food", null, 0, 20, 0, -1.3); }, "searching with wrong type of product rank");
        }

        [TestCleanup]
        public void TestClean()
        {
            userServiceLayer1 = null;
            userServiceLayer2 = null;
            userServiceLayer3 = null;
            serviceLayer.CleanUpSystem();
            serviceLayer = null;
        }
    }
}

