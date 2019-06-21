using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Infrastructure;
using System.Linq;

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
        private List<String> keywords; 
        public List<String> Keywords
        {
            get { return keywords; }
            set { keywords = value; }
        }

        [Required]
        public string KeywordsAsString
        {
            get { return String.Join(",", keywords); }
            set => keywords = value.Split(',').ToList();
        }
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
            Product pr = new Product(name, category, rank);
            allProducts.Add(pr);
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
                    ctx.Products.Add(this);
                    ctx.SaveChanges();
                    return true;
                }
            }
            catch (DbUpdateException e)
            {
                Console.WriteLine("save product to DB faild : " + e.ToString());
                return false;
            }
        }

        public Product(string name, string category, double rank)
        {
            
            this.name = name;
            this.category = category;
            this.rank = rank;
            this.keywords = new List<string>();
        }

        public Product(string name, string category, double rank, List<string> keywords)
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