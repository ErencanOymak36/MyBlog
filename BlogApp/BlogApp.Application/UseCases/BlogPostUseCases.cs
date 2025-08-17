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
    public class BlogPostUseCases
    {
        private readonly IBlogPostRepository _repository;
        public BlogPostUseCases(IBlogPostRepository repository)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }
        // Yeni blog yazısı oluştur
        public async Task<BlogPostDto> CreateBlogPostAsync(CreateBlogPostDto createDto)
        {
            // Domain entity'si oluştur
            var blogPost = new BlogPost(createDto.Title, createDto.Content, createDto.AuthorName);
            // Repository'ye kaydet
            await _repository.AddAsync(blogPost);
            // DTO'ya dönüştür ve döndür
            return ConvertToDto(blogPost);
        }
        // Blog yazısını güncelle
        public async Task<BlogPostDto> UpdateBlogPostAsync(UpdateBlogPostDto updateDto)
        {
            // Mevcut blog yazısını bul
            var existingBlogPost = await _repository.GetByIdAsync(updateDto.Id);
            if (existingBlogPost == null)
                throw new InvalidOperationException($"ID {updateDto.Id} olan blog yazısı bulunamadı");
            // Domain entity'sini güncelle
            existingBlogPost.Update(updateDto.Title, updateDto.Content);
            // Repository'de güncelle
            await _repository.UpdateAsync(existingBlogPost);
            // DTO'ya dönüştür ve döndür
            return ConvertToDto(existingBlogPost);
        }
        // Blog yazısını sil
        public async Task DeleteBlogPostAsync(int id)
        {
            // Blog yazısının var olup olmadığını kontrol et
            var exists = await _repository.ExistsAsync(id);
            if (!exists)
                throw new InvalidOperationException($"ID {id} olan blog yazısı bulunamadı");
            // Repository'den sil
            await _repository.DeleteAsync(id);
        }
        // Blog yazısını yayınla
        public async Task<BlogPostDto> PublishBlogPostAsync(int id)
        {
            var blogPost = await _repository.GetByIdAsync(id);
            if (blogPost == null)
                throw new InvalidOperationException($"ID {id} olan blog yazısı bulunamadı");
            blogPost.Publish();
            await _repository.UpdateAsync(blogPost);
            return ConvertToDto(blogPost);
        }
        // Blog yazısını yayından kaldır
        public async Task<BlogPostDto> UnpublishBlogPostAsync(int id)
        {
            var blogPost = await _repository.GetByIdAsync(id);
            if (blogPost == null)
                throw new InvalidOperationException($"ID {id} olan blog yazısı bulunamadı");
            blogPost.UnPublish();
            await _repository.UpdateAsync(blogPost);
            return ConvertToDto(blogPost);
        }
        // Blog yazısı detayını getir
        public async Task<BlogPostDto> GetBlogPostByIdAsync(int id)
        {
            var blogPost = await _repository.GetByIdAsync(id);
            if (blogPost == null)
                return null;
            return ConvertToDto(blogPost);
        }
        // Tüm blog yazılarını getir
        public async Task<IEnumerable<BlogPostDto>> GetAllBlogPostsAsync()
        {
            var blogPosts = await _repository.GetAllAsync();
            return blogPosts.Select(ConvertToDto);
        }
        // Sadece yayınlanmış blog yazılarını getir
        public async Task<IEnumerable<BlogPostDto>> GetPublishedBlogPostsAsync()
        {
            var blogPosts = await _repository.GetPublishedAsync();
            return blogPosts.Select(ConvertToDto);
        }
        // Yazara göre blog yazılarını getir
        public async Task<IEnumerable<BlogPostDto>> GetBlogPostsByAuthorAsync(string authorName)
        {
            var blogPosts = await _repository.GetByAuthorAsync(authorName);
            return blogPosts.Select(ConvertToDto);
        }
        // Blog yazısı ara
        public async Task<IEnumerable<BlogPostDto>> SearchBlogPostsAsync(string searchTerm)
        {
            var blogPosts = await _repository.SearchAsync(searchTerm);
            return blogPosts.Select(ConvertToDto);
        }
        // Entity'yi DTO'ya dönüştür
        private BlogPostDto ConvertToDto(BlogPost blogPost)
        {
            return new BlogPostDto
            {
                Id = blogPost.Id,
                Title = blogPost.Title,
                Content = blogPost.Content,
                AuthorName = blogPost.AuthorName,
                CreatedAt = blogPost.CreatedAt,
                UpdatedAt = blogPost.UpdateAt,
                IsPublished = blogPost.IsPublished,
                TimeAgo = blogPost.GetTimeAgo()
            };
        }
    }
}
