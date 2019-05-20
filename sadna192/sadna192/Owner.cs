using System;
using System.Collections.Generic;

namespace sadna192
{
    internal class Owner
    {
        private Store store;
        private Member user;
        private List<Owner> has_Apointed;

        public Owner(Member u, Store s){
            this.store = s;
            this.user = u;
            s.addOwner(this);

            this.has_Apointed = new List<Owner>(); 
        }

        internal Store getStore()
        {
            return this.store;
        }

        internal virtual bool addProduct(string product_name, string product_category, double product_price, int product_amount, Discount product_discount, Policy product_policy)
        {
            return this.store.addProduct(product_name, product_category, product_price, product_amount, product_discount, product_policy);
        }

        internal virtual bool addManager(Member new_manager, bool permision_add, bool permission_remove, bool permission_update)
        {
            try
            {
                foreach (Owner o in new_manager.owner)
                {
                    if (o.store.isMe(this.store.getName()) && o is Manager) {
                        ((Manager)o).permision_add = ((Manager)o).permision_add || permision_add;
                        ((Manager)o).permision_remove = ((Manager)o).permision_remove || permission_remove;
                        ((Manager)o).permision_update = ((Manager)o).permision_update || permission_update;
                        return true;
                    }
                }
                throw new Sadna192Exception("add manager faild. <Owner: addManager>");
            }
            catch
            {
                Manager nm = new Manager(new_manager, this.store, permision_add, permission_remove, permission_update);
                new_manager.owner.Add(nm);
                this.has_Apointed.Add(nm);
                return true;
            }
        }

        internal virtual bool addOwner(string store_name, Member new_owner)
        {
            Owner nm = new Owner(new_owner, this.store);
            new_owner.owner.Add(nm);
            this.has_Apointed.Add(nm);
            return true;
        }

        internal virtual bool removeProduct(string product_name)
        {
            return this.store.removeProduct(product_name);
        }

        private  bool removeMe()
        {
            this.store.removeApointed(this);
            this.user.owner.Remove(this);
            foreach (Owner apointed in this.has_Apointed) apointed.removeMe();
            return true;
        }

        internal virtual bool removeApointed(Member other_Owner)
        {
            Owner o = this.findOwner(other_Owner);
            return o.removeMe();
        }

        private Owner findOwner(Member other)
        {
            if (this.user.isMe(other)) return this;
            foreach(Owner apointed in this.has_Apointed)
            {
                try
                {
                    return apointed.findOwner(other);
                }
                catch (Exception) { }
            }
            throw new Sadna192Exception("the Other member was not assigned by this Owner. <Owner: findOwner>");
        }

        internal virtual bool updateProduct(string product_name, string product_new_name, string product_new_category, double product_new_price, int product_new_amount, Discount product_new_discount, Policy product_new_policy)
        {
            return this.store.updateProduct(product_name, product_new_name, product_new_category, product_new_price, product_new_amount, product_new_discount, product_new_policy);
        }

        internal virtual bool removeManager(Member other_Manager)
        {
            Owner other = findOwner(other_Manager);
            if (other is Manager) return other.removeMe();
            throw new Sadna192Exception("the member is an owner of the shop. <Owner: removeManager> ");
        }

        internal virtual bool removeOwner(Member other_Owner)
        {
            Owner other = findOwner(other_Owner);
            if (other is Owner && !(other is Manager)) return other.removeMe();
            throw new Sadna192Exception("the member is an manager of the shop. <Owner: removeOwner>");
        }
    }
}