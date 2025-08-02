using Microsoft.EntityFrameworkCore;
using RGL_LMS.Dto;
using RGL_LMS.Models;
using AutoMapper;
using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System.Data;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Hosting;
using System.Linq;

namespace RGL_LMS.Service
{
    public class CourseDAL
    {
        private readonly RGL_LMS_DBContext _context;
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly ILogger<CourseDAL> _logger;

        string _conStr;
        public CourseDAL(IConfiguration config, RGL_LMS_DBContext context, IMapper mapper, IWebHostEnvironment webHostEnvironment, ILogger<CourseDAL> logger)
        {
            _context = context;
            _configuration = config;
            _conStr = _configuration.GetConnectionString("DefaultConnection");
            _webHostEnvironment = webHostEnvironment;
            _logger = logger;
            _mapper = mapper;
        }


        public bool IsLinux
        {
            get
            {
                int p = (int)Environment.OSVersion.Platform;
                return (p == 4) || (p == 6) || (p == 128);
            }
        }

        public DateTime GetLocalStdDT()
        {
            if (!IsLinux)
            {
                DateTime localTime = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTime.Now, TimeZoneInfo.Local.Id, "Myanmar Standard Time");
                return localTime;
            }
            else
            {
                TimeZoneInfo tzi = TimeZoneInfo.FindSystemTimeZoneById("Asia/Yangon");
                DateTime pacific = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, tzi);
                return pacific;
            }
        }

        public async Task<DataTable> GetDataTableAsync(string sSQL, params SqlParameter[] para)
        {
            using var newCon = new SqlConnection(_conStr);
            using var cmd = new SqlCommand(sSQL, newCon);
            cmd.CommandType = CommandType.Text;

            if (para != null && para.Length > 0)
                cmd.Parameters.AddRange(para);

            await newCon.OpenAsync();

            using var reader = await cmd.ExecuteReaderAsync();

            var dt = new DataTable();
            dt.Load(reader); // loads data from reader into DataTable

            return dt;
        }

        #region Register Course Jun-10-2025

        public async Task<List<UserDto>> GetUsersByRoleAsync(string role)
        {
            var validRoles = new[] { "Admin", "Instructor" };
            if (!validRoles.Contains(role, StringComparer.OrdinalIgnoreCase))
                return new List<UserDto>(); // Empty list if not allowed

            return await _context.User
                .Where(u => u.Role == role && u.IsActive == true)
                .Select(u => new UserDto
                {
                    UserId = u.UserId,
                    FullName = u.FullName
                })
                .ToListAsync();
        }     
        public async Task<ResponseMessage> CreateCourseAsync(CourseDto dto)
        {
            ResponseMessage msg = new() { Status = false };

            try
            {
                // Only Admin and Instructor roles are allowed
                if (dto.Role != UserRole.Admin && dto.Role != UserRole.Instructor)
                {
                    msg.MessageContent = "Only Admin and Instructor roles are allowed.";
                    return msg;
                }

                // Get user info by role & userId
                var user = await _context.User
                    .FirstOrDefaultAsync(u => u.UserId == dto.UserId && u.Role == dto.Role.ToString());

                if (user == null)
                {
                    msg.MessageContent = $"{dto.Role} with specified UserId not found.";
                    return msg;
                }
                // ✅ Check if Course already exists with same UserId
                var exists = await _context.Courses.AnyAsync(c => c.UserId == dto.UserId);
                if (exists)
                {
                    msg.MessageContent = "A course already exists with the specified UserId.";
                    return msg;
                }

                int count = await _context.Courses.CountAsync();
                string courseId = $"C-{(count + 1):D4}";

                var course = _mapper.Map<Courses>(dto);
                course.CourseId = courseId;
                course.Role = user.Role;
                course.Name = user.FullName;
                course.CreatedDate = GetLocalStdDT();
                course.IsActive = true;

                _context.Courses.Add(course);
                await _context.SaveChangesAsync();

                msg.Status = true;
                msg.MessageContent = "Course created successfully!";
            }
            catch (Exception ex)
            {
                msg.MessageContent = ex.InnerException?.Message ?? ex.Message;
            }

            return msg;
        }

        public async Task<ResponseMessage> UpdateCourse([FromForm] CourseDto info)
        {
            ResponseMessage msg = new() { Status = false };
            try
            {
                var data = await _context.Courses
                    .FirstOrDefaultAsync(u => u.CourseId == info.CourseId);

                if (data == null)
                {
                    msg.MessageContent = "Data not found!";
                    return msg;
                }

                data.CourseId = info.CourseId; // Update string version of enum
                data.Title = info.Title;
                data.Description = info.Description;
               // data.Role = info.Role;
                data.VideoLink = info.VideoLink;
                data.Department = info.Department;
                //data.Name = info.Name;

                data.IsActive = info.IsActive;
                data.UpdatedUser = info.UpdatedUser;
                data.UpdatedDate = GetLocalStdDT();

                await _context.SaveChangesAsync();

                msg.Status = true;
                msg.MessageContent = "Successfully updated!";
            }
            catch (Exception ex)
            {
                msg.MessageContent = ex.InnerException?.Message ?? ex.Message;
            }

            return msg;
        }


        public async Task<ResponseMessage> DeleteCourse(string id)
        {
            ResponseMessage msg = new ResponseMessage { Status = false };
            try
            {
                Courses data = await _context.Courses
                    .FromSqlRaw("SELECT * FROM [Courses] WHERE CourseId = @id", new SqlParameter("@id", id))
                    .SingleOrDefaultAsync();

                if (data == null)
                {
                    msg.MessageContent = "Data not found.";
                    return msg;
                }

                _context.Courses.Remove(data);
                await _context.SaveChangesAsync();

                msg.Status = true;
                msg.MessageContent = "Successfully deleted!";
            }
            catch (Exception ex)
            {
                msg.MessageContent = ex.InnerException?.Message ?? ex.Message;
            }
            return msg;
        }

        //public async Task<List<CourseDto>> GetAllCoursesAsync()
        //{
        //    var courses = await _context.Courses
        //        .Where(c => c.IsActive == true)
        //        .OrderByDescending(c => c.CreatedDate)
        //        .ToListAsync();

        //    return _mapper.Map<List<CourseDto>>(courses);
        //}

        public async Task<List<Courses>> GetAllCoursesAsync()
        {
            return await _context.Courses.ToListAsync();
        }

        //public async Task<CourseDto?> GetCourseByIdAsync(int id)
        //{
        //    var course = await _context.Courses.FirstOrDefaultAsync(c => c.Id == id);
        //    return course != null ? _mapper.Map<CourseDto>(course) : null;
        //}

        public async Task<CourseDto?> GetCourseByIdAsync(int courseId)
        {
            var course = await _context.Courses.FirstOrDefaultAsync(c => c.Id == courseId);
            return course != null ? _mapper.Map<CourseDto>(course) : null;
        }


        #endregion
    }
}
