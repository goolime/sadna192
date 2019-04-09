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
    public class Register_AT
    {
        private static I_ServiceLayer serviceLayer ;
        private I_User_ServiceLayer userServiceLayer1 ;
        private I_DeliverySystem deliverySystem ;
        private I_PaymentSystem paymentSystem ;

        [TestInitialize]
        public void init()
        {
            serviceLayer = I_ServiceLayer.Create_ServiceLayer(deliverySystem, paymentSystem, "admin", "1234");
            userServiceLayer1 = serviceLayer.connect();
            bool user = userServiceLayer1.Register("register_user", "1221");

            userServiceLayer2 = serviceLayer.connect();
        }

        [TestMethod()]
        public void Register_happy_test()
        {
            I_User_ServiceLayer userServiceLayer2 = serviceLayer.connect();
            Assert.IsFalse(userServiceLayer2.Login("bob", "9876"));      
            Assert.IsTrue(userServiceLayer2.Register("bob", "9876"));
            Assert.IsTrue(userServiceLayer2.Login("bob", "9876"));
        }

        [TestMethod()]
        public void Register_bad1_test()
        {
            I_User_ServiceLayer userServiceLayer2 = serviceLayer.connect();
            Assert.ThrowsException<Exception>(() => { userServiceLayer2.Register("register_user", "69375"); }, "this user name is not avilable");

        }

        [TestMethod()]
        public void Register_bad2_test()
        {
            I_User_ServiceLayer userServiceLayer = serviceLayer.connect();
            Assert.ThrowsException<Exception>(() => { userServiceLayer.Register("", "3456"); }, "user name is mandatory field");
            Assert.ThrowsException<Exception>(() => { userServiceLayer.Register("Alice", ""); }, "password is mandatory field");
        }

        [TestMethod()]
        public void Register_sad_test()
        {
            if (userServiceLayer1.Login("register_user", "1221"))
                Assert.ThrowsException<Exception>(() => { userServiceLayer1.Register("Alice", "69375"); }, "trying to register when already logedin");
        }

    }
}