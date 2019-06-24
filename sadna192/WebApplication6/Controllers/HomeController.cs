using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using sadna192; 
using System.Web.Mvc;

namespace WebApplication6.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            this.validateConnection();
            return View();
        }

        public ActionResult Login(bool? error)
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

        public ActionResult Basket()
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

        public ActionResult Recipt(string store)
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

        public ActionResult MyStores(string storename, bool? ownererr, bool? managererr, bool? producterr, int? disnum, bool? resetdis, bool? resetpol, bool? alertdis, int? polnum)
        {
            I_User_ServiceLayer sl = this.validateConnection();
            Dictionary<string, dynamic> storeDictionary = sl.usersStores().Find(d => ((Store)d["store"]).getName() == storename);
            ViewData["currStore"] = storeDictionary["store"];
            ViewData["ownererr"] = ownererr.HasValue ? ownererr.Value : false;
            ViewData["managererr"] = managererr.HasValue ? managererr.Value : false;
            ViewData["producterr"] = producterr.HasValue ? producterr.Value : false;
            ViewData["alertAddDiscount"] = alertdis.HasValue ? alertdis.Value : false;

            ViewData["SL"] = sl;
            StoreViewModel Store = new StoreViewModel { StoreName = storename };
            Store_AddManagerViewModel model =
                new Store_AddManagerViewModel()
                {
                    S = Store,
                    AM = null,
                    AD = new AddDiscountViewModel()
                    {
                        NumberOfDiscounts = disnum ?? 1,
                        DiscountVisible = ((disnum ?? 1) > 1) || (resetdis ?? false)
                    },
                    APolicy = new AddPolicyViewModel()
                    {
                        NumberOfPolicies = polnum ?? 1,
                        IsPolicyVisible = ((polnum ?? 1) > 1) || (resetpol ?? false),
                    }

                };
            if (storename != null)
            {
                ViewData["storename"] = storename;

            }
            return View(model);
        }

        public ActionResult Register()
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

        public ActionResult Notifications()
        {
            if (!IsLoggedIn())
            {
                this.validateConnection();
                ViewData["Message"] = "Your application Register";

                return RedirectToAction("index");
            }
            else
            {
                return View();
            }
        }

        public ActionResult SearchResults(string key, string keys, string cat, double max, double min)
        {
            I_User_ServiceLayer SL = validateConnection();
            List<ProductInStore> list =
                SL.GlobalSearch(key,
                cat,
                keys?.Split('-').ToList(),
                min == 0 ? -1 : min,
                max == 0 ? -1 : max,
                -1,
                -1);
            ViewData["products"] = list;
            ViewData["keyword"] = key;
            return View();
        }

        public ActionResult LoggedIn(bool? storeerr)
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

        public ActionResult RegisterSuccess()
        {
            this.validateConnection();
            return View();
        }

        public ActionResult Allgood()
        {
            this.validateConnection();
            return View();
        }
        public ActionResult Privacy()
        {
            this.validateConnection();
            return View();
        }

        public ActionResult Product(string storename, string productname)
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

        //[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public ActionResult Error()
        {
            this.validateConnection();
            return View(new ErrorViewModel() /*{ RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier }*/);
        }

        public class ErrorViewModel
        {
            public string RequestId { get; set; }

            public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
        }

        private I_User_ServiceLayer validateConnection()
        {
            string currentUserId = HttpContext.User.ToString();
            //string currentUserId = HttpContext.Connection.RemoteIpAddress.ToString();
            I_User_ServiceLayer SL = SessionControler.GetSession(currentUserId);
            ViewData["SL"] = SL;
            ViewData["state"] = SL.ToString();
            ViewData["Error"] = "";
            return SL;
        }
        public class SessionControler
        {
            ///Todo Remove old sessions
            private static Dictionary<string, I_User_ServiceLayer> Sessions = new Dictionary<string, I_User_ServiceLayer>();

            public static I_User_ServiceLayer GetSession(string ip)
            {
                if (!Sessions.ContainsKey(ip))
                {
                    try
                    {
                        I_ServiceLayer tmp_sl = new sadna192.ServiceLayer();
                        Sessions[ip] = tmp_sl.Connect(null);
                    }
                    catch (Sadna192Exception e)
                    {
                        I_ServiceLayer tmp_sl = new sadna192.ServiceLayer();
                        tmp_sl.Create_ServiceLayer(new Stubs.Stub_deliverySystem(), new Stubs.Stub_paymentSystem(), "admin", "1234Abcd");
                        init(tmp_sl);
                        //I_ServiceLayer tmp_sl = new sadna192.ServiceLayer();
                        Sessions[ip] = tmp_sl.Connect(null);
                    }
                }
                return Sessions[ip];
            }

        }

        public static void init(I_ServiceLayer SL)
        {
            I_User_ServiceLayer UL = SL.Connect(null);
            UL.Register("initUser", "1234Abcd");
            UL.Login("initUser", "1234Abcd");
            UL.Open_Store("testStore");
            UL.Add_Product_Store("testStore", "Banna", "food", 5.0, 50, new noDiscount(), new regularPolicy());
            UL.Add_Product_Store("testStore", "Apple", "food", 5.0, 70, new noDiscount(), new regularPolicy());
            UL.Add_To_ShopingBasket(UL.GlobalSearch("Banna", null, null, -1, -1, -1, -1)[0], 5);
            UL.Add_To_ShopingBasket(UL.GlobalSearch("Apple", null, null, -1, -1, -1, -1)[0], 5);
            UL.Logout();
        }

        [HttpPost]
        public ActionResult Buy_from_store_form(string Store)
        {
            return RedirectToAction("Recipt", new { store = Store });

        }

        [HttpPost]
        public ActionResult update_product_inCart(string newAmount, string store, string product)
        {
            I_User_ServiceLayer SL = this.validateConnection();
            List<KeyValuePair<ProductInStore, int>> tmp = SL.Watch_Cart();
            //var map = new Dictionary<string, List<KeyValuePair<ProductInStore, int>>>();
            foreach (KeyValuePair<ProductInStore, int> p in tmp)
            {
                if (p.Key.getName() == product && p.Key.getStore().getName() == store)
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
                    SL.Add_Product_Store(model.S.StoreName, model.AP.ProductName, model.AP.ProductCategory, model.AP.ProductPrice, model.AP.ProductAmount, model.AP.Discount, model.AP.ProductPolicy))
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

        public ActionResult SearchForm(string keyword, string keywords, string category, double pricemax, double pricemin)
        {
            validateConnection();
            return RedirectToAction("SearchResults", new { key = keyword, keys = keywords, cat = category, max = pricemax, min = pricemin });

        }

        public bool IsLoggedIn()
        {
            //string currentUserId = HttpContext.Connection.RemoteIpAddress.ToString();
            //I_User_ServiceLayer ius = SessionControler.GetSession(currentUserId);
            validateConnection();
            return !"Visitor".Equals(ViewData["state"].ToString());
        }
        [HttpPost]
        public ActionResult return_to_cart()
        {
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
                    return RedirectToAction("MyStores", new { storename = vm.StoreName });
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

                if (SL.Update_Product_Store(vm.StoreName, vm.Name, vm.EditProduct.NewName, vm.EditProduct.ProductCategory, vm.EditProduct.ProductPrice
                    , vm.EditProduct.ProductAmount, null, null))
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


        [HttpPost]
        public ActionResult AddToCart(ProductInStoreViewModel vm)
        {
            I_User_ServiceLayer SL = validateConnection();
            try
            {
                if (SL.Add_To_ShopingBasket(SL.GetProductFromStore(vm.Name, vm.StoreName), vm.AddToCart.Amount))
                {
                    return RedirectToAction("Basket");
                }
            }
            catch
            {

            }
            ViewData["alertAddToCart"] = true;
            return RedirectToAction("Product", new { storename = vm.StoreName, productname = vm.Name });
        }


        [HttpPost]
        public ActionResult AddDiscountForm(Store_AddManagerViewModel vm, string command)
        {
            if (command.Equals("finalize"))
            {
                return FinalizeDiscounts(vm);
            }
            else if (command.Equals("add"))
            {
                return RedirectToAction("MyStores", new { storename = vm.S.StoreName, disnum = vm.AD.NumberOfDiscounts + 1 });
            }
            else if (command.Equals("reset"))
            {
                return RedirectToAction("MyStores", new { storename = vm.S.StoreName, resetdis = true });
            }
            return new EmptyResult();
        }

        private ActionResult FinalizeDiscounts(Store_AddManagerViewModel vm)
        {
            Discount d = GenerateDiscount(vm.AD);
            I_User_ServiceLayer SL = validateConnection();
            try
            {
                if (SL.Update_Product_Store(vm.S.StoreName, vm.O.Name, null, null, -1, -1, d, null))
                {
                    return RedirectToAction("MyStores", new { storename = vm.S.StoreName });
                }
            }
            catch
            {

            }
            return RedirectToAction("MyStores", new { storename = vm.S.StoreName, alertdis = true });
        }

        private Discount GenerateDiscount(AddDiscountViewModel ad)
        {
            Discount dis;
            if (ad.IsProductsDiscount)
            {
                dis = oneDiscountGen(ad);
            }
            else
            {
                dis = new ProductAmountInBasketDiscount(ad.Discounts[0].Amount, ((double)(ad.Discounts[0].DiscountPercent)) / 100);

            }
            if (ad.TimeSpan.IsTimeLimited)
            {
                dis = TimeDiscount(ad.TimeSpan.Start, ad.TimeSpan.Finish, dis);
            }
            dis = StoreDiscount(dis);
            return dis;
        }

        private static Discount oneDiscountGen(AddDiscountViewModel ad)
        {
            Discount dis;
            List<Discount> list = new List<Discount>();
            foreach (DiscountViewModel dvm in ad.Discounts)
            {

                ProductAmountDiscount productDis = new ProductAmountDiscount(dvm.ProductName, dvm.Amount, ((double)dvm.DiscountPercent) / 100);
                list.Add(productDis);
            }
            dis = LogicConnection(ad.LogicConnection, list);
            return dis;
        }

        private static Discount TimeDiscount(DateTime start, DateTime finish, Discount dis)
        {
            return new TimeDiscount(start, finish, dis);
        }

        private static Discount StoreDiscount(Discount dis)
        {

            dis = new AndDiscount(new List<Discount>()
                {
                    dis,
                    new IncludeStoreDiscount()
                });


            return dis;
        }

        private static Discount LogicConnection(string logicConn, List<Discount> list)
        {
            if (logicConn == "OR")
            {
                return new XOrDiscount(list);
            }
            else if (logicConn == "AND")
            {
                return new AndDiscount(list);
            }
            throw new Exception("no logic connection was chosen");

        }

        [HttpPost]
        public ActionResult AddPolicyForm(Store_AddManagerViewModel vm, string command)
        {
            if (command.Equals("finalize"))
            {
                return FinalizePolicies(vm);
            }
            else if (command.Equals("add"))
            {
                return RedirectToAction("MyStores", new { storename = vm.S.StoreName, polnum = vm.APolicy.NumberOfPolicies + 1 });
            }
            else if (command.Equals("reset"))
            {
                return RedirectToAction("MyStores", new { storename = vm.S.StoreName, resetpol = true });
            }
            return new EmptyResult();
        }

        private ActionResult FinalizePolicies(Store_AddManagerViewModel vm)
        {
            Policy policy = ConstructPolicy(vm.APolicy);
            I_User_ServiceLayer SL = validateConnection();
            if (SL.Update_Product_Store(vm.S.StoreName, vm.APolicy.Name, null, null, -1, -1, null, policy)) ;
            {
                return RedirectToAction("MyStores", new { storename = vm.S.StoreName });
            }
        }

        private Policy ConstructPolicy(AddPolicyViewModel aPolicy)
        {
            if (aPolicy.PolicyKind != "Product" && aPolicy.PolicyKind != "Store")
            {
                throw new Exception("no option was chosen for product kind");
            }
            List<Policy> PoliciesToOr = new List<Policy>();
            foreach (ProductPolicyViewModel pp in aPolicy.ProductPolicies)
            {
                List<Policy> PoliciesToAnd = GenerateOnePolicy(pp);
                IsIncluded(aPolicy, pp, PoliciesToAnd);
                MaxMinHandle(aPolicy, pp, PoliciesToAnd);
                Policy AndPolicy = new AndPolicy(PoliciesToAnd);
                PoliciesToOr.Add(AndPolicy);
            }
            return new OrPolicy(PoliciesToOr);
        }

        private static void IsIncluded(AddPolicyViewModel aPolicy, ProductPolicyViewModel pp, List<Policy> PoliciesToAnd)
        {
            if (aPolicy.PolicyKind == "Product" && pp.IncludeStorePolicy)
            {
                PoliciesToAnd.Add(new IncludeStorePolicy());
            }
        }

        private static void MaxMinHandle(AddPolicyViewModel aPolicy, ProductPolicyViewModel pp, List<Policy> PoliciesToAnd)
        {
            if (pp.Constraint == "MAX" || pp.Constraint == "MIN")
            {
                if (aPolicy.PolicyKind == "Product")
                {
                    if (pp.Constraint == "MAX")
                    {
                        PoliciesToAnd.Add(new MaximumProductInCart(pp.Amount));
                    }
                    else //(pp.Constraint == "MIN")
                    {
                        PoliciesToAnd.Add(new MinimumProductInCart(pp.Amount));
                    }
                }
                else //(pp.ProductKind == "Store")
                {
                    if (pp.Constraint == "MAX")
                    {
                        PoliciesToAnd.Add(new MaximumInCart(pp.Amount));
                    }
                    else //(pp.Constraint == "MIN")
                    {
                        PoliciesToAnd.Add(new MinimumInCart(pp.Amount));
                    }
                }
            }
        }

        private List<Policy> GenerateOnePolicy(ProductPolicyViewModel pp)
        {
            List<Policy> PoliciesToAnd = new List<Policy>();
            if (pp.Immidiate)
            {
                PoliciesToAnd.Add(new immidiatePolicy());
            }
            if (pp.Member)
            {
                PoliciesToAnd.Add(new MamberPolicy());
            }
            if (pp.Time.IsTimeLimited)
            {
                PoliciesToAnd.Add(new TimePolicy(pp.Time.Start, pp.Time.Finish));
            }
            return PoliciesToAnd;
        }
    }

    public class ProductInStoreViewModel
    {
        public string Name { get; set; }
        public string StoreName { get; set; }
        public EditProductViewModel EditProduct { get; set; }
        public AddToCartViewModel AddToCart { get; set; }
    }

    public class EditProductViewModel
    {
        public string ProductCategory { get; set; }
        public double ProductPrice { get; set; } = -1;
        public int ProductAmount { get; set; } = -1;
        public string NewName { get; set; }
    }

    public class AddToCartViewModel
    {
        public int Amount { get; set; }
    }

    public class StoreViewModel
    {
        public string StoreName { get; set; }

    }

    public class OwnerViewModel
    {
        public string Name { get; set; }

        public void SetName(string name)
        {
            Name = name;
        }

    }

    public class AddManagerViewModel
    {
        public string Name { get; set; }
        public bool AddPermission { get; set; }
        public bool RemovePermission { get; set; }
        public bool UpdatePermission { get; set; }
    }

    public class AddProductViewModel
    {
        public string ProductCategory { get; set; }
        public double ProductPrice { get; set; } = -1;
        public Discount Discount;
        public Policy ProductPolicy;

        public string ProductName { get; set; }
        public int ProductAmount { get; set; } = -1;
    }

    public class Store_AddManagerViewModel
    {
        public OwnerViewModel O { get; set; }
        public StoreViewModel S { get; set; }
        public AddManagerViewModel AM { get; set; }
        public AddProductViewModel AP { get; set; }
        public AddDiscountViewModel AD { get; set; }
        public AddPolicyViewModel APolicy { get; set; }


        public string DeleteOwnerConfig(string owner, string store)
        {
            O = new OwnerViewModel() { Name = owner };
            S = new StoreViewModel() { StoreName = store };
            return "";
        }

        public string DeleteManagerConfig(string owner, string store)
        {
            return DeleteOwnerConfig(owner, store);
        }
    }

    public class AddDiscountViewModel
    {
        public bool IsProductsDiscount { get; set; }
        public string LogicConnection { get; set; }
        public List<DiscountViewModel> Discounts { get; set; }

        public TimeSpanViewModel TimeSpan { get; set; }

        public bool IsStoreDiscount { get; set; }
        public int NumberOfDiscounts { get; set; }
        public bool DiscountVisible { get; set; }
    }

    public class DiscountViewModel
    {
        public string ProductName { get; set; }
        public int Amount { get; set; }
        public int DiscountPercent { get; set; }
    }

    public class AddPolicyViewModel
    {
        public bool IsPolicyVisible { get; set; }
        public string PolicyKind { get; set; }//Store Product Policy
        public List<ProductPolicyViewModel> ProductPolicies { get; set; }
        public int NumberOfPolicies { get; set; }
        public string Name { get; set; }


    }

    public class TimeSpanViewModel
    {
        public bool IsTimeLimited { get; set; }
        public DateTime Start { get; set; }
        public DateTime Finish { get; set; }
    }

    public class ProductPolicyViewModel
    {
        public string Constraint { get; set; }
        public int Amount { get; set; }
        public TimeSpanViewModel Time { get; set; }
        public bool Member { get; set; }
        public bool IncludeStorePolicy { get; set; }
        public bool Immidiate { get; set; }

    }
}