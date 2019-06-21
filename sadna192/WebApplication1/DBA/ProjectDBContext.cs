using sadna192;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication1.DBA
{
    public class ProjectDBContext : DbContext
    {
        public DbSet<Member> members { get; set; }
    }
}
