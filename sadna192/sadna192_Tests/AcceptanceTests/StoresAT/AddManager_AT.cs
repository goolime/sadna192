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
    public class AddManager_AT
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
            if (userServiceLayer1.Register("addManager_user", "1221"))
                if (userServiceLayer1.Login("addManager_user", "1221"))
                {
                    if (userServiceLayer1.Open_Store("mamma mia"))
                    {
                        bool product = userServiceLayer1.Add_Product_Store("mamma mia", "melon", "fruit", 5, 4, null, null);
                        product = userServiceLayer1.Add_Product_Store("mamma mia", "mint tea", "drink", 9, 100, null, null);
                        product = userServiceLayer1.Add_Product_Store("mamma mia", "potato", "food", 17, 50, null, null);
                    }
                }
            userServiceLayer2 = serviceLayer.connect();
            if (userServiceLayer2.Register("addManager_user2", "87654"))
                bool login1 = userServiceLayer2.Login("addManager_user2", "87654");


        }

        [TestMethod()]
        public void add_shop_manager_and_check_permissions_test()
        {
            Assert.ThrowsException<Exception>(() => { userServiceLayer2.Add_Product_Store("mamma mia", "seven up", "drink", 7, 10, null, null); }, "user2 still is not manager so he can't add products");
            Assert.ThrowsException<Exception>(() => { userServiceLayer2.Update_Product_Store("mamma mia", "potato", " potato", "vegetable", 30, 9, null, null); }, "user2 still is not manager so he can't update products");
            Assert.ThrowsException<Exception>(() => { userServiceLayer2.Remove_Product_Store("mamma mia", "potato"); }, "user2 still is not manager so he can't remove products");

            Assert.IsTrue(userServiceLayer1.Add_Store_Manager("mamma mia", "addManager_user2", true, false, false));

            List<ProductInStore> search1 = userServiceLayer2.GlobalSearch("seven up", "drink", null, 0, 100, 0, 0);
            int pre_amount = search1.Count;
            Assert.IsTrue(userServiceLayer2.Add_Product_Store("mamma mia", "seven up", "drink", 7, 10, null, null));
            search1 = userServiceLayer2.GlobalSearch("seven up", "drink", null, 0, 100, 0, 0);
            Assert.AreEquals(pre_amount + 1, search1.Count);
            Assert.ThrowsException<Exception>(() => { userServiceLayer2.Update_Product_Store("mamma mia", "potato", " potato", "vegetable", 30, 9, null, null); }, "user2 has no update permissions so he can't update products");
            Assert.ThrowsException<Exception>(() => { userServiceLayer2.Remove_Product_Store("mamma mia", "potato"); }, "user2 has no remove permissions so he can't remove products");

            Assert.IsTrue(userServiceLayer1.Add_Store_Manager("mamma mia", "addManager_user2", true, true, false));

            search1 = userServiceLayer2.GlobalSearch("potato", "food", null, 0, 100, 0, 0);
            int pre_amount1 = search1.Count;
            List<ProductInStore> search2 = userServiceLayer2.GlobalSearch("potato", "vegetable", null, 0, 100, 0, 0);
            int pre_amount2 = search2.Count;
            Assert.IsTrue(userServiceLayer2.Update_Product_Store("mamma mia", "potato", " potato", "vegetable", 30, 9, null, null));
            search1 = userServiceLayer2.GlobalSearch("potato", "food", null, 0, 100, 0, 0);
            Assert.AreEquals(pre_amount1 - 1, search1.Count);
            search2 = userServiceLayer2.GlobalSearch("potato", "vegetable", null, 0, 100, 0, 0);
            Assert.AreEquals(pre_amount2 + 1, search2.Count);
            Assert.AreEquals(search2[0].getStoreName(), "mamma mia");
            for (int i = 0; i < search1.Count; i++)
            {
                if (search1[i].getStoreName().Equals("shopipi"))
                    Assert.Fail("manager faild to update the product properly");
            }
            Assert.ThrowsException<Exception>(() => { userServiceLayer2.Remove_Product_Store("mamma mia", "potato"); }, "user2 has no remove permissions so he can't remove products");

            Assert.IsTrue(userServiceLayer1.Add_Store_Manager("mamma mia", "addManager_user2", true, true, true));

            Assert.IsTrue(userServiceLayer2.Remove_Product_Store("mamma mia", "potato"));
            search2 = userServiceLayer2.GlobalSearch("potato", "vegetable", null, 0, 100, 0, 0);
            Assert.AreEquals(pre_amount2 -1, search2.Count);
            for (int i = 0; i < search2.Count; i++)
            {
                if (search2[i].getStoreName().Equals("mamma mia"))
                    Assert.Fail("manager faild to remove the product properly");
            }
        }

        [TestMethod()]
        public void add_shop_manager_that_not_exist_test()
        {
            Assert.ThrowsException<Exception>(() => { userServiceLayer1.Add_Store_Manager("mamma mia", "user_not_exist", true, false, true); }, "faild to add manager that not exist");
        }

        [TestMethod()]
        public void add_the_same_manager_again_test()
        {
            I_User_ServiceLayer userServiceLayer3 = serviceLayer.connect();
            if (userServiceLayer3.Register("addManager_user3", "12345"))
            {
                bool log = userServiceLayer3.Login("addManager_user3", "12345");
                if(userServiceLayer1.Add_Store_Manager("mamma mia", "addManager_user3", true, true , true))
                    Assert.ThrowsException<Exception>(() => { userServiceLayer1.Add_Store_Manager("mamma mia", "addManager_user3", true, false, true); }, "the manager is already manager");
            }
        }

        [TestMethod()]
        public void not_owner_try_to_add_manager_test()  //bad
        {
            I_User_ServiceLayer tmp_userServiceLayer = serviceLayer.connect();
            bool reg = tmp_userServiceLayer.Register("addManager_tmp_user", "11111");

            I_User_ServiceLayer userServiceLayer3 = serviceLayer.connect();
            if (userServiceLayer3.Register("addManager_user3", "12345"))
                bool log = userServiceLayer3.Login("addManager_user3", "12345");

            Assert.ThrowsException<Exception>(() => { userServiceLayer3.Add_Store_Manager("mamma mia", "addManager_tmp_user", false, true, true); }, "only store owner can add new manager to the store");
        }
    }
}
