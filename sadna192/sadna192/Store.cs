using System;
using System.Collections.Generic;
using F23.StringSimilarity;



namespace sadna192
{
    public class Store
    {
        private string name;
        private List<ProductInStore> productInStores= new List<ProductInStore>();
        private List<Owner> owners;
        private static NormalizedLevenshtein similarety = new NormalizedLevenshtein();

        public Store(string name)
        {
            this.name = name;
            this.productInStores = new List<ProductInStore>();
            this.owners = new List<Owner>();
        }

        public string getName()
        {
            return name;
        }

        public List<ProductInStore> getProductInStore()
        {
            return productInStores;
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
                ProductInStore oldPr = FindProductInStore(product_name);
                oldPr.setAmount (product_amount);
                oldPr.setPrice(product_price);
                oldPr.setDiscount(product_discount);
                oldPr.setPolicy(product_policy);
                return true;
            }
            catch
            {
                Product pr = Product.getProduct(product_name, product_category, product_price);
                ProductInStore P = new ProductInStore(pr, product_amount, product_price, this, product_discount, product_policy);
                this.productInStores.Add(P);
                return true;
            }
        }

        internal bool removeProduct(string product_name)
        {
            foreach(ProductInStore p in productInStores)
            {
                if (p.getName() == product_name)
                {
                    productInStores.Remove(p);
                    return true;
                }
            }
            throw new Exception("Product to be removed was not found");
            
        }

        internal bool updateProduct(string product_name, string product_new_name, string product_new_category, double product_new_price, int product_new_amount, Discount product_new_discount, Policy product_new_policy)
        {
            
            ProductInStore p = this.FindProductInStore(product_name);

            if(product_new_name != null)
            {
                Product newProductWithName = Product.getProduct(product_new_name, p.getCategory(), p.getRank());
                p.setProduct(newProductWithName);
            }

            if(product_new_category != null)
            {
                Product tmp = p.getProduct();
                tmp.setCategory(product_new_category);
            }

            if(product_new_price != -1)
            {
                p.setPrice(product_new_price);
            }


            if(product_new_amount != -1)
            {
                p.setAmount(product_new_amount);
            }

            if (product_new_discount != null)
            {
                p.setDiscount(product_new_discount);
            }

            if (product_new_policy != null)
            {
                p.setPolicy(product_new_policy);
            }
            return true;
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
            throw new Exception("The Product " + product_name + " wasn't found in the store " + this.getName());
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
            if (price_min != -1)
            {
                ans = SearchProductByMinPriceRange(price_min, ans);
            }
            if (price_max != -1)
            {
                ans = SearchProductByMaxPriceRange(price_max, ans);
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
                    if (p.getProduct().getKeywords().Contains(keyword))
                    {
                        listToReturn.Add(p);
                    }
                }
            }
            return listToReturn;
        }

        private List<ProductInStore> SearchProductByMinPriceRange(double price_min, List<ProductInStore> list)
        {
            List<ProductInStore> productsResult = new List<ProductInStore>();
            foreach (ProductInStore p in list)
            {
                if (p.getPrice() > price_min)
                {
                    productsResult.Add(p);
                }
            }
            return productsResult;
        }

        private List<ProductInStore> SearchProductByMaxPriceRange(double price_max, List<ProductInStore> list)
        {
            List<ProductInStore> productsResult = new List<ProductInStore>();
            foreach (ProductInStore p in list)
            {
                if (p.getPrice() < price_max)
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
            int currentAmount = p.getAmount();
            if (currentAmount - amount < 0)
            {
                throw new Exception("The Amount to purchase os more tham the amount available in the store for this moment");
            }
            else
            {
                try
                {
                    p.setAmount(currentAmount - amount);
                    return true;
                }
                catch
                {

                }
                return false;
            }
        }


    }
}