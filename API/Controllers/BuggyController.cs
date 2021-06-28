using Microsoft.AspNetCore.Mvc;
using API.Data;
using API.Entities;
using Microsoft.AspNetCore.Authorization;

using Microsoft.AspNetCore.Hosting;
namespace API.Controllers
{
    public class BuggyController:BaseApiController
    {
        public DataContext _context { get; }
         public BuggyController(DataContext context)
         {
            _context = context;

         }
         [Authorize]
         [HttpGet("auth")]
         public ActionResult<string>GetSecret()
         {
             return "secret text";
         }
          [HttpGet("not-found")]
         public ActionResult<AppUser>GetNotFound()
         {
             var thing=_context.Users.Find(-1);
             if (thing ==null)
             return NotFound();
             else
             {
                 return Ok(thing);
             }
         }
          [HttpGet("server-error")]
        //  public ActionResult<AppUser>GetServerError()
        //  {
            
        //           var thing=_context.Users.Find(-1);
        //     var thingToReturn=thing.ToString();
        //     return thingToReturn;

            
             
        //  }
          [HttpGet("bad-request")]
         public ActionResult<string>GetBadRequest()
         {
             return BadRequest("This is bad request");
         }
         
    }
}