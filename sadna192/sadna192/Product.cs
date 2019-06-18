using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace sadna192
{
    [Table("Products")]
    public class Product
    {
        [Key]
        public int id { get; set; }
        private string name;
        private string category;
        private double rank;
        private List<string> keywords;
        private static List<Product> allProducts;


        public static Product getProduct(string name, string category, double rank)
        {
            if (allProducts == null) allProducts = new List<Product>();
            foreach (Product p in allProducts)
            {
                if (p.getName() == name)
                {
                    return p;
                }
            }

            Product pr = new Product(name, category, rank, 1);
            allProducts.Add(pr);
            return pr;


        }

        public Product(string name, string category, double rank, int id)
        {
            this.id = id;
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