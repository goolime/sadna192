using System;
using System.Collections.Generic;

namespace sadna192
{
    public interface I_ServiceLayer
    {
        // UC 1.1
        /// <summary>
        /// a devloper creats the system
        /// </summary>
        /// <param name="deliverySystem">the external delivery system</param>
        /// <param name="paymentSystem">the external payment system</param>
        /// <param name="admin_name">the admin's name</param>
        /// <param name="admin_pass">the admin's passwords</param>
        /// <returns></returns>
        I_ServiceLayer Create_ServiceLayer(I_DeliverySystem deliverySystem, I_PaymentSystem paymentSystem, string admin_name, string admin_pass);
        /// <summary>
        /// represent a connection to a single user in the system. needed for later parallel conection.
        /// </summary>
        /// <returns>a connection to a single user in the system</returns>
        I_User_ServiceLayer Connect();

        void CleanUpSystem();
    }

    public interface I_User_ServiceLayer
    {
        // UC 2.2
        bool Register(string user_name, string user_pass);
        // UC 2.3
        bool Login(string user_name, string user_pass);
        // UC 2.5

        List<ProductInStore> GlobalSearch(string name, string Category, List<String> keywords, double price_min, double price_max, double Store_rank, double product_rank);

        // UC 2.6
        bool Add_To_ShopingBasket(ProductInStore p, int amount);

        // UC 2.7.1
        List<KeyValuePair<ProductInStore, int>> Watch_Cart();
        // UC 2.7.2
        bool Edit_Product_In_ShopingBasket(ProductInStore p, int amount);

        // UC 2.8.1
        List<KeyValuePair<ProductInStore, KeyValuePair<int, double>>> Purchase_Store_cart(string store_name);
        // UC 2.8.4
        List<KeyValuePair<ProductInStore, KeyValuePair<int, double>>> Purchase_product(ProductInStore p, int amount);

        bool Finalize_Purchase(string address, string payment);

        // UC 3.1 
        bool Logout();

        // UC 3.2
        bool Open_Store(string name);

        // UC 4.1.1/5.1
        bool Add_Product_Store(string Store_name, string product_name, string product_category, double product_price, int product_amount, Discount product_discount, Policy product_policy);

        // UC 4.1.2/5.1
        bool Remove_Product_Store(string Store_name, string product_name);

        // UC 4.1.3/5.1
        bool Update_Product_Store(string Store_name, string product_name, string product_new_name, string product_new_category, double product_new_price, int product_new_amount, Discount product_new_discount, Policy product_new_policy);

        // UC 4.3
        bool Add_Store_Owner(string Store_name, string new_owner_name);

        // UC 4.4
        bool Remove_Store_Owner(string Store_name, string other_owner_name);

        // UC 4.5
        bool Add_Store_Manager(string Store_name, string new_manger_name, bool permision_add, bool permission_remove, bool permission_update);

        // UC 4.6
        bool Remove_Store_Manager(string Store_name, string other_Manager_name);

        // UC 6.2
        bool Remove_User(string other_user);

        // RSL 7
        void Add_Log(string log);

        UserState GetUserState();

        List<Dictionary<string, dynamic>> usersStores();

        bool canclePurch();

         ProductInStore GetProductFromStore(string productName, string storeName);
    }
    

}
