using Microsoft.VisualStudio.TestTools.UnitTesting;
using sadna192;
using System;
using System.Collections.Generic;
using static sadna192_Tests.Stubs;

namespace sadna192.Tests.AcceptanceTests
{
    [TestClass()]
    public class AddManager_AT
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
                serviceLayer.Create_ServiceLayer(new Stub_deliverySystem(), new Stub_paymentSystem(), "admin", "1234AsdF");
            }
            catch (Exception) { }
            userServiceLayer1 = serviceLayer.Connect();
            userServiceLayer2 = serviceLayer.Connect();
            try
            {
                userServiceLayer1.Register("addManagerUser", "1221Abcd");
                userServiceLayer2.Register("addManagerUser2", "87654Abcd");
            }
            catch (Exception) { }
            try
            {
                userServiceLayer2.Login("addManagerUser2", "87654Abcd");
                if (userServiceLayer1.Login("addManagerUser", "1221Abcd"))
                {
                    if (userServiceLayer1.Open_Store("mamma mia"))
                    {
                        userServiceLayer1.Add_Product_Store("mamma mia", "melon", "fruit", 5, 4, new noDiscount(), new regularPolicy());
                        userServiceLayer1.Add_Product_Store("mamma mia", "mint tea", "drink", 9, 100, new noDiscount(), new regularPolicy());
                        userServiceLayer1.Add_Product_Store("mamma mia", "potato", "food", 17, 50, new noDiscount(), new regularPolicy());
                    }
                }

            }
            catch (Exception) { }

        }

        [TestMethod()]
        public void Add_shop_manager_and_check_permissions_happyTest()
        {
            Assert.ThrowsException<Exception>(() => { userServiceLayer2.Add_Product_Store("mamma mia", "seven up", "drink", 7, 10, new noDiscount(), new regularPolicy()); }, "user2 still is not manager so he can't add products");
            Assert.ThrowsException<Exception>(() => { userServiceLayer2.Update_Product_Store("mamma mia", "potato", "chips potato", "food", 30, 5, new noDiscount(), new regularPolicy()); }, "user2 still is not manager so he can't update products");
            Assert.ThrowsException<Exception>(() => { userServiceLayer2.Remove_Product_Store("mamma mia", "potato1"); }, "user2 still is not manager so he can't remove products");

            Assert.IsTrue(userServiceLayer1.Add_Store_Manager("mamma mia", "addManagerUser2", true, false, false));

            List<ProductInStore> search1 = userServiceLayer2.GlobalSearch("seven up", null, null, -1, -1, -1, -1);
            int pre_amount = search1.Count;
            Assert.IsTrue(userServiceLayer2.Add_Product_Store("mamma mia", "seven up", "drink", 7, 10, new noDiscount(), new regularPolicy()));
            search1 = userServiceLayer2.GlobalSearch("seven up", null, null, -1, -1, -1, -1);
            Assert.AreEqual(pre_amount + 1, search1.Count);
            Assert.ThrowsException<Exception>(() => { userServiceLayer2.Update_Product_Store("mamma mia", "potato", "chips potato", "food", 30, 9, new noDiscount(), new regularPolicy()); }, "user2 has no update permissions so he can't update products");
            Assert.ThrowsException<Exception>(() => { userServiceLayer2.Remove_Product_Store("mamma mia", "potato"); }, "user2 has no remove permissions so he can't remove products");
            
           Assert.IsTrue(userServiceLayer1.Add_Store_Manager("mamma mia", "addManagerUser2", true, false, true));

            search1 = userServiceLayer2.GlobalSearch("potato", null, null, -1, -1, -1, -1);
            int pre_amount1 = search1.Count;
            List<ProductInStore> search2 = userServiceLayer2.GlobalSearch("chips potato", null, null, -1, -1, -1, -1);
            int pre_amount2 = search2.Count;
            Assert.IsTrue(userServiceLayer2.Update_Product_Store("mamma mia", "potato", "chips potato", "food", 30, 9, new noDiscount(), new regularPolicy()));
            search1 = userServiceLayer2.GlobalSearch("potato", null, null, -1, -1, -1, -1);
            Assert.AreEqual(pre_amount1 - 1, search1.Count);
            search2 = userServiceLayer2.GlobalSearch("chips potato", null, null, -1, -1, -1, -1);
            Assert.AreEqual(pre_amount2 + 1, search2.Count);
            Assert.AreEqual(search2[0].getStore().getName(), "mamma mia");
            for (int i = 0; i < search1.Count; i++)
            {
                if (search1[i].getStore().getName().Equals("shopipi"))
                    Assert.Fail("manager faild to update the product properly");
            }
            Assert.ThrowsException<Exception>(() => { userServiceLayer2.Remove_Product_Store("mamma mia", "potato"); }, "user2 has no remove permissions so he can't remove products");
            
            Assert.IsTrue(userServiceLayer1.Add_Store_Manager("mamma mia", "addManagerUser2", true, true, true));
            search2 = userServiceLayer2.GlobalSearch("chips potato", null, null, -1, -1, -1, -1);
            pre_amount2 = search2.Count;
            Assert.IsTrue(userServiceLayer2.Remove_Product_Store("mamma mia", "chips potato"));
            search2 = userServiceLayer2.GlobalSearch("chips potato", null, null, -1, -1, -1, -1);
            Assert.AreEqual(pre_amount2 -1, search2.Count);
            for (int i = 0; i < search2.Count; i++)
            {
                if (search2[i].getStore().getName().Equals("mamma mia"))
                    Assert.Fail("manager faild to remove the product properly");
            }
            
        }

        [TestMethod()]
        public void Add_shop_manager_that_not_exist_happyTest()
        {
            Assert.ThrowsException<Exception>(() => { userServiceLayer1.Add_Store_Manager("mamma mia", "userNotExist", true, false, true); }, "faild to add manager that not exist");
        }

        [TestMethod()]
        public void Add_the_same_manager_again_happyTest()
        {
            I_User_ServiceLayer userServiceLayer3 = serviceLayer.Connect();
            if (userServiceLayer3.Register("addManagerUser3", "12345Fgbh"))
            {
                userServiceLayer3.Login("addManagerUser3", "12345Fgbh");
                if (userServiceLayer1.Add_Store_Manager("mamma mia", "addManagerUser3", true, true, true))
                    Assert.IsTrue(userServiceLayer1.Add_Store_Manager("mamma mia", "addManagerUser3", true, true, true));
            }
        }

        [TestMethod()]
        public void Not_owner_try_to_add_manager_sadTest()
        {
            I_User_ServiceLayer tmp_userServiceLayer = serviceLayer.Connect();
            tmp_userServiceLayer.Register("addManagerTmpUser", "11111RTrt");

            I_User_ServiceLayer userServiceLayer4 = serviceLayer.Connect();
            userServiceLayer4.Register("addManagerUser4", "12345Ghgh");
            userServiceLayer4.Login("addManagerUser4", "12345Ghgh");


            Assert.ThrowsException<Exception>(() => { userServiceLayer4.Add_Store_Manager("mamma mia", "addManagerTmpUser", false, true, true); }, "only store owner can add new manager to the store");
        }

        [TestCleanup]
        public void TestClean()
        {
            userServiceLayer1.Logout();
            userServiceLayer2.Logout();
            userServiceLayer1 = null;
            userServiceLayer2 = null;
            serviceLayer.CleanUpSystem();
            serviceLayer = null;
        }
    }
}
