using Linux;
using Linux.Models;
using Linux.Services;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using System.Linq;

namespace UnitTestProject1
{
    [TestClass]
    public class UsersTest


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
        [TestMethod]
        public void GetUsers()
        {
            _config = new ApiConfigurationSettings();
            _config.GroupFiles = "C:\\test\\group.txt"; //System.Environment.GetEnvironmentVariable("groupfiles");
            _config.Passwd = "C:\\test\\passwd.txt";//System.Environment.GetEnvironmentVariable("passwd");
            UserService userService = new UserService(_config);
            var query = new User();
            query.Name = "root";
            var result = userService.GetUsers(query).Result;

            Assert.IsFalse(result.Count !=1, "total user is 1");
            Assert.IsFalse(result[0].Uid != "0", "the uid is wrong");
            query.Uid = "0";
             result = userService.GetUsers(query).Result;
            Assert.IsFalse(result.Count != 1, "uid & name filter does not work");
          
            query.Shell = "/usr/bin/ksh";
             result = userService.GetUsers(query).Result;

            Assert.IsFalse(result.Count != 1, "uid & name & shel filter does not work");
       
        }

        [TestMethod]
        public void GetUserByUID()
        {
            _config = new ApiConfigurationSettings();
            _config.GroupFiles = "C:\\test\\group.txt"; //System.Environment.GetEnvironmentVariable("groupfiles");
            _config.Passwd = "C:\\test\\passwd.txt";//System.Environment.GetEnvironmentVariable("passwd");
            UserService userService = new UserService(_config);
            var query = new User();
            query.Name = "root";
            var result = userService.GetUserByUID("0").Result;

            Assert.IsNotNull(result , "GetUserByUID is wrong");
            Assert.IsFalse(result.Uid != "0", "the uid is wrong");

            result = userService.GetUserByUID("1").Result;
            Assert.IsNotNull(result, "GetUserByUID is wrong");
            Assert.IsFalse(result.Uid != "1", "the uid is wrong");

            result = userService.GetUserByUID("1000").Result;
            Assert.IsNull(result , "GetUserByUID is wrong");
      



      
        }

        [TestMethod]
        public void GetAllGroupsOfUser()
        {
            _config = new ApiConfigurationSettings();
            _config.GroupFiles = "C:\\test\\group.txt"; //System.Environment.GetEnvironmentVariable("groupfiles");
            _config.Passwd = "C:\\test\\passwd.txt";//System.Environment.GetEnvironmentVariable("passwd");
            UserService userService = new UserService(_config);
            var query = new User();
            query.Name = "root";
            var result = userService.GetAllGroupsOfUser("0").Result;

            Assert.IsNotNull(result, "null :GetAllGroupsOfUser is wrong");
            Assert.IsTrue(result.Count != 0, "count : GetAllGroupsOfUser");
            Assert.IsTrue(result.Count == 3, "count 3: GetAllGroupsOfUser");

            Assert.IsTrue(result[0].Gid == "0", "Gid 0: GetAllGroupsOfUser");
            Assert.IsTrue(result[1].Gid == "1", "Gid 1: GetAllGroupsOfUser");
            Assert.IsTrue(result[2].Gid == "4", "Gid 2: GetAllGroupsOfUser");

           






        }

    }

   
}
