using Helperland.Entity.DataContext;
using Helperland.Entity.DataModels;
using Helperland.Entity.Model;
using System.Collections;
using static System.Runtime.InteropServices.JavaScript.JSType;

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
                        ErrorMessage = "User not registered, Please register first!"
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
                            ErrorMessage = "Either Email or Password is incorrect, Please check and try again!"
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
                UserDataModel userData = new()
                {
                    IsError = true,
                    ErrorMessage = "User not registered, Please register first!"
                };
                return userData;
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
                    UserDataModel userModel = new()
                    {
                        IsError = true,
                        ErrorMessage = "User already exists, Please Login"
                    };
                    return userModel;
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
                    RoleId = user.Roleid,
                    IsError = false
                };
                return userDataModel;
            }
            catch
            {
                UserDataModel userModel = new()
                {
                    IsError = true,
                    ErrorMessage = "User already exists, Please Login"
                };
                return userModel;
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
                        ErrorMessage = "User not registered, Please register first!"
                    };
                    return passwordObject;
                }
                else
                {
                    Guid g = Guid.NewGuid();
                    userVar.ResetKey = g.ToString();
                    _context.Users.Update(userVar);
                    _context.SaveChanges();
                    var subject = "Helperland - Change Password";
                    var agreementUrl = $"http://localhost:4200/resetpassword?t={_emailConfig.Encode(g.ToString())}&e={_emailConfig.Encode(userVar.Email)}&dt={_emailConfig.Encode(DateTime.Now.ToString())}";
                    var emailBody = $"<p>Here's the link to change the password.</p><p><a href='{agreementUrl}'>Link to change the password</a></p><p>Thanks and Regards,<br> Helperland Team</p>";
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
                ResetPass passwordObject = new()
                {
                    IsError = true,
                    ErrorMessage = "User not registered, Please register first!"
                };
                return passwordObject;
            }
        }
        #endregion

        #region ResetPassLink
        public ResetPass ResetPassLink(ResetPass user)
        {
            try
            {
                var email = _emailConfig.Decode(user.Email ?? "");
                var token = _emailConfig.Decode(user.ResetKey ?? "");
                var date = _emailConfig.Decode(user.Date ?? "");

                TimeSpan time = DateTime.Now - DateTime.Parse(date);

                if (time.TotalHours > 2)
                {
                    ResetPass passwordObject = new()
                    {
                        IsError = true,
                        ErrorMessage = "Link is either expired or invalid!"
                    };
                    return passwordObject;
                }

                var userVar = _context.Users.FirstOrDefault(u => u.Email == email && u.ResetKey == token);
                if (userVar == null)
                {
                    ResetPass passwordObject = new()
                    {
                        IsError = true,
                        ErrorMessage = "Link is either expired or invalid!"
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
                ResetPass passwordObject = new()
                {
                    IsError = true,
                    ErrorMessage = "Link is either expired or invalid!"
                };
                return passwordObject;
            }
        }
        #endregion

        #region ResetPass
        public ResetPass ResetPass(ResetPass user)
        {
            try
            {
                var email = _emailConfig.Decode(user.Email ?? "");

                var userVar = _context.Users.FirstOrDefault(u => u.Email == email);
                if (userVar == null)
                {
                    ResetPass passObject = new()
                    {
                        IsError = true,
                        ErrorMessage = "Internal Server Error!"
                    };
                    return passObject;
                }
                else
                {
                    userVar.Password = user.Password;
                    userVar.ResetKey = null;
                    userVar.ModifiedDate = DateTime.Now;
                    _context.Update(userVar);
                    _context.SaveChanges();
                    ResetPass passwordObject = new()
                    {
                        Email = userVar.Email,
                        IsError = false
                    };
                    return passwordObject;
                }
            }
            catch {
                ResetPass passObject = new()
                {
                    IsError = true,
                    ErrorMessage = "Password can not be updated!"
                };
                return passObject;
            }
        }
        #endregion

        #region GetUsers
        public List<UserDataModel> GetUsers()
        {
            try
            {
                List<UserDataModel> user = (from u in _context.Users
                                            orderby u.UserId
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

        #region GetProfile
        public ProfileDataModel GetProfile(string email)
        {
            try
            {
                if (email == null)
                {
                    ProfileDataModel profile = new()
                    {
                        IsError = true,
                        ErrorMessage = "Can not load profile data"
                    };
                    return profile;
                }
                else
                {
                    var user = _context.Users.FirstOrDefault(u => u.Email == email);
                    if (user == null)
                    {
                        ProfileDataModel profile = new()
                        {
                            IsError = true,
                            ErrorMessage = "Can not load profile data"
                        };
                        return profile;
                    }
                    else
                    {
                        ProfileDataModel profile = new()
                        {
                            FirstName = user.FirstName,
                            LastName = user.LastName,
                            Email = user.Email,
                            Contact = user.Mobile,
                            DateOfBirth = user.DateOfBirth,
                            Language = user.Language,
                            IsError = false
                        };
                        return profile;
                    }
                }
            }
            catch
            {
                ProfileDataModel profile = new()
                {
                    IsError = true,
                    ErrorMessage = "Can not load profile data"
                };
                return profile;
            }
        }
        #endregion

        #region UpdateProfile
        public ProfileDataModel UpdateProfile(ProfileDataModel profile)
        {
            try
            {
                if (profile.Email == null)
                {
                    ProfileDataModel profileData = new()
                    {
                        IsError = true,
                        ErrorMessage = "Can not load profile data"
                    };
                    return profileData;
                }
                else
                {
                    var user = _context.Users.FirstOrDefault(u => u.Email == profile.Email);
                    if (user == null)
                    {
                        ProfileDataModel profileData = new()
                        {
                            IsError = true,
                            ErrorMessage = "Can not load profile data"
                        };
                        return profileData;
                    }
                    else
                    {
                        user.FirstName = profile.FirstName ?? user.FirstName;
                        user.LastName = profile.LastName ?? user.LastName;
                        user.Mobile = profile.Contact ?? user.Mobile;
                        user.DateOfBirth = profile.DateOfBirth ?? user.DateOfBirth;
                        user.Language = profile.Language ?? user.Language;
                        user.ModifiedDate = DateTime.Now;
                        _context.Users.Update(user);
                        _context.SaveChanges();

                        ProfileDataModel profileData = new()
                        {
                            FirstName = user.FirstName,
                            LastName = user.LastName,
                            Email = user.Email,
                            Contact = user.Mobile,
                            Language = user.Language,
                            IsError = false
                        };
                        return profileData;
                    }
                }
            }
            catch
            {
                ProfileDataModel profileData = new()
                {
                    IsError = true,
                    ErrorMessage = "Can not update profile data"
                };
                return profileData;
            }
        }
        #endregion

        #region UpdatePassword
        public PasswordModel UpdatePassword(PasswordModel password)
        {
            try
            {
                if (password.Email == null)
                {
                    PasswordModel passwordModel = new()
                    {
                        IsError = true,
                        ErrorMessage = "Can not update the password"
                    };
                    return passwordModel;
                }
                else
                {
                    var user = _context.Users.FirstOrDefault(u => u.Email == password.Email && u.Password == password.OldPassword);
                    if (user == null)
                    {
                        PasswordModel passwordModel = new()
                        {
                            IsError = true,
                            ErrorMessage = "Please enter the correct old password to update the password."
                        };
                        return passwordModel;
                    }
                    else if (user.Password == password.NewPassword)
                    {
                        PasswordModel passwordModel = new()
                        {
                            IsError = true,
                            ErrorMessage = "New password must be different from old password."
                        };
                        return passwordModel;
                    }
                    else
                    {
                        user.Password = password.NewPassword;
                        user.ModifiedDate = DateTime.Now;
                        _context.Users.Update(user);
                        _context.SaveChanges();

                        PasswordModel passwordModel = new()
                        {
                            Email = password.Email,
                            IsError = false
                        };
                        return passwordModel;
                    }
                }
            }
            catch
            {
                PasswordModel passwordModel = new()
                {
                    IsError = true,
                    ErrorMessage = "Can not update the password"
                };
                return passwordModel;
            }
        }
        #endregion
    }
}
