using System.Threading.Tasks;
using API.Entities;
using System.Collections.Generic;
using API.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using API.DTOs;
using API.Helpers;
using System;

namespace API.Data
{
    public class UserRepository:IUserRepository
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;
        public UserRepository(DataContext context,IMapper mapper)
        {
            _mapper = mapper;
            _context = context;

        }
    public async  Task<AppUser> GetUserByIdAsync(int id)
       {
          return await _context.Users.FindAsync(id);
       }
     public async Task<AppUser> GetUserByUsernameAsync(string username)
     {
         return await _context.Users.Include(p=>p.Photos).SingleOrDefaultAsync(x=>x.UserName==username);
     }
    public async  Task<IEnumerable<AppUser>> GetUsersAsync()
      {
            return await _context.Users.Include(p=>p.Photos).ToListAsync();
      }
      public void update (AppUser user)
      {
          _context.Entry(user).State=EntityState.Modified;
      }
      public async Task<bool> SaveAllAsync()
      {
          return await _context.SaveChangesAsync()>0;
      }
     public async Task<PagedList<MemberDTO>> GetMembersAsync(UserParams userParams)
     {
            var query=_context.Users.AsQueryable();
            query=query.Where(u=>u.UserName!=userParams.CurrentUsername);
            query=query.Where(u=>u.Gender!=userParams.Gender);
            var minDob=DateTime.Today.AddYears(-userParams.MaxAge-1);
            var maxDob=DateTime.Today.AddYears(-userParams.MinAge);
            query=query.Where(u=>u.DateOfBirth>=minDob && u.DateOfBirth<=maxDob); 
            query=userParams.OrderBy switch
            {
               "created"=>query.OrderByDescending(u=>u.Created),
               _=>query.OrderByDescending(u=>u.LastActive)
            };
           return await PagedList<MemberDTO>.CreateAsync(query.ProjectTo<MemberDTO>(_mapper.ConfigurationProvider).AsNoTracking(),userParams.PageNumber,userParams.PageSize);
     }
    public async  Task<MemberDTO> GetMemberAsync(string username)
       {
           return await _context.Users.Include(p=>p.Photos)
           .Where(x=>x.UserName==username)
           .ProjectTo<MemberDTO>(_mapper.ConfigurationProvider)
           .SingleOrDefaultAsync();
       }

    }
}