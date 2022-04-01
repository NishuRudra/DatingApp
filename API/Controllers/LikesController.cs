using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Extensions;
using API.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using API.DTOs;
using API.Helpers;

namespace API.Controllers
{
    [Authorize]
    public class LikesController : BaseApiController
    {
        private readonly ILikesRepository _likesRepository ;
         private readonly IUserRepository _userRepository ;
        public LikesController(IUserRepository userRepository,ILikesRepository likesRepository)
        {
            _likesRepository = likesRepository;
            _userRepository=userRepository;
        }
        [HttpPost("{username}")]
        public async Task<ActionResult>Addlike(string username)
        {
            var SourceUserId=User.getUserId();
            var LikedUser=await _userRepository.GetUserByUsernameAsync(username);
            var SourceUser=await _likesRepository.GetUserWithLikes(SourceUserId);
            if(LikedUser==null)
            return NotFound();
            if(SourceUser.UserName==username) 
            return BadRequest("You cannot like yourself");
            var userLike=await _likesRepository.GetUserLike(SourceUserId,LikedUser.Id);
            if(userLike!=null)
            return BadRequest("You already liked this user");
            userLike=new Entities.UserLike{
                SourceUserId=SourceUserId,
                LikedUserId=LikedUser.Id
                
            };
            SourceUser.LikedUsers.Add(userLike);
            if (await _userRepository.SaveAllAsync()) return Ok();
            return BadRequest("Failed to like user");
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<LikeDto>>>GetUserLikes([FromQuery]LikeParams likesParams)
        {
            likesParams.UserId=User.getUserId();
           var user= await _likesRepository.GetUserLikes(likesParams);
           Response.AddPaginationHeader(user.CurrentPage,user.PageSize,user.TotalCount,user.TotalPages);
           return Ok(user);
        }
    }
}