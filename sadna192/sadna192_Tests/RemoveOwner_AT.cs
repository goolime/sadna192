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
    public class RemoveOwner_AT
    {
        private static I_ServiceLayer serviceLayer;
        private I_User_ServiceLayer userServiceLayer1;
        private I_User_ServiceLayer userServiceLayer2;

        [TestInitialize]
        public void init()
        {
            serviceLayer = new ServiceLayer();
            serviceLayer.Create_ServiceLayer(new stub_deliverySystem(), new stub_paymentSystem(), "admin", "1234");
            userServiceLayer2 = serviceLayer.Connect();
            if (userServiceLayer2.Register("removeOwner_user2", "87654"))
                userServiceLayer2.Login("removeOwner_user2", "87654");

            userServiceLayer1 = serviceLayer.Connect();
            if (userServiceLayer1.Register("removeOwner_user", "1221"))
                if (userServiceLayer1.Login("removeOwner_user", "1221"))
                {
                    if (userServiceLayer1.Open_Store("shopit"))
                    {
                        userServiceLayer1.Add_Product_Store("shopit", "cheese cake", "food", 28, 6, null, null);
                        userServiceLayer1.Add_Store_Owner("shopit", "removeOwner_user2");
                    }
                }          
        }

        [TestMethod()]
        public void remove_shop_owner_tests()
        {
            Assert.IsTrue(userServiceLayer2.Add_Product_Store("shopit", "cola", "drink", 7, 10, null, null));
            Assert.IsTrue(userServiceLayer2.Update_Product_Store("shopit", "cola", "coca cola", "drink", 6.8, 19, null, null)); 
            Assert.IsTrue(userServiceLayer2.Remove_Product_Store("shopit", "coca cola")); 

            Assert.IsTrue(userServiceLayer1.Remove_Store_Owner("shopit", "removeOwner_user2"));

            Assert.ThrowsException<Exception>(() => { userServiceLayer2.Add_Product_Store("shopit", "cola", "drink", 7, 10, null, null); }, "removeOwner_user2 is no longer an owner so he can't add product");
            Assert.ThrowsException<Exception>(() => { userServiceLayer2.Update_Product_Store("shopit", "cheese cake", "cake", "food", 28, 9, null, null); }, "removeOwner_user2 is no longer an owner so he can't update product");
            Assert.ThrowsException<Exception>(() => { userServiceLayer2.Remove_Product_Store("shopit", "cheese cake"); }, "removeOwner_user2 is no longer an owner so he can't remove product");
        }

        [TestMethod()]
        public void remove_shop_owner_that_not_exist_test()
        {
            Assert.ThrowsException<Exception>(() => { userServiceLayer1.Remove_Store_Owner("shopit", "user_not_exist"); }, "faild to remove owner that not exist");
        }

        [TestMethod()]
        public void remove_shop_owner_that_is_not_owner_test()
        {
            I_User_ServiceLayer userServiceLayer3 = serviceLayer.Connect();
            bool reg = userServiceLayer3.Register("removeOwner_user3", "9999");

            Assert.ThrowsException<Exception>(() => { userServiceLayer1.Remove_Store_Owner("shopit", "removeOwner_user3"); }, "faild to remove owner that was not defined to be an owner");
        }

        [TestMethod()]
        public void remove_owner_that_defined_by_someonelse_test()
        {
            I_User_ServiceLayer userServiceLayer3 = serviceLayer.Connect();
            userServiceLayer3.Register("removeOwner_user3", "9999");
            userServiceLayer3.Login("removeOwner_user3", "9999");

            I_User_ServiceLayer userServiceLayer4 = serviceLayer.Connect();
            userServiceLayer4.Register("removeOwner_user4", "345");
            userServiceLayer4.Login("removeOwner_user4", "345");

            userServiceLayer1.Add_Store_Owner("shopit", "removeOwner_user3");
            userServiceLayer1.Add_Store_Owner("shopit", "removeOwner_user4");

            Assert.ThrowsException<Exception>(() => { userServiceLayer3.Remove_Store_Manager("shopit", "removeOwner_user4"); }, "faild to remove owner that someonelse defined him to be an owner");

            I_User_ServiceLayer userServiceLayer5 = serviceLayer.Connect();
            userServiceLayer5.Register("removeOwner_user5", "8765");
            userServiceLayer5.Login("removeOwner_user5", "8765");
            userServiceLayer3.Add_Store_Owner("shopit", "removeOwner_user5");

            Assert.ThrowsException<Exception>(() => { userServiceLayer4.Remove_Store_Manager("shopit", "removeOwner_user5"); }, "faild to remove owner that someonelse defined him to be an owner");
            Assert.IsTrue(userServiceLayer5.Add_Product_Store("shopit", "chips", "food", 17, 50, null, null)); 
            Assert.IsTrue(userServiceLayer3.Remove_Store_Owner("shopit", "removeOwner_user5"));
            Assert.ThrowsException<Exception>(() => { userServiceLayer5.Remove_Product_Store("shopit", "cheese cake"); }, "removeOwner_user5 is no longer an owner so he can't remove product");
        }

        [TestMethod()]
        public void remove_owner_remove_all_the_owners_that_defined_by_him_test()
        {
            I_User_ServiceLayer userServiceLayer3 = serviceLayer.Connect();
            userServiceLayer3.Register("removeOwner_user3", "9999");
            userServiceLayer3.Login("removeOwner_user3", "9999");

            I_User_ServiceLayer userServiceLayer4 = serviceLayer.Connect();
            userServiceLayer4.Register("removeOwner_user4", "345");
            userServiceLayer4.Login("removeOwner_user4", "345");

            userServiceLayer1.Add_Store_Owner("shopit", "removeOwner_user3");
            userServiceLayer3.Add_Store_Owner("shopit", "removeOwner_user4");

            Assert.IsTrue(userServiceLayer4.Update_Product_Store("shopit", "cheese cake", "cheese cake", "desserts", 22, 26, null, null));

            Assert.IsTrue(userServiceLayer1.Remove_Store_Owner("shopit", "removeOwner_user3"));
            Assert.ThrowsException<Exception>(() => { userServiceLayer4.Update_Product_Store("shopit", "cheese cake", "cheese cake", "cakes", 22, 26, null, null); }, "no longer owner because his definder has been removed and is no longer owner ");
            Assert.ThrowsException<Exception>(() => { userServiceLayer3.Update_Product_Store("shopit", "cheese cake", "cheese cake", "cakes", 22, 26, null, null); }, "no longer owner");
        }


        [TestMethod()]
        public void not_owner_try_to_remove_an_owner_test()  //bad
        {
            I_User_ServiceLayer tmp_userServiceLayer = serviceLayer.Connect();
            tmp_userServiceLayer.Register("removeOwner_tmp_user", "11111");

            I_User_ServiceLayer userServiceLayer3 = serviceLayer.Connect();
            userServiceLayer3.Register("removeOwner_user3", "12345");
            userServiceLayer3.Login("removeOwner_user3", "12345");
            userServiceLayer1.Add_Store_Owner("shopit", "removeOwner_user3");

            Assert.ThrowsException<Exception>(() => { tmp_userServiceLayer.Remove_Store_Manager("shopit", "removeOwner_user3"); }, "only store owner can remove an owner from the store");
            Assert.IsTrue(userServiceLayer3.Update_Product_Store("shopit", "cheese cake", "cheese cake", "cakes", 22, 26, null, null));
        }
    }
}
