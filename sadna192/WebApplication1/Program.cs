using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using sadna192;


namespace WebApplication1
{
    public class Program
    {
        public static void Main(string[] args)
        {
            I_ServiceLayer tmp_sl = new sadna192.ServiceLayer();
            tmp_sl.Create_ServiceLayer(new Stubs.Stub_deliverySystem(), new Stubs.Stub_paymentSystem(),"admin", "1234Abcd");
            CreateWebHostBuilder(args).Build().Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>();
    }
}
