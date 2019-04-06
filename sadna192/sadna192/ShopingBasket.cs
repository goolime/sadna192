using System;
using System.Collections.Generic;

namespace sadna192
{
    internal class ShopingBasket
    {
        //private Member member;
        private List<ShoppingCart> shoppingCarts;

        //Constructor
        public ShopingBasket(List<ShoppingCart> shoppingCarts)
        {
            this.shoppingCarts = shoppingCarts;
        }

        internal bool addProduct(ProductInStore p, int amount)
        {
            //checking if already there is a Shopping cart for this prodcut in the shopping basket
            foreach(ShoppingCart sc in shoppingCarts)
            {
                if (sc.getStore()==p.getStore()){
                    sc.addProduct(p, amount);
                    return true;
                }

            }
            //if there is no Shopping cart for this store, add it
            List<Pair<ProductInStore, int>> shoppingCartContent = new List<Pair<ProductInStore, int>>();
            Pair<ProductInStore, int> productToAdd = new Pair<ProductInStore, int>(p, amount);
            shoppingCartContent.Add(productToAdd);
            ShoppingCart ShoppingCartToAdd = new ShoppingCart(p.getStore(), shoppingCartContent);
            shoppingCarts.Add(ShoppingCartToAdd);
            return true;
            
        }

        internal bool editProduct(ProductInStore p, int amount)
        {
            throw new NotImplementedException();
        }

        internal bool Finalize_Purchase(string address, string payment)
        {
            throw new NotImplementedException();
        }

        internal List<KeyValuePair<ProductInStore, KeyValuePair<int, double>>> Purchase_product(ProductInStore p, int amount)
        {
            throw new NotImplementedException();
        }

        internal List<KeyValuePair<ProductInStore, KeyValuePair<int, double>>> Purchase_Store_cart(string store_name)
        {
            throw new NotImplementedException();
        }

        internal List<KeyValuePair<ProductInStore, int>> get_basket()
        {
            throw new NotImplementedException();
        }
    }
}