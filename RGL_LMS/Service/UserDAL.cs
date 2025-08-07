using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using RGL_LMS.Dto;
using RGL_LMS.Models;
using System.Data;

namespace RGL_LMS.Service
{
    public class UserDAl
    {
        private readonly RGL_LMS_DBContext _context;
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly ILogger<UserDAl> _logger;
        string _conStr;
        public UserDAl(IConfiguration config, RGL_LMS_DBContext context, IMapper mapper, IWebHostEnvironment webHostEnvironment, ILogger<UserDAl> logger)
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


        public List<Dictionary<string, object>> ConvertDataTableToDictionary(DataTable dt)
        {
            var list = new List<Dictionary<string, object>>();

            foreach (DataRow row in dt.Rows)
            {
                var dict = new Dictionary<string, object>();
                foreach (DataColumn col in dt.Columns)
                {
                    dict[col.ColumnName] = row[col];
                }
                list.Add(dict);
            }

            return list;
        }



        #region Register User May-26-2025

        //public async Task<DataTable> GetUserList()
        //{
        //    string sql = @"SELECT * from [User]";  // <-- Wrap User in brackets
        //    DataTable dt = await GetDataTableAsync(sql);
        //    return dt;
        //}

        public async Task<List<Dictionary<string, object>>> GetUserList()
        {
            string sql = @"SELECT * FROM [User]";
            DataTable dt = await GetDataTableAsync(sql);
            return ConvertDataTableToDictionary(dt);
        }


        public async Task<UserDto> GetUserById(string id)
        {
            UserDto user = new UserDto();
            try
            {
                Users data = await _context.User
                    .FromSqlRaw("SELECT * FROM [User] WHERE UserId = @id", new SqlParameter("@id", id))
                    .SingleOrDefaultAsync();

                if (data != null)
                {
                    user = _mapper.Map<UserDto>(data);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in GetUserById");
            }

            return user;
        }

       

        public async Task<ResponseMessage> SaveUser([FromForm] UserDto info)
        {
            ResponseMessage msg = new() { Status = false };

            try
            {
                // ✅ Role validation
                if (!Enum.IsDefined(typeof(UserRole), info.Role))
                {
                    _logger.LogWarning($"Invalid role received: {info.Role}");
                    msg.MessageContent = "Invalid role specified.";
                    return msg;
                }

                // ✅ Set prefix based on role
                string prefix = info.Role switch
                {
                    UserRole.Admin => "AD",
                    UserRole.Instructor => "IST",
                    UserRole.Learner => "L",
                    _ => throw new InvalidOperationException("Unsupported role.")
                };

                // ✅ Get role-wise existing users count and generate next ID
                int nextRoleCount = await _context.User
                    .Where(u => u.Role == info.Role.ToString())
                    .CountAsync() + 1;

                info.UserId = $"{prefix}-{nextRoleCount:0000}";

                // ✅ Ensure uniqueness
                if (await _context.User.AnyAsync(u => u.UserId == info.UserId))
                {
                    // Keep checking until we get a unique ID
                    while (await _context.User.AnyAsync(u => u.UserId == info.UserId))
                    {
                        nextRoleCount++;
                        info.UserId = $"{prefix}-{nextRoleCount:0000}";
                    }
                }

                // ✅ Global SrNo for display or tracking
                var globalSrNo = await _context.User.MaxAsync(u => (int?)u.SrNo) ?? 0;
                info.SrNo = globalSrNo + 1;

                // ✅ Map and assign
                var user = _mapper.Map<Users>(info);
                user.Role = info.Role.ToString();
                user.CreatedDate = GetLocalStdDT();
                user.IsActive = true;

                _context.User.Add(user);
                await _context.SaveChangesAsync();

                msg.Status = true;
                msg.MessageContent = $"User created successfully with ID: {info.UserId}";
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, "Database update error in SaveUser");
                msg.MessageContent = ex.InnerException?.Message?.Contains("PRIMARY KEY") == true
                    ? "Duplicate UserId. Please try again."
                    : ex.InnerException?.Message ?? ex.Message;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error in SaveUser");
                msg.MessageContent = ex.Message;
            }

            return msg;
        }




        //public async Task<ResponseMessage> UpdateUser([FromForm] UserDto info)
        //{
        //    ResponseMessage msg = new ResponseMessage { Status = false };
        //    try
        //    {
        //        Users data = await _context.User
        //            .FromSqlRaw("SELECT * FROM [User] WHERE UserId = @id", new SqlParameter("@id", info.UserId))
        //            .SingleOrDefaultAsync();

        //        if (data == null)
        //        {
        //            msg.MessageContent = "Data not found!";
        //            return msg;
        //        }

        //        data.IsActive = info.IsActive;
        //        data.FullName = info.FullName;
        //        data.Email = info.Email;
        //        data.PasswordHash = info.PasswordHash;
        //        data.Role = info.Role;
        //        data.NRC = info.NRC;
        //        data.PhoneNumber = info.PhoneNumber;
        //        data.Department = info.Department;
        //        data.EmployeeId = info.EmployeeId;
        //        data.Position = info.Position;
        //        data.Remark = info.Remark;
        //        data.UpdatedDate = GetLocalStdDT();
        //        data.UpdatedUser = info.UpdatedUser;

        //        await _context.SaveChangesAsync();

        //        msg.Status = true;
        //        msg.MessageContent = "Successfully updated!";
        //    }
        //    catch (DbUpdateException e)
        //    {
        //        _logger.LogError(e, "Error in UpdateUser");
        //        msg.MessageContent = e.Message;
        //    }
        //    return msg;
        //}
        public async Task<ResponseMessage> UpdateUser([FromForm] UserDto info)
        {
            ResponseMessage msg = new() { Status = false };
            try
            {
                var data = await _context.User
                    .FirstOrDefaultAsync(u => u.UserId == info.UserId);

                if (data == null)
                {
                    msg.MessageContent = "Data not found!";
                    return msg;
                }

                data.Role = info.Role.ToString(); // Update string version of enum
                data.FullName = info.FullName;
                data.Email = info.Email;
                data.PasswordHash = info.PasswordHash;
                data.NRC = info.NRC;
                data.PhoneNumber = info.PhoneNumber;
                data.Department = info.Department;
                data.EmployeeId = info.EmployeeId;
                data.Position = info.Position;
                data.Remark = info.Remark;
                data.IsActive = info.IsActive;
                data.UpdatedUser = info.UpdatedUser;
                data.UpdatedDate = GetLocalStdDT();

                await _context.SaveChangesAsync();

                msg.Status = true;
                msg.MessageContent = "Successfully updated!";
            }
            catch (DbUpdateException e)
            {
                _logger.LogError(e, "Error in UpdateUser");
                msg.MessageContent = e.Message;
            }

            return msg;
        }


        public async Task<ResponseMessage> DeleteUser(string id)
        {
            ResponseMessage msg = new ResponseMessage { Status = false };
            try
            {
                Users data = await _context.User
                    .FromSqlRaw("SELECT * FROM [User] WHERE UserId = @id", new SqlParameter("@id", id))
                    .SingleOrDefaultAsync();

                if (data == null)
                {
                    msg.MessageContent = "Data not found.";
                    return msg;
                }

                _context.User.Remove(data);
                await _context.SaveChangesAsync();

                msg.Status = true;
                msg.MessageContent = "Successfully deleted!";
            }
            catch (DbUpdateException e)
            {
                _logger.LogError(e, "Error in DeleteUser");
                msg.MessageContent = e.Message;
            }
            return msg;
        }

        #endregion
    }
}

