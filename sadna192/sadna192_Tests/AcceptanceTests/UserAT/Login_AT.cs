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
    public class Login_AT
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
            bool user = userServiceLayer1.Register("login_user", "1221");

            userServiceLayer2 = serviceLayer.connect();
            bool user2 = userServiceLayer2.Register("login_user2", "9876");
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
                Assert.ThrowsException<Exception>(() => { userServiceLayer2.Login("login_user2", "9876"); }, "user cannot perform login when he is already logedin");
        }
    }
}