using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace sadna192
{
    public class Owner
    {
        public int id { get; set; }
        public int storeRef { get; set; }
        [ForeignKey("storeRef")]
        public Store store { get; set; }
        public int userRef { get; set; }
        [ForeignKey("userRef")]
        public Member user { get; set; }

        public Owner(Member u, Store s){
            this.waiting_to_aprove = new List<Owner>();
            
            foreach (Owner o in s.GetOwners())
            {
                if (!o.isManger() && o.waiting_to_aprove.ToArray().Length==0)
                {
                    this.waiting_to_aprove.Add(o);
                }
            }
            this.storeRef = s.storeID;           
            this.userRef = u.id;         
            s.addOwner(this);
        }

        internal Store getStore()
        {
            return this.store;
        }

        internal bool are_assigned()
        {
            if (this.waiting_to_aprove.ToArray().Length == 0)
                return true;
            throw new Exception("you are still waiting for aproval from other owners of the store");
        }

        internal virtual bool addProduct(string product_name, string product_category, double product_price, int product_amount, Discount product_discount, Policy product_policy)
        {
            this.are_assigned();
            return this.store.addProduct(product_name, product_category, product_price, product_amount, product_discount, product_policy);
        }

        internal virtual bool addManager(Member new_manager, bool permision_add, bool permission_remove, bool permission_update)
        {
            this.are_assigned();
            try
            {
                foreach (Owner o in new_manager.owner)
                {
                    if (o.store.isMe(this.store.getName()) && o is Manager){
                        ((Manager)o).permision_add = ((Manager)o).permision_add || permision_add;
                        ((Manager)o).permision_remove = ((Manager)o).permision_remove || permission_remove;
                        ((Manager)o).permision_update = ((Manager)o).permision_update || permission_update;
                        return true;
                    }
                }
                throw new Sadna192Exception("add manager faild", "Owner", "addManager");
            }
            catch
            {
                Manager nm = new Manager(new_manager, this.store, permision_add, permission_remove, permission_update);
                new_manager.owner.Add(nm);
                this.has_Apointed.Add(nm);
                if (!DBAccess.SaveToDB(nm))
                    DBAccess.DBerror("could not save manager to DB");
                return true;
            }
        }

        internal virtual bool addOwner(string store_name, Member new_owner)
        {
            this.are_assigned();
            Owner nm = new Owner(new_owner, this.store);
            if (new_owner.owner == null)
                new_owner.owner = new List<Owner>();
            new_owner.owner.Add(nm);       
            if (!DBAccess.SaveToDB(nm))
                DBAccess.DBerror("could not save owner to DB");
            if (this.has_Apointed == null)
                this.has_Apointed = new List<Owner>();
            nm.waiting_to_aprove.Remove(this);
            foreach (Owner o in nm.waiting_to_aprove)
            {
                o.user.alerts.Add(this.user.name + " wants to appoint " + new_owner + " as an owner and is waiting for your approval");
            }
            new_owner.owner.Add(nm);
            this.has_Apointed.Add(nm);
            return true;
        }

        internal virtual bool removeProduct(string product_name)
        {
            this.are_assigned();
            return this.store.removeProduct(product_name);
        }

        private  bool removeMe()
        {
      
            this.user.owner.Remove(this);
            this.store.owners.Remove(this);
            return DBAccess.removeOwnerFromDB(this.id);
        }

        internal virtual bool removeApointed(Member other_Owner)
        {
            this.are_assigned();
            Owner o = this.findOwner(other_Owner);
            return o.removeMe();
        }

        private Owner findOwner(Member other)
        {
            this.are_assigned();
            if (this.user.isMe(other) && this.isManger()) return this;
            foreach(Owner apointed in this.store.GetOwners())
            {
                try
                {
                    return apointed.findOwner(other);
                }
                catch (Exception) { }
            }
            throw new Sadna192Exception("the Other member was not assigned by this Owner", "Owner", "findOwner");
        }

        internal virtual bool updateProduct(string product_name, string product_new_name, string product_new_category, double product_new_price, int product_new_amount, Discount product_new_discount, Policy product_new_policy)
        {
            this.are_assigned();
            return this.store.updateProduct(product_name, product_new_name, product_new_category, product_new_price, product_new_amount, product_new_discount, product_new_policy);
        }

        internal virtual bool removeManager(Member other_Manager)
        {
            this.are_assigned();
            Owner other = findOwner(other_Manager);
            if (other is Manager) return other.removeMe();
            throw new Sadna192Exception("the member is an owner of the shop", "Owner", "removeManager");
        }

        internal virtual bool removeOwner(Member other_Owner)
        {
            /*
            this.are_assigned();
            Owner other = findOwner(other_Owner);
            if (other is Owner && !(other is Manager)) return other.removeMe();
            throw new Sadna192Exception("the member is an manager of the shop", "Owner", "removeOwner");
            */
            throw new Sadna192Exception("You can't remove Owners", "Owner", "removeOwner");
        }

        public virtual bool isManger() => false;

        public string GetUsername()
        {
            return user.name;
        }

        public List<string> waiting_for_owner_approval()
        {
            List<string> ans = new List<string>();
            foreach(Owner o in this.waiting_to_aprove)
            { 
                ans.Add(o.user.name);
            }
            return ans;
        }

        internal virtual bool addShopdiscount(Discount dis)
        {
            this.are_assigned();
            return this.store.setDiscount(dis);
        }

        public bool approveAssignmet(string name, bool ans)
        {
            Owner toapprove = null;
            foreach (Owner o in this.store.GetOwners())
            {
                if (o.user.name == name) toapprove = o;
            }

            if (toapprove == null) throw new Exception("user was not found");

            if (ans)
            {
                toapprove.waiting_to_aprove.Remove(this);
                if (toapprove.waiting_to_aprove.ToArray().Length == 0)
                {
                    if (toapprove.isManger())
                        toapprove.user.alerts.Add("You where approved as Mannager in the store " + this.store.getName());
                    else
                        toapprove.user.alerts.Add("You where approved as Owner in the store " + this.store.getName());
                }
            }
            else
            {
                foreach (Owner o in this.store.GetOwners())
                {
                    if (!o.isManger()) o.user.alerts.Add("the apointment of " + toapprove.user.name + " was disapproved by " + this.user.name);
                }
                toapprove.removeMe();
            }
            return true;
        }

        internal bool addShopPolicy(Policy dis)
        {
            this.are_assigned();
            return this.store.setPolicy(dis);
        }
    }
}