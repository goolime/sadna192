using Microsoft.VisualStudio.TestTools.UnitTesting;
using sadna192;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AcceptanceTests
{
    [TestClass()]
    public class UpdateProductInStore_AT
    {
        private static I_ServiceLayer serviceLayer;
        private I_User_ServiceLayer userServiceLayer1;
        private I_User_ServiceLayer userServiceLayer2;
        private I_DeliverySystem deliverySystem;
        private I_PaymentSystem paymentSystem;

        [TestInitialize]
        public void init()
        {
            serviceLayer = I_ServiceLayer.Create_ServiceLayer(deliverySystem, paymentSystem, "admin", "1234");
            userServiceLayer1 = serviceLayer.connect();
            if (userServiceLayer1.Register("updateProductInStore_user", "1221"))
                if (userServiceLayer1.Login("updateProductInStore_user", "1221"))
                {
                    if (userServiceLayer1.Open_Store("my store"))
                    {
                        bool product = userServiceLayer1.Add_Product_Store("my store", "bread", "food", 9.9, 42, null, null);
                         product = userServiceLayer1.Add_Product_Store("my store", "pie", "food", 19.9, 22, null, null);
                         product = userServiceLayer1.Add_Product_Store("my store", "brown suger", "food", 11.9, 12, null, null);
                    }
                }
            userServiceLayer2 = serviceLayer.connect();
            if (userServiceLayer2.Register("updateProductInStore_user2", "87654"))
                bool login1 = userServiceLayer2.Login("updateProductInStore_user2", "87654");

        }

        [TestMethod()]
        public void update_valid_product_in_store_tests()
        {
            List<ProductInStore> search1 = userServiceLayer2.GlobalSearch("bread", "food", null, 0, 100, 0, 0);
            int pre_amount = search1.Count;
            Assert.IsTrue(userServiceLayer1.Update_Product_Store("my store", "bread", "light bread", "health food", 8.9, 90, null, null));   //happy 1
            search1 = userServiceLayer2.GlobalSearch("bread", "food", null, 0, 100, 0, 0);
            List<ProductInStore> search2 = userServiceLayer2.GlobalSearch("light bread", "health food", null, 0, 100, 0, 0);
            Assert.AreEqual(1, search2.Count);
            Assert.AreEqual(pre_amount, search1.Count - 1);
        }

        [TestMethod()]
        public void update_product_that_not_exisit_test()
        {
            List<ProductInStore> search1 = userServiceLayer2.GlobalSearch("pie", "food", null, 0, 100, 0, 0);
            int pre_amount = search1[0].getAmount(); 
            Assert.ThrowsException<Exception>(() => { userServiceLayer1.Update_Product_Store("my store", "tea", "green tea", "drink", 6.8, 900, null, null);}, "product does not exist in store");
            search1 = userServiceLayer2.GlobalSearch("pie", "food", null, 0, 100, 0, 0);
            Assert.IsTrue(pre_amount , search1[0].getAmount());   //happy 2
        }

        [TestMethod()]
        public void update_product_amount_to_zero_test()
        {
            List<ProductInStore> search1 = userServiceLayer2.GlobalSearch("pie", "food", null, 0, 100, 0, 0);
            int pre_amount = search1.Count;
            Assert.IsTrue(userServiceLayer1.Update_Product_Store("my store", "pie", "pie", "food", 8.9, 0, null, null));
            search1 = userServiceLayer2.GlobalSearch("pie", "food", null, 0, 100, 0, 0);
            Assert.IsTrue(pre_amount, search1.Count-1);   //happy 2
            for(int i=0; i<search1.Count; i++)
            {
                if(search1[i].getStoreName().Equals("my store"))
                    Assert.Fail("product is still in the store!");
            }
        }

        [TestMethod()]
        public void not_owner_update_product_test()  //bad
        {
            List<ProductInStore> search1 = userServiceLayer2.GlobalSearch("brown suger", "food", null, 0, 100, 0, 0);
            List<ProductInStore> search2 = userServiceLayer2.GlobalSearch("suger", "food", null, 0, 100, 0, 0);
            int pre_amount_brown_suger = search1.Count;
            int pre_amount_suger = search2.Count;
            Assert.ThrowsException<Exception>(() => { userServiceLayer2.Update_Product_Store("my store", "brown suger", "suger", "food", 9, 8, null, null); }, "only store owner or manager can update product in the store");
            search1 = userServiceLayer2.GlobalSearch("brown suger", "food", null, 0, 100, 0, 0);
            search2 = userServiceLayer2.GlobalSearch("suger", "food", null, 0, 100, 0, 0);
            Assert.AreEqual(pre_amount_brown_suger, search1.Count);
            Assert.AreEqual(pre_amount_suger, search2.Count);

            I_User_ServiceLayer tmp_userServiceLayer = serviceLayer.connect();
            Assert.ThrowsException<Exception>(() => { tmp_userServiceLayer.Update_Product_Store("my store", "brown suger", "suger", "food", 9, 8, null, null); }, "only store owner or manager can update product in the store");
            search1 = userServiceLayer2.GlobalSearch("brown suger", "food", null, 0, 100, 0, 0);
            search2 = userServiceLayer2.GlobalSearch("suger", "food", null, 0, 100, 0, 0);
            Assert.AreEqual(pre_amount_brown_suger, search1.Count);
            Assert.AreEqual(pre_amount_suger, search2.Count);

            for(int i =0; i<search2.Count; i++)
            {
                if (search2[i].getStoreName().Equals("my store"))
                    Assert.Fail("not manager/owner update the product");
            }
        }
    }
}
