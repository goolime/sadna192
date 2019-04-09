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
    public class OpenStore_AT
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
            if (userServiceLayer1.Register("openStore_user", "1221"))
                userServiceLayer1.Login("openStore_user", "1221");

            userServiceLayer2 = serviceLayer.Connect();
            if (userServiceLayer2.Register("openStore_user2", "1221"))
                userServiceLayer2.Login("openStore_user2", "1221");

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
            I_User_ServiceLayer tmp_userServiceLayer = serviceLayer.Connect();
            Assert.ThrowsException<Exception>(() => { tmp_userServiceLayer.Open_Store("dig dig dog"); }, "only registered user can open stores");  //happy 2
        }

    }
}