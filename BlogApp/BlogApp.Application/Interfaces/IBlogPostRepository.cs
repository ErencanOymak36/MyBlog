using BlogApp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogApp.Application.Interfaces
{
    public interface IBlogPostRepository
    {
        // Temel CRUD operasyonları
        Task<BlogPost> GetByIdAsync(int id);
        Task<IEnumerable<BlogPost>> GetAllAsync();
        Task<IEnumerable<BlogPost>> GetPublishedAsync();
        Task AddAsync(BlogPost blogPost);
        Task UpdateAsync(BlogPost blogPost);
        Task DeleteAsync(int id);
        Task<bool> ExistsAsync(int id);
        // İş mantığına özel sorgular
        Task<IEnumerable<BlogPost>> GetByAuthorAsync(string authorName);
        Task<IEnumerable<BlogPost>> SearchAsync(string searchTerm);
        Task<int> GetTotalCountAsync();
        Task<int> GetPublishedCountAsync();
    }
}
