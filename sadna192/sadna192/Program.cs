using System;
using System.Collections.Generic;
using System.Linq;
using F23.StringSimilarity;
using sadna192;


public class Program
{
    public static void Main(string[] args)
    {
        var l = new NormalizedLevenshtein();
        
        Console.WriteLine(l.Distance("My string", "my string"));
        Console.WriteLine(l.Distance("My string", "My dtring"));
        Console.WriteLine(l.Distance("My string", "My stttring"));

        
        /*ember m = new Member("Banani983", "Banani983");
        DBAccess.SaveToDB(m);
       Member m2 = new Member("Banani983", "Banani983");
        DBAccess.SaveToDB(m2);*/


          I_ServiceLayer serviceLayer = new ServiceLayer();
         try
         {
             serviceLayer.Create_ServiceLayer( new Stub_deliverySystem(),  new Stub_paymentSystem(), "admin123", "1234AsdF");
       }
       catch (Exception) { Console.WriteLine("66666666"); }
       I_User_ServiceLayer userServiceLayer2 = serviceLayer.Connect();
       userServiceLayer2.Register("bobi", "9876ASdf");
       Console.WriteLine(userServiceLayer2.Login("bobi", "9876ASdf"));
        userServiceLayer2.Open_Store("the store");
        //Console.WriteLine(userServiceLayer2.Add_Product_Store("the store", "eggs", "food", 13.4, 63, noDiscount.creteNoDiscount(), regularPolicy.creteRegularPolicy()));
        //
        //Console.WriteLine(userServiceLayer2.Update_Product_Store("the store", "eggs", "organic eggs", "food", 8.9, 90, null, null));
         I_User_ServiceLayer userServiceLayer1 = serviceLayer.Connect();
        // I_User_ServiceLayer userServiceLayer2 = serviceLayer.Connect();
          userServiceLayer1.Register("Banani983", "Banani983");
        // userServiceLayer2.Register("Banani9832", "Banani9832");
         // Console.WriteLine(userServiceLayer2.Login("Banani9832", "Banani9832"));
          Console.WriteLine(userServiceLayer1.Login("Banani983", "Banani983"));
        Console.ReadKey();
        Console.WriteLine(userServiceLayer2.Add_Store_Owner("the store", "Banani983")); 
            //userServiceLayer1.Open_Store("naimig");



        Console.WriteLine("END"); 
        Console.ReadKey();
    }



        public static void printList (List<ProductInStore> lst)
    {
        Console.WriteLine(">>>>> start print list: ");
        foreach (ProductInStore p in lst)
        {
            Console.WriteLine(p.product.name + " | " + p.id + " | " + p.store.storeID); 
        }
        Console.WriteLine("<<<<< end print list: ");
    }

    public class Stub_deliverySystem : I_DeliverySystem
    {
        int index = 0;
        bool isConnect = false;

        public Stub_deliverySystem()
        {
            isConnect = true;
        }

        public virtual void canclePackage(string code)
        {

        }

        public virtual bool check_address(string address)
        {
            return true;
        }

        public virtual bool Connect()
        {
            return isConnect;
        }

        public virtual string sendPackage(string address)
        {
            string ans = "test " + index;
            index++;
            return ans;
        }
    }

    public class Stub_paymentSystem : I_PaymentSystem
    {
        bool isConnect = false;

        public Stub_paymentSystem()
        {
            isConnect = true;
        }

        public virtual bool check_payment(string payment)
        {
            return true;
        }

        public virtual bool Connect()
        {
            return isConnect;
        }

        public virtual void pay(double total, string payment)
        {

        }
    }
}