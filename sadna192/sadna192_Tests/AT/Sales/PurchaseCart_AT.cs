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
    public class PurchaseCart_AT
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
                serviceLayer.Create_ServiceLayer(new Stub_deliverySystem(), new Stub_paymentSystem(), "admin", "123456Ui");
            }
            catch (Exception) { }
            userServiceLayer_seller = serviceLayer.Connect(new Stub_Alerter());
            userServiceLayer_buyer = serviceLayer.Connect(new Stub_Alerter());
            try
            {
                userServiceLayer_seller.Register("purchaseCartUser", "1221Qwer");
                userServiceLayer_buyer.Register("purchaseCartBuyer", "97531Tyuy");
            }
            catch (Exception) { }
            try
            {
                userServiceLayer_buyer.Login("purchaseCartBuyer", "97531Tyuy");
                userServiceLayer_seller.Login("purchaseCartUser", "1221Qwer");
                    if (userServiceLayer_seller.Open_Store("Renuar") && userServiceLayer_seller.Open_Store("mike place"))
                    {
                        userServiceLayer_seller.Add_Product_Store("Renuar", "red dress", "clothes", 45, 10, new noDiscount(), new regularPolicy());
                        userServiceLayer_seller.Add_Product_Store("Renuar", "pants", "clothes", 55, 5, new noDiscount(), new regularPolicy());
                        userServiceLayer_seller.Add_Product_Store("mike place", "soda", "drinks", 5, 50, new noDiscount(), new regularPolicy());
                    }
                    List<ProductInStore> toBuy1 = userServiceLayer_buyer.GlobalSearch("red dress", null, null, -1, -1, -1, -1);
                    List<ProductInStore> toBuy2 = userServiceLayer_buyer.GlobalSearch("soda", null, null, -1, -1, -1, -1);
                    userServiceLayer_buyer.Add_To_ShopingBasket(toBuy1[0], 1);
                    userServiceLayer_buyer.Add_To_ShopingBasket(toBuy2[0], 1);    
            }
            catch (Exception) { }
        }

        [TestMethod()]
        public void PurchaseCart_happyTest()
        {
            
            int amount = userServiceLayer_buyer.Watch_Cart().Count;
            List<KeyValuePair<ProductInStore, KeyValuePair<int, double>>> buying = userServiceLayer_buyer.Purchase_Store_cart("Renuar");
            double sum =0;
            for (int i=0; i< buying.Count; i++)
            {
                sum = sum + buying[i].Value.Value;
            }
            Assert.AreEqual(sum, 45);
            Assert.IsTrue(userServiceLayer_buyer.Finalize_Purchase("address buyer 1", "paymnent 1234")); 
            Assert.AreEqual(userServiceLayer_buyer.Watch_Cart().Count, amount - 1);  // the products from renuar has been removed from the cart
            Assert.ThrowsException<Exception>(() => { userServiceLayer_buyer.Purchase_Store_cart("Renuar"); }, "the buyer has no item in the cart from this store");

            List<ProductInStore> redDress = userServiceLayer_seller.GlobalSearch("red dress", null, null, -1, -1, -1, -1);
            for (int i = 0; i < redDress.Count; i++)
                if (redDress[i].getStore().getName().Equals("Renuar"))
                    Assert.AreEqual(redDress[i].getAmount(), 9);
        }

     /*   [TestMethod()]
        public void Canceled_purchaseCart_10MIN_happyTest()
        {
            int amount = userServiceLayer_buyer.Watch_Cart().Count;
            List<KeyValuePair<ProductInStore, KeyValuePair<int, double>>> buing = userServiceLayer_buyer.Purchase_Store_cart("mike place");
            double sum = 0;
            for (int i = 0; i < buing.Count; i++)
            {
                sum = sum + buing[i].Value.Value;
            }
            Assert.AreEqual(sum, 5);

            System.Threading.Thread.Sleep(TimeSpan.FromMinutes(10));   //canceled the purchase

            Assert.AreEqual(userServiceLayer_buyer.Watch_Cart().Count, amount - 1);  // the product has been removed from the cart
            Assert.ThrowsException<Exception>(() => { userServiceLayer_buyer.Purchase_Store_cart("mike place"); }, "the buyer has no item in the cart from this store");

            List<ProductInStore> sodaList = userServiceLayer_seller.GlobalSearch("soda", "drinks", null, 0, 150, 0, 0);
            for (int i = 0; i < sodaList.Count; i++)
                if (sodaList[i].getStore().getName().Equals("mike place"))
                    Assert.AreEqual(sodaList[i].getAmount(), 50);       
        }*/

        /*
        [TestMethod()]
        public void purchaseCart_address_details_wrong_test()
        {
            List<ProductInStore> sodaToBuy = userServiceLayer_buyer.GlobalSearch("soda", "drinks", null, 5, 150, 0, 0);
            userServiceLayer_buyer.Add_To_ShopingBasket(sodaToBuy[0], 2);

            int amount = userServiceLayer_buyer.Watch_Cart().Count;
            List<KeyValuePair<ProductInStore, KeyValuePair<int, double>>> buing = userServiceLayer_buyer.Purchase_Store_cart("mike place");
            double sum = 0;
            for (int i = 0; i < buing.Count; i++)
            {
                sum = sum + buing[i].Value.Value;
            }
            Assert.AreEqual(sum, 10);
            Assert.ThrowsException<Exception>(() => { userServiceLayer_buyer.Finalize_Purchase("", "paymnent 1234"); }, "adress details are not valid");
            Assert.AreEqual(userServiceLayer_buyer.Watch_Cart().Count, amount - 1);  // the product has been removed from the cart

            List<ProductInStore> sodaList = userServiceLayer_seller.GlobalSearch("soda", "drinks", null, 0, 150, 0, 0);
            for (int i = 0; i < sodaList.Count; i++)
                if (sodaList[i].getStore().getName().Equals("mike place"))
                    Assert.AreEqual(sodaList[i].getAmount(), 50);
        }

        [TestMethod()]
        public void purchaseCart_payment_details_wrong_test()
        {
            List<ProductInStore> sodaToBuy = userServiceLayer_buyer.GlobalSearch("soda", "drinks", null, 5, 150, 0, 0);
            userServiceLayer_buyer.Add_To_ShopingBasket(sodaToBuy[0], 2);

            int amount = userServiceLayer_buyer.Watch_Cart().Count;
            List<KeyValuePair<ProductInStore, KeyValuePair<int, double>>> buing = userServiceLayer_buyer.Purchase_Store_cart("mike place");
            double sum = 0;
            for (int i = 0; i < buing.Count; i++)
            {
                sum = sum + buing[i].Value.Value;
            }
            Assert.AreEqual(sum, 10);
            Assert.ThrowsException<Exception>(() => { userServiceLayer_buyer.Finalize_Purchase("address buyer 1", " "); }, "payment details are not valid");
            Assert.AreEqual(userServiceLayer_buyer.Watch_Cart().Count, amount - 1);  // the product has been removed from the cart

            List<ProductInStore> sodaList = userServiceLayer_seller.GlobalSearch("soda", "drinks", null, 0, 150, 0, 0);
            for (int i = 0; i < sodaList.Count; i++)
                if (sodaList[i].getStore().getName().Equals("mike place"))
                    Assert.AreEqual(sodaList[i].getAmount(), 50);
        }
        */

        [TestCleanup]
        public void TestClean()
        {
            userServiceLayer_buyer.Logout();
            userServiceLayer_seller.Logout();
            serviceLayer.CleanUpSystem();
            serviceLayer = null;
        }

    }
}
