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
        Owner owner;
        Member member1;

        [TestInitialize]
        public void Init()
        {
            member = new Member("Ron", "12121212");
            member1 = new Member("Dvir", "99999999");
            store = new Store("Maccabi");
            owner = new Owner(member, store);
            member.owner.Add(owner);
        }

        [TestMethod()]
        public void get_Store_Test()
        {
            owner = new Owner(member, store);
            Assert.IsTrue(owner.getStore().getName().Equals("Maccabi"));
        }


        [TestMethod()]
        public void addProduct_Positive_Test()
        {
            owner.addProduct("Bisli", "Food", 14, 5, new noDiscount(), new regularPolicy());
            Assert.IsTrue(owner.getStore().FindProductInStore("Bisli")!=null);
        }

        [TestMethod()]
        public void addProduct_Negetive_Test()
        {
            owner.addProduct("Bisli", "Food", 14, 5, new noDiscount(), new regularPolicy());
            Assert.ThrowsException<Exception>(() => { owner.getStore().FindProductInStore("Bamba"); }, "Manager can't assign other Managers");

        }

        [TestMethod()]
        public void AddManager_Test()
        {
            owner.addManager(member, true, false, true);
            //member.isManager();
        }

        [TestMethod()]
        public void AddOwner_Test()
        {
            Store s = new Store("Maccabi");
            owner.addOwner("Maccabi", member1);
            Assert.IsTrue(member1.isOwner("Maccabi"));
        }


        [TestMethod()]
        public void removeProduct_Test()
        {
            store.addProduct("Banana", "food", 15, 10, new noDiscount(), new regularPolicy());
            store.addProduct("apple", "food", 15, 10, new noDiscount(), new regularPolicy());
            Assert.IsTrue(store.FindProductInStore("Banana") != null);
            owner.removeProduct("Banana");
            Assert.ThrowsException<Exception>(() => { store.FindProductInStore("Banana"); }, "Manager can't assign other Managers");
        }

        
        [TestMethod()]
        public void removeApointed_Test()
        {
            owner.addOwner("Maccabi", member1);
            Assert.IsTrue(member1.isOwner("Maccabi"));
            owner.removeApointed(member1);
            Assert.IsFalse(member1.isOwner("Maccabi"));
        }

        /* [TestMethod()]
         public void findOwner_Test()
         {
             owner.addOwner("Maccabi", member);
             Owner p = owner.findOwner(member);
             Assert.IsTrue(member.isOwner("Maccabi"));
             owner.removeApointed(member);
             Assert.IsFalse(member.isOwner("Maccabi"));
         }
         */

        [TestMethod()]
        public void updateProduct_Test()
        {
            store.addProduct("Banana", "food", 15, 10, new noDiscount(), new regularPolicy());
            owner.updateProduct("Banana", "Apple", null, -1, -1, null, null);
            Assert.IsTrue(store.FindProductInStore("Apple") != null);
            Assert.ThrowsException<Exception>(() => { store.FindProductInStore("Banana"); }, "Manager can't assign other Managers");
        }

        [TestMethod()]
        public void removeManager_Test()
        {
            owner.addManager(member1, true, false, true);
            //Assert.IsTrue(member1.isManager());
            owner.removeManager(member1);
            //Assert.IsFalse(member1.isManager());
        }

        [TestMethod()]
        public void removeOwner_Test()
        {
            owner.addOwner("Maccabi", member1);
            Assert.IsTrue(member1.isOwner("Maccabi"));
            owner.removeOwner(member1);
            Assert.IsFalse(member1.isOwner("Maccabi"));
        }

        //addManager
        [TestMethod()]
        public void removeOwnerByHimself_Test()
        {
            Assert.IsTrue(member.isOwner("Maccabi"));
            Assert.ThrowsException<Exception>(() => { owner.removeOwner(member); }, "Manager can't Remove himself to be owner of a shop");
        }













    }
}
