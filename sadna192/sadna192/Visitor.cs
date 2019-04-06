using System.Collections.Generic;
using System;

namespace sadna192
{
    internal class Visitor : UserState
    {
        private ShopingBasket shopingBasket;

        public Visitor()
        {
            this.shopingBasket = new ShopingBasket();
        }

        public virtual bool Add_Product_Store(string Store_name, string product_name, string product_category, double product_price, int product_amount, Discount product_discount, Policy product_policy)
        {
            throw new Exception("User must login to add a product to a store"); 
        }

        public virtual bool Add_Store_Manager(string Store_name, Member new_manger_name, bool permision_add, bool permission_remove, bool permission_update)
        {
            throw new Exception("User must login to add a manager to a store");
        }

        public virtual bool Add_Store_Owner(string Store_name, Member new_owner_name)
        {
            throw new Exception("User must login to add a Owner to a store");
        }

        public bool Add_To_ShopingBasket(ProductInStore p, int amount)
        {
            return this.shopingBasket.addProduct(p, amount);
        }

        public bool Edit_Product_In_ShopingBasket(ProductInStore p, int amount)
        {
            return this.shopingBasket.editProductAmount(p, amount);
        }

        public bool Finalize_Purchase(string address, string payment)
        {
            return this.shopingBasket.Finalize_Purchase(address, payment);
        }

        //public List<KeyValuePair<ProductInStore, int>> get_ShopingBasket()

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
            return false;
        }

        public bool isVistor()
        {
            return true;
        }

        public bool Open_Store(Store name)
        {
            throw new Exception("User must login to םפקמ a store");
        }

        public List<KeyValuePair<ProductInStore, KeyValuePair<int, double>>> Purchase_product(ProductInStore p, int amount)
        {
            return this.shopingBasket.Purchase_product(p, amount);
        }

        public List<KeyValuePair<ProductInStore, KeyValuePair<int, double>>> Purchase_Store_cart(string store_name)
        {
            return this.shopingBasket.Purchase_Store_cart(store_name);
        }

        public virtual bool Remove_Product_Store(string Store_name, string product_name)
        {
            throw new Exception("User must login to remove a product from a store");
        }

        public virtual bool Remove_Store_Manager(string Store_name, Member other_Manager)
        {
            throw new Exception("User must login to remove a Manager from a store");
        }

        public virtual bool Remove_Store_Owner(string Store_name, Member other_owner)
        {
            throw new Exception("User must login to remove a owner from a store");
        }

        public virtual bool Update_Product_Store(string Store_name, string product_name, string product_new_name, string product_new_category, double product_new_price, int product_new_amount, Discount product_new_discount, Policy product_new_policy)
        {
            throw new Exception("User must login to update a product in a store");
        }

        public List<KeyValuePair<ProductInStore, int>> Watch_Cart()
        {
            return this.shopingBasket.get_basket();
        }
    }
}