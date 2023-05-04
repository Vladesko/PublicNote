using IS4.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IS4.Controllers
{
    public class RoleController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        public RoleController(UserManager<AppUser> userManager)
        {
            _userManager = userManager;
        }
        [HttpGet]
        [Authorize(Roles = "Administrator, MainAdministrator")]
        public IActionResult ChangeRoleView()
        {
            return View();
        }
        [Authorize(Roles = "Administrator, MainAdministrator")]
        [HttpPost]
        public async Task<IActionResult> ChangeRoleByUserName(string userName)
        {
            try
            {
                var user = await _userManager.FindByNameAsync(userName);
                if (user == null) 
                {
                  ViewBag.Message = "User is not found";
                  return View();
                }
                await _userManager.AddToRoleAsync(user, "Administrator");
                ViewBag.Message = "Role is change";
                return View();
            }
            catch (Exception exception)
            {
                ViewBag.Message = exception.Message;
                return View();
            }
        }
        [Authorize(Roles = "Administrator")]
        [HttpGet]
        public IActionResult RemoveUserView() 
        {
            return View();
        }
        [HttpPost]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> RemoveUserByUserName(string userName)
        {
            try
            {
                var user = await _userManager.FindByNameAsync(userName);
                if (user == null)
                {
                    ViewBag.Message = "User is not found";
                    return View();
                }
                var roles = await _userManager.GetRolesAsync(user);
                
                if(roles.Any(oprions => oprions == "Administrator")) //If its user is 'Administrator' that Administrator can't remove it
                {
                    ViewBag.Message = "You can't remove administrator";
                    return View();
                }
                await _userManager.DeleteAsync(user);
                ViewBag.Message = "User delete";

                return View();
            }
            catch (Exception exception)
            {
                ViewBag.Message = exception.Message;
                return View();
            }
        }
        [Authorize(Roles = "MainAdministrator")]
        [HttpGet]
        public IActionResult RemoveUserByMainAdminView()
        {
            return View();
        }
        [HttpPost]
        [Authorize(Roles = "MainAdministrator")]
        public async Task<IActionResult> RemoveUserByUserNameMainAdmin(string userName)
        {
            try
            {
                var user = await _userManager.FindByNameAsync(userName);
                if (user == null)
                {
                    ViewBag.Message = "User is not found";
                    return View();
                }
                await _userManager.DeleteAsync(user);
                ViewBag.Message = "User delete";

                return View();
            }
            catch (Exception exception)
            {
                ViewBag.Message = exception.Message;
                return View();
            }
        }
    }
}
