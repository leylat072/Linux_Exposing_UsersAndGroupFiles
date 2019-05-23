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
    public class GroupsController : ControllerBase
    {
        private readonly IFileProvider _fileProvider;
        private readonly IUserService _userService;
        public GroupsController(IFileProvider fileProvider, IUserService userService)
        {

            _fileProvider = fileProvider;
            _userService = userService;

        }
        // GET api/values
        [HttpGet("query")]
        public async Task<ActionResult> GetGroups([FromQuery]  Groups query)
        {
            try
            {
                var ret = await _userService.GetGroups(query);
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
        public async Task<ActionResult> GetAllGroups()
        {
            try
            {
                var ret = await _userService.GetAllGroups();
                return Ok(ret);
            }
            catch (Exception)
            {
                // return StatusCode(StatusCodes.)
                return StatusCode(404);
            }
        }

        [HttpGet("{gid}")]
        public async Task<ActionResult> GetGroupByGID(string gid)
        {
            try
            {
                var ret = await _userService.GetGroupByGID(gid);
                if (ret == null) return StatusCode(404);
                return Ok(ret);
            }
            catch (Exception)
            {
                // return StatusCode(StatusCodes.)
                return StatusCode(404);
            }
        }

       

        
    }
}


