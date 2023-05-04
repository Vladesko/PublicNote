using IdentityModel.Client;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Notes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Mvc.Controllers
{
    public class HomeController : Controller
    {
        private const string URL = "https://localhost:44370"; //Url Note Api
        private readonly IHttpClientFactory httpClientFactory;
        public HomeController(IHttpClientFactory httpClientFactory)
        {
            this.httpClientFactory = httpClientFactory;
        }
        public async Task<IActionResult> Index()
        {
            if (User.Identity.IsAuthenticated)
                return Login(); //its need for enter in your account right now

            var client = httpClientFactory.CreateClient();

            var responce = await client.GetStringAsync($"{URL}/Note/GetNotes");

            List<Note> notes = JsonConvert.DeserializeObject<List<Note>>(responce);
            
            return View(notes);
        }
        [Authorize]
        public IActionResult Login()
        {
            List<Claim> claims = User.Claims.ToList();
            //Search role and redirect to actual action
            string role = claims.Where(options => options.Type == "http://schemas.microsoft.com/ws/2008/06/identity/claims/role").FirstOrDefault().Value; 
            switch (role)
            {
                case "User":
                    return RedirectToAction("Index","User");
                case "Administrator":
                    return RedirectToAction("Index", "Administrator");
                case "MainAdministrator":
                    return RedirectToAction("Index", "MainAdministrator");
                default:
                    return RedirectToAction("Index","Home");
            }
        }
        public IActionResult Logout()
        {
            return SignOut(CookieAuthenticationDefaults.AuthenticationScheme,
                OpenIdConnectDefaults.AuthenticationScheme);
        }
    }
}
