using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;

namespace UnitTestProject1
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            IWebDriver driver = new ChromeDriver(@"C:\Users\abia1\source\repos\Linux\UnitTestProject1\bin\Debug\netcoreapp2.1\chromedriver_win32\");
            //driver.Navigate().GoToUrl("http://www.google.com");
            driver.Navigate().GoToUrl("https://localhost:44382/api/users");
            Console.WriteLine("Test Completed !!!");
            /*IWebElement element = driver.FindElement(By.Name("q"));
            element.SendKeys("execute automation");
            IWebElement btnK = driver.FindElement(By.Name("btnK"));
            btnK.Submit();*/

            
        }
    }
}

