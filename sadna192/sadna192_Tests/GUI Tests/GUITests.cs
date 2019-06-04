using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.IE;
using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium.Remote;
using System.Threading;

namespace sadna192_Tests.GUI_Tests
{
    [TestClass]
    public class GUITests
    {

        IWebDriver driver;
        ChromeOptions options = new ChromeOptions();

        [TestInitialize]
        public void startBrowser()
        {
            options.AddArguments("disable-infobars");
            driver = new ChromeDriver(options);
            driver.Navigate().GoToUrl("https://localhost:44309/");
        }


        [TestMethod]
        public void RegisterAndLogInTest()
        {
            driver.FindElement(By.XPath("/html/body/nav/div/div[2]/ul/li[3]")).Click();
            driver.FindElement(By.XPath("/html/body/div[2]/form/input[1]")).SendKeys("rrON9898");
            driver.FindElement(By.XPath("/html/body/div[2]/form/input[2]")).SendKeys("rrON9898");
            driver.FindElement(By.XPath("/html/body/div[2]/form/input[3]")).Click();
            driver.FindElement(By.XPath("/html/body/nav/div/div[2]/ul/li[1]")).Click();
            driver.FindElement(By.XPath("/html/body/div[2]/form/input[1]")).SendKeys("rrON9898");
            driver.FindElement(By.XPath("/html/body/div[2]/form/input[2]")).SendKeys("rrON9898");
            driver.FindElement(By.XPath("/html/body/div[2]/form/input[3]")).Click();
        }

        [TestMethod]
        public void RegisterAndLogInAndLogOutTest()
        {
            Thread.Sleep(2000);
            driver.FindElement(By.XPath("/html/body/nav/div/div[2]/ul/li[3]")).Click();
            Thread.Sleep(2000);
            driver.FindElement(By.XPath("/html/body/div[2]/form/input[1]")).SendKeys("Avi121212");
            Thread.Sleep(2000);
            driver.FindElement(By.XPath("/html/body/div[2]/form/input[2]")).SendKeys("Avi121212");
            Thread.Sleep(1000);
            driver.FindElement(By.XPath("/html/body/div[2]/form/input[3]")).Click();
            Thread.Sleep(1000);
            driver.FindElement(By.XPath("/html/body/nav/div/div[2]/ul/li[1]")).Click();
            Thread.Sleep(1000);
            driver.FindElement(By.XPath("/html/body/div[2]/form/input[1]")).SendKeys("Avi121212");
            Thread.Sleep(1000);
            driver.FindElement(By.XPath("/html/body/div[2]/form/input[2]")).SendKeys("Avi121212");
            Thread.Sleep(1000);
            driver.FindElement(By.XPath("/html/body/div[2]/form/input[3]")).Click();
            Thread.Sleep(1000);
            driver.FindElement(By.XPath("/html/body/nav/div/div[2]/ul/li[1]")).Click();
            Thread.Sleep(1000);

        }


        [TestMethod]
        public void SimpleSearchTest()
        {
            Thread.Sleep(2000);
            driver.FindElement(By.XPath("/html/body/nav/div/div[2]/ul/li[4]/form/input")).SendKeys("Banna");
            Thread.Sleep(2000);
            driver.FindElement(By.XPath("/html/body/nav/div/div[2]/ul/li[4]/form/button")).Click();
            Thread.Sleep(1000);
        }


        [TestMethod]
        public void AdvancedSearchTest()
        {
            Thread.Sleep(2000);
            driver.FindElement(By.XPath("/html/body/nav/div/div[2]/ul/li[5]")).Click();
            Thread.Sleep(1000);
            driver.FindElement(By.XPath("//*[@id='searchModal']/div/div/div[2]/form/input[3]")).SendKeys("food");
            Thread.Sleep(2000);
            driver.FindElement(By.XPath("//*[@id='searchModal']/div/div/div[2]/form/button[2]")).Click();
        }

        [TestMethod]
        public void AddingProductToCartTest()
        {
            Thread.Sleep(2000);
            driver.FindElement(By.XPath("/html/body/nav/div/div[2]/ul/li[5]")).Click();
            Thread.Sleep(1000);
            driver.FindElement(By.XPath("//*[@id='searchModal']/div/div/div[2]/form/input[3]")).SendKeys("food");
            Thread.Sleep(2000);
            driver.FindElement(By.XPath("//*[@id='searchModal']/div/div/div[2]/form/button[2]")).Click();
            Thread.Sleep(2000);
            driver.FindElement(By.XPath("/html/body/div[2]/table/tbody[1]/tr[1]/td[4]/a")).Click();
            Thread.Sleep(2000);
            driver.FindElement(By.XPath("/html/body/div[2]/button")).Click();
            Thread.Sleep(2000);
            driver.FindElement(By.XPath("//*[@id='AddToCart_Amount']")).SendKeys("10");
            Thread.Sleep(2000);
            driver.FindElement(By.XPath("//*[@id='addToCart']/form/button")).Click();
        }

        [TestMethod]
        public void AddingProductToCartAndChangeProductAmountTest()
        {
            Thread.Sleep(1500);
            driver.FindElement(By.XPath("/html/body/nav/div/div[2]/ul/li[5]")).Click();
            Thread.Sleep(1500);
            driver.FindElement(By.XPath("//*[@id='searchModal']/div/div/div[2]/form/input[3]")).SendKeys("food");
            Thread.Sleep(1500);
            driver.FindElement(By.XPath("//*[@id='searchModal']/div/div/div[2]/form/button[2]")).Click();
            Thread.Sleep(1500);
            driver.FindElement(By.XPath("/html/body/div[2]/table/tbody[1]/tr[1]/td[4]/a")).Click();
            Thread.Sleep(1500);
            driver.FindElement(By.XPath("/html/body/div[2]/button")).Click();
            Thread.Sleep(1500);
            driver.FindElement(By.XPath("//*[@id='AddToCart_Amount']")).SendKeys("10");
            Thread.Sleep(1500);
            driver.FindElement(By.XPath("//*[@id='addToCart']/form/button")).Click();
            Thread.Sleep(1500);
            driver.FindElement(By.XPath("//*[@id='update_BannatestStore']")).Click();
            Thread.Sleep(1500);
            driver.FindElement(By.XPath("//*[@id='form_BannatestStore']/input[1]")).Clear();
            driver.FindElement(By.XPath("//*[@id='form_BannatestStore']/input[1]")).SendKeys("8");
            Thread.Sleep(1500);
            driver.FindElement(By.XPath("//*[@id='form_BannatestStore']/input[2]")).Click();
        }


        [TestMethod]
        public void DeleteProductFromBasketTest()
        {
            Thread.Sleep(1500);
            driver.FindElement(By.XPath("/html/body/nav/div/div[2]/ul/li[5]")).Click();
            Thread.Sleep(1500);
            driver.FindElement(By.XPath("//*[@id='searchModal']/div/div/div[2]/form/input[3]")).SendKeys("food");
            Thread.Sleep(1500);
            driver.FindElement(By.XPath("//*[@id='searchModal']/div/div/div[2]/form/button[2]")).Click();
            Thread.Sleep(1500);
            driver.FindElement(By.XPath("/html/body/div[2]/table/tbody[1]/tr[1]/td[4]/a")).Click();
            Thread.Sleep(1500);
            driver.FindElement(By.XPath("/html/body/div[2]/button")).Click();
            Thread.Sleep(1500);
            driver.FindElement(By.XPath("//*[@id='AddToCart_Amount']")).SendKeys("10");
            Thread.Sleep(1500);
            driver.FindElement(By.XPath("//*[@id='addToCart']/form/button")).Click();
            Thread.Sleep(1500);
            driver.FindElement(By.XPath("//*[@id='br_BannatestStore']")).Click();
            Thread.Sleep(1500);
            driver.FindElement(By.XPath("//*[@id='remove_BannatestStore']/input[1]")).Click();
        }

        [TestMethod]
        public void PurchaseOfBasketTest()
        {
            Thread.Sleep(1500);
            driver.FindElement(By.XPath("/html/body/nav/div/div[2]/ul/li[5]")).Click();
            Thread.Sleep(1500);
            driver.FindElement(By.XPath("//*[@id='searchModal']/div/div/div[2]/form/input[3]")).SendKeys("food");
            Thread.Sleep(1500);
            driver.FindElement(By.XPath("//*[@id='searchModal']/div/div/div[2]/form/button[2]")).Click();
            Thread.Sleep(1500);
            driver.FindElement(By.XPath("/html/body/div[2]/table/tbody[1]/tr[1]/td[4]/a")).Click();
            Thread.Sleep(1500);
            driver.FindElement(By.XPath("/html/body/div[2]/button")).Click();
            Thread.Sleep(1500);
            driver.FindElement(By.XPath("//*[@id='AddToCart_Amount']")).SendKeys("10");
            Thread.Sleep(1500);
            driver.FindElement(By.XPath("//*[@id='addToCart']/form/button")).Click();
            Thread.Sleep(1500);
            driver.FindElement(By.XPath("/html/body/div[2]/form/input[1]")).Click();
            Thread.Sleep(1500);
            driver.FindElement(By.XPath("/html/body/div[2]/table[2]/tbody/tr/th[2]/form/input[1]")).SendKeys("123456789");
            Thread.Sleep(1500);
            driver.FindElement(By.XPath("/html/body/div[2]/table[2]/tbody/tr/th[2]/form/input[2]")).SendKeys("Ben Gurion University");
            Thread.Sleep(1500);
            driver.FindElement(By.XPath("/html/body/div[2]/table[2]/tbody/tr/th[2]/form/input[3]")).Click();
            Thread.Sleep(1500);
            driver.FindElement(By.XPath("/html/body/nav/div/div[2]/ul/li[4]/form/input")).SendKeys("Banna");
            Thread.Sleep(1500);
            driver.FindElement(By.XPath("/html/body/nav/div/div[2]/ul/li[4]/form/button")).Click();
            Thread.Sleep(1500);
            driver.FindElement(By.XPath("/html/body/div[2]/table/tbody[1]/tr/td[4]/a")).Click();
            Thread.Sleep(1500);
            driver.FindElement(By.XPath("/html/body/div[2]/button")).Click();
        }


        [TestMethod]
        public void AddingShopTest()
        {
            driver.FindElement(By.XPath("/html/body/nav/div/div[2]/ul/li[3]")).Click();
            driver.FindElement(By.XPath("/html/body/div[2]/form/input[1]")).SendKeys("rrON9898");
            driver.FindElement(By.XPath("/html/body/div[2]/form/input[2]")).SendKeys("rrON9898");
            driver.FindElement(By.XPath("/html/body/div[2]/form/input[3]")).Click();
            driver.FindElement(By.XPath("/html/body/nav/div/div[2]/ul/li[1]")).Click();
            driver.FindElement(By.XPath("/html/body/div[2]/form/input[1]")).SendKeys("rrON9898");
            driver.FindElement(By.XPath("/html/body/div[2]/form/input[2]")).SendKeys("rrON9898");
            driver.FindElement(By.XPath("/html/body/div[2]/form/input[3]")).Click();
            Thread.Sleep(1500);
            driver.FindElement(By.XPath("//*[@id='AddStoresButton']")).Click();
            Thread.Sleep(1500);
            driver.FindElement(By.XPath("//*[@id='addStoreDiv']/form/input[1]")).SendKeys("Shufersal");
            Thread.Sleep(1500);
            driver.FindElement(By.XPath("//*[@id='addStoreDiv']/form/input[2]")).Click();
        }


        [TestMethod]
        public void AddingProductToShopTest()
        {
            driver.FindElement(By.XPath("/html/body/nav/div/div[2]/ul/li[3]")).Click();
            driver.FindElement(By.XPath("/html/body/div[2]/form/input[1]")).SendKeys("Bibi1212");
            driver.FindElement(By.XPath("/html/body/div[2]/form/input[2]")).SendKeys("Bibi1212");
            driver.FindElement(By.XPath("/html/body/div[2]/form/input[3]")).Click();
            driver.FindElement(By.XPath("/html/body/nav/div/div[2]/ul/li[1]")).Click();
            driver.FindElement(By.XPath("/html/body/div[2]/form/input[1]")).SendKeys("Bibi1212");
            driver.FindElement(By.XPath("/html/body/div[2]/form/input[2]")).SendKeys("Bibi1212");
            driver.FindElement(By.XPath("/html/body/div[2]/form/input[3]")).Click();
            Thread.Sleep(1500);
            driver.FindElement(By.XPath("//*[@id='AddStoresButton']")).Click();
            Thread.Sleep(1500);
            driver.FindElement(By.XPath("//*[@id='addStoreDiv']/form/input[1]")).SendKeys("Shufersal");
            Thread.Sleep(1500);
            driver.FindElement(By.XPath("//*[@id='addStoreDiv']/form/input[2]")).Click();
            Thread.Sleep(1500);
            driver.FindElement(By.XPath("/html/body/div[2]/div[2]/a")).Click();
            Thread.Sleep(1500);
            driver.FindElement(By.XPath("/html/body/div[2]/button[4]")).Click();
            Thread.Sleep(1500);
            driver.FindElement(By.XPath("//*[@id='AP_ProductName']")).SendKeys("Kinder");
            Thread.Sleep(1500);
            driver.FindElement(By.XPath("//*[@id='AP_ProductAmount']")).SendKeys("20");
            Thread.Sleep(1500);
            driver.FindElement(By.XPath("//*[@id='AP_ProductCategory']")).SendKeys("food");
            Thread.Sleep(1500);
            driver.FindElement(By.XPath("//*[@id='AP_ProductPrice']")).SendKeys("3");
            Thread.Sleep(1500);
            driver.FindElement(By.XPath("//*[@id='addProductDiv']/form/input[6]")).Click();
            Thread.Sleep(1500);
            driver.FindElement(By.XPath("/html/body/div[2]/div[6]/a")).Click();
            Thread.Sleep(1500);
            driver.FindElement(By.XPath("/html/body/div[2]/button[1]")).Click();
        }

        //TODO
        [TestMethod]
        public void AddingManagerToShopTest()
        {
            driver.FindElement(By.XPath("/html/body/nav/div/div[2]/ul/li[3]")).Click();
            driver.FindElement(By.XPath("/html/body/div[2]/form/input[1]")).SendKeys("Bibi1212");
            driver.FindElement(By.XPath("/html/body/div[2]/form/input[2]")).SendKeys("Bibi1212");
            driver.FindElement(By.XPath("/html/body/div[2]/form/input[3]")).Click();
            driver.FindElement(By.XPath("/html/body/nav/div/div[2]/ul/li[1]")).Click();
            driver.FindElement(By.XPath("/html/body/div[2]/form/input[1]")).SendKeys("Bibi1212");
            driver.FindElement(By.XPath("/html/body/div[2]/form/input[2]")).SendKeys("Bibi1212");
            driver.FindElement(By.XPath("/html/body/div[2]/form/input[3]")).Click();
            Thread.Sleep(1500);
            driver.FindElement(By.XPath("//*[@id='AddStoresButton']")).Click();
            Thread.Sleep(1500);
            driver.FindElement(By.XPath("//*[@id='addStoreDiv']/form/input[1]")).SendKeys("Shufersal");
            Thread.Sleep(1500);
            driver.FindElement(By.XPath("//*[@id='addStoreDiv']/form/input[2]")).Click();
            Thread.Sleep(1500);
            driver.FindElement(By.XPath("/html/body/div[2]/div[2]/a")).Click();
            Thread.Sleep(1500);
            driver.FindElement(By.XPath("/html/body/div[2]/button[4]")).Click();
            Thread.Sleep(1500);
            driver.FindElement(By.XPath("//*[@id='AP_ProductName']")).SendKeys("Kinder");
            Thread.Sleep(1500);
            driver.FindElement(By.XPath("//*[@id='AP_ProductAmount']")).SendKeys("20");
            Thread.Sleep(1500);
            driver.FindElement(By.XPath("//*[@id='AP_ProductCategory']")).SendKeys("food");
            Thread.Sleep(1500);
            driver.FindElement(By.XPath("//*[@id='AP_ProductPrice']")).SendKeys("3");
            Thread.Sleep(1500);
            driver.FindElement(By.XPath("//*[@id='addProductDiv']/form/input[6]")).Click();
            Thread.Sleep(1500);
            driver.FindElement(By.XPath("/html/body/div[2]/div[6]/a")).Click();
            Thread.Sleep(1500);
            driver.FindElement(By.XPath("/html/body/div[2]/button[1]")).Click();
        }

        //TODO
        [TestMethod]
        public void AddingOwnerToShopTest()
        {
            driver.FindElement(By.XPath("/html/body/nav/div/div[2]/ul/li[3]")).Click();
            driver.FindElement(By.XPath("/html/body/div[2]/form/input[1]")).SendKeys("Bibi1212");
            driver.FindElement(By.XPath("/html/body/div[2]/form/input[2]")).SendKeys("Bibi1212");
            driver.FindElement(By.XPath("/html/body/div[2]/form/input[3]")).Click();
            driver.FindElement(By.XPath("/html/body/nav/div/div[2]/ul/li[1]")).Click();
            driver.FindElement(By.XPath("/html/body/div[2]/form/input[1]")).SendKeys("Bibi1212");
            driver.FindElement(By.XPath("/html/body/div[2]/form/input[2]")).SendKeys("Bibi1212");
            driver.FindElement(By.XPath("/html/body/div[2]/form/input[3]")).Click();
            Thread.Sleep(1500);
            driver.FindElement(By.XPath("//*[@id='AddStoresButton']")).Click();
            Thread.Sleep(1500);
            driver.FindElement(By.XPath("//*[@id='addStoreDiv']/form/input[1]")).SendKeys("Shufersal");
            Thread.Sleep(1500);
            driver.FindElement(By.XPath("//*[@id='addStoreDiv']/form/input[2]")).Click();
            Thread.Sleep(1500);
            driver.FindElement(By.XPath("/html/body/div[2]/div[2]/a")).Click();
            Thread.Sleep(1500);
            driver.FindElement(By.XPath("/html/body/div[2]/button[4]")).Click();
            Thread.Sleep(1500);
            driver.FindElement(By.XPath("//*[@id='AP_ProductName']")).SendKeys("Kinder");
            Thread.Sleep(1500);
            driver.FindElement(By.XPath("//*[@id='AP_ProductAmount']")).SendKeys("20");
            Thread.Sleep(1500);
            driver.FindElement(By.XPath("//*[@id='AP_ProductCategory']")).SendKeys("food");
            Thread.Sleep(1500);
            driver.FindElement(By.XPath("//*[@id='AP_ProductPrice']")).SendKeys("3");
            Thread.Sleep(1500);
            driver.FindElement(By.XPath("//*[@id='addProductDiv']/form/input[6]")).Click();
            Thread.Sleep(1500);
            driver.FindElement(By.XPath("/html/body/div[2]/div[6]/a")).Click();
            Thread.Sleep(1500);
            driver.FindElement(By.XPath("/html/body/div[2]/button[1]")).Click();
        }

        [TestMethod]
        public void EditProductoOfShopTest()
        {
            driver.FindElement(By.XPath("/html/body/nav/div/div[2]/ul/li[3]")).Click();
            driver.FindElement(By.XPath("/html/body/div[2]/form/input[1]")).SendKeys("Gigi1212");
            driver.FindElement(By.XPath("/html/body/div[2]/form/input[2]")).SendKeys("Gigi1212");
            driver.FindElement(By.XPath("/html/body/div[2]/form/input[3]")).Click();
            driver.FindElement(By.XPath("/html/body/nav/div/div[2]/ul/li[1]")).Click();
            driver.FindElement(By.XPath("/html/body/div[2]/form/input[1]")).SendKeys("Gigi1212");
            driver.FindElement(By.XPath("/html/body/div[2]/form/input[2]")).SendKeys("Gigi1212");
            driver.FindElement(By.XPath("/html/body/div[2]/form/input[3]")).Click();
            Thread.Sleep(1200);
            driver.FindElement(By.XPath("//*[@id='AddStoresButton']")).Click();
            Thread.Sleep(1200);
            driver.FindElement(By.XPath("//*[@id='addStoreDiv']/form/input[1]")).SendKeys("Shufersal");
            Thread.Sleep(1200);
            driver.FindElement(By.XPath("//*[@id='addStoreDiv']/form/input[2]")).Click();
            Thread.Sleep(1200);
            driver.FindElement(By.XPath("/html/body/div[2]/div[2]/a")).Click();
            Thread.Sleep(1200);
            driver.FindElement(By.XPath("/html/body/div[2]/button[4]")).Click();
            Thread.Sleep(1200);
            driver.FindElement(By.XPath("//*[@id='AP_ProductName']")).SendKeys("Kinder");
            Thread.Sleep(1200);
            driver.FindElement(By.XPath("//*[@id='AP_ProductAmount']")).SendKeys("20");
            Thread.Sleep(1200);
            driver.FindElement(By.XPath("//*[@id='AP_ProductCategory']")).SendKeys("food");
            Thread.Sleep(1200);
            driver.FindElement(By.XPath("//*[@id='AP_ProductPrice']")).SendKeys("3");
            Thread.Sleep(1200);
            driver.FindElement(By.XPath("//*[@id='addProductDiv']/form/input[6]")).Click();
            Thread.Sleep(1200);
            driver.FindElement(By.XPath("/html/body/div[2]/div[6]/a")).Click();
            Thread.Sleep(1500);
            driver.FindElement(By.XPath("/html/body/div[2]/button[1]")).Click();
            Thread.Sleep(1200);
            driver.FindElement(By.XPath("//*[@id='EditProduct_ProductPrice']")).Clear();
            driver.FindElement(By.XPath("//*[@id='EditProduct_ProductPrice']")).SendKeys("7");
            Thread.Sleep(1200);
            driver.FindElement(By.XPath("//*[@id='editProduct']/form/button")).Click();
            Thread.Sleep(1200);
            driver.FindElement(By.XPath("/html/body/nav/div/div[2]/ul/li[4]/form/input")).SendKeys("Kinder");
            Thread.Sleep(1200);
            driver.FindElement(By.XPath("/html/body/nav/div/div[2]/ul/li[4]/form/button")).Click();
        }

        [TestMethod]
        public void DeleteProductFromShopTest()
        {
            driver.FindElement(By.XPath("/html/body/nav/div/div[2]/ul/li[3]")).Click();
            driver.FindElement(By.XPath("/html/body/div[2]/form/input[1]")).SendKeys("Gigi1212");
            driver.FindElement(By.XPath("/html/body/div[2]/form/input[2]")).SendKeys("Gigi1212");
            driver.FindElement(By.XPath("/html/body/div[2]/form/input[3]")).Click();
            driver.FindElement(By.XPath("/html/body/nav/div/div[2]/ul/li[1]")).Click();
            driver.FindElement(By.XPath("/html/body/div[2]/form/input[1]")).SendKeys("Gigi1212");
            driver.FindElement(By.XPath("/html/body/div[2]/form/input[2]")).SendKeys("Gigi1212");
            driver.FindElement(By.XPath("/html/body/div[2]/form/input[3]")).Click();
            Thread.Sleep(1200);
            driver.FindElement(By.XPath("//*[@id='AddStoresButton']")).Click();
            Thread.Sleep(1200);
            driver.FindElement(By.XPath("//*[@id='addStoreDiv']/form/input[1]")).SendKeys("Shufersal");
            Thread.Sleep(1200);
            driver.FindElement(By.XPath("//*[@id='addStoreDiv']/form/input[2]")).Click();
            Thread.Sleep(1200);
            driver.FindElement(By.XPath("/html/body/div[2]/div[2]/a")).Click();
            Thread.Sleep(1200);
            driver.FindElement(By.XPath("/html/body/div[2]/button[4]")).Click();
            Thread.Sleep(1200);
            driver.FindElement(By.XPath("//*[@id='AP_ProductName']")).SendKeys("Kinder");
            Thread.Sleep(1200);
            driver.FindElement(By.XPath("//*[@id='AP_ProductAmount']")).SendKeys("20");
            Thread.Sleep(1200);
            driver.FindElement(By.XPath("//*[@id='AP_ProductCategory']")).SendKeys("food");
            Thread.Sleep(1200);
            driver.FindElement(By.XPath("//*[@id='AP_ProductPrice']")).SendKeys("3");
            Thread.Sleep(1200);
            driver.FindElement(By.XPath("//*[@id='addProductDiv']/form/input[6]")).Click();
            Thread.Sleep(1200);
            driver.FindElement(By.XPath("/html/body/div[2]/div[6]/a")).Click();
            Thread.Sleep(1200);
            driver.FindElement(By.XPath("/html/body/div[2]/form/button")).Click();

        }








    }
}
