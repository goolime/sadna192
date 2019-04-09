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
    public class AddToCart_AT
    {
        private static I_ServiceLayer serviceLayer;
        private I_User_ServiceLayer userServiceLayer1;
        private I_User_ServiceLayer userServiceLayer2;

        [TestInitialize]
        public void init()
        {
            serviceLayer = new ServiceLayer();
            serviceLayer.Create_ServiceLayer(new stub_deliverySystem(), new stub_paymentSystem(), "admin", "1234DfGb");
            userServiceLayer1 = serviceLayer.Connect();
            if (userServiceLayer1.Register("addToCartUser", "1221Xccv"))
                if (userServiceLayer1.Login("addToCartUser", "1221Xccv"))
                    if (userServiceLayer1.Open_Store("Mini Grocery"))
                    {
                        userServiceLayer1.Add_Product_Store("Mini Grocery", "apple", "food", 4, 10, new noDiscount(), new regularPolicy());
                        userServiceLayer1.Add_Product_Store("Mini Grocery", "cheese", "food", 8.6, 2, new noDiscount(), new regularPolicy());
                    }

            userServiceLayer2 = serviceLayer.Connect();
            if (userServiceLayer2.Register("addToCartUser2", "9876THgb"))
                userServiceLayer2.Login("addToCartUser2", "9876THgb");
                   
        }

        [TestMethod()]
        public void addToCart_happy_test()
        {
            Assert.AreEqual(userServiceLayer2.Watch_Cart().Count, 0);
            List<ProductInStore> appleToBuy = userServiceLayer2.GlobalSearch("apple", "food",null, 0.5, 15, 0,0);
            Assert.IsTrue(userServiceLayer2.Add_To_ShopingBasket(appleToBuy[0], 1));
            Assert.AreEqual(userServiceLayer2.Watch_Cart().Count, 1);
            List<ProductInStore> cheeseToBuy = userServiceLayer2.GlobalSearch("cheese", "food", null, 0.5, 15, 0, 0);
            Assert.ThrowsException<Exception>(() => { userServiceLayer2.Add_To_ShopingBasket(cheeseToBuy[0], 6); }, "wanted amount of the product is bigger than the amount that the store have");
            Assert.AreEqual(userServiceLayer2.Watch_Cart().Count, 1);    //the cart hasn't changed. 
        }

        [TestMethod()]
        public void addToCart_bad_test()
        {
            Assert.Fail("add to cart product that not exist in the store - how to do this?"); 
        }

    }
}
