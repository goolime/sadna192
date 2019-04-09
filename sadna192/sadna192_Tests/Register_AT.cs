using Microsoft.VisualStudio.TestTools.UnitTesting;
using sadna192.Tests;
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
    public class Register_AT
    {
        private static I_ServiceLayer serviceLayer ;
        private I_User_ServiceLayer userServiceLayer1 ;
        private I_User_ServiceLayer userServiceLayer2;

        [TestInitialize]
        public void init()
        {
            serviceLayer = new ServiceLayer();
            serviceLayer.Create_ServiceLayer(new stub_deliverySystem(), new stub_paymentSystem(), "admin", "123456Ui");
            userServiceLayer1 = serviceLayer.Connect();
            bool user = userServiceLayer1.Register("registerUser", "1221zxcV");

        }

        [TestMethod()]
        public void Register_happy_test()
        {
            I_User_ServiceLayer userServiceLayer2 = serviceLayer.Connect();
            Assert.IsFalse(userServiceLayer2.Login("bob", "9876ASdf"));      
            Assert.IsTrue(userServiceLayer2.Register("bob", "9876ASdf"));
            Assert.IsTrue(userServiceLayer2.Login("bob", "9876ASdf"));
        }

        [TestMethod()]
        public void Register_bad1_test()
        {
            I_User_ServiceLayer userServiceLayer2 = serviceLayer.Connect();
            Assert.ThrowsException<Exception>(() => { userServiceLayer2.Register("registerUser", "69375LOpo"); }, "this user name is not avilable");

        }

        [TestMethod()]
        public void Register_bad2_test()
        {
            I_User_ServiceLayer userServiceLayer = serviceLayer.Connect();
            Assert.ThrowsException<Exception>(() => { userServiceLayer.Register("", "3456Sdfg"); }, "user name is mandatory field");
            Assert.ThrowsException<Exception>(() => { userServiceLayer.Register("Alice", ""); }, "password is mandatory field");
        }

        [TestMethod()]
        public void Register_sad_test()
        {
            if (userServiceLayer1.Login("registerUser", "1221zxcV"))
                Assert.ThrowsException<Exception>(() => { userServiceLayer1.Register("Alice", "3456Sdfg"); }, "trying to register when already logedin");
        }

    }
}