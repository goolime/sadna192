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
    public class AddProductToStore_AT
    {
        private static I_ServiceLayer serviceLayer;
        private I_User_ServiceLayer userServiceLayer1;
        private I_User_ServiceLayer userServiceLayer2;

        [TestInitialize]
        public void init()
        {
            serviceLayer = new ServiceLayer();
            serviceLayer.Create_ServiceLayer(new stub_deliverySystem(), new stub_paymentSystem(), "admin", "1234Yuio");
            userServiceLayer1 = serviceLayer.Connect();
            if (userServiceLayer1.Register("addProductToStoreUser", "1221JHgh"))
                if (userServiceLayer1.Login("addProductToStoreUser", "1221JHgh"))
                {
                    userServiceLayer1.Open_Store("the store");
                }
            userServiceLayer2 = serviceLayer.Connect();
            if (userServiceLayer2.Register("addProductToStoreUser2", "87654Gther"))
                userServiceLayer2.Login("addProductToStoreUser2", "87654Gther");

        }

        [TestMethod()]
        public void Add_valid_product_to_store_tests()
        {
            List<ProductInStore> eggSearch1 = userServiceLayer2.GlobalSearch("eggs", "food", null, 0, 100, 0, 0);
            Assert.IsTrue(userServiceLayer1.Add_Product_Store("the store", "eggs", "food", 13.4, 63, new noDiscount(), new regularPolicy()));   //happy 1
            List<ProductInStore> eggSearch2 = userServiceLayer2.GlobalSearch("eggs", "food", null, 0, 100, 0, 0);
            Assert.AreEqual(eggSearch1.Count + 1, eggSearch2.Count);
            Assert.AreEqual(eggSearch2[0].getStore().getName(), "the store");

        }

        [TestMethod()]
        public void Add_not_valid_product_to_store_tests()
        {
            List<ProductInStore> search1 = userServiceLayer2.GlobalSearch("orange", "fruit", null, 0, 100, 0, 0);
            int pre_amount = search1.Count;

            Assert.ThrowsException<Exception>(() => { userServiceLayer1.Add_Product_Store("", "orange", "food", 13.4, 63, new noDiscount(), new regularPolicy()); }, "store name can not be empty");
            search1 = userServiceLayer2.GlobalSearch("orange", "fruit", null, 0, 100, 0, 0);
            Assert.AreEqual(pre_amount, search1.Count);

            Assert.ThrowsException<Exception>(() => { userServiceLayer1.Add_Product_Store("the store", "", "food", 13.4, 63, new noDiscount(), new regularPolicy()); }, "product name can not be empty");
            search1 = userServiceLayer2.GlobalSearch("orange", "fruit", null, 0, 100, 0, 0);
            Assert.AreEqual(pre_amount, search1.Count);

            Assert.ThrowsException<Exception>(() => { userServiceLayer1.Add_Product_Store("the store", "orange", "", 13.4, 63, new noDiscount(), new regularPolicy()); }, "Category can not be empty");
            search1 = userServiceLayer2.GlobalSearch("orange", "fruit", null, 0, 100, 0, 0);
            Assert.AreEqual(pre_amount, search1.Count);

            Assert.ThrowsException<Exception>(() => { userServiceLayer1.Add_Product_Store("the store", "orange", "food", -6.4, 63, new noDiscount(), new regularPolicy()); }, "price can't be negative number");
            search1 = userServiceLayer2.GlobalSearch("orange", "fruit", null, 0, 100, 0, 0);
            Assert.AreEqual(pre_amount, search1.Count);

            Assert.ThrowsException<Exception>(() => { userServiceLayer1.Add_Product_Store("the store", "orange", "food", 5, -7, new noDiscount(), new regularPolicy()); }, "amount can't be negative number");
            search1 = userServiceLayer2.GlobalSearch("orange", "fruit", null, 0, 100, 0, 0);
            Assert.AreEqual(pre_amount, search1.Count);
        }

        [TestMethod()]
        public void Add_existing_product_tests()  //happy -3
        {
            List<ProductInStore> search1 = userServiceLayer2.GlobalSearch("orange", "fruit", null, 0, 100, 0, 0);
            int pre_amount = search1.Count;
            userServiceLayer1.Add_Product_Store("the store", "orange", "food", 13.4, 63, new noDiscount(), new regularPolicy());
            search1 = userServiceLayer2.GlobalSearch("orange", "fruit", null, 0, 100, 0, 0);
            int pre_orange_amount=0;
            for (int i = 0; i < search1.Count; i++)
            {
                if (search1[i].getStore().getName().Equals("the store"))
                    pre_orange_amount = search1[i].getAmount();
            }
            Assert.AreEqual(pre_amount + 1, search1.Count);

            userServiceLayer1.Add_Product_Store("the store", "orange", "food", 13.4, 70, new noDiscount(), new regularPolicy());
            search1 = userServiceLayer2.GlobalSearch("orange", "fruit", null, 0, 100, 0, 0);
            Assert.AreEqual(pre_amount + 1, search1.Count);
            for (int i = 0; i < search1.Count; i++)
            {
                if (search1[i].getStore().getName().Equals("the store"))
                {
                    Assert.AreNotEqual(pre_orange_amount, search1[i].getAmount());
                    Assert.IsTrue(search1[i].getAmount() == 70);
                    Assert.IsTrue(pre_orange_amount == 63);
                }
            }
        }

        [TestMethod()]
        public void not_owner_add_product_test()  //bad
        {
            List<ProductInStore> search1 = userServiceLayer2.GlobalSearch("orange", "fruit", null, 0, 100, 0, 0);
            int pre_amount = search1.Count;
            Assert.ThrowsException<Exception>(() => { userServiceLayer2.Add_Product_Store("the store", "orange", "food", 13.4, 63, new noDiscount(), new regularPolicy()); }, "only store owner or manager can add product to the store");
            search1 = userServiceLayer2.GlobalSearch("orange", "fruit", null, 0, 100, 0, 0);
            Assert.AreEqual(pre_amount, search1.Count);

            I_User_ServiceLayer tmp_userServiceLayer = serviceLayer.Connect();
            Assert.ThrowsException<Exception>(() => { tmp_userServiceLayer.Add_Product_Store("the store", "orange", "food", 13.4, 63, new noDiscount(), new regularPolicy()); }, "only store owner or manager can add product to the store");
            search1 = userServiceLayer2.GlobalSearch("orange", "fruit", null, 0, 100, 0, 0);
            Assert.AreEqual(pre_amount, search1.Count);
        }

    }
}