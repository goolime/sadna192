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
    public class RemoveOwner_AT
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
                serviceLayer.Create_ServiceLayer(new Stub_deliverySystem(), new Stub_paymentSystem(), "admin", "1234WeeR");
            }
            catch (Exception) { }
            userServiceLayer2 = serviceLayer.Connect(new Stub_Alerter());
            userServiceLayer1 = serviceLayer.Connect(new Stub_Alerter());

            try
            {
                userServiceLayer2.Register("removeOwnerUser2", "87654GfdF");
                userServiceLayer1.Register("removeOwnerUser", "1221YhnN");
            }
            catch (Exception) { }
            try
            {
                userServiceLayer2.Login("removeOwnerUser2", "87654GfdF");
                if (userServiceLayer1.Login("removeOwnerUser", "1221YhnN"))
                    if (userServiceLayer1.Open_Store("shopit"))
                    {
                        userServiceLayer1.Add_Product_Store("shopit", "cheese cake", "food", 28, 6, new noDiscount(), new regularPolicy());
                        userServiceLayer1.Add_Store_Owner("shopit", "removeOwnerUser2");
                    }
            }
            catch (Exception) { }
        }

        [TestMethod()]
        public void Remove_shop_owner_happyTests()
        {
            Assert.IsTrue(userServiceLayer2.Add_Product_Store("shopit", "cola", "drink", 7, 10, new noDiscount(), new regularPolicy()));
            Assert.IsTrue(userServiceLayer2.Update_Product_Store("shopit", "cola", "coca cola", "drink", 6.8, 19, new noDiscount(), new regularPolicy())); 
            Assert.IsTrue(userServiceLayer2.Remove_Product_Store("shopit", "coca cola")); 

            Assert.IsTrue(userServiceLayer1.Remove_Store_Owner("shopit", "removeOwnerUser2"));

            Assert.ThrowsException<Exception>(() => { userServiceLayer2.Add_Product_Store("shopit", "cola", "drink", 7, 10, new noDiscount(), new regularPolicy()); }, "removeOwner_user2 is no longer an owner so he can't add product");
            Assert.ThrowsException<Exception>(() => { userServiceLayer2.Update_Product_Store("shopit", "cheese cake", "cake", "food", 28, 9, new noDiscount(), new regularPolicy()); }, "removeOwner_user2 is no longer an owner so he can't update product");
            Assert.ThrowsException<Exception>(() => { userServiceLayer2.Remove_Product_Store("shopit", "cheese cake"); }, "removeOwner_user2 is no longer an owner so he can't remove product");
        }

        [TestMethod()]
        public void Remove_shop_owner_that_not_exist_happyTest()
        {
            Assert.ThrowsException<Exception>(() => { userServiceLayer1.Remove_Store_Owner("shopit", "userNotExist"); }, "faild to remove owner that not exist");
        }

        [TestMethod()]
        public void Remove_shop_owner_that_is_not_owner_happyTest()
        {
            I_User_ServiceLayer userServiceLayer31 = serviceLayer.Connect(new Stub_Alerter());
            bool reg = userServiceLayer31.Register("removeOwnerUser31", "9999GThy");          
            Assert.ThrowsException<Exception>(() => { userServiceLayer1.Remove_Store_Owner("shopit", "removeOwnerUser31"); }, "faild to remove owner that was not defined to be an owner");
        }

        [TestMethod()]
        public void Remove_owner_that_defined_by_someonelse_happyTest()
        {
            I_User_ServiceLayer userServiceLayer4 = serviceLayer.Connect(new Stub_Alerter());
            userServiceLayer4.Register("removeOwnerUser4", "9999GThy");
            userServiceLayer4.Login("removeOwnerUser4", "9999GThy");

            I_User_ServiceLayer userServiceLayer5 = serviceLayer.Connect(new Stub_Alerter());
            userServiceLayer5.Register("removeOwnerUser5", "345HDty6s");
            userServiceLayer5.Login("removeOwnerUser5", "345HDty6s");

            userServiceLayer1.Add_Store_Owner("shopit", "removeOwnerUser5");
            userServiceLayer1.Add_Store_Owner("shopit", "removeOwnerUser4");

            Assert.ThrowsException<Exception>(() => { userServiceLayer4.Remove_Store_Manager("shopit", "removeOwnerUser5"); }, "faild to remove owner that someonelse defined him to be an owner");

            I_User_ServiceLayer userServiceLayer6 = serviceLayer.Connect(new Stub_Alerter());
            userServiceLayer6.Register("removeOwnerUser6", "8765GFDf");
            userServiceLayer6.Login("removeOwnerUser6", "8765GFDf");
            userServiceLayer4.Add_Store_Owner("shopit", "removeOwnerUser6");

            Assert.ThrowsException<Exception>(() => { userServiceLayer5.Remove_Store_Manager("shopit", "removeOwnerUser6"); }, "faild to remove owner that someonelse defined him to be an owner");
            Assert.IsTrue(userServiceLayer6.Add_Product_Store("shopit", "chips", "food", 17, 50, new noDiscount(), new regularPolicy())); 
            Assert.IsTrue(userServiceLayer4.Remove_Store_Owner("shopit", "removeOwnerUser6"));
            Assert.ThrowsException<Exception>(() => { userServiceLayer6.Remove_Product_Store("shopit", "cheese cake"); }, "removeOwner_user5 is no longer an owner so he can't remove product");
        }

        [TestMethod()]
        public void Remove_owner_remove_all_the_owners_that_defined_by_him_happyTest()
        {
            I_User_ServiceLayer userServiceLayer3 = serviceLayer.Connect(new Stub_Alerter());
            userServiceLayer3.Register("removeOwnerUser3", "9999GThy");
            userServiceLayer3.Login("removeOwnerUser3", "9999GThy");

            I_User_ServiceLayer userServiceLayer7 = serviceLayer.Connect(new Stub_Alerter());
            userServiceLayer7.Register("removeOwnerUser7", "345HDty6s");
            userServiceLayer7.Login("removeOwnerUser7", "345HDty6s");

            userServiceLayer1.Add_Store_Owner("shopit", "removeOwnerUser3");
            userServiceLayer3.Add_Store_Owner("shopit", "removeOwnerUser7");

            Assert.IsTrue(userServiceLayer7.Update_Product_Store("shopit", "cheese cake", "cheese cake", "desserts", 22, 26, new noDiscount(), new regularPolicy()));

            Assert.IsTrue(userServiceLayer1.Remove_Store_Owner("shopit", "removeOwnerUser3"));
            Assert.ThrowsException<Exception>(() => { userServiceLayer7.Update_Product_Store("shopit", "cheese cake", "cheese cake", "cakes", 22, 26, new noDiscount(), new regularPolicy()); }, "no longer owner because his definder has been removed and is no longer owner ");
            Assert.ThrowsException<Exception>(() => { userServiceLayer3.Update_Product_Store("shopit", "cheese cake", "cheese cake", "cakes", 22, 26, new noDiscount(), new regularPolicy()); }, "no longer owner");
        }


        [TestMethod()]
        public void Not_owner_try_to_remove_an_owner_sadTest() 
        {
            I_User_ServiceLayer tmp_userServiceLayer = serviceLayer.Connect(new Stub_Alerter());
            tmp_userServiceLayer.Register("removeOwnerTmpUser", "11111DerJ");

            I_User_ServiceLayer userServiceLayer8 = serviceLayer.Connect(new Stub_Alerter());
            userServiceLayer8.Register("removeOwnerUser8", "12345JGth");
            userServiceLayer8.Login("removeOwnerUser8", "12345JGth");
            userServiceLayer1.Add_Store_Owner("shopit", "removeOwnerUser8");

            Assert.ThrowsException<Exception>(() => { tmp_userServiceLayer.Remove_Store_Manager("shopit", "removeOwnerUser8"); }, "only store owner can remove an owner from the store");
            Assert.IsTrue(userServiceLayer8.Update_Product_Store("shopit", "cheese cake", "cheese cake", "cakes", 22, 26, new noDiscount(), new regularPolicy()));
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
