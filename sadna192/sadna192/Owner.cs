using System;

namespace sadna192
{
    internal class Owner
    {
        private Store store;
        private UserState user;

        public Owner(UserState u, Store s){
            this.store = s;
            this.user = u;
            s.addOwner(this);
        }

        internal Store getStore()
        {
            throw new NotImplementedException();
        }

        internal bool addProduct(string product_name, string product_category, double product_price, int product_amount, Discount product_discount, Policy product_policy)
        {
            throw new NotImplementedException();
        }

        internal bool addManager(Member new_manager, bool permision_add, bool permission_remove, bool permission_update)
        {
            throw new NotImplementedException();
        }

        internal bool addOwner(string store_name, Member new_owner_name)
        {
            throw new NotImplementedException();
        }

        internal bool removeProduct(string product_name)
        {
            throw new NotImplementedException();
        }

        internal bool removeManager(Member other_Manager)
        {
            throw new NotImplementedException();
        }

        internal bool removeOwner(Member other_Owner)
        {
            throw new NotImplementedException();
        }

        internal bool updateProduct(string product_name, string product_new_name, string product_new_category, double product_new_price, int product_new_amount, Discount product_new_discount, Policy product_new_policy)
        {
            throw new NotImplementedException();
        }
    }
}