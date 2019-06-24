using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("sadna192_Tests.Unit_Tests")]

namespace sadna192
{
    
    public class Manager : Owner
    {
        public int id { set; get; }
        public bool permision_add { get;  set; }
        public bool permision_remove { get;  set; }
        public bool permision_update { get;  set; }

        public Manager() { }

        public Manager(Member u, Store s, bool permision_add, bool permission_remove, bool permission_update) : base(u, s)
        {
            base.waiting_to_aprove.Clear();
            this.permision_add = permision_add;
            this.permision_remove = permission_remove;
            this.permision_update = permission_update;
        }

        internal override bool addProduct(string product_name, string product_category, double product_price, int product_amount, Discount product_discount, Policy product_policy)
        {
            if (!this.permision_add) throw new Sadna192Exception("this manager is not alowed to add products","Manager" ,"addProduct");
            return base.addProduct(product_name, product_category, product_price, product_amount, product_discount, product_policy);
        }

        internal override bool addManager(Member new_manager, bool permision_add, bool permission_remove, bool permission_update)
        {
            throw new Sadna192Exception("Manager can't assign other Managers", "Manager", "addManager");
        }

        internal override bool addOwner(string store_name, Member new_owner)
        {
            throw new Sadna192Exception("Manager can't assign Owners", "Manager", "addOwner");
        }

        internal override bool removeProduct(string product_name)
        {
            if (!this.permision_remove) throw new Sadna192Exception("this manager is not alowed to remove products", "Manager", "removeProduct");
            return base.removeProduct(product_name);
        }

        internal override bool removeApointed(Member other_Owner)
        {
            throw new Sadna192Exception("Manager can't remove other managers or owners", "Manager", "removeApointed");
        }

        internal override bool updateProduct(string product_name, string product_new_name, string product_new_category, double product_new_price, int product_new_amount, Discount product_new_discount, Policy product_new_policy)
        {
            if (!this.permision_update) throw new Sadna192Exception("this manager is not alowed to update products", "Manager", "updateProduct");
            return base.updateProduct(product_name, product_new_name, product_new_category, product_new_price, product_new_amount, product_new_discount, product_new_policy);
        }

        internal override bool removeManager(Member other_Manager)
        {
            throw new Sadna192Exception("Manager can't remove Managers", "Manager", "removeManager");
        }

        internal override bool removeOwner(Member other_Owner)
        {
            throw new Sadna192Exception("Manager can't remove Owners", "Manager", "removeOwner");
        }

        public override bool isManger() => true;

        internal override bool addShopdiscount(Discount dis)
        {
            if(this.permision_update)
                return this.store.setDiscount(dis);
            return false;
        }

        internal bool addShopPolicy(Policy dis)
        {
            if (this.permision_update)
                return this.store.setPolicy(dis);
            return false;
        }
    }
}