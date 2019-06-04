using System;
using sadna192;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using static sadna192_Tests.Stubs;
using System.Collections.Generic;

namespace sadna192_Tests.Integration_Tests
{
    [TestClass]
    public class UserTest
    {
        private static I_ServiceLayer serviceLayer;

        [TestInitialize]
        public void Init()
        {
            serviceLayer = new ServiceLayer();
            try
            {
                serviceLayer.Create_ServiceLayer(new Stub_deliverySystem(), new Stub_paymentSystem(), "admin", "1234Abcd");
            }
            catch (Exception) { }
        }

        [TestMethod]
        public void Login_Register_Logout_Test()

        {
            I_User_ServiceLayer person1 = serviceLayer.Connect();
            Assert.IsTrue(person1.GetUserState().isVistor());
            Assert.ThrowsException<Exception>(() => { person1.Login("usertest34", "69375Abcd"); }, "this user is not exist in the system");
            Assert.IsTrue(person1.Register("usertest34", "69375Abcd"));
            Assert.IsTrue(person1.Login("usertest34", "69375Abcd"));
            Assert.IsTrue(person1.GetUserState().isMember());
            Assert.IsFalse(person1.GetUserState().isVistor());
            Assert.IsTrue(person1.Logout());
            Assert.IsTrue(person1.GetUserState().isVistor());
            Assert.IsFalse(person1.GetUserState().isMember());
        }

        [TestMethod]
        public void ManageStore_addProduct()
        {
            I_User_ServiceLayer storeOwner = serviceLayer.Connect();
            storeOwner.Register("person2", "sdFg2345"); 
            storeOwner.Login("person2", "sdFg2345");
            Assert.IsTrue(storeOwner.Open_Store("bigbug"));
            Assert.IsTrue(storeOwner.GetUserState().isOwner("bigbug"));
            List<ProductInStore> appleSearch = storeOwner.GlobalSearch("apple", null, null, -1, -1, -1, -1);
            int preAmount = appleSearch.Count; 
            Assert.IsTrue(storeOwner.Add_Product_Store("bigbug", "apple", "fruit", 4, 100, new noDiscount(), new RegularPolicy()));
            appleSearch = storeOwner.GlobalSearch("apple", null, null, -1, -1, -1, -1);
            Assert.AreEqual(preAmount + 1, appleSearch.Count);
            bool ans = false;
            for (int i =0; i< appleSearch.Count; i++)
            {
                if (appleSearch[i].getStore().getName() == "bigbug")
                    ans = true;
            }
            Assert.IsTrue(ans);
        }

        [TestMethod]
        public void ManageStore_addManagerAndOwner()
        {
            Member storeOwner = new Member("storeOwner", "123GFDsc");
            Member storeOwnerToBe = new Member("storeOwnerToBe", "sdfgT543");
            Member storeManagerToBe = new Member("storeManagerToBe", "poiU9876");
            Store store1 = new Store("ginot");
            storeOwner.Open_Store(store1);
            Assert.ThrowsException<Exception>(() =>
            {storeOwnerToBe.Add_Product_Store("ginot", "cherry", "fruit", 15, 60, new noDiscount(), new RegularPolicy()); }, "only store owner ang permission manager can add product to store");
            Assert.ThrowsException<Exception>(() =>
            {storeOwnerToBe.Add_Store_Manager("ginot", storeManagerToBe , true, false , false) ; }, "only store owner ang permission manager can add product to store");         
            Assert.ThrowsException<Exception>(() => 
            {storeManagerToBe.Add_Product_Store("ginot", "banana", "fruit", 5, 100, new noDiscount(), new RegularPolicy()) ; }, "only store owner ang permission manager can add product to store");
            Assert.IsTrue(storeOwner.Add_Store_Owner("ginot", storeOwnerToBe));
            Assert.IsTrue(storeOwnerToBe.Add_Product_Store("ginot", "cherry", "fruit", 15, 60, new noDiscount(), new RegularPolicy()));
            Assert.IsTrue(storeOwnerToBe.Add_Store_Manager("ginot", storeManagerToBe, true, false, false));
            Assert.IsTrue(storeManagerToBe.Add_Product_Store("ginot", "banana", "fruit", 5, 100, new noDiscount(), new RegularPolicy())); 
        }
    }
}
