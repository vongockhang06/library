using library.Data;
using library.Models;
using library.Services;
using library.Services.Interfaces;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<AppDbContext>(options => 
	options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
);


//Register Interface and Service
builder.Services.AddScoped<ICatalogService, CatalogService>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IPasswordHasher<User>, PasswordHasher<User>>();
builder.Services.AddScoped<IMemberService, MemberService>();

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Account/SignIn";
		options.ExpireTimeSpan = TimeSpan.FromHours(8);
		options.Cookie.HttpOnly = true; //prevent javascript access to the cookie
		options.SlidingExpiration = true; //reset the expiration time if the user is active
		options.Cookie.SameSite= SameSiteMode.Lax; //prevent cross-site request forgery
    });

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
	app.UseExceptionHandler("/Home/Error");
	// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
	app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
	name: "default",
	pattern: "{controller=Catalog}/{action=Index}");

app.Run();
