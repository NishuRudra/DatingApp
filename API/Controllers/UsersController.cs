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

namespace API.Controllers
{
    [Authorize]
    public class UsersController : BaseApiController
    {
        private readonly IMapper _mapper;
       
        public IUserRepository _userRepository ;
        public UsersController(IUserRepository userRepository,IMapper mapper)
        {
            _mapper = mapper;
            _userRepository = userRepository;
            
        }
     
        [HttpGet]
        public async Task <ActionResult<IEnumerable<MemberDTO>>>GetUsers()
        {
            var users= await _userRepository.GetMembersAsync();
            var usersToReturn=_mapper.Map<IEnumerable<MemberDTO>>(users);
            return Ok(usersToReturn);
        }
       
         [HttpGet("{username}")]
         public async Task< ActionResult<MemberDTO>>GetUser(string username)
        {
            return await _userRepository.GetMemberAsync(username);
            //return _mapper.Map<MemberDTO>(users);
        }
        [HttpPut]
        public async Task<ActionResult>UpdateUser(MemberUpdateDTO memberUpdateDTO)
        {
            var username=User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var user=await _userRepository.GetUserByUsernameAsync(username);
            _mapper.Map(memberUpdateDTO,user);
            _userRepository.update(user);
            if(await _userRepository.SaveAllAsync()) return NoContent();
            return BadRequest ("Fialeed to update user");

        }

      
    }
}