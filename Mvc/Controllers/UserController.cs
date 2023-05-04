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
using System.Text;
using System.Threading.Tasks;

namespace Mvc.Controllers
{
    [Authorize(Roles = "User")]
    public class UserController : Controller
    {
        private const string URL = "https://localhost:44370"; //Url Note Api
        private readonly IHttpClientFactory httpClientFactory;
        public UserController(IHttpClientFactory httpClientFactory)
        {
            this.httpClientFactory = httpClientFactory;
        }
        public async Task<IActionResult> Index()
        {
            var client = httpClientFactory.CreateClient();

            var responce = await client.GetStringAsync($"{URL}/Note/GetNotes");

            List<Note> notes = JsonConvert.DeserializeObject<List<Note>>(responce);

            return View(notes);
        }
        [HttpGet]
        public IActionResult PostNoteAsync()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> AddNote(Note note)
        {
            note.Time = DateTime.Now;
            note.Author = GetAuthorOfMessage();

            var accessTokenResponce = await HttpContext.GetTokenAsync("access_token");
            var client = httpClientFactory.CreateClient();

            client.SetBearerToken(accessTokenResponce);
            string json = JsonConvert.SerializeObject(note);
            await client.PostAsync($"{URL}/Note/AddNote", new StringContent(json, Encoding.UTF8, "application/json"));

            return RedirectToAction("Index", "Home");
        }
        private string GetAuthorOfMessage()
        {
            return User.Claims.ToList().Where(options => options.Type == "preferred_username").FirstOrDefault().Value;
        }

        public IActionResult Logout()
        {
            return SignOut(CookieAuthenticationDefaults.AuthenticationScheme,
                OpenIdConnectDefaults.AuthenticationScheme);
        }
    }
}
