using Helperland.Entity.DataContext;
using Helperland.Entity.DataModels;
using Helperland.Entity.Model;
using System.Collections;

namespace Helperland.Repository.Interface
{
    public class UserService : IUserService
    {
        private readonly HelperlandContext _context;
        private readonly EmailConfig _emailConfig;

        public UserService(HelperlandContext context, EmailConfig emailConfig)
        {
            _context = context;
            _emailConfig = emailConfig;
        }

        #region Login
        public UserDataModel Login(LoginModel user)
        {
            try
            {
                var userVar = _context.Users.FirstOrDefault(u => u.Email == user.Email);
                if (userVar == null)
                {
                    UserDataModel userData = new()
                    {
                        IsError = true,
                        ErrorMessage = "User not registered, Please register first!!!!!!"
                    };
                    return userData;
                }
                else
                {
                    if (userVar.Password != user.Password)
                    {
                        UserDataModel userData = new()
                        {
                            IsError = true,
                            ErrorMessage = "Either Email or Password is incorrect, Please check and try again!!!!!!"
                        };
                        return userData;
                    }
                    else
                    {
                        UserDataModel userData = new()
                        {
                            FirstName = userVar.FirstName,
                            LastName = userVar.LastName,
                            Email = userVar.Email,
                            RoleId = userVar.RoleId,
                            IsError = false,
                            ErrorMessage = ""
                        };
                        return userData;
                    }
                }
            }
            catch
            {
                return new UserDataModel();
            }
        }
        #endregion

        #region Signup
        public UserDataModel Signup(UserModel user)
        {
            try
            {
                var userVar = _context.Users.FirstOrDefault(u => u.Email == user.Email);
                if (userVar != null)
                {
                    return new UserDataModel();
                }

                User userData = new()
                {
                    FirstName = user.Firstname,
                    LastName = user.Lastname,
                    Email = user.Email,
                    Password = user.Password,
                    Mobile = user.Contact,
                    RoleId = user.Roleid,
                    UserTypeId = 1,
                    IsRegisteredUser = new BitArray(1) { [0] = true },
                    WorksWithPets = new BitArray(1),
                    CreatedDate = DateTime.Now,
                    ModifiedDate = DateTime.Now,
                    IsActive = new BitArray(1) { [0] = true },
                    IsApproved = new BitArray(1) { [0] = true },
                    IsDeleted = new BitArray(1),
                    IsOnline = new BitArray(1) { [0] = true }
                };
                _context.Users.Add(userData);
                _context.SaveChanges();
                UserDataModel userDataModel = new()
                {
                    FirstName = user.Firstname,
                    LastName = user.Lastname,
                    Email = user.Email,
                    RoleId = user.Roleid
                };
                return userDataModel;
            }
            catch
            {
                return new UserDataModel();
            }
        }
        #endregion

        #region ForgotPass
        public ResetPass ForgotPass(ResetPass user)
        {
            try
            {
                var userVar = _context.Users.FirstOrDefault(u => u.Email == user.Email);
                if (userVar == null)
                {
                    ResetPass passwordObject = new()
                    {
                        IsError = true,
                        ErrorMessage = "User not registered, Please register first!!!!!!"
                    };
                    return passwordObject;
                }
                else
                {
                    Guid g = Guid.NewGuid();
                    userVar.ResetKey = g.ToString();
                    _context.Users.Update(userVar);
                    _context.SaveChanges();
                    var subject = "Change PassWord";
                    var agreementUrl = "http://localhost:4200/resetpassword?t=" + _emailConfig.Encode(g.ToString()) + "&e=" + _emailConfig.Encode(userVar.Email);
                    var emailBody = $"<a href='{agreementUrl}'>Link to reset the password</a>";
                    _emailConfig.SendMail(userVar.Email, subject, emailBody);
                    ResetPass passwordObject = new()
                    {
                        Email = userVar.Email,
                        IsError = false
                    };
                    return passwordObject;
                }
            }
            catch
            {
                return new ResetPass();
            }
        }
        #endregion

        #region ResetPassLink
        public ResetPass ResetPassLink(ResetPass user)
        {
            try
            {
                var email = _emailConfig.Decode(user.Email);
                var token = _emailConfig.Decode(user.ResetKey);

                var userVar = _context.Users.FirstOrDefault(u => u.Email == email && u.ResetKey == token);
                if (userVar == null)
                {
                    ResetPass passwordObject = new()
                    {
                        IsError = true,
                        ErrorMessage = "Link is either expired or invaild!!!!!!"
                    };
                    return passwordObject;
                }
                else
                {
                    ResetPass passwordObject = new()
                    {
                        Email = userVar.Email,
                        IsError = false
                    };
                    return passwordObject;
                }
            }
            catch {
                return new ResetPass();
            }
        }
        #endregion

        #region ResetPass
        public ResetPass ResetPass(ResetPass user)
        {
            try
            {
                var email = _emailConfig.Decode(user.Email);

                var userVar = _context.Users.FirstOrDefault(u => u.Email == email);
                userVar.Password = user.Password;
                userVar.ResetKey = null;
                _context.Update(userVar);
                _context.SaveChanges();
                ResetPass passwordObject = new()
                {
                    Email = userVar.Email,
                    IsError = false
                };
                return passwordObject;
            }
            catch {
                return new ResetPass();
            }
        }
        #endregion

        #region GetUsers
        public List<UserDataModel> GetUsers()
        {
            try
            {
                List<UserDataModel> user = (from u in _context.Users
                                            select new UserDataModel
                                            {
                                                FirstName = u.FirstName,
                                                LastName = u.LastName,
                                                Email = u.Email,
                                                RoleId = u.RoleId,
                                                ResetKey = u.ResetKey
                                            }).ToList();
                return user;
            }
            catch
            {
                return new List<UserDataModel>();
            }
        }
        #endregion
    }
}
