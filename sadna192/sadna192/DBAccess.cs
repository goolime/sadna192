using System;
using System.Collections.Generic;
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
            try
            {
                using (var ctx = new sadna192.Model1())
                {
                    if (o.GetType() == typeof(Product))
                        ctx.Products.Add((Product)o);
                    else if (o is Member)
                        ctx.Members.Add((Member)o);
                    else if (o.GetType() == typeof(Manager))
                        ctx.Managers.Add((Manager)o);
                    else if (o.GetType() == typeof(Owner))
                        ctx.Owners.Add((Owner)o);
                    else if (o.GetType() == typeof(Store))
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

        public static bool saveProductToDB(Product p)
        {
            try
            {
                using (var ctx = new Model1())
                {
                    ctx.Products.Add(p);
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


        public static bool saveMemberToDB(Member m)
        {
            try
            {
                using (var ctx = new Model1())
                {
                    ctx.Members.Add(m);
                    ctx.SaveChanges();
                    return true;
                }
            }
            catch (DbUpdateException e)
            {
                Console.WriteLine("save member to DB faild : " + e.ToString());
                return false;
            }
        }

        public static Member getMemberFromDB(string memberName)
        {
            Member ans=null;
            try
            {
                using (var ctx = new Model1())
                {
                    // Query for the Blog named ADO.NET Blog
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
                        // Query for all blogs with names starting with B
                        /* var member = from b in ctx.Members
                                     where b.Name.StartsWith("B")
                                     select b;*/
             }
         }
