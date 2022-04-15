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
using Microsoft.AspNetCore.Identity;

namespace API.Controllers
{
    public class AccountController:BaseApiController
    {
        private readonly DataContext _context;
     
        private readonly ITokenService _tokenService;
        private readonly IMapper _mapper;
        private readonly UserManager<AppUser> _usermanager;
        private readonly SignInManager<AppUser> _signInManager;
        public AccountController(
            UserManager<AppUser> userManager,
            SignInManager<AppUser> signInManager,
            ITokenService tokenService,
            IMapper mapper)
        {
          _tokenService = tokenService;
          _signInManager=signInManager;
          _usermanager=userManager;
           _mapper=mapper;

        }
        [HttpPost("register")]
        public async Task<ActionResult<UserDTO>>Register(RegisterDTO registerDto)
        {
           if(await UserExists(registerDto.Username))return BadRequest("Username is taken");
           var user=_mapper.Map<AppUser>(registerDto);
           // using var hmac=new HMACSHA512();
          
                user.UserName=registerDto.Username.ToLower();
                //  user.PasswordHash=hmac.ComputeHash(Encoding.UTF8.GetBytes(registerDto.Password));
                //  user.PasswordSalt=hmac.Key;
           var result=await _usermanager.CreateAsync(user,registerDto.Password);
           if(!result.Succeeded) return BadRequest(result.Errors);
           var roleResult=await _usermanager.AddToRoleAsync(user,"Member");
           if(!roleResult.Succeeded) return BadRequest(result.Errors);
           return new UserDTO{
               Username=user.UserName,
               Token=await _tokenService.CreateToken(user),
               knownAs=user.KnownAs,
               Gender=user.Gender
           };
            // _context.Users.Add(user);
            // await _context.SaveChangesAsync();
            // return new UserDTO{
            //     Username=user.UserName,
            //     Token=_tokenService.CreateToken(user),
            //     knownAs=user.KnownAs,
            //     Gender=user.Gender
                
            // };
        }
        private async Task< bool> UserExists(string Username)
        {
        //    var Userdata=  (from data in _context.Users
        //                     where data.UserName == Username
        //                     select data).FirstOrDefault();
        //                         if(Userdata==null)
        //                         return false;
        //                         else
        //                         return true;
        return await _usermanager.Users.AnyAsync(x=>x.UserName==Username.ToLower());
                                 
    }
        [HttpPost("login")]
    public async Task<ActionResult<UserDTO>>Login(LoginDTO loginDTO)
    {
        var user= _usermanager.Users.Include(p=>p.Photos).
        SingleOrDefault(x => x.UserName==loginDTO.Username.ToLower());
        if(user==null)
        return Unauthorized("Invalid username");
        // using var hmac=new HMACSHA512(user.PasswordSalt);
        // var computeHash=hmac.ComputeHash(Encoding.UTF8.GetBytes(loginDTO.Password));
        // for(int i=0;i<computeHash.Length;i++)
        // {
        //     if(computeHash[i]!=user.PasswordHash[i])
        //     return Unauthorized("Invalid password");
        // }
        var result=await _signInManager.CheckPasswordSignInAsync(user,loginDTO.Password,false);
        if(!result.Succeeded) return Unauthorized();
        return new UserDTO{
                Username=user.UserName,
                Token=await _tokenService.CreateToken(user),
                PhotoUrl=user.Photos.FirstOrDefault(x => x.IsMain)?.Url,
                knownAs=user.KnownAs,
                Gender=user.Gender
            };
    }
}
}