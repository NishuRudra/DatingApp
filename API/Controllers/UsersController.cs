using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Data;
using API.Entities;
using Microsoft.AspNetCore.Authorization;

using Microsoft.AspNetCore.Mvc;


namespace API.Controllers
{
   
    public class UsersController : BaseApiController
    {
        public DataContext _context;
        public UsersController(DataContext context)
        {
            _context=context;
        }
        [AllowAnonymous]
        [HttpGet]
        public ActionResult<IEnumerable<AppUser>>Get()
        {
            return _context.Users.ToList();
        }
        [Authorize]
         [HttpGet("{Id}")]
         public async Task< ActionResult<AppUser>>Get(int Id)
        {
            return await _context.Users.FindAsync(Id);
        }
    }
}