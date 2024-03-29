using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    public class AdminController:BaseApiController
    {
      private readonly UserManager<AppUser> _usermanager;
        public AdminController(UserManager<AppUser>userManager)
        {
            _usermanager=userManager;
            
        }

      

        [Authorize(Policy="RequireAdminRole")]
        [HttpGet("users-with-roles")]
        public async Task<ActionResult> GetUserWithRoles()
        {
            var users=await _usermanager.Users
            .Include(r=>r.UserRoles)
            .ThenInclude(r=>r.Role)
            .OrderBy(u=>u.UserName)
            .Select(u=>new
            {
                u.Id,
                UserName=u.UserName,
                Roles=u.UserRoles.Select(r=>r.Role.Name).ToList()
            })
            .ToListAsync();
            return Ok(users);
        }

        [Authorize(Policy="ModeratePhotoRole")]
        [HttpGet("photos-to-moderate")]
        public ActionResult GetPhotosForModeration()
        {
            return Ok("Admins or moderators can see this");
        }
    }
}