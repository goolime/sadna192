using System;
using System.Collections.Generic;

namespace sadna192
{
    public class Product
    {
        public int id { get; set; }
        public string name { get; set; }
        private string category;
        private double rank;
        private List<string> keywords;
        private static List<Product> allProducts;

        public static Product getProduct(string name, string category, double rank)
        {
            if (allProducts == null) allProducts = new List<Product>();
            foreach(Product p in allProducts)
            {
                if (p.getName() == name)
                {
                    return p;
                }
            }

            Product pr = new Product(name, category, rank);
            try
            {              
                using (var ctx = new Model1())
                {
                    ctx.Products.Add(pr);
                    ctx.SaveChanges();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Fail : " + e.ToString());
            }


            allProducts.Add(pr);
           
            return pr;


        }

        private Product(string name, string category, double rank)
        {
            this.name = name;
            this.category = category;
            this.rank = rank;
            this.keywords = new List<string>();
        }

        private Product(string name, string category, double rank, List<string> keywords)
        {
            this.name = name;
            this.category = category;
            this.rank = rank;
            if (keywords == null) this.keywords = new List<string>();
            else this.keywords = keywords;
        }

        public Product(Product product)
        {
            this.name = "" + product.name;
            this.category = "" + product.category;
            this.rank = product.rank;
            this.keywords = new List<string>();
            foreach( string s in product.keywords)
            {
                this.keywords.Add("" + s);
            }
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