using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using eStartup_Back_End.DAL;
using eStartup_Back_End.Models;
using eStartup_Back_End.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace eStartup_Back_End.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<AppUser> _manager;
        private readonly SignInManager<AppUser> _signIn;

        public AccountController(UserManager<AppUser> manager,SignInManager<AppUser> signIn)
        {
            _manager = manager;
            _signIn = signIn;
        }
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [AutoValidateAntiforgeryToken]

        public async Task<IActionResult> Register(RegisterVM register)
        {
            AppUser user = new AppUser
            {
                FirstName = register.FirstName,
                LastName = register.LastName,
                Email = register.Email,
                UserName = register.Username
            };

            IdentityResult result = await _manager.CreateAsync(user, register.Password);

            if (!result.Succeeded)
            {
                foreach(IdentityError error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
                return View();
            }

            return RedirectToAction("Index", "Home");
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [AutoValidateAntiforgeryToken]

        public async Task<IActionResult> Login(LoginVM login)
        {
            AppUser user = await _manager.FindByNameAsync(login.Username);

            try
            {
                Microsoft.AspNetCore.Identity.SignInResult result = await _signIn.PasswordSignInAsync(user, login.Password, false, false);

                if (!result.Succeeded)
                {
                    ModelState.AddModelError("", "Incorrect password or username");
                    return View();
                }

                return RedirectToAction("Index", "Home");
            }
            catch (ArgumentNullException)
            {
                throw new ArgumentNullException("In registers is not such be users");
            }
        }

        public async Task<IActionResult> Logout()
        {
            await _signIn.SignOutAsync();

            return RedirectToAction("Index", "Home");
        }

        public async Task<IActionResult> Edit()
        {
            AppUser user = await _manager.FindByNameAsync(User.Identity.Name);

            EditVM editVM = new EditVM
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                Username = user.UserName
            };

            return View(editVM);
        }


        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> Edit(EditVM editUser)
        {
            AppUser existeduser = await _manager.FindByNameAsync(User.Identity.Name);

            EditVM userEdit = new EditVM
            {
                FirstName = editUser.FirstName,
                LastName = editUser.LastName,
                Username = editUser.Username,
            };

            if (!ModelState.IsValid) return NotFound();

            bool result = userEdit.CurrentPassword == null && userEdit.Password != null && userEdit.ConfirmPassword != null;
            if(editUser.Email == null || editUser.Email != existeduser.Email)
            {
                ModelState.AddModelError("", "Email connot be changed");
                return View(userEdit);
            }
            if (result)
            {
                existeduser.FirstName = editUser.FirstName;
                existeduser.LastName = editUser.LastName;
                existeduser.UserName = editUser.Username;
                await _manager.UpdateAsync(existeduser);
            }
            else
            {
                existeduser.FirstName = editUser.FirstName;
                existeduser.LastName = editUser.LastName;
                existeduser.UserName = editUser.Username;

                IdentityResult identityResult = await _manager.ChangePasswordAsync(existeduser, editUser.CurrentPassword, editUser.Password);

                if (!identityResult.Succeeded)
                {
                    foreach(IdentityError error in identityResult.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }
                    return View(userEdit);
                }
            }

            return RedirectToAction("Index", "Home");
        }
    } 
}
