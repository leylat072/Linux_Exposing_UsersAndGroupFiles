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
        public async Task<ActionResult> GetUsers([FromQuery]  User userQuery)
        {
            try
            {
                var ret = await _userService.GetUsers(userQuery);
                return Ok(ret);
            }
            catch (Exception)
            {
                // return StatusCode(StatusCodes.)
                return StatusCode(404);
            }

        }
        // GET api/values
        [HttpGet]
        public async Task<ActionResult> GetAllUsers()
        {
            try
            {
                var ret = await _userService.GetAllUsers();
                return Ok(ret);
            }
            catch (Exception)
            {
                // return StatusCode(StatusCodes.)
                return StatusCode(404);
            }
        }

        [HttpGet("{uid}")]
        public async Task<ActionResult> GetUserByUID(string uid)
        {
            try
            {
                var ret = await _userService.GetUserByUID(uid);
                if (ret == null) return StatusCode(404);
                return Ok(ret);
            }
            catch (Exception)
            {
                // return StatusCode(StatusCodes.)
                return StatusCode(404);
            }
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


