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
    public class I_ServiceLayer_AT
    {
        private static I_ServiceLayer serviceLayer;
      
        private class stub_bad_deliverySystem : stub_deliverySystem
        {
            
            public override bool Connect()
            {
                return false;
            }
        }

        private class stub_bad_paymentSystem : stub_paymentSystem
        {

            public override bool Connect()
            {
                return false;
            }
        }

        //Use Case 1.1
        [TestMethod()]
        public void Create_ServiceLayer_GoodTest()
        {
            serviceLayer = new ServiceLayer();
            serviceLayer.Create_ServiceLayer(new stub_deliverySystem(), new stub_paymentSystem(), "admin", "1234Abcd");
            I_User_ServiceLayer userServiceLayer = serviceLayer.Connect();
            Assert.IsTrue(userServiceLayer.Login("admin", "1234Abcd"));
        }

        [TestMethod()]
        public void Create_ServiceLayer_BadTest()
        {
            Assert.ThrowsException<Exception>(() => { serviceLayer.Create_ServiceLayer(new stub_deliverySystem(), new stub_paymentSystem(), "", "1234"); }, "admin details are not valid.");
            Assert.ThrowsException<Exception>(() => { serviceLayer.Create_ServiceLayer(new stub_deliverySystem(), new stub_paymentSystem(), "admin", ""); }, "admin details are not valid.");
            Assert.ThrowsException<Exception>(() => { serviceLayer.Create_ServiceLayer(null, new stub_paymentSystem(), "admin", "1234"); }, "Delivary System is missing");
            Assert.ThrowsException<Exception>(() => { serviceLayer.Create_ServiceLayer(new stub_deliverySystem(), null, "admin", "1234"); }, "Payment System is missing");
            Assert.ThrowsException<Exception>(() => { serviceLayer.Create_ServiceLayer(new stub_bad_deliverySystem() , new stub_paymentSystem(), "admin", "1234"); }, "Delivary System cannot connect");
            Assert.ThrowsException<Exception>(() => { serviceLayer.Create_ServiceLayer(new stub_deliverySystem(), new stub_bad_paymentSystem(), "admin", "1234"); }, "Payment System cannot connect");
        }
    }
}