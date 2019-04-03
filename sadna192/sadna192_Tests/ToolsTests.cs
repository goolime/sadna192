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
    public class ToolsTests
    {
        [TestMethod()]
        public void check_usernameTest()
        {
            Assert.ThrowsException<Exception>(()=> {Tools.check_username(""); }, "fail on username check when length is 0");
            Assert.ThrowsException<Exception>(() => { Tools.check_username("a"); }, "fail on username check when length is 1");
            Assert.ThrowsException<Exception>(() => { Tools.check_username("ab"); }, "fail on username check when length is 2");
            Assert.ThrowsException<Exception>(() => { Tools.check_username("abc"); }, "fail on username check when length is 3");
            Assert.ThrowsException<Exception>(() => { Tools.check_username("4abc"); }, "fail on username check when it doesn't start with a letter");
            
            //Assert.IsTrue(Tools.check_username(""),"check ")
        }

        [TestMethod()]
        public void check_passwordTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void check_storeNameTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void check_productNamesTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void check_productCategoryTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void check_priceTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void check_amountTest()
        {
            Assert.Fail();
        }
    }
}