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
    public class OpenStore_AT
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
            if (userServiceLayer1.Register("openStore_user", "1221"))
                bool login1 = userServiceLayer1.Login("openStore_user", "1221");
            userServiceLayer2 = serviceLayer.connect();
            if (userServiceLayer2.Register("openStore_user2", "1221"))
                bool login1 = userServiceLayer2.Login("openStore_user2", "1221");

        }

        [TestMethod()]
        public void openStore_happy_tests()
        {
            Assert.IsTrue(userServiceLayer1.Open_Store("marketim"));   //happy 1
            Assert.ThrowsException<Exception>(() => { userServiceLayer2.Open_Store("marketim"); }, "This store name is already taken");  //happy 2
        }

        [TestMethod()]
        public void openStore_visitor_state_tests()  //bad
        {
            I_User_ServiceLayer tmp_userServiceLayer = serviceLayer.connect();
            Assert.IsFalse(tmp_userServiceLayer.Open_Store("dig dig dog"));
        }

    }
}