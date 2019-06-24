using Microsoft.VisualStudio.TestTools.UnitTesting;
using sadna192;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static sadna192_Tests.Stubs;

namespace sadna192.Tests.AcceptanceTests
{
    [TestClass()]
    public class RemoveUser_AT
    {
        private static I_ServiceLayer serviceLayer;
        private I_User_ServiceLayer userServiceLayer_admin;

        [TestInitialize]
        public void Init()
        {
            serviceLayer = new ServiceLayer();
            try
            {
                serviceLayer.Create_ServiceLayer(new Stub_deliverySystem(), new Stub_paymentSystem(), "admin", "123456Ui");
            }
            catch (Exception) { }
            userServiceLayer_admin = serviceLayer.Connect(new Stub_Alerter());
            
            try
            {
                userServiceLayer_admin.Login("admin", "123456Ui");
               
            }
            catch (Exception) { }
           
        }

        [TestMethod()]
        public void Remove_user_from_the_system_happyTest()
        {
            I_User_ServiceLayer userServiceLayer1 = serviceLayer.Connect(new Stub_Alerter());
            userServiceLayer1.Register("removeUser", "1221Asdf");
            Assert.IsTrue(userServiceLayer1.Login("removeUser", "1221Asdf"));
            userServiceLayer1.Logout();
            Assert.IsTrue(userServiceLayer_admin.Remove_User("removeUser"));
            Assert.ThrowsException<Sadna192Exception>(() => { userServiceLayer1.Login("removeUser", "1221Asdf"); }, "this user has been removed from the system");
        }

        [TestMethod()]
        public void Remove_user_that_not_exist_happyTest()
        {
            Assert.ThrowsException<Sadna192Exception>(() => { userServiceLayer_admin.Remove_User("stamUser"); }, "can't remove user that dont exist from the system");
        }

        [TestMethod()]
        public void Admin_try_remove_himself_happyTest()
        {
            Assert.ThrowsException<Sadna192Exception>(() => { userServiceLayer_admin.Remove_User("admin"); }, "admin cannot remove himself from the system");
            Assert.IsTrue(userServiceLayer_admin.Open_Store("AdminStore")); 
        }

        [TestMethod()]
        public void Not_admin_try_remove_user_SadTest()
        {
            I_User_ServiceLayer userServiceLayer1 = serviceLayer.Connect(new Stub_Alerter());
            userServiceLayer1.Register("removeUser2", "1221Asdf");
            userServiceLayer1.Login("removeUser2", "1221Asdf");

            I_User_ServiceLayer userServiceLayer2 = serviceLayer.Connect(new Stub_Alerter());
            userServiceLayer2.Register("removeUserTmp", "1221Poiu");
            Assert.ThrowsException<Sadna192Exception>(() => { userServiceLayer1.Remove_User("removeUserTmp"); }, "only admin can remove users from the system");
            Assert.IsTrue(userServiceLayer2.Login("removeUserTmp", "1221Poiu"));
        }

        [TestCleanup]
        public void TestClean()
        {
            userServiceLayer_admin.Logout();
            serviceLayer.CleanUpSystem();
            serviceLayer = null;
        }
    }
}

