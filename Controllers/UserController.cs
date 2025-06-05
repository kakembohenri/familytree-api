using familytree_api.Dtos.User;
using familytree_api.Services;
using familytree_api.Services.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace familytree_api.Controllers
{
    [Route("api/users")]
    [ApiController]
    public class UserController(
        IUserService _userService
        ) : Controller
    {

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Create(UserInputDto body)
        {
            try
            {
                await _userService.Create(body);

                return Ok(new ApiResponse<string>(data: "", message: "User created successfully",
                    statusCode: (int)HttpStatusCode.Created));
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResponse<string>(errors: [], message: ex.Message));

            }
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> List([FromQuery] UserFilterDto filter)
        {
            try
            {
                var result = await _userService.List(filter);

                return Ok(new ApiResponse<UserFilterOutputDto<UserOutputDto>>(data: result, message: "Ok",
                    statusCode: (int)HttpStatusCode.Created));
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResponse<string>(errors: [], message: ex.Message));

            }
        }


        [Authorize]
        [HttpPut]
        public async Task<IActionResult> Update(UserUpdateDto body)
        {
            try
            {
                await _userService.Update(body);

                return Ok(new ApiResponse<string>(data: "", message: "User updated successfully",
                    statusCode: (int)HttpStatusCode.Created));
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResponse<string>(errors: [], message: ex.Message));

            }
        }
    }
}
