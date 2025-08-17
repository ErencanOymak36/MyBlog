using BlogApp.Application.DTOs;
using BlogApp.Application.Interfaces;
using BlogApp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogApp.Application.UseCases
{
    public class UserUseCases
	{
		private readonly IUserRepository _userRepository;
        public UserUseCases(IUserRepository userRepository)
        {
			_userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
		}


		public async Task<UserDto> CreateUserasync(CreateUserDto user)
		{
			var NewUser = new User(user.UserName, user.FirstName, user.LastName, user.Email, user.RoleId);

			await _userRepository.AddAsync(NewUser); // User entity gönderiliyor
			return ConvertToDto(NewUser); // entity'den DTO'ya dönüş
		}

		public async Task<UserDto> UpdateUserasync(UpdateUserDto user)
		{
			var existUser= await _userRepository.GetById(user.Id);
            if (existUser==null)
            {
				throw new InvalidOperationException($"ID {user.Id} olan Kullanıcı bulunamadı");
			}
            existUser.FirstName = user.FirstName;
            existUser.LastName = user.LastName;
            existUser.UserName = user.UserName;
            existUser.Email = user.Email;
            existUser.RoleId = user.RoleId;
            await _userRepository.UpdateAsync(existUser);
			return ConvertToDto(existUser);
        }
		public  async Task DeleteUserasync(int id)
		{
			
			// Repository'den sil
			await _userRepository.DeleteAsync(id);
		}

		public async Task<IEnumerable<UserDto>> GetAllUsersAsync()
		{
			var users = await _userRepository.GetAllAsync();
			return users.Select(ConvertToDto);
		}
		public async Task<UserDto> GetUserById(int id)
		{
			var user=await _userRepository.GetById(id);
			if (user == null)
				return null;
			return ConvertToDto(user);
		}

		public async Task<UserDto> GetUserByEmail(string email)
		{
			var user = await _userRepository.GetByEmail(email);
            if (user == null)
                return null;
            return ConvertToDto(user);
        }
	

		public async Task<bool> UserLoginAsync(string email)
		{
			var user= await _userRepository.LoginAsync(email);
			if (user)
			{
				return true;
			}
			return false;
		}

		private UserDto ConvertToDto(User user)
		{
			return new UserDto
			{
				Id = user.Id,
				UserName = user.UserName,
				FirstName = user.FirstName,
				LastName = user.LastName,
				Email = user.Email,
				RoleId = user.RoleId,
				
			};
		}
	}
}
