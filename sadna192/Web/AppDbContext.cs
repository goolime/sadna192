using System;
using Microsoft.EntityFrameworkCore;
using sadna192;
using static sadna192_Tests.Stubs;

namespace Web

{
    public class AppDbContext : DbContext
    {
        private readonly I_User_ServiceLayer Iusl;
        private readonly I_ServiceLayer Isl;
        public AppDbContext(DbContextOptions options)
            : base(options)
        {
            Isl = new ServiceLayer();
            try
            {
                Isl.Create_ServiceLayer(new stub_deliverySystem(), new stub_paymentSystem(), "admin", "1234Abcd");
            }
            catch (Exception) { }
            Iusl = Isl.Connect();
        }

        public DbSet<Customer> Customers { get; set; }

        internal bool Login(string name, string password)
        {
            return Iusl.Login(name,password);
        }
    }
}