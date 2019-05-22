﻿using System;
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
            //SHELL
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
            while (watcher.passwdFileIsChanged == true)
            {
                users = await ReadFile();
                json = Newtonsoft.Json.JsonConvert.SerializeObject(users);
                watcher.passwdFileIsChanged = false;
            }
            return json;
        }

        [HttpGet("{uid}")]
        public async Task<ActionResult<string>> GetUserByUID(string uid)
        {
            //SHELL
            var json = string.Empty;
            var users = await ReadFile();
            var user = users.Where(u => u.Uid == uid).FirstOrDefault();
            if (user != null)           
                 json = Newtonsoft.Json.JsonConvert.SerializeObject(user);
            while (watcher.passwdFileIsChanged == true)
            {
                users = await ReadFile();
                user = users.Where(u => u.Uid == uid).FirstOrDefault();
                if (user != null)
                    json = Newtonsoft.Json.JsonConvert.SerializeObject(user);
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
        private Task<List<User>> ReadFile1()
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
        /// <summary>
        /// GET /users/<uid>/groups
        //Return all the groups for a given user.
        /// </summary>
        [HttpGet("{uid}/groups")]
        public void GetAllGroupsOfUser()
        {

        }
    }
}

group_name: It is the name of group.If you run ls -l command, you will see this name printed in the group field.
Password: Generally password is not used, hence it is empty/blank.It can store encrypted password.This is useful to implement privileged groups.
Group ID (GID): Each user must be assigned a group ID. You can see this number in your /etc/passwd file.
Group List: It is a list of user names of users who are members of the group.The user names, must be separated by commas.

GET /users/<uid>/groups
Return all the groups for a given user.
Example Response:
[
{“name”: “docker”, “gid”: 1002, “members”: [“dwoodlins”]}
]
GET /groups
Return a list of all groups on the system, a defined by /etc/group.
Example Response:
[
{“name”: “_analyticsusers”, “gid”: 250, “members”:
[“_analyticsd’,”_networkd”,”_timed”]},
{“name”: “docker”, “gid”: 1002, “members”: []}
]

GET
/groups/query[?name =< nq >][&gid =< gq >][&member =< mq1 >[&member =< mq2 >][&.
..]]
Return a list of groups matching all of the specified query fields.The bracket notation indicates that any of the
following query parameters may be supplied:
- name
- gid
- member(repeated)
Any group containing all the specified members should be returned, i.e.when query members are a subset of
group members.
Example Query: GET /groups/query? member = _analyticsd & member = _networkd
Example Response:
[
{“name”: “_analyticsusers”, “gid”: 250, “members”:
[“_analyticsd’,”_networkd”,”_timed”]}
]

GET /groups/<gid>
Return a single group with<gid>.Return 404 if <gid> is not found.