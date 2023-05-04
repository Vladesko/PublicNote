using IdentityServer4.Services;
using IS4.Entities;
using IS4.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IS4.Controllers
{
    public class AccountController : Controller
    {
        private readonly SignInManager<AppUser> _signInManager;
        private readonly UserManager<AppUser> _userManager;
        private readonly IIdentityServerInteractionService _interactionService;
        private readonly AuthorizeContext _context;
        private readonly RoleManager<IdentityRole> _roleManager;

        public AccountController(SignInManager<AppUser> signInManager, UserManager<AppUser> userManager,
            IIdentityServerInteractionService interactionService, AuthorizeContext context, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _interactionService = interactionService; //Need for logout
            _context = context;   //Need for search users in database
            _roleManager = roleManager;
        }
        [HttpGet]
        public IActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;  //Need for registration ReturnUrl
            var viewLogin = new LoginViewModel { ReturnUrl = returnUrl };
            return View(viewLogin);
        }
        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            //This need for searching user. If it is, that is success
            var person = _context.Users.Where(options => options.LogIn == model.LogIn).FirstOrDefault();

            if(person == null) 
            {
                ModelState.AddModelError(string.Empty, "User not found");
                return View(model);
            }
            model.UserName = person.UserName; //userManger search user by UserName. Our program use Login for login 
            var user = await _userManager.FindByNameAsync(model.UserName);
            var result = await _signInManager.PasswordSignInAsync(user, model.Password, false, false);
            if (result.Succeeded)
                return Redirect(model.ReturnUrl);

            ModelState.AddModelError(string.Empty, "Login error");
            return View(model);
        }
        [HttpGet]
        public IActionResult Registration(string returnUrl)
        {           

            var viewRegistration = new RegistrationViewModel { ReturnUrl = returnUrl};
            return View(viewRegistration);

        }
        private bool ExistUser(RegistrationViewModel model)
        {
            //If new user in registration form add phone or login or userName or email its user not can be 
            return _context.Users.Any(options =>
            options.LogIn == model.LogIn ||
            options.PhoneNumber == model.NumberPhone ||
            options.Email == model.Email ||
            options.UserName == model.UserName);
        }
        [HttpPost]
        public async Task<IActionResult> Registration(RegistrationViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            if (ExistUser(model))
            {
                ModelState.AddModelError(string.Empty, "This user already exists");
                return View(model);
            }

            var user = new AppUser
            {
                LogIn = model.LogIn,
                PhoneNumber = model.NumberPhone,
                Email = model.Email,
                FullName = model.PersonFullName,
                UserName = model.UserName,
            };

            var result = await _userManager.CreateAsync(user, model.Password);
            await _userManager.AddToRoleAsync(user, "User"); //Add to user role is 'User' 
            if (result.Succeeded)
            {
                await _signInManager.SignInAsync(user, false);
                return Redirect(model.ReturnUrl);
            }
            ModelState.AddModelError(string.Empty, "Registration error");
            return View(model);
        }
        [HttpGet]
        public async Task<IActionResult> Logout(string logOutId)
        {
            await _signInManager.SignOutAsync();
            var logoutRequest = await _interactionService.GetLogoutContextAsync(logOutId);
            if (string.IsNullOrEmpty(logoutRequest.PostLogoutRedirectUri))
                return RedirectToAction("Index", "Home");
            return Redirect(logoutRequest.PostLogoutRedirectUri);
        }
    }
}
