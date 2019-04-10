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
    public class AddProductToStore_AT
    {
        private static I_ServiceLayer serviceLayer;
        private I_User_ServiceLayer userServiceLayer1;
        private I_User_ServiceLayer userServiceLayer2;
        private I_DeliverySystem deliverySystem;
        private I_PaymentSystem paymentSystem;

        [TestInitialize]
        public void init()
        {
            serviceLayer = I_ServiceLayer.Create_ServiceLayer(deliverySystem, paymentSystem, "admin", "1234");
            userServiceLayer1 = serviceLayer.connect();
            if (userServiceLayer1.Register("addProductToStore_user", "1221"))
                if (userServiceLayer1.Login("addProductToStore_user", "1221"))
                {
                    bool store = userServiceLayer1.Open_Store("the store");
                }
            userServiceLayer2 = serviceLayer.connect();
            if (userServiceLayer2.Register("addProductToStore_user2", "87654"))
                bool login1 = userServiceLayer2.Login("addProductToStore_user2", "87654");

        }

        [TestMethod()]
        public void Add_valid_product_to_store_tests()
        {
            List<ProductInStore> eggSearch1 = userServiceLayer2.GlobalSearch("eggs", "food", null, 0, 100, 0, 0);
            Assert.IsTrue(userServiceLayer1.Add_Product_Store("the store", "eggs", "food", 13.4, 63, null, null));   //happy 1
            List<ProductInStore> eggSearch2 = userServiceLayer2.GlobalSearch("eggs", "food", null, 0, 100, 0, 0);
            Assert.AreEqual(eggSearch1.Count + 1, eggSearch2.Count);
            Assert.AreEqual(eggSearch2[0].getStoreName(), "the store");

        }

        [TestMethod()]
        public void Add_not_valid_product_to_store_tests()
        {
            List<ProductInStore> search1 = userServiceLayer2.GlobalSearch("orange", "fruit", null, 0, 100, 0, 0);
            int pre_amount = search1.Count;
            Assert.ThrowsException<Exception>(() => { userServiceLayer1.Add_Product_Store("the store", 87, "food", 13.4, 63, null, null); }, "product name must be string");
            search1 = userServiceLayer2.GlobalSearch("orange", "fruit", null, 0, 100, 0, 0);
            Assert.AreEqual(pre_amount, search1.Count);

            Assert.ThrowsException<Exception>(() => { userServiceLayer1.Add_Product_Store("the store", "orange", 23, 13.4, 63, null, null); }, "Category must be string");
            search1 = userServiceLayer2.GlobalSearch("orange", "fruit", null, 0, 100, 0, 0);
            Assert.AreEqual(pre_amount, search1.Count);

            Assert.ThrowsException<Exception>(() => { userServiceLayer1.Add_Product_Store("the store", "orange", "food", -6.4, 63, null, null); }, "price can't be negative number");
            search1 = userServiceLayer2.GlobalSearch("orange", "fruit", null, 0, 100, 0, 0);
            Assert.AreEqual(pre_amount, search1.Count);

            Assert.ThrowsException<Exception>(() => { userServiceLayer1.Add_Product_Store("the store", "orange", "food", 5, -7, null, null); }, "amount can't be negative number");
            search1 = userServiceLayer2.GlobalSearch("orange", "fruit", null, 0, 100, 0, 0);
            Assert.AreEqual(pre_amount, search1.Count);
        }

        [TestMethod()]
        public void Add_existing_product_tests()  //happy -3
        {
            List<ProductInStore> search1 = userServiceLayer2.GlobalSearch("orange", "fruit", null, 0, 100, 0, 0);
            int pre_amount = search1.Count;
            userServiceLayer1.Add_Product_Store("the store", "orange", "food", 13.4, 63, null, null);
            search1 = userServiceLayer2.GlobalSearch("orange", "fruit", null, 0, 100, 0, 0);
            int pre_orange_amount;
            for (int i = 0; i < search1.Count; i++)
            {
                if (search1[i].getStoreName().Equals("the store"))
                    pre_orange_amount = search1[i].getAmount();
            }
            Assert.AreEqual(pre_amount + 1, search1.Count);

            userServiceLayer1.Add_Product_Store("the store", "orange", "food", 13.4, 70, null, null);
            search1 = userServiceLayer2.GlobalSearch("orange", "fruit", null, 0, 100, 0, 0);
            Assert.AreEqual(pre_amount + 1, search1.Count);
            for (int i = 0; i < search1.Count; i++)
            {
                if (search1[i].getStoreName().Equals("the store"))
                {
                    Assert.AreNotEual(pre_orange_amount, search1[i].getAmount);
                    Assert.IsTrue(search1[i].getAmount == 70);
                    Assert.IsTrue(pre_orange_amount == 63);
                }
            }
        }

        [TestMethod()]
        public void not_owner_add_product_test()  //bad
        {
            List<ProductInStore> search1 = userServiceLayer2.GlobalSearch("orange", "fruit", null, 0, 100, 0, 0);
            int pre_amount = search1.Count;
            Assert.ThrowsException<Exception>(() => { userServiceLayer2.Add_Product_Store("the store", "orange", "food", 13.4, 63, null, null); }, "only store owner or manager can add product to the store");
            search1 = userServiceLayer2.GlobalSearch("orange", "fruit", null, 0, 100, 0, 0);
            Assert.AreEqual(pre_amount, search1.Count);

            I_User_ServiceLayer tmp_userServiceLayer = serviceLayer.connect();
            Assert.ThrowsException<Exception>(() => { tmp_userServiceLayer.Add_Product_Store("the store", "orange", "food", 13.4, 63, null, null); }, "only store owner or manager can add product to the store");
            search1 = userServiceLayer2.GlobalSearch("orange", "fruit", null, 0, 100, 0, 0);
            Assert.AreEqual(pre_amount, search1.Count);
        }

    }
}