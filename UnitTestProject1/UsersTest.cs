using Linux;
using Linux.Models;
using Linux.Services;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using System.IO;
using System.Linq;

namespace UnitTestProject1
{
    [TestClass]
    public class UsersTest


    {
        public readonly IApiConfigurationSettings _config;
        public UsersTest()
        {
            LaunchSettingsFixture fixture = new LaunchSettingsFixture();
             _config = new ApiConfigurationSettings();
            _config.GroupFiles = System.Environment.GetEnvironmentVariable("groupfiles");
            _config.Passwd = System.Environment.GetEnvironmentVariable("passwd");
         //   _config.ContentRoot = env.ContentRootPath;
        }
        
        [TestMethod]
        public  void TestGetUsers()
        {
             UserService userService = new UserService(_config);
            var result =  userService.GetAllUsers().Result;

            Assert.IsFalse(result.Count != 18, "total user is 18");
        }
        [TestMethod]
        public void GetUsers()
        {
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

    public class LaunchSettingsFixture : IDisposable
    {
        public LaunchSettingsFixture()
        {
            using (var file = File.OpenText("Properties\\launchSettings.json"))
            {
                var reader = new JsonTextReader(file);
                var jObject = JObject.Load(reader);

                var variables = jObject
                    .GetValue("profiles")
                    //select a proper profile here
                    .SelectMany(profiles => profiles.Children())
                    .SelectMany(profile => profile.Children<JProperty>())
                    .Where(prop => prop.Name == "environmentVariables")
                    .SelectMany(prop => prop.Value.Children<JProperty>())
                    .ToList();

                foreach (var variable in variables)
                {
                    Environment.SetEnvironmentVariable(variable.Name, variable.Value.ToString());
                }
            }
        }

        public void Dispose()
        {
            // ... clean up
        }
    }


}

