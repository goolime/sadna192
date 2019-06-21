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
