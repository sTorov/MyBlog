﻿using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MyBlog.Services.Extensions;
using MyBlog.Services.Services.Interfaces;
using MyBlog.Services.ViewModels.Users.Request;
using MyBlog.Services.ViewModels.Users.Response;
using MyBlog.Data.DBModels.Roles;
using MyBlog.Data.DBModels.Users;
using System.Security.Claims;

namespace MyBlog.Services.Services
{
    public class UserService : IUserService
    {
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<Role> _roleManager;
        private readonly IMapper _mapper;

        public UserService(UserManager<User> userManager, IMapper mapper, RoleManager<Role> roleManager)
        {
            _userManager = userManager;
            _mapper = mapper;
            _roleManager = roleManager;
        }

        public async Task<(IdentityResult, User)> CreateUserAsync(UserRegisterViewModel model)
        {
            var user = _mapper.Map<User>(model);
            
            var defaultRole = await _roleManager.FindByNameAsync("User");
            if (defaultRole != null) 
                user.Roles = new List<Role> { defaultRole };

            var result = await _userManager.CreateAsync(user, model.PasswordReg);

            return (result, user);
        }

        public async Task<IdentityResult> CreateUserAsync(UserCreateViewModel model)
        {
            var user = _mapper.Map<User>(model);
            user.Roles = await GetRoleListFromDictionary(model.AllRoles);

            var result = await _userManager.CreateAsync(user, model.PasswordReg);
            return result;
        }


        public async Task<IdentityResult> UpdateUserAsync(UserEditViewModel model, User user)
        {
            user.Convert(model);
            user.Roles = await GetRoleListFromDictionary(model.AllRoles!);
            
            var result = await _userManager.UpdateAsync(user);
            if(result.Succeeded) 
                return await _userManager.UpdateSecurityStampAsync(user);

            return result;
        }

        private async Task<List<Role>> GetRoleListFromDictionary(Dictionary<string, bool> dict)
        {
            var roles = new List<Role>();
            foreach (var pair in dict)
            {
                if (pair.Value)
                {
                    var role = await _roleManager.FindByNameAsync(pair.Key);
                    if (role != null)
                        roles.Add(role);
                }
            }
            return roles;
        }



        public async Task<IdentityResult> UpdateUserAsync(User user) => await _userManager.UpdateAsync(user);

        public async Task<List<User>> GetAllUsersAsync() => await _userManager.Users.ToListAsync();

        public async Task<User?> GetUserByIdAsync(int id) => 
            await Task.Run(() => _userManager.Users.Include(u => u.Roles).AsEnumerable().FirstOrDefault(u => u.Id == id));

        public async Task<User?> GetUserByEmailAsync(string email) => 
            await _userManager.Users.Include(u => u.Roles).FirstOrDefaultAsync(u => u.Email == email);

        public async Task<bool> DeleteByIdAsync(int id, int? userId, bool fullAccess)
        {
            var user = await _userManager.FindByIdAsync(id.ToString());
            var check = fullAccess
                ? user != null
                : user != null && user.Id == userId;

            if (check) return (await _userManager.DeleteAsync(user!)).Succeeded;
            return false;
        }

        public async Task<UserEditViewModel?> GetUserEditViewModelAsync(int id, int? userId, bool fullAccess)
        {
            var user = await GetUserByIdAsync(id);
            var check = fullAccess
                ? user != null
                : user != null && user.Id == userId;

            if(!check)
                return null;
            return _mapper.Map<UserEditViewModel>(user);
        }

        public async Task<UsersViewModel?> GetUsersViewModelAsync(int? roleId)
        {
            var model = new UsersViewModel();
            
            if (roleId == null)
                model.Users = await _userManager.Users.Include(u => u.Roles).ToListAsync();
            else
            {
                var role = await _roleManager.Roles.Include(r => r.Users).FirstOrDefaultAsync(r => r.Id == roleId);
                if (role == null) return null;

                model.Users = role.Users;
                for(int i = 0; i < model.Users.Count; i++)
                {
                    model.Users[i].Roles = await _roleManager.Roles.Include(r => r.Users)
                        .SelectMany(r => r.Users, (r, u) => new { Role = r, User = u })
                        .Where(o => o.User.Id == model.Users[i].Id).Select(o => o.Role).ToListAsync();
                }
            }

            return model;
        }

        public async Task<List<Claim>> GetClaimsAsync(User user)
        {
            var userId = await _userManager.GetUserIdAsync(user);
            var claims = new List<Claim>();

            foreach (var role in user.Roles)
                claims.Add(new Claim(ClaimsIdentity.DefaultRoleClaimType, role.Name!));
            claims.Add(new Claim("UserID", userId));

            return claims;
        }

        public async Task<UserViewModel?> GetUserViewModelAsync(int id)
        {
            var user = await _userManager.Users.Include(u => u.Roles).FirstOrDefaultAsync(u => u.Id == id);

            if (user == null) return null;
            return _mapper.Map<UserViewModel>(user);
        }
    }
}