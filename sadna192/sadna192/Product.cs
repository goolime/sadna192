namespace sadna192
{
    public class Product
    {
        private int id;
        private string name;
        private string category;
        private double rank;

        public Product(string name, int id, string category, double rank)
        {
            this.name = name;
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