﻿using Helperland.Entity.DataContext;
using Helperland.Entity.DataModels;
using Helperland.Entity.Model;
using Helperland.Repository.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Helperland.Repository
{
    public class Login : Ilogin
    {
        private readonly HelperlandContext _context;
        public Login(HelperlandContext context)
        {
            _context = context;
        }

        #region login
        public UserDataModel login(LoginModel user)
        {
            var uservar = _context.Users.FirstOrDefault(u => u.Email == user.Email && u.Password == user.Password);
            if (uservar != null)
            {
                UserDataModel U = new()
                {
                    FirstName = uservar.FirstName,
                    LastName = uservar.LastName,
                    Email = uservar.Email,
                    RoleId = uservar.RoleId
                };
                return U;
            }
            return new UserDataModel();
        }
        #endregion

        #region signup
        public UserDataModel signup(UserModel user)
        {
            var uservar = _context.Users.FirstOrDefault(u => u.Email == user.Email);
            if (uservar != null) 
            {
                return new UserDataModel();
            }

            User u = new()
            {
                FirstName = user.Firstname,
                LastName = user.Lastname,
                Email = user.Email,
                Password = user.Password,
                Mobile = user.Contact,
                RoleId = user.Roleid,
                UserTypeId = 1,
                IsRegisteredUser = true,
                WorksWithPets = false,
                CreatedDate = DateTime.Now,
                ModifiedDate = DateTime.Now,
                IsActive = true,
                IsApproved = true,
                IsDeleted = false,
                IsOnline = true
            };
            _context.Users.Add(u);
            _context.SaveChanges();
            UserDataModel U = new()
            {
                FirstName = user.Firstname,
                LastName = user.Lastname,
                Email = user.Email,
                RoleId = user.Roleid
            };
            return U;
        }
        #endregion
    }
}
