using Microsoft.VisualStudio.TestTools.UnitTesting;
using sadna192;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static sadna192_Tests.Stubs;

namespace sadna192.Tests
{
    [TestClass()]
    public class UpdateProductInStore_AT
    {
        private static I_ServiceLayer serviceLayer;
        private I_User_ServiceLayer userServiceLayer1;
        private I_User_ServiceLayer userServiceLayer2;

        [TestInitialize]
        public void init()
        {
            serviceLayer = new ServiceLayer();
            serviceLayer.Create_ServiceLayer(new stub_deliverySystem(), new stub_paymentSystem(), "admin", "1234ASdf");
            userServiceLayer1 = serviceLayer.Connect();
            if (userServiceLayer1.Register("updateProductInStoreUser", "1221YhnJ"))
                if (userServiceLayer1.Login("updateProductInStoreUser", "1221YhnJ"))
                {
                    if (userServiceLayer1.Open_Store("my store"))
                    {
                        userServiceLayer1.Add_Product_Store("my store", "bread", "food", 9.9, 42, new noDiscount(), new regularPolicy());
                        userServiceLayer1.Add_Product_Store("my store", "pie", "food", 19.9, 22, new noDiscount(), new regularPolicy());
                        userServiceLayer1.Add_Product_Store("my store", "brown suger", "food", 11.9, 12, new noDiscount(), new regularPolicy());
                    }
                }
            userServiceLayer2 = serviceLayer.Connect();
            if (userServiceLayer2.Register("updateProductInStoreUser2", "87654GfbH"))
                userServiceLayer2.Login("updateProductInStoreUser2", "87654GfbH");

        }

        [TestMethod()]
        public void update_valid_product_in_store_tests()
        {
            List<ProductInStore> search1 = userServiceLayer2.GlobalSearch("bread", "food", null, 0, 100, 0, 0);
            int pre_amount = search1.Count;
            Assert.IsTrue(userServiceLayer1.Update_Product_Store("my store", "bread", "light bread", "health food", 8.9, 90, new noDiscount(), new regularPolicy()));   //happy 1
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
            Assert.ThrowsException<Exception>(() => { userServiceLayer1.Update_Product_Store("my store", "tea", "green tea", "drink", 6.8, 900, new noDiscount(), new regularPolicy());}, "product does not exist in store");
            search1 = userServiceLayer2.GlobalSearch("pie", "food", null, 0, 100, 0, 0);
            Assert.AreEqual(pre_amount , search1[0].getAmount());   //happy 2
        }

        [TestMethod()]
        public void update_product_amount_to_zero_test()
        {
            List<ProductInStore> search1 = userServiceLayer2.GlobalSearch("pie", "food", null, 0, 100, 0, 0);
            int pre_amount = search1.Count;
            Assert.IsTrue(userServiceLayer1.Update_Product_Store("my store", "pie", "pie", "food", 8.9, 0, new noDiscount(), new regularPolicy()));
            search1 = userServiceLayer2.GlobalSearch("pie", "food", null, 0, 100, 0, 0);
            Assert.AreEqual(pre_amount, search1.Count-1);   //happy 2
            for(int i=0; i<search1.Count; i++)
            {
                if(search1[i].getStore().getName().Equals("my store"))
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
            Assert.ThrowsException<Exception>(() => { userServiceLayer2.Update_Product_Store("my store", "brown suger", "suger", "food", 9, 8, new noDiscount(), new regularPolicy()); }, "only store owner or manager can update product in the store");
            search1 = userServiceLayer2.GlobalSearch("brown suger", "food", null, 0, 100, 0, 0);
            search2 = userServiceLayer2.GlobalSearch("suger", "food", null, 0, 100, 0, 0);
            Assert.AreEqual(pre_amount_brown_suger, search1.Count);
            Assert.AreEqual(pre_amount_suger, search2.Count);

            I_User_ServiceLayer tmp_userServiceLayer = serviceLayer.Connect();
            Assert.ThrowsException<Exception>(() => { tmp_userServiceLayer.Update_Product_Store("my store", "brown suger", "suger", "food", 9, 8, new noDiscount(), new regularPolicy()); }, "only store owner or manager can update product in the store");
            search1 = userServiceLayer2.GlobalSearch("brown suger", "food", null, 0, 100, 0, 0);
            search2 = userServiceLayer2.GlobalSearch("suger", "food", null, 0, 100, 0, 0);
            Assert.AreEqual(pre_amount_brown_suger, search1.Count);
            Assert.AreEqual(pre_amount_suger, search2.Count);

            for(int i =0; i<search2.Count; i++)
            {
                if (search2[i].getStore().getName().Equals("my store"))
                    Assert.Fail("not manager/owner update the product");
            }
        }
    }
}
