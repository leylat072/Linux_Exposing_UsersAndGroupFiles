using Linux;
using Linux.Services;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;

namespace UnitTestProject1
{
    [TestClass]
    public class LinuxUnitTest


    {
        public  IApiConfigurationSettings _config;
        [TestMethod]
        public  void TestGetUsers()
        {
            // var passwd = System.Environment.GetEnvironmentVariable("passwd");
            //IWebDriver driver = new ChromeDriver(@"C:\Users\abia1\source\repos\Linux\UnitTestProject1\bin\Debug\netcoreapp2.1\chromedriver_win32\");
            //   driver.Navigate().GoToUrl("https://localhost:44382/api/users");
            //Console.WriteLine("Test Completed !!!");
            // var services = new ServiceCollection();
            // services.AddSingleton(typeof(IApiConfigurationSettings), _config);
            //IUserService iuserService;
            // IFileProvider fileProvide = null;
           // var buider = new ConfigurationBuilder().set

            //var server = new TestServer(new WebHostBuilder()
            //.UseConfiguration(config)
            //.UseStartup<Startup>());
            _config = new ApiConfigurationSettings();
            _config.GroupFiles = "C:\\test\\group.txt"; //System.Environment.GetEnvironmentVariable("groupfiles");
            _config.Passwd = "C:\\test\\passwd.txt";//System.Environment.GetEnvironmentVariable("passwd");
            UserService userService = new UserService(_config);
            var result =  userService.GetAllUsers().Result;

            Assert.IsFalse(result.Count != 18, "total user is 18");




        }
        //[TestMethod]
        //public void TestMethod1()
        //{
        //    IWebDriver driver = new ChromeDriver(@"C:\Users\abia1\source\repos\Linux\UnitTestProject1\bin\Debug\netcoreapp2.1\chromedriver_win32\");
        //    //driver.Navigate().GoToUrl("http://www.google.com");
        //    driver.Navigate().GoToUrl("https://localhost:44382/api/users");
        //    Console.WriteLine("Test Completed !!!");
        //    /*IWebElement element = driver.FindElement(By.Name("q"));
        //    element.SendKeys("execute automation");
        //    IWebElement btnK = driver.FindElement(By.Name("btnK"));
        //    btnK.Submit();*/

            
        //}
    }

   
}

