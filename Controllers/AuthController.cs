using familytree_api.Dtos.Auth;
using familytree_api.Dtos.Emails;
using familytree_api.Services;
using familytree_api.Services.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace familytree_api.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthController(IAuthService _authService) : Controller
    {
        [HttpPost("signup")]
        public async Task<IActionResult> SignUp(SignUpDto body)
        {
            try
            {
                await _authService.SignUp(body);

                return Ok(new ApiResponse<string>(data: "", message: "Signup successfull. We have sent you an email verification token via email inorder to verify you account", 
                    statusCode: (int)HttpStatusCode.OK));
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResponse<string>(errors:[], message: ex.Message));
            }
        }


        [HttpPost("signin")]
        public async Task<IActionResult> SignIn(SignInDto body)
        {
            try
            {
                var result = await _authService.SignIn(body);

                return Ok(new ApiResponse<string>(data: result, message: "Login successfull!",
                    statusCode: (int)HttpStatusCode.OK));
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResponse<string>(errors: [], message: ex.Message));
            }
        }

        [Authorize]
        [HttpPost("change-password")]
        public async Task<IActionResult> ChangePassword(ChangePasswordDto body)
        {
            try
            {
               await _authService.ChangePassword(body);

                return Ok(new ApiResponse<string>(data: "", message: "Password successfully changed!",
                    statusCode: (int)HttpStatusCode.OK));
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResponse<string>(errors: [], message: ex.Message));
            }
        }

        [HttpPost("reset-password-request/{Email}")]
        public async Task<IActionResult> ResetPasswordRequest(string Email)
        {
            try
            {
                await _authService.ResetPasswordRequest(Email);

                return Ok(new ApiResponse<string>(data: "", message: "Password request successfull. Check your email address",
                    statusCode: (int)HttpStatusCode.OK));
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResponse<string>(errors: [], message: ex.Message));
            }
        }

        [HttpPost("token-exists")]
        public async Task<IActionResult> PasswordResetTokenExists(VerifyTokenDto body)
        {
            try
            {
                var result = await _authService.PasswordResetTokenExists(body);

                return Ok(new ApiResponse<bool>(data: result, message: result ? "Token is valid!" : "Token is invalid",
                    statusCode: result ? (int)HttpStatusCode.OK : (int)HttpStatusCode.BadRequest));
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResponse<string>(errors: [], message: ex.Message));
            }
        }

        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword(ResetPasswordDto body)
        {
            try
            {
                var result = await _authService.ResetPassword(body);

                return Ok(new ApiResponse<string>(data: result, message: "Password reset successfull!",
                    statusCode: (int)HttpStatusCode.OK));
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResponse<string>(errors: [], message: ex.Message));
            }
        }


        [HttpPost("validate-account")]
        public async Task<IActionResult> ValidateUserAccount(VerifyTokenDto body)
        {
            try
            {
                var result = await _authService.ValidateUserAccount(body);

                return Ok(new ApiResponse<string>(data: result, message: "Account successfully validated!",
                    statusCode: (int)HttpStatusCode.OK));
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResponse<string>(errors: [], message: ex.Message));
            }
        }

    }
}
