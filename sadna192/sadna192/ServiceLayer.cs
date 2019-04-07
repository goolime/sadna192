using System;
using System.Collections.Generic;

namespace sadna192
{
    public class ServiceLayer : I_ServiceLayer
    {
        private static single_ServiceLayer singleton=null;

        ServiceLayer(I_DeliverySystem deliverySystem, I_PaymentSystem paymentSystem, string admin_name, string admin_pass)
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

                if (!Tools.check_username(admin_name) && !Tools.check_password(admin_pass)) throw new Exception("invalid admin details");
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
                this.single_ServiceLayer.log.Add(log);
            }

            public bool Add_Product_Store(string Store_name, string product_name, string product_category, double product_price, int product_amount, Discount product_discount, Policy product_policy)
            {
                if (Tools.check_storeName(Store_name) &&
                    Tools.check_productNames(product_name) &&
                    Tools.check_productCategory(product_category) &&
                    Tools.check_price(product_price) &&
                    Tools.check_amount(product_amount) &&
                    product_discount.check() &&
                    product_policy.check()
                )
                {
                    return this.userState.Add_Product_Store(Store_name, product_name, product_category, product_price, product_amount, product_discount, product_policy);
                }
                return false;
            }

            public bool Add_Store_Manager(string Store_name, string new_manger_name, bool permision_add, bool permission_remove, bool permission_update)
            {
                if (Tools.check_storeName(Store_name) && Tools.check_username(new_manger_name))
                {
                    if (!this.userState.isOwner(Store_name)) throw new Exception("you are not an owner of this store");
                    Member other_user = this.GetMember(new_manger_name);
                    if (other_user == null) throw new Exception("new Store manager was not found");
                    this.userState.Add_Store_Manager(Store_name,other_user, permision_add, permission_remove, permission_update);
                    return true;
                }
                return false;
            }

            public bool Add_Store_Owner(string Store_name, string new_owner_name)
            {
                if (Tools.check_username(new_owner_name) && Tools.check_storeName(Store_name))
                {
                    Member other_user = this.GetMember(new_owner_name);
                    if (other_user == null) throw new Exception("new Store owner was not found");
                    return this.userState.Add_Store_Owner(Store_name, other_user);
                    //store.addOwner(this.userState, other_user);
                    //return true;
                }
                return false;
            }

            public bool Add_To_ShopingBasket(ProductInStore p, int amount)
            {
                if (isProductInStore(p) && Tools.check_amount(amount))
                {
                    return this.userState.Add_To_ShopingBasket(p, amount);
                }
                return false;
            }

            public bool Edit_Product_In_ShopingBasket(ProductInStore p, int amount)
            {
                if (isProductInStore(p) && Tools.check_amount(amount))
                {
                    return this.userState.Edit_Product_In_ShopingBasket(p, amount);
                }
                return false;
            }

            public bool Finalize_Purchase(string address, string payment)
            {
                if (this.single_ServiceLayer.deliverySystem.check_address(address) && this.single_ServiceLayer.paymentSystem.check_payment(payment))
                {
                    return this.userState.Finalize_Purchase(address, payment);
                }
                return false;
            }

            public List<ProductInStore> GlobalSearch(string name, string Category, List<string> keywords, double price_min, double price_max, double Store_rank, double product_rank)
            {
                if (Tools.check_productNames(name) &&
                    Tools.check_productCategory(Category) &&
                    Tools.check_price(price_min) &&
                    Tools.check_price(price_max) &&
                    price_min<=price_max &&
                    Tools.check_price(Store_rank) &&
                    Tools.check_price(product_rank)
                    ) {
                    List<ProductInStore> ans = new List<ProductInStore>();
                    foreach (Store store in this.single_ServiceLayer.store)
                        ans.AddRange(store.Search(name, Category, keywords, price_min, price_max, Store_rank, product_rank));
                    return ans;
                }
                return null;
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
                    this.userState = new Visitor();
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
                    this.userState.Open_Store(newstore);
                    return true;
                }
                return false;
            }

            public List<KeyValuePair<ProductInStore, KeyValuePair<int, double>>> Purchase_product(ProductInStore p, int amount)
            {
                if (isProductInStore(p) && Tools.check_amount(amount))
                {
                    return this.userState.Purchase_product(p, amount);
                }
                return null;
            }

            public List<KeyValuePair<ProductInStore, KeyValuePair<int, double>>> Purchase_Store_cart(string store_name)
            {
                if (Tools.check_storeName(store_name))
                {
                    return this.userState.Purchase_Store_cart(store_name);
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
                    return this.userState.Remove_Product_Store(Store_name, product_name);
                }
                return false;
            }

            public bool Remove_Store_Manager(string Store_name, string other_Manager_name)
            {
                if (Tools.check_storeName(Store_name) && Tools.check_username(other_Manager_name))
                {
                    return this.userState.Remove_Store_Manager(Store_name, this.GetMember(other_Manager_name));
                }
                return false;
            }

            public bool Remove_Store_Owner(string Store_name, string other_owner_name)
            {
                if (Tools.check_storeName(Store_name) && Tools.check_username(other_owner_name))
                {
                    return this.userState.Remove_Store_Owner(Store_name, this.GetMember(other_owner_name));
                }
                return false;
            }

            public bool Remove_User(string other_user)
            {
                if (Tools.check_username(other_user))
                {
                    if (this.userState.isAdmin())
                    {
                        foreach (User_ServiceLayer user in single_ServiceLayer.users)
                        {
                            if (user.userState.isMember())
                            {
                                if (((Member)user.userState).isMe(other_user))
                                {
                                    Member tmp = (Member)user.userState;
                                    user.Logout();
                                    single_ServiceLayer.members.Remove(tmp);
                                    return true;
                                }
                            }
                        }
                        foreach (Member member in single_ServiceLayer.members)
                        {
                            if (member.isMe(other_user))
                            {
                                single_ServiceLayer.members.Remove(member);
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
                    Tools.check_amount(product_new_amount) &&
                    product_new_discount.check() &&
                    product_new_policy.check()
                    ) {
                    return this.userState.Update_Product_Store(Store_name, product_name, product_new_name, product_new_category, product_new_price, product_new_amount, product_new_discount, product_new_policy);
                }
                return false;
            }

            public List<KeyValuePair<ProductInStore, int>> Watch_Cart()
            {
                return this.userState.Watch_Cart();
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

            private bool isProductInStore(ProductInStore p)
            {
                throw new NotImplementedException(); // TODO: depends on rons implemantation
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
                return ans;
            }
        }
    }
}
