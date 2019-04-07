using System;

namespace sadna192
{
    public interface Policy
    {
        bool check();
        bool immidiate();
    }

    public class regularPolicy: Policy
    {
        public bool check()
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
        public bool check()
        {
            return true;
        }
       
        public bool immidiate()
        {
            return 
        }
    }
}