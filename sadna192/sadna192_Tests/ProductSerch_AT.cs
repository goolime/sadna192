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
    public class ProductSearch_AT
    {
        private static I_ServiceLayer serviceLayer;
        private I_User_ServiceLayer userServiceLayer1;
        private I_User_ServiceLayer userServiceLayer2;
        private I_User_ServiceLayer userServiceLayer3;

        [TestInitialize]
        public void init()
        {
            serviceLayer = new ServiceLayer();
            serviceLayer.Create_ServiceLayer(new stub_deliverySystem(), new stub_paymentSystem(), "admin", "1234AsDF");
            userServiceLayer1 = serviceLayer.Connect();
            if (userServiceLayer1.Register("productSearchuser", "1221Abcd"))
                if (userServiceLayer1.Login("productSearchuser", "1221Abcd"))
                    if (userServiceLayer1.Open_Store("Grocery"))
                        userServiceLayer1.Add_Product_Store("Grocery", "milk", "food", 4.5, 10, null, null);

            userServiceLayer2 = serviceLayer.Connect();
            if (userServiceLayer2.Register("productSearchuser2", "9876Abcd"))
                if (userServiceLayer2.Login("productSearchuser2", "9876Abcd"))
                    if (userServiceLayer2.Open_Store("handm") && userServiceLayer2.Open_Store("Market"))
                    {
                         userServiceLayer2.Add_Product_Store("handm", "dress", "clothes", 29.90, 5, null, null);
                         userServiceLayer2.Add_Product_Store("Market", "milk", "foodd", 3, 6, null, null);
                    }

            userServiceLayer3 = serviceLayer.Connect();
            if (userServiceLayer3.Register("productSearchuser3", "4455Abcd"))
                userServiceLayer3.Login("productSearchuser3", "4455Abcd");
        }

        [TestMethod()]
        public void Product_Search_by_Category()
        {
            List<ProductInStore> milkSearch = userServiceLayer3.GlobalSearch(null, "food", null, -1, -1, -1, -1);
            Assert.AreEqual(milkSearch.Count, 2);
        }

        [TestMethod()]
        public void Product_Search_by_price()
        {
            List<ProductInStore> milkSearch = userServiceLayer3.GlobalSearch(null, null, null, 5, -1, -1, -1);
            Assert.AreEqual(milkSearch.Count, 1);
            List<ProductInStore> dressSearch = userServiceLayer3.GlobalSearch(null, null, null, -1, 5, -1, -1);
            Assert.AreEqual(dressSearch.Count, 2);
            //Assert.ThrowsException<Exception>(() => { userServiceLayer3.GlobalSearch("apple", "fruit", null, 0, 10, 0, 0); }, "searching a product that not exist in the system");
        }

        [TestMethod()]
        public void Product_search_by_name()
        {
            List<ProductInStore> milkSearch = userServiceLayer3.GlobalSearch("milk", null, null, -1, -1, -1, -1);
            Assert.AreEqual(milkSearch.Count, 2);
           
        }


        [TestMethod()]
        public void Product_search_bad_test()
        {       
            //Assert.ThrowsException<Exception>(() => { userServiceLayer3.GlobalSearch(null, null, null, 0, 0, 0, 0); }, "searching with no parameters at all"); //?? 
            Assert.ThrowsException<Exception>(() => { userServiceLayer3.GlobalSearch("", "food", null, 0, -2, 0, 0); }, "searching with wrong type of product name");
            Assert.ThrowsException<Exception>(() => { userServiceLayer3.GlobalSearch("milk‚Î", null, null, 0, 20, 0, 0); }, "searching with wrong type of catagory");
            Assert.ThrowsException<Exception>(() => { userServiceLayer3.GlobalSearch("milk", null, null, -2, 0, 0, 0); }, "searching with wrong type of keywords list");
            Assert.ThrowsException<Exception>(() => { userServiceLayer3.GlobalSearch("milk", "food", null, 40, 20, 0, 0); }, "searching with min price > max price");
            Assert.ThrowsException<Exception>(() => { userServiceLayer3.GlobalSearch("milk", "food", null, -2, 20, 0, 0); }, "searching with wrong type of min price");
            Assert.ThrowsException<Exception>(() => { userServiceLayer3.GlobalSearch("milk", "food", null, 0, -5, 0, 0); }, "searching with wrong type of max price");
            Assert.ThrowsException<Exception>(() => { userServiceLayer3.GlobalSearch("milk", "food", null, 0, 20, -0.5, 0); }, "searching with wrong type of store rank");
            Assert.ThrowsException<Exception>(() => { userServiceLayer3.GlobalSearch("milk", "food", null, 0, 20, 0, -1.3); }, "searching with wrong type of product rank");
            
            //Assert.Fail("how to search product with wrong parameters?"); 
        }

    }
}

