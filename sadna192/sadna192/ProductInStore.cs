namespace sadna192
{
    public class ProductInStore
    {
        private Product product;
        private int amount;
        private int price;
        private Store store;
        private Discount discount;

        public ProductInStore(Product product, int amount, int price, Store store, Discount discount)
        {
            this.product = product;
            this.amount = amount;
            this.price = price;
            this.store = store;
            this.discount = discount;
        }

        public string getName()
        {
            return product.getName();
        }

        public string getCategory()
        {
            return product.getCategory();
        }

        public int getPrice()
        {
            return price;
        }

        public double getRank()
        {
            return product.getRank();
        }

        public int getAmount()
        {
            return amount;
        }

        public Store getStore()
        {
            return store;
        }

        public void setAmount(int amount)
        {
           this.amount=amount;
        }

    }
}