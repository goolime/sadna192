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

            throw new NotImplementedException();
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

        internal bool removeProduct(string product_name)
        {
            throw new NotImplementedException();
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

        internal bool updateProduct(string product_name, string product_new_name, string product_new_category, double product_new_price, int product_new_amount, Discount product_new_discount, Policy product_new_policy)
        {
            throw new NotImplementedException();
        }



        // <<<<<<<<<<<<<<< ===============TODO=========== >>>>>>>>>>>>>>>>>>>>>
        // <<<<<<<<<<<<<<< ===============TODO=========== >>>>>>>>>>>>>>>>>>>>>
        // <<<<<<<<<<<<<<< ===============TODO=========== >>>>>>>>>>>>>>>>>>>>>

        private List<ProductInStore> SearchProductByKeywords(List<string> keywords, List<ProductInStore> list)
        {
            throw new NotImplementedException();

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
                throw new SystemException("The Amount to purchase os more tham the amount available in the store for this moment");
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