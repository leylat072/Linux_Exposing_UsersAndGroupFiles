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
using System.Collections.Generic;
using System.Linq;

namespace UnitTestProject1
{
    [TestClass]
    public class GroupsTest


    {
        public readonly IApiConfigurationSettings _config;
        public GroupsTest()
        {
            LaunchSettingsFixture fixture = new LaunchSettingsFixture();
            _config = new ApiConfigurationSettings();
            _config.GroupFiles = System.Environment.GetEnvironmentVariable("groupfiles");
            _config.Passwd = System.Environment.GetEnvironmentVariable("passwd");
            //   _config.ContentRoot = env.ContentRootPath;
        }

        [TestMethod]
        public void GetAllGroups()
        {
           UserService userService = new UserService(_config);
            var result = userService.GetAllGroups().Result;
            Assert.IsNotNull(result, "null: GetAllGroups");
            Assert.IsTrue(result.Count == 4, "count : GetAllGroups");
        }
        [TestMethod]
        public void GetGroups()
        {
              UserService userService = new UserService(_config);
            var query = new Groups();
            query.Name = "root";
            var result = userService.GetGroups(query).Result;

            Assert.IsNotNull(result, "null; GetGroups");
            Assert.IsTrue(result.Count == 1, "count: GetGroups");
            Assert.IsTrue(result[0].Gid == "0", "gid : GetGroups");
            query.Gid = "0";
            result = userService.GetGroups(query).Result;
            Assert.IsNotNull(result, "null; GetGroups");
            Assert.IsTrue(result.Count == 1, "count: GetGroups");
            Assert.IsTrue(result[0].Gid == "0", "gid : GetGroups");
            query = new Groups();
            query.Member = new System.Collections.Generic.List<string>();
            query.Member.Add("bin");
            query.Member.Add("root");
            result = userService.GetGroups(query).Result;

            Assert.IsNotNull(result, "null; GetGroups");
            Assert.IsTrue(result.Count == 2, "count: GetGroups");
            Assert.IsTrue((result[0].Member.Contains("bin") && result[0].Member.Contains("root")), "contain row1 : GetGroups");
            Assert.IsTrue((result[1].Member.Contains("bin") && result[0].Member.Contains("root")), "contain row 2 : GetGroups");
            Assert.IsTrue(result[0].Gid == "1", "gid 0: GetGroups");
            Assert.IsTrue(result[1].Gid == "4", "gid 4: GetGroups");
        }

        [TestMethod]
        public void GetGroupByGID()
        {
            UserService userService = new UserService(_config);
            var query = new Groups();
            query.Name = "root";
            var result = userService.GetGroupByGID("0").Result;

            Assert.IsNotNull(result, " null: GetGroupByGID is wrong");
            Assert.IsFalse(result.Gid != "0", "gid : GetGroupByGID is wrong");

            result = userService.GetGroupByGID("1").Result;
            Assert.IsNotNull(result, "null: GetGroupByGID is wrong");
            Assert.IsTrue(result.Gid == "1", "gid : GetGroupByGID is wrong");

            result = userService.GetGroupByGID("1000").Result;
            Assert.IsNull(result, "null : GetGroupByGID is wrong");





        }

      
    }


}

