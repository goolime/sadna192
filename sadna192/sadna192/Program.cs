using System;
using System.Collections.Generic;
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

        Product p = new Product("aaaa", "qqqq", 5, keywords);
        Member m = new Member("name12", "name12");
        Store s = new Store("s"); 
        Owner o = new Owner(m, new Store("store"));
        Owner o1 = new Owner(m, s);
        Member m1 = new Member("namerew43", "name12");
        Owner o2 = new Owner(m1, new Store("retsd"));
        Manager man = new Manager(m1,s, true, false, true); 
        o2.has_Apointed.Add(o1);
        using (var ctx = new sadna192.Model1())
        {
            ctx.Products.Add(p);
            ctx.Members.Add(m);
          //  ctx.Stores.Add(s); 
            ctx.Owners.Add(o);
            ctx.Members.Add(m1);
            ctx.Owners.Add(o1);
            ctx.Owners.Add(o2);
            ctx.Managers.Add(man); 
            ctx.SaveChanges();
        }
       // Product p2 = Product.getProduct("erty", "rhfss", 34);
        
        Console.ReadKey();
    }
}