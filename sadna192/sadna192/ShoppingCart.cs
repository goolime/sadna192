using System;
using System.Collections.Generic;


namespace sadna192
{
    internal class ShoppingCart
    {
        private Store store;
        private List<Pair<ProductInStore, int>> shoppingCartContent;
        private ProductPurchaseType productPurchaseType;

        public ShoppingCart(Store store, List<Pair<ProductInStore, int>> shoppingCartContent)
        {
            this.store = store;
            this.shoppingCartContent = shoppingCartContent;
        }

        public Store getStore()
        {
            return store;
        }

        internal bool addProduct(ProductInStore p, int amount)
        {
            Pair<ProductInStore, int> producttoadd = new Pair<ProductInStore, int>(p, amount);
            //checking if the prouct exists in the current shopping cart
            if (shoppingCartContent.Contains(producttoadd) | store.ChangeAmoutStoreInPurchase(p, amount))
            {
                return false;
            }
            else
            {
                try
                {
                    shoppingCartContent.Add(producttoadd);
                }
                catch (Exception)
                {
                    throw new Sadna192Exception("Fail to add product " + p.getName() + " to the cart" , "ShoppingCart" , "addProduct");
                }
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
                else
                {
                    throw new Sadna192Exception("There is no such product " + p.getName() + " in the shopping cart", "ShoppingCart", "DeleteProduct");
                }
            }
            return false;
        }
        


        internal bool editAmount(ProductInStore p, int amount)             //editAmount
        {
            if (amount == 0) return this.DeleteProduct(p, amount);
            foreach (Pair<ProductInStore,int> v in shoppingCartContent)
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
            foreach (Pair<ProductInStore, int> p in this.shoppingCartContent)
            {
                ans.Add(new KeyValuePair<ProductInStore, int>(p.First, p.Second));
            }
            return ans;
        }
    }

    
}