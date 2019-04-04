using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sadna192
{
    public class Tools
    {
        private static string leagalCapitalLetters = "QWERTYUIOPASDFGHJKLZXCVBNM";
        private static string leagalSmallLetters = "qwertyuiopasdfghjklzxcvbnm";
        private static string leagalNumbers = "0123456789";

        public static bool check_username(string name)
        {
            if (name.Length < 4) throw new Exception("name must contain at least 4 charecters");
            if (!isAlfa(name.ToCharArray()[0])) throw new Exception("name must start with a letter");
            if (!isAlfaNumeric(name)) throw new Exception("name must contain only english letters and numbers");
            return true;
        }

        private static bool isAlfaNumeric(string name)
        {
            foreach (char c in name.ToCharArray())
            {
                if (!isAlfa(c) && !isNumber(c)) throw new Exception("char '" + c + "' is not leagal");
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
            if (pass.Length < 8 || pass.Length > 16) throw new Exception("passowrd must be atleast 8 chars long ans not longer than 16");
            if (!isAlfaNumeric(pass)) throw new Exception("password must contain only english letters and numbers");
            if (!containOneofEach(pass)) throw new Exception("password must contain atleast one small letter one capitol letter and one letter");
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
            if (name.Length < 4 || name.Length > 32) throw new Exception("the shop name must be longer then 4 chars and shorter then 32");
            if (!isAlfa(name.ToCharArray()[0])) throw new Exception("store name must start with a letter");
            if (!isAlfaNumeric(name)) throw new Exception("store name must contain only english letters and numbers");
            return true;
        }

        public static bool check_productNames(string name)
        {
            if (name.Length < 4 || name.Length > 32) throw new Exception("the product name must be longer then 4 chars and shorter then 32");
            if (!isAlfa(name.ToCharArray()[0])) throw new Exception("product name must start with a letter");
            if (!isAlfaNumeric(name)) throw new Exception("product name must contain only english letters and numbers");
            return true;
        }

        public static bool check_productCategory(string category)
        {
            if (category.Length < 4 || category.Length > 32) throw new Exception("the product name must be longer then 4 chars and shorter then 32");
            foreach(char c in category)
            {
                if(!isAlfa(c)) throw new Exception("char '" + c + "' is not leagal in category name");
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
