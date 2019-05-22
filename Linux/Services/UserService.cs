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
        private readonly IUserService _userService;

        public UserService(IFileProvider fileProvider, IUserService  userService )        {

            _fileProvider = fileProvider;
            _userService = userService;

        }
        public async Task<List<Groups>> GetAllGroupsOfUser(string uid)
        {
            var json = string.Empty;
            var users = await ReadFile();
            var groups = await ReadGroupFile();
            var user = users.Where(u => u.Uid == uid).FirstOrDefault();
            if (user == null || groups ==null) return new List<Groups>();
            var findGroups = new List<Groups>();
            foreach (var group in groups)
            {
                if (group.GroupList.Contains(user.Name))
                    findGroups.Add(group);
            }
            return findGroups;

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
        private Task<List<Groups>> ReadGroupFile()
        {
            Task<List<Groups>> obTask = Task.Run(() =>
            {
                /*
                group_name: It is the name of group.If you run ls -l command, you will see this name printed in the group field.
                Password: Generally password is not used, hence it is empty/blank.It can store encrypted password.This is useful to implement privileged groups.
                Group ID (GID): Each user must be assigned a group ID. You can see this number in your /etc/passwd file.
                Group List: It is a list of user names of users who are members of the group.The user names, must be separated by commas.
                */

                FileStream fileStream = new FileStream("C:\\test\\test.txt", FileMode.Open);
                string line = "";
                using (StreamReader reader = new StreamReader(fileStream))
                {
                    line = reader.ReadToEnd();
                }
                string[] vv = line.Split("\r\n");
                var groups = new List<Groups>();
                foreach (string s in vv)
                {
                    var group = new Groups();
                    var s2 = s.Split(":");
                    group.GroupName = s2[0];
                    group.Gid = s2[2];
                    group.GroupList = new List<string>();
                    var gl = s.Split(",");
                    foreach (string g in gl)
                        group.GroupList.Add(g);
                    groups.Add(group);
                }
                return groups;
            });
            return obTask;
        }
    }
}
