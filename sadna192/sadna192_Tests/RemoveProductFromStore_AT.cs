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
    public class RemoveProductFromStore_AT
    {
        private static I_ServiceLayer serviceLayer;
        private I_User_ServiceLayer userServiceLayer1;
        private I_User_ServiceLayer userServiceLayer2;

        [TestInitialize]
        public void init()
        {
            serviceLayer = new ServiceLayer();
            serviceLayer.Create_ServiceLayer(new stub_deliverySystem(), new stub_paymentSystem(), "admin", "1234GfrT");
            userServiceLayer1 = serviceLayer.Connect();
            if (userServiceLayer1.Register("removeProductFromStoreUser", "1221Gbbv"))
                if (userServiceLayer1.Login("removeProductFromStoreUser", "1221Gbbv"))
                {
                   if(userServiceLayer1.Open_Store("our store"))
                    {
                        userServiceLayer1.Add_Product_Store("our store", "pizza", "food", 29.9, 34, new noDiscount(), new regularPolicy());
                    }
                }
            userServiceLayer2 = serviceLayer.Connect();
            if (userServiceLayer2.Register("removeProductFromStoreUser2", "87654DFgh"))
                userServiceLayer2.Login("removeProductFromStoreUser2", "87654DFgh");

        }

        [TestMethod()]
        public void remove_valid_product_from_store_tests()
        {
            List<ProductInStore> search1 = userServiceLayer2.GlobalSearch("pizza", "food", null, 0, 100, 0, 0);
            Assert.IsTrue(userServiceLayer1.Remove_Product_Store("our store", "pizza"));   //happy 1
            List<ProductInStore> search2 = userServiceLayer2.GlobalSearch("pizza", "food", null, 0, 100, 0, 0);
            Assert.AreEqual(search1.Count, search2.Count+1);
            for(int i=0; i< search2.Count; i++)
            {
                if (search2[i].getStore().getName().Equals("our store"))
                    Assert.Fail("product is still in the store!");
            }
        }

        [TestMethod()]
        public void remove_product_that_not_exisit_tests()
        {

            Assert.ThrowsException<Exception>(() => { userServiceLayer1.Remove_Product_Store("our store", "fish"); }, "product does not exist in store");
            List<ProductInStore> search1 = userServiceLayer2.GlobalSearch("pizza", "food", null, 0, 100, 0, 0);
            Assert.IsTrue(userServiceLayer1.Remove_Product_Store("our store", "pizza"));   //happy 2
            List<ProductInStore> search2 = userServiceLayer2.GlobalSearch("pizza", "food", null, 0, 100, 0, 0);
            Assert.AreEqual(search1, search2);
        }

        [TestMethod()]
        public void not_owner_remove_product_test()  //bad
        {
            List<ProductInStore> search1 = userServiceLayer2.GlobalSearch("pizza", "food", null, 0, 100, 0, 0);
            int pre_amount = search1.Count;
            Assert.ThrowsException<Exception>(() => { userServiceLayer2.Remove_Product_Store("our store", "pizza"); }, "only store owner or manager can remove product from the store");
            search1 = userServiceLayer2.GlobalSearch("pizza", "food", null, 0, 100, 0, 0);
            Assert.AreEqual(pre_amount, search1.Count);

            I_User_ServiceLayer tmp_userServiceLayer = serviceLayer.Connect();
            Assert.ThrowsException<Exception>(() => { tmp_userServiceLayer.Remove_Product_Store("our store", "pizza"); }, "only store owner or manager can remove product to the store");
            search1 = userServiceLayer2.GlobalSearch("pizza", "food", null, 0, 100, 0, 0);
            Assert.AreEqual(pre_amount, search1.Count);
        }

    }
}