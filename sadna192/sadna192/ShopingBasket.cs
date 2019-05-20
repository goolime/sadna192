using System;
using System.Collections.Generic;
using System.Timers;

namespace sadna192
{
    internal class ShopingBasket
    {
        //private Member member;
        private List<ShoppingCart> shoppingCarts;
        internal List<KeyValuePair<ProductInStore, KeyValuePair<int, double>>> savedProducts;
        internal bool toBeRemoved;


        //Constructor
        public ShopingBasket(List<ShoppingCart> shoppingCarts)
        {
            this.shoppingCarts = shoppingCarts;
        }

        public ShopingBasket()
        {
            this.shoppingCarts = null;
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

        internal bool editProductAmount(ProductInStore p, int amount)
        {
            foreach (ShoppingCart sc in shoppingCarts)
            {
                //checking for the same store
                if (sc.getStore() == p.getStore())
                {
                    return sc.editAmount(p, amount);
                }

            }
            throw new SystemException("There is no such product in store" + p.getStore().getName());
        }

        internal double Finalize_Purchase()
        {
            if (this.savedProducts == null) throw new Exception("there are no save products");
            double ans = 0;
            foreach (KeyValuePair<ProductInStore, KeyValuePair<int, double>> p in this.savedProducts)
            {
                ans += p.Value.Value;
            }
            return ans;
        }


        internal List<KeyValuePair<ProductInStore, KeyValuePair<int, double>>> Purchase_product(ProductInStore p, int amount, UserState u)
        {
            savedProducts = null; 
            if (p.getStore().FindProductInStore(p.getName()).GetPolicy().immidiate())
            {
                if (p.getAmount() - amount >= 0)
                {
                    p.setAmount(p.getAmount() - amount);
                    savedProducts.Add(new KeyValuePair<ProductInStore, KeyValuePair<int, double>> (p,new KeyValuePair<int, double>(amount, p.getPrice()*amount-p.getDiscount().calculate(p, u))));
                }
                else
                {
                    this.returnProducts();
                    throw new Exception("There are no enough pieces of " + p.getName() + "in the store " + p.getStore());
                }
            }
            return this.savedProducts;
        }

        internal List<KeyValuePair<ProductInStore, KeyValuePair<int, double>>> Purchase_Store_cart(string store_name,UserState u)
        {
            foreach(ShoppingCart sc in shoppingCarts)
            {
                if(sc.getStore().getName() == store_name)
                {
                    savedProducts = new List<KeyValuePair<ProductInStore, KeyValuePair<int, double>>>();
                    foreach(KeyValuePair<ProductInStore,int> p in sc.getCart())
                    {
                        if (p.Key.getAmount() - p.Value >= 0)
                        {
                            p.Key.setAmount(p.Key.getAmount() - p.Value);
                            savedProducts.Add(new KeyValuePair<ProductInStore, KeyValuePair<int, double>>(p.Key, new KeyValuePair<int, double>(p.Value, p.Key.getPrice() * p.Value - p.Key.getDiscount().calculate(p.Key, u))));
                            sc.DeleteProduct(p.Key,0);
                        }
                        else
                        {
                            this.returnProducts();
                            throw new Exception("There are no enough pieces of " + p.Key.getName() + "in the store " + p.Key.getStore());
                        }
                    }
                    this.shoppingCarts.Remove(sc);
                    return this.savedProducts;
                }
            }
            throw new Exception("no cart for the store");
        }

        internal List<KeyValuePair<ProductInStore, int>> get_basket()
        {
            List<KeyValuePair<ProductInStore, int>> ans = new List<KeyValuePair<ProductInStore, int>>();

            foreach (ShoppingCart sc in this.shoppingCarts)
            {
                ans.AddRange(sc.getCart());
            }
            if (ans.Count == 0) throw new Exception("there are no product in the store");
            return ans;
        }

        internal void returnProducts()
        {
            foreach(KeyValuePair<ProductInStore, KeyValuePair<int, double>> productToReturn in savedProducts)
            {
                productToReturn.Key.setAmount(productToReturn.Key.getAmount() + productToReturn.Value.Key);
            }
            savedProducts = null;
        }
    }
}