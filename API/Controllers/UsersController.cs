using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Data;
using API.Entities;
using Microsoft.AspNetCore.Authorization;
using API.DTOs;
using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using API.Interfaces;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using API.Extensions;

namespace API.Controllers
{
    [Authorize]
    public class UsersController : BaseApiController
    {
        private readonly IMapper _mapper;
       
        public IUserRepository _userRepository ;
        public IPhotosService _photoService ;
        public UsersController(IUserRepository userRepository,IMapper mapper,IPhotosService photoService)
        {
            _mapper = mapper;
            _userRepository = userRepository;
            _photoService=photoService;
            
        }
     
        [HttpGet]
        public async Task <ActionResult<IEnumerable<MemberDTO>>>GetUsers()
        {
            var users= await _userRepository.GetMembersAsync();
            var usersToReturn=_mapper.Map<IEnumerable<MemberDTO>>(users);
            return Ok(usersToReturn);
        }
       
         [HttpGet("{username}",Name ="GetUser")]
         public async Task< ActionResult<MemberDTO>>GetUser(string username)
        {
            return await _userRepository.GetMemberAsync(username);
            //return _mapper.Map<MemberDTO>(users);
        }
        [HttpPut]
        public async Task<ActionResult>UpdateUser(MemberUpdateDTO memberUpdateDTO)
        {
           
            var user=await _userRepository.GetUserByUsernameAsync(User.getUserName());
            _mapper.Map(memberUpdateDTO,user);
            _userRepository.update(user);
            if(await _userRepository.SaveAllAsync()) return NoContent();
            return BadRequest ("Failed to update user");

        }
        [HttpPost("add-photo")]
        public async Task<ActionResult<PhotoDTO>> AddPhoto(IFormFile file)
        {
            var user = await _userRepository.GetUserByUsernameAsync(User.getUserName());
            var result = await _photoService.AddPhotoAsync(file);
            if (result.Error != null)
                return BadRequest(result.Error.Message);
            var photo = new Photo
            {
                Url = result.SecureUrl.AbsoluteUri,
                PublicId = result.PublicId

            };
            if (user.Photos.Count == 0)
            {
                photo.IsMain = true;
            }
            user.Photos.Add(photo);
            if (await _userRepository.SaveAllAsync())
            {
                return CreatedAtRoute("GetUser",new{username=user.UserName},_mapper.Map<PhotoDTO>(photo));
                
            }
            return BadRequest("Problem adding photo");
        }
        [HttpPut("set-main-photo/{photoId}")]
        public async Task<ActionResult>SetMainPhoto(int photoId)
        {
            var user=await _userRepository.GetUserByUsernameAsync(User.getUserName()); 
            var photo=user.Photos.FirstOrDefault(x=>x.Id==photoId);
            if(photo.IsMain) return BadRequest("This is already your main photo");
            var currentMain=user.Photos.FirstOrDefault(x=>x.IsMain);
            if(currentMain!=null)currentMain.IsMain=true;
            photo.IsMain=true;
            if(await _userRepository.SaveAllAsync()) return NoContent();
            return BadRequest("Failed to set main photo");
        }

[HttpDelete("delete-photo/{photoId}")]
public async Task<ActionResult>DeletePhoto(int photoId)
{
    var user=await _userRepository.GetUserByUsernameAsync(User.getUserName());
    var photo=user.Photos.FirstOrDefault(x=>x.Id==photoId);
    if(photo==null)
    return NotFound();
    if(photo.IsMain)
    return BadRequest("you cannot delete your main photo");
    if(photo.PublicId!=null)
    {
       var result= await _photoService.DeletePhotoAsyn(photo.PublicId);
       if(result.Error!=null) return BadRequest(result.Error.Message);
    }
    user.Photos.Remove(photo);
    if(await _userRepository.SaveAllAsync()) return Ok();
}
    }
}