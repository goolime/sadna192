using System;
using System.Data.SqlClient;
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
        //  Product p = new Product("Asfd2", "assdfd", 342234.435);
        Product p = Product.getProduct("dfgdfdfgd", "dfgdfg", 342234.435);
        using (var ctx = new sadna192.Model1(new SqlConnection("Data Source=132.73.195.209;Initial Catalog=Sadna;Integrated Security=False;User ID=sa; Password=rrr")))
        {
            Console.WriteLine("here");

            ctx.Products.Add(p);
            ctx.SaveChanges();
        }
        Console.ReadKey();
    }
}