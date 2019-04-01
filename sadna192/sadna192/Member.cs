using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
namespace sadna192
{
    internal class Member : Visitor
    {
        private string name;
        private string code;

        public Member(string name, string password)
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

        internal Store getUserStore(string store_name)
        {
            throw new NotImplementedException();
        }
    }
}