using System;
using System.Collections.Generic;

namespace sadna192
{
    internal class ShopingBasket
    {
        public ShopingBasket()
        {
        }

        internal bool addProduct(ProductInStore p, int amount)
        {
            throw new NotImplementedException();
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