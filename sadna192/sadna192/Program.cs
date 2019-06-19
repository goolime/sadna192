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
        using (var ctx = new sadna192.Model1())
        {
            ctx.Products.Add(p);
            ctx.SaveChanges();
        }
       // Product p2 = Product.getProduct("erty", "rhfss", 34);
        
        Console.ReadKey();
    }
}