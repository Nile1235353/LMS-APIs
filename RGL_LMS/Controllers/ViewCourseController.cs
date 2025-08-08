//using Microsoft.AspNetCore.Http;
//using Microsoft.AspNetCore.Mvc;

//namespace RGL_LMS.Controllers
//{
//    [Route("api/[controller]")]
//    [ApiController]
//    public class ViewCourseController : ControllerBase
//    {
//    }
//}

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RGL_LMS.DTO;
using RGL_LMS.Service;
using RGL_LMS.Dto;
using RGL_LMS.Models;

namespace RGL_LMS.Controllers {

    [ApiController]
    [Route("api/[controller]")]
    public class ViewCourseController : ControllerBase
    {
        private readonly ViewCourseDAL _dal;
        private readonly UserDAl _userService;
        private readonly RGL_LMS_DBContext _context;
        private readonly CourseDAL _courseDal;

        // Inject both ViewCourseDAL and IUserService in the constructor
        public ViewCourseController(RGL_LMS_DBContext context,ViewCourseDAL dal, UserDAl userService, CourseDAL courseDal)
        {
            _dal = dal;
            _userService = userService;  // Assign injected userService here
            _context = context;
            _courseDal = courseDal;
        }


       
        [HttpGet("GetCourseById/{courseId}")]
        public async Task<IActionResult> GetCourseById(string courseId)
        {
            var result = await _dal.GetCourseById(courseId); 
            if (result == null) return NotFound("Course not found.");
            return Ok(result);
        }
       
        [HttpGet("GetUserById/{id}")]
        public async Task<IActionResult> GetUserById(string id)
        {
            var data = await _dal.GetUserById(id);  
            if (data == null)
                return NotFound(new { message = "User not found" });

            return Ok(data);
        }


        [HttpGet("GetAllViewCourse")]
        public async Task<IActionResult> GetAllViewCourseData()
        {
            var data = await _dal.GetAllViewCourseAsync();
            return Ok(data);
        }

        //[HttpGet("getbyid/{id}")]
        //public async Task<IActionResult> GetById(string id)
        //{
        //    var item = await _dal.GetByIdAsync(id);
        //    if (item == null) return NotFound("Not found");
        //    return Ok(item);
        //}

        [HttpPost("create")]
        public async Task<IActionResult> Create([FromBody] ViewCourseDto dto)
        {
            var res = await _dal.CreateAsync(dto);
            return res.Status ? Ok(res) : BadRequest(res);
        }

        //[HttpPut("update")]
        //public async Task<IActionResult> Update([FromBody] ViewCourseDto dto)
        //{
        //    var res = await _dal.UpdateAsync(dto);
        //    return res.Status ? Ok(res) : BadRequest(res);
        //}

        //[HttpDelete("delete/{id}")]
        //public async Task<IActionResult> Delete(string id)
        //{
        //    var res = await _dal.DeleteAsync(id);
        //    return res.Status ? Ok(res) : BadRequest(res);
        //}
    }
}




