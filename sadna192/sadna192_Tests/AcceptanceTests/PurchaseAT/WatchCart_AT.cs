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
    public class WatchCart_AT
    {
        private static I_ServiceLayer serviceLayer;
        private I_User_ServiceLayer userServiceLayer_buyer;
        private I_User_ServiceLayer userServiceLayer_seller;
        private I_DeliverySystem deliverySystem;
        private I_PaymentSystem paymentSystem;

        [TestInitialize]
        public void init()
        {
            serviceLayer = I_ServiceLayer.Create_ServiceLayer(deliverySystem, paymentSystem, "admin", "1234");
            userServiceLayer_seller = serviceLayer.connect();
            if (userServiceLayer_seller.Register("'watchCart_user", "1221"))
                if (userServiceLayer_seller.Login("watchCart_user", "1221"))
                    if (userServiceLayer_seller.Open_Store("grocery store") && userServiceLayer_seller.Open_Store("zara"))
                    {
                        bool product1 = userServiceLayer_seller.Add_Product_Store("grocery store", "suger", "food", 12, 50, null, null);
                        bool product2 = userServiceLayer_seller.Add_Product_Store("grocery store", "salt", "food", 10, 25, null, null);
                        bool product3 = userServiceLayer_seller.Add_Product_Store("zara", "shirt", "clothes", 75, 20, null, null);
                    }

            userServiceLayer_buyer = serviceLayer.connect();
            if (userServiceLayer_buyer.Register("watchCart_user2", "3456"))
                if (userServiceLayer_buyer.Login("watchCart_user2", "3456"))
                {
                    List<ProductInStore> toBuy1 = userServiceLayer_buyer.GlobalSearch("suger", "food", null, 0.5, 100, 0, 0);
                    List<ProductInStore> toBuy2 = userServiceLayer_buyer.GlobalSearch("salt", "food", null, 0.5, 100, 0, 0);
                    List<ProductInStore> toBuy3 = userServiceLayer_buyer.GlobalSearch("suger", "food", null, 50, 150, 0, 0);
                    bool build_cart = userServiceLayer_buyer.Add_To_ShopingBasket(toBuy1[0], 1);
                    build_cart = userServiceLayer_buyer.Add_To_ShopingBasket(toBuy3[0], 2);
                    build_cart = userServiceLayer_buyer.Add_To_ShopingBasket(toBuy2[0], 1);
                }
        }

        [TestMethod()]
        public void watch_cart_of_existing_store_in_cart_test()
        {
            List<KeyValuePair<ProductInStore, int>> user_cart = userServiceLayer_buyer.Watch_Cart();
            Assert.AreEqual(user_cart.Count, 3);
            Assert.AreEqual(user_cart[0].Key.getStoreName(), "grocery store");
            Assert.AreEqual(user_cart[1].Key.getStoreName(), "grocery store");
            Assert.AreEqual(user_cart[2].Key.getStoreName(), "zara");
        }

        [TestMethod()]
        public void watch_cart_when_not_existing_store_in_cart_test()
        {
            if (userServiceLayer_seller.Open_Store("Bim Bam Bom"))
                bool product4 = userServiceLayer_seller.Add_Product_Store("Bim Bam Bom", "polaroid camera", "electronic", 170, 20, null, null);

            List<KeyValuePair<ProductInStore, int>> user_cart = userServiceLayer_buyer.Watch_Cart();
            int cart_amount = user_cart.Count;
            for (int i = 0; i < cart_amount; i++)
            {
                Assert.AreNotEqual(user_cart[i].Key.getStoreName(), "Bim Bam Bom");
            }
        }
    }
}
