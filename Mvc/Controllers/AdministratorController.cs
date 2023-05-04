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
    [Authorize(Roles ="Administrator")]
    public class AdministratorController : Controller
    {
        private const string URL = "https://localhost:44370"; //Url Api Note
        private readonly IHttpClientFactory httpClientFactory;
        public AdministratorController(IHttpClientFactory httpClientFactory)
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
            note.Author = GetAuthor();

            var accessTokenResponce = await HttpContext.GetTokenAsync("access_token");
            var client = httpClientFactory.CreateClient();

            client.SetBearerToken(accessTokenResponce);
            string json = JsonConvert.SerializeObject(note);
            await client.PostAsync($"{URL}/Note/AddNote", new StringContent(json, Encoding.UTF8, "application/json"));

            return RedirectToAction("Index", "Administrator");
        }
        private string GetAuthor()
        {
            return User.Claims.ToList().Where(options => options.Type == "preferred_username").FirstOrDefault().Value;
        }
        [HttpGet]
        public IActionResult ChangeNote(int id)
        {
            return View(id);
        }
        [HttpGet]
        public IActionResult ChangeText(int id)
        {
            return View(id);
        }
        [HttpPost]
        public async Task<IActionResult> ChangeNoteWithText(int id, string text)
        {
            var client = httpClientFactory.CreateClient();
            var accessTokenResponce = await HttpContext.GetTokenAsync("access_token");
            client.SetBearerToken(accessTokenResponce);

            var responce = await client.GetStringAsync($"{URL}/Note/GetNotes");

            List<Note> notes = JsonConvert.DeserializeObject<List<Note>>(responce); //get list with notes that get the note, who need change
            Note note = notes.Where(options => options.Id == id).FirstOrDefault();
            note.Text = text;

            string json = JsonConvert.SerializeObject(note);
            await client.PostAsync($"{URL}/Note/ChangeNote", new StringContent(json, Encoding.UTF8, "application/json"));
            return RedirectToAction("Index", "Administrator");
        }
        public async Task<IActionResult> RemoveNote(int id)
        {
            var client = httpClientFactory.CreateClient();
            var accessTokenResponce = await HttpContext.GetTokenAsync("access_token");
            client.SetBearerToken(accessTokenResponce);

            var responce = await client.GetStringAsync($"{URL}/Note/GetNotes");

            List<Note> notes = JsonConvert.DeserializeObject<List<Note>>(responce); //get list with notes that get the note, who need remove
            Note note = notes.Where(options => options.Id == id).FirstOrDefault();
            string json = JsonConvert.SerializeObject(note);

            await client.PostAsync($"{URL}/Note/RemoveNote", new StringContent(json, Encoding.UTF8, "application/json"));

            return RedirectToAction("Index", "Administrator");
        }
        public IActionResult Logout()
        {
            return SignOut(CookieAuthenticationDefaults.AuthenticationScheme,
                OpenIdConnectDefaults.AuthenticationScheme);
        }
    }
}
