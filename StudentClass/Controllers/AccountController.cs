using Microsoft.AspNetCore.Mvc;
using StudentClass.Application.ViewModels;
using StudentClass.Application.Interfaces;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using StudentClass.Domain;

namespace StudentClass.MVC.Controllers
{
    public class AccountController : Controller
    {
        private readonly IAccountService _accountService;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly ILogger<AccountController> _logger;
        public AccountController(IAccountService accountService,SignInManager<AppUser> signInManager,ILogger<AccountController> logger)
        {
            _accountService = accountService;
            _signInManager = signInManager;
            _logger = logger;
        }

        [HttpGet]
        public IActionResult Login(string ReturnUrl)
        {
            return View(new LoginViewModel { ReturnUrl = ReturnUrl });
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            try
            {
                if(ModelState.IsValid)
                {
                    var url = await _accountService.LoginAsync(model);
                    TempData["msg"] = "success! Login successful";
                    return Redirect(url);
                }
                return View(model);
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                TempData["msg"] = "danger! " + e.Message;
                return View();
            }
        }

        public async Task<IActionResult> LogOut()
        {
            await _accountService.LogoutAsync();
            return RedirectToAction("Login");
        }

        public IActionResult SignUp()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> SignUp(SignUpViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    await _accountService.SignUp(model);
                    TempData["msg"] = "success! SignUp successful";
                    return RedirectToAction("Login");
                }
                catch (Exception e)
                {
                    _logger.LogError(e.Message);
                    TempData["msg"] = "danger! " + e.Message;
                    return View();
                }  
            }
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Detail(string name)
        {
            return View(await _accountService.GetByName(name));
        }

        [HttpPost]
        public IActionResult ExternalLogin(string provider, string? returnUrl = null)
        {
            var redirectUrl = Url.Action("ExternalLoginCallback","Account",new {ReturnUrl = returnUrl});
            var properties = _signInManager.ConfigureExternalAuthenticationProperties(provider, redirectUrl);
            return Challenge(properties, provider);
        }

        public async Task<IActionResult> ExternalLoginCallback()
        {
            try
            {
                var result = await _accountService.ExternalLogin();
                if (result)
                {
                    TempData["msg"] = "success! Login successful";
                    return RedirectToAction("Index", "Home");
                }
                return RedirectToAction("Login", "Account");
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                return BadRequest();
            }
        }
    }
}
