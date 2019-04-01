using System.Collections.Generic;

namespace sadna192
{
    internal class Visitor : UserState
    {
        private ShopingCart shopingCart;

        public Visitor()
        {
        }

        public bool Add_To_ShopingBasket(ProductInStore p, int amount)
        {
            throw new System.NotImplementedException();
        }

        public bool Edit_Product_In_ShopingBasket(ProductInStore p, int amount)
        {
            throw new System.NotImplementedException();
        }

        public bool Finalize_Purchase(string address, string payment)
        {
            throw new System.NotImplementedException();
        }

        public List<KeyValuePair<ProductInStore, int>> get_ShopingBasket()
        {
            throw new System.NotImplementedException();
        }

        public virtual bool isAdmin()
        {
            return false;
        }

        public virtual bool isMember()
        {
            return false;
        }

        public bool isVistor()
        {
            return true;
        }

        public List<KeyValuePair<ProductInStore, KeyValuePair<int, double>>> Purchase_product(ProductInStore p, int amount)
        {
            throw new System.NotImplementedException();
        }

        public List<KeyValuePair<ProductInStore, KeyValuePair<int, double>>> Purchase_Store_cart(int Shop_ID)
        {
            throw new System.NotImplementedException();
        }
    }
}