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
    public class AddOwner_AT
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
            if (userServiceLayer1.Register("addOwner_user", "1221"))
                if (userServiceLayer1.Login("addOwner_user", "1221"))
                {
                    if (userServiceLayer1.Open_Store("shopipi"))
                    {
                        bool product = userServiceLayer1.Add_Product_Store("shopipi", "cake", "food", 25, 4, null, null);
                        product = userServiceLayer1.Add_Product_Store("shopipi", "water", "drink", 9, 100, null, null);
                        product = userServiceLayer1.Add_Product_Store("shopipi", "chips", "food", 17, 50, null, null);
                    }
                }
            userServiceLayer2 = serviceLayer.connect();
            if (userServiceLayer2.Register("addOwner_user2", "87654"))
                bool login1 = userServiceLayer2.Login("addOwner_user2", "87654");
     

        }

        [TestMethod()]
        public void add_shop_owner_tests()
        {
            Assert.ThrowsException<Exception>(() => { userServiceLayer2.Add_Product_Store("shopipi", "cola", "drink", 7, 10, null, null); }, "user2 still not owner so he can't add products");
            Assert.ThrowsException<Exception>(() => { userServiceLayer2.Update_Product_Store("shopipi", "cake", "chocolate cake", "food", 30, 9, null, null); }, "user2 still not owner so he can't update products");
            Assert.ThrowsException<Exception>(() => { userServiceLayer2.Remove_Product_Store("shopipi", "cake"); }, "user2 still not owner so he can't remove products");

            Assert.IsTrue(userServiceLayer1.Add_Store_Owner("shopipi", "addOwner_user2"));

            List<ProductInStore> search1 = userServiceLayer2.GlobalSearch("cola", "drink", null, 0, 100, 0, 0);
            int pre_amount = search1.Count;
            Assert.IsTrue(userServiceLayer2.Add_Product_Store("shopipi", "cola", "drink", 7, 10, null, null));
            search1 = userServiceLayer2.GlobalSearch("cola", "drink", null, 0, 100, 0, 0);
            Assert.AreEquals(pre_amount + 1, search1.Count);

            search1 = userServiceLayer2.GlobalSearch("cake", "food", null, 0, 100, 0, 0);
            int cake_pre_amount = search1.Count;
            List<ProductInStore> search2 = userServiceLayer2.GlobalSearch("chocolate cake", "food", null, 0, 100, 0, 0);
            int chocolate_cake_pre_amount = search2.Count;
            Assert.IsTrue(userServiceLayer2.Update_Product_Store("shopipi", "cake", "chocolate cake", "food", 30, 9, null, null));
            search1 = userServiceLayer2.GlobalSearch("cake", "food", null, 0, 100, 0, 0);
            Assert.AreEquals(cake_pre_amount - 1, search1.Count);
            search2 = userServiceLayer2.GlobalSearch("chocolate cake", "food", null, 0, 100, 0, 0);
            Assert.AreEquals(chocolate_cake_pre_amount +1 , search2.Count);
            Assert.AreEquals(search2[0].getStoreName(), "shopipi");
            for (int i = 0; i < search1.Count; i++)
            {
                if (search1[i].getStoreName().Equals("shopipi"))
                    Assert.Fail("owner faild to update the product properly");
            }

            Assert.IsTrue(userServiceLayer2.Remove_Product_Store("shopipi", "chocolate cake"));
            search2 = userServiceLayer2.GlobalSearch("chocolate cake", "food", null, 0, 100, 0, 0);
            Assert.AreEquals(chocolate_cake_pre_amount -1, search2.Count);
            for (int i = 0; i < search2.Count; i++)
            {
                if (search2[i].getStoreName().Equals("shopipi"))
                    Assert.Fail("owner faild to remove the product properly");
            }

        }

        [TestMethod()]
        public void add_shop_owner_that_not_exist_test()
        {
            Assert.ThrowsException<Exception>(() => { userServiceLayer1.Add_Store_Owner("shopipi", "user_not_exist"); }, "faild to add owner that not exist");
        }

        [TestMethod()]
        public void add_the_same_owner_again_test()
        {
            I_User_ServiceLayer userServiceLayer3 = serviceLayer.connect();
            bool reg = userServiceLayer3.Register("addOwner_user3", "9999");

            Assert.IsTrue(userServiceLayer1.Add_Store_Owner("shopipi", "addOwner_user3"));
            Assert.ThrowsException<Exception>(() => { userServiceLayer1.Add_Store_Owner("shopipi", "addOwner_user3"); }, "this user is already owner");

        }

        [TestMethod()]
        public void not_owner_try_to_add_owner_test()  //bad
        {
            I_User_ServiceLayer tmp_userServiceLayer = serviceLayer.connect();
            bool reg = tmp_userServiceLayer.Register("addOwner_tmp_user", "11111");

            I_User_ServiceLayer userServiceLayer4 = serviceLayer.connect();
            if (userServiceLayer4.Register("addOwner_user4", "12345"))
                bool log = userServiceLayer4.Login("addOwner_user4", "12345");

            Assert.ThrowsException<Exception>(() => { userServiceLayer4.Add_Store_Owner("shopipi", "addOwner_tmp_user");}, "only store owner can add new owner to the store");
        }
    }
}
