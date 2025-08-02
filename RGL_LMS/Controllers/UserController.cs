using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using Azure.Storage.Blobs.Models;
using Azure.Storage.Blobs;
using Azure;
using Microsoft.AspNetCore.Authorization;
using RGL_LMS.Models;
using RGL_LMS.Dto;
using RGL_LMS.Service;



namespace RGL_LMS.Controllers
{
    
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly UserDAl _queryDAL;

        public UserController(UserDAl queryDAL)
        {
            _queryDAL = queryDAL;
        }

        

        [HttpGet("ping")]
        public IActionResult Get() => Ok("Working");



        [HttpGet("roles")]
        public IActionResult GetUserRoles()
        {
            var roles = Enum.GetNames(typeof(UserRole));
            return Ok(roles);
        }


        [HttpGet("GetUserList")]
        public async Task<IActionResult> GetUserList()
        {
            try
            {
                var userList = await _queryDAL.GetUserList();  // _queryDAL ကို သုံးပါ
                return Ok(userList);
            }
            catch (Exception ex)
            {
                // log exception
                return StatusCode(500, "Internal server error");
            }
        }
       

        [HttpGet("{id}")]
        public async Task<IActionResult> GetUserById(string id)
        {
            var data = await _queryDAL.GetUserById(id);
            return Ok(data);
        }

        //[HttpPost]
        //public async Task<IActionResult> SaveUser([FromForm] UserDto info)
        //{
        //    var msg = await _queryDAL.SaveUser(info);
        //    return Ok(msg);
        //}

        [HttpPost]
        public async Task<IActionResult> SaveUser([FromForm] UserDto info)
        {
            if (!Enum.IsDefined(typeof(UserRole), info.Role))
                return BadRequest("Invalid role.");

            var msg = await _queryDAL.SaveUser(info);
            return Ok(msg);
        }
    

        [HttpPut]
        public async Task<IActionResult> UpdateUser([FromForm] UserDto info)
        {
            var msg = await _queryDAL.UpdateUser(info);
            return Ok(msg);
        }
       
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(string id)
        {
            var msg = await _queryDAL.DeleteUser(id);
            return Ok(msg);
        }
    }


}
