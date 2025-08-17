using BlogApp.UI.Models;
using Microsoft.AspNetCore.Mvc;
using System.Text;
using System.Text.Json;

namespace BlogApp.UI.Controllers
{
    public class BlogPostController : Controller
    {
        private readonly HttpClient _httpClient;
        public BlogPostController(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient("BlogApi");
        }
        public async Task<IActionResult> Index()
        {
            var response = await _httpClient.GetAsync("/api/blogposts");
            if (!response.IsSuccessStatusCode)
            {
                return View(new List<BlogPostDto>());
            }
            var content = await response.Content.ReadAsStringAsync();
            var posts = JsonSerializer.Deserialize<List<BlogPostDto>>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            return View(posts);
        }


        [HttpGet]
        public IActionResult SignUp()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> SignUp(UserDto user)
        {
            var json = JsonSerializer.Serialize(user);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var result = await _httpClient.PostAsync("/api/Users/CreateUser", content);
            if (!result.IsSuccessStatusCode)
            {
                ViewBag.Error = "Kayıt İşlemi Sırasında bir Sorun Oluştu!";
                return View();
            }
            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult CreatePost()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreatePost(CreateBlogPostDto post)
        {
            var json= JsonSerializer.Serialize(post);
            var content= new StringContent(json, Encoding.UTF8, "application/json");

            var result= await _httpClient.PostAsync("/api/blogposts", content);
            if (!result.IsSuccessStatusCode)
            {
                var roleId= HttpContext.Session.GetInt32("RoleId");
                if (roleId.HasValue && roleId.Value == 1)
                {
                    // Admin ise
                    return RedirectToAction("GetAllPosts", "Admin");
                }
                else
                {
                    // Normal kullanıcı ise
                    return RedirectToAction("Index", "BlogPost");
                }
            }
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> MyPosts()
        {
            // Session'dan kullanıcı adını al
            var authorName = HttpContext.Session.GetString("UserName");
            if (string.IsNullOrEmpty(authorName))
            {
                // Kullanıcı bilgisi yoksa boş liste döndür
                return View(new List<BlogPostDto>());
            }

            // API çağrısı: /api/blogposts/author/{authorName}
            var response = await _httpClient.GetAsync($"/api/blogposts/author/{Uri.EscapeDataString(authorName)}");

            if (!response.IsSuccessStatusCode)
            {
                return View(new List<BlogPostDto>());
            }

            var content = await response.Content.ReadAsStringAsync();
            var posts = JsonSerializer.Deserialize<List<BlogPostDto>>(content, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            return View(posts);
        }


        public async Task<IActionResult> GetAllUsers()
        {
            var response = await _httpClient.GetAsync("/api/users/GetAllUsers");
            if (!response.IsSuccessStatusCode)
            {
                return View();

            }
            var content = await response.Content.ReadAsStringAsync();
            var posts = JsonSerializer.Deserialize<List<UserDto>>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            return View(posts);
        }


    }
}
