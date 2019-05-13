using Microsoft.VisualStudio.TestTools.UnitTesting;
using sadna192;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sadna192.Tests
{
    [TestClass()]
    public class I_ServiceLayer_AT
    {
        private static I_ServiceLayer serviceLayer;
        private I_DeliverySystem deliverySystem;
        private I_PaymentSystem paymentSystem;

        //Use Case 1.1
        [TestMethod()]
        public void Create_ServiceLayer_GoodTest()
        {
            serviceLayer = I_ServiceLayer.Create_ServiceLayer(deliverySystem, paymentSystem, "admin", "1234");
            Assert.IsNotNull(deliverySystem);
            Assert.IsNotNull(paymentSystem);
            Assert.IsNotNull(serviceLayer);
            userServiceLayer = serviceLayer.connect();
            Assert.IsTrue(userServiceLayer.Login("admin", "1234"));
        }

        [TestMethod()]
        public void Create_ServiceLayer_BadTest()
        {
            Assert.ThrowsException<Exception>(() => { serviceLayer=I_ServiceLayer.Create_ServiceLayer(deliverySystem, paymentSystem, "", "1234"); }, "admin details are not valid.");
            Assert.IsNull(serviceLayer);
            Assert.ThrowsException<Exception>(() => { serviceLayer = I_ServiceLayer.Create_ServiceLayer(deliverySystem, paymentSystem, "admin", ""); }, "admin details are not valid.");
            Assert.IsNull(serviceLayer);
            Assert.ThrowsException<Exception>(() => { serviceLayer = I_ServiceLayer.Create_ServiceLayer(null, paymentSystem, "admin", "1234"); }, "Delivary System is missing");
            Assert.IsNull(serviceLayer);
            Assert.ThrowsException<Exception>(() => { serviceLayer = I_ServiceLayer.Create_ServiceLayer(deliverySystem, null, "admin", ""); }, "Payment System is missing");
            Assert.IsNull(serviceLayer);
        }
    }
}