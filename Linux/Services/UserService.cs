using Microsoft.Extensions.FileProviders;
using System;
using System.Collections.Generic;
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
    }
}
