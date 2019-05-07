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
    public class AddToCart_AT
    {
        private static I_ServiceLayer serviceLayer;
        private I_User_ServiceLayer userServiceLayer1;
        private I_User_ServiceLayer userServiceLayer2;
        private I_DeliverySystem deliverySystem;
        private I_PaymentSystem paymentSystem;

        [TestInitialize]
        public void init()
        {
            serviceLayer = I_ServiceLayer.Create_ServiceLayer(deliverySystem, paymentSystem, "admin", "1234");
            userServiceLayer1 = serviceLayer.connect();
            if (userServiceLayer1.Register("addToCart_user", "1221"))
                if (userServiceLayer1.Login("addToCart_user", "1221"))
                    if (userServiceLayer1.Open_Store("Mini Grocery"))
                    {
                        bool apple_product1 = userServiceLayer1.Add_Product_Store("Mini Grocery", "apple", "food", 4, 10, null, null);
                        bool cheese_product1 = userServiceLayer1.Add_Product_Store("Mini Grocery", "cheese", "food", 8.6, 2, null, null);
                    }

            userServiceLayer2 = serviceLayer.connect();
            if (userServiceLayer2.Register("addToCart_user2", "9876"))
                bool loginUser = userServiceLayer2.Login("addToCart_user2", "9876");
                   
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
            Assert.AreEqual(userServiceLayer.Watch_Cart().Count, 1);    //the cart hasn't changed. 
        }

        [TestMethod()]
        public void addToCart_bad_test()
        {
            int amount_of_products = userServiceLayer.Watch_Cart().Count; 
            List<ProductInStore> TVToBuy = userServiceLayer.GlobalSearch("TV", "electronics", null, 1000, 1500, 0, 0);  //the list will be empty
            ///???
        }

    }
}
