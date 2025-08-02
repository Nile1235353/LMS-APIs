using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RGL_LMS.Dto;
using RGL_LMS.Models;
using RGL_LMS.Service;
using System;

namespace RGL_LMS.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CourseController : ControllerBase
    {
        private readonly CourseDAL _courseDal;
        private readonly RGL_LMS_DBContext _context;


        public CourseController(RGL_LMS_DBContext context, CourseDAL courseDal)
        {
            _context = context;
            _courseDal = courseDal;

           
        }

        //[HttpPost("Create")]
        //public async Task<IActionResult> CreateCourse([FromForm] CourseDto dto)
        //{
        //    var result = await _courseDal.CreateCourseAsync(dto);
        //    return Ok(result);
        //}

        [HttpPost("Create")]
        public async Task<IActionResult> CreateCourse([FromForm] CourseDto dto)
        {
            var result = await _courseDal.CreateCourseAsync(dto);

            if (result.Status)
            {
                return Ok(result); // success
            }
            else
            {
                // Return proper error response
                return BadRequest(result.MessageContent);
            }
        }

        [HttpPut("Update")]
        public async Task<IActionResult> UpdateCourse([FromForm] CourseDto info)
        {
            var result = await _courseDal.UpdateCourse(info);
            return Ok(result);
        }
     



        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCourse(string id)
        {
            var result = await _courseDal.DeleteCourse(id);
            return Ok(result);
        }

        //[HttpGet("GetAll")]
        //public async Task<IActionResult> GetAllCourses()
        //{
        //    var result = await _courseDal.GetAllCoursesAsync();
        //    return Ok(result);
        //}

        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAllCourses()
        {
            var courses = await _courseDal.GetAllCoursesAsync();
            return Ok(courses);
        }

        [HttpGet("GetById/{id}")]
        public async Task<IActionResult> GetCourseById(int id)
        {
            var result = await _courseDal.GetCourseByIdAsync(id);
            if (result == null) return NotFound();
            return Ok(result);
        }
     
        //[HttpGet("GetById/{courseId}")]
        //public async Task<IActionResult> GetCourseById(int courseId)
        //{
        //    var result = await _courseDal.GetCourseByIdAsync(courseId);
        //    if (result == null) return NotFound();
        //    return Ok(result);
        //}

        [HttpGet("GetUsersByRole/{role}")]
        public async Task<IActionResult> GetUsersByRole(string role)
        {
            var validRoles = new[] { "Admin", "Instructor" };

            if (!validRoles.Contains(role, StringComparer.OrdinalIgnoreCase))
            {
                return BadRequest("Invalid role.");
            }

            var lowerRole = role.ToLower();

            var users = await _context.User
                .Where(u => u.Role.ToLower() == lowerRole)
                .Select(u => new
                {
                    u.UserId,
                    Name = u.FullName // assuming FullName is the actual field
                })
                .ToListAsync();

            return Ok(users);
        }


    }
}
