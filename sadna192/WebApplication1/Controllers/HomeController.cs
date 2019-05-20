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
                I_User_ServiceLayer SL = this.validateConnection();
                List<KeyValuePair<ProductInStore, int>> tmp = SL.Watch_Cart();
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
            catch (Exception e)
            {
                ViewData["Error"] = e.Message;
            }
            //ViewData["Message"] = "Your application Visitor Basket";

            return View();
        }

        public IActionResult Recipt(string store)
        {
            I_User_ServiceLayer SL = this.validateConnection();
            try
            {
                ViewData["recipt"] = SL.Purchase_Store_cart(store);
                return View();
            }
            catch (Exception e)
            {
                ViewData["Error"] = e.Message;
                return RedirectToAction("Error");
            }
            
        }

        public IActionResult MyStores(string storename, bool? ownererr, bool? managererr,bool? producterr)
        {
            I_User_ServiceLayer sl = this.validateConnection();
            Dictionary<string, dynamic> storeDictionary = sl.usersStores().Find(d => ((Store)d["store"]).getName() == storename);
            ViewData["currStore"] = storeDictionary["store"];
            ViewData["ownererr"] = ownererr.HasValue ? ownererr.Value : false;
            ViewData["managererr"] = managererr.HasValue ? managererr.Value : false;
            ViewData["producterr"] = producterr.HasValue ? producterr.Value : false;
            StoreViewModel Store = new StoreViewModel { StoreName = storename };
            Store_AddManagerViewModel model = new Store_AddManagerViewModel() { S = Store, AM = null };
            if (storename != null)
            {
                ViewData["storename"] = storename;

            }
            return View(model);
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

            if (key != null)
            {
                I_User_ServiceLayer SL = validateConnection();
                List<ProductInStore> list = SL.GlobalSearch(key, null, null, -1, -1, -1, -1);
                ViewData["products"] = list;
                ViewData["keyword"] = key;
            }
            else
            {
                ViewData["products"] = new List<ProductInStore>();
            }
            return View();
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

        public IActionResult Allgood()
        {
            this.validateConnection();
            return View();
        }
        public IActionResult Privacy()
        {
            this.validateConnection();
            return View();
        }

        public IActionResult Product(string storename, string productname)
        {
            I_User_ServiceLayer SL = this.validateConnection();
            ViewData["SL"] = SL;
            ProductInStore product = SL.GetProductFromStore(productname, storename);
            return base.View(new ProductInStoreViewModel()
            {
                Name = productname,
                StoreName = storename,
                EditProduct = new EditProductViewModel()
                {
                    
                    ProductAmount = product.getAmount(),
                    ProductCategory = product.getCategory(),
                    ProductPrice = product.getPrice(),
                    NewName = productname
               }
            });
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
            ViewData["SL"] = SL;
            ViewData["state"] = SL.ToString();
            ViewData["Error"] = "";
            return SL;
        }

        [HttpPost]
        public ActionResult Buy_from_store_form(string Store)
        {
            return RedirectToAction("Recipt", new { store=Store });
            
        }
       
        [HttpPost]
        public ActionResult update_product_inCart(string newAmount, string store, string product)
        {
            I_User_ServiceLayer SL = this.validateConnection();
            List<KeyValuePair<ProductInStore, int>> tmp = SL.Watch_Cart();
            //var map = new Dictionary<string, List<KeyValuePair<ProductInStore, int>>>();
            foreach (KeyValuePair<ProductInStore, int> p in tmp)
            {
                if (p.Key.getName()==product && p.Key.getStore().getName() == store)
                {
                    SL.Edit_Product_In_ShopingBasket(p.Key, int.Parse(newAmount));
                    return RedirectToAction("Basket");
                }
            }
            ViewData["Error"] = "product awsnot found";
            return RedirectToAction("Error");
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
        public ActionResult AddOwnerForm(Store_AddManagerViewModel model, string ownername)
        {

            string store = model.S.StoreName;
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

        [HttpPost]
        public ActionResult AddManagerForm(Store_AddManagerViewModel model)
        {
            I_User_ServiceLayer SL = validateConnection();
            try
            {
                if (ModelState.IsValid &&
                    SL.Add_Store_Manager(model.S.StoreName,
                    model.AM.Name,
                    model.AM.AddPermission,
                    model.AM.RemovePermission,
                    model.AM.UpdatePermission))
                {
                    return RedirectToAction("MyStores", new { storename = model.S.StoreName });
                }
            }
            catch
            {
            }
            return RedirectToAction("MyStores", new { storename = model.S.StoreName, managererr = true });
        }

        [HttpPost]
        public ActionResult AddProductForm(Store_AddManagerViewModel model)
        {
            I_User_ServiceLayer SL = validateConnection();
            try
            {
                if (ModelState.IsValid &&
                    SL.Add_Product_Store(model.S.StoreName,model.AP.ProductName, model.AP.ProductCategory, model.AP.ProductPrice, model.AP.ProductAmount, model.AP.Discount, model.AP.ProductPolicy))
                {
                    return RedirectToAction("MyStores", new { storename = model.S.StoreName });
                }
            }
            catch
            {
            }
            return RedirectToAction("MyStores", new { storename = model.S.StoreName, producterr = true });
        }

        [HttpPost]
        public ActionResult EditManagerForm(Store_AddManagerViewModel model)
        {
            I_User_ServiceLayer SL = validateConnection();
            try
            {
                if (SL.Remove_Store_Manager(model.S.StoreName, model.O.Name)
                    && SL.Add_Store_Manager(model.S.StoreName,
                    model.O.Name,
                    model.AM.AddPermission,
                    model.AM.RemovePermission,
                    model.AM.UpdatePermission))
                {
                    return RedirectToAction("MyStores", new { storename = model.S.StoreName });
                }
            }
            catch
            {
            }
            return RedirectToAction("MyStores", new { storename = model.S.StoreName, managererr = true });
        }

        [HttpPost]
        public ActionResult RemoveManagerForm(Store_AddManagerViewModel model)
        {
            I_User_ServiceLayer SL = validateConnection();
            try
            {
                if (SL.Remove_Store_Manager(model.S.StoreName, model.O.Name))
                {
                    return RedirectToAction("MyStores", new { storename = model.S.StoreName });
                }
            }
            catch
            {
            }
            return RedirectToAction("MyStores", new { storename = model.S.StoreName, managerremove = true });
        }

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
            validateConnection();
            return RedirectToAction("SearchResults", new { key = keyword });

        }

        public bool IsLoggedIn()
        {
            string currentUserId = HttpContext.Connection.RemoteIpAddress.ToString();
            I_User_ServiceLayer ius = SessionControler.GetSession(currentUserId);
            return !"Visitor".Equals(ius.ToString());
        }
        [HttpPost]
        public ActionResult return_to_cart() {
            I_User_ServiceLayer SL = this.validateConnection();
            SL.canclePurch();
            return RedirectToAction("Baskt");
        }

        [HttpPost]
        public ActionResult finalize(string payment, string shipping)
        {
            I_User_ServiceLayer SL = this.validateConnection();
            try
            {
                SL.Finalize_Purchase(shipping, payment);
                return RedirectToAction("Allgood");
            }
            catch (Exception e)
            {
                ViewData["Error"] = e.Message;
                return RedirectToAction("Error");
            }
        }

        [HttpPost]
        public ActionResult RemoveOwnerForm(Store_AddManagerViewModel vm)
        {
            I_User_ServiceLayer SL = validateConnection();
            try
            {

                if (SL.Remove_Store_Owner(vm.S.StoreName, vm.O.Name))
                {
                    return RedirectToAction("LoggedIn");
                }
            }
            catch
            {

            }
            ViewData["alertRemoveOwner"] = true;
            return RedirectToAction("MyStores", new { storename = vm.S.StoreName });
        }

        [HttpPost]
        public ActionResult RemoveProductForm(ProductInStoreViewModel vm)
        {
            I_User_ServiceLayer SL = validateConnection();
            try
            {

                if (SL.Remove_Product_Store(vm.StoreName, vm.Name))
                {
                    return RedirectToAction("MyStores",new { storename = vm.StoreName});
                }
            }
            catch
            {

            }
            ViewData["alertRemoveProduct"] = true;
            return RedirectToAction("Product", new { storename = vm.StoreName, productname = vm.Name });
        }

        
        [HttpPost]
        public ActionResult EditProductForm(ProductInStoreViewModel vm)
        {
            I_User_ServiceLayer SL = validateConnection();
            try
            {

                if (SL.Update_Product_Store(vm.StoreName,vm.Name,vm.EditProduct.NewName,vm.EditProduct.ProductCategory, vm.EditProduct.ProductPrice
                    , vm.EditProduct.ProductAmount,null,null))
                {
                    return RedirectToAction("MyStores", new { storename = vm.StoreName });
                }
            }
            catch
            {

            }
            ViewData["alertEditProduct"] = true;
            return RedirectToAction("Product", new { storename = vm.StoreName, productname = vm.Name });
        }
    }
}