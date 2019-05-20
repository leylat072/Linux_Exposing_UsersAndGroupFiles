using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Linux.Models;
using Microsoft.Extensions.FileProviders;
using System.IO;
using Linux.Classes;

namespace Linux.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IFileProvider _fileProvider;

        public UsersController(IFileProvider fileProvider)
        {

            _fileProvider = fileProvider;

        }
        // GET api/values
        [HttpGet("query")]
        public async Task<ActionResult<string>> GetUsers([FromQuery]  User userQuery)
        {
            var users = await ReadFile();
            users = userQuery.Name == null ? users : users.Where(u => u.Name == userQuery.Name).ToList();
            users = userQuery.Uid == null ? users : users.Where(u => u.Uid == userQuery.Uid).ToList();
            users = userQuery.Gid == null ? users : users.Where(u => u.Gid == userQuery.Gid).ToList();
            users = userQuery.Home == null ? users : users.Where(u => u.Home == userQuery.Home).ToList();
            users = userQuery.Comment == null ? users : users.Where(u => u.Comment == userQuery.Comment).ToList();
            var json = Newtonsoft.Json.JsonConvert.SerializeObject(users);
            while (watcher.passwdFileIsChanged == true)
            {
                users = await ReadFile();
                json = Newtonsoft.Json.JsonConvert.SerializeObject(users);
                watcher.passwdFileIsChanged = false;
            }        

            return json;
        }
        // GET api/values
        [HttpGet]
        public async Task<ActionResult<string>> GetAllUsers()
        {
            var users = await ReadFile();
            var json = Newtonsoft.Json.JsonConvert.SerializeObject(users);
            while(watcher.passwdFileIsChanged == true)
            {
                users = await ReadFile();
                json = Newtonsoft.Json.JsonConvert.SerializeObject(users);
                watcher.passwdFileIsChanged = false;
            }
            return json;
        }

        private Task<List<User>> ReadFile()
        {
            Task<List<User>> obTask = Task.Run(() =>
            {
                /*
                * User name
                   Encrypted password
                   User ID number (UID)
                   User's group ID number (GID)
                   Full name of the user (GECOS)
                   User home directory
                   Login shell root:!:0:0::/:/usr/bin/ksh
                */

                FileStream fileStream = new FileStream("C:\\test\\test.txt", FileMode.Open);
                string line = "";
                using (StreamReader reader = new StreamReader(fileStream))
                {
                    line = reader.ReadToEnd();
                }
                string[] vv = line.Split("\r\n");
                var users = new List<User>();
                foreach (string s in vv)
                {
                    var user = new User();
                    var s2 = s.Split(":");
                    user.Name = s2[0];
                    user.Uid = s2[2];
                    user.Gid = s2[3];
                    user.Comment = s2[4];
                    user.Home = s2[5];
                    users.Add(user);
                }

                return users;

            });
            return obTask;        
           
        }


    }
}
