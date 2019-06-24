using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using static sadna192_Tests.Stubs;

namespace sadna192.Tests.AcceptanceTests
{
    [TestClass()]
    public class AddOwner_AT
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
                serviceLayer.Create_ServiceLayer(new Stub_deliverySystem(), new Stub_paymentSystem(), "admin", "1234FgHY");
            }
            catch (Exception) { }
            userServiceLayer1 = serviceLayer.Connect(new Stub_Alerter());
            userServiceLayer2 = serviceLayer.Connect(new Stub_Alerter());
            try
            {
                userServiceLayer1.Register("addOwneruser", "1221Abcd");
                userServiceLayer2.Register("addOwneruser2", "87654Abcd");
            }
            catch (Exception) { }
            try
            {
                userServiceLayer2.Login("addOwneruser2", "87654Abcd");
                if (userServiceLayer1.Login("addOwneruser", "1221Abcd"))
                {
                    if (userServiceLayer1.Open_Store("shopipi"))
                    {
                        userServiceLayer1.Add_Product_Store("shopipi", "cake", "food", 25, 4, new noDiscount(), new regularPolicy());
                        userServiceLayer1.Add_Product_Store("shopipi", "water", "drink", 9, 100, new noDiscount(), new regularPolicy());
                        userServiceLayer1.Add_Product_Store("shopipi", "chips", "food", 17, 50, new noDiscount(), new regularPolicy());
                    }
                }
            }
            catch (Exception) { }
        }

        [TestMethod()]
        public void AddShopOwner_happyTest()
        {         
            Assert.IsTrue(userServiceLayer1.Add_Store_Owner("shopipi", "addOwneruser2"));
        }

        [TestMethod()]
        public void AddShopOwnerThatNotExist_happyTest()
        {
            Assert.ThrowsException<Sadna192Exception>(() => { userServiceLayer1.Add_Store_Owner("shopipi", "userNotExist"); }, "faild to add owner that not exist");
        }

        [TestMethod()]
        public void AddTheSameOwnerAgain_happyTest()
        {
            I_User_ServiceLayer userServiceLayer3 = serviceLayer.Connect(new Stub_Alerter());
            userServiceLayer3.Register("addOwneruser3", "9999Abcd");

            Assert.IsTrue(userServiceLayer1.Add_Store_Owner("shopipi", "addOwneruser3"));
            Assert.ThrowsException<Sadna192Exception>(() => { userServiceLayer1.Add_Store_Owner("shopipi", "addOwneruser3"); }, "this user is already owner");

        }

        [TestMethod()]
        public void NotOwnerTryToAddOwner_sadTest()
        {
            I_User_ServiceLayer tmp_userServiceLayer = serviceLayer.Connect(new Stub_Alerter());
           tmp_userServiceLayer.Register("addOwnertmpuser", "11111Abcd");

            I_User_ServiceLayer userServiceLayer4 = serviceLayer.Connect(new Stub_Alerter());
            if (userServiceLayer4.Register("addOwneruser4", "12345Abcd"))
                userServiceLayer4.Login("addOwneruser4", "12345Abcd");

            Assert.ThrowsException<Sadna192Exception>(() => { userServiceLayer4.Add_Store_Owner("shopipi", "addOwner_tmp_user");}, "only store owner can add new owner to the store");
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
