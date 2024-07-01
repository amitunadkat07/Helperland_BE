using Helperland.Entity.DataContext;
using Helperland.Entity.DataModels;
using Helperland.Entity.Model;
using Microsoft.AspNetCore.Http;
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
        public ResponseModel<UserDataModel> Login(LoginModel user)
        {
            try
            {
                var userVar = _context.Users.FirstOrDefault(u => u.Email == user.Email);
                if (userVar == null)
                {
                    return new ResponseModel<UserDataModel>
                    {
                        IsSuccess = false,
                        Message = "User not registered, Please register first!",
                        StatusCode = StatusCodes.Status404NotFound,
                    };
                }
                else
                {
                    if (userVar.Password != user.Password)
                    {
                        return new ResponseModel<UserDataModel>
                        {
                            IsSuccess = false,
                            Message = "Either Email or Password is incorrect, Please check and try again!",
                            StatusCode = StatusCodes.Status404NotFound,
                        };
                    }
                    else
                    {
                        UserDataModel userData = new()
                        {
                            FirstName = userVar.FirstName,
                            LastName = userVar.LastName,
                            Email = userVar.Email,
                            RoleId = userVar.RoleId,
                        };
                        return new ResponseModel<UserDataModel>
                        {
                            Data = userData,
                            IsSuccess = true,
                            Message = "Logged in Successfully.",
                            StatusCode = StatusCodes.Status200OK,
                        };
                    }
                }
            }
            catch (Exception ex) 
            {
                return new ResponseModel<UserDataModel>
                {
                    IsSuccess = false,
                    Message = ex.Message,
                    StatusCode = StatusCodes.Status400BadRequest,
                };
            }
        }
        #endregion

        #region Signup
        public ResponseModel<UserDataModel> Signup(UserModel user)
        {
            try
            {
                var userVar = _context.Users.FirstOrDefault(u => u.Email == user.Email);
                if (userVar != null)
                {
                    return new ResponseModel<UserDataModel>
                    {
                        IsSuccess = false,
                        Message = "User already exists, Please Login",
                        StatusCode = StatusCodes.Status400BadRequest,
                    };
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
                };
                return new ResponseModel<UserDataModel>
                {
                    Data = userDataModel,
                    IsSuccess = true,
                    Message = "Welcome to Helperland.",
                    StatusCode = StatusCodes.Status200OK,
                };
            }
            catch (Exception ex) 
            {
                return new ResponseModel<UserDataModel>
                {
                    IsSuccess = false,
                    Message = ex.Message,
                    StatusCode = StatusCodes.Status400BadRequest,
                };
            }
        }
        #endregion

        #region ForgotPass
        public ResponseModel<ResetPass> ForgotPass(ResetPass user)
        {
            try
            {
                var userVar = _context.Users.FirstOrDefault(u => u.Email == user.Email);
                if (userVar == null)
                {
                    return new ResponseModel<ResetPass>
                    {
                        IsSuccess = false,
                        Message = "User not registered, Please register first!",
                        StatusCode = StatusCodes.Status404NotFound,
                    };
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
                        Email = userVar.Email
                    };
                    return new ResponseModel<ResetPass>
                    {
                        Data = passwordObject,
                        IsSuccess = true,
                        Message = "Link to reset the password is sent.",
                        StatusCode = StatusCodes.Status200OK,
                    };
                }
            }
            catch (Exception ex)
            {
                return new ResponseModel<ResetPass>
                {
                    IsSuccess = false,
                    Message = ex.Message,
                    StatusCode = StatusCodes.Status400BadRequest,
                };
            }
        }
        #endregion

        #region ResetPassLink
        public ResponseModel<ResetPass> ResetPassLink(ResetPass user)
        {
            try
            {
                var email = _emailConfig.Decode(user.Email ?? "");
                var token = _emailConfig.Decode(user.ResetKey ?? "");
                var date = _emailConfig.Decode(user.Date ?? "");

                TimeSpan time = DateTime.Now - DateTime.Parse(date);

                var userVar = _context.Users.FirstOrDefault(u => u.Email == email && u.ResetKey == token);
                if (userVar == null || time.TotalHours > 2)
                {
                    return new ResponseModel<ResetPass>
                    {
                        IsSuccess = false,
                        Message = "Link is either expired or invalid!",
                        StatusCode = StatusCodes.Status400BadRequest,
                    };
                }
                else
                {
                    ResetPass passwordObject = new()
                    {
                        Email = userVar.Email
                    };
                    return new ResponseModel<ResetPass>
                    {
                        Data = passwordObject,
                        IsSuccess = true,
                        Message = "You can change your password, but please remember to follow security guidelines.",
                        StatusCode = StatusCodes.Status200OK,
                    };
                }
            }
            catch (Exception ex)
            {
                return new ResponseModel<ResetPass>
                {
                    IsSuccess = false,
                    Message = ex.Message,
                    StatusCode = StatusCodes.Status400BadRequest,
                };
            }
        }
        #endregion

        #region ResetPass
        public ResponseModel<ResetPass> ResetPass(ResetPass user)
        {
            try
            {
                var email = _emailConfig.Decode(user.Email ?? "");

                var userVar = _context.Users.FirstOrDefault(u => u.Email == email);
                if (userVar == null)
                {
                    return new ResponseModel<ResetPass>
                    {
                        IsSuccess = false,
                        Message = "Internal Server Error!",
                        StatusCode = StatusCodes.Status500InternalServerError,
                    };
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
                        Email = userVar.Email
                    };
                    return new ResponseModel<ResetPass>
                    {
                        Data = passwordObject,
                        IsSuccess = true,
                        Message = "Password changed successfully.",
                        StatusCode = StatusCodes.Status200OK,
                    };
                }
            }
            catch (Exception ex)
            {
                return new ResponseModel<ResetPass>
                {
                    IsSuccess = false,
                    Message = ex.Message,
                    StatusCode = StatusCodes.Status400BadRequest,
                };
            }
        }
        #endregion

        #region GetUsers
        public ResponseModel<List<UserDataModel>> GetUsers()
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
                return new ResponseModel<List<UserDataModel>>
                {
                    Data = user,
                    IsSuccess = true,
                    Message = "",
                    StatusCode = StatusCodes.Status200OK,
                };
            }
            catch (Exception ex)
            {
                return new ResponseModel<List<UserDataModel>>
                {
                    IsSuccess = false,
                    Message = ex.Message,
                    StatusCode = StatusCodes.Status400BadRequest,
                };
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

        /*#region GetProfile
        public ResponseModel<ProfileDataModel> GetProfile(string email)
        {
            try
            {
                if (string.IsNullOrEmpty(email))
                {
                    return new ResponseModel<ProfileDataModel>
                    {
                        IsSuccess = false,
                        Message = "Can not load profile data",
                        StatusCode = StatusCodes.Status400BadRequest,
                    };
                }
                else
                {
                    var user = _context.Users.FirstOrDefault(u => u.Email == email);
                    if (user == null || string.IsNullOrEmpty(user.Email))
                    {
                        return new ResponseModel<ProfileDataModel>
                        {
                            IsSuccess = false,
                            Message = "Can not load profile data",
                            StatusCode = StatusCodes.Status400BadRequest,
                        };
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
                            Language = user.Language
                        };
                        return new ResponseModel<ProfileDataModel>
                        {
                            Data = profile,
                            IsSuccess = true,
                            Message = "",
                            StatusCode = StatusCodes.Status200OK,
                        };
                    }
                }
            }
            catch (Exception ex)
            {
                return new ResponseModel<ProfileDataModel>
                {
                    IsSuccess = false,
                    Message = ex.Message,
                    StatusCode = StatusCodes.Status400BadRequest,
                };
            }
        }
        #endregion*/

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
            catch (Exception ex)
            {
                ProfileDataModel profileData = new()
                {
                    IsError = true,
                    ErrorMessage = ex.Message
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
            catch (Exception ex)
            {
                PasswordModel passwordModel = new()
                {
                    IsError = true,
                    ErrorMessage = ex.Message
                };
                return passwordModel;
            }
        }
        #endregion

        #region CreateAddress
        public AddressDataModel CreateAddress(AddressDataModel address)
        {
            try
            {
                if (address == null)
                {
                    AddressDataModel addressData = new()
                    {
                        IsError = true,
                        ErrorMessage = "Address is empty!"
                    };
                    return addressData;
                }
                else
                {
                    var user = _context.Users.FirstOrDefault(x => x.Email == address.Email);
                    if (user == null)
                    {
                        AddressDataModel addressData = new()
                        {
                            IsError = true,
                            ErrorMessage = "Address not added!"
                        };
                        return addressData;
                    }
                    else
                    {
                        UserAddress userAddress = new()
                        {
                            UserId = user.UserId,
                            AddressLine1 = address.Street,
                            AddressLine2 = address.House,
                            City = address.City,
                            PostalCode = address.ZipCode,
                            Mobile = address.Contact,
                            Email = address.Email,
                            IsDefault = new BitArray(1),
                            IsDeleted = new BitArray(1),
                        };
                        _context.UserAddresses.Add(userAddress);
                        _context.SaveChanges();

                        AddressDataModel addressData = new()
                        {
                            IsError = false
                        };
                        return addressData;
                    }
                }
            }
            catch (Exception ex)
            {
                AddressDataModel addressData = new()
                {
                    IsError = true,
                    ErrorMessage = ex.Message
                };
                return addressData;
            }
        }
        #endregion

        #region UpdateAddress
        public AddressDataModel UpdateAddress(AddressDataModel address)
        {
            try
            {
                var addressData = _context.UserAddresses.FirstOrDefault(x => x.AddressId == address.AddressId);
                if (addressData == null)
                {
                    AddressDataModel addressDataModel = new()
                    {
                        IsError = true,
                        ErrorMessage = "Address not updated!"
                    };
                    return addressDataModel;
                }
                else
                {
                    addressData.AddressLine1 = address.Street ?? addressData.AddressLine1;
                    addressData.AddressLine2 = address.House ?? addressData.AddressLine2;
                    addressData.City = address.City ?? addressData.City;
                    addressData.PostalCode = address.ZipCode ?? addressData.PostalCode;
                    addressData.Mobile = address.Contact ?? addressData.Mobile;
                    _context.UserAddresses.Update(addressData);
                    _context.SaveChanges();

                    AddressDataModel addressDataModel = new()
                    {
                        IsError = false
                    };
                    return addressDataModel;
                }
            }
            catch (Exception ex)
            {
                AddressDataModel addressData = new()
                {
                    IsError = true,
                    ErrorMessage = ex.Message
                };
                return addressData;
            }
        }
        #endregion

        #region GetAddressByUser
        public List<AddressDataModel> GetAddressByUser(string email)
        {
            try
            {
                List<AddressDataModel> addressData = (from address in _context.UserAddresses
                                                      where address.Email == email && address.IsDeleted == new BitArray(1)
                                                      orderby address.AddressId
                                                      select new AddressDataModel
                                                      {
                                                        AddressId = address.AddressId,
                                                        Street = address.AddressLine1,
                                                        House = address.AddressLine2,
                                                        City = address.City,
                                                        ZipCode = address.PostalCode,
                                                        Contact = address.Mobile
                                                      }).ToList();
                return addressData;
            }
            catch (Exception ex)
            {
                List<AddressDataModel> addressData = ( from address in _context.UserAddresses
                                                       select new AddressDataModel
                                                       {
                                                         IsError = true,
                                                         ErrorMessage = ex.Message
                                                       }).ToList();
                return addressData;
            }
        }
        #endregion

        #region GetAddressById
        public AddressDataModel GetAddressById(int id)
        {
            try
            {
                AddressDataModel? addressData = (from address in _context.UserAddresses
                                                where address.AddressId == id
                                                select new AddressDataModel
                                                {
                                                    AddressId = address.AddressId,
                                                    Street = address.AddressLine1,
                                                    House = address.AddressLine2,
                                                    ZipCode = address.PostalCode,
                                                    City = address.City,
                                                    Contact = address.Mobile
                                                }).FirstOrDefault();
                if (addressData == null)
                {
                    return new AddressDataModel()
                    {
                        IsError = true,
                        ErrorMessage = "No address exist with that address id!"
                    };
                }
                return addressData;
            }
            catch (Exception ex)
            {
                AddressDataModel addressData = new()
                {
                    IsError = true,
                    ErrorMessage = ex.Message
                };
                return addressData;
            }
        }
        #endregion

        #region DeleteAddress
        public bool DeleteAddress(int id)
        {
            try
            {
                var address = _context.UserAddresses.FirstOrDefault(x => x.AddressId == id);
                if (address == null)
                {
                    return false;
                }
                else
                {
                    address.IsDeleted = new BitArray(1);
                    address.IsDeleted[0] = true;
                    _context.UserAddresses.Update(address);
                    _context.SaveChanges();
                    return true;
                }
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        #endregion
    }
}
