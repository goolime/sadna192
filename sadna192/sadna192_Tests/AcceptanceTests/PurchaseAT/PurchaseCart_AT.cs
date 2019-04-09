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
    public class PurchaseCart_AT
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
            if (userServiceLayer_seller.Register("purchaseCart_user", "1221"))
                if (userServiceLayer_seller.Login("purchaseCart_user", "1221"))
                    if (userServiceLayer_seller.Open_Store("Renuar") && userServiceLayer_seller.Open_Store("mike place"))
                    {
                        bool product1 = userServiceLayer_seller.Add_Product_Store("Renuar", "red dress", "clothes", 45, 10, null, null);  //new regulerPolicy()  new immidiatePolicy() ect. 
                        bool product2 = userServiceLayer_seller.Add_Product_Store("Renuar", "pants", "clothes", 55, 5, null, null);
                        bool product3 = userServiceLayer_seller.Add_Product_Store("mike place", "soda", "drinks", 5, 50, null, null);
                    }

            userServiceLayer_buyer = serviceLayer.connect();
            if (userServiceLayer_buyer.Register("purchaseCart_user2", "97531"))
                if (userServiceLayer_buyer.Login("purchaseCart_user2", "97531"))
                {
                    List<ProductInStore> toBuy1 = userServiceLayer_buyer.GlobalSearch("red dress", "clothes", null, 5, 150, 0, 0);
                    List<ProductInStore> toBuy2 = userServiceLayer_buyer.GlobalSearch("pants", "clothes", null, 5, 150, 0, 0);
                    List<ProductInStore> toBuy3 = userServiceLayer_buyer.GlobalSearch("soda", "drinks", null, 5, 150, 0, 0);
                    bool build_cart = userServiceLayer_buyer.Add_To_ShopingBasket(toBuy1[0], 1);
                    build_cart = userServiceLayer_buyer.Add_To_ShopingBasket(toBuy2[0], 1);
                    build_cart = userServiceLayer_buyer.Add_To_ShopingBasket(toBuy3[0], 1);
                }

        }

        [TestMethod()]
        public void purchaseCart_happy_test()
        {
            int amount = userServiceLayer_buyer.Watch_Cart().Count;
            List<KeyValuePair<ProductInStore, KeyValuePair<int, double>>> buing = userServiceLayer_buyer.Purchase_Store_cart("Renuar");
            Assert.AreEqual(userServiceLayer_buyer.Watch_Cart().Count, amount - 2);  // the products from renuar has been removed from the cart

            List<ProductInStore> redDress = userServiceLayer_seller.GlobalSearch("red dress", "clothes", null, 0, 150, 0, 0);
            for (int i = 0; i < redDress.Count; i++)
                if (redDress[i].getStoreName.Equals("Renuar"))
                    Assert.AreEqual(redDress[i].getAmount(), 9);
        }
    }
}
