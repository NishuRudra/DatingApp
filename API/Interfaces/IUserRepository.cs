using API.Entities;
using System.Threading.Tasks;
using System.Collections.Generic;
using  API.DTOs;
namespace API.Interfaces
{
    public interface IUserRepository
    {
        void update (AppUser user);

       Task <bool> SaveAllAsync();

       Task <IEnumerable<AppUser>> GetUsersAsync();

       Task <AppUser> GetUserByIdAsync (int id);

       Task <AppUser> GetUserByUsernameAsync(string username);

       Task <IEnumerable<MemberDTO>> GetMembersAsync();
       
        Task <MemberDTO> GetMemberAsync(string username);
        
    } 
}