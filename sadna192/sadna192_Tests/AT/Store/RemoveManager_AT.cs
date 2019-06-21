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
    public class RemoveManager_AT
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
                serviceLayer.Create_ServiceLayer(new Stub_deliverySystem(), new Stub_paymentSystem(), "admin", "1234Gthy");
            }
            catch (Exception) { } 
            userServiceLayer2 = serviceLayer.Connect(new Stub_Alerter());
            userServiceLayer1 = serviceLayer.Connect(new Stub_Alerter());
            try
            {
                userServiceLayer2.Register("removeManagerUser2", "87654TyTy");
                userServiceLayer1.Register("removeManagerUser", "1221GhHg");
            }
            catch (Exception) { }
            try
            {
                userServiceLayer2.Login("removeManagerUser2", "87654TyTy");
                if (userServiceLayer1.Login("removeManagerUser", "1221GhHg"))
                    if (userServiceLayer1.Open_Store("shoes store"))
                    {
                        userServiceLayer1.Add_Product_Store("shoes store", "heel shoes", "womenshoes", 280, 26, new noDiscount(), new regularPolicy());
                        userServiceLayer1.Add_Store_Manager("shoes store", "removeManagerUser2", true, true, true);
                    }
            }
            catch (Exception) { } 
        }

        [TestMethod()]
        public void Remove_shop_manager_happyTest()
        {
            Assert.IsTrue(userServiceLayer2.Add_Product_Store("shoes store", "blundstone", "menshoes", 700, 100, new noDiscount(), new regularPolicy()));
            Assert.IsTrue(userServiceLayer2.Update_Product_Store("shoes store", "blundstone", "blundstone", "shoes", 580, 90, new noDiscount(), new regularPolicy()));
            Assert.IsTrue(userServiceLayer2.Remove_Product_Store("shoes store", "blundstone"));

            Assert.IsTrue(userServiceLayer1.Remove_Store_Manager("shoes store", "removeManagerUser2"));

            Assert.ThrowsException<Exception>(() => { userServiceLayer2.Add_Product_Store("shoes store", "blundstone", "menshoes", 700, 100, new noDiscount(), new regularPolicy()); }, "removeManager_user2 is no longer a manager so he can't add product");
            Assert.ThrowsException<Exception>(() => { userServiceLayer2.Update_Product_Store("shoes store", "blundstone", "blundstone", "shoes", 580, 90, new noDiscount(), new regularPolicy()); }, "removeManager_user2 is no longer a manager so he can't update product");
            Assert.ThrowsException<Exception>(() => { userServiceLayer2.Remove_Product_Store("shoes store", "blundstone"); }, "removeManager_user2 is no longer a manager so he can't remove product");
        }

        [TestMethod()]
        public void Remove_shop_manager_that_not_exist_happyTest()
        {
            Assert.ThrowsException<Exception>(() => { userServiceLayer1.Remove_Store_Owner("shoes store", "userNotExist"); }, "faild to remove manager that not exist");
        }

        [TestMethod()]
        public void Remove_shop_manager_that_is_not_manager_happyTest()
        {
            I_User_ServiceLayer userServiceLayer3 = serviceLayer.Connect(new Stub_Alerter());
            userServiceLayer3.Register("removeManagerUser3", "9999Jhhj");

            Assert.ThrowsException<Exception>(() => { userServiceLayer1.Remove_Store_Owner("shoes store", "removeManagerUser3"); }, "faild to remove manager that was not defined to be an manager");
        }

        [TestMethod()]
        public void Manager_try_to_remove_a_manager_happyTest()  
        {
            I_User_ServiceLayer tmp_userServiceLayer = serviceLayer.Connect(new Stub_Alerter());
            tmp_userServiceLayer.Register("removeManagerTmpUser", "11111FfgG");
            tmp_userServiceLayer.Login("removeManagerTmpUser", "11111FfgG");
            userServiceLayer1.Add_Store_Manager("shoes store", "removeManagerTmpUser", true, true, true);

            I_User_ServiceLayer userServiceLayer5 = serviceLayer.Connect(new Stub_Alerter());
            userServiceLayer5.Register("removeManagerUser5", "12345YhhY");
            userServiceLayer5.Login("removeManagerUser5", "12345YhhY");
            userServiceLayer1.Add_Store_Manager("shoes store", "removeManagerUser5", true, true, true);

            Assert.ThrowsException<Exception>(() => { tmp_userServiceLayer.Remove_Store_Manager("shoes store", "removeManagerUser5"); }, "only store owner can remove manager from the store");
            Assert.IsTrue(userServiceLayer5.Update_Product_Store("shoes store", "heel shoes", "heel shoes", "shoes", 280, 20, new noDiscount(), new regularPolicy()));

            Assert.IsTrue(userServiceLayer1.Remove_Store_Manager("shoes store", "removeManagerUser5"));
            Assert.ThrowsException<Exception>(() => { userServiceLayer5.Update_Product_Store("shoes store", "heel shoes", "heel shoes", "evening shoes", 550, 9, new noDiscount(), new regularPolicy()); }, "removeManager_user3 is no longer a manager");
        }

        [TestMethod()]
        public void Regular_user_try_to_remove_a_manager_sadTest() 
        {
            I_User_ServiceLayer tmp_userServiceLayer2 = serviceLayer.Connect(new Stub_Alerter());
            tmp_userServiceLayer2.Register("removeManagerTmpUser2", "11111AssA");

            I_User_ServiceLayer userServiceLayer4 = serviceLayer.Connect(new Stub_Alerter());
            userServiceLayer4.Register("removeManagerUser4", "12345KkJj");
            userServiceLayer4.Login("removeManagerUser4", "12345KkJj");
            userServiceLayer1.Add_Store_Manager("shoes store", "removeManagerUser4", true, true, true);

            Assert.ThrowsException<Exception>(() => { tmp_userServiceLayer2.Remove_Store_Manager("shoes store", "removeManagerUser4"); }, "only store owner can remove manager from the store");
            Assert.IsTrue(userServiceLayer4.Update_Product_Store("shoes store", "heel shoes", "heel shoes", "eveningshoes", 280, 20, new noDiscount(), new regularPolicy()));
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
