namespace sadna192
{
    internal class Manager : Owner
    {
        private bool permision_add;
        private bool permision_remove;
        private bool permision_update;
        public Manager(Member u, Store s, bool permision_add, bool permission_remove, bool permission_update) : base(u, s)
        {
            this.permision_add = permision_add;
            this.permision_remove = permission_remove;
            this.permision_update = permission_update;
        }

        internal override bool addProduct(string product_name, string product_category, double product_price, int product_amount, Discount product_discount, Policy product_policy)
        {
            if (!this.permision_add) throw new System.Exception("this manager is not alowed to add products");
            return base.addProduct(product_name, product_category, product_price, product_amount, product_discount, product_policy);
        }

        internal override bool addManager(Member new_manager, bool permision_add, bool permission_remove, bool permission_update)
        {
            throw new System.Exception("Manager can't assign other Managers");
        }

        internal override bool addOwner(string store_name, Member new_owner)
        {
            throw new System.Exception("Manager can't assign Owners");
        }

        internal override bool removeProduct(string product_name)
        {
            if (!this.permision_remove) throw new System.Exception("this manager is not alowed to remove products");
            return base.removeProduct(product_name);
        }

        internal override bool removeApointed(Member other_Owner)
        {
            throw new System.Exception("Manager can't remove other managers or owners");
        }

        internal override bool updateProduct(string product_name, string product_new_name, string product_new_category, double product_new_price, int product_new_amount, Discount product_new_discount, Policy product_new_policy)
        {
            if (!this.permision_update) throw new System.Exception("this manager is not alowed to update products");
            return base.updateProduct(product_name, product_new_name, product_new_category, product_new_price, product_new_amount, product_new_discount, product_new_policy);
        }

        internal override bool removeManager(Member other_Manager)
        {
            throw new System.Exception("Manager can't remove Managers");
        }

        internal override bool removeOwner(Member other_Owner)
        {
            throw new System.Exception("Manager can't remove Owners");
        }
    }
}