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
            if (shoppingCartContent.Contains(producttoadd))
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
                    throw new SystemException("Fail to add product " + p.getName() + " to the cart");
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
                
            }
            throw new Exception("There is no such product " + p.getName() + " in the shopping cart");
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
            throw new Exception("There is no such product " + p.getName() + " in the shopping cart of " + store.getName());
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