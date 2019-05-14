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

            public bool Finalize_Purchase(string address, string payment)
            {
                if (this.single_ServiceLayer.deliverySystem.check_address(address) && this.single_ServiceLayer.paymentSystem.check_payment(payment))
                {
                    double total = this.userState.Finalize_Purchase();
                    string code = this.single_ServiceLayer.deliverySystem.sendPackage(address);
                    try
                    {
                        this.single_ServiceLayer.paymentSystem.pay(total,payment);
                    }
                    catch (Exception e)
                    {
                        this.single_ServiceLayer.deliverySystem.canclePackage(code);
                        throw e;
                    }
                    this.Add_Log("Finish Purchased with total payment of " + total + " payed in " + payment +". package '" + code +"' was sent to address - " + address);
                    return true;
                }
                return false;
            }

            public List<ProductInStore> GlobalSearch(string name, string Category, List<string> keywords, double price_min, double price_max, double Store_rank, double product_rank)
            {
                if ((name == null || Tools.check_productNames(name)) &&
                    (Category == null || Tools.check_productCategory(Category)) &&
                    (Tools.check_price(price_min) || price_min == -1) &&
                    (Tools.check_price(price_max) || price_max == -1) &&
                     (price_min <=price_max || (price_min>0 && price_max==-1)) &&
                    (Tools.check_price(Store_rank) || Store_rank==-1) &&
                    (Tools.check_price(product_rank) || product_rank==-1)
                    ) {
                    List<ProductInStore> ans = new List<ProductInStore>();
                    foreach (Store store in this.single_ServiceLayer.store)
                        ans.AddRange(store.Search(name, Category, keywords, price_min, price_max, Store_rank, product_rank));
                    string kw;
                    if (keywords == null) kw = "";
                    else kw = keywords.ToString();
                    this.Add_Log("got "+ ans.Count +" matches in Global Search with parameters: name : " +name+" ,category :"+ Category+" ,keywords :" + kw  + " ,minimum price :" +price_min +" ,maximum price :" +price_max + " ,store rank :"+ Store_rank + " ,product rank :" +product_rank);
                    return ans;
                }
                throw new Exception("one or more of the parameters is wrong");
            }

            public bool Login(string user_name, string user_pass)
            {
                if (!this.userState.isVistor()) throw new Exception("you are already logedin");
                if (Tools.check_username(user_name) && Tools.check_password(user_pass))
                {
                    foreach (Member member in single_ServiceLayer.members)
                    {
                        if (member.check(user_name, user_pass))
                        {
                            this.userState = member;
                            single_ServiceLayer.members.Remove(member);
                            this.Add_Log("Logged In");
                            return true;
                        }
                    }
                    throw new Exception("user not found");
                }
                return false;
            }

            public bool Logout()
            {
                if (this.userState.isMember())
                {
                    
                    single_ServiceLayer.members.Add((Member)this.userState);
                    this.Add_Log("logout");
                    this.userState = new Visitor();
                    return true;
                }
                throw new Exception("you are not logedin");
            }

            public bool Open_Store(string name)
            {
                if (this.userState.isVistor()) throw new Exception("you can't open the store if not logedin");
                if (Tools.check_storeName(name))
                {
                    foreach (Store store in this.single_ServiceLayer.store)
                    {
                        if (store.isMe(name)) throw new Exception("name is allready in use");
                    }
                    Store newstore = new Store(name);
                    bool ans = this.userState.Open_Store(newstore);
                    this.single_ServiceLayer.store.Add(newstore);
                    if (ans) this.Add_Log("opened new store named - " +name);
                    return ans;
                }
                return false;
            }

            public List<KeyValuePair<ProductInStore, KeyValuePair<int, double>>> Purchase_product(ProductInStore p, int amount)
            {
                if (isProductInStore(p) && Tools.check_amount(amount))
                {
                    List < KeyValuePair < ProductInStore, KeyValuePair<int, double> >> ans = this.userState.Purchase_product(p, amount);
                    this.Add_Log("saved for purching [" + amount + "]" + p.getName() + " from store " + p.getStore().getName()) ;
                    return ans;
                }
                return null;
            }

            public List<KeyValuePair<ProductInStore, KeyValuePair<int, double>>> Purchase_Store_cart(string store_name)
            {
                if (Tools.check_storeName(store_name))
                {
                    List<KeyValuePair<ProductInStore, KeyValuePair<int, double>>> ans = this.userState.Purchase_Store_cart(store_name);
                    string log = "in store " + store_name + " saved for purching:";
                    foreach(KeyValuePair<ProductInStore, KeyValuePair<int, double>> p in ans) {
                        log += "[" + p.Value.Key + "]" + p.Key.getName()+", ";
                    }
                    this.Add_Log(log);
                    return ans;
                }
                return null;
            }

            public bool Register(string user_name, string user_pass)
            {
                if (Tools.check_username(user_name) && Tools.check_password(user_pass))
                {
                    if (this.userState.isVistor())
                    {
                        bool ans = false;
                        foreach (User_ServiceLayer user in single_ServiceLayer.users)
                        {
                            if (user.userState.isMember())
                            {
                                if (((Member)user.userState).isMe(user_name))
                                {
                                    ans = true;
                                }
                            }
                        }
                        foreach (Member member in single_ServiceLayer.members)
                        {
                            if (member.isMe(user_name))
                            {
                                ans = true;
                            }
                        }
                        if (ans) throw new Exception("the user name is allready in use");
                        this.single_ServiceLayer.members.Add(new Member(user_name, user_pass));
                        this.Add_Log("Registerd new user with name '" + user_name + "'");
                        return true;
                    }
                    throw new Exception("you are allready logedin");
                }
                return false;
            }

            public bool Remove_Product_Store(string Store_name, string product_name)
            {
                if (Tools.check_storeName(Store_name) && Tools.check_productNames(product_name))
                {
                    bool ans = this.userState.Remove_Product_Store(Store_name, product_name);
                    if (ans) Add_Log("removed product '"+product_name+"' from store '" + Store_name +"'");
                    return ans;
                }
                return false;
            }

            public bool Remove_Store_Manager(string Store_name, string other_Manager_name)
            {
                if (Tools.check_storeName(Store_name) && Tools.check_username(other_Manager_name))
                {
                    bool ans= this.userState.Remove_Store_Manager(Store_name, this.GetMember(other_Manager_name));
                    if (ans) this.Add_Log("removed the assigndment of '" +other_Manager_name+ "' as manager in store '" + Store_name + "'");
                    return ans;
                }
                return false;
            }

            public bool Remove_Store_Owner(string Store_name, string other_owner_name)
            {
                if (Tools.check_storeName(Store_name) && Tools.check_username(other_owner_name))
                {
                    bool ans = this.userState.Remove_Store_Owner(Store_name, this.GetMember(other_owner_name));
                    if (ans) this.Add_Log("removed the assigndment of '" + other_owner_name + "' as owner in store '" + Store_name + "'");
                    return ans;
                }
                return false;
            }

            public bool Remove_User(string other_user)
            {
                if (Tools.check_username(other_user))
                {
                    if (this.userState.isAdmin())
                    {

                        if (((Member)this.userState).isMe(other_user)) throw new Exception("You can't remove yourself");
                        foreach (User_ServiceLayer user in single_ServiceLayer.users)
                        {
                            if (user.userState.isMember())
                            {
                                if (((Member)user.userState).isMe(other_user))
                                {
                                    Member tmp = (Member)user.userState;
                                    user.Logout();
                                    single_ServiceLayer.members.Remove(tmp);
                                    this.Add_Log("removed '" + other_user + "' from the system");
                                    return true;
                                }
                            }
                        }
                        foreach (Member member in single_ServiceLayer.members)
                        {
                            if (member.isMe(other_user))
                            {
                                single_ServiceLayer.members.Remove(member);
                                this.Add_Log("removed '" + other_user + "' from the system");
                                return true;
                            }
                        }
                        throw new Exception("user not found");
                    }
                    throw new Exception("the user is not an Admin");
                }
                return false;
            }

            public bool Update_Product_Store(string Store_name, string product_name, string product_new_name, string product_new_category, double product_new_price, int product_new_amount, Discount product_new_discount, Policy product_new_policy)
            {
                if (Tools.check_storeName(Store_name) &&
                    Tools.check_productNames(product_name) &&
                    Tools.check_productNames(product_new_name) &&
                    Tools.check_productCategory(product_new_category) &&
                    Tools.check_price(product_new_price) &&
                    Tools.check_amount(product_new_amount)
                    )
                {
                    bool ans = false;
                    if (product_new_amount > 0)
                    {
                        ans = this.userState.Update_Product_Store(Store_name, product_name, product_new_name, product_new_category, product_new_price, product_new_amount, product_new_discount, product_new_policy);
                        if (ans) this.Add_Log("in store '" + Store_name + "' updated '" + product_name + "' - new name:'" + product_new_name + "',new category:'" + product_new_category + "' ,new price:" + product_new_price + " ,new amount:" + product_new_amount + " , new discount:" + product_new_discount.ToString() + " ,new policy:" + product_new_policy.ToString());
                    }
                    else
                        ans = this.Remove_Product_Store(Store_name, product_name);
                    return ans;
                }
                return false;
            }

            public List<KeyValuePair<ProductInStore, int>> Watch_Cart()
            {
                List < KeyValuePair<ProductInStore, int> > ans = this.userState.Watch_Cart();
                ans.Sort(new cartOrder());
                this.Add_Log("watched his cart");
                return ans;
            }

            public string get_log()
            {
                if (this.userState.isAdmin())
                {
                    string ans = "";
                    for(int i=0; i<singleton.log.Count; i++)
                    {
                        ans += singleton.log[i] + "\n";
                    }
                    return ans;
                }
                throw new Exception("only admins can view the log");
            }

            public override string ToString()
            {
                if (this.userState.isVistor())
                {
                    return "Visitor";
                }
                else
                {
                    return ((Member)this.userState).name;
                }
            }


            ///////////////////////////////////////////////////////////////////////////////////////////////////////////////
            ///////////////////////////////////////////////////////////////////////////////////////////////////////////////
            //////////                                                                                           //////////
            //////////  ////////    ////////    ///////   ///       ///    //////      ///////////  ///////////  //////////
            //////////  ///   ///   ///   ///     ///      ///     ///    ///  ///    ///////////   ///    ///   //////////
            //////////  ///   ///   ///   ///     ///       ///   ///    ///    ///       ///       ///...       //////////
            //////////  ///////     ///////       ///        /// ///    ////////////      ///       ///'''       //////////
            //////////  ///         ///  ///      ///         /////    ////      ////     ///       ///     ///  //////////
            //////////  ///         ///   ///   ///////       ////    ////        ////    ///       //////////   //////////
            //////////                                                                                           //////////
            ///////////////////////////////////////////////////////////////////////////////////////////////////////////////
            ///////////////////////////////////////////////////////////////////////////////////////////////////////////////

            private class cartOrder : IComparer<KeyValuePair<ProductInStore, int>>
            {
                public int Compare(KeyValuePair<ProductInStore, int> x, KeyValuePair<ProductInStore, int> y)
                {
                    return x.Key.getStore().getName().CompareTo(y.Key.getStore().getName());
                }
            }

            private bool isProductInStore(ProductInStore p)
            {
                try
                {
                    p.getStore().FindProductInStore(p.getProduct().getName());
                }
                catch
                {
                    throw new Exception("Product is not in the store");
                }
                return true;
            }

            private Member GetMember(string Username)
            {
                Member ans = null;
                foreach (User_ServiceLayer user in single_ServiceLayer.users)
                {
                    if (user.userState.isMember())
                    {
                        if (((Member)user.userState).isMe(Username))
                        {
                            ans = (Member)user.userState;
                        }
                    }
                }
                if (ans == null)
                {
                    foreach (Member member in single_ServiceLayer.members)
                    {
                        if (member.isMe(Username))
                        {
                            ans = member;
                        }
                    }
                }
                if (ans == null) throw new Exception("no user was found");
                return ans;
            }

            
        }
    }
}
