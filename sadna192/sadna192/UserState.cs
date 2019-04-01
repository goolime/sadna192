using System.Collections.Generic;

namespace sadna192
{
    interface UserState
    {
        bool isVistor();
        bool isMember();
        bool isAdmin();

        List<KeyValuePair<ProductInStore, int>> get_ShopingBasket();
        bool Add_To_ShopingBasket(ProductInStore p, int amount);
        bool Edit_Product_In_ShopingBasket(ProductInStore p, int amount);
        List<KeyValuePair<ProductInStore, KeyValuePair<int, double>>> Purchase_Store_cart(string store_name);
        List<KeyValuePair<ProductInStore, KeyValuePair<int, double>>> Purchase_product(ProductInStore p, int amount);
        bool Finalize_Purchase(string address, string payment);
        bool Add_Store_Manager(string Store_name, string new_manger_name, bool permision_add, bool permission_remove, bool permission_update);
        bool Add_Store_Owner(string Store_name, string new_owner_name);
        bool Remove_Product_Store(string Store_name, string product_name);
        bool Remove_Store_Manager(string Store_name, string other_Manager_name);
        bool Remove_Store_Owner(string Store_name, string other_owner_name);
        bool Update_Product_Store(string Store_name, string product_name, string product_new_name, string product_new_category, double product_new_price, int product_new_amount, Discount product_new_discount, Policy product_new_policy);
        bool Open_Store(Store name);
        bool isOwner(string store_name);
        bool Add_Product_Store(string Store_name, string product_name, string product_category, double product_price, int product_amount, Discount product_discount, Policy product_policy);
        List<KeyValuePair<ProductInStore, int>> Watch_Cart();
    }
}