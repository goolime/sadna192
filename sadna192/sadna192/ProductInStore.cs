using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace sadna192
{
    public class ProductInStore
    {
        public int id { get; set; }
        public int productID { get; set; }
        [ForeignKey("productID")]
        public virtual Product product { get; set; }
        public int amount { get; set; }
        public double price { get; set; }
        public int storeID { get; set; }
        [ForeignKey("storeID")]
        public Store store { get; set; }
        public int discountID { get; set; }
        [ForeignKey("discountID")]
        public Discount discount { get; set; }
        public int policyID { get; set; }
        [ForeignKey("policyID")]
        public Policy policy { get; set; }

        public ProductInStore() { }
        public ProductInStore(Product product, int amount, double price, Store store, Discount discount, Policy policy)
        {
            this.product = product;
            this.amount = amount;
            this.price = price;
            this.store = store;
            this.discount = discount;
            this.policy = policy;
        }

        // ============================= //
        // ========== Getters ========== //
        // ============================= //

        public Policy getPolicy()
        {
            return this.policy;
        }

        public Product getProduct()
        {
            return product;
        }

        public int getAmount()
        {
            return amount;
        }

        public double getPrice()
        {
            return price;
        }

        public Store getStore()
        {
            return store;
        }

        public Discount getDiscount()
        {
            return discount;
        }

        internal Policy GetPolicy()
        {
            return policy;
        }

        public string getName()
        {
            return product.getName();
        }

        public string getCategory()
        {
            return product.getCategory();
        }

        public double getRank()
        {
            return product.getRank();
        }



        // ============================= //
        // ========== Setters ========== //
        // ============================= //

        public void setAmount(int amount)
        {
           this.amount=amount;
        }

        public void setProduct(Product p)
        {
            this.product = p;
        }

        public void setPrice(double price)
        {
            this.price = price;
        }

        public void setDiscount(Discount discount)
        {
            this.discount = discount;
        }

        public void setPolicy(Policy policy)
        {
            this.policy = policy;
        }

        
    }
}