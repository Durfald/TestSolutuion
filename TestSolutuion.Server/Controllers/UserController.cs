namespace TestSolutuion.Server.Controllers
{
    using global::TestSolutuion.Server.Domain.Models;
    using global::TestSolutuion.Server.Domain.UserManager;
    using global::TestSolutuion.Server.Extensions;
    using Microsoft.AspNetCore.Mvc;
    using System;

    namespace TestSolutuion.Server.Controllers
    {

        [ApiController]
        [Route("[controller]")]
        [AllowRoles(DefaultStaticData.ManagerRole)]
        public class UserController : ControllerBase
        {

            private readonly ILogger<UserController> _logger;
            private readonly IUserService _userService;
            public UserController(ILogger<UserController> logger, IUserService userService)
            {
                _logger = logger;
                _userService = userService;
            }

            [HttpGet("GetUser/{id}")]
            public async Task<IActionResult> GetUser(string id)
            {
                try
                {
                    var user = await _userService.GetUserByIdAsync(id);
                    return Ok(user);
                }
                catch (ArgumentException ex)
                {
                    _logger.LogError(ex, "Error get user");
                    return BadRequest(ex.Message);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error get user");
                    return StatusCode(500, "Internal server error");
                }
            }

            [HttpGet("GetAllUsers")]
            public async Task<IActionResult> GetAllUsers()
            {
                try
                {
                    var users = await _userService.GetAllUsersAsync();
                    return Ok(users);
                }
                catch (ArgumentException ex)
                {
                    _logger.LogError(ex, "Error get users");
                    return BadRequest(ex.Message);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error get user");
                    return StatusCode(500, "Internal server error");
                }
            }

            [HttpPost("CreateUser")]
            public async Task<IActionResult> CreateUser(UserModel user)
            {
                if(!ModelState.IsValid)
                    return BadRequest(ModelState);
                try
                {
                    await _userService.CreateUserAsync(user);
                    return Ok(user);
                }
                catch (ArgumentException ex)
                {
                    _logger.LogError(ex, "Error create user");
                    return BadRequest(ex.Message);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error create user");
                    return StatusCode(500, "Internal server error");
                }
            }

            [HttpDelete("DeleteUser/{id}")]
            public async Task<IActionResult> DeleteUser(string id)
            {
               try
                {
                    await _userService.DeleteUserAsync(id);
                    return Ok();
                }
                catch (ArgumentException ex) {
                    _logger.LogError(ex, "Error delete user");
                    return BadRequest(ex.Message);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error delete user");
                    return StatusCode(500, "Internal server error");
                }
            }

            [HttpPut("UpdateUser/{id}")]
            public async Task<IActionResult> UpdateUser(string id,UserModel user)
            {
                try
                {
                    await _userService.UpdateUserAsync(id, user);
                    return Ok(user);
                }
                catch (ArgumentException ex)
                {
                    _logger.LogError(ex, "Error update user");
                    return BadRequest(ex.Message);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error update user");
                    return StatusCode(500, "Internal server error");
                }
            }

        }
    }
}
