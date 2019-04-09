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
    public class RemoveManager_AT
    {
        private static I_ServiceLayer serviceLayer;
        private I_User_ServiceLayer userServiceLayer1;
        private I_User_ServiceLayer userServiceLayer2;

        [TestInitialize]
        public void init()
        {
            serviceLayer = new ServiceLayer();
            serviceLayer.Create_ServiceLayer(new stub_deliverySystem(), new stub_paymentSystem(), "admin", "1234Gthy");
            userServiceLayer2 = serviceLayer.Connect();
            if (userServiceLayer2.Register("removeManagerUser2", "87654TyTy"))
                userServiceLayer2.Login("removeManagerUser2", "87654TyTy");

            userServiceLayer1 = serviceLayer.Connect();
            if (userServiceLayer1.Register("removeManagerUser", "1221GhHg"))
                if (userServiceLayer1.Login("removeManagerUser", "1221GhHg"))
                    if (userServiceLayer1.Open_Store("shoes store"))
                    {
                        userServiceLayer1.Add_Product_Store("shoes store", "heel shoes", "women shoes", 280, 26, new noDiscount(), new regularPolicy());
                        userServiceLayer1.Add_Store_Manager("shoes store", "removeManagerUser2" , true, true, true);
                    }  
        }

        [TestMethod()]
        public void remove_shop_manager_tests()
        {
            Assert.IsTrue(userServiceLayer2.Add_Product_Store("shoes store", "blundstone", "men shoes", 700, 100, new noDiscount(), new regularPolicy()));
            Assert.IsTrue(userServiceLayer2.Update_Product_Store("shoes store", "blundstone", "blundstone", "shoes", 580, 90, new noDiscount(), new regularPolicy()));
            Assert.IsTrue(userServiceLayer2.Remove_Product_Store("shoes store", "blundstone"));

            Assert.IsTrue(userServiceLayer1.Remove_Store_Owner("shoes store", "removeManagerUser2"));

            Assert.ThrowsException<Exception>(() => { userServiceLayer2.Add_Product_Store("shoes store", "blundstone", "men shoes", 700, 100, new noDiscount(), new regularPolicy()); }, "removeManager_user2 is no longer a manager so he can't add product");
            Assert.ThrowsException<Exception>(() => { userServiceLayer2.Update_Product_Store("shoes store", "blundstone", "blundstone", "shoes", 580, 90, new noDiscount(), new regularPolicy()); }, "removeManager_user2 is no longer a manager so he can't update product");
            Assert.ThrowsException<Exception>(() => { userServiceLayer2.Remove_Product_Store("shoes store", "blundstone"); }, "removeManager_user2 is no longer a manager so he can't remove product");
        }

        [TestMethod()]
        public void remove_shop_manager_that_not_exist_test()
        {
            Assert.ThrowsException<Exception>(() => { userServiceLayer1.Remove_Store_Owner("shoes store", "userNotExist"); }, "faild to remove manager that not exist");
        }

        [TestMethod()]
        public void remove_shop_manager_that_is_not_manager_test()
        {
            I_User_ServiceLayer userServiceLayer3 = serviceLayer.Connect();
            userServiceLayer3.Register("removeManagerUser3", "9999Jhhj");

            Assert.ThrowsException<Exception>(() => { userServiceLayer1.Remove_Store_Owner("shoes store", "removeManagerUser3"); }, "faild to remove manager that was not defined to be an manager");
        }

        [TestMethod()]
        public void manager_try_to_remove_a_manager_test()  //bad
        {
            I_User_ServiceLayer tmp_userServiceLayer = serviceLayer.Connect();
            tmp_userServiceLayer.Register("removeManagerTmpUser", "11111FfgG");
            userServiceLayer1.Add_Store_Manager("shoes store", "removeManagerTmpUser", true, true, true);

            I_User_ServiceLayer userServiceLayer3 = serviceLayer.Connect();
            userServiceLayer3.Register("removeManagerUser3", "12345YhhY");
            userServiceLayer3.Login("removeManagerUser3", "12345YhhY");
            userServiceLayer1.Add_Store_Manager("shoes store", "removeManagerUser3", true, true, true);

            Assert.ThrowsException<Exception>(() => { tmp_userServiceLayer.Remove_Store_Manager("shoes store", "removeManagerUser3"); }, "only store owner can remove manager from the store");
            Assert.IsTrue(userServiceLayer3.Update_Product_Store("shoes store", "heel shoes", "heel shoes", "shoes", 280, 20, new noDiscount(), new regularPolicy()));

            Assert.IsTrue(userServiceLayer1.Remove_Store_Manager("shoes store", "removeManagerUser3"));
            Assert.ThrowsException<Exception>(() => { userServiceLayer2.Update_Product_Store("shoes store", "heel shoes", "heel shoes", "evening shoes", 550, 9, new noDiscount(), new regularPolicy()); }, "removeManager_user3 is no longer a manager");
        }

        [TestMethod()]
        public void regularUser_try_to_remove_a_manager_test()  //bad
        {
            I_User_ServiceLayer tmp_userServiceLayer = serviceLayer.Connect();
            tmp_userServiceLayer.Register("removeManagerTmpUser", "11111AssA");

            I_User_ServiceLayer userServiceLayer3 = serviceLayer.Connect();
            userServiceLayer3.Register("removeManagerUser3", "12345KkJj");
            userServiceLayer3.Login("removeManagerUser3", "12345KkJj");
            userServiceLayer1.Add_Store_Manager("shoes store", "removeManagerUser3", true, true, true);

            Assert.ThrowsException<Exception>(() => { tmp_userServiceLayer.Remove_Store_Manager("shoes store", "removeManagerUser3"); }, "only store owner can remove manager from the store");
            Assert.IsTrue(userServiceLayer3.Update_Product_Store("shoes store", "heel shoes", "heel shoes", "evening shoes", 280, 20, new noDiscount(), new regularPolicy()));

            Assert.IsTrue(userServiceLayer1.Remove_Store_Manager("shoes store", "removeManagerUser3"));
            Assert.ThrowsException<Exception>(() => { userServiceLayer2.Update_Product_Store("shoes store", "heel shoes", "heel shoes", "evening shoes", 550, 9, new noDiscount(), new regularPolicy()); }, "removeManager_user3 is no longer a manager");
        }
    }
}
