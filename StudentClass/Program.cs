using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using StudentClass.Application.Interfaces;
using StudentClass.Infrastructure.Data;
using StudentClass.Infrastructure.Services;
using StudentClass.Domain;
using StudentClass.MVC.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;
// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<DatabaseDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});
builder.Services.AddAuthentication()
.AddGoogle(googleOptions =>
{
    googleOptions.ClientId = configuration["Authentication:Google:ClientId"];
    googleOptions.ClientSecret = configuration["Authentication:Google:ClientSecret"];
})
.AddFacebook(facebookOption =>
{
    facebookOption.AppId = configuration["Authentication:Facebook:AppId"];
    facebookOption.AppSecret = configuration["Authentication:Facebook:AppSecret"];
});
//TODO: fix addscope
builder.Services.ConfigureAppService();
builder.Services.AddIdentity<AppUser, IdentityRole>()
    .AddEntityFrameworkStores<DatabaseDbContext>()
    .AddDefaultTokenProviders();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
app.UseStatusCodePagesWithRedirects("/Home/Error?statuscode={0}");

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
