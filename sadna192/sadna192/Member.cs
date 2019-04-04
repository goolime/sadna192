﻿using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
namespace sadna192
{
    internal class Member : Visitor
    {
        private string name;
        private string code;
        private List<Owner> owner;


        public Member(string name, string password) : base()
        {
            this.name = name;
            this.code = this.Encrypt(this.name, password);
        }

        public override bool isMember()
        {
            return true;
        }

        public bool isMe(string other)
        {
            return other == this.name;
        }

        // this code was taken from https://www.c-sharpcorner.com/UploadFile/f8fa6c/data-encryption-and-decryption-in-C-Sharp/
        private string Encrypt(string input, string key)
        {
            byte[] inputArray = UTF8Encoding.UTF8.GetBytes(input);
            TripleDESCryptoServiceProvider tripleDES = new TripleDESCryptoServiceProvider();
            tripleDES.Key = UTF8Encoding.UTF8.GetBytes(key);
            tripleDES.Mode = CipherMode.ECB;
            tripleDES.Padding = PaddingMode.PKCS7;
            ICryptoTransform cTransform = tripleDES.CreateEncryptor();
            byte[] resultArray = cTransform.TransformFinalBlock(inputArray, 0, inputArray.Length);
            tripleDES.Clear();
            return Convert.ToBase64String(resultArray, 0, resultArray.Length);
        }

        internal bool check(string user_name, string user_pass)
        {
            return (this.code == this.Encrypt(user_name, user_pass));
        }

        internal Owner getUserStore(string store_name)
        {
            foreach (Owner owner in this.owner)
            {
                if (owner.getStore().isMe(store_name)) return owner;
            }
            throw new Exception("the user is not associated with the store '" + store_name + "'");
        }

        public override bool Add_Product_Store(string Store_name, string product_name, string product_category, double product_price, int product_amount, Discount product_discount, Policy product_policy)
        {
            Owner s = this.getUserStore(Store_name);
            return s.addProduct(product_name, product_category,product_price,product_amount,product_discount,product_policy);
        }

        public override bool Add_Store_Manager(string Store_name, Member new_manager, bool permision_add, bool permission_remove, bool permission_update)
        {
            Owner s = this.getUserStore(Store_name);
            return s.addManager(new_manager, permision_add, permission_remove, permission_update);
        }

        public override bool Add_Store_Owner(string Store_name, Member new_owner_name)
        {
            Owner s = this.getUserStore(Store_name);
            return s.addOwner(Store_name, new_owner_name);
        }

        public override bool Remove_Product_Store(string Store_name, string product_name)
        {
            Owner s = this.getUserStore(Store_name);
            return s.removeProduct(product_name);
        }

        public override bool Remove_Store_Manager(string Store_name, Member other_Manager)
        {
            Owner s = this.getUserStore(Store_name);
            return s.removeManager(other_Manager);
        }

        public override bool Remove_Store_Owner(string Store_name, Member other_Owner)
        {
            Owner s = this.getUserStore(Store_name);
            return s.removeOwner(other_Owner);
        }

        public override bool Update_Product_Store(string Store_name, string product_name, string product_new_name, string product_new_category, double product_new_price, int product_new_amount, Discount product_new_discount, Policy product_new_policy)
        {
            Owner s = this.getUserStore(Store_name);
            return s.updateProduct(product_name, product_new_name, product_new_category, product_new_price, product_new_amount, product_new_discount, product_new_policy);
        }
    }
}