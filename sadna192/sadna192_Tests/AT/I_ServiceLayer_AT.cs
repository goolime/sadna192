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
    public class I_ServiceLayer_AT
    {
        private static I_ServiceLayer serviceLayer;
      
        private class Stub_bad_deliverySystem : Stub_deliverySystem
        {
            
            public override Task<bool> Connect()
            {
                return new Task<bool>(()=>false);
            }
        }

        private class Stub_bad_paymentSystem : Stub_paymentSystem
        {

            public override Task<bool> Connect()
            {
                return new Task<bool>(()=>false);
            }
        }

        //Use Case 1.1
        [TestMethod()]
        public void Create_ServiceLayer_HappyTest()
        {
            serviceLayer = new ServiceLayer();
            serviceLayer.Create_ServiceLayer(new Stub_deliverySystem(), new Stub_paymentSystem(), "adminTest", "1234Abcd");
            I_User_ServiceLayer userServiceLayer = serviceLayer.Connect(new Stub_Alerter());
            Assert.IsTrue(userServiceLayer.Login("adminTest", "1234Abcd"));
        }

        [TestMethod()]
        public void Create_ServiceLayer_SadTest()
        {
            serviceLayer = new ServiceLayer();
            Assert.ThrowsException<Exception>(() => { serviceLayer.Create_ServiceLayer(new Stub_deliverySystem(), new Stub_paymentSystem(), "", "1234SdFg"); }, "admin details are not valid.");
            Assert.ThrowsException<Exception>(() => { serviceLayer.Create_ServiceLayer(new Stub_deliverySystem(), new Stub_paymentSystem(), "admin", ""); }, "admin details are not valid.");
            Assert.ThrowsException<Exception>(() => { serviceLayer.Create_ServiceLayer(new Stub_bad_deliverySystem() , new Stub_paymentSystem(), "admin", "1234FgHj"); }, "Delivary System cannot connect");
            Assert.ThrowsException<Exception>(() => { serviceLayer.Create_ServiceLayer(new Stub_deliverySystem(), new Stub_bad_paymentSystem(), "admin", "1234GhJk"); }, "Payment System cannot connect");
        }

    }
}