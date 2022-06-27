using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Normad.Helpers;
using Normad.Models;
using Normad.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Normad.Controllers
{
    public class AuthController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public AuthController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
        }
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Register(RegisterVM registerVM)
        {
            if (!ModelState.IsValid) return View();
            AppUser user = new AppUser
            {
                Fullname=registerVM.Fullname,
                UserName = registerVM.Username,
                Email = registerVM.Email,
            };
            IdentityResult result = await _userManager.CreateAsync(user, registerVM.Password);
            if (!result.Succeeded)
            {
                foreach (var item in result.Errors)
                {
                    ModelState.AddModelError("", item.Description);
                    return View();
                }
            }
            await _userManager.AddToRoleAsync(user,UserRoles.Member.ToString());
            return RedirectToAction("Index", "Home");
        }
        public async Task<IActionResult> LogOut()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(LoginVM loginVM)
        {
            AppUser user;
            if (loginVM.UsernameOrName.Contains("@"))
            {
                user = await _userManager.FindByEmailAsync(loginVM.UsernameOrName);
            }
            else
            {
                user = await _userManager.FindByNameAsync(loginVM.UsernameOrName);
            }
            if (user == null)
            {
                ModelState.AddModelError("", "Sifre,Email veya Username yanlisdir!!");
                return View();
            }
            var result = await _signInManager.PasswordSignInAsync(user, loginVM.Password, loginVM.RememberMe, false);
            if (!result.Succeeded)
            {
                ModelState.AddModelError("", "Sifre,Email veya Username yanlisdir!!");
            }
            if (result.IsLockedOut)
            {
                ModelState.AddModelError("", "Sizin hesabiniz bloklanib biraz gozleyin zehmet olmasa");
                return View();
            }
            await _signInManager.SignInAsync(user, loginVM.RememberMe);
            return RedirectToAction("Index","Home");
        }

        public async Task CreateRole()
        {
            foreach (var item in Enum.GetValues(typeof(UserRoles)))
            {
                if(!await _roleManager.RoleExistsAsync(item.ToString()))
                {
                    await _roleManager.CreateAsync(new IdentityRole(item.ToString()));
                }
            }
        }
    }
}
