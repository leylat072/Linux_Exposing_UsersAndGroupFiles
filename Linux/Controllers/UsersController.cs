using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Linux.Models;
using Microsoft.Extensions.FileProviders;
using System.IO;
using Linux.Classes;
using Linux.Services;

namespace Linux.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IFileProvider _fileProvider;
        private readonly IUserService _userService;
        public UsersController(IFileProvider fileProvider, IUserService userService)
        {

            _fileProvider = fileProvider;
            _userService = userService;

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
        
        /// <summary>
        /// GET /users/<uid>/groups
        //Return all the groups for a given user.
        /// </summary>
        [HttpGet("{uid}/groups")]
        public async Task<IActionResult>  GetAllGroupsOfUser(string uid)
        {
            try
            {
                var ret =  await _userService.GetAllGroupsOfUser(uid);
                return Ok(ret);
            }
            catch(Exception)
            {
                // return StatusCode(StatusCodes.)
                return StatusCode(404);
            }
           
           
        }
    }
}



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