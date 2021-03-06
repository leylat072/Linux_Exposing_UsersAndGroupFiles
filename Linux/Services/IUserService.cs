﻿using Linux.Models;
using Microsoft.Extensions.FileProviders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Linux.Services
{
    public interface IUserService
    {
        Task<List<Groups>> GetAllGroupsOfUser(string uid);
        Task<List<User>> GetUsers(User userQuery);
        Task<List<User>> GetAllUsers();
        Task<User> GetUserByUID(string uid);
        Task<List<Groups>> GetAllGroups();
        Task<List<Groups>> GetGroups(Groups query);
        Task<Groups> GetGroupByGID(string gid);
    }
}
