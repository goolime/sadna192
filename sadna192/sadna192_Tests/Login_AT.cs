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
    public class Login_AT
    {
        private static I_ServiceLayer serviceLayer;
        private I_User_ServiceLayer userServiceLayer1;
        private I_User_ServiceLayer userServiceLayer2;

        [TestInitialize]
        public void init()
        {
            serviceLayer = new ServiceLayer();
            serviceLayer.Create_ServiceLayer(new stub_deliverySystem(), new stub_paymentSystem(), "admin", "1234");
            userServiceLayer1 = serviceLayer.Connect();
            userServiceLayer1.Register("login_user", "1221");

            userServiceLayer2 = serviceLayer.Connect();
            userServiceLayer2.Register("login_user2", "9876");
        }

        [TestMethod()]
        public void Login_happy_test()
        {
            Assert.IsTrue(userServiceLayer1.Login("login_user", "1221"));
            Assert.IsTrue(userServiceLayer1.Open_Store("login_user store"));
            Assert.IsTrue(userServiceLayer1.Logout());

        }

        [TestMethod()]
        public void Login_bad1_test()
        {
            Assert.ThrowsException<Exception>(() => { userServiceLayer2.Login("user34", "69375"); }, "this user is not exist in the system");
            Assert.ThrowsException<Exception>(() => { userServiceLayer2.Login("", "9876"); }, "user name is mandatory field");
            Assert.ThrowsException<Exception>(() => { userServiceLayer2.Login("login_user2", ""); }, "password is mandatory field");
        }

        [TestMethod()]
        public void Login_sad_test()
        {
            if (userServiceLayer1.Login("login_user", "1221"))
                Assert.ThrowsException<Exception>(() => { userServiceLayer1.Login("login_user2", "9876"); }, "user cannot perform login when he is already logedin");
        }
    }
}