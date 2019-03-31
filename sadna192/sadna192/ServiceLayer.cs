using System;
using System.Timers;
using System.Collections.Generic;

namespace sadna192
{
    public class ServiceLayer : I_ServiceLayer
    {
        private static single_ServiceLayer singleton=null;

        public ServiceLayer()
        {
            // do nothing...
        }

        public I_User_ServiceLayer Connect()
        {
            if (singleton == null) throw new Exception("the system dosnwt exist");
            return singleton.Connect();
        }

        public I_ServiceLayer Create_ServiceLayer(I_DeliverySystem deliverySystem, I_PaymentSystem paymentSystem, string admin_name, string admin_pass)
        {
            if (singleton == null)
            {
                singleton = new single_ServiceLayer(deliverySystem, paymentSystem, admin_name, admin_pass);
                return this;
            }
            else throw new Exception("the system already exist");
        }

        private static void OnTimedEvent(Object source, ElapsedEventArgs e)
        {
            if (singleton != null)
            {
                foreach(Member m in singleton.members)
                {
                    if (m.shopingBasket.toBeRemoved)
                    {
                        if (m.shopingBasket.savedProducts != null)
                        {
                            m.shopingBasket.returnProducts();
                            singleton.log.Add(System.DateTime.Now.ToString() + "SYSTEM : returned to store saved products of user '" + m.name +"'" );
                        }
                        m.shopingBasket.toBeRemoved = false;
                    }
                    else
                    {
                        m.shopingBasket.toBeRemoved = (m.shopingBasket.savedProducts != null);
                    }
                }
            }
            
        }


        private class single_ServiceLayer
        {
            public List<I_User_ServiceLayer> users;
            public List<Member> members;
            protected static ServiceLayer singleton = null;
            public I_DeliverySystem deliverySystem;
            public I_PaymentSystem paymentSystem;
            public List<string> log;
            public List<Store> store=new List<Store>();

            public single_ServiceLayer(I_DeliverySystem deliverySystem, I_PaymentSystem paymentSystem, string admin_name, string admin_pass)
            {
                if (!deliverySystem.Connect() || !paymentSystem.Connect()) throw new Exception("can't access external systems");
                this.deliverySystem = deliverySystem;
                this.paymentSystem = paymentSystem;
                this.members = new List<Member>();
                this.users = new List<I_User_ServiceLayer>();
                this.log = new List<string>();
                Timer t = new Timer(1000*60*10);
                t.Elapsed += OnTimedEvent;
                t.AutoReset = true;
                t.Enabled = true;

                if (!Tools.check_username(admin_name) || !Tools.check_password(admin_pass)) throw new Exception("invalid admin details");
                members.Add(new Admin(admin_name, admin_pass));
            }

            internal I_User_ServiceLayer Connect()
            {
                I_User_ServiceLayer ans = new User_ServiceLayer(this);
                this.users.Add(ans);
                return ans;
            }
        }

        private class User_ServiceLayer:I_User_ServiceLayer
        {
            private single_ServiceLayer single_ServiceLayer;
            private UserState userState;
            
            public User_ServiceLayer(single_ServiceLayer single_ServiceLayer)
            {
                this.single_ServiceLayer = single_ServiceLayer;
                this.userState = new Visitor();
            }

            public void Add_Log(string log)
            {
                this.single_ServiceLayer.log.Add(System.DateTime.Now.ToString() + this.userState.ToString() + ":" + log);
            }

            public bool Add_Product_Store(string Store_name, string product_name, string product_category, double product_price, int product_amount, Discount product_discount, Policy product_policy)
            {
                if (Tools.check_storeName(Store_name) &&
                    Tools.check_productNames(product_name) &&
                    Tools.check_productCategory(product_category) &&
                    Tools.check_price(product_price) &&
                    Tools.check_amount(product_amount)
                )
                {
                    bool ans = this.userState.Add_Product_Store(Store_name, product_name, product_category, product_price, product_amount, product_discount, product_policy);
                    string p, d;
                    if (product_discount == null) d = "";
                    else d = product_discount.ToString();
                    if (product_policy == null) p = "";
                    else p = product_policy.ToString();
                    if (ans) this.Add_Log("in store " + Store_name + " Added Product" + product_name + " ,category - " + product_category + " ,price - " + product_price + " ,amount - " + product_amount + " ,policy - " + p + " ,discount - "+d);
                    return ans;
                }
                throw new Exception("one of the parameters was wrong");
            }

            public bool Add_Store_Manager(string Store_name, string new_manger_name, bool permision_add, bool permission_remove, bool permission_update)
            {
                if (Tools.check_storeName(Store_name) && Tools.check_username(new_manger_name))
                {
                    if (!this.userState.isOwner(Store_name)) throw new Exception("you are not an owner of this store");
                    Member other_user = this.GetMember(new_manger_name);
                    if (other_user == null) throw new Exception("new Store manager was not found");
                    bool ans = this.userState.Add_Store_Manager(Store_name,other_user, permision_add, permission_remove, permission_update);
                    this.Add_Log("in store "+ Store_name + "Assgined " + new_manger_name + " as new Manager with the permisions: Add- " +permision_add+" ,Remove - " +permission_remove+ " ,update - "+ permission_update);
                    return ans;
                }
                return false;
            }

            public bool Add_Store_Owner(string Store_name, string new_owner_name)
            {
                if (Tools.check_username(new_owner_name) && Tools.check_storeName(Store_name))
                {
                    Member other_user = this.GetMember(new_owner_name);
                    if (other_user.isOwner(Store_name)) throw new Exception(new_owner_name+"is allready owner of the store");
                    if (other_user == null) throw new Exception("new Store owner was not found");
                    bool ans= this.userState.Add_Store_Owner(Store_name, other_user);
                    if (ans) this.Add_Log("added the user " + new_owner_name +" as owner in the store " + Store_name);
                    return ans;
                }
                return false;
            }

            public bool Add_To_ShopingBasket(ProductInStore p, int amount)
            {
                if (isProductInStore(p) && Tools.check_amount(amount))
                {
                    bool ans = this.userState.Add_To_ShopingBasket(p, amount);
                    if (ans) this.Add_Log("Added ["+amount+"]"+ p.getName() +" from the store " + p.getStore().getName() + " to his shopping basket");
                    return ans;
                }
                return false;
            }

            public bool Edit_Product_In_ShopingBasket(ProductInStore p, int amount)
            {
                if (Tools.check_amount(amount))
                {
                    try { 
                    if (isProductInStore(p))
                    {
                        bool ans = this.userState.Edit_Product_In_ShopingBasket(p, amount);
                        if (ans) this.Add_Log("changed th amount of " + p.getName() + " from store " + p.getStore().getName() + " to " + amount);
                        return ans;
                    }
                    else
                    {
                        bool ans = this.userState.Edit_Product_In_ShopingBasket(p, 0);
                        if (ans) this.Add_Log("changed th amount of " + p.getName() + " from store " + p.getStore().getName() + " to " + amount);
                        return ans;
                    }
                    }
                    catch
                    {
                        bool ans = this.userState.Edit_Product_In_ShopingBasket(p, 0);
                        if (ans) this.Add_Log("changed th amount of " + p.getName() + " from store " + p.getStore().getName() + " to " + 0);
                        throw new Exception("product was removed from store");
                    }
                }
                throw new Exception("unvalid amount");
            }

        // RSL 7
        void Add_Log(string log);
        ////////
    }
}
