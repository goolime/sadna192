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
            if(!IsLoggedIn())
                return View();
            else
            {
                return RedirectToAction("LoggedIn");
            }
        }

        public IActionResult Login()
        {
            this.validateConnection();
            return View();
        }

        public IActionResult Logout()
        {
            this.validateConnection().Logout();
            return RedirectToAction("index");
        }

        public IActionResult Basket()
        {
            this.validateConnection();
            ViewData["Message"] = "Your application Visitor Basket";

            return View();
        }

        public IActionResult Register()
        {
            this.validateConnection();
            ViewData["Message"] = "Your application Register";

            return View();
        }

        public IActionResult BasketLogged()
        {
            this.validateConnection();
            ViewData["Message"] = "Your application Basket Logged.";

            return View();
        }

        public IActionResult LoggedIn()
        {
            this.validateConnection();
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
            this.ViewData["state"] = SL.ToString();
            return SL;
        }

        [HttpPost]
        public ActionResult LoginForm(string name, string password)
        {
            I_User_ServiceLayer SL = validateConnection();
            try
            {
                if (SL.Login(name, password))
                    return RedirectToAction("LoggedIn");
                return RedirectToAction("Error");
            }
            catch
            {
                return RedirectToAction("Error");
            }
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

        public bool IsLoggedIn()
        {
            string currentUserId = HttpContext.Connection.RemoteIpAddress.ToString();
            I_User_ServiceLayer ius = SessionControler.GetSession(currentUserId);
            return "Visitir".Equals(ius.ToString());
        }
    }
}