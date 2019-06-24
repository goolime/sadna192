using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sadna192
{
    class DBAccess
    {
        public static bool SaveToDB(object o)
        {
            Console.WriteLine("saveToDB: " + o.ToString());
            try
            { 
                using (var ctx = new sadna192.Model1())
                {
                    if (o.GetType() == typeof(Product))
                        ctx.Products.Add((Product)o);
                    else if (o is Member)
                        ctx.Members.Add((Member)o);
                    else if (o.GetType() == typeof(Manager))
                        ctx.Owners.Add((Owner)o);
                    //ctx.Managers.Add((Manager)o);
                    else if (o.GetType() == typeof(Owner))
                        ctx.Owners.Add((Owner)o);
                    else if (o is Store)
                        ctx.Stores.Add((Store)o);
                    else if (o is Policy)
                        ctx.Policies.Add((Policy)o);
                    else if (o is Discount)
                        ctx.Discounts.Add((Discount)o);
                    else if (o.GetType() == typeof(ProductInStore))
                        ctx.ProductInStores.Add((ProductInStore)o);
                    else if (o.GetType() == typeof(ShoppingCart))
                        ctx.ShoppingCarts.Add((ShoppingCart)o);
                    else if (o.GetType() == typeof(ShopingBasket))
                        ctx.ShopingBaskets.Add((ShopingBasket)o);
                    else if (o.GetType() == typeof(ItemsInCart))
                        ctx.ItemsInCarts.Add((ItemsInCart)o);
                    else if (o.GetType() == typeof(Admin))
                        ctx.Members.Add((Admin)o);
                    else Console.WriteLine("type " + o.GetType().ToString() + " is not catched");
                    
                    ctx.SaveChanges();
                    return true;
                }
            }
            catch (DbUpdateException e)
            {
                Console.WriteLine("save product to DB faild : " + e.ToString());
                return false;
            }           
        }


        public static void DBerror(String e)
        {
            Console.WriteLine("DB ERROR: "+e+" TODO!!!!!");
        }
        /**************** check ****************/
        public static Member loginCheck(string memberName, string pass)
        {
            Console.WriteLine("loginCheck: " + memberName);
            Member ans = null;
            try
            {
                using (var ctx = new Model1())
                {
                    ans = ctx.Members
                        .Where(m => m.name == memberName)
                        .FirstOrDefault();

                    if (ans == null || !ans.check(memberName, pass))
                        ans = null;
                }
            }
            catch (DbUpdateException e)
            {
                Console.WriteLine("get member from DB faild : " + e.ToString());
            }
            return ans;
        }
        
        public static bool MemberIsOwner(string member_name , string store_name)
        {
            Owner ans = null;
            try
            {
                using (var ctx = new Model1())
                {
                    ans = ctx.Owners.Include("Store").Include("user")
                                    .Where(o => o.store.name == store_name
                                    && o.user.name == member_name)
                                    .FirstOrDefault();
                }
            }
            catch (DbUpdateException e)
            {
                Console.WriteLine("check if member is store owner faild : " + e.ToString());
            }
            return ans!= null;
        }
        /**************** Get from DB ****************/
        public static Admin getAdminFromDB(string name)
        {
            Admin ans = null;
            try
            {
                using (var ctx = new Model1())
                {
                    ans = (Admin)ctx.Members
                                    .Where(m => m.name == name)
                                    .FirstOrDefault();
                    return ans;
                }
            }
            catch (DbUpdateException e)
            {
                Console.WriteLine("get Admin from DB faild : " + e.ToString());
            }
            return ans;
        }


        public static Member getMemberFromDB(string memberName)
        {
            Console.WriteLine("get MemberFromDB: " + memberName);
            Member ans=null;
            try
            {
                using (var ctx = new Model1())
                {
                    ans = ctx.Members
                                    .Where(m => m.name == memberName)
                                    .FirstOrDefault();
                }
            }
            catch (DbUpdateException e)
            {
                Console.WriteLine("get member from DB faild : " + e.ToString());
            }
            return ans;
        }

        public static Owner getOwnerByNameAndStore(string user_name , string store_name)
        {
            Owner ans = null;
            try
            {
                using (var ctx = new Model1())
                {
                    ans = ctx.Owners.Include("Store").Include("user")
                                    .Where(o => o.store.name == store_name
                                    && o.user.name == user_name)
                                    .FirstOrDefault();
                }
            }
            catch (DbUpdateException e)
            {
                Console.WriteLine("check if member is store owner faild : " + e.ToString());
            }
            return ans;
        }

        public static ProductInStore findProductInStore(string store_name , string product_name)
        {
            ProductInStore ans = null;
            try
            {
                using (var ctx = new Model1())
                {
                    ans = ctx.ProductInStores.Include("Product").Include("Store").Include("Discount").Include("Policy")
                                    .Where(p => p.product.name == product_name && 
                                    p.store.name == store_name)
                                    .FirstOrDefault();
                    return ans;
                }
            }
            catch (DbUpdateException e)
            {
                Console.WriteLine("get member from DB faild : " + e.ToString());
            }
            throw new Sadna192Exception("product not exsist in store", "DBAccess", "findProductInStore");
            //return ans;
        }


        public static Owner getUserStore(String store_name , string member_name)
        {
            Owner owner = null;
            try
            {
                using (var ctx = new Model1())
                {
                    owner = ctx.Owners.Include("Store").Include("user")
                                    .Where(o => o.store.name == store_name 
                                    && o.user.name==member_name)
                                    .FirstOrDefault();
                }
            }
            catch (DbUpdateException e)
            {
                Console.WriteLine("get User Store from DB faild : " + e.ToString());
            }
            return owner;
        }

        /*public static List<KeyValuePair<ProductInStore, int>> getBasket()
        {
            List<KeyValuePair<ProductInStore, int>> ans = new List<KeyValuePair<ProductInStore, int>>();
            try
            {
                using (var ctx = new Model1())
                {
                    ans = ctx.ShoppingCarts.Include("Store").Include("member")
                                    .Where(o => o.store.getName() == store_name
                                    )
                                    .FirstOrDefault();
                }
            }
            catch (DbUpdateException e)
            {
                Console.WriteLine("get User Basket from DB faild : " + e.ToString());
            }
            return ans;

        }*/


        public static List<ProductInStore> searchProductInStore
            (string name, string Category, List<string> keywords, double price_min, double price_max, double product_rank)
        {
            List<ProductInStore> products = new List<ProductInStore>();

            using (var ctx = new Model1())
            {
                var query = from i in ctx.ProductInStores.Include("Product").Include("Store") select i;                          
                if (name != null)
                    query = query.Where(q => q.product.name == name);
                if (Category != null)
                    query = query.Where(q => q.product.category == Category);
                /* if (keywords != null)
                     foreach (string word in keywords)
                     {
                         query = query.Where(q => q.product.KeywordsAsString.Contains(word));
                     }*/
                if (price_min != -1)
                     query = query.Where(q => q.price > price_min);
                 if (price_max != -1)
                     query = query.Where(q => q.price < price_max);
                 if (product_rank != -1)
                     query = query.Where(q => q.product.rank >= product_rank);  

                products = query.ToList();
            }
           
            return products;
        }

        public static Product searchProduct (string name, string Category, double product_rank)
        {
            Product product = new Product();
            try
            {
                using (var ctx = new Model1())
                {
                    product = ctx.Products
                                    .Where(p => p.name == name 
                                    && p.category == Category)
                                    .FirstOrDefault();
                }
            }
            catch (DbUpdateException e)
            {
                Console.WriteLine("get member from DB faild : " + e.ToString());
            }          

            return product;
        }


        public static Product getProductByID (int id)
        {
            Product ans = null;
            try
            {
                using (var ctx = new Model1())
                {
                    ans = ctx.Products
                        .Where(p => p.id == id)
                        .FirstOrDefault();
                }
            }
            catch (DbUpdateException e)
            {
                Console.WriteLine("get product from DB faild : " + e.ToString());
            }
            return ans;
        }

        public static Store getStoreByID(int id)
        {
            Store ans = null;
            try
            {
                using (var ctx = new Model1())
                {
                    ans = ctx.Stores
                        .Where(s => s.storeID == id)
                        .FirstOrDefault();
                }
            }
            catch (DbUpdateException e)
            {
                Console.WriteLine("get product from DB faild : " + e.ToString());
            }
            return ans;
        }

        public static Store getStoreByName(string name)
        {
            Console.WriteLine("5: " + name);
            Store ans = null;
            try
            {
                using (var ctx = new Model1())
                {
                    ans = ctx.Stores
                        .Where(s => s.name == name)
                        .FirstOrDefault();
                }
            }
            catch (DbUpdateException e)
            {
                Console.WriteLine("get product from DB faild : " + e.ToString());
            }
            return ans;
        }

        public static Discount getDiscountByID(int id)
        {
            Discount ans = null;
            try
            {
                using (var ctx = new Model1())
                {
                    ans = ctx.Discounts
                        .Where(d => d.DiscountID == id)
                        .FirstOrDefault();
                }
            }
            catch (DbUpdateException e)
            {
                Console.WriteLine("get Discount from DB faild : " + e.ToString());
            }
            return ans;
        }

/**************** Update DB ****************/ 

        public static bool updateProductInStoreIfExist(string store_name ,string product_name, double product_price, int product_amount, Discount product_discount, Policy product_policy)
        {
            bool ans = false;
            try
            {
                using (var ctx = new Model1())
                {
                    var res = ctx.ProductInStores.Include("Product").Include("Store").Include("Discount").Include("Policy")
                                    .Where(p => p.product.name == product_name 
                                    && p.store.name == store_name)
                                    .FirstOrDefault();
                    if (res == null)
                        throw new Sadna192Exception("product not exsist in store", "DBAccess", "updateProductInStoreIfExist");
                    res.setAmount(product_amount);
                    res.setPrice(product_price);
                    res.setDiscount(product_discount);
                    res.setPolicy(product_policy);
                    ctx.SaveChanges(); 
                }
            }
            catch (DbUpdateException e)
            {
                Console.WriteLine("get ProductInStore from DB & update him faild : " + e.ToString());
            }
            return ans;  
        }

        public static bool updateProductInStore(string store_name, string prev_product_name, string product_name, string product_category , double product_price, int product_amount, Discount product_discount, Policy product_policy)
        {
            bool ans = false;
            try
            {
                using (var ctx = new Model1())
                {
                    var res = ctx.ProductInStores.Include("Product").Include("Store").Include("Discount").Include("Policy")
                                    .Where(p => p.product.name == prev_product_name
                                    && p.store.name == store_name)
                                    .FirstOrDefault();
                    if (res == null)
                        throw new Sadna192Exception("product not exsist in store", "DBAccess", "updateProductInStore");
                    if (product_name != null)
                        res.product.name = product_name;
                    if (product_category != null)
                        res.product.category = product_category;
                    if (product_amount != -1)
                        res.amount=product_amount;
                    if (product_price != -1)
                        res.price = product_price;
                    if (product_discount != null)
                        res.setDiscount(product_discount);
                    if (product_policy != null)
                        res.setPolicy(product_policy); 
                    ctx.SaveChanges();
                    return true; 
                }
            }
            catch (DbUpdateException e)
            {
                Console.WriteLine("get ProductInStore from DB & update him faild : " + e.ToString());
            }
            return ans;
        }


        // ********** Remove From DB *********** //
        public static bool removeProductInStore(string store_name, string product_name)
        {
            try
            {
                using (var ctx = new Model1())
                {
                    var res = ctx.ProductInStores.Include("Product").Include("Store")
                                    .Where(p => p.product.name == product_name
                                    && p.store.name == store_name)
                                    .FirstOrDefault();
                    if (res == null)
                    {
                        Console.WriteLine("trying to delete ProductInStore that is not exist");
                        return false;
                    }
                    ctx.ProductInStores.Remove(res);
                    ctx.SaveChanges();
                    return true;
                }
            }
            catch (DbUpdateException e)
            {
                Console.WriteLine("remove ProductInStore from DB faild : " + e.ToString());
                throw new Sadna192Exception("Connection to DB lost: remove ProductInStore from DB faild", "DBAccess", "removeProductInStore");
            }
        }

        public static bool removeUserFromDB(string user_name)
        {
            bool ans = false;
            try
            {
                using (var ctx = new Model1())
                {
                    var res = ctx.Members
                                    .Where(m => m.name == user_name)
                                    .FirstOrDefault();
                    if (res == null)
                    {
                        Console.WriteLine("trying to delete user that is not exist");
                        return false;
                    }
                    ans = true; 
                    ctx.Members.Remove(res);
                    ctx.SaveChanges();
                }
            }
            catch (DbUpdateException e)
            {
                Console.WriteLine("remove user from DB faild : " + e.ToString());
            }

            return ans; 
        }

        public static bool removeOwnerFromDB(int id)//Owner toRemove)
        {
            bool ans = false;
            try
            {
                using (var ctx = new Model1())
                {
                    var res = ctx.Owners.Find(id); 
                        
                    if (res == null)
                    {
                        Console.WriteLine("trying to delete user that is not owner");
                        return false;
                    }
                    ans = true;
                    ctx.Owners.Remove(res);
                    ctx.SaveChanges();
                }
            }
            catch (DbUpdateException e)
            {
                Console.WriteLine("remove user from DB faild : " + e.ToString());
            }
            return ans;
        }

        public static void removeDuplicatUserFromDB(string user_name)
        {
            Member ans = null;
            using (var ctx = new Model1())
            {
                ans = ctx.Members
                         .Where(m => m.name == user_name)
                         .FirstOrDefault();
                if (ans != null)
                {
                    int def_id = ans.id;
                    var res = ctx.Members
                                    .Where(m => m.name == user_name && m.id > def_id)
                                    .ToList();
                    foreach (Member tmp in res)
                    {
                        var del = ctx.Members.Find(tmp.id);
                        ctx.Members.Remove(del);
                    }
                    ctx.SaveChanges();
                }
                else Console.WriteLine(" ~~~~~ there is just one"); 
            }   
        }




    }
}
