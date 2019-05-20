using System;
using System.Linq;

namespace sadna192
{
    public class Tools
    {
        private static string leagalCapitalLetters = "QWERTYUIOPASDFGHJKLZXCVBNM";
        private static string leagalSmallLetters = "qwertyuiopasdfghjklzxcvbnm";
        private static string leagalNumbers = "0123456789 ";

        public static bool check_username(string name)
        {
            if (name.Length < 4) throw new Sadna192Exception("name must contain at least 4 charecters", "Tools" , "check_username(1)");
            if (!isAlfa(name.ToCharArray()[0])) throw new Sadna192Exception("name must start with a letter", "Tools", "check_username(2)");
            if (!isAlfaNumeric(name)) throw new Sadna192Exception("name must contain only english letters and numbers", "Tools", "check_username(3)");
            return true;
        }

        private static bool isAlfaNumeric(string name)
        {
            foreach (char c in name.ToCharArray())
            {
                if (!isAlfa(c) && !isNumber(c)) throw new Sadna192Exception("char '" + c + "' is not leagal", "Tools", "isAlfaNumeric");
            }
            return true;
        }

        private static bool isNumber(char c)
        {
            return leagalNumbers.Contains(c);
        }


        private static bool isAlfa(char v)
        {
            return leagalCapitalLetters.Contains(v) || leagalSmallLetters.Contains(v);
        }

        public static bool check_password(string pass)
        {
            if (pass.Length < 8 || pass.Length > 16) throw new Sadna192Exception("passowrd must be atleast 8 chars long ans not longer than 16", "Tools", "check_password(1)");
            if (!isAlfaNumeric(pass)) throw new Sadna192Exception("password must contain only english letters and numbers", "Tools", "check_password(2)");
            if (!containOneofEach(pass)) throw new Sadna192Exception("password must contain atleast one small letter one capitol letter and one letter", "Tools", "check_password(3)");
            return true;
        }

        private static bool containOneofEach(string pass)
        {
            int capitol = 0, smaller = 0, number = 0;
            foreach (char c in pass)
            {
                if (isNumber(c)) number++;
                if (leagalCapitalLetters.Contains(c)) capitol++;
                if (leagalSmallLetters.Contains(c)) smaller++;
            }
            return number > 0 && capitol > 0 && smaller > 0;
        }

        public static bool check_storeName(string name)
        {
            if (name.Length < 4 || name.Length > 32) throw new Sadna192Exception("the shop name must be longer then 4 chars and shorter then 32", "Tools", "check_storeName(1)");
            if (!isAlfa(name.ToCharArray()[0])) throw new Sadna192Exception("store name must start with a letter", "Tools", "check_storeName(2)");
            if (!isAlfaNumeric(name)) throw new Sadna192Exception("store name must contain only english letters and numbers", "Tools", "check_storeName(3)");
            return true;
        }

        public static bool check_productNames(string name)
        {
            if (name.Length < 4 || name.Length > 32) throw new Sadna192Exception("the product name must be longer then 4 chars and shorter then 32", "Tools", "check_productNames(1)");
            char c = name[0];
            if (!isAlfa(c)) throw new Sadna192Exception("product name must start with a letter", "Tools", "check_productNames(2)");
            if (!isAlfaNumeric(name)) throw new Sadna192Exception("product name must contain only english letters and numbers", "Tools", "check_productNames(3)");
            return true;
        }

        public static bool check_productCategory(string category)
        {
            if (category.Length < 4 || category.Length > 32) throw new Sadna192Exception("the product name must be longer then 4 chars and shorter then 32", "Tools", "check_productCategory(1)");
            foreach(char c in category)
            {
                if(!isAlfa(c)) throw new Sadna192Exception("char '" + c + "' is not leagal in category name", "Tools", "check_productCategory");
            }
            return true;
        }

        public static bool check_price(double price)
        {
            return price >= 0;
        }

        public static bool check_amount(int amount)
        {
            return amount>=0;
        }
    }
}
