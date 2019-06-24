using System;
using sadna192;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace sadna192_Tests.Unit_Tests
{
    [TestClass]
    public class Member_UT
    {
        Member member;
        Member member1;
        Store store;
        Store store1;
        Owner owner;

        [TestInitialize]
        public void Init()
        {
            member = new Member("Ron", "12121212");
            member1 = new Member("Guy", "34343434");
            store = new Store("Max Brenner");
            store1 = new Store("Ossem");
            owner = new Owner(member, store);
            member.Open_Store(store);


        }


        [TestMethod]
        public void isOwner_Test()
        {
            member.Add_Store_Owner("Max Brenner", member1);
            Assert.IsTrue(member1.isOwner("Max Brenner"));
        }

        [TestMethod]
        public void isMe_Test()
        {
            Assert.IsTrue(member.isMe("Ron"));
        }

        [TestMethod]
        public void getUserStore_Test()
        {
            Assert.IsTrue(member.getUserStore("Max Brenner").getStore().getName()== "Max Brenner");
        }
        






    }
}
