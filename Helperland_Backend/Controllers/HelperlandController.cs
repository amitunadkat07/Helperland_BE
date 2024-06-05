using Helperland.Entity.DataModels;
using Helperland.Entity.Model;
using Helperland.Repository.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;

namespace Helperland.Controllers
{
    [Route("api/Helperland")]
    [ApiController]
    public class HelperlandController : ControllerBase
    {
        private readonly Ilogin _login;

        public HelperlandController(Ilogin login)
        {
            _login = login;
        }

        #region login
        [HttpPost, Route("Login")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult login([FromBody] LoginModel user)
        {
            UserDataModel U = _login.login(user);
            if (U.Email != null)
            {
                return Ok(U);
            }
            return NotFound("You are not an authorised user, Please register first!!!");
        }
        #endregion

        #region signup
        [HttpPost, Route("Signup")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult signup([FromBody] UserModel user)
        {
            if (user.Password != user.Confpassword)
            {
                return BadRequest("Password and confirm password must match");
            }
            if (_login.signup(user))
            {
                return Ok("User created successfully, Kindly Login......");
            }
            return StatusCode(StatusCodes.Status500InternalServerError);
        }
        #endregion
    }
}
