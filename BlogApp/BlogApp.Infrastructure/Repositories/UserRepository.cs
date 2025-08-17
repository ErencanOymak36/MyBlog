using BlogApp.Application.Interfaces;
using BlogApp.Domain.Entities;
using BlogApp.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace BlogApp.Infrastructure.Repositories
{
    public class UserRepository : IUserRepository
	{
		private readonly BlogDbContext _context;

		public UserRepository(BlogDbContext context)
		{
			_context = context ?? throw new ArgumentNullException(nameof(context));
		}
		public async Task AddAsync(User user)
		{
			await _context.Users.AddAsync(user);
			await _context.SaveChangesAsync();
		}

		public async Task DeleteAsync(int id)
		{
			var user= await _context.Users.FindAsync(id);
			if(user != null)
			{
				_context.Users.Remove(user);
				await _context.SaveChangesAsync();
			}
			
		}

		public async Task<IEnumerable<User>> GetAllAsync()
		{
			return  await _context.Users.OrderByDescending(b => b.FirstName)
				 .ToListAsync();
		}

		public async Task<User> GetById(int id)
		{
			return await _context.Users.FindAsync(id);
		}

		public async Task<User> GetByName(string name)
		{
			return await _context.Users.Where(b=>b.FirstName == name).FirstOrDefaultAsync();
		}


        public async Task<User> GetByEmail(string email)
        {
            return await _context.Users.Where(b => b.Email == email).FirstOrDefaultAsync();
        }

        public async Task<bool> LoginAsync(string email)
        {
            var userData= await _context.Users.FirstOrDefaultAsync(b=>b.Email == email);
			if(userData != null)
			{
				return true;
			}
			return false;
        }

       

        public async Task UpdateAsync(User user)
		{
			_context.Users.Update(user);
			await _context.SaveChangesAsync();
		}
	}
}
