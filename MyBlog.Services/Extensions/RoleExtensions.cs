﻿using MyBlog.Services.ViewModels.Roles.Response;
using MyBlog.Data.DBModels.Roles;

namespace MyBlog.Services.Extensions
{
    public static class RoleExtensions
    {
        public static Role Convert(this Role role, RoleEditViewModel model)
        {
            role.Name = model.Name;
            role.Description = model.Description;

            return role;
        }
    }
}