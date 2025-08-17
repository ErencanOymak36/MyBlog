using BlogApp.Application.DTOs;
using BlogApp.Application.UseCases;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace BlogApp.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BlogPostsController : ControllerBase
    {
        private readonly BlogPostUseCases _blogPostUseCases;
        public BlogPostsController(BlogPostUseCases blogPostUseCases)
        {
            _blogPostUseCases = blogPostUseCases ?? throw new ArgumentNullException(nameof(blogPostUseCases));
        }

        // GET: api/blogposts
        [HttpGet]
        public async Task<ActionResult<IEnumerable<BlogPostDto>>> GetAllBlogPosts()
        {
            try
            {
                var blogPost = await _blogPostUseCases.GetAllBlogPostsAsync();
                return Ok(blogPost);
            }
            catch (Exception ex)
            {

                return BadRequest($"Hata: {ex.Message}");
            }
        }
        // GET: api/blogposts/published
        [HttpGet("published")]
        public async Task<ActionResult<IEnumerable<BlogPostDto>>> GetPublishedBlogPosts()
        {
            try
            {
                var blogPost = await _blogPostUseCases.GetPublishedBlogPostsAsync();
                return Ok(blogPost);
            }
            catch (Exception ex)
            {

                return BadRequest($"Hata: {ex.Message}");
            }


        }

        // GET: api/blogposts/5
        [HttpGet("{id}")]
        public async Task<ActionResult<BlogPostDto>> GetBlogPost(int id)
        {
            try
            {
                var blogPost = await _blogPostUseCases.GetBlogPostByIdAsync(id);
                return Ok(blogPost);
            }
            catch (Exception ex)
            {

                return BadRequest($"Hata: {ex.Message}");
            }
        }

        // POST: api/blogposts
        [HttpPost]
        public async Task<ActionResult<BlogPostDto>> CreateBlogPost(CreateBlogPostDto createDto)
        {
            try
            {
                var blogPost = await _blogPostUseCases.CreateBlogPostAsync(createDto);
                return CreatedAtAction(nameof(GetBlogPost), new { id = blogPost.Id }, blogPost);
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
        // PUT: api/blogposts/5
        [HttpPut("{id}")]
        public async Task<ActionResult<BlogPostDto>> UpdateBlogPost(int id, UpdateBlogPostDto updateDto)
        {
            try
            {
                updateDto.Id = id;
                var blogPost = await _blogPostUseCases.UpdateBlogPostAsync(updateDto);
                return Ok(blogPost);
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
        // DELETE: api/blogposts/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBlogPost(int id)
        {
            try
            {
                await _blogPostUseCases.DeleteBlogPostAsync(id);
                return NoContent();
            }
            catch (InvalidOperationException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return BadRequest($"Hata: {ex.Message}");
            }
        }
        // POST: api/blogposts/5/publish
        [HttpPost("{id}/publish")]
        public async Task<ActionResult<BlogPostDto>> PublishBlogPost(int id)
        {
            try
            {
                var blogPost = await _blogPostUseCases.PublishBlogPostAsync(id);
                return Ok(blogPost);
            }
            catch (InvalidOperationException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return BadRequest($"Hata: {ex.Message}");
            }
        }
        // POST: api/blogposts/5/unpublish
        [HttpPost("{id}/unpublish")]
        public async Task<ActionResult<BlogPostDto>> UnpublishBlogPost(int id)
        {
            try
            {
                var blogPost = await _blogPostUseCases.UnpublishBlogPostAsync(id);
                return Ok(blogPost);
            }
            catch (InvalidOperationException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return BadRequest($"Hata: {ex.Message}");
            }
        }
        // GET: api/blogposts/search?q=aramakelimesi
        [HttpGet("search")]
        public async Task<ActionResult<IEnumerable<BlogPostDto>>> SearchBlogPosts([FromQuery] string q)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(q))
                    return BadRequest("Arama kelimesi boş olamaz");
                var blogPosts = await _blogPostUseCases.SearchBlogPostsAsync(q);
                return Ok(blogPosts);
            }
            catch (Exception ex)
            {
                return BadRequest($"Hata: {ex.Message}");
            }
        }
        // GET: api/blogposts/author/ahmet
        [HttpGet("author/{authorName}")]
        public async Task<ActionResult<IEnumerable<BlogPostDto>>> GetBlogPostsByAuthor(string authorName)
        {
            try
            {
                var blogPosts = await _blogPostUseCases.GetBlogPostsByAuthorAsync(authorName);
                return Ok(blogPosts);
            }
            catch (Exception ex)
            {
                return BadRequest($"Hata: {ex.Message}");
            }
        }
    }
}
