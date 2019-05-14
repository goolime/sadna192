using System;

namespace sadna192
{
    interface Policy
    {
        bool Check(ProductInStore p, UserState u);
        bool immidiate();
    }

    class regularPolicy: Policy
    {
        public bool Check(ProductInStore pro, UserState user) { 
            return true;
        }

        public bool immidiate()
        {
            return false;
        }
    }

    class immidiatePolicy: Policy
    {
        public bool Check(ProductInStore pro, UserState user)
        {
            throw new NotImplementedException();
        }

        public bool immidiate()
        {
            return true; 
        }
    }
}