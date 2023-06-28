using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using StudentClass.Application.Interfaces;
using StudentClass.Application.ViewModels;

using StudentClass.Domain;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace StudentClass.Infrastructure.Services
{
    public class AccountService : IAccountService
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly SignInManager<AppUser> _signInManager;
        private IConfiguration _config;

        public AccountService(UserManager<AppUser> userManager, RoleManager<IdentityRole> roleManager, SignInManager<AppUser> signInManager, IConfiguration configuration)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _signInManager = signInManager;
            _config = configuration;
        }

        public async Task<bool> ExternalLogin()
        {
            var info = await _signInManager.GetExternalLoginInfoAsync();
            if (info == null)
            {
                throw new Exception("User Not Found!");
            }
            var result = await _signInManager.ExternalLoginSignInAsync(info.LoginProvider, info.ProviderKey, false);
            AppUser user = new AppUser
            {
                Email = info.Principal?.FindFirst(ClaimTypes.Email)?.Value,
                UserName = info.Principal?.FindFirst(ClaimTypes.Email)?.Value,
                Name = info.Principal?.Identity?.Name,
                Role = null,
                EmailConfirmed = true,
                PhoneNumberConfirmed = true,
            };
            if (result.Succeeded)
            {
                await _signInManager.SignInAsync(user, false);
                return true;
            }
            else
            {
                IdentityResult identityResult = await _userManager.CreateAsync(user);
                if (identityResult.Succeeded)
                {
                    identityResult = await _userManager.AddLoginAsync(user, info);
                    if (identityResult.Succeeded)
                    {
                        await _signInManager.SignInAsync(user, false);
                        return true;
                    }
                }
            }
            return false;
        }

        public async Task<AppUserViewModel> GetByName(string name)
        {

            var user = await _userManager.FindByNameAsync(name);
            if (user != null)
            {
                return new AppUserViewModel()
                {
                    UserName = user.UserName,
                    Email = user.Email,
                    PhoneNumber = user.PhoneNumber,
                    Role = user.Role
                };
            }
            throw new Exception("User not found!");
        }

        public async Task<string> LoginAPI(LoginViewModel login)
        {
            var user = await _userManager.FindByNameAsync(login.UserName);
            if (user == null)
            {
                throw new Exception("Invalid username");
            }

            bool checkPassword = await _userManager.CheckPasswordAsync(user, login.Password);
            if (!checkPassword)
            {
                throw new Exception("Invalid password");
            }
            if (user != null && checkPassword)
            {
                var claims = new[] {
                        new Claim(JwtRegisteredClaimNames.Sub, _config["Jwt:Subject"]),
                        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                        new Claim(JwtRegisteredClaimNames.Iat, DateTime.UtcNow.ToString()),
                        new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                        new Claim("UserName", user.UserName),
                };
                var securitykey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
                var credentials = new SigningCredentials(securitykey, SecurityAlgorithms.HmacSha256);

                var token = new JwtSecurityToken(
                            _config["Jwt:Issuer"],
                            _config["Jwt:Audience"],
                            claims,
                            expires: DateTime.UtcNow.AddMinutes(1),
                            signingCredentials: credentials);
                return new JwtSecurityTokenHandler().WriteToken(token);
            }


            throw new Exception("Error");
        }

        public async Task<string> LoginAsync(LoginViewModel login)
        {
            var user = await _userManager.FindByNameAsync(login.UserName);
            if (user == null)
            {
                throw new Exception("Invalid username");
            }
            if (!await _userManager.CheckPasswordAsync(user, login.Password))
            {
                throw new Exception("Invalid password");
            }
            var signInResult = await _signInManager.PasswordSignInAsync(user, login.Password, true, true);

            if (signInResult.Succeeded)
            {
                var userRoles = await _userManager.GetRolesAsync(user);
                var authClaims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name,user.UserName),
                };
                foreach (var userRole in userRoles)
                {
                    authClaims.Add(new Claim(ClaimTypes.Role, userRole));
                }
            }
            else if (signInResult.IsLockedOut)
            {
                throw new Exception("User is locked out");
            }
            else
            {
                throw new Exception("Error on logging in");
            }
            return login.ReturnUrl ?? "/";
        }
        public async Task LogoutAsync()
        {
            await _signInManager.SignOutAsync();
        }
        public async Task SignUp(SignUpViewModel signUp)
        {
            var userExists = await _userManager.FindByNameAsync(signUp.UserName);
            if (userExists != null)
            {
                throw new ArgumentNullException("User already exist");
            }
            var user = new AppUser()
            {
                Email = signUp.Email,
                SecurityStamp = Guid.NewGuid().ToString(),
                UserName = signUp.UserName,
                Name = signUp.Name,
                EmailConfirmed = true,
                PhoneNumberConfirmed = true,
            };
            var result = await _userManager.CreateAsync(user, signUp.Password);
            if (!result.Succeeded)
            {
                throw new Exception("User creation failed");
            }
            if (!await _roleManager.RoleExistsAsync(signUp.Role))
            {
                await _roleManager.CreateAsync(new IdentityRole(signUp.Role));
            }
            else
            {
                await _userManager.AddToRoleAsync(user, signUp.Role);
            }
        }

    }
}
