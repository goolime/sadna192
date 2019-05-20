using System;
using sadna192;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace sadna192_Tests.Unit_Tests
{
    [TestClass]
    public class Shopping_Cart_UT
    {

        Store store;
        ShoppingCart shoppingCart;

        [TestInitialize]
        public void Init()
        {
            store = new Store("Maccabi");
            shoppingCart = new ShoppingCart(store, new List<Pair<ProductInStore, int>>());
            store.addProduct("T-shirt", "Clothes", 15, 20, new noDiscount(), new regularPolicy());
            store.addProduct("Shoes", "Clothes", 15, 20, new noDiscount(), new regularPolicy());
        }

        [TestMethod]
        public void Add_Product_Classic_Test()
        {
            //checking if the cart is empty
            Assert.IsTrue(shoppingCart.getCart().Count == 0);

            shoppingCart.addProduct(store.FindProductInStore("T-shirt"), 6);
            shoppingCart.addProduct(store.FindProductInStore("Shoes"), 3);
            Assert.IsTrue(shoppingCart.getCart().Count == 2);
        }


        [TestMethod]
        public void Add_Same_Product_Test()
        {
            shoppingCart.addProduct(store.FindProductInStore("T-shirt"), 6);
            shoppingCart.addProduct(store.FindProductInStore("T-shirt"), 3);
            Assert.IsTrue(shoppingCart.getCart().Count == 1);
            Assert.IsTrue(shoppingCart.getCart()[0].Value == 9);
        }

        [TestMethod]
        public void Delete_Product_Test()
        {
            shoppingCart.addProduct(store.FindProductInStore("T-shirt"), 6);
            shoppingCart.DeleteProduct(store.FindProductInStore("T-shirt"), 3);
            Assert.IsTrue(shoppingCart.getCart().Count == 0);
        }

        [TestMethod]
        public void EditAmount_Test()
        {
            shoppingCart.addProduct(store.FindProductInStore("T-shirt"), 6);
            shoppingCart.editAmount(store.FindProductInStore("T-shirt"),3);
            Assert.IsTrue(shoppingCart.FindProductInCart("T-shirt").Second == 3);
        }

        [TestMethod]
        public void FindProductInCart_Test()
        {
            Assert.ThrowsException<Exception>(() => { shoppingCart.FindProductInCart("Water"); }, "There is no such product called Water");
            shoppingCart.addProduct(store.FindProductInStore("T-shirt"), 6);
            Assert.IsTrue(shoppingCart.FindProductInCart("T-shirt").Second==6);
        }
    }
}
