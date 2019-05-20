using System;
using System.Collections.Generic;

namespace sadna192
{
    public interface Policy
    {
        bool check(ProductInStore p, UserState u);
        bool immidiate();
    }

    public class regularPolicy: Policy
    {
        public bool check(ProductInStore p, UserState u)
        {
            return true;
        }

        public bool immidiate()
        {
            return false;
        }
    }

    public class immidiatePolicy: Policy
    {
        public bool check(ProductInStore p, UserState u)
        {
            return true;
        }
       
        public bool immidiate()
        {
            return true; 
        }
    }

    public abstract class MultiplePolicy : Policy
    {
        internal List<Policy> Policies;

        public List<Policy> getPolicies()
        {
            List<Policy> ans = new List<Policy>();
            foreach (Policy p in this.Policies) ans.Add(p);
            return ans;
        }
        public abstract bool check(ProductInStore p, UserState u);
        public abstract bool immidiate();
    }
}