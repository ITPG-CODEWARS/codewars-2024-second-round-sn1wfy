using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using ShortIT.Entity;
using ShortIT.ViewModel;
using Scrypt;
using ShortIT.Repository;

namespace ShortIT.Controllers
{
    public class UserController : Controller
    {
        private readonly UserRepository _userRepository;

        public UserController(UserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        // GET: Login
        [HttpGet]
        public IActionResult Login()
        {
            return View(new MainUserVM());
        }

        // POST: Login
        [HttpPost]
        public async Task<IActionResult> Login(MainUserVM model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var user = _userRepository.FindByUsernameAndPassword(model.Username, model.Password);
            if (user == null)
            {
                ModelState.AddModelError("", "Invalid username or password.");
                return View(model);
            }

            // Set up claims for the user
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Username),
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.Role, user.IsAdmin ? "Admin" : "User"),
                new Claim(ClaimTypes.Sid, user.Id.ToString())
            };

            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var authProperties = new AuthenticationProperties
            {
                AllowRefresh = true,
                IsPersistent = true
            };

            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity),
                authProperties);

            return RedirectToAction("Index", "Home");
        }

        // GET: Register
        [HttpGet]
        public IActionResult Register()
        {
            return View(new MainUserVM());
        }

        // POST: Register
        [HttpPost]
        public IActionResult Register(MainUserVM model)
        {
            if (!ModelState.IsValid)
                return View(model);

            if (_userRepository.findByUsername(model.Username))
            {
                ModelState.AddModelError("", "Username is already taken.");
                return View(model);
            }

            // Hash the password and create a new user
            var encoder = new ScryptEncoder();
            var hashedPassword = encoder.Encode(model.Password);
            var userToRegister = new User
            {
                Username = model.Username,
                Password = hashedPassword,
                IsAdmin = false // Set admin rights here if needed
            };

            _userRepository.Add(userToRegister);
            return RedirectToAction("Login");
        }

        // Logout
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Home");
        }

        // GET: Shorten URL
        [HttpGet]
        public IActionResult Shorten()
        {
            return View(new ShortenVM());
        }

        // POST: Shorten URL
        [HttpPost]
        public IActionResult Shorten(ShortenVM model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var idGenerator = new IDGenerator();
            var shortenedURL = new ShortenedURL(idGenerator.GenerateId(5), model.Url);
            return RedirectToAction("Dashboard");
        }

        // GET: Dashboard
        [HttpGet]
        public IActionResult Dashboard()
        {
            if (!User.Identity.IsAuthenticated)
                return RedirectToAction("Index", "Home");

            return View();
        }
    }
}