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
        Member m = new Member("name12T", "name12T");
        Store s = new Store("s"); 
        Owner o = new Owner(m, new Store("store"));
        Owner o1 = new Owner(m, s);
        Member m1 = new Member("namerew43", "name12");
        Owner o2 = new Owner(m1, s);
        Manager man = new Manager(m1,s, true, false, true); 
        o2.has_Apointed.Add(o1);
        Policy p1 = new regularPolicy();
        Policy p2 = new MinimumInCart(3);
        Policy p3 = new MinimumProductInCart(2);
        Policy p4 = new MaximumInCart(8);
        DateTime dateFrom = new DateTime(2005, 12, 12);
        DateTime dateTo = new DateTime(2015, 12, 12);
        Policy p5 = new TimePolicy(dateFrom, dateTo); 
        List<Policy> pl = new List<Policy>();
        pl.Add(p1);
        pl.Add(p2);
        MultiplePolicy mp = new AndPolicy(pl);
        List<Policy> pl2 = new List<Policy>();
        pl2.Add(p1);
        pl2.Add(p3);
        pl2.Add(p5);
        MultiplePolicy mp1 = new OrPolicy(pl2);
        s.storePolicy = mp1;
        Discount discount = new fixDiscount(0.2);
        Discount d2 = new ProductAmountDiscount("apple", 4, 0.5);
        List<Discount> dl = new List<Discount>();
        dl.Add(discount);
        dl.Add(d2);
        multipleDiscount md = new AndDiscount(dl);
        ProductInStore ps = new ProductInStore(p, 40, 3, s, discount, mp);
        ItemsInCart items = new ItemsInCart(ps, 2);
        List<ItemsInCart> itemsList = new List<ItemsInCart>();
        itemsList.Add(items);
        ShoppingCart sc = new ShoppingCart(s, itemsList);
        List<ShoppingCart> scList = new List<ShoppingCart>();
        scList.Add(sc);
        ShopingBasket sb = new ShopingBasket(scList); 
        using (var ctx = new sadna192.Model1())
        {
            ctx.Products.Add(p);
            ctx.Members.Add(m);
            ctx.Stores.Add(s); 
            ctx.Owners.Add(o);
            ctx.Members.Add(m1);
            ctx.Owners.Add(o1);
            ctx.Owners.Add(o2);
            ctx.Managers.Add(man);
            ctx.Policies.Add(p1);
            ctx.Policies.Add(p2);
            ctx.Policies.Add(p3);
            ctx.Policies.Add(p4);
            ctx.Policies.Add(p5);
            ctx.Policies.Add(mp);
            ctx.Policies.Add(mp1);
            ctx.MultiplePolicies.Add(mp);
            ctx.MultiplePolicies.Add(mp1);
            ctx.Discounts.Add(discount);
            ctx.Discounts.Add(d2);
            ctx.MultipleDiscounts.Add(md);
            ctx.ProductInStores.Add(ps);
            ctx.ItemsInCarts.Add(items);
            ctx.ShoppingCarts.Add(sc);
            ctx.ShopingBaskets.Add(sb);
            ctx.SaveChanges();
        }
        // Product p2 = Product.getProduct("erty", "rhfss", 34);
         Console.ReadKey();
    }
}