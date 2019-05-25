using Linux.Models;
using Microsoft.Extensions.FileProviders;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Linux.Services
{
    public class UserService : IUserService
    {
        private readonly IFileProvider _fileProvider;
        private readonly IApiConfigurationSettings _iApiConfigurationSettings;
        public UserService(IApiConfigurationSettings iApiConfigurationSettings)
        {

            _iApiConfigurationSettings = iApiConfigurationSettings;


        }
        //public UserService(IFileProvider fileProvider)
        //{

        //    _fileProvider = fileProvider;


        //}
        public async Task<List<User>> GetUsers(User userQuery)
        {
            //SHELL
            var users = await ReadPasswdFile();
            users = userQuery.Name == null ? users : users.Where(u => u.Name == userQuery.Name).ToList();
            users = userQuery.Uid == null ? users : users.Where(u => u.Uid == userQuery.Uid).ToList();
            users = userQuery.Gid == null ? users : users.Where(u => u.Gid == userQuery.Gid).ToList();
            users = userQuery.Home == null ? users : users.Where(u => u.Home == userQuery.Home).ToList();
            users = userQuery.Comment == null ? users : users.Where(u => u.Comment == userQuery.Comment).ToList();
            users = userQuery.Shell == null ? users : users.Where(u => u.Shell == userQuery.Shell).ToList();
            //var json = Newtonsoft.Json.JsonConvert.SerializeObject(users);
            /* while (watcher.passwdFileIsChanged == true)
             {
                 users = await ReadFile();
                 json = Newtonsoft.Json.JsonConvert.SerializeObject(users);
                 watcher.passwdFileIsChanged = false;
             }*/

            return users;
        }
        public async Task<List<User>> GetAllUsers()
        {
            var users = await ReadPasswdFile();
            //  var json = Newtonsoft.Json.JsonConvert.SerializeObject(users);
            /* while (watcher.passwdFileIsChanged == true)
             {
                 users = await ReadFile();
                 json = Newtonsoft.Json.JsonConvert.SerializeObject(users);
                 watcher.passwdFileIsChanged = false;
             }*/
            return users;
        }
        public async Task<User> GetUserByUID(string uid)
        {
            //SHELL
            var json = string.Empty;
            var users = await ReadPasswdFile();
            var user = users.Where(u => u.Uid == uid).FirstOrDefault();
            return user;
            /*if (user != null)
                json = Newtonsoft.Json.JsonConvert.SerializeObject(user);*/
            /* while (watcher.passwdFileIsChanged == true)
             {
                 users = await ReadFile();
                 user = users.Where(u => u.Uid == uid).FirstOrDefault();
                 if (user != null)
                     json = Newtonsoft.Json.JsonConvert.SerializeObject(user);
                 watcher.passwdFileIsChanged = false;
             }*/

            //return json;
        }
        public async Task<List<Groups>> GetAllGroupsOfUser(string uid)
        {
            var json = string.Empty;
            var users = await ReadPasswdFile();
            var groups = await ReadGroupFile();
            var user = users.Where(u => u.Uid == uid).FirstOrDefault();
            if (user == null || groups == null) return new List<Groups>();
            var findGroups = new List<Groups>();
            foreach (var group in groups)
            {
                if (group.Member.Contains(user.Name))
                    findGroups.Add(group);
            }
            return findGroups;

        }
        private Task<List<User>> ReadPasswdFile()
        {
            var passwd = _iApiConfigurationSettings.Passwd;
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

                FileStream fileStream = new FileStream(passwd, FileMode.Open);
                string line = "";
                using (StreamReader reader = new StreamReader(fileStream))
                {
                    line = reader.ReadToEnd();
                }
                string[] passwdUsers = line.Split("\r\n");
                var users = new List<User>();
                foreach (string passwdUser in passwdUsers)
                {
                    var user = new User();
                    var passwdUserElements = passwdUser.Split(":");
                    user.Name = passwdUserElements[0];
                    user.Uid = passwdUserElements[2];
                    user.Gid = passwdUserElements[3];
                    user.Comment = passwdUserElements[4];
                    user.Home = passwdUserElements[5];
                    user.Shell = passwdUserElements[6];
                    users.Add(user);
                }

                return users;

            });
            return obTask;

        }
        private Task<List<Groups>> ReadGroupFile()
        {
           var groupFileName = _iApiConfigurationSettings.GroupFiles;
            Task<List<Groups>> obTask = Task.Run(() =>
            {
                /*
                group_name: It is the name of group.If you run ls -l command, you will see this name printed in the group field.
                Password: Generally password is not used, hence it is empty/blank.It can store encrypted password.This is useful to implement privileged groups.
                Group ID (GID): Each user must be assigned a group ID. You can see this number in your /etc/passwd file.
                Group List: It is a list of user names of users who are members of the group.The user names, must be separated by commas.
                */

                FileStream fileStream = new FileStream(groupFileName, FileMode.Open);
                string line = "";
                using (StreamReader reader = new StreamReader(fileStream))
                {
                    line = reader.ReadToEnd();
                }
                string[] groupFiles = line.Split("\r\n");
                var groups = new List<Groups>();
                foreach (string groupFile in groupFiles)
                {
                    var group = new Groups();
                    var groupFileInfo = groupFile.Split(":");
                    group.Name = groupFileInfo[0];
                    group.Gid = groupFileInfo[2];
                    group.Member = new List<string>();
                    var groupFileMembers = groupFileInfo[3].ToString().Split(",");
                    foreach (string groupFileMember in groupFileMembers)
                        group.Member.Add(groupFileMember);
                    groups.Add(group);
                }
                return groups;
            });
            return obTask;
        }
        public async Task<List<Groups>> GetAllGroups()
        {
            //SHELL
            var groups = await ReadGroupFile();
            return groups;
        }
        public async Task<List<Groups>> GetGroups(Groups query)
        {
            //SHELL
            var groups = await ReadGroupFile();
            groups = query.Name == null ? groups : groups.Where(u => u.Name == query.Name).ToList();
            groups = query.Gid == null ? groups : groups.Where(u => u.Gid == query.Gid).ToList();
            groups = query.Member == null ? groups : groups.Where(p => query.Member.All(x => p.Member.Contains(x))).ToList();

            //var json = Newtonsoft.Json.JsonConvert.SerializeObject(users);
            /* while (watcher.passwdFileIsChanged == true)
             {
                 users = await ReadFile();
                 json = Newtonsoft.Json.JsonConvert.SerializeObject(users);
                 watcher.passwdFileIsChanged = false;
             }*/

            return groups.OrderBy(g => g.Gid).ToList();
        }
        public async Task<Groups> GetGroupByGID(string gid)
        {
            //SHELL
            var json = string.Empty;
            var groups = await ReadGroupFile();
            var group = groups.Where(u => u.Gid == gid).FirstOrDefault();
            return group;
            /*if (user != null)
                json = Newtonsoft.Json.JsonConvert.SerializeObject(user);*/
            /* while (watcher.passwdFileIsChanged == true)
             {
                 users = await ReadFile();
                 user = users.Where(u => u.Uid == uid).FirstOrDefault();
                 if (user != null)
                     json = Newtonsoft.Json.JsonConvert.SerializeObject(user);
                 watcher.passwdFileIsChanged = false;
             }*/

            //return json;
        }

    }
}

/*Example Query: GET /users/query? shell =% 2Fbin%2Ffalse
Example Response:
[
{“name”: “dwoodlins”, “uid”: 1001, “gid”: 1001, “comment”: “”, “home”:
“/home/dwoodlins”, “shell”: “/bin/false”}
]
GET /users/<uid>
Return a single user with<uid>.Return 404 if <uid> is not found.
Example Response:
{“name”: “dwoodlins”, “uid”: 1001, “gid”: 1001, “comment”: “”, “home”:
“/ home / dwoodlins”, “shell”: “/ bin / false”}
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
Example Response:
{“name”: “docker”, “gid”: 1002, “members”: [“dwoodlins”]}
*/
