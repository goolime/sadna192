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
    public class UpdateCart_AT
    {
        private static I_ServiceLayer serviceLayer;
        private I_User_ServiceLayer userServiceLayer_buyer;
        private I_User_ServiceLayer userServiceLayer_seller;

        [TestInitialize]
        public void init()
        {
            serviceLayer = new ServiceLayer();
            serviceLayer.Create_ServiceLayer(new stub_deliverySystem(), new stub_paymentSystem(), "admin", "1234");
            userServiceLayer_seller = serviceLayer.Connect();
            if (userServiceLayer_seller.Register("updateCart_user", "1221"))
                if (userServiceLayer_seller.Login("updateCart_user", "1221"))              
                    if (userServiceLayer_seller.Open_Store("MiniMarket"))
                    {
                        userServiceLayer_seller.Add_Product_Store("MiniMarket", "red apple", "food", 4, 10, null, null);
                        userServiceLayer_seller.Add_Product_Store("MiniMarket", "cheesecake", "food", 15, 5, null, null);
                    }

            userServiceLayer_buyer = serviceLayer.Connect();
            if (userServiceLayer_buyer.Register("updateCart_user2", "97531"))
                if (userServiceLayer_buyer.Login("updateCart_user2", "97531"))
                {
                    List<ProductInStore> appleToBuy = userServiceLayer_buyer.GlobalSearch("red apple", "food", null, 0.5, 15, 0, 0);
                    userServiceLayer_buyer.Add_To_ShopingBasket(appleToBuy[0], 3);
                }

        }

        [TestMethod()]
        public void addToCart_happy_test()
        {
            Assert.AreEqual(userServiceLayer_buyer.Watch_Cart().Count, 1);
            Assert.AreEqual(userServiceLayer_buyer.Watch_Cart()[0].Value, 3);
            Assert.IsTrue(userServiceLayer_buyer.Edit_Product_In_ShopingBasket(userServiceLayer_buyer.Watch_Cart()[0].Key, 1));
            Assert.AreEqual(userServiceLayer_buyer.Watch_Cart()[0].Value, 1);  // the amount has been changed - 1 
            Assert.IsTrue(userServiceLayer_buyer.Edit_Product_In_ShopingBasket(userServiceLayer_buyer.Watch_Cart()[0].Key, 0));
            Assert.AreEqual(userServiceLayer_buyer.Watch_Cart().Count, 0);  //the item has been removed from the cart - 2 

            //Assert.IsFalse(userServiceLayer.Edit_Product_In_ShopingBasket(userServiceLayer.Watch_Cart()[0].Key, 20));
            Assert.ThrowsException<Exception>(() => { userServiceLayer_buyer.Edit_Product_In_ShopingBasket(userServiceLayer_buyer.Watch_Cart()[0].Key, 20); }, "this amount is not available in stock. UC 2.7.2 happy-3");
            Assert.AreEqual(userServiceLayer_buyer.Watch_Cart().Count, 0);  //tha cart has not changed. 
        }

        [TestMethod()]
        public void update_unvalid_product_test()   //Use Case: 2.7.2 Happy-4
        {
            List<ProductInStore> appleToBuy = userServiceLayer_buyer.GlobalSearch("red apple", "food", null, 0.5, 15, 0, 0);
            bool build_cart = userServiceLayer_buyer.Add_To_ShopingBasket(appleToBuy[0], 3);
            Assert.AreEqual(userServiceLayer_buyer.Watch_Cart().Count, 1);
            Assert.AreEqual(userServiceLayer_buyer.Watch_Cart()[0].Value, 3);

            if(userServiceLayer_seller.Remove_Product_Store("MiniMarket", "red apple"))
            {
                Assert.ThrowsException<Exception>(() => { userServiceLayer_buyer.Edit_Product_In_ShopingBasket(userServiceLayer_buyer.Watch_Cart()[0].Key, 2); }, "this product is no longer avilable in store. UC 2.7.2 happy-4");
                Assert.AreEqual(userServiceLayer_buyer.Watch_Cart().Count, 0);
            } 
        }


        [TestMethod()]
        public void update_product_that_is_not_in_cart_test()   //Use Case: 2.7.2 Happy-4
        {
            List<ProductInStore> cheescakeToBuy = userServiceLayer_buyer.GlobalSearch("cheescake", "food", null, 0.5, 15, 0, 0);
            int cart_amount = userServiceLayer_buyer.Watch_Cart().Count;
            Assert.ThrowsException<Exception>(() => { userServiceLayer_buyer.Edit_Product_In_ShopingBasket(cheescakeToBuy[0], 2); }, "tring to update producat that is not in cart.   UC 2.7.2 bad");
            Assert.AreEqual(userServiceLayer_buyer.Watch_Cart().Count, cart_amount);
        }

        [TestMethod()]
        public void update_unvalid_amount_of_product__test()   //Use Case: 2.7.2 Happy-4
        {
            List<ProductInStore> cheescakeToBuy = userServiceLayer_buyer.GlobalSearch("cheescake", "food", null, 0.5, 15, 0, 0);
            int cart_amount = userServiceLayer_buyer.Watch_Cart().Count;
            Assert.ThrowsException<Exception>(() => { userServiceLayer_buyer.Edit_Product_In_ShopingBasket(cheescakeToBuy[0], -3); }, "tring to update to nagetive amount of producat.   UC 2.7.2 sad-1");
            Assert.AreEqual(userServiceLayer_buyer.Watch_Cart().Count, cart_amount);
        }

    }
}
