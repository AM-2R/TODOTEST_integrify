using Microsoft.VisualBasic.ApplicationServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

[RoutePrefix("api/v1/auth")]

namespace TODOTEST.Controllers
{
    
    public class AuthController : ApiController
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost]
        [Route("signup")]
        public IHttpActionResult Signup(UserDto userDto)
        {
            // Validate input
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Create user
            var user = _authService.CreateUser(userDto.Email, userDto.Password);

            // Return user
            return Ok(user);
        }

        [HttpPost]
        [Route("signin")]
        public IHttpActionResult Signin(UserDto userDto)
        {
            // Validate input
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Authenticate user
            var user = _authService.Authenticate(userDto.Email, userDto.Password);

            // Return token
            return Ok(new
            {
                Token = _authService.GenerateToken(user)
            });
        }

        [HttpPut]
        [Route("changepassword")]
        [Authorize]
        public IHttpActionResult ChangePassword(ChangePasswordDto changePasswordDto)
        {
            // Validate input
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Get user id
            var userId = int.Parse(User.Identity.GetUserId());

            // Change password
            _authService.ChangePassword(userId, changePasswordDto.NewPassword);

            // Return success
            return Ok();
        }
    }

