
using BlogApp.UI.Models;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Net.Http;
using System.Reflection;
using System.Text;
using System.Text.Json;

namespace BlogApp.UI.Controllers
{
	public class AdminController : Controller
	{

        private readonly HttpClient _httpClient;
        public AdminController(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient("BlogApi");
        }
        public IActionResult Index()
		{
			return View();
		}

        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login([FromForm] LoginDto model)
        {
            
            try
            {

                var email = model.Email;
                var encodedEmail = Uri.EscapeDataString(email);

                var response = await _httpClient.PostAsync($"/api/users/login?email={Uri.EscapeDataString(email)}", null);
                if (!response.IsSuccessStatusCode)
                {

                    ModelState.AddModelError("", "Giriş başarısız. Lütfen e-posta adresinizi kontrol edin.");
                    return RedirectToAction("Login", "Admin");

                }

                var userResponse = await _httpClient.GetAsync($"/api/users/byemail/{encodedEmail}");

                if (!userResponse.IsSuccessStatusCode)
                {
                    ModelState.AddModelError("", "Kullanıcı bilgileri alınamadı.");
                    return RedirectToAction("Login", "Admin");
                }

                var json = await userResponse.Content.ReadAsStringAsync();
                var user = JsonSerializer.Deserialize<UserDto>(json, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                if (user == null)
                {
                    ModelState.AddModelError("", "Kullanıcı bulunamadı.");
                    return RedirectToAction("Login", "Admin");
                }

                // TempData["SuccessMessage"] = "Giriş başarılı!";

                HttpContext.Session.SetString("UserName", user.UserName);
                HttpContext.Session.SetInt32("RoleId", user.RoleId);

                if (user.RoleId == 1)
                {
                    return RedirectToAction("GetAllPosts", "Admin");

                }
                else
                {
                    return RedirectToAction("Index", "Blogpost");
                }
               
                
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", $"Bir hata oluştu: {ex.Message}");
                return RedirectToAction("Login", "Admin");
            }



        }

        public IActionResult Logout()
        {
            // Session içindeki tüm verileri temizle
            HttpContext.Session.Clear();

            // Login sayfasına yönlendir
            return RedirectToAction("Login", "Admin");
        }



        public async Task<IActionResult> GetAllPosts()
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

        [HttpGet]
        public async Task<IActionResult> UserUpdate(int id)
        {
            var response= await _httpClient.GetAsync($"/api/users/{id}");

            if (!response.IsSuccessStatusCode)
            {
                TempData["Error"] = "Kullanıcı bilgileri alınamadı.";
                return RedirectToAction("GetAllUsers"); // geri listeye dön
            }

            var content = await response.Content.ReadAsStringAsync();
            var user = JsonSerializer.Deserialize<UserDto>(content, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            return View(user);
        }


        [HttpPost]
        public async Task<IActionResult> UserUpdate(UserDto user)
        {
            if (!ModelState.IsValid)
            {
                return View(user);
            }

            var jsonContent = new StringContent(JsonSerializer.Serialize(user),Encoding.UTF8,"application/json");
            var response= await _httpClient.PutAsync($"/api/users/userUpdate/{user.Id}", jsonContent);
            if (response.IsSuccessStatusCode)
            {
                TempData["Success"] = "Kullanıcı başarıyla güncellendi.";
                return RedirectToAction("GetAllUsers"); // örnek redirect
            }
            else
            {
                var error = await response.Content.ReadAsStringAsync();
                ModelState.AddModelError(string.Empty, $"API hatası: {error}");
                return RedirectToAction("GetAllUsers");
            }
        }

        [HttpGet]
        public async Task<IActionResult> UpdatePost(int id)
        {
            var response = await _httpClient.GetAsync($"/api/blogposts/{id}");
            if (!response.IsSuccessStatusCode)
            {
                TempData["Error"] = "Post bilgileri alınamadı.";
                return RedirectToAction("GetAllPost"); // geri listeye dön
            }

            var content = await response.Content.ReadAsStringAsync();
            var post = JsonSerializer.Deserialize<CreateBlogPostDto>(content, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            return View(post);
        }


        [HttpPost]
        public async Task<IActionResult> UpdatePost(UpdateBlogPostDto post)
        {
            if (!ModelState.IsValid)
            {
                return View(post);
            }

            var jsonContent = new StringContent(JsonSerializer.Serialize(post), Encoding.UTF8, "application/json");
            var response = await _httpClient.PutAsync($"/api/blogposts/{post.Id}", jsonContent);
            if (response.IsSuccessStatusCode)
            {
                TempData["Success"] = "Post başarıyla güncellendi.";
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
            else
            {
                var error = await response.Content.ReadAsStringAsync();
                ModelState.AddModelError(string.Empty, $"API hatası: {error}");
                return RedirectToAction("GetAllUsers");
            }
        }

    }
}
