using BlogApp.Application.Interfaces;
using BlogApp.Domain.Entities;
using BlogApp.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogApp.Infrastructure.Repositories
{
    public class BlogPostRepository : IBlogPostRepository
    {
        private readonly BlogDbContext _context;
        public BlogPostRepository(BlogDbContext context)
        {
            _context=context ?? throw new ArgumentNullException(nameof(context));
        }
        public async Task AddAsync(BlogPost blogPost)
        {
            await _context.BlogPosts.AddAsync(blogPost);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var blogPost=await _context.BlogPosts.FindAsync(id);
            if (blogPost != null) { 
                
                _context.BlogPosts.Remove(blogPost);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _context.BlogPosts.AnyAsync(b=>b.Id==id);
        }

        public async Task<IEnumerable<BlogPost>> GetAllAsync()
        {
            return await _context.BlogPosts
                 .OrderByDescending(b => b.CreatedAt)
                 .ToListAsync();
        }

        public async Task<IEnumerable<BlogPost>> GetByAuthorAsync(string authorName)
        {
            return await _context.BlogPosts.Where(b => b.AuthorName == authorName)
                .OrderByDescending(b => b.CreatedAt)
                .ToListAsync();
        }

        public async Task<BlogPost> GetByIdAsync(int id)
        {
            return await _context.BlogPosts.FindAsync(id);
        }

        public async Task<IEnumerable<BlogPost>> GetPublishedAsync()
        {
            return await _context.BlogPosts.Where(b => b.IsPublished).OrderByDescending(b => b.CreatedAt)
                .ToListAsync();
        }

        public async Task<int> GetPublishedCountAsync()
        {
           // return await _context.BlogPosts.CountAsync(b => b.IsPublished);
            return await _context.BlogPosts.CountAsync(b => b.IsPublished);
        }

        public async Task<int> GetTotalCountAsync()
        {
            return await _context.BlogPosts.CountAsync();
        }

        public async Task<IEnumerable<BlogPost>> SearchAsync(string searchTerm)
        {
            return await _context.BlogPosts
                 .Where(b => b.Title.Contains(searchTerm) || b.Content.Contains(searchTerm))
                 .OrderByDescending(b => b.CreatedAt)
                 .ToListAsync();
        }

        public async Task UpdateAsync(BlogPost blogPost)
        {
            _context.BlogPosts.Update(blogPost);
            await _context.SaveChangesAsync();
        }
    }
}
