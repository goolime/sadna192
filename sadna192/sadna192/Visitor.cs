using System.Collections.Generic;

namespace sadna192
{
    internal class Visitor : UserState
    {
        private ShopingCart shopingCart;

        public Visitor()
        {
        }

        public bool Add_Product_Store(string Store_name, string product_name, string product_category, double product_price, int product_amount, Discount product_discount, Policy product_policy)
        {
            throw new System.NotImplementedException();
        }

        public bool Add_Store_Manager(string Store_name, string new_manger_name, bool permision_add, bool permission_remove, bool permission_update)
        {
            throw new System.NotImplementedException();
        }

        public bool Add_Store_Owner(string Store_name, string new_owner_name)
        {
            throw new System.NotImplementedException();
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

        public bool isOwner(string store_name)
        {
            throw new System.NotImplementedException();
        }

        public bool isVistor()
        {
            return true;
        }

        public bool Open_Store(Store name)
        {
            throw new System.NotImplementedException();
        }

        public List<KeyValuePair<ProductInStore, KeyValuePair<int, double>>> Purchase_product(ProductInStore p, int amount)
        {
            throw new System.NotImplementedException();
        }

        public List<KeyValuePair<ProductInStore, KeyValuePair<int, double>>> Purchase_Store_cart(string store_name)
        {
            throw new System.NotImplementedException();
        }

        public bool Remove_Product_Store(string Store_name, string product_name)
        {
            throw new System.NotImplementedException();
        }

        public bool Remove_Store_Manager(string Store_name, string other_Manager_name)
        {
            throw new System.NotImplementedException();
        }

        public bool Remove_Store_Owner(string Store_name, string other_owner_name)
        {
            throw new System.NotImplementedException();
        }

        public bool Update_Product_Store(string Store_name, string product_name, string product_new_name, string product_new_category, double product_new_price, int product_new_amount, Discount product_new_discount, Policy product_new_policy)
        {
            throw new System.NotImplementedException();
        }

        public List<KeyValuePair<ProductInStore, int>> Watch_Cart()
        {
            throw new System.NotImplementedException();
        }
    }
}