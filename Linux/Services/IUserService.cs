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
    }
}
