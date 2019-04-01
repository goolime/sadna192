using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sadna192
{
    internal class ServiceLayer : I_ServiceLayer
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
            private I_DeliverySystem deliverySystem;
            private I_PaymentSystem paymentSystem;
            public List<string> log;
            public List<Store> store;

            public single_ServiceLayer(I_DeliverySystem deliverySystem, I_PaymentSystem paymentSystem, string admin_name, string admin_pass)
            {
                this.deliverySystem = deliverySystem;
                this.paymentSystem = paymentSystem;
                this.members = new List<Member>();
                this.users = new List<I_User_ServiceLayer>();
                this.log = new List<string>();

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
                return this.userState.Add_Product_Store(Store_name, product_name, product_category, product_price, product_amount, product_discount, product_policy);
            }

            public bool Add_Store_Manager(string Store_name, string new_manger_name, bool permision_add, bool permission_remove, bool permission_update)
            {
                if (!this.userState.isOwner(Store_name)) throw new Exception("you are not an owner of this store");
                Store store = ((Member)this.userState).getUserStore(Store_name);
                Member other_user= null;
                foreach (User_ServiceLayer user in single_ServiceLayer.users)
                {
                    if (user.userState.isMember())
                    {
                        if (((Member)user.userState).isMe(new_manger_name))
                        {
                            other_user = (Member)user.userState;
                        }
                    }
                }
                if (other_user == null) {
                    foreach (Member member in single_ServiceLayer.members)
                    {
                        if (member.isMe(new_manger_name))
                        {
                            other_user = member;
                        }
                    }
                }
                if (other_user == null) throw new Exception("new Store manager was not found");
                store.addManager(this.userState,other_user, permision_add, permission_remove, permission_update);
                return true;
            }

            public bool Add_Store_Owner(string Store_name, string new_owner_name)
            {
                if (!this.userState.isOwner(Store_name)) throw new Exception("you are not an owner of this store");
                Store store = ((Member)this.userState).getUserStore(Store_name);
                Member other_user = null;
                foreach (User_ServiceLayer user in single_ServiceLayer.users)
                {
                    if (user.userState.isMember())
                    {
                        if (((Member)user.userState).isMe(new_owner_name))
                        {
                            other_user = (Member)user.userState;
                        }
                    }
                }
                if (other_user == null)
                {
                    foreach (Member member in single_ServiceLayer.members)
                    {
                        if (member.isMe(new_owner_name))
                        {
                            other_user = member;
                        }
                    }
                }
                if (other_user == null) throw new Exception("new Store owner was not found");
                store.addOwner(this.userState ,other_user);
                return true;
            }

            public bool Add_To_ShopingBasket(ProductInStore p, int amount)
            {
                throw new NotImplementedException();
            }

            public bool Edit_Product_In_ShopingBasket(ProductInStore p, int amount)
            {
                throw new NotImplementedException();
            }

            public bool Finalize_Purchase(string address, string payment)
            {
                throw new NotImplementedException();
            }

            public List<ProductInStore> GlobalSearch(string name, string Category, List<string> keywords, double price_min, double price_max, double Store_rank, double product_rank)
            {
                throw new NotImplementedException();
            }

            public bool Login(string user_name, string user_pass)
            {
                if (!this.userState.isVistor()) throw new Exception("you are already logedin");
                foreach(Member member in single_ServiceLayer.members)
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
                foreach(Store store in this.single_ServiceLayer.store)
                {
                    if (store.isMe(name)) throw new Exception("name is allready in use");
                }
                Store newstore = new Store(name);
                this.userState.Open_Store(newstore);
                return true;
            }

            public List<KeyValuePair<ProductInStore, KeyValuePair<int, double>>> Purchase_product(ProductInStore p, int amount)
            {
                throw new NotImplementedException();
            }

            public List<KeyValuePair<ProductInStore, KeyValuePair<int, double>>> Purchase_Store_cart(int Shop_ID)
            {
                throw new NotImplementedException();
            }

            public bool Register(string user_name, string user_pass)
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

            public bool Remove_Product_Store(string Store_name, string product_name)
            {
                throw new NotImplementedException();
            }

            public bool Remove_Store_Manager(string Store_name, string other_Manager_name)
            {
                throw new NotImplementedException();
            }

            public bool Remove_Store_Owner(string Store_name, string other_owner_name)
            {
                throw new NotImplementedException();
            }

            public bool Remove_User(string other_user)
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
                        if (member.isMe(other_user)){
                            single_ServiceLayer.members.Remove(member);
                            return true;
                        }
                    }
                    throw new Exception("user not found");
                }
                throw new Exception("the user is not an Admin");
            }

            public bool Update_Product_Store(string Store_name, string product_name, string product_new_name, string product_new_category, double product_new_price, int product_new_amount, Discount product_new_discount, Policy product_new_policy)
            {
                throw new NotImplementedException();
            }

            public List<KeyValuePair<ProductInStore, int>> Watch_Cart()
            {
                throw new NotImplementedException();
            }
        }
    }
}
