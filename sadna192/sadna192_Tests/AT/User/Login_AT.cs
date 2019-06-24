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
    public class Login_AT
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
                serviceLayer.Create_ServiceLayer(new Stub_deliverySystem(), new Stub_paymentSystem(), "admin", "1234Abcd");
            }
            catch (Exception) { }
            userServiceLayer1 = serviceLayer.Connect(new Stub_Alerter());
            userServiceLayer2 = serviceLayer.Connect(new Stub_Alerter());
            try
            {
                userServiceLayer1.Register("loginuser", "1221Abcd");
                userServiceLayer2.Register("loginuser2", "9876Abcd");
            }
            catch (Exception) { }
        }

        [TestMethod()]
        public void Login_happyTest()
        {
            Assert.IsTrue(userServiceLayer1.Login("loginuser", "1221Abcd"));
            userServiceLayer1.Logout();
        }

        [TestMethod()]
        public void Login_sadTest()
        {
            Assert.ThrowsException<Exception>(() => { userServiceLayer2.Login("user34", "69375Abcd"); }, "this user is not exist in the system");
            Assert.ThrowsException<Exception>(() => { userServiceLayer2.Login("", "9876"); }, "user name is mandatory field");
            Assert.ThrowsException<Exception>(() => { userServiceLayer2.Login("login_user2", ""); }, "password is mandatory field");
        }

        [TestMethod()]
        public void Login_badTest()
        {
            if (userServiceLayer1.Login("loginuser", "1221Abcd"))
                Assert.ThrowsException<Exception>(() => { userServiceLayer1.Login("login_user2", "9876"); }, "user cannot perform login when he is already logedin");
        }

        [TestCleanup]
        public void CleanUp()
        {
            serviceLayer.CleanUpSystem();
            serviceLayer = null;
        }
    }
}