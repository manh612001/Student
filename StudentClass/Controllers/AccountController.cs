using Microsoft.AspNetCore.Mvc;
using StudentClass.Interfaces;
using StudentClass.ViewModels;

namespace StudentClass.Controllers
{
    public class AccountController : Controller
    {
        private readonly ILogger<AccountController> _logger;
        private readonly IAccountService _accountService;
        
        public AccountController(ILogger<AccountController> logger,IAccountService accountService)
        {
            _logger= logger;
            _accountService = accountService;
           
        }
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel login)
        {
            try
            {
                if (!ModelState.IsValid)
                    return View(login);
                await _accountService.LoginAsync(login);
                TempData["alert"] = "alert-success";
                TempData["msg"] = "Logging successfully";
                return RedirectToAction("Index", "Home");
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                TempData["alert"] = "alert-danger";
                TempData["msg"] = e.Message;
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
        public async Task<IActionResult> SignUp(RegistrationViewModel registration)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    await _accountService.Registration(registration);

                    TempData["alert"] = "alert-success";
                    TempData["msg"] = "Registration successfully";
                    return RedirectToAction("Login");
                }
                catch (Exception e)
                {
                    _logger.LogError(e.Message);
                    TempData["alert"] = "alert-danger";
                    TempData["msg"] = e.Message;
                    return View();
                }  
            }
            return View();
        }
    }
}
