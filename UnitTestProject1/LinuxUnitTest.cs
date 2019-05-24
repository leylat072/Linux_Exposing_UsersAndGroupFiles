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
             _config = new ApiConfigurationSettings();
            _config.GroupFiles = "C:\\test\\group.txt"; //System.Environment.GetEnvironmentVariable("groupfiles");
            _config.Passwd = "C:\\test\\passwd.txt";//System.Environment.GetEnvironmentVariable("passwd");
            UserService userService = new UserService(_config);
            var result =  userService.GetAllUsers().Result;

            Assert.IsFalse(result.Count != 18, "total user is 18");




        }
       
    }

   
}

