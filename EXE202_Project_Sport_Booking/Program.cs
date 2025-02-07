
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using EXE202_Project_Sport_Booking.Models;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllersWithViews()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.Preserve;
    });

//builder.Services.AddDbContext<SereinContext>(options =>
//    options.UseSqlServer(builder.Configuration.GetConnectionString("DB")));

//builder.Services.AddTransient<EmailService>(provider =>
//    new EmailService(builder.Configuration));
//builder.Services.AddAuthentication(options =>
//{
//    options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
//    options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
//    options.DefaultChallengeScheme = CookieAuthenticationDefaults.AuthenticationScheme;
//})
//.AddCookie(options =>
//{
//    options.LoginPath = "/Views/Account/Login";
//    options.LogoutPath = "/Views/Account/Logout";
//    options.AccessDeniedPath = "/Views/Account/Login";
//})
//.AddGoogle(GoogleDefaults.AuthenticationScheme, options =>
//{
//    options.ClientId = builder.Configuration.GetSection("GoogleKeys:ClientId").Value;
//    options.ClientSecret = builder.Configuration.GetSection("GoogleKeys:ClientSecret").Value;
//});
// Thêm dịch vụ session
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30); // Thời gian tồn tại của session
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/FormLogin/Login"; // Đường dẫn đến trang đăng nhập
        options.AccessDeniedPath = "/AccessDenied"; // Đường dẫn khi bị từ chối truy cập
        options.ExpireTimeSpan = TimeSpan.FromMinutes(30); // Thời gian hết hạn phiên đăng nhập
    });
builder.Services.AddDbContext<EXE201_Rental_Sport_FieldContext>();
builder.Services.AddRazorPages();
// Thêm dịch vụ ủy quyền
builder.Services.AddAuthorization();
builder.Services.AddRazorPages();

//builder.Services.AddSingleton<IVnPayService, VnPayService>();
var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}
// Sử dụng session trong pipeline
app.UseSession();

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapRazorPages();

app.Run();
