using System;
using sadna192;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace sadna192_Tests.Unit_Tests
{
    [TestClass]
    public class Manager_UT
    {
        Member member;
        Member member1;
        Store store;
        Store store1;
        Manager manager;
        Manager manager1;

        [TestInitialize]
        public void Init()
        {
            member = new Member("Ron", "12121212");
            member1 = new Member("Guy", "34343434");
            store = new Store("Max Brenner");
            store1 = new Store("Ossem");


        }

        [TestMethod()]
        public void add_Product_Positive_Test()
        {
            manager = new Manager(member, store, true, false, false);
            Assert.IsTrue(manager.addProduct("banana", "food", 25, 4, new noDiscount(), new regularPolicy()));

            manager = new Manager(member, store, false, false, true);
            Assert.IsTrue(manager.updateProduct("banana", "banani","food", 25, 4, new noDiscount(), new regularPolicy()));

            manager = new Manager(member, store, false, true, false);
            Assert.IsTrue(manager.removeProduct("banani"));

        }

        [TestMethod()]
        public void add_Product_Negetive_Test()
        {
            manager = new Manager(member, store, false, false, false);
            Assert.ThrowsException<Exception>(() => { manager.addProduct("banana", "food", 25, 4, new noDiscount(), new regularPolicy()); }, "Add Product is not allowed for this manager");

            manager = new Manager(member, store, true, false, false);
            manager.addProduct("banana", "food", 25, 4, new noDiscount(), new regularPolicy());
            Assert.ThrowsException<Exception>(() => { manager.updateProduct("banana", "banani", "food", 25, 4, new noDiscount(), new regularPolicy()); }, "Update Product is not allowed for this manager");
            Assert.ThrowsException<Exception>(() => { manager.removeProduct("banana"); }, "Update Product is not allowed for this manager");
        }


        [TestMethod()]
        public void add_Manager_Test()
        {
            manager = new Manager(member, store, false, false, false);
            Assert.ThrowsException<Exception>(() => { manager.addManager(member1, true, true, true); }, "Manager can't assign other Managers");
        }

        [TestMethod()]
        public void add_Owner_Test()
        {
            manager = new Manager(member, store, false, false, false);
            Assert.ThrowsException<Exception>(() => { manager.addOwner("baba", member1); }, "Manager can't assign Owners");
        }

        [TestMethod()]
        public void remove_Product_Positive_Test()
        {
            manager = new Manager(member, store, true, true, false);
            manager.addProduct("apple", "food", 10, 6, new noDiscount(), new regularPolicy());
            Assert.IsTrue(manager.removeProduct("apple"));
        }

        [TestMethod()]
        public void remove_Product_Negetive_Test()
        {
            manager = new Manager(member, store, true, false, false);
            manager.addProduct("apple", "food", 10, 6, new noDiscount(), new regularPolicy());
            Assert.ThrowsException<Exception>(() => { manager.removeProduct("apple"); }, "this manager isn't allowed to remove product");
        }


        [TestMethod()]
        public void removeApointed_Test()
        {
            manager = new Manager(member, store, true, true, true);
            Assert.ThrowsException<Exception>(() => { manager.removeApointed(member1); }, "Manager can't remove other managers or owners");
        }

        [TestMethod()]
        public void updateProduct_Test()
        {
            manager = new Manager(member, store, true, true, true);
            manager.addProduct("apple", "food", 10, 6, new noDiscount(), new regularPolicy());
            Assert.IsTrue(manager.updateProduct("apple", "apple_new", "fruits", 10, 6, new noDiscount(), new regularPolicy()));
            //Assert.IsTrue(manager.updateProduct("apple", "apple_new", "fruits", 10, 6, new noDiscount(), new regularPolicy()));
            Assert.IsTrue(store.FindProductInStore("apple_new").getCategory().Equals("fruits"));
        }


        [TestMethod()]
        public void removeManager_Test()
        {
            manager = new Manager(member, store, true, true, true);
            Assert.ThrowsException<Exception>(() => { manager.removeManager(member1); }, "Manager can't remove Managers");
        }


        [TestMethod()]
        public void removeOwner_Test()
        {
            manager = new Manager(member, store, true, true, true);
            Assert.ThrowsException<Exception>(() => { manager.removeOwner(member1); }, "Manager can't remove Owners");
        }



        [TestCleanup]
        public void TestClean()
        {
            member = null;
            store = null;
            manager = null;
        }









    }
}
