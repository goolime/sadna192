using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using F23.StringSimilarity;



namespace sadna192
{
    public class Store
    {
        public int storeID { get; set; }
        public string name { get; set; }
        public List<ProductInStore> productInStores { get; set; }
        public List<Owner> owners { get; set; }
        public int policyID { get; set; }
        [ForeignKey("policyID")]
        public Policy storePolicy { get; set; }
        public int discountID { get; set; }
        [ForeignKey("discountID")]
        public Discount storeDiscount { get; set; }

        public Store() { }

        public Store(string name)
        {
            this.name = name;
            this.productInStores = new List<ProductInStore>();
            this.owners = new List<Owner>();
            this.storePolicy = new regularPolicy();
            this.storeDiscount = new noDiscount();
        }

        public Store(Store store)
        {
            this.name = "" + store.name;
            this.productInStores = new List<ProductInStore>();
            this.owners = new List<Owner>();
            foreach(ProductInStore p in store.productInStores)
            {
                this.productInStores.Add(new ProductInStore(p,this));
            }
            foreach (Owner o in store.owners)
            {
                this.owners.Add(o);
            }
            this.storePolicy = store.storePolicy.Copy();
            this.storeDiscount = store.storeDiscount.Copy();
        }

        public Policy GetPolicy()
        {
            return this.storePolicy;
        }

        public Discount GetDiscount()
        {
            return this.storeDiscount;
        }

        public string getName()
        {
            return name;
        }

        public List<ProductInStore> getProductInStore()
        {
            return productInStores;
        }

        public ProductInStore getProductInStore(string name)
        {
            ProductInStore ans = null;
            foreach (ProductInStore p in this.productInStores)
            {
                if (name == p.getProduct().name) ans = p;
            }

            return ans;
        }

        internal bool isMe(string name)
        {
            return this.name == name;
        }

        internal void addOwner(Owner owner)
        {
            this.owners.Add(owner);
        }

        internal bool addProduct(string product_name, string product_category, double product_price, int product_amount, Discount product_discount, Policy product_policy)
        {
            //checks if the product is already exists, if not - adds it.
            try
            {
                return DBAccess.updateProductInStoreIfExist(this.name, product_name, product_price, product_amount, product_discount, product_policy);
            }
            catch
            {
                Product pr = Product.getProduct(product_name, product_category, product_price);
                ProductInStore P = new ProductInStore(pr, product_amount, product_price, this, product_discount, product_policy);
                if (this.productInStores == null)
                    this.productInStores = new List<ProductInStore>();
                this.productInStores.Add(P);
                if (!DBAccess.SaveToDB(P))
                    DBAccess.DBerror("could not save ProductInStore to DB");
                return true;
            }
        }

        internal bool removeProduct(string product_name)
        {
            bool ans = DBAccess.removeProductInStore(this.name, product_name);
            if (!ans)
                throw new Sadna192Exception("Product to be removed was not found", "Store", "removeProduct");
            return ans;

        }

        internal bool updateProduct(string product_name, string product_new_name, string product_new_category, double product_new_price, int product_new_amount, Discount product_new_discount, Policy product_new_policy)
        {
            return DBAccess.updateProductInStore(this.name, product_name, product_new_name, product_new_category, product_new_price, product_new_amount, product_new_discount, product_new_policy);
        }
        
        //
        //This fuction search for a specific product by name in the products in store list.
        // in case of finding the product, returns this product.
        //
        internal ProductInStore FindProductInStore(String product_name)
        {
            foreach(ProductInStore p in productInStores)
            {
                if (p.getName() == product_name)
                {
                    return p;
                }
            }
            throw new Sadna192Exception("The Product " + product_name + " wasn't found in the store " + this.getName(), "Store", "FindProductInStore");
        }

