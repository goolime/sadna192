using System;
using sadna192;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace sadna192_Tests.Unit_Tests
{
    [TestClass]
    public class Owner_UT
    {
        Member member;
        Store store;
        Manager manager;

        [TestInitialize]
        public void Init()
        {
            member = new Member("Ron", "12121212");
            store = new Store("Max Brenner");
        }

        [TestMethod()]
        public void get_Store_Test()
        {
            Owner owner = new Owner(member, store);
            Assert.IsTrue(owner.getStore().getName().Equals("Max Brenner"));
        }

        



    }
}
