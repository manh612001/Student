using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using StudentClass.Interfaces;
using StudentClass.Models;
using StudentClass.ViewModels;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace StudentClass.Service
{
    public class AccountService : IAccountService
    {
        private IConfiguration _config;
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly SignInManager<AppUser> _signInManager;
        public AccountService(UserManager<AppUser> userManager, RoleManager<IdentityRole> roleManager, SignInManager<AppUser> signInManager, IConfiguration configuration)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _signInManager = signInManager;
            _config = configuration;
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
            if(user!= null&& checkPassword) 
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

        public async Task LoginAsync(LoginViewModel login)
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




        }

        public async Task LogoutAsync()
        {
            try
            {
                await _signInManager.SignOutAsync();
            }
            catch (Exception)
            {

                throw new Exception("logout faile");
            }

        }

        public async Task Registration(RegistrationViewModel registration)
        {

            var userExists = await _userManager.FindByNameAsync(registration.UserName);
            if (userExists != null)
            {
                throw new Exception("User already exist");
            }
            AppUser user = new AppUser()
            {
                Email = registration.Email,
                SecurityStamp = Guid.NewGuid().ToString(),
                UserName = registration.UserName,
                Name = registration.Name,
                EmailConfirmed = true,
                PhoneNumberConfirmed = true,
            };
            var result = await _userManager.CreateAsync(user, registration.Password);
            if (!result.Succeeded)
            {
                throw new Exception("User creation failed");
            }

            //if (!await _roleManager.RoleExistsAsync(registration.Role))
            //    await _roleManager.CreateAsync(new IdentityRole(registration.Role));


            //if (await _roleManager.RoleExistsAsync(registration.Role))
            //{
            //    await _userManager.AddToRoleAsync(user, registration.Role);
            //}
        }



    }

}

