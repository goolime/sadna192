using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using WebApplication1.Models;
using sadna192;


namespace WebApplication1.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            this.validateConnection();
            return View();
        }

        public IActionResult Login(bool? error)
        {
            this.validateConnection();
            if (!IsLoggedIn())
            {
                ViewResult viewResult = View();
                if (error.HasValue)
                {
                    viewResult.ViewData["error"] = error.Value;
                }
                else
                {
                    viewResult.ViewData["error"] = false;
                }

                return viewResult;
            }
            else
            {
                this.validateConnection().Logout();
                return RedirectToAction("index");
            }
        }

        public IActionResult Basket()
        {
            
            try
            {
                List<KeyValuePair<ProductInStore, int>> tmp = this.validateConnection().Watch_Cart();
                var map = new Dictionary<string, List<KeyValuePair<ProductInStore, int>>>();
                foreach (KeyValuePair<ProductInStore, int> p in tmp)
                {
                    string store = p.Key.getStore().getName();
                    if (!map.Keys.Contains(store))
                    {
                        map[store] = new List<KeyValuePair<ProductInStore, int>>();
                    }
                    map[store].Add(p);
                }

                ViewData["cart"] = map;
            }
            catch(Exception e)
            {
                ViewData["Error"] = e.Message;
            }
            //ViewData["Message"] = "Your application Visitor Basket";
            
            return View();
        }


        public IActionResult MyStores(string storename,bool? ownererr,bool? managererr)
        {
            ViewData["ownererr"] = ownererr.HasValue ? ownererr.Value : false;
            ViewData["managererr"] = managererr.HasValue ? managererr.Value : false;
            this.validateConnection();
            ViewResult viewResult = View(new StoreViewModel() { StoreName = storename});
            if (storename != null)
            {
                ViewData["storename"] = storename;
                
            }
            return viewResult;
        }

        public IActionResult Register()
        {
            if (!IsLoggedIn())
            {
                this.validateConnection();
                ViewData["Message"] = "Your application Register";

                return View();
            }
            else
            {
                return RedirectToAction("LoggedIn");
            }
        }

        public IActionResult SearchResults(string key)
        {
            ViewResult viewResult = View();

            if (key != null)
            {
                I_User_ServiceLayer SL = validateConnection();
                List<ProductInStore> list = SL.GlobalSearch(key, null, null, -1, -1, -1, -1);
                viewResult.ViewData["products"] = list;
            }
            else
            {
                viewResult.ViewData["products"] = new List<ProductInStore>();
            }
            return viewResult;
        }

        public IActionResult LoggedIn(bool? storeerr)
        {
            if (storeerr.HasValue)
            {
                ViewData["storeerr"] = storeerr.Value;
            }
            else
            {
                ViewData["storeerr"] = false;
            }
            ViewData["stores"] = this.validateConnection().usersStores();
            I_User_ServiceLayer SL = this.validateConnection();
            ViewData["Owner"] = SL.usersStores();
            return View();
        }

        public IActionResult RegisterSuccess()
        {
            this.validateConnection();
            return View();
        }

        public IActionResult Privacy()
        {
            this.validateConnection();
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            this.validateConnection();
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        private I_User_ServiceLayer validateConnection()
        {
            string currentUserId = HttpContext.Connection.RemoteIpAddress.ToString();
            I_User_ServiceLayer SL = SessionControler.GetSession(currentUserId);
            ViewData["state"] = SL.ToString();
            ViewData["Error"] = "";
            return SL;
        }

        [HttpPost]
        public ActionResult LoginForm(string name, string password)
        {
            I_User_ServiceLayer SL = validateConnection();
            try
            {
                if (SL.Login(name, password))
                {
                    return RedirectToAction("LoggedIn");
                }
                return RedirectToAction("Login", new { error = true });
            }
            catch
            {
                return RedirectToAction("Login", new { error = true });


            }
        }


        [HttpPost]
        public ActionResult OpenStoreForm(string storeName)
        {
            I_User_ServiceLayer SL = validateConnection();
            try
            {
                if (SL.Open_Store(storeName))
                {
                    return RedirectToAction("LoggedIn");
                }
                return RedirectToAction("LoggedIn", new { storeerr = true });
            }
            catch
            {
                return RedirectToAction("LoggedIn", new { storeerr = true });


            }
        }

        [HttpPost]
        public ActionResult RemoveUserForm(string name)
        {
            I_User_ServiceLayer SL = validateConnection();
            try
            {

                if (SL.Remove_User(name))
                {
                    return RedirectToAction("LoggedIn");
                }
                return RedirectToAction("LoggedIn", new { error = true });
            }
            catch
            {
                return RedirectToAction("LoggedIn", new { error = true });


            }
        }

        [HttpPost]
        public ActionResult AddOwnerForm(StoreViewModel model, string ownername)
        {
            string store = model.StoreName;
            I_User_ServiceLayer SL = validateConnection();
            try
            {

                if (SL.Add_Store_Owner(store, ownername))
                {
                    return RedirectToAction("MyStores", new { storename = store });
                }
            }
            catch
            {
            }
            return RedirectToAction("MyStores", new { storename = store, ownererr = true });
        }
        /*
        [HttpPost]
        public ActionResult AddManagerForm(string managername)
        {
            I_User_ServiceLayer SL = validateConnection();
            try
            {

                if (SL.Add_Store_Manager((string)ViewData["storename"], managername))
                {
                    return RedirectToAction("MyStores", new { storename = (string)ViewData["storename"] });
                }
            }
            catch
            {
            }
            return RedirectToAction("MyStores", new { storename = (string)ViewData["storename"], ownererr = true });
        }*/

        [HttpPost]
        public ActionResult RegisterForm(string name, string password)
        {
            I_User_ServiceLayer SL = validateConnection();
            try
            {
                if (SL.Register(name, password))
                    return RedirectToAction("RegisterSuccess");
                return RedirectToAction("Error");
            }
            catch
            {
                return RedirectToAction("Error");
            }
        }

        public ActionResult SearchForm(string keyword)
        {
           return RedirectToAction("SearchResults",new { key = keyword});
            
        }

        public bool IsLoggedIn()
        {
            string currentUserId = HttpContext.Connection.RemoteIpAddress.ToString();
            I_User_ServiceLayer ius = SessionControler.GetSession(currentUserId);
            return !"Visitor".Equals(ius.ToString());
        }
    }
}