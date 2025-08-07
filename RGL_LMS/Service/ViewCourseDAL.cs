//namespace RGL_LMS.Service
//{
//    public class ViewCourseService
//    {

//    }
//}

using RGL_LMS.DTO;
using RGL_LMS.Models;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System.Data;
using RGL_LMS.Dto;
using RGL_LMS.Service;
using Microsoft.Extensions.Logging;

public class ViewCourseDAL
{
    private readonly RGL_LMS_DBContext _context;
    private readonly IMapper _mapper;
    private readonly IConfiguration _configuration;
    private readonly IWebHostEnvironment _webHostEnvironment;
    private readonly ILogger<UserDAl> _logger;

    public ViewCourseDAL(IConfiguration config, RGL_LMS_DBContext context, IWebHostEnvironment webHostEnvironment, IMapper mapper, ILogger<UserDAl> logger)
    {
        _context = context;
        _configuration = config;
        _webHostEnvironment = webHostEnvironment;
        _mapper = mapper;
        _logger = logger;
    }
 
    public async Task<CourseDto> GetCourseById(string courseId)
    {
        var course = await _context.Courses
            .Where(x => x.CourseId == courseId)
            .Select(x => new CourseDto
            {
                CourseId = x.CourseId,
                VideoLink=x.VideoLink,
                Title = x.Title,
                Description = x.Description,
                //InstructorId = x.InstructorId,
                // etc...
            })
            .FirstOrDefaultAsync();

        return course;
    }
    
    public async Task<UserDto> GetUserById(string id)
    {
        UserDto user = null;

        try
        {
            Users data = await _context.User
                .FromSqlRaw("SELECT * FROM [User] WHERE UserId = @id", new SqlParameter("@id", id))
                .SingleOrDefaultAsync();

            if (data != null)
            {
                user = new UserDto
                {
                    UserId = data.UserId,
                    //Role = data.Role,
                    FullName = data.FullName,
                    EmployeeId = data.EmployeeId,
                    Department = data.Department,
                    PhoneNumber = data.PhoneNumber
                };
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error in GetUserById");
        }

        return user;
    }

    public async Task<List<ViewCourse>> GetAllAsync()
    {
        return await _context.ViewCourses.ToListAsync();
    }

    public async Task<ViewCourse?> GetByIdAsync(string id)
    {
        return await _context.ViewCourses.FirstOrDefaultAsync(x => x.ViewCourseId == id);
    }

    public async Task<ResponseMessage> CreateAsync(ViewCourseDto dto)
    {
        var res = new ResponseMessage();

        try
        {
            if (dto.Status?.ToLower() != "finished")
            {
                res.Status = false;
                res.MessageContent = "Only 'finished' status will be counted.";
                return res;
            }

            var existing = await _context.ViewCourses
                .FirstOrDefaultAsync(x => x.CourseId == dto.CourseId && x.UserId == dto.UserId);

            if (existing != null)
            {
                existing.ViewCount = (existing.ViewCount ?? 0) + 1;
                existing.Status = dto.Status;
                _context.ViewCourses.Update(existing);
            }
            else
            {
                var view = new ViewCourse
                {
                    ViewCourseId = GenerateId(),
                    CourseId = dto.CourseId,
                    UserId = dto.UserId,
                    ViewCount = 1,
                    Status = dto.Status,
                    Name = dto.Name,
                    EmployeeId = dto.EmployeeId,
                    PhoneNumber = dto.PhoneNumber,
                    Email = dto.Email,
                    Department = dto.Department,
                    Title = dto.Title,
                    Description = dto.Description,
                    VideoLink = dto.VideoLink,
                    CreatedUser = dto.CreatedUser,
                    CreatedDate = DateTime.Now
                };

                await _context.ViewCourses.AddAsync(view);
            }

            await _context.SaveChangesAsync();
            res.Status = true;
            res.MessageContent = "Saved successfully.";
        }
        catch (Exception ex)
        {
            res.Status = false;
            res.MessageContent = ex.Message;
        }

        return res;
    }

    public async Task<ResponseMessage> UpdateAsync(ViewCourseDto dto)
    {
        var res = new ResponseMessage();

        try
        {
            var existing = await _context.ViewCourses
                .FirstOrDefaultAsync(x => x.ViewCourseId == dto.ViewId);

            if (existing == null)
            {
                res.Status = false;
                res.MessageContent = "Record not found.";
                return res;
            }

            existing.CourseId = dto.CourseId;
            existing.UserId = dto.UserId;
            existing.Status = dto.Status;
            existing.Name = dto.Name;
            existing.EmployeeId = dto.EmployeeId;
            existing.PhoneNumber = dto.PhoneNumber;
            existing.Email = dto.Email;
            existing.Department = dto.Department;
            existing.Title = dto.Title;
            existing.Description = dto.Description;
            existing.VideoLink = dto.VideoLink;
            existing.CreatedUser = dto.CreatedUser;

            _context.ViewCourses.Update(existing);
            await _context.SaveChangesAsync();

            res.Status = true;
            res.MessageContent = "Updated successfully.";
        }
        catch (Exception ex)
        {
            res.Status = false;
            res.MessageContent = ex.Message;
        }

        return res;
    }

    public async Task<ResponseMessage> DeleteAsync(string id)
    {
        var res = new ResponseMessage();

        try
        {
            var existing = await _context.ViewCourses.FirstOrDefaultAsync(x => x.ViewCourseId == id);
            if (existing == null)
            {
                res.Status = false;
                res.MessageContent = "Record not found.";
                return res;
            }

            _context.ViewCourses.Remove(existing);
            await _context.SaveChangesAsync();

            res.Status = true;
            res.MessageContent = "Deleted successfully.";
        }
        catch (Exception ex)
        {
            res.Status = false;
            res.MessageContent = ex.Message;
        }

        return res;
    }

    private string GenerateId()
    {
        var last = _context.ViewCourses.OrderByDescending(x => x.ViewCourseId).FirstOrDefault();
        int num = 0;

        if (last != null && last.ViewCourseId.Length > 2)
            int.TryParse(last.ViewCourseId.Substring(2), out num);

        return "VC" + (num + 1).ToString("D4");
    }
}
