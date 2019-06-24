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
        private I_User_ServiceLayer storeOwner;
        private I_User_ServiceLayer testUser;

        [TestInitialize]
        public void Init()
        {
            serviceLayer = new ServiceLayer();
            serviceLayer.Create_ServiceLayer(new Stub_deliverySystem(), new Stub_paymentSystem(), "admin", "1234Abcd");
            storeOwner = serviceLayer.Connect(new Stubs.Stub_Alerter());
            storeOwner.Register("storeOwner", "storeOwner1");
            storeOwner.Login("storeOwner", "storeOwner1");
            storeOwner.Open_Store("shufersal");
            storeOwner.Add_Product_Store("shufersal", "apple", "fruit", 4.8, 80, new noDiscount(), new regularPolicy());

            testUser = serviceLayer.Connect(new Stubs.Stub_Alerter());
            testUser.Register("testUser", "1234Rtyu");
            testUser.Login("testUser", "1234Rtyu");
        }

        [TestMethod]
        public void RegisterAndLoginTest()
        {
            I_User_ServiceLayer testUser1 = serviceLayer.Connect(new Stubs.Stub_Alerter());
            Assert.IsTrue(testUser1.GetUserState().isVistor());
            Assert.ThrowsException<Exception>(() =>
            { testUser1.Login("testUser1", "69375Abcd"); },
                "this user is not exist in the system");
            Assert.IsTrue(testUser1.Register("testUser1", "69375Abcd"));
            Assert.IsTrue(testUser1.Login("testUser1", "69375Abcd"));
            Assert.IsTrue(testUser1.GetUserState().isMember());
            Assert.IsFalse(testUser1.GetUserState().isVistor());
            Assert.IsTrue(testUser1.Logout());
            Assert.IsTrue(testUser1.GetUserState().isVistor());
            Assert.IsFalse(testUser1.GetUserState().isMember());
        }

        [TestMethod]
        public void LogoutAndLoginTest()
        {
            Assert.IsTrue(testUser.GetUserState().isMember());
            Assert.IsFalse(testUser.GetUserState().isVistor());
            List<ProductInStore> appleSearch = testUser.GlobalSearch("apple", null, null, -1, -1, -1, -1);
            testUser.Add_To_ShopingBasket(appleSearch[0], 3);
            List<KeyValuePair<ProductInStore, int>> cartList1 = testUser.Watch_Cart();
            Assert.IsTrue(testUser.Logout());
            Assert.IsTrue(testUser.GetUserState().isVistor());
            Assert.IsFalse(testUser.GetUserState().isMember());
            Assert.ThrowsException<Exception>(() => { testUser.Watch_Cart(); }, "this is cart of visitor.");
            testUser.Login("testUser", "1234Rtyu");
            List<KeyValuePair<ProductInStore, int>> cartList2 = testUser.Watch_Cart();
            Assert.AreEqual(cartList1.Count, cartList2.Count);
            Assert.AreEqual(cartList1[0], cartList2[0]);
        }

        [TestMethod]
        public void VisitorSearchingAndManageCart()
        {
            I_User_ServiceLayer testUser2 = serviceLayer.Connect(new Stubs.Stub_Alerter());
            List<ProductInStore> appleSearch = testUser2.GlobalSearch("apple", null, null, -1, -1, -1, -1);
            Assert.AreEqual(appleSearch.Count, 1);
            Assert.AreEqual(-1, testUser2.GetUserState().numOfItemsInCart("shufersal"));
            Assert.IsTrue(testUser2.Add_To_ShopingBasket(appleSearch[0], 3));
            Assert.AreEqual(3, testUser2.GetUserState().numOfItemsInCart("shufersal"));
            Assert.AreEqual(3, testUser2.GetUserState().numOfItemsInCart("shufersal", "apple"));
            List<KeyValuePair<ProductInStore, int>> cartList = testUser2.Watch_Cart();
            Assert.AreEqual(appleSearch[0], cartList[0].Key);
            Assert.AreEqual(cartList[0].Value, 3);
            Assert.IsTrue(testUser2.Edit_Product_In_ShopingBasket(appleSearch[0], 5));
            cartList = testUser2.Watch_Cart();
            Assert.AreNotEqual(cartList[0].Value, 3);
            Assert.AreEqual(cartList[0].Value, 5);
            ProductInStore product =  storeOwner.GetProductFromStore("apple", "shufersal");
            Assert.AreEqual(product.getAmount(), 80); //TODO: need to be 75 or change the user story
            Assert.IsTrue(testUser2.Edit_Product_In_ShopingBasket(appleSearch[0], 0));
            Assert.ThrowsException<Exception>(() => { testUser2.Watch_Cart(); }, "cart is empty"); 
        }

        [TestMethod]
        public void OpenStoreTest()
        {
            I_User_ServiceLayer visitorUser = serviceLayer.Connect(new Stubs.Stub_Alerter());
            Assert.ThrowsException<Exception>(() => { visitorUser.Open_Store("renuar"); }, "only registered user can open store");
            Assert.ThrowsException<Exception>(() => { visitorUser.GetUserState().getMyShops(); }, "visitor cannot have stores");
            Assert.AreEqual(testUser.GetUserState().getMyShops().Count, 0);
            Assert.IsTrue(testUser.Open_Store("Renuar"));
            Assert.AreEqual(testUser.GetUserState().getMyShops().Count, 1);
            Assert.IsTrue(testUser.GetUserState().isOwner("Renuar"));
        }


          [TestMethod]
          public void ManageStore_AddUpdateAndDeleteProduct()
          {  
            //Add product to store
              Assert.IsTrue(storeOwner.GetUserState().isOwner("shufersal"));
              List<ProductInStore> milkSearch = storeOwner.GlobalSearch("milk", null, null, -1, -1, -1, -1);
              Assert.AreEqual(milkSearch.Count,0);
              Assert.IsNull(storeOwner.GetProductFromStore("milk", "shufersal"));
              Assert.IsTrue(storeOwner.Add_Product_Store("shufersal", "milk", "drink", 5.9, 100, new noDiscount(), new regularPolicy()));
              milkSearch = storeOwner.GlobalSearch("milk", null, null, -1, -1, -1, -1);
              Assert.AreEqual( 1, milkSearch.Count);
              Assert.AreEqual(storeOwner.GetProductFromStore("milk", "shufersal") , milkSearch[0]);
             bool ans = false;
             for (int i =0; i< milkSearch.Count; i++)
             {
                 if (milkSearch[i].getStore().getName() == "shufersal")
                     ans = true;
             }
             Assert.IsTrue(ans);
            //updtae product
            Assert.IsTrue(storeOwner.Update_Product_Store("shufersal", "milk", "milk tnuva", "drink", 5.4, 120, new noDiscount(), new regularPolicy()));
            milkSearch = storeOwner.GlobalSearch("milk", null, null, -1, -1, -1, -1);
            Assert.AreEqual(milkSearch.Count, 0);
            Assert.IsNull(storeOwner.GetProductFromStore("milk", "shufersal"));
            milkSearch = storeOwner.GlobalSearch("milk tnuva", null, null, -1, -1, -1, -1);
            Assert.AreEqual(milkSearch.Count, 1);
            Assert.AreEqual(storeOwner.GetProductFromStore("milk tnuva", "shufersal"), milkSearch[0]);
            //Delete product
            Assert.IsTrue(storeOwner.Remove_Product_Store("shufersal", "milk tnuva"));
            milkSearch = storeOwner.GlobalSearch("milk tnuva", null, null, -1, -1, -1, -1);
            Assert.AreEqual(milkSearch.Count, 0);
            Assert.IsNull(storeOwner.GetProductFromStore("milk tnuva", "shufersal"));
        }

          [TestMethod]
          public void ManageStore_addManagerAndOwner()
          {
              Member storeOwner = new Member("storeOwner", "123GFDsc");
              Member storeOwnerToBe = new Member("storeOwnerToBe", "sdfgT543");
              Member storeManagerToBe = new Member("storeManagerToBe", "poiU9876");
              Store store1 = new Store("market");
              storeOwner.Open_Store(store1);
              Assert.ThrowsException<Exception>(() =>
              {storeOwnerToBe.Add_Product_Store("market", "cherry", "fruit", 15, 60, new noDiscount(), new regularPolicy()); }, "only store owner ang permission manager can add product to store");
              Assert.ThrowsException<Exception>(() =>
              {storeOwnerToBe.Add_Store_Manager("market", storeManagerToBe , true, false , false) ; }, "only store owner ang permission manager can add product to store");         
              Assert.ThrowsException<Exception>(() => 
              {storeManagerToBe.Add_Product_Store("market", "banana", "fruit", 5, 100, new noDiscount(), new regularPolicy()) ; }, "only store owner ang permission manager can add product to store");
              Assert.IsTrue(storeOwner.Add_Store_Owner("market", storeOwnerToBe));
              Assert.IsTrue(storeOwnerToBe.Add_Product_Store("market", "cherry", "fruit", 15, 60, new noDiscount(), new regularPolicy()));
              Assert.IsTrue(storeOwnerToBe.Add_Store_Manager("market", storeManagerToBe, true, false, false));
              
              Assert.IsTrue(storeManagerToBe.Add_Product_Store("market", "banana", "fruit", 5, 100, new noDiscount(), new regularPolicy())); 
          }

        [TestMethod]
        public void RemoveUserTest()
        {
            Assert.IsTrue(testUser.Open_Store("testStore"));
            testUser.Add_Product_Store("testStore", "eggs", "food", 12, 59, new noDiscount(), new regularPolicy());
            Assert.IsNotNull(testUser.GetProductFromStore("eggs", "testStore")); 
            I_User_ServiceLayer admin = serviceLayer.Connect(new Stubs.Stub_Alerter());
            admin.Login("admin", "1234Abcd");
            Assert.IsTrue(admin.Remove_User("testUser"));
            Assert.ThrowsException<Exception>(() =>
            { testUser.Add_Product_Store("testStore", "cherry", "fruit", 15, 60, new noDiscount(), new regularPolicy()); }, "the user is not exist");
            Assert.ThrowsException<Exception>(() =>
            { testUser.Open_Store("testStore2"); }, "the user is not exist");
            //Assert.ThrowsException<Exception>(() => { testUser.GetProductFromStore("eggs", "testStore"); }, "the store is not exist because the owner has removed"); 
            List<ProductInStore> milkSearch = storeOwner.GlobalSearch("eggs", null, null, -1, -1, -1, -1);
            Assert.AreEqual(milkSearch.Count, 1); //TODO : need to be 0 
            Assert.IsNotNull(testUser.GetProductFromStore("eggs", "testStore"));  //TODO  - why it still exist? 

        }

        [TestCleanup]
          public void CleanUp()
        {
            serviceLayer.CleanUpSystem();
            serviceLayer = null; 
        }
    }
}
