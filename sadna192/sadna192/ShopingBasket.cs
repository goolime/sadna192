using System;
using System.Collections.Generic;

namespace sadna192
{
    internal class ShopingBasket
    {
        //private Member member;
        private List<ShoppingCart> shoppingCarts;
        private List<KeyValuePair<ProductInStore, KeyValuePair<int, double>>> savedProducts;


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
                    foreach (ProductInStore pp in sc.getStore().getProductInStore())
                    {
                        if (pp.getName() == p.getName())
                        {
                            pp.setAmount(amount);
                            return true;
                        }
                    }

                }

            }
            throw new SystemException("There is no such product in store" + p.getStore().getName());
        }

        internal bool Finalize_Purchase(string address, string payment)
        {
            throw new NotImplementedException();
        }

        internal List<KeyValuePair<ProductInStore, KeyValuePair<int, double>>> Purchase_product(ProductInStore p, int amount)
        {
            if (p.getStore().FindProductInStore(p.getName()).getPolicy().immidiate())
            {
                if (p.getAmount() - amount >= 0)
                {
                    p.setAmount(p.getAmount() - amount);
                    savedProducts.Add(new KeyValuePair<ProductInStore, KeyValuePair<int, double>> (p,new KeyValuePair<int, double>(amount, p.getPrice()*amount-p.getDiscount().calculate(amount, p.getPrice()))));
                }
                else
                {
                    this.returnProducts();
                    throw new Exception("There are no enough pieces of " + p.getName() + "in the store " + p.getStore());
                }
            }
            return this.savedProducts;
        }

        internal List<KeyValuePair<ProductInStore, KeyValuePair<int, double>>> Purchase_Store_cart(string store_name)
        {

            return this.savedProducts;
        }

        internal List<KeyValuePair<ProductInStore, int>> get_basket()
        {
            throw new NotImplementedException();
        }

        internal void returnProducts()
        {

        }
    }
}