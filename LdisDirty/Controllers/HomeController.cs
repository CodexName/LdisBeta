using LdisDirty.DataBaseContext;
using LdisDirty.Models;
using LdisDirty.Services;
using LdisDirty.SignalREngine;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json;
using System.Diagnostics;
using System.Security.Claims;

namespace LdisDirty.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly DbContextApplication _Context;
        private readonly IChatHandlerService _ChatHandler;
        private IHttpContextAccessor _httpContextAccess;
        private IMemoryCache _Cache;
        public HomeController(ILogger<HomeController> logger,DbContextApplication context,IHttpContextAccessor contextaccess,IMemoryCache cache)
        {
            _Context = context;
            _logger = logger;
            _Cache = cache;
            _httpContextAccess = contextaccess;
            _ChatHandler = new ChatHandlerRealize(_httpContextAccess,_Context,_Cache);
        }

        public IActionResult Index()
        {
            var chats = _Context.Chats.ToList();
            return View(chats);
        }

        public IActionResult Privacy()
        {
            return View();
        }
        [HttpPost]
        public IActionResult ConnectToHubHandler(string namegroup)
        {
            string CookieData = HttpContext.Request.Cookies["userkey"];
            var User = JsonConvert.DeserializeObject<User>(CookieData);
            _ChatHandler.Enter(User.Name,namegroup);
            return View("/Views/Group.cshtml");
        }

        public IActionResult Registration ()
        {
            return View("Views/RegLog/Registration.cshtml");
        }
        [HttpPost]
        public async Task<IActionResult> RegistrationHandler (User user)
        {
            if (ModelState.IsValid)
            {
                var Userfind = _Context.Users.FirstOrDefault(x => x.Email == user.Email);
                if (Userfind == null)
                {
                    var User = new User
                    {
                        Email = user.Email,
                        Name = user.Name,
                        Password = user.Password
                    };
                    string jsonData = System.Text.Json.JsonSerializer.Serialize(User);
                    _httpContextAccess.HttpContext.Response.Cookies.Append("userkey", jsonData);
                    _Context.Add(User);
                    _Context.SaveChanges();
                    var ClaimsList = new List<Claim>
                    {
                      new Claim (ClaimTypes.Email, user.Email)
                    };
                    var claimsIdentity = new ClaimsIdentity(ClaimsList, CookieAuthenticationDefaults.AuthenticationScheme);
                    var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);
                    await HttpContext.SignInAsync(claimsPrincipal);
                }
                if (Userfind != null)
                {
                    ModelState.AddModelError("Email", "Такая почта уже существует");
                    ModelState.AddModelError("Password", "Такой пароль уже существует");
                    return View("Views/RegLog/Registration.cshtml");
                }
            }
            return View("Views/RegLog/Registration.cshtml");
        }

        public IActionResult LoginView ()
        {
            return View("/Views/RegLog/Login.cshtml");
        }
        public async Task<IActionResult> LoginhandlerAsync (LoginModel login)
        {
            if (ModelState.IsValid)
            {
                var userlogin = _Context.Users.AsNoTracking().FirstOrDefault(x => x.Email == login.Email && x.Password == login.Password);
                if (userlogin != null)
                {
                    var ClaimsList = new List<Claim>
                    {
                      new Claim (ClaimTypes.Email, login.Email)
                    };
                    var claimsIdentity = new ClaimsIdentity(ClaimsList, CookieAuthenticationDefaults.AuthenticationScheme);
                    var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);
                    await HttpContext.SignInAsync(claimsPrincipal);

                    string jsonData = System.Text.Json.JsonSerializer.Serialize(userlogin);
                    _httpContextAccess.HttpContext.Response.Cookies.Append("userkey", jsonData);
                    return RedirectToAction("Index");
                }
                if (userlogin == null)
                {
                    ModelState.AddModelError("Email","Такой почты не существует");
                    ModelState.AddModelError("Password", "Такого пароля не существует");
                    return View("/Views/RegLog/Login.cshtml");
                }
            }
            return View("/Views/RegLog/Login.cshtml");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}