using Helperland.Entity.Model;
using Helperland.Repository.Interface;
using Helperland.Repository.TokenService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Helperland.Controllers
{
    [Route("api/Helperland")]
    [ApiController]
    public class HelperlandController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly ITokenService _tokenService;
        private readonly ILogger<HelperlandController> _logger;

        public HelperlandController(IUserService userService, ITokenService tokenService, ILogger<HelperlandController> logger)
        {
            _userService = userService;
            _tokenService = tokenService;
            _logger = logger;
        }

        #region Login
        [HttpPost, Route("Login")]
        public ActionResult<ResponseModel<UserDataModel>> Login([FromBody] LoginModel user)
        {
            try
            {
                ResponseModel<UserDataModel> responseModel = _userService.Login(user);
                if (responseModel.IsSuccess && responseModel.Data != null)
                {
                    var jwtToken = _tokenService.GenerateJWTAuthetication(responseModel.Data);
                    responseModel.Data.Token = jwtToken;
                    return responseModel;
                }
                string errorMessage = "Unauthorized user " + user.Email + " tried to login";
                _logger.LogError(errorMessage);
                return responseModel;
            }
            catch (Exception ex)
            {
                return new ResponseModel<UserDataModel>
                {
                    IsSuccess = false,
                    Message = ex.Message,
                    StatusCode = StatusCodes.Status404NotFound,
                };
            }
        }
        #endregion

        #region Signup
        [HttpPost, Route("Signup")]
        public ActionResult<ResponseModel<UserDataModel>> Signup([FromBody] UserModel user)
        {
            try
            {
                ResponseModel<UserDataModel> responseModel = _userService.Signup(user);
                if (responseModel.IsSuccess && responseModel.Data != null)
                {
                    if (responseModel.Data.RoleId == 2)
                    {
                        var jwtToken = _tokenService.GenerateJWTAuthetication(responseModel.Data);
                        responseModel.Data.Token = jwtToken;
                    }
                    return responseModel;
                }
                else
                {
                    string errorMessage = user.Firstname + " " + user.Lastname + " tried to register again";
                    _logger.LogError(errorMessage);
                    return responseModel;
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

        #region ForgotPass
        [HttpPost, Route("ForgotPass")]
        public ActionResult<ResponseModel<ResetPass>> ForgotPass([FromBody] ResetPass user)
        {
            try
            {

                ResponseModel<ResetPass> responseModel = _userService.ForgotPass(user);
                if (responseModel.IsSuccess)
                {
                    return responseModel;
                }
                string errorMessage = "Unauthorized user " + user.Email + " wants to change the password";
                _logger.LogError(errorMessage);
                return responseModel;
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
        [HttpPost, Route("ResetPassLink")]
        public ActionResult<ResponseModel<ResetPass>> ResetPassLink([FromBody] ResetPass user)
        {
            try
            {
                ResponseModel<ResetPass> responseModel = _userService.ResetPassLink(user);
                return responseModel;
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
        [HttpPost, Route("ResetPass")]
        public ActionResult<ResponseModel<ResetPass>> ResetPass([FromBody] ResetPass user)
        {
            try
            {
                ResponseModel<ResetPass> responseModel = _userService.ResetPass(user);
                return responseModel;
            }
            catch (Exception ex)
            {
                return new ResponseModel<ResetPass>
                {
                    IsSuccess = false,
                    Message = ex.Message,
                    StatusCode = StatusCodes.Status404NotFound,
                };
            }
        }
        #endregion

        #region GetUsers
        [Authorize]
        [HttpGet, Route("GetUsers")]
        public ActionResult<ResponseModel<List<UserDataModel>>> GetUsers()
        {
            try
            {
                ResponseModel<List<UserDataModel>> responseModel = _userService.GetUsers();
                return responseModel;
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

        /*#region GetProfile
        [Authorize]
        [HttpGet("GetProfile")]
        public ActionResult<ResponseModel<ProfileDataModel>> GetProfile(string email)
        {
            try
            {
                ResponseModel<ProfileDataModel> responseModel = _userService.GetProfile(email);
                return responseModel;
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

        #region GetProfile
        [Authorize]
        [HttpGet("GetProfile")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<ProfileDataModel> GetProfile(string email)
        {
            try
            {
                if (string.IsNullOrEmpty(email))
                {
                    return BadRequest();
                }
                ProfileDataModel profile = _userService.GetProfile(email);
                if (profile.Email == null)
                {
                    return NotFound(profile);
                }
                return Ok(profile);
            }
            catch
            {
                return Unauthorized();
            }
        }
        #endregion


        #region UpdateProfile
        [Authorize]
        [HttpPut("UpdateProfile")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public ActionResult<ProfileDataModel> UpdateProfile(ProfileDataModel profile)
        {
            try
            {
                ProfileDataModel profileDataModel = _userService.UpdateProfile(profile);
                if (profileDataModel.Email == null)
                {
                    return BadRequest(profileDataModel);
                }
                return Ok(profileDataModel);
            }
            catch (Exception ex)
            {
                return Unauthorized(ex.Message);
            }
        }
        #endregion

        #region ChangePassword
        [Authorize]
        [HttpPut("UpdatePassword")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public ActionResult<PasswordModel> UpdatePassword(PasswordModel password)
        {
            try
            {
                PasswordModel passwordModel = _userService.UpdatePassword(password);
                if (passwordModel.Email == null)
                {
                    return BadRequest(passwordModel);
                }
                return Ok(passwordModel);
            }
            catch (Exception ex)
            {
                return Unauthorized(ex.Message);
            }
        }
        #endregion

        #region CreateAddress
        [HttpPost, Route("CreateAddress")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<AddressDataModel> CreateAddress([FromBody] AddressDataModel address)
        {
            try
            {
                AddressDataModel addressDataModel = _userService.CreateAddress(address);
                if (addressDataModel.IsError)
                {
                    return BadRequest(addressDataModel);
                }
                else
                {
                    return Ok(addressDataModel);
                }
            }
            catch (Exception ex) 
            {
                return Unauthorized(ex.Message);
            }
        }
        #endregion

        #region UpdateAddress
        [HttpPut, Route("UpdateAddress")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public ActionResult<AddressDataModel> UpdateAddress(AddressDataModel address)
        {
            try
            {
                if (address.AddressId == null)
                {
                    return BadRequest();
                }
                else
                {
                    AddressDataModel addressDataModel = _userService.UpdateAddress(address);
                    if (addressDataModel.IsError)
                    {
                        return BadRequest(addressDataModel);
                    }
                    else
                    {
                        return Ok(addressDataModel);
                    }
                }
            }
            catch (Exception ex)
            {
                return Unauthorized(ex.Message);
            }
        }
        #endregion

        #region GetAddressByUser
        [Authorize]
        [HttpGet, Route("GetAddressByUser")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public ActionResult<IEnumerable<AddressDataModel>> GetAddressByUser(string email)
        {
            try
            {
                List<AddressDataModel> addressData = _userService.GetAddressByUser(email);
                return Ok(addressData);
            }
            catch (Exception ex)
            {
                return Unauthorized(ex.Message);
            }
        }
        #endregion

        #region GetAddressById
        [Authorize]
        [HttpGet, Route("GetAddressById")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public ActionResult<AddressDataModel> GetAddressById(int id)
        {
            try
            {
                AddressDataModel addressData = _userService.GetAddressById(id);
                if (addressData.IsError)
                {
                    return BadRequest(addressData);
                }
                return Ok(addressData);
            }
            catch (Exception ex)
            {
                return Unauthorized(ex.Message);
            }
        }
        #endregion

        #region DeleteAddress
        [Authorize]
        [HttpDelete, Route("DeleteAddress")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult DeleteAddress(int id)
        {
            try
            {
                if (id == 0)
                {
                    return BadRequest();
                }
                if (!_userService.DeleteAddress(id))
                {
                    return NotFound();
                }
                return NoContent();
            }
            catch (Exception ex)
            {
                return Unauthorized(ex.Message);
            }
        }
        #endregion
    }
}
