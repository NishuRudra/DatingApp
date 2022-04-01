using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Data;
using API.Entities;
using Microsoft.AspNetCore.Mvc;
using  System.Security.Cryptography;
using System.Text;
using API.DTOs;
using API.Interfaces;
using System.Data;
using Microsoft.EntityFrameworkCore;
using AutoMapper;

namespace API.Controllers
{
    public class AccountController:BaseApiController
    {
        private readonly DataContext _context;
     
        private readonly ITokenService _tokenService;
        private readonly IMapper _mapper;
        public AccountController(DataContext context,ITokenService tokenService,IMapper mapper)
        {
          _tokenService = tokenService;
           _context = context;
           _mapper=mapper;

        }
        [HttpPost("register")]
        public async Task<ActionResult<UserDTO>>Register(RegisterDTO registerDto)
        {
           if(UserExists(registerDto.Username))return BadRequest("Username is taken");
           var user=_mapper.Map<AppUser>(registerDto);
            using var hmac=new HMACSHA512();
          
                user.UserName=registerDto.Username;
                 user.PasswordHash=hmac.ComputeHash(Encoding.UTF8.GetBytes(registerDto.Password));
                 user.PasswordSalt=hmac.Key;
           
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            return new UserDTO{
                Username=user.UserName,
                Token=_tokenService.CreateToken(user),
                knownAs=user.KnownAs,
                Gender=user.Gender
                
            };
        }
        private  bool UserExists(string Username)
        {
           var Userdata=  (from data in _context.Users
                            where data.UserName == Username
                            select data).FirstOrDefault();
                                if(Userdata==null)
                                return false;
                                else
                                return true;
                                 
    }
        [HttpPost("login")]
    public ActionResult<UserDTO>Login(LoginDTO loginDTO)
    {
        var user= _context.Users.Include(p=>p.Photos).
        SingleOrDefault(x => x.UserName==loginDTO.Username);
        if(user==null)
        return Unauthorized("Invalid username");
        using var hmac=new HMACSHA512(user.PasswordSalt);
        var computeHash=hmac.ComputeHash(Encoding.UTF8.GetBytes(loginDTO.Password));
        for(int i=0;i<computeHash.Length;i++)
        {
            if(computeHash[i]!=user.PasswordHash[i])
            return Unauthorized("Invalid password");
        }
        return new UserDTO{
                Username=user.UserName,
                Token=_tokenService.CreateToken(user),
                PhotoUrl=user.Photos.FirstOrDefault(x => x.IsMain)?.Url,
                knownAs=user.KnownAs,
                Gender=user.Gender
            };
    }
}
}