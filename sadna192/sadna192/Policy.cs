﻿using System;
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

    public class OrPolicy : MultiplePolicy
    {
        public OrPolicy(List<Policy> l):base()
        {
            this.Policies = l;
        }
        public override bool check(ProductInStore p, UserState u)
        {
            foreach (Policy poli in this.Policies)
            {
                if (poli.check(p, u)) return true;
            }
            return false;
        }

        public override bool immidiate()
        {
            foreach (Policy poli in this.Policies)
            {
                if (poli.immidiate()) return true;
            }
            return false;
        }
    }

    public class AndPolicy : MultiplePolicy
    {
        public AndPolicy(List<Policy> l) : base()
        {
            this.Policies = l;
        }
        public override bool check(ProductInStore p, UserState u)
        {
            foreach (Policy poli in this.Policies)
            {
                if (!poli.check(p, u)) return false;
            }
            return true;
        }

        public override bool immidiate()
        {
            foreach (Policy poli in this.Policies)
            {
                if (!poli.immidiate()) return false;
            }
            return true;
        }
    }
}