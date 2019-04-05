using System;

namespace sadna192
{
    public class Product
    {
        private string name;
        private int id;
        private string category;
        private double rank;

        public Product(string name, int id, string category, double rank)
        {
            this.name = name;
            this.id = id;
            this.category = category;
            this.rank = rank;
        }

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



    }



}