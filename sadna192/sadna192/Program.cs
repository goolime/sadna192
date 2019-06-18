using System;
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
        Product p = new Product("Asfd", "assdfd", 342234.435, 1);
        using (var ctx = new sadna192.Model1())
        {
            Console.WriteLine("here");

            ctx.Products.Add(p);
            ctx.SaveChanges();
        }
        Console.ReadKey();
    }
}