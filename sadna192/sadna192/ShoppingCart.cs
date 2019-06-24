using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace sadna192
{
    public class ShoppingCart
    {
        private Store store;
        private List<Pair<ProductInStore, int>> shoppingCartContent;

        public ShoppingCart(Store store, List<ItemsInCart> shoppingCartContent)
        {
            this.store = store;
            this.shoppingCartContent = shoppingCartContent;
        }

        public ShoppingCart(ShoppingCart sc, UserState u)
        {
            this.store = new Store(sc.store);
            this.shoppingCartContent = new List<Pair<ProductInStore, int>>();
            foreach (Pair<ProductInStore, int> p in sc.shoppingCartContent)
            {
                this.shoppingCartContent.Add(new Pair<ProductInStore, int>(this.store.getProductInStore(p.First.getProduct().name), p.Second));
            }
        }

        public Store getStore()
        {
            return store;
        }

        internal bool addProduct(ProductInStore p, int amount, bool isMember)
        {
            ItemsInCart producttoadd = new ItemsInCart(p, amount);
            //checking if the prouct exists in the current shopping cart
                foreach (ItemsInCart pro in shoppingCartContent)
                {
                    if (pro.First.getName() == p.getName())
                    {
                        pro.Second = pro.Second + amount;
                        return true;
                    }
                }
                try
                {
                    shoppingCartContent.Add(producttoadd);
                if(isMember)
                    if (!DBAccess.SaveToDB(producttoadd))
                        DBAccess.DBerror("could not save ProductInStore & amount to DB");
                }
                catch (Exception)
                {
                    throw new Sadna192Exception("Fail to add product " + p.getName() + " to the cart" , "ShoppingCart" , "addProduct");
                }
            
            return true;
        }


        internal bool DeleteProduct(ProductInStore p, int amount)             //DeleteProduct
        {
            foreach (var v in shoppingCartContent)
            {

                if (v.First.getName() == p.getName())
                {
                    shoppingCartContent.Remove(v);
                    return true;
                }
            }
            throw new Sadna192Exception("There is no such product " + p.getName() + " in the shopping cart", "ShoppingCart", "DeleteProduct");
        }
        


        internal bool editAmount(ProductInStore p, int amount)             //editAmount
        {
            if (amount == 0) return this.DeleteProduct(p, amount);
            foreach (ItemsInCart v in shoppingCartContent)
            {

                if (v.First.getName() == p.getName())
                {
                    v.Second = amount;
                    return true;
                }
            }
            throw new Sadna192Exception("There is no such product " + p.getName() + " in the shopping cart of " + store.getName(), "ShoppingCart", "editAmount");
        }


        internal bool Finalize_Purchase(ProductInStore product, string address, string payment)
        {
            throw new NotImplementedException();                     
        }

        internal List<KeyValuePair<ProductInStore, int>> getCart()
        {
            List<KeyValuePair<ProductInStore, int>> ans = new List<KeyValuePair<ProductInStore, int>>();
            foreach (ItemsInCart p in this.shoppingCartContent)
            {
                ans.Add(new KeyValuePair<ProductInStore, int>(p.First, p.Second));
            }
            return ans;
        }

        internal ItemsInCart FindProductInCart(String s)
        {
            foreach (ItemsInCart pro in shoppingCartContent)
            {
                if (pro.First.getName() == s)
                {
                    return pro;
                }
            }
            throw new Sadna192Exception("There is no such product in this cart this moment", "ShoppingCart", "FindProductInCart");
        }

        internal int numOfItemsInCart()
        {
            int ans = 0;
            foreach (ItemsInCart p in this.shoppingCartContent)
            {
                ans += p.Second;
            }
            return ans;
        }

        internal int numOfItemsInCart(string op)
        {
            foreach (ItemsInCart p in this.shoppingCartContent)
            {
                if (op == p.First.getName()) return p.Second;
            }
            return -1;
        }
    }

    
}