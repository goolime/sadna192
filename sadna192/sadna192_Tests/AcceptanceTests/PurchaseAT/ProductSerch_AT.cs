using Microsoft.VisualStudio.TestTools.UnitTesting;
using sadna192;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AcceptanceTests
{
    [TestClass()]
    public class ProductSearch_AT
    {
        private static I_ServiceLayer serviceLayer;
        private I_User_ServiceLayer userServiceLayer1;
        private I_User_ServiceLayer userServiceLayer2;
        private I_User_ServiceLayer userServiceLayer3;
        private I_DeliverySystem deliverySystem;
        private I_PaymentSystem paymentSystem;

        [TestInitialize]
        public void init()
        {
            serviceLayer = I_ServiceLayer.Create_ServiceLayer(deliverySystem, paymentSystem, "admin", "1234");
            userServiceLayer1 = serviceLayer.connect();
            if (userServiceLayer1.Register("productSearch_user", "1221"))
                if (userServiceLayer1.Login("productSearch_user", "1221"))
                    if (userServiceLayer1.Open_Store("Grocery"))
                        bool milk_product1 = userServiceLayer1.Add_Product_Store("Grocery", "milk", "food", 4.5, 10, null, null);

            userServiceLayer2 = serviceLayer.connect();
            if (userServiceLayer2.Register("productSearch_user2", "9876"))
                if (userServiceLayer2.Login("productSearch_user2", "9876"))
                    if (userServiceLayer2.Open_Store("h&m") && userServiceLayer2.Open_Store("Market"))
                    {
                        bool dress_product = userServiceLayer.Add_Product_Store("h&m", "dress", "clothes", 29.90, 5, null, null);
                        bool milk_product2 = userServiceLayer.Add_Product_Store("Market", "milk", "food", 3, 6, null, null);
                    }

            userServiceLayer3 = serviceLayer.connect();
            if (userServiceLayer3.Register("productSearch_user3", "4455"))
                bool user = userServiceLayer3.Login("productSearch_user3", "4455");
        }

        [TestMethod()]
        public void Product_search_happy_test()
        {
            List<ProductInStore> milkSearch = userServiceLayer3.GlobalSearch("milk", "food", null, 0, 20, 0, 0);
            Assert.AreEqual(milkSearch.Count, 2);
            milkSearch = userServiceLayer3.GlobalSearch("milk", "food", null, 4, 20, 0, 0);
            Assert.AreEqual(milkSearch.Count, 1);
            milkSearch = userServiceLayer3.GlobalSearch("milk", "food", null, 0, 0, 0, 0);
            Assert.AreEqual(milkSearch.Count, 0);
            List<ProductInStore> dressSearch = userServiceLayer3.GlobalSearch(null, "clothes", null, 0, 70, 0, 0);
            Assert.AreEqual(dressSearch.Count, 1);
            Assert.ThrowsException<Exception>(() => { userServiceLayer3.GlobalSearch("apple", "fruit", null, 0, 10, 0, 0); }, "searching a product that not exist in the system");
        }


        [TestMethod()]
        public void Product_search_bad_test()
        {       
            Assert.ThrowsException<Exception>(() => { userServiceLayer3.GlobalSearch(null, null, null, null, null, null, null); }, "searching with no parameters at all");
            Assert.ThrowsException<Exception>(() => { userServiceLayer3.GlobalSearch(785, "food", null, 0, 20, 0, 0); }, "searching with wrong type of product name");
            Assert.ThrowsException<Exception>(() => { userServiceLayer3.GlobalSearch("milk", 985, null, 0, 20, 0, 0); }, "searching with wrong type of catagory");
            Assert.ThrowsException<Exception>(() => { userServiceLayer3.GlobalSearch("milk", "food", 456, 0, 20, 0, 0); }, "searching with wrong type of keywords list");
            Assert.ThrowsException<Exception>(() => { userServiceLayer3.GlobalSearch("milk", "food", null, 40, 20, 0, 0); }, "searching with min price > max price");
            Assert.ThrowsException<Exception>(() => { userServiceLayer3.GlobalSearch("milk", "food", null, "0", 20, 0, 0); }, "searching with wrong type of min price");
            Assert.ThrowsException<Exception>(() => { userServiceLayer3.GlobalSearch("milk", "food", null, 0, "2op", 0, 0); }, "searching with wrong type of max price");
            Assert.ThrowsException<Exception>(() => { userServiceLayer3.GlobalSearch("milk", "food", null, 0, 20, "2", 0); }, "searching with wrong type of store rank");
            Assert.ThrowsException<Exception>(() => { userServiceLayer3.GlobalSearch("milk", "food", null, 0, 20, 0, "as"); }, "searching with wrong type of product rank");
        }

    }
}

