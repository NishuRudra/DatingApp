using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;
using System.Collections.Generic;
using API.Entities;
using  System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Identity;

namespace API.Data
{
    public class seed
    {
        public static async Task SeedUsers(UserManager<AppUser> userManager,
        RoleManager<AppRole>roleManager)
        {
            // if(await context.Users.AnyAsync()) return;
            if (await userManager.Users.AnyAsync()) return;
            var UserData=await System.IO.File.ReadAllTextAsync("Data/UserSeedData.json");
            var users=JsonSerializer.Deserialize<List<AppUser>>(UserData);
            if(users==null) return;
            var roles=new List<AppRole>
            {
                    new AppRole{Name="Member"},
                    new AppRole{Name="Admin"},
                    new AppRole{Name="Moderate"}
            };
            foreach(var role in roles)
            {
                await roleManager.CreateAsync(role);
            }
            foreach (var user in users)
            {
                // using var hmac=new HMACSHA512();
                user.UserName=user.UserName.ToLower();
                // user.PasswordHash=hmac.ComputeHash(Encoding.UTF8.GetBytes("Pa$$w0rd"));
                // user.PasswordSalt=hmac.Key;
                //context.Users.Add(user);
                await userManager.CreateAsync(user,"Pa$$w0rd");
                await userManager.AddToRoleAsync(user,"Member");
            }
            var admin=new AppUser{
                UserName="admin"
            };
            await userManager.CreateAsync(admin,"Pa$$w0rd");
            await userManager.AddToRolesAsync(admin,new[]{"Admin","Moderate"});
           // await context.SaveChangesAsync();
        }
    }
}