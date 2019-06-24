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
            init(tmp_sl);
            CreateWebHostBuilder(args).Build().Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>();

        public static void init(I_ServiceLayer SL)
        {
            I_User_ServiceLayer UL = SL.Connect(null);
            UL.Register("initUser", "1234Abcd");
            UL.Login("initUser", "1234Abcd");
            UL.Open_Store("testStore");
            UL.Add_Product_Store("testStore", "Banna", "food", 5.0, 50, new noDiscount(), new regularPolicy());
            UL.Add_Product_Store("testStore", "Apple", "food", 5.0, 70, new noDiscount(), new regularPolicy());
            UL.Add_To_ShopingBasket(UL.GlobalSearch("Banna", null, null, -1, -1, -1, -1)[0],5);
            UL.Add_To_ShopingBasket(UL.GlobalSearch("Apple", null, null, -1, -1, -1, -1)[0], 5);
            UL.Logout();
        }
    }

    
}
