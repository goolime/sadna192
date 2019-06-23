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
        List<String> keywords = new List<string>();
        keywords.Add("keyword");
        Product pr = Product.getProduct("apple", "food", 10);
       
        Store s = new Store("shepo");
        DBAccess.SaveToDB(s);
        ProductInStore ps = new ProductInStore(pr, 34, 5, s, noDiscount.creteNoDiscount(), regularPolicy.creteRegularPolicy());
        DBAccess.SaveToDB(ps);
        List<ProductInStore> lst = DBAccess.searchProductInStore(null, null, null, 2, 15, -1);
        printList(lst);
        //  printList(searchPro(null, "food"));
        //  printList(searchPro("apple", "fruit" ));
        //  printList(searchPro("apple", null));

        /* Product pr1 = Product.getProduct("agg", "food", 11);
        Product pr2 = Product.getProduct("dingdong", "food", 8);
        Product pr3 = Product.getProduct("apple", "fruit", 6);
         * I_ServiceLayer serviceLayer = new ServiceLayer();
        try
        {
            serviceLayer.Create_ServiceLayer( new Stub_deliverySystem(),  new Stub_paymentSystem(), "admin123", "1234AsdF");
        }
        catch (Exception) { }
        I_User_ServiceLayer userServiceLayer1 = serviceLayer.Connect();
        userServiceLayer1.Register("ASSdo300", "ASSdo300");
        Console.WriteLine(userServiceLayer1.Login("ASSdo300", "ASSdo300"));
        */



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