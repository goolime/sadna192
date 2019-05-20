using System;
using sadna192;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace sadna192_Tests.Unit_Tests
{

    [TestClass]
    public class Shopping_Basket_UT
    {
        Store store1;
        Store store2;

        ShopingBasket shopingBasket;

        [TestInitialize]
        public void Init()
        {
            store1 = new Store("Castro");
            store2 = new Store("Fox");
            shopingBasket = new ShopingBasket(new List<ShoppingCart>());
            store1.addProduct("T-shirt", "Clothes", 10, 20, new noDiscount(), new regularPolicy());
            store1.addProduct("Shoes", "Clothes", 15, 25, new noDiscount(), new regularPolicy());
            store2.addProduct("Socks", "Clothes", 5, 10, new noDiscount(), new regularPolicy());
            store2.addProduct("Pants", "Clothes", 18, 15, new noDiscount(), new regularPolicy());
        }

        [TestMethod]
        public void addProduct_Classic_Case_Test()
        {
            shopingBasket.addProduct(store1.FindProductInStore("T-shirt"), 14);
            shopingBasket.addProduct(store2.FindProductInStore("Socks"), 2);
            Assert.IsTrue(shopingBasket.getshoppingCarts().Count==2);
        }

        [TestMethod]
        public void addProduct_Same_Store_Test()
        {
            shopingBasket.addProduct(store1.FindProductInStore("T-shirt"), 14);
            shopingBasket.addProduct(store1.FindProductInStore("Shoes"), 2);
            Assert.IsTrue(shopingBasket.getshoppingCarts().Count == 1);
            Assert.IsTrue(shopingBasket.getshoppingCarts()[0].getStore().getName() == store1.getName());
        }

        [TestMethod]
        public void EditProductAmount_Test()
        {
            shopingBasket.addProduct(store1.FindProductInStore("T-shirt"), 14);
            Assert.IsTrue(shopingBasket.getshoppingCarts().Count == 1);
            Assert.IsTrue(shopingBasket.getshoppingCarts()[0].FindProductInCart("T-shirt").Second == 14);
            shopingBasket.editProductAmount(shopingBasket.getshoppingCarts()[0].FindProductInCart("T-shirt").First, 12);
            int p = shopingBasket.getshoppingCarts()[0].FindProductInCart("T-shirt").First.getAmount();
            Assert.IsTrue(shopingBasket.getshoppingCarts()[0].FindProductInCart("T-shirt").Second == 12);
        }


        /*[TestMethod]
        public void Finalize_Purchase_Test()
        {
            shopingBasket.addProduct(store1.FindProductInStore("T-shirt"), 14);
            Assert.IsTrue(shopingBasket.Finalize_Purchase());
            shopingBasket.editProductAmount(shopingBasket.getshoppingCarts()[0].FindProductInCart("T-shirt").First, 12);
            int p = shopingBasket.getshoppingCarts()[0].FindProductInCart("T-shirt").First.getAmount();
            Assert.IsTrue(shopingBasket.getshoppingCarts()[0].FindProductInCart("T-shirt").Second == 12);
        }


    */




    }
}
