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
    public class AddProductToStore_AT
    {
        private static I_ServiceLayer serviceLayer;
        private I_User_ServiceLayer userServiceLayer1;
        private I_User_ServiceLayer userServiceLayer2;
        private Discount no_discount = noDiscount.creteNoDiscount();
        private Policy regPolicy = regularPolicy.creteRegularPolicy();

        [TestInitialize]
        public void Init()
        {
            serviceLayer = new ServiceLayer();
            try
            {
                serviceLayer.Create_ServiceLayer(new Stub_deliverySystem(), new Stub_paymentSystem(), "admin", "1234Yuio");
            }
            catch (Exception) { }
            userServiceLayer1 = serviceLayer.Connect(new Stub_Alerter());
            userServiceLayer2 = serviceLayer.Connect(new Stub_Alerter());
            try
            {
                userServiceLayer1.Register("addProductToStoreUser", "1221JHgh");
                userServiceLayer2.Register("addProductToStoreUser2", "87654Gther");
            }
            catch (Exception) { }
            try
            {
                userServiceLayer2.Login("addProductToStoreUser2", "87654Gther");
                if (userServiceLayer1.Login("addProductToStoreUser", "1221JHgh"))
                {
                    userServiceLayer1.Open_Store("the store");
                }
            }
            catch (Exception) { }
        }


        [TestMethod()]
        public void Add_valid_product_to_store_happyTest()
        {
            List<ProductInStore> eggSearch1 = userServiceLayer2.GlobalSearch("eggs", null, null, -1, -1, -1, -1);
            Assert.IsTrue(userServiceLayer1.Add_Product_Store("the store", "eggs", "food", 13.4, 63, no_discount,regPolicy));   
            List<ProductInStore> eggSearch2 = userServiceLayer2.GlobalSearch("eggs", null, null, -1, -1, -1, -1);
            Assert.AreEqual(eggSearch1.Count + 1, eggSearch2.Count);
            Assert.AreEqual(eggSearch2[0].getStore().getName(), "the store");

        }

        [TestMethod()]
        public void Add_not_valid_product_to_store_happyTest()
        {
            Assert.ThrowsException<Sadna192Exception>(() => { userServiceLayer1.Add_Product_Store("", "orange", "food", 13.4, 63, no_discount,regPolicy); }, "store name can not be empty");

            Assert.ThrowsException<Sadna192Exception>(() => { userServiceLayer1.Add_Product_Store("the store", "", "food", 13.4, 63, no_discount,regPolicy); }, "product name can not be empty");
            
            Assert.ThrowsException<Sadna192Exception>(() => { userServiceLayer1.Add_Product_Store("the store", "orange", "", 13.4, 63, no_discount,regPolicy); }, "Category can not be empty");
            
            Assert.ThrowsException<Sadna192Exception>(() => { userServiceLayer1.Add_Product_Store("the store", "orange", "food", -6.4, 63, no_discount,regPolicy); }, "price can't be negative number");
            
            Assert.ThrowsException<Sadna192Exception>(() => { userServiceLayer1.Add_Product_Store("the store", "orange", "food", 5, -7, no_discount,regPolicy); }, "amount can't be negative number");
            
        }

        [TestMethod()]
        public void Add_existing_product_happyTest()  
        {
            userServiceLayer1.Add_Product_Store("the store", "orange", "food", 13.4, 63, no_discount,regPolicy);
            List<ProductInStore> search1 = userServiceLayer2.GlobalSearch("orange", null, null, -1, -1, -1, -1);
            Assert.AreEqual(1, search1.Count);

            userServiceLayer1.Add_Product_Store("the store", "orange", "food", 13.4, 70, no_discount,regPolicy);
            search1 = userServiceLayer2.GlobalSearch("orange", null, null, -1, -1, -1, -1);
            Assert.AreEqual(1, search1.Count);

        }

        [TestMethod()]
        public void Not_owner_add_product_sadTest()  
        {
            //registered user that is not store owner/menager tring to add product to the store 
            List<ProductInStore> search1 = userServiceLayer2.GlobalSearch("orange", "fruit", null, 0, 100, 0, 0);
            int pre_amount = search1.Count;
            Assert.ThrowsException<Sadna192Exception>(() => { userServiceLayer2.Add_Product_Store("the store", "orange", "food", 13.4, 63, no_discount,regPolicy); }, "only store owner or manager can add product to the store");
            search1 = userServiceLayer2.GlobalSearch("orange", "fruit", null, 0, 100, 0, 0);
            Assert.AreEqual(pre_amount, search1.Count);

            //visitor user tring to add product to store 
            I_User_ServiceLayer tmp_userServiceLayer = serviceLayer.Connect();
            Assert.ThrowsException<Sadna192Exception>(() => { tmp_userServiceLayer.Add_Product_Store("the store", "orange", "food", 13.4, 63, no_discount,regPolicy); }, "only store owner or manager can add product to the store");
            search1 = userServiceLayer2.GlobalSearch("orange", "fruit", null, 0, 100, 0, 0);
            Assert.AreEqual(pre_amount, search1.Count);
        }

        [TestCleanup]
        public void TestClean()
        {
            userServiceLayer1.Logout();
            userServiceLayer2.Logout();
            userServiceLayer1 = null;
            userServiceLayer2 = null;
            serviceLayer.CleanUpSystem();
            serviceLayer = null;
        }
    }
}