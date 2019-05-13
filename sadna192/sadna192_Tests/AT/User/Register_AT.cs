using Microsoft.VisualStudio.TestTools.UnitTesting;
using sadna192.Tests;
using System;
using static sadna192_Tests.Stubs;

namespace sadna192.Tests.AcceptanceTests
{
    [TestClass()]
    public class Register_AT
    {
        private static I_ServiceLayer serviceLayer ;
        private I_User_ServiceLayer userServiceLayer1 ;

        [TestInitialize]
        public void Init()
        {
            serviceLayer = new ServiceLayer();
            try
            {
                serviceLayer.Create_ServiceLayer(new Stub_deliverySystem(), new Stub_paymentSystem(), "admin", "123456Ui");
            }
            catch (Exception) { }
            userServiceLayer1 = serviceLayer.Connect();
            try
            {
                userServiceLayer1.Register("registerUser", "1221zxcV");
            }
            catch (Exception) { }
        }

        [TestMethod()]
        public void Register_happyTest()
        {
            I_User_ServiceLayer userServiceLayer2 = serviceLayer.Connect();
            Assert.ThrowsException<Exception>(() => { userServiceLayer2.Login("bobi", "9876ASdf"); }, "try to log in with user name that not exist");
            Assert.IsTrue(userServiceLayer2.Register("bobi", "9876ASdf"));
            Assert.IsTrue(userServiceLayer2.Login("bobi", "9876ASdf"));
        }

        [TestMethod()]
        public void Register_sadTest1()
        {
            I_User_ServiceLayer userServiceLayer2 = serviceLayer.Connect();
            Assert.ThrowsException<Exception>(() => { userServiceLayer2.Register("registerUser", "69375LOpo"); }, "this user name is not avilable");

        }

        [TestMethod()]
        public void Register_sadTest2()
        {
            I_User_ServiceLayer userServiceLayer = serviceLayer.Connect();
            Assert.ThrowsException<Exception>(() => { userServiceLayer.Register("", "3456Sdfg"); }, "user name is mandatory field");
            Assert.ThrowsException<Exception>(() => { userServiceLayer.Register("Alice", ""); }, "password is mandatory field");
        }

        [TestMethod()]
        public void Register_badTest()
        {
            if (userServiceLayer1.Login("registerUser", "1221zxcV"))
                Assert.ThrowsException<Exception>(() => { userServiceLayer1.Register("Alice", "3456Sdfg"); }, "trying to register when already logedin");
        }
    }
}