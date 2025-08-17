using BlogApp.Application.DTOs;
using BlogApp.Application.UseCases;
using BlogApp.Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BlogApp.API.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class UsersController : ControllerBase
	{
		private readonly UserUseCases _userUseCases;

        public UsersController(UserUseCases userUseCases)
        {
            _userUseCases = userUseCases ?? throw new ArgumentNullException(nameof(userUseCases));
		}

		// GET: api/Users
		[HttpGet("GetAllUsers")]
		public async Task<ActionResult<IEnumerable<UserDto>>> GetAllUsers()
		{
			try
			{
				var Users = await _userUseCases.GetAllUsersAsync();
				return Ok(Users);
			}
			catch (Exception ex)
			{

				return BadRequest($"Hata: {ex.Message}");
			}
		}
		//Post: api/Users
		[HttpPost("CreateUser")]
		public async Task<ActionResult<UserDto>> CreateUser(CreateUserDto user)
		{
			try
			{
				var NewUser = await _userUseCases.CreateUserasync(user);
				return CreatedAtAction(nameof(GetAllUsers), new { id = NewUser.Id }, NewUser);
			}

			catch (ArgumentException ex)
			{

				return BadRequest($"Geçersiz veri: {ex.Message}");
			}
			catch (Exception ex)
			{

				return BadRequest($"Hata: {ex.Message}");
			}

		}

		[HttpGet("{id}")]
		public async Task<ActionResult<UserDto>> GetUser(int id)
		{
            try
            {
                var user = await _userUseCases.GetUserById(id);
                return Ok(user);
            }
            catch (Exception ex)
            {

                return BadRequest($"Hata: {ex.Message}");
            }
        }

        [HttpGet("byemail/{email}")]
        public async Task<ActionResult<UserDto>> GetUserByEmail(string email)
		{
            try
            {
                var user = await _userUseCases.GetUserByEmail(email);
                return Ok(user);
            }
            catch (Exception ex)
            {

                return BadRequest($"Hata: {ex.Message}");
            }
        }

		[HttpPost("login")]
		public async Task<ActionResult<bool>> Login(string email)
		{
			var Login = await _userUseCases.UserLoginAsync(email);
			if (Login)
			{
				return Ok(Login);
			}
			return BadRequest(Login);
		}

		[HttpPut("userUpdate/{id}")]
		public  async Task<ActionResult<UserDto>> UpdateUserAsyc(int id, UpdateUserDto updateDto)
		{
			try
			{
				updateDto.Id = id;
				var user = await _userUseCases.UpdateUserasync(updateDto);
				return Ok(updateDto);
			}
            catch (InvalidOperationException ex)
            {
                return NotFound(ex.Message);
            }
            catch (ArgumentException ex)
            {
                return BadRequest($"Geçersiz veri: {ex.Message}");
            }
            catch (Exception ex)
            {
                return BadRequest($"Hata: {ex.Message}");
            }
        }

	}
}
