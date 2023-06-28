using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StudentClass.Application.Interfaces;
using StudentClass.Application.ViewModels;

namespace StudentClass.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IAccountService _accountService;
        public AccountController(IAccountService accountService)
        {
            _accountService = accountService;
        }

        [HttpPost]
        [Route("Login")]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            try
            {

                return Ok(await _accountService.LoginAPI(model));
            }
            catch (Exception e)
            {

                return BadRequest(e.Message);
            }

        }

        [HttpPost]
        [Route("SignUp")]
        public async Task<IActionResult> SignUp(SignUpViewModel model)
        {
            try
            {
                await _accountService.SignUp(model);
                return Ok("Registration successfully");
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}
