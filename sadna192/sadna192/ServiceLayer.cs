using System;
using System.Timers;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("sadna192_Tests.Integration_Tests")]
namespace sadna192
{
    public class ServiceLayer : I_ServiceLayer
    {
        private static single_ServiceLayer singleton=null;

        public ServiceLayer()
        {
            // do nothing...
        }

        public I_User_ServiceLayer Connect(Alerter alerter)
        {
            if (singleton == null) throw new Sadna192Exception("the system dosn't exist" , "ServiceLayer" ,  "Connect");
            return singleton.Connect(alerter);
        }

        public I_ServiceLayer Create_ServiceLayer(I_DeliverySystem deliverySystem, I_PaymentSystem paymentSystem, string admin_name, string admin_pass)
        {
            if (singleton == null)
            {
                singleton = new single_ServiceLayer(deliverySystem, paymentSystem, admin_name, admin_pass);
                return this;
            }
            else throw new Sadna192Exception("the system already exist",  "ServiceLayer", "Create_ServiceLayer");
        }

        public void CleanUpSystem()
        {
            if (singleton == null) throw new Exception("CleanUp faild - the system not exist.");
            singleton.CleanUp();
            singleton = null; 
        }

        private static void OnTimedEvent(Object source, ElapsedEventArgs e)
        {
            if (singleton != null)
            {
                List<Visitor> all = new List<Visitor>();
                all.AddRange(singleton.members);
                foreach (I_User_ServiceLayer u in singleton.users)
                {
                    all.Add((Visitor)u.GetUser());
                }
                foreach (Visitor m in all)
                {
                    if (m.shopingBasket.toBeRemoved)
                    {
                        if (m.shopingBasket.savedProducts != null)
                        {
                            m.shopingBasket.returnProducts();
                            Sadna192Exception.AddToEventLog("SYSTEM : returned to store saved products of user '" + m.ToString() + "'");

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

        private static void OnTimedEvent2(Object source, ElapsedEventArgs e)
        {
            if (singleton != null)
            {
                foreach (I_User_ServiceLayer u in singleton.users)
                {
                    if (u.GetUser().isMember())
                    {
                        foreach(string s in ((Member)u.GetUser()).alerts)
                        {
                            u.alert(s);
                        }
                        ((Member)u.GetUser()).alerts.Clear();
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
            public List<Store> store=new List<Store>();

            public single_ServiceLayer(I_DeliverySystem deliverySystem, I_PaymentSystem paymentSystem, string admin_name, string admin_pass)
            {
                if (!deliverySystem.Connect() || !paymentSystem.Connect()) throw new Sadna192Exception("can't access external systems", "(ServiceLayer) single_ServiceLayer",  "single_ServiceLayer(1)");
                this.deliverySystem = deliverySystem;
                this.paymentSystem = paymentSystem;
                this.members = new List<Member>();
                this.users = new List<I_User_ServiceLayer>();
                Timer t = new Timer(1000*60*10);
                t.Elapsed += OnTimedEvent;
                t.AutoReset = true;
                t.Enabled = true;
                Timer t1 = new Timer(1000 * 30 * 1);
                t1.Elapsed += OnTimedEvent2;
                t1.AutoReset = true;
                t1.Enabled = true;

                if (!Tools.check_username(admin_name) || !Tools.check_password(admin_pass))
                    throw new Sadna192Exception("invalid admin details", "(ServiceLayer) single_ServiceLayer", "single_ServiceLayer(2)");
                Admin admin = Admin.GetAdmin(admin_name, admin_pass);
                members.Add(admin);
            }

            internal I_User_ServiceLayer Connect(Alerter alerter)
            {
                I_User_ServiceLayer ans = new User_ServiceLayer(this, alerter);
                this.users.Add(ans);
                return ans;
            }

            internal void CleanUp()
            {
                users = null;
                members = null;
                singleton = null;
                deliverySystem = null;
                paymentSystem = null;
                store = null;
            }

            internal bool alert_user(string name, string messege)
            {
                foreach (I_User_ServiceLayer u in users)
                {
                    if (u.GetUserState().isMember() && ((Member)u).name == name)
                    {
                        return u.alert(messege);
                    }
                }

                foreach (Member m in members)
                {
                    if (m.name == name)
                    {
                        m.alerts.Add(messege);
                        return true;
                    }
                }
                return false;
            }
        }

       
        private class User_ServiceLayer:I_User_ServiceLayer
        {
            private single_ServiceLayer single_ServiceLayer;
            public UserState userState;
            private Alerter alerter;
            
            public User_ServiceLayer(single_ServiceLayer single_ServiceLayer, Alerter alerter)
            {
                this.single_ServiceLayer = single_ServiceLayer;
                this.userState = new Visitor();
                this.alerter = alerter;
            }

            public UserState GetUser()
            {
                return this.userState.Copy();
            }

            public void Add_Log(string log)
            {
                Sadna192Exception.AddToEventLog(this.userState.ToString() + ":" + log);
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
                throw new Sadna192Exception("one of the parameters was wrong", "(ServiceLayer) User_ServiceLayer", "Add_Product_Store");
            }

            public bool Add_Store_Manager(string Store_name, string new_manger_name, bool permision_add, bool permission_remove, bool permission_update)
            {
                if (Tools.check_storeName(Store_name) && Tools.check_username(new_manger_name))
                {
                    if (!this.userState.isOwner(Store_name)) throw new Sadna192Exception("you are not an owner of this store", "(ServiceLayer) User_ServiceLayer", " Add_Store_Manager(1)");
                    Member other_user = this.GetMember(new_manger_name);
                    if (other_user == null) throw new Sadna192Exception("new Store manager was not found", "(ServiceLayer) User_ServiceLayer", "Add_Store_Manager(2)");
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
                    Member other_user = DBAccess.getMemberFromDB(new_owner_name); //this.GetMember(new_owner_name);
                    if (other_user == null)
                        throw new Sadna192Exception("new Store owner was not found", "(ServiceLayer) User_ServiceLayer", "Add_Store_Owner(2)");
                    if (DBAccess.MemberIsOwner(other_user.name, Store_name)) //(other_user.isOwner(Store_name)) 
                        throw new Sadna192Exception(new_owner_name + "is allready owner of the store", "(ServiceLayer) User_ServiceLayer", "Add_Store_Owner(1)");
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
                        throw new Sadna192Exception("product was removed from store", "(ServiceLayer) User_ServiceLayer", "Edit_Product_In_ShopingBasket(1)");
                    }
                }
                throw new Sadna192Exception("unvalid amount", "(ServiceLayer) User_ServiceLayer", "Edit_Product_In_ShopingBasket(2)");
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
                    /*
                    foreach (Store store in this.single_ServiceLayer.store)
                        foreach (ProductInStore p in store.Search(name, Category, keywords, price_min, price_max, Store_rank, product_rank))
                            ans.Add(new ProductInStore(p, new Store(p.getStore())));
                            */
                    List <ProductInStore> tmp = DBAccess.searchProductInStore(name, Category, keywords, price_min, price_max, product_rank);
                    foreach (ProductInStore p in tmp)
                        ans.Add(new ProductInStore(p, new Store(p.getStore())));
                    string kw;
                    if (keywords == null) kw = "";
                    else kw = keywords.ToString();
                    this.Add_Log("got "+ ans.Count +" matches in Global Search with parameters: name : " +name+" ,category :"+ Category+" ,keywords :" + kw  + " ,minimum price :" +price_min +" ,maximum price :" +price_max + " ,store rank :"+ Store_rank + " ,product rank :" +product_rank);
                    return ans;
                }
                throw new Sadna192Exception("one or more of the parameters is wrong", "(ServiceLayer) User_ServiceLayer", "GlobalSearch>");
            }

            public bool Login(string user_name, string user_pass)
            {
                if (!this.userState.isVistor()) throw new Sadna192Exception("you are already logedin", "(ServiceLayer) User_ServiceLayer", "Login(1)");
                if (Tools.check_username(user_name) && Tools.check_password(user_pass))
                {
                    Member member = DBAccess.loginCheck(user_name, user_pass);
                    if (member != null)
                     {
                        this.userState = member;
                            single_ServiceLayer.members.Remove(member);
                            this.Add_Log("Logged In");
                            foreach (string s in member.alerts)
                            {
                                alerter.AlertUser(s);
                            }
                            member.alerts.Clear();
                            return true;
                     }
                    
                    throw new Sadna192Exception("user not found", "(ServiceLayer) User_ServiceLayer", "Login(2)");
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
                throw new Sadna192Exception("you are not logedin", "(ServiceLayer) User_ServiceLayer", "Logout");
            }

            public bool Open_Store(string name)
            {
                if (this.userState.isVistor()) throw new Sadna192Exception("you can't open the store if not logedin", "(ServiceLayer) User_ServiceLayer", "Open_Store(1)");
                if (Tools.check_storeName(name))
                {
                    if (DBAccess.getStoreByName(name) != null) throw new Sadna192Exception("name is allready in use", "(ServiceLayer) User_ServiceLayer", "Open_Store(2)");
                    Store newstore = new Store(name);
                    bool ans = this.userState.Open_Store(newstore);
                    this.single_ServiceLayer.store.Add(newstore);
                    if (!(DBAccess.SaveToDB(newstore)))  //&& DBAccess.SaveToDB(newstore.GetPolicy()) && DBAccess.SaveToDB(newstore.GetDiscount()))
                        DBAccess.DBerror("could not save Store to DB");
                    if (ans) this.Add_Log("opened new store named - " + name);
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
                        ans = DBAccess.getMemberFromDB(user_name) != null;
                        if (ans) throw new Sadna192Exception("the user name is allready in use", "(ServiceLayer) User_ServiceLayer", "Register(1)");
                        Member newMember = new Member(user_name, user_pass);
                        this.single_ServiceLayer.members.Add(newMember);
                        if (!(DBAccess.SaveToDB(newMember))) //&& DBAccess.SaveToDB(newMember.shopingBasket)
                            DBAccess.DBerror("could not save member and shopping basket to DB ");

                        this.Add_Log("Registerd new user with name '" + user_name + "'");
                        return true;
                    }
                    throw new Sadna192Exception("you are allready logedin", "(ServiceLayer) User_ServiceLayer", "Register(2)");
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

                        if (((Member)this.userState).isMe(other_user)) throw new Sadna192Exception("You can't remove yourself", "(ServiceLayer) User_ServiceLayer", "Remove_User(1)");
                        Member tmp = DBAccess.getMemberFromDB(other_user);
                        if (tmp == null)
                            throw new Sadna192Exception("user not found", "(ServiceLayer) User_ServiceLayer", "Remove_User(2)");
                        if (!DBAccess.removeUserFromDB(other_user))
                        {
                            DBAccess.DBerror("remove  " + other_user + "from DB faild");
                            return false;
                        }
                        foreach (User_ServiceLayer user in single_ServiceLayer.users)
                        {
                            if (user.userState.isMember())
                            {
                                if (((Member)user.userState).isMe(other_user))
                                {
                                    user.Logout();
                                    single_ServiceLayer.members.Remove(tmp);
                                    this.Add_Log("removed '" + other_user + "' from the system");

                                }
                            }
                        }
                        return true;
                    }
                    throw new Sadna192Exception("the user is not an Admin", "(ServiceLayer) User_ServiceLayer", "Remove_User(3)");
                }
                return false;
            }

            public bool Update_Product_Store(string Store_name, string product_name, string product_new_name, string product_new_category, double product_new_price, int product_new_amount, Discount product_new_discount, Policy product_new_policy)
            {
                if (Tools.check_storeName(Store_name) &&
                    Tools.check_productNames(product_name) &&
                    (product_new_name == null || Tools.check_productNames(product_new_name)) &&
                    (product_new_category == null || Tools.check_productCategory(product_new_category)) &&
                    (product_new_price == -1 || Tools.check_price(product_new_price)) &&
                    (product_new_amount == -1) || Tools.check_amount(product_new_amount)
                    )
                {
                    bool ans = false;
                    if (product_new_amount > 0 || product_new_amount==-1)
                    {
                        ans = this.userState.Update_Product_Store(Store_name, product_name, product_new_name, product_new_category, product_new_price, product_new_amount, product_new_discount, product_new_policy);
                        if (ans)
                        {
                            string toLog = "in store '" + Store_name + "' updated '" + product_name;
                            if (product_new_name != null)
                            {
                                toLog += "' - new name:'" + product_new_name;
                            }
                            if (product_new_category != null)
                            {
                                toLog += "',new category:'" + product_new_category;
                            }
                            if (product_new_price!=-1)
                            {
                                toLog += "' ,new price:" + product_new_price;
                            }
                            if (product_new_amount!=-1)
                            {
                                toLog +=" ,new amount:" + product_new_amount ;
                            }
                            if (product_new_discount != null)
                            {
                                toLog += " , new discount:" + product_new_discount.ToString();
                            }
                            if (product_new_policy != null)
                            {
                                toLog += " ,new policy:" + product_new_policy.ToString();
                            }
                            this.Add_Log(toLog);
                        }
                    }
                    else
                        ans = this.Remove_Product_Store(Store_name, product_name);
                    return ans;
                }
                return false;
            }

            public List<KeyValuePair<ProductInStore, int>> Watch_Cart()
            {
                List < KeyValuePair<ProductInStore, int> > tmp = this.userState.Watch_Cart();
                List<KeyValuePair<ProductInStore, int>> ans = new List<KeyValuePair<ProductInStore, int>>();
                foreach(KeyValuePair<ProductInStore, int> p in tmp)
                {
                    ans.Add(new KeyValuePair<ProductInStore, int>());
                }
                ans.Sort(new cartOrder());
                this.Add_Log("watched his cart");
                return ans;
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

            public UserState GetUserState()
            {
                return this.userState;
            }
            public List<Dictionary<string, dynamic>> usersStores()
            {
                return this.userState.getMyShops();
            }

            public bool alert(string messege)
            {
                return this.alerter.AlertUser(messege);
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
                    ProductInStore pro = DBAccess.findProductInStore(p.store.name, p.product.name);
                    return (pro != null);
                }
                catch
                {
                    throw new Sadna192Exception("Product is not in the store", "(ServiceLayer) User_ServiceLayer", "isProductInStore");
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
                if (ans == null) throw new Sadna192Exception("no user was found", "(ServiceLayer) User_ServiceLayer", "GetMember");
                return ans;
            }

            public bool canclePurch()
            {
                ((Visitor)this.userState).shopingBasket.returnProducts();
                return true;
            }

            public ProductInStore GetProductFromStore(string productName,string storeName)
            {
                /*foreach (Store s in this.single_ServiceLayer.store)
                {
                    if (s.getName() == storeName)
                    {
                        return s.getProductInStore().Find(t => t.getName() == productName);
                    }
                }*/
                return DBAccess.findProductInStore(storeName, productName);

            }
        }
    }
}
