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
    public class AddToCart_AT
    {
        private static I_ServiceLayer serviceLayer;
        private I_User_ServiceLayer userServiceLayer1;
        private I_User_ServiceLayer userServiceLayer2;

        [TestInitialize]
        public void Init()
        {
            serviceLayer = new ServiceLayer();
            try
            {
                serviceLayer.Create_ServiceLayer(new Stub_deliverySystem(), new Stub_paymentSystem(), "admin", "1234DfGb");
            }
            catch (Exception) { }

            userServiceLayer1 = serviceLayer.Connect();
            userServiceLayer2 = serviceLayer.Connect();
            try
            {
                userServiceLayer1.Register("addToCartUser", "1221Xccv");
                userServiceLayer2.Register("addToCartUser2", "9876THgb");
            }
            catch (Exception) { }
            try
            {
                userServiceLayer2.Login("addToCartUser2", "9876THgb");
                if (userServiceLayer1.Login("addToCartUser", "1221Xccv"))
                    if (userServiceLayer1.Open_Store("MiniGrocery"))
                    {
                        userServiceLayer1.Add_Product_Store("MiniGrocery", "apple", "food", 4, 10, new noDiscount(), new regularPolicy());
                        userServiceLayer1.Add_Product_Store("MiniGrocery", "cheese", "food", 8.6, 2, new noDiscount(), new regularPolicy());
                    }
            }
            catch (Exception) { }
        }

        [TestMethod()]
        public void AddToCart_happyTest()
        {
            Assert.ThrowsException<Exception>(() => { userServiceLayer2.Watch_Cart(); }, "watch list is empty");
            List<ProductInStore> appleToBuy = userServiceLayer2.GlobalSearch("apple", null,null, -1, -1, -1,-1);
            Assert.IsTrue(userServiceLayer2.Add_To_ShopingBasket(appleToBuy[0], 1));
            Assert.AreEqual(userServiceLayer2.Watch_Cart().Count, 1);
            List<ProductInStore> cheeseToBuy = userServiceLayer2.GlobalSearch("cheese", null, null, -1, -1, -1, -1);
            Assert.ThrowsException<Exception>(() => { userServiceLayer2.Add_To_ShopingBasket(cheeseToBuy[0], 6); }, "the wanted amount of the product is bigger than the amount that the store have");
            Assert.AreEqual(userServiceLayer2.Watch_Cart().Count, 1);    //the cart hasn't changed. 
        }

        [TestMethod()]
        public void AddToCart_sadTest()
        {
            if (userServiceLayer1.Open_Store("Mini Grocery2"))
            {
                userServiceLayer1.Add_Product_Store("Mini Grocery2", "tea2", "food", 4, 10, new noDiscount(), new regularPolicy());
            }
            List<ProductInStore> appleToBuy = userServiceLayer2.GlobalSearch("apple", null, null, -1, -1, -1, -1);
            List<ProductInStore> teaToBuy = userServiceLayer2.GlobalSearch("tea2", null, null, -1, -1, -1, -1);
            ProductInStore broken_product=new ProductInStore(teaToBuy[0].getProduct(), teaToBuy[0].getAmount(), teaToBuy[0].getPrice(), appleToBuy[0].getStore(), teaToBuy[0].getDiscount(), teaToBuy[0].getPolicy());
            Assert.ThrowsException<Exception>(() => { userServiceLayer2.Add_To_ShopingBasket(broken_product, 1); }, "trying to add to cart product that not exist in store");
            Assert.ThrowsException<Exception>(() => { userServiceLayer2.Watch_Cart(); });
        }

        [TestCleanup]
        public void TestClean()
        {
            userServiceLayer1.Logout();
            userServiceLayer2.Logout();
        }

    }
}
