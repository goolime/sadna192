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
    public class RemoveManager_AT
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

            userServiceLayer2 = serviceLayer.connect();
            if (userServiceLayer2.Register("removeManager_user2", "87654"))
                bool login1 = userServiceLayer2.Login("removeManager_user2", "87654");

            userServiceLayer1 = serviceLayer.connect();
            if (userServiceLayer1.Register("removeManager_user", "1221"))
                if (userServiceLayer1.Login("removeManager_user", "1221"))
                    if (userServiceLayer1.Open_Store("shoes store"))
                    {
                        bool product = userServiceLayer1.Add_Product_Store("shoes store", "heel shoes", "women shoes", 280, 26, null, null);
                        bool owner = userServiceLayer1.Add_Store_Manager("shoes store", "removeManager_user2" , true, true, true);
                    }  
        }

        [TestMethod()]
        public void remove_shop_manager_tests()
        {
            Assert.IsTrue(userServiceLayer2.Add_Product_Store("shoes store", "blundstone", "men shoes", 700, 100, null, null));
            Assert.IsTrue(userServiceLayer2.Update_Product_Store("shoes store", "blundstone", "blundstone", "shoes", 580, 90, null, null));
            Assert.IsTrue(userServiceLayer2.Remove_Product_Store("shoes store", "blundstone"));

            Assert.IsTrue(userServiceLayer1.Remove_Store_Owner("shoes store", "removeManager_user2"));

            Assert.ThrowsException<Exception>(() => { userServiceLayer2.Add_Product_Store("shoes store", "blundstone", "men shoes", 700, 100, null, null); }, "removeManager_user2 is no longer a manager so he can't add product");
            Assert.ThrowsException<Exception>(() => { userServiceLayer2.Update_Product_Store("shoes store", "blundstone", "blundstone", "shoes", 580, 90, null, null); }, "removeManager_user2 is no longer a manager so he can't update product");
            Assert.ThrowsException<Exception>(() => { userServiceLayer2.Remove_Product_Store("shoes store", "blundstone"); }, "removeManager_user2 is no longer a manager so he can't remove product");
        }

        [TestMethod()]
        public void remove_shop_manager_that_not_exist_test()
        {
            Assert.ThrowsException<Exception>(() => { userServiceLayer1.Remove_Store_Owner("shoes store", "user_not_exist"); }, "faild to remove manager that not exist");
        }

        [TestMethod()]
        public void remove_shop_manager_that_is_not_manager_test()
        {
            I_User_ServiceLayer userServiceLayer3 = serviceLayer.connect();
            bool reg = userServiceLayer3.Register("removeManager_user3", "9999");

            Assert.ThrowsException<Exception>(() => { userServiceLayer1.Remove_Store_Owner("shoes store", "removeManager_user3"); }, "faild to remove manager that was not defined to be an manager");
        }

        [TestMethod()]
        public void manager_try_to_remove_a_manager_test()  //bad
        {
            I_User_ServiceLayer tmp_userServiceLayer = serviceLayer.connect();
            bool ans = tmp_userServiceLayer.Register("removeManager_tmp_user", "11111");
            ans = userServiceLayer1.Add_Store_Manager("shoes store", "removeManager_tmp_user", true, true, true);

            I_User_ServiceLayer userServiceLayer3 = serviceLayer.connect();
            ans = userServiceLayer3.Register("removeManager_user3", "12345");
            ans = userServiceLayer3.Login("removeManager_user3", "12345");
            ans = userServiceLayer1.Add_Store_Manager("shoes store", "removeManager_user3", true, true, true);

            Assert.ThrowsException<Exception>(() => { tmp_userServiceLayer.Remove_Store_Manager("shoes store", "removeManager_user3"); }, "only store owner can remove manager from the store");
            Assert.IsTrue(userServiceLayer3.Update_Product_Store("shoes store", "heel shoes", "heel shoes", "shoes", 280, 20, null, null));

            Assert.IsTrue(userServiceLayer1.Remove_Store_Manager("shoes store", "removeManager_user3"));
            Assert.ThrowsException<Exception>(() => { userServiceLayer2.Update_Product_Store("shoes store", "heel shoes", "heel shoes", "evening shoes", 550, 9, null, null); }, "removeManager_user3 is no longer a manager");
        }

        [TestMethod()]
        public void regularUser_try_to_remove_a_manager_test()  //bad
        {
            I_User_ServiceLayer tmp_userServiceLayer = serviceLayer.connect();
            bool ans = tmp_userServiceLayer.Register("removeManager_tmp_user", "11111");

            I_User_ServiceLayer userServiceLayer3 = serviceLayer.connect();
            ans = userServiceLayer3.Register("removeManager_user3", "12345");
            ans = userServiceLayer3.Login("removeManager_user3", "12345");
            ans = userServiceLayer1.Add_Store_Manager("shoes store", "removeManager_user3", true, true, true);

            Assert.ThrowsException<Exception>(() => { tmp_userServiceLayer.Remove_Store_Manager("shoes store", "removeManager_user3"); }, "only store owner can remove manager from the store");
            Assert.IsTrue(userServiceLayer3.Update_Product_Store("shoes store", "heel shoes", "heel shoes", "evening shoes", 280, 20, null, null));

            Assert.IsTrue(userServiceLayer1.Remove_Store_Manager("shoes store", "removeManager_user3"));
            Assert.ThrowsException<Exception>(() => { userServiceLayer2.Update_Product_Store("shoes store", "heel shoes", "heel shoes", "evening shoes", 550, 9, null, null); }, "removeManager_user3 is no longer a manager");
        }
    }
}
