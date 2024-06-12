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
        private readonly IUserService _login;
        private readonly ITokenService _tokenService;
        private readonly ILogger<HelperlandController> _logger;

        public HelperlandController(IUserService login, ITokenService tokenService, ILogger<HelperlandController> logger)
        {
            _login = login;
            _tokenService = tokenService;
            _logger = logger;
        }

        #region Login
        [HttpPost, Route("Login")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult Login([FromBody] LoginModel user)
        {
            UserDataModel U = _login.Login(user);
            if (U.IsError == false)
            {
                var jwtToken = _tokenService.GenerateJWTAuthetication(U);
                U.Token = jwtToken;
                _logger.LogInformation(U.FirstName + " " + U.LastName + " logged in");
                return Ok(U);
            }
            _logger.LogError("Unauthorized user " + user.Email + " tried to login");
            return NotFound(U);
        }
        #endregion

        #region Signup
        [HttpPost, Route("Signup")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult Signup([FromBody] UserModel user)
        {
            if (user.Password != user.Confpassword)
            {
                _logger.LogError(user.Firstname + " " + user.Lastname + " entered different passwords");
                return BadRequest("Password and confirm password must match");
            }
            UserDataModel U = _login.Signup(user);
            if (U.Email != null)
            {
                _logger.LogInformation(U.FirstName + " " + U.LastName + " registered as new user");
                return Ok(U);
            }
            _logger.LogError(user.Firstname + " " + user.Lastname + " tried to register again");
            return StatusCode(StatusCodes.Status500InternalServerError);
        }
        #endregion

        #region ForgotPass
        [HttpPost, Route("ForgotPass")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult ForgotPass([FromBody] ResetPass user)
        {
            ResetPass U = _login.ForgotPass(user);
            if (U.IsError == false)
            {
                _logger.LogInformation(U.Email + " wants to change the password");
                return Ok(U);
            }
            _logger.LogError("Unauthorized user " + user.Email + " wants to change the password");
            return NotFound(U);
        }
        #endregion

        #region ResetPassLink
        [HttpPost, Route("ResetPassLink")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult ResetPassLink([FromBody] ResetPass user)
        {
            ResetPass U = _login.ResetPassLink(user);
            if (U.IsError == false)
            {
                return Ok(U);
            }
            return NotFound(U);
        }
        #endregion

        #region ResetPass
        [HttpPost, Route("ResetPass")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult ResetPass([FromBody] ResetPass user)
        {
            ResetPass U = _login.ResetPass(user);
            if (U.IsError == false)
            {
                _logger.LogInformation("Password changed for " + U.Email);
                return Ok(U);
            }
            return NotFound(U);
        }
        #endregion

        #region GetUsers
        [HttpGet, Route("GetUsers")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public ActionResult<IEnumerable<UserDataModel>> GetUsers()
        {
            List<UserDataModel> user = _login.GetUsers();
            return Ok(user);
        }
        #endregion
    }
}
