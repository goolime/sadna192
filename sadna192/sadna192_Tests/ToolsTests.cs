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
            Assert.ThrowsException<Exception>(() => { Tools.check_username(""); }, "fail on username check when length is 0");
            Assert.ThrowsException<Exception>(() => { Tools.check_username("a"); }, "fail on username check when length is 1");
            Assert.ThrowsException<Exception>(() => { Tools.check_username("ab"); }, "fail on username check when length is 2");
            Assert.ThrowsException<Exception>(() => { Tools.check_username("abc"); }, "fail on username check when length is 3");
            Assert.ThrowsException<Exception>(() => { Tools.check_username("4abc"); }, "fail on username check when it doesn't start with a letter");
            Assert.ThrowsException<Exception>(() => { Tools.check_username("a%bc"); }, "check a char which is not exceptable - '%'");
            Assert.ThrowsException<Exception>(() => { Tools.check_username("abc "); }, "check a char which is not exceptable - ' '");
            Assert.ThrowsException<Exception>(() => { Tools.check_username("abcע"); }, "check a char which is not exceptable - 'ע'");
            Assert.ThrowsException<Exception>(() => { Tools.check_username("abc&"); }, "check a char which is not exceptable - '&'");
            Assert.ThrowsException<Exception>(() => { Tools.check_username("abc\n"); }, "check a char which is not exceptable - '\\n'");
            Assert.ThrowsException<Exception>(() => { Tools.check_username("abc!"); }, "check a char which is not exceptable - '!'");
            Assert.IsTrue(Tools.check_username("abcd"), "check leagal name - abcd");
            Assert.IsTrue(Tools.check_username("ABCD"), "check leagal name - ABCD ");
            Assert.IsTrue(Tools.check_username("a123"), "check leagal name - a123");
            Assert.IsTrue(Tools.check_username("A123"), "check leagal name - A123");
        }

        [TestMethod()]
        public void check_passwordTest()
        {
            Assert.ThrowsException<Exception>(() => { Tools.check_password(""); }, "fail on password check when length is 0");
            Assert.ThrowsException<Exception>(() => { Tools.check_password("1234"); }, "fail on password check when length is 4");
            Assert.ThrowsException<Exception>(() => { Tools.check_password("1234567"); }, "fail on password check when length is 7");
            Assert.ThrowsException<Exception>(() => { Tools.check_password("0123456789abcdefg"); }, "fail on password check when length is 17");
            Assert.ThrowsException<Exception>(() => { Tools.check_password("01234567!"); }, "check a char which is not exceptable - '!'");
            Assert.ThrowsException<Exception>(() => { Tools.check_password("01234567%"); }, "check a char which is not exceptable - '%'");
            Assert.ThrowsException<Exception>(() => { Tools.check_password("01234567ע"); }, "check a char which is not exceptable - 'ע'");
            Assert.ThrowsException<Exception>(() => { Tools.check_password("12345678"); }, "check a password which contain only numbers");
            Assert.ThrowsException<Exception>(() => { Tools.check_password("abcdefgh"); }, "check a password which contain only small letters");
            Assert.ThrowsException<Exception>(() => { Tools.check_password("ABCDEFGH"); }, "check a password which contain only capital letters");
            Assert.ThrowsException<Exception>(() => { Tools.check_password("abcdABCD"); }, "check a password which not contain numbers");
            Assert.ThrowsException<Exception>(() => { Tools.check_password("abcd1234"); }, "check a password which not contain capital letters");
            Assert.ThrowsException<Exception>(() => { Tools.check_password("ABCD1234"); }, "check a password which not contain small letters");
            Assert.IsTrue(Tools.check_password("Password123"), "check leagal password - 'Password123'");
            Assert.IsTrue(Tools.check_password("123Password"), "check leagal password - '123Password'");
            Assert.IsTrue(Tools.check_password("passwordGal91"), "check leagal password - 'passwordGal91'");
        }

        [TestMethod()]
        public void check_storeNameTest()
        {
            Assert.ThrowsException<Exception>(() => { Tools.check_storeName(""); }, "fail on password check when length is 0");
            Assert.ThrowsException<Exception>(() => { Tools.check_storeName("abc"); }, "fail on password check when length is 3");
            Assert.ThrowsException<Exception>(() => { Tools.check_storeName("aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaab"); }, "fail on password check when length is 33");
            Assert.ThrowsException<Exception>(() => { Tools.check_storeName("1abc"); }, "fail on password don't start with a char");
            Assert.ThrowsException<Exception>(() => { Tools.check_storeName("aaa!"); }, "check a char which is not exceptable - '!'");
            Assert.ThrowsException<Exception>(() => { Tools.check_storeName("aaa:"); }, "check a char which is not exceptable - ':'");
            Assert.ThrowsException<Exception>(() => { Tools.check_storeName("aaa\n"); }, "check a char which is not exceptable - '\\n'");
            Assert.ThrowsException<Exception>(() => { Tools.check_storeName("aaaעbla"); }, "check a char which is not exceptable - 'ע'");
            Assert.IsTrue(Tools.check_storeName("Store"), "check leagal store name - 'Store'");
            Assert.IsTrue(Tools.check_storeName("St50"), "check leagal store name - 'St50'");
        }

        [TestMethod()]
        public void check_productNamesTest()
        {
            Assert.ThrowsException<Exception>(() => { Tools.check_productNames(""); }, "fail on password check when length is 0");
            Assert.ThrowsException<Exception>(() => { Tools.check_productNames("abc"); }, "fail on password check when length is 3");
            Assert.ThrowsException<Exception>(() => { Tools.check_productNames("aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaab"); }, "fail on password check when length is 33");
            Assert.ThrowsException<Exception>(() => { Tools.check_productNames("1abc"); }, "fail on password don't start with a char");
            Assert.ThrowsException<Exception>(() => { Tools.check_productNames("aaa!"); }, "check a char which is not exceptable - '!'");
            Assert.ThrowsException<Exception>(() => { Tools.check_productNames("aaa:"); }, "check a char which is not exceptable - ':'");
            Assert.ThrowsException<Exception>(() => { Tools.check_productNames("aaa\n"); }, "check a char which is not exceptable - '\\n'");
            Assert.ThrowsException<Exception>(() => { Tools.check_productNames("aaaעbla"); }, "check a char which is not exceptable - 'ע'");
            Assert.IsTrue(Tools.check_productNames("Store"), "check leagal product name - 'Store'");
            Assert.IsTrue(Tools.check_productNames("St50"), "check leagal product name - 'St50'");
        }

        [TestMethod()]
        public void check_productCategoryTest()
        {
            Assert.ThrowsException<Exception>(() => { Tools.check_productCategory(""); }, "fail on password check when length is 0");
            Assert.ThrowsException<Exception>(() => { Tools.check_productCategory("abc"); }, "fail on password check when length is 3");
            Assert.ThrowsException<Exception>(() => { Tools.check_productCategory("aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaab"); }, "fail on password check when length is 33");
            Assert.ThrowsException<Exception>(() => { Tools.check_productCategory("1abc"); }, "check a char which is not exceptable - '1'");
            Assert.ThrowsException<Exception>(() => { Tools.check_productCategory("aaa!"); }, "check a char which is not exceptable - '!'");
            Assert.ThrowsException<Exception>(() => { Tools.check_productCategory("aaa:"); }, "check a char which is not exceptable - ':'");
            Assert.ThrowsException<Exception>(() => { Tools.check_productCategory("aaa\n"); }, "check a char which is not exceptable - '\\n'");
            Assert.ThrowsException<Exception>(() => { Tools.check_productCategory("aaaעbla"); }, "check a char which is not exceptable - 'ע'");
            Assert.IsTrue(Tools.check_productCategory("Blaaa"), "check leagal product name - 'Blaaa'");
            Assert.IsTrue(Tools.check_productCategory("GalTest"), "check leagal product name - 'GalTest'");
        }

        [TestMethod()]
        public void check_priceTest()
        {
            Assert.IsFalse(Tools.check_price(-1),"smaller then 0");
            Assert.IsTrue(Tools.check_price(0),"equal to 0");
            Assert.IsTrue(Tools.check_price(0.5), "positive fruction");
            Assert.IsTrue(Tools.check_price(1),"positive number");
        }

        [TestMethod()]
        public void check_amountTest()
        {
            Assert.IsFalse(Tools.check_amount(-1), "smaller then 0");
            Assert.IsTrue(Tools.check_amount(0), "equal to 0");
            Assert.IsTrue(Tools.check_amount(1), "positive number");
        }
    }
}