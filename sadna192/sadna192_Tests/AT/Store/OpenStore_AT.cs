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
    public class OpenStore_AT
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
                serviceLayer.Create_ServiceLayer(new Stub_deliverySystem(), new Stub_paymentSystem(), "admin", "123489Op");
            }
            catch (Exception) { }
            userServiceLayer1 = serviceLayer.Connect();
            userServiceLayer2 = serviceLayer.Connect();
            try
            {
                userServiceLayer1.Register("openStoreUser", "1221LkkJ");
                userServiceLayer2.Register("openStoreUser2", "1221Xccv");
                userServiceLayer1.Login("openStoreUser", "1221LkkJ");
                userServiceLayer2.Login("openStoreUser2", "1221Xccv");
            }
            catch (Exception) { }
        }


        [TestMethod()]
        public void OpenStore_happyTest()
        {
            Assert.IsTrue(userServiceLayer1.Open_Store("marketim"));   //happy 1
            Assert.ThrowsException<Exception>(() => { userServiceLayer2.Open_Store("marketim"); }, "This store name is already taken");  //happy 2
        }

        [TestMethod()]
        public void OpenStore_visitor_state_sadTest()  
        {
            I_User_ServiceLayer tmp_userServiceLayer = serviceLayer.Connect();
            Assert.ThrowsException<Exception>(() => { tmp_userServiceLayer.Open_Store("dig dig dog"); }, "only registered user can open stores");  //happy 2
        }

        [TestCleanup]
        public void CleanUp()
        {
            serviceLayer.CleanUpSystem();
            serviceLayer = null;
        }

    }
}