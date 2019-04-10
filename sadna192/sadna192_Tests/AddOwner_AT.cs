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
    public class AddOwner_AT
    {
        private static I_ServiceLayer serviceLayer;
        private I_User_ServiceLayer userServiceLayer1;
        private I_User_ServiceLayer userServiceLayer2;

        [TestInitialize]
        public void init()
        {
            serviceLayer = new ServiceLayer();
            serviceLayer.Create_ServiceLayer(new stub_deliverySystem(), new stub_paymentSystem(), "admin", "1234FgHY");
            userServiceLayer1 = serviceLayer.Connect();
            if (userServiceLayer1.Register("addOwneruser", "1221Abcd"))
                if (userServiceLayer1.Login("addOwneruser", "1221Abcd"))
                {
                    if (userServiceLayer1.Open_Store("shopipi"))
                    {
                        userServiceLayer1.Add_Product_Store("shopipi", "cake", "food", 25, 4, new noDiscount(), new regularPolicy());
                        userServiceLayer1.Add_Product_Store("shopipi", "water", "drink", 9, 100, new noDiscount(), new regularPolicy());
                        userServiceLayer1.Add_Product_Store("shopipi", "chips", "food", 17, 50, new noDiscount(), new regularPolicy());
                    }
                }
            userServiceLayer2 = serviceLayer.Connect();
            if (userServiceLayer2.Register("addOwneruser2", "87654Abcd"))
                userServiceLayer2.Login("addOwneruser2", "87654Abcd");
     

        }

        [TestMethod()]
        public void add_shop_owner_tests()
        {
            Assert.ThrowsException<Exception>(() => { userServiceLayer2.Add_Product_Store("shopipi", "cola", "drink", 7, 10, new noDiscount(), new regularPolicy()); }, "user2 still not owner so he can't add products");
            Assert.ThrowsException<Exception>(() => { userServiceLayer2.Update_Product_Store("shopipi", "cake", "chocolate cake", "food", 30, 9, new noDiscount(), new regularPolicy()); }, "user2 still not owner so he can't update products");
            Assert.ThrowsException<Exception>(() => { userServiceLayer2.Remove_Product_Store("shopipi", "cake"); }, "user2 still not owner so he can't remove products");

            Assert.IsTrue(userServiceLayer1.Add_Store_Owner("shopipi", "addOwneruser2"));

            List<ProductInStore> search1 = userServiceLayer2.GlobalSearch("cola", null, null, -1, -1, -1, -1);
            int pre_amount = search1.Count;
            Assert.IsTrue(userServiceLayer2.Add_Product_Store("shopipi", "cola", "drink", 7, 10, new noDiscount(), new regularPolicy()));
            search1 = userServiceLayer2.GlobalSearch("cola", null, null, -1, -1, -1, -1);
            Assert.AreEqual(pre_amount + 1, search1.Count);

            search1 = userServiceLayer2.GlobalSearch("cake", null, null, -1, -1, -1, -1);
            int cake_pre_amount = search1.Count;
            List<ProductInStore> search2 = userServiceLayer2.GlobalSearch("chocolate cake", null, null, -1, -1, -1, -1);
            int chocolate_cake_pre_amount = search2.Count;
            Assert.IsTrue(userServiceLayer2.Update_Product_Store("shopipi", "cake", "chocolate cake", "food", 30, 9, new noDiscount(), new regularPolicy()));
            search1 = userServiceLayer2.GlobalSearch("cake", null, null, -1, -1, -1, -1);
            Assert.AreEqual(cake_pre_amount - 1, search1.Count);
            search2 = userServiceLayer2.GlobalSearch("chocolate cake", null, null, -1, -1, -1, -1);
            Assert.AreEqual(chocolate_cake_pre_amount +1 , search2.Count);
            chocolate_cake_pre_amount = search2.Count;
            Assert.AreEqual(search2[0].getStore().getName(), "shopipi");
            for (int i = 0; i < search1.Count; i++)
            {
                if (search1[i].getStore().getName().Equals("shopipi"))
                    Assert.Fail("owner faild to update the product properly");
            }

            Assert.IsTrue(userServiceLayer2.Remove_Product_Store("shopipi", "chocolate cake"));
            search2 = userServiceLayer2.GlobalSearch("chocolate cake", null, null, -1, -1, -1, -1);
            Assert.AreEqual(chocolate_cake_pre_amount -1, search2.Count);
            for (int i = 0; i < search2.Count; i++)
            {
                if (search2[i].getStore().getName().Equals("shopipi"))
                    Assert.Fail("owner faild to remove the product properly");
            }
            
        }

        [TestMethod()]
        public void add_shop_owner_that_not_exist_test()
        {
            Assert.ThrowsException<Exception>(() => { userServiceLayer1.Add_Store_Owner("shopipi", "userNotExist"); }, "faild to add owner that not exist");
        }

        [TestMethod()]
        public void add_the_same_owner_again_test()
        {
            I_User_ServiceLayer userServiceLayer3 = serviceLayer.Connect();
            userServiceLayer3.Register("addOwneruser3", "9999Abcd");

            Assert.IsTrue(userServiceLayer1.Add_Store_Owner("shopipi", "addOwneruser3"));
            Assert.ThrowsException<Exception>(() => { userServiceLayer1.Add_Store_Owner("shopipi", "addOwneruser3"); }, "this user is already owner");

        }

        [TestMethod()]
        public void not_owner_try_to_add_owner_test()  //bad
        {
            I_User_ServiceLayer tmp_userServiceLayer = serviceLayer.Connect();
           tmp_userServiceLayer.Register("addOwnertmpuser", "11111Abcd");

            I_User_ServiceLayer userServiceLayer4 = serviceLayer.Connect();
            if (userServiceLayer4.Register("addOwneruser4", "12345Abcd"))
                userServiceLayer4.Login("addOwneruser4", "12345Abcd");

            Assert.ThrowsException<Exception>(() => { userServiceLayer4.Add_Store_Owner("shopipi", "addOwner_tmp_user");}, "only store owner can add new owner to the store");
        }
    }
}
