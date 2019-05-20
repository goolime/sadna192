using System;
using sadna192;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace sadna192_Tests.Unit_Tests
{
    [TestClass]
    public class Store_UT
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
            store.addProduct("Banana", "Food", 1, 5, new noDiscount(), new regularPolicy());
            store.FindProductInStore("Banana").getProduct().setKeywords(new List<String> { "a", "b", "c"});
            store.addProduct("Apple", "Food", 2, 5, new noDiscount(), new regularPolicy());
            store.FindProductInStore("Apple").getProduct().setKeywords(new List<String> { "b", "c" });
            store.addProduct("Orange", "Food", 7, 5, new noDiscount(), new regularPolicy());
            store.FindProductInStore("Orange").getProduct().setKeywords(new List<String> { "c" });
            store.addProduct("Nuts", "Veg", 9, 5, new noDiscount(), new regularPolicy());
        }

        [TestMethod()]
        public void addProduct_Test()
        {
            store.addProduct("T-shirt", "Cloths", 4, 5, new noDiscount(), new regularPolicy());
            Assert.IsTrue(store.FindProductInStore("T-shirt")!=null);
        }

        [TestMethod()]
        public void removeProduct_Test()
        {
            store.addProduct("T-shirt", "Cloths", 4, 5, new noDiscount(), new regularPolicy());
            store.removeProduct("T-shirt");
            Assert.ThrowsException<Exception>(() => { owner.getStore().FindProductInStore("T-shirt"); }, "There's no product called T-shirt in this store");
        }

        [TestMethod()]
        public void updateProduct_Test()
        {
            store.addProduct("T-shirt", "Cloths", 4, 5, new noDiscount(), new regularPolicy());
            store.updateProduct("T-shirt", "Hat", "Cloths", 4, 5, new noDiscount(), new regularPolicy());
            Assert.IsTrue(store.FindProductInStore("Hat") != null);
            Assert.ThrowsException<Exception>(() => { store.FindProductInStore("T-shirt"); }, "There's no product called T-shirt in this store");
        }

        [TestMethod()]
        public void FindProductInStore_Test()
        {
            store.addProduct("T-shirt", "Cloths", 4, 5, new noDiscount(), new regularPolicy());
            Assert.IsTrue(store.FindProductInStore("T-shirt") != null);
            Assert.ThrowsException<Exception>(() => { store.FindProductInStore("Hat"); }, "There's no such product called Hat in the store");
        }

        [TestMethod()]
        public void FindProductInStoreNoInput_Test()
        {
            Assert.IsTrue(store.Search(null, null, null, -1, -1, -1, -1).Count == 4);
        }


        [TestMethod()]
        public void FindProductInStoreByName_Test()
        {
            Assert.IsTrue(store.Search("Banana", null, null, -1 ,- 1 ,- 1 ,- 1).Count==1);
            Assert.IsTrue(store.Search("Grapes", null, null, -1, -1, -1, -1).Count == 0);
        }

        [TestMethod()]
        public void FindProductInStoreByCategory_Test()
        {
            Assert.IsTrue(store.Search(null, "Food", null, -1, -1, -1, -1).Count == 3);
            Assert.IsTrue(store.Search(null, "Veg", null, -1, -1, -1, -1).Count == 1);
        }

        [TestMethod()]
        public void FindProductInStoreByPriceRange_Test()
        {
            Assert.IsTrue(store.Search(null, null, null, 3, 10, -1, -1).Count == 2);
            Assert.IsTrue(store.Search(null, null, null, 2, 2, -1, -1).Count == 1);
            Assert.IsTrue(store.Search(null, null, null, 3, 4, -1, -1).Count == 0);
            Assert.IsTrue(store.Search(null, null, null, 1, -1, -1, -1).Count == 4);
            Assert.IsTrue(store.Search(null, null, null, -1, 3, -1, -1).Count == 2);
        }

        [TestMethod()]
        public void FindProductInStoreByKeywords_Test()
        {
            Assert.IsTrue(store.Search(null, null, new List<String> { "a" }, -1, -1, -1, -1).Count == 1);
            Assert.IsTrue(store.Search(null, null, new List<String> { "b" }, -1, -1, -1, -1).Count == 2);
            Assert.IsTrue(store.Search(null, null, new List<String> { "c" }, -1, -1, -1, -1).Count == 3);
            Assert.IsTrue(store.Search(null, null, new List<String> { "d" }, -1, -1, -1, -1).Count == 0);
        }






    }
}
