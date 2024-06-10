using Helperland.Entity.DataContext;
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
        private readonly EmailConfig _emailConfig;

        public Login(HelperlandContext context, EmailConfig emailConfig)
        {
            _context = context;
            _emailConfig = emailConfig;
        }

        #region login
        public UserDataModel login(LoginModel user)
        {
            var uservar = _context.Users.FirstOrDefault(u => u.Email == user.Email);
            if (uservar == null)
            {
                UserDataModel U = new()
                {
                    IsError = true,
                    ErrorMessage = "User not registered, Please register first!!!!!!"
                };
                return U;
            }
            else
            {
                var password = _context.Users.FirstOrDefault(u => u.Email == user.Email && u.Password == user.Password);
                if (password == null)
                {
                    UserDataModel U = new()
                    {
                        IsError = true,
                        ErrorMessage = "Either Email or Password is incorrect, Please check and try again!!!!!!"
                    };
                    return U;
                }
                else
                {
                    UserDataModel U = new()
                    {
                        FirstName = uservar.FirstName,
                        LastName = uservar.LastName,
                        Email = uservar.Email,
                        RoleId = uservar.RoleId,
                        IsError = false,
                        ErrorMessage = ""
                    };
                    return U;
                }
            }
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

        #region forgotPass
        public ResetPass forgotPass(ResetPass user)
        {
            var uservar = _context.Users.FirstOrDefault(u => u.Email == user.Email);
            if (uservar == null)
            {
                ResetPass U = new()
                {
                    IsError = true,
                    ErrorMessage = "User not registered, Please register first!!!!!!"
                };
                return U;
            }
            else
            {
                Guid g = Guid.NewGuid();
                uservar.ResetKey = g.ToString();
                _context.Users.Update(uservar);
                _context.SaveChanges();
                var Subject = "Change PassWord";
                var agreementUrl = "http://localhost:4200/resetpassword?t=" + _emailConfig.Encode(g.ToString()) + "&e=" + _emailConfig.Encode(uservar.Email);
                var emailbody = $"<a href='{agreementUrl}'>Link to reset the password</a>";
                _emailConfig.SendMail(uservar.Email, Subject, emailbody);
                ResetPass U = new()
                {
                    Email = uservar.Email,
                    IsError = false
                };
                return U;
            }
        }
        #endregion

        #region resetPassLink
        public ResetPass resetPassLink(ResetPass user)
        {
            var email = _emailConfig.Decode(user.Email);
            var token = _emailConfig.Decode(user.ResetKey);

            var uservar = _context.Users.FirstOrDefault(u => u.Email == email && u.ResetKey == token);
            if (uservar == null)
            {
                ResetPass U = new()
                {
                    IsError = true,
                    ErrorMessage = "Link is either expired or invaild!!!!!!"
                };
                return U;
            }
            else
            {
                ResetPass U = new()
                {
                    Email = uservar.Email,
                    IsError = false
                };
                return U;
            }
        }
        #endregion

        #region resetPass
        public ResetPass resetPass(ResetPass user)
        {
            var email = _emailConfig.Decode(user.Email);

            var uservar = _context.Users.FirstOrDefault(u => u.Email == email);
            uservar.Password = user.Password;
            uservar.ResetKey = null;
            _context.Update(uservar);
            _context.SaveChanges();
            ResetPass U = new()
            {
                Email = uservar.Email,
                IsError = false
            };
            return U;
        }
        #endregion
    }
}
