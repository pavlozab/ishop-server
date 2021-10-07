using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Dto;
using Entities;
using Microsoft.AspNetCore.Identity;

namespace Services
{
    public class UserService: IUserService
    {
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<Role> _roleManager;
        private readonly IMapper _mapper;

        public UserService(UserManager<User> userManager,RoleManager<Role> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }
        
        public async Task<User> GetUserByEmail(string email)
        {
            return await _userManager.FindByEmailAsync(email);
        }

        public async Task Delete(Guid id)
        {
            var existingUser = await _userManager.FindByIdAsync(id.ToString());
            if (existingUser is null)
                throw new KeyNotFoundException("User hasn't been found");

            await _userManager.DeleteAsync(existingUser);
        }

        public async Task<User> Create(RegistrationDto registrationDto)
        {
            await CheckRoles();
            User user = new()
            {
                Id = Guid.NewGuid(),
                Email = registrationDto.Email,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(registrationDto.Password),
                UserName = registrationDto.Email
            }; 
            var newUser = await _userManager.CreateAsync(user);
            Console.WriteLine(newUser);
            await _userManager.AddToRoleAsync(user, "User");
            
            return user;
        }

        public async Task<UserResultDto> UpdateRoleOfUser(Guid id, Role newRole)
        {
            // var existingUser = await _repository.GetOne(id);
            // if (existingUser is null)
            //     throw new KeyNotFoundException("User hasn't been found");
            //
            // existingUser.Roles = newRole;
            //
            // await _repository.Update(existingUser);
            //
            return new UserResultDto
            {
                Id = id
            };
        }
        
        public async Task<IEnumerable<string>> GetUserRoles(User user)
        {
            return await _userManager.GetRolesAsync(user);
        }

        private async Task CheckRoles()
        {
            if (!await _roleManager.RoleExistsAsync("Admin"))
            {
                await _roleManager.CreateAsync(new Role { Description = "Admin", Name = "Admin" });
            }
            if (!await _roleManager.RoleExistsAsync("User"))
            {
                await _roleManager.CreateAsync(new Role { Description = "User", Name = "User" });
            }
        }
    }
}