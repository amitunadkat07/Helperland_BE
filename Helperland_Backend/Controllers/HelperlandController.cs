﻿using Helperland.Entity.Model;
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
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<LoginModel> Login([FromBody] LoginModel user)
        {
            try
            {
                UserDataModel userData = _userService.Login(user);
                if (!userData.IsError)
                {
                    var jwtToken = _tokenService.GenerateJWTAuthetication(userData);
                    userData.Token = jwtToken;
                    return Ok(userData);
                }
                string errorMessage = "Unauthorized user " + user.Email + " tried to login";
                _logger.LogError(errorMessage);
                return NotFound(userData);
            }
            catch
            {
                return NotFound();
            }
        }
        #endregion

        #region Signup
        [HttpPost, Route("Signup")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<UserModel> Signup([FromBody] UserModel user)
        {
            try
            {
                if (user.Password != user.Confpassword)
                {
                    string message = user.Firstname + " " + user.Lastname + " entered different passwords";
                    _logger.LogError(message);
                    return BadRequest("Password and confirm password must match");
                }
                UserDataModel userData = _userService.Signup(user);
                if (userData.Email != null)
                {
                    var jwtToken = _tokenService.GenerateJWTAuthetication(userData);
                    userData.Token = jwtToken;
                    return Ok(userData);
                }
                string errorMessage = user.Firstname + " " + user.Lastname + " tried to register again";
                _logger.LogError(errorMessage);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
            catch
            {
                return BadRequest();
            }
        }
        #endregion

        #region ForgotPass
        [HttpPost, Route("ForgotPass")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<ResetPass> ForgotPass([FromBody] ResetPass user)
        {
            try
            {
                ResetPass passwordObject = _userService.ForgotPass(user);
                if (!passwordObject.IsError)
                {
                    return Ok(passwordObject);
                }
                string errorMessage = "Unauthorized user " + user.Email + " wants to change the password";
                _logger.LogError(errorMessage);
                return NotFound(passwordObject);
            }
            catch 
            { 
                return BadRequest(); 
            }
        }
        #endregion

        #region ResetPassLink
        [HttpPost, Route("ResetPassLink")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<ResetPass> ResetPassLink([FromBody] ResetPass user)
        {
            try
            {
                ResetPass passwordObject = _userService.ResetPassLink(user);
                if (!passwordObject.IsError)
                {
                    return Ok(passwordObject);
                }
                return NotFound(passwordObject);
            }
            catch
            {
                return NotFound();
            }
        }
        #endregion

        #region ResetPass
        [HttpPost, Route("ResetPass")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<ResetPass> ResetPass([FromBody] ResetPass user)
        {
            try
            {
                ResetPass passwordObject = _userService.ResetPass(user);
                if (!passwordObject.IsError)
                {
                    return Ok(passwordObject);
                }
                return NotFound(passwordObject);
            }
            catch
            {
                return NotFound();
            }
        }
        #endregion

        #region GetUsers
        [HttpGet, Route("GetUsers")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public ActionResult<IEnumerable<UserDataModel>> GetUsers()
        {
            try
            {
                List<UserDataModel> user = _userService.GetUsers();
                return Ok(user);
            }
            catch
            {
                return Unauthorized();
            }
        }
        #endregion
    }
}
