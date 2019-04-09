using Microsoft.VisualStudio.TestTools.UnitTesting;
using sadna192;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static sadna192_Tests.Stubs;

namespace sadna192_Tests
{
    [TestClass()]
    class RemoveUser_AT
    {
        private static I_ServiceLayer serviceLayer;
        private I_User_ServiceLayer userServiceLayer_admin;
        private I_User_ServiceLayer userServiceLayer1;

        [TestInitialize]
        public void init()
        {
            serviceLayer = new ServiceLayer();
            serviceLayer.Create_ServiceLayer(new stub_deliverySystem(), new stub_paymentSystem(), "admin", "123456Ui");
            userServiceLayer_admin = serviceLayer.Connect();
            userServiceLayer_admin.Login("admin", "123456Ui"); 

            userServiceLayer1 = serviceLayer.Connect();
            userServiceLayer1.Register("removeUser", "1221Asdf");
        }

        [TestMethod()]
        public void remove_user_from_the_system_test()
        {
            Assert.IsTrue(userServiceLayer1.Login("removeUser", "1221Asdf"));
            Assert.IsTrue(userServiceLayer_admin.Remove_User("removeUser"));
            Assert.ThrowsException<Exception>(() => { userServiceLayer1.Login("removeUser", "1221Asdf"); }, "this user has been removed from the system");
        }

        [TestMethod()]
        public void remove_user_that_not_exist_test()
        {
            Assert.ThrowsException<Exception>(() => { userServiceLayer_admin.Remove_User("stamUser"); }, "can't remove user that dont exist from the system");
        }

        [TestMethod()]
        public void admin_try_remove_himself_test()
        {
            Assert.ThrowsException<Exception>(() => { userServiceLayer_admin.Remove_User("admin"); }, "admin cannot remove himself from the system");
            Assert.IsTrue(userServiceLayer_admin.Open_Store("AdminStore")); 
        }

        [TestMethod()]
        public void not_admin_try_remove_user_test()
        {
            userServiceLayer1.Register("removeUser1", "1221Asdf");
            userServiceLayer1.Login("removeUser", "1221Asdf");

            I_User_ServiceLayer userServiceLayer2 = serviceLayer.Connect();
            userServiceLayer2.Register("removeUserTmp", "1221Poiu");

            Assert.ThrowsException<Exception>(() => { userServiceLayer1.Remove_User("removeUserTmp"); }, "only admin can remove users from the system");
            Assert.IsTrue(userServiceLayer2.Login("removeUserTmp", "1221Poiu"));
        }
    }
}

