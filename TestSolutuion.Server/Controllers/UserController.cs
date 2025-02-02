namespace TestSolutuion.Server.Controllers
{
    using global::TestSolutuion.Server.Domain.Models;
    using global::TestSolutuion.Server.Domain.UserManager;
    using Microsoft.AspNetCore.Mvc;
    using System;

    namespace TestSolutuion.Server.Controllers
    {

        [ApiController]
        [Route("[controller]")]
        public class UserController : ControllerBase
        {

            private readonly ILogger<UserController> _logger;
            private readonly IUserService _userService;
            public UserController(ILogger<UserController> logger, UserService userService)
            {
                _logger = logger;
                _userService = userService;
            }

            [HttpGet("GetUser/{id}")]
            public async Task<IActionResult> GetUser(Guid id)
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
            public async Task<IActionResult> DeleteUser(Guid id)
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

            //[HttpDelete("DeleteUsers")]
            //public async Task<IActionResult> DeleteUsers(IEnumerable<Guid> ids)
            //{
            //    foreach (var id in ids)
            //        await repositoryUnitOfWork.User.DeleteAsync(id);
            //    return Ok();
            //}

            [HttpPut("UpdateUser/{id}")]
            public async Task<IActionResult> UpdateUser(Guid id,UserModel user)
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

            //[HttpPut("UpdateUsers")]
            //public async Task<IActionResult> UpdateUsers(IEnumerable<User> users)
            //{
            //    foreach (var p in users)
            //        await repositoryUnitOfWork.User.UpdateAsync(p);
            //    return Ok(users);
            //}
        }
    }
}
