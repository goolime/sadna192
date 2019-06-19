using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Infrastructure;

namespace sadna192
{
    [Table("Products")]
    public class Product
    {
        [Key]
        public int id { get; set; }
        public string name { get; set; }
        public string category { get; set; }
        public double rank { get; set; }
        private List<string> keywords;
        private static List<Product> allProducts;


        public static Product getProduct(string name, string category, double rank)
        {
            if (allProducts == null) allProducts = new List<Product>();
            foreach (Product p in allProducts)
            {
                if (p.getName() == name)
                {
                    Console.WriteLine("found product name " + name);
                    return p;
                }
            }
          //  Console.WriteLine("all pis o1k ");
            Product pr = new Product(name, category, rank);
          //  Console.WriteLine("allis o3k ");
            allProducts.Add(pr);
            //    Console.WriteLine("all products is ok4 ");
            if (!pr.saveProductToDB())
                Console.WriteLine("could not save Product to DB  -TODO!!!!!");

            return pr;


        }

        private bool saveProductToDB()
        {
            try
            {
                using (var ctx = new sadna192.Model1())
                {
                    // Console.WriteLine("%%%%%%%%%%%%%%");
                    ctx.Products.Add(this);
                    //  Console.WriteLine("%%%%%%%%%%%%%%2");
                    ctx.SaveChanges();
                    //  Console.WriteLine("%%%%%%%%%%%%%%2");
                    return true;
                }
            }
            catch (DbUpdateException e)
            {
                return false;
            }
        }

        public Product(string name, string category, double rank)
        {
            
            this.name = name;
            this.category = category;
            this.rank = rank;
            this.keywords = null;
        }

        private Product(string name, string category, double rank, List<string> keywords)
        {
            this.name = name;
            this.category = category;
            this.rank = rank;
            this.keywords = keywords;
        }


        // ============================= //
        // ========== Getters ========== //
        // ============================= //

        public string getName()
        {
            return name;
        }

        public string getCategory()
        {
            return category;
        }

        public double getRank()
        {
            return rank;
        }

        public List<string> getKeywords()
        {
            return keywords;
        }


        // ============================= //
        // ========== Setters ========== //
        // ============================= //

        public void setName(string name)
        {
            this.name = name;
        }

        public void setCategory(string category)
        {
            this.category = category;
        }

        public void SetRank(string rank) => this.rank = double.Parse(rank);

        public void setKeywords(List<string> keywords)
        {
            this.keywords = keywords;
        }


    }



}