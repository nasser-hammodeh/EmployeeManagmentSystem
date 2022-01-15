using EmployeeAPI.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeeAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserProfileController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        public UserProfileController(ApplicationDbContext _context)
        {
            this._context = _context;
        }
        [HttpGet]
        [Authorize]
        public async Task<Object> GetUserProfile()
        {
            var user = await _context.Users.Where(x => x.UserName == "Admin").FirstOrDefaultAsync();
            if (user != null)
            {
                return new
                {
                    user.Id,
                    user.UserName
                };
            }
            else
            {
                return NotFound();
            }
        }
    }
}
