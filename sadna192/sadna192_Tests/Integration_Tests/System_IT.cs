using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using static sadna192_Tests.Stubs;
using sadna192;
using System.Collections.Generic;

namespace sadna192_Tests.Integration_Tests
{
    [TestClass]
    public class System_IT
    {
        private static I_ServiceLayer serviceLayer;

        [TestMethod]
        public void SystemRestart()
        {
            serviceLayer = new ServiceLayer();
            I_DeliverySystem deliverySystem = new Stub_deliverySystem();
            I_PaymentSystem paymentSystem = new Stub_paymentSystem();
            serviceLayer.Create_ServiceLayer(deliverySystem, paymentSystem, "adminTest", "1234Abcd");
            Assert.IsTrue(deliverySystem.Connect().Result);
            Assert.IsTrue(paymentSystem.Connect().Result);
            I_User_ServiceLayer adminUser = serviceLayer.Connect(new Stub_Alerter());
            Assert.IsTrue(adminUser.Login("adminTest", "1234Abcd"));
            Assert.IsTrue(adminUser.GetUserState().isAdmin());
        }

      [TestCleanup]
      public void CleanUp()
        {
            serviceLayer.CleanUpSystem();
            serviceLayer = null;
        }
    }
}