        internal List<ProductInStore> Search(string name, string category, List<string> keywords, double price_min, double price_max, double store_rank, double product_rank)
        {
            List<ProductInStore> ans = new List<ProductInStore>();
            ans.AddRange(this.productInStores.ToArray());

            //Searching By Name
            if(name!=null)
            {
                ans = SearchProductByName(name,ans);
            }
            //Searching By Category
            if (category != null)
            {
                ans = SearchProductByCategory(category, ans);
            }
            //Searching By Keywords
            if (keywords != null)
            {
                ans =  SearchProductByKeywords(keywords, ans);
            }
            //Searching by price range
            if (price_min != -1 && price_max != -1)
            {
                ans = SearchProductByMinPriceRangeBoth(price_min,price_max, ans);
            }
            if (price_min != -1 &&  price_max==-1)
            {
                ans = SearchProductByMinPriceOnlyRange(price_min, ans);
            }
            if (price_max != -1 && price_min==-1)
            {
                ans = SearchProductByMaxPriceOnlyRange(price_max, ans);
            }
            if (product_rank != -1)
            {
                ans = SearchProductByProductRank(product_rank, ans);
            }
            return ans;
        }

        internal void removeApointed(Owner owner)
        {
            this.owners.Remove(owner);
        }

        //Implementation of the searching methods
        private List<ProductInStore> SearchProductByName(string name, List<ProductInStore> list)
        {
            NormalizedLevenshtein similarety = new NormalizedLevenshtein();
            List<ProductInStore> productsResult = new List<ProductInStore>();
            foreach(ProductInStore p in list)
            {
                if (similarety.Distance(name,p.getName())<0.2)
                {
                    productsResult.Add(p);
                }
            }
            return productsResult;
        }

        private List<ProductInStore> SearchProductByCategory(string category, List<ProductInStore> list)
        {
            NormalizedLevenshtein similarety = new NormalizedLevenshtein();
            List<ProductInStore> productsResult = new List<ProductInStore>();
            foreach (ProductInStore p in list)
            {
                if (similarety.Distance(category,p.getCategory())<0.2)
                {
                    productsResult.Add(p);
                }
            }
            return productsResult;
        }

        private List<ProductInStore> SearchProductByKeywords(List<string> keywords, List<ProductInStore> list)
        {
            List < ProductInStore > listToReturn = new List<ProductInStore>();
            foreach (String keyword in keywords)
            {
                foreach(ProductInStore p in list)
                {
                    if (p.getProduct().getKeywords() != null && p.getProduct().getKeywords().Contains(keyword))
                    {
                        listToReturn.Add(p);
                    }
                }
            }
            return listToReturn;
        }

        private List<ProductInStore> SearchProductByMinPriceRangeBoth(double price_min, double price_max, List<ProductInStore> list)
        {
            List<ProductInStore> productsResult = new List<ProductInStore>();
            foreach (ProductInStore p in list)
            {
                if (p.getPrice() >= price_min && p.getPrice() <= price_max)
                {
                    productsResult.Add(p);
                }
            }
            return productsResult;
        }


        private List<ProductInStore> SearchProductByMinPriceOnlyRange(double price_min, List<ProductInStore> list)
        {
            List<ProductInStore> productsResult = new List<ProductInStore>();
            foreach (ProductInStore p in list)
            {
                if (p.getPrice() >= price_min)
                {
                    productsResult.Add(p);
                }
            }
            return productsResult;
        }

        private List<ProductInStore> SearchProductByMaxPriceOnlyRange(double price_max, List<ProductInStore> list)
        {
            List<ProductInStore> productsResult = new List<ProductInStore>();
            foreach (ProductInStore p in list)
            {
                if (p.getPrice() <= price_max)
                {
                    productsResult.Add(p);
                }
            }
            return productsResult;
        }

        
        private List<ProductInStore> SearchProductByProductRank(double rank, List<ProductInStore> list)
        {
            List<ProductInStore> productsResult = new List<ProductInStore>();
            foreach (ProductInStore p in list)
            {
                if (p.getRank() == rank)
                {
                    productsResult.Add(p);
                }
            }
            return productsResult;
        }


        //Change amount in case of purchase
        public bool ChangeAmoutStoreInPurchase(ProductInStore p, int amount)
        {
            //checking if the amount to purcahse is available in the store
            if (!(Tools.check_amount(p.getAmount() - amount)))
            {
                throw new Sadna192Exception("The Amount to purchase os more tham the amount available in the store for this moment", "Store", "ChangeAmoutStoreInPurchase");
            }
            else
            {
                    p.setAmount(p.getAmount() - amount);
                    return true;
            }
        }

        public List<Owner> GetOwners()
        {
            return this.owners;
        }
    }
}