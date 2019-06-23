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
    public class WatchCart_AT
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
                serviceLayer.Create_ServiceLayer(new Stub_deliverySystem(), new Stub_paymentSystem(), "admin", "1234Asdf");
            } catch (Exception) { }

            userServiceLayer_seller = serviceLayer.Connect();
            userServiceLayer_buyer = serviceLayer.Connect();

            try
            {
                userServiceLayer_seller.Register("watchCartUser", "1221LoOl");
                userServiceLayer_buyer.Register("watchCartUser2", "3456ZxcX");
            } catch (Exception) { }
            try
            {
                userServiceLayer_seller.Login("watchCartUser", "1221LoOl");
                userServiceLayer_buyer.Login("watchCartUser2", "3456ZxcX");
                if (userServiceLayer_seller.Open_Store("grocery store") && userServiceLayer_seller.Open_Store("zara"))
                {
                    userServiceLayer_seller.Add_Product_Store("grocery store", "suger", "food", 12, 50, new noDiscount(), new regularPolicy());
                    userServiceLayer_seller.Add_Product_Store("grocery store", "salt", "food", 10, 25, new noDiscount(), new regularPolicy());
                    userServiceLayer_seller.Add_Product_Store("zara", "shirt", "clothes", 75, 20, new noDiscount(), new regularPolicy());
                }
                List<ProductInStore> toBuy1 = userServiceLayer_buyer.GlobalSearch("suger", null, null, -1, -1, -1, -1);
                List<ProductInStore> toBuy2 = userServiceLayer_buyer.GlobalSearch("shirt", null, null, -1, -1, -1, -1);
                List<ProductInStore> toBuy3 = userServiceLayer_buyer.GlobalSearch("salt", null, null, -1, -1, -1, -1);
                userServiceLayer_buyer.Add_To_ShopingBasket(toBuy1[0], 1);
                userServiceLayer_buyer.Add_To_ShopingBasket(toBuy3[0], 3);
                userServiceLayer_buyer.Add_To_ShopingBasket(toBuy2[0], 2);
            }
            catch (Exception) { }
        }

        [TestMethod()]
        public void Watch_cart_of_existing_store_in_cart_happyTest()
        {
            List<KeyValuePair<ProductInStore, int>> user_cart = userServiceLayer_buyer.Watch_Cart();
            Assert.AreEqual(user_cart.Count, 2);
            Assert.AreEqual(user_cart[0].Key.getStore().getName(), "grocery store");
            Assert.AreEqual(user_cart[1].Key.getStore().getName(), "zara");
        }

        [TestMethod()]
        public void Watch_cart_when_not_existing_store_in_cart_happyTest()
        {
            if (userServiceLayer_seller.Open_Store("Bim Bam Bom"))
                userServiceLayer_seller.Add_Product_Store("Bim Bam Bom", "polaroid camera", "electronic", 170, 20, new noDiscount(), new regularPolicy());

            List<KeyValuePair<ProductInStore, int>> user_cart = userServiceLayer_buyer.Watch_Cart();
            int cart_amount = user_cart.Count;
            for (int i = 0; i < cart_amount; i++)
            {
                Assert.AreNotEqual(user_cart[i].Key.getStore().getName(), "Bim Bam Bom");
            }
        }

        [TestCleanup]
        public void TestClean()
        {
            userServiceLayer_buyer.Logout();
            userServiceLayer_seller.Logout();
        }
    }
}
