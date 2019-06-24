using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using static sadna192_Tests.Stubs;

namespace sadna192.Tests.AcceptanceTests
{
    [TestClass()]
    public class UpdateCart_AT
    {
        private static I_ServiceLayer serviceLayer;
        private I_User_ServiceLayer userServiceLayer_buyer;
        private I_User_ServiceLayer userServiceLayer_seller;

        [TestInitialize]
        public void Init()
        {
            serviceLayer = new ServiceLayer();
            try
            {
                serviceLayer.Create_ServiceLayer(new Stub_deliverySystem(), new Stub_paymentSystem(), "admin", "1234DFgh");
            }
            catch (Exception) { }
            userServiceLayer_seller = serviceLayer.Connect(new Stub_Alerter());
            userServiceLayer_buyer = serviceLayer.Connect(new Stub_Alerter());
            try
            {
                userServiceLayer_seller.Register("updateCartUser", "1221THyu");
                userServiceLayer_buyer.Register("updateCartUser2", "97531ERty");
            }
            catch (Exception) { }
            try
            {
                userServiceLayer_seller.Login("updateCartUser", "1221THyu");
                userServiceLayer_buyer.Login("updateCartUser2", "97531ERty");
                userServiceLayer_seller.Open_Store("MiniMarket");
                userServiceLayer_seller.Open_Store("mtokim");
                userServiceLayer_seller.Add_Product_Store("MiniMarket", "red apple", "food", 4, 10, new noDiscount(), new regularPolicy());
                userServiceLayer_seller.Add_Product_Store("mtokim", "muffin", "food", 3.5, 100, new noDiscount(), new regularPolicy());
                userServiceLayer_seller.Add_Product_Store("MiniMarket", "cheesecake", "food", 15, 5, new noDiscount(), new regularPolicy());
                List<ProductInStore> appleToBuy = userServiceLayer_buyer.GlobalSearch("red apple", null, null, -1, -1, -1, -1);
                userServiceLayer_buyer.Add_To_ShopingBasket(appleToBuy[0], 2);
            }
            catch (Exception) { }

           

        }

        [TestMethod()]
        public void AddToCart_happyTest()
        {
            Assert.AreEqual(userServiceLayer_buyer.Watch_Cart().Count, 1);
            Assert.AreEqual(userServiceLayer_buyer.Watch_Cart()[0].Value, 2);
           //// Assert.ThrowsException<Exception>(() => { userServiceLayer_buyer.Edit_Product_In_ShopingBasket(userServiceLayer_buyer.Watch_Cart()[0].Key, 20); }, "this amount is not available in stock. UC 2.7.2 happy-3");
           //// Assert.AreEqual(userServiceLayer_buyer.Watch_Cart()[0].Value, 2);

            Assert.IsTrue(userServiceLayer_buyer.Edit_Product_In_ShopingBasket(userServiceLayer_buyer.Watch_Cart()[0].Key, 1));
            Assert.AreEqual(userServiceLayer_buyer.Watch_Cart()[0].Value, 1);  // the amount has been changed - 1 
            Assert.IsTrue(userServiceLayer_buyer.Edit_Product_In_ShopingBasket(userServiceLayer_buyer.Watch_Cart()[0].Key, 0));
            Assert.ThrowsException<Exception>(() => { userServiceLayer_buyer.Watch_Cart(); });  //the item has been removed from the cart - 2 

            Assert.ThrowsException<Exception>(() => { userServiceLayer_buyer.Edit_Product_In_ShopingBasket(userServiceLayer_buyer.Watch_Cart()[0].Key, 20); }, "this amount is not available in stock. UC 2.7.2 happy-3");
            Assert.ThrowsException<Exception>(() => { userServiceLayer_buyer.Watch_Cart(); });  //tha cart has not changed. 
        }

        [TestMethod()]
        public void Update_unvalid_product_happyTest()   //Use Case: 2.7.2 Happy-4
        {
            List<KeyValuePair<ProductInStore, int>> user_cart = userServiceLayer_buyer.Watch_Cart();
            Assert.AreEqual(user_cart.Count, 1);
            List<ProductInStore> toBuy = userServiceLayer_buyer.GlobalSearch("muffin", null, null, -1, -1, -1, -1);
            Assert.IsTrue(userServiceLayer_buyer.Add_To_ShopingBasket(toBuy[0], 5));
            Assert.AreEqual(userServiceLayer_buyer.Watch_Cart().Count, 2);
            Assert.AreEqual(userServiceLayer_buyer.Watch_Cart()[1].Value, 5);
            if (userServiceLayer_seller.Remove_Product_Store("mtokim", "muffin"))
            {
                Assert.ThrowsException<Exception>(() => { userServiceLayer_buyer.Edit_Product_In_ShopingBasket(userServiceLayer_buyer.Watch_Cart()[1].Key, 2); }, "this product is no longer avilable in store. UC 2.7.2 happy-4");
                Assert.AreEqual(user_cart.Count, 1);  
            }
        }


        [TestMethod()]
        public void Update_product_that_is_not_in_cart_happyTest()   //Use Case: 2.7.2 Happy-4
        {
            List<ProductInStore> cheescakeToBuy = userServiceLayer_buyer.GlobalSearch("cheescake", null, null, -1, -1, -1, -1);
            int cart_amount = userServiceLayer_buyer.Watch_Cart().Count;
            Assert.ThrowsException<Exception>(() => { userServiceLayer_buyer.Edit_Product_In_ShopingBasket(cheescakeToBuy[0], 2); }, "tring to update producat that is not in cart.   UC 2.7.2 bad");
            Assert.AreEqual(userServiceLayer_buyer.Watch_Cart().Count, cart_amount);
        }

        [TestMethod()]
        public void Update_unvalid_amount_of_product_happyTest()   //Use Case: 2.7.2 Happy-4
        {
            List<ProductInStore> cheescakeToBuy = userServiceLayer_buyer.GlobalSearch("cheescake", null, null, -1, -1, -1, -1);
            int cart_amount = userServiceLayer_buyer.Watch_Cart().Count;
            Assert.ThrowsException<Exception>(() => { userServiceLayer_buyer.Edit_Product_In_ShopingBasket(cheescakeToBuy[0], -3); }, "tring to update to nagetive amount of producat.   UC 2.7.2 sad-1");
            Assert.AreEqual(userServiceLayer_buyer.Watch_Cart().Count, cart_amount);
        }

        [TestCleanup]
        public void TestClean()
        {
            userServiceLayer_buyer.Logout();
            userServiceLayer_seller.Logout();
            userServiceLayer_buyer = null;
            userServiceLayer_seller = null;
            serviceLayer.CleanUpSystem();
            serviceLayer = null;
        }

    }
}
