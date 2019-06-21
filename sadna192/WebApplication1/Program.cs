using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
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
        //List<Pair<String, I_User_ServiceLayer>> mapping;
        I_User_ServiceLayer currUser;
        string curUserName;

        public static void Main(string[] args)
        {
            I_ServiceLayer tmp_sl = new sadna192.ServiceLayer();
            tmp_sl.Create_ServiceLayer(new Stubs.Stub_deliverySystem(), new Stubs.Stub_paymentSystem(), "User1111 ", "Aa111111");
            init(tmp_sl);
            CreateWebHostBuilder(args).Build().Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>();

        public static void init(I_ServiceLayer SL)
        {

            I_User_ServiceLayer UL = SL.Connect();

            string[] lines = File.ReadAllLines(@"C:\Users\Ron\Desktop\file.txt", Encoding.UTF8);
            int sizez = lines.Length;
            for (int i = 0; i < sizez; i++)
            {
                String currentLine = lines[i];
                string[] currentLineSplitted = currentLine.Split(" ");
                executeLineFromFile(currentLineSplitted, UL);
            }


            /* foreach (string line in lines)
                 Console.WriteLine(line);
             I_User_ServiceLayer UL = SL.Connect();
             UL.Register("initUser", "1234Abcd");
             UL.Login("initUser", "1234Abcd");
             UL.Open_Store("testStore");
             UL.Add_Product_Store("testStore", "Banna", "food", 5.0, 50, new noDiscount(), new regularPolicy());
             UL.Add_Product_Store("testStore", "Apple", "food", 5.0, 70, new noDiscount(), new regularPolicy());
             UL.Add_To_ShopingBasket(UL.GlobalSearch("Banna", null, null, -1, -1, -1, -1)[0],5);
             UL.Add_To_ShopingBasket(UL.GlobalSearch("Apple", null, null, -1, -1, -1, -1)[0], 5);
             UL.Logout();*/
        }

        private static void executeLineFromFile(string[] currentLineSplitted, I_User_ServiceLayer UL)
        {

            if (currentLineSplitted[0].Equals("Register"))
            {
                UL.Register(currentLineSplitted[1], currentLineSplitted[2]);
            }
            else if (currentLineSplitted[0].Equals("Login"))
            {
                UL.Login(currentLineSplitted[1], currentLineSplitted[2]);
            }
            else if (currentLineSplitted[0].Equals("Open_Store"))
            {
                UL.Open_Store(currentLineSplitted[1]);
            }
            else if (currentLineSplitted[0].Equals("Add_Product_Store"))
            {
                if (currentLineSplitted[6].Equals("noDiscount") && currentLineSplitted[7].Equals("regularPolicy"))
                {
                    UL.Add_Product_Store(currentLineSplitted[1], currentLineSplitted[2], currentLineSplitted[3], double.Parse(currentLineSplitted[4]), Int32.Parse(currentLineSplitted[5]), new noDiscount(), new regularPolicy());
                }
            }
            else if (currentLineSplitted[0].Equals("Add_To_ShopingBasket"))
            {
                UL.Add_To_ShopingBasket(UL.GlobalSearch(currentLineSplitted[1], null, null, double.Parse(currentLineSplitted[4]), double.Parse(currentLineSplitted[5]), double.Parse(currentLineSplitted[6]), double.Parse(currentLineSplitted[7]))[int.Parse(currentLineSplitted[8])], int.Parse(currentLineSplitted[9]));
            }
            else if (currentLineSplitted[0].Equals("Logout"))
            {
                UL.Logout();
            }
            else if (currentLineSplitted[0].Equals("Add_Store_Manager"))
           
                if (currentLineSplitted[3].Equals("1") && currentLineSplitted[4].Equals("1") && currentLineSplitted[5].Equals("1"))
                {
                    UL.Add_Store_Manager(currentLineSplitted[1], currentLineSplitted[2], true, true, true);
                }
                else if (currentLineSplitted[3].Equals("0") && currentLineSplitted[4].Equals("0") && currentLineSplitted[5].Equals("0"))
                {
                    UL.Add_Store_Manager(currentLineSplitted[1], currentLineSplitted[2], false, false, false);
                }
                else if (currentLineSplitted[3].Equals("0") && currentLineSplitted[4].Equals("0") && currentLineSplitted[5].Equals("1"))
                {
                    UL.Add_Store_Manager(currentLineSplitted[1], currentLineSplitted[2], false, false, true);
                }
                else if (currentLineSplitted[3].Equals("0") && currentLineSplitted[4].Equals("1") && currentLineSplitted[5].Equals("0"))
                {
                    UL.Add_Store_Manager(currentLineSplitted[1], currentLineSplitted[2], false, true, false);
                }
                else if (currentLineSplitted[3].Equals("0") && currentLineSplitted[4].Equals("1") && currentLineSplitted[5].Equals("1"))
                {
                    UL.Add_Store_Manager(currentLineSplitted[1], currentLineSplitted[2], false, true, true);
                }
                else if (currentLineSplitted[3].Equals("1") && currentLineSplitted[4].Equals("0") && currentLineSplitted[5].Equals("0"))
                {
                    UL.Add_Store_Manager(currentLineSplitted[1], currentLineSplitted[2], true, false, false);
                }
                else if (currentLineSplitted[3].Equals("1") && currentLineSplitted[4].Equals("0") && currentLineSplitted[5].Equals("1"))
                {
                    UL.Add_Store_Manager(currentLineSplitted[1], currentLineSplitted[2], true, false, true);
                }
                else if (currentLineSplitted[3].Equals("1") && currentLineSplitted[4].Equals("1") && currentLineSplitted[5].Equals("0"))
                {
                    UL.Add_Store_Manager(currentLineSplitted[1], currentLineSplitted[2], true, true, false);
                }

            }


            /*private void CheckUser(String wantedUser, Boolean loginAction)
            {
                if(wantedUser != curUserName)
                {
                    currUser.Logout();
                    curUserName = wantedUser;
                    if (!loginAction)
                        currUser.Login(wantedUser, "needTo");
                }
            }*/

        }
    

    
}
