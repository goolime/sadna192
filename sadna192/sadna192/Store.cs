using System;
using System.Collections.Generic;


namespace sadna192
{
    public class Store
    {
        private string name;
        private List<ProductInStore> productInStores= new List<ProductInStore>();

        public Store(string name)
        {
            this.name = name;
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
            throw new NotImplementedException();
        }

        internal void addManager(UserState userState, Member other_user, bool permision_add, bool permission_remove, bool permission_update)
        {
            throw new NotImplementedException();
        }

        internal bool addProduct(string product_name, string product_category, double product_price, int product_amount, Discount product_discount, Policy product_policy)
        {

            throw new NotImplementedException();
        }

        internal void addOwner(UserState userState, Member other_user)
        {
            throw new NotImplementedException();
        }

        internal List<ProductInStore> Search(string name, string category, List<string> keywords, double price_min, double price_max, double store_rank, double product_rank)
        {
            //Searching By Name
            if(name!=null && category==null && keywords == null && price_min == -1 && price_max == -1 && store_rank == -1 && product_rank == -1)
            {
                return SearchProductByName(name);
            }
            //Searching By Category
            else if (name==null && category != null && keywords==null && price_min == -1 && price_max == -1 && store_rank == -1 && product_rank== -1)
            {
                return SearchProductByCategory(category);
            }
            //Searching By Keywords
            else if (name==null && category==null && keywords != null && price_min== -1 && price_max==0 && store_rank== -1 && product_rank== -1)
            {
                return SearchProductByKeywords(keywords);
            }
            //Searching by price range
            else if (name == null && category == null && keywords == null && price_min != -1 && price_max != -1 && store_rank == -1 && product_rank == -1)
            {
                return SearchProductByPriceRange(price_min, price_max);
            }
            else if (name == null && category == null && keywords == null && price_min == -1 && price_max == -1 && store_rank == -1 && product_rank != -1)
            {
                return SearchProductByProductRank(product_rank);
            }

            throw new SystemException("Error in input of searching options");
        }

        internal void removeApointed(Owner owner)
        {
            throw new NotImplementedException();
        }

        internal bool removeProduct(string product_name)
        {
            throw new NotImplementedException();
        }



        //Implementation of the searching methods
        private List<ProductInStore> SearchProductByName(string name)
        {
            List<ProductInStore> productsResult = new List<ProductInStore>();
            foreach(ProductInStore p in productInStores)
            {
                if (p.getName() == name)
                {
                    productsResult.Add(p);
                }
            }
            return productsResult;
        }

        private List<ProductInStore> SearchProductByCategory(string category)
        {
            List<ProductInStore> productsResult = new List<ProductInStore>();
            foreach (ProductInStore p in productInStores)
            {
                if (p.getCategory() == category)
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

        private List<ProductInStore> SearchProductByKeywords(List<string> keywords)
        {
            throw new NotImplementedException();

        }

        private List<ProductInStore> SearchProductByPriceRange(double price_min, double price_max)
        {
            List<ProductInStore> productsResult = new List<ProductInStore>();
            foreach (ProductInStore p in productInStores)
            {
                if (p.getPrice() > price_min && p.getPrice() < price_max)
                {
                    productsResult.Add(p);
                }
            }
            return productsResult;
        }


        private List<ProductInStore> SearchProductByProductRank(double rank)
        {
            List<ProductInStore> productsResult = new List<ProductInStore>();
            foreach (ProductInStore p in productInStores)
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