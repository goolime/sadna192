using System;
using sadna192;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace sadna192_Tests.Unit_Tests
{
    [TestClass]
    public class Visitor_UT
    {
        Visitor visitor;
        Member member;
        Store store;
        Manager manager;
        Owner owner;
        Member member1;

        [TestInitialize]
        public void Init()
        {
            store = new Store("Maccabi");
            store.addProduct("Banana", "Food", 1, 5, new noDiscount(), new regularPolicy());
            store.addProduct("Apple", "Food", 2, 5, new noDiscount(), new regularPolicy());
            store.addProduct("Orange", "Food", 7, 5, new noDiscount(), new regularPolicy());
            store.addProduct("Nuts", "Veg", 9, 5, new noDiscount(), new regularPolicy());
            visitor = new Visitor();
        }

        [TestMethod()]
        public void Add_To_ShopingBasket_Test()
        {
            visitor.Add_To_ShopingBasket(store.FindProductInStore("Banana"), 2);
            Assert.IsTrue(visitor.Watch_Cart().Count == 1);
        }


        /*[TestMethod()]
        public void Add_To_ShopingBasketNotPossible_Test()
        {
            visitor.Add_To_ShopingBasket(store.FindProductInStore("Banana"), 7);
            Assert.IsTrue(visitor.Watch_Cart().Count == 1);
        }*/
        [TestMethod()]
        public void Edit_Product_In_ShopingBasket_Test()
        {
            visitor.Add_To_ShopingBasket(store.FindProductInStore("Banana"), 2);
            visitor.Edit_Product_In_ShopingBasket(store.FindProductInStore("Banana"), 1);
            Assert.IsTrue(visitor.Watch_Cart()[0].Value == 1);
        }

        [TestMethod()]
        public void isAdmin_Test()
        {
            Assert.IsFalse(visitor.isAdmin());
            
        }

        [TestMethod()]
        public void isMember_Test()
        {
            Assert.IsFalse(visitor.isMember());
        }


        [TestMethod()]
        public void isVisitor_Test()
        {
            Assert.IsTrue(visitor.isVistor());
        }

        [TestMethod()]
        public void OpenStore_Test()
        {
            Assert.ThrowsException<Exception>(() => { visitor.Open_Store(store); }, "Visitor Cannot Open A New Store!");
        }


        //TODO
        /*
         * 
        [TestMethod()]
        public void Purchase_product_Test()
        {
            visitor.Add_To_ShopingBasket(store.FindProductInStore("Banana"), 2);
            List<KeyValuePair<ProductInStore, KeyValuePair<int, double>>>  List = visitor.Purchase_product(visitor.Watch_Cart()[0].Key, visitor.Watch_Cart()[0].Value);
            Assert.IsTrue(List.Count == 1);  

        }
        */

        //TODO
        /*
         * 
        [TestMethod()]
        public void Purchase_Store_cart_Test()
        {
            visitor.Add_To_ShopingBasket(store.FindProductInStore("Banana"), 2);
            List<KeyValuePair<ProductInStore, KeyValuePair<int, double>>>  List = visitor.Purchase_product(visitor.Watch_Cart()[0].Key, visitor.Watch_Cart()[0].Value);
            Assert.IsTrue(List.Count == 1);  

        }
        */

        [TestMethod()]
        public void Remove_Product_Store_Test()
        {
            Assert.ThrowsException<Exception>(() => { visitor.Remove_Product_Store("store","product"); }, "Visitor Cannot Remove Product From A Store!");

        }

        [TestMethod()]
        public void Remove_Store_Manager_Test()
        {
            Assert.ThrowsException<Exception>(() => { visitor.Remove_Store_Manager("store", member1); }, "Visitor Cannot Remove A Manager From A Store!");

        }

        [TestMethod()]
        public void Remove_Store_Owner_Test()
        {
            Assert.ThrowsException<Exception>(() => { visitor.Remove_Store_Owner("store", member1); }, "Visitor Cannot Remove Store Owner From A Store!");

        }

        [TestMethod()]
        public void Update_Product_Store()
        {
            Assert.ThrowsException<Exception>(() => { visitor.Update_Product_Store("store", "product", "newproduct", "category", -1, -1 , new noDiscount(), new regularPolicy()); }, "Visitor Cannot Update Product in A Store!");

        }









    }
}
