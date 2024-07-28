using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.FileProviders;
using ShoppingBackstage.BackstageService.Interface;
using ShoppingBackstage.BackstageService.Service;
using ShoppingBackstage.Areas.Account.Services.Interface;
using ShoppingBackstage.Areas.Account.Services.Service;
using ShoppingBackstage.Areas.Categories.Services.Interface;
using ShoppingBackstage.Areas.Categories.Services.Service;
using ShoppingBackstage.Service.Interface;
using ShoppingBackstage.Service.Service;

var builder = WebApplication.CreateBuilder(args);

// connection string
builder.Services
        .AddScoped<SqlConnection, SqlConnection>(_ =>
        {
            var conn = new SqlConnection();
            conn.ConnectionString =
                    builder.Configuration.GetConnectionString("Connection");
            return conn;
        });

// cookie
builder.Services
        .AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
        .AddCookie(options =>
        {
            options.Cookie.Name = "admin_user";
            options.ExpireTimeSpan = TimeSpan.FromMinutes(60);
            options.SlidingExpiration = true;
            options.LoginPath = "/Login";
            options.AccessDeniedPath = "/Home/UnAuth";
        });

// Area service
builder.Services.AddScoped<IAccountManagementService, AccountManagementService>();
builder.Services.AddScoped<ICategoriesManagementService, CategoriesManagementService>();

// global service
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IDropDownService, DropDownService>();
builder.Services.AddScoped<IBannerManagementService, BannerManagementService>();
builder.Services.AddScoped<IAboutManagementService, AboutManagementService>();
builder.Services.AddScoped<IServerFileService, ServerFileService>();

builder.Services.AddSession();
// Add services to the container.
builder.Services.AddControllersWithViews();

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

if (!Directory.Exists(builder.Configuration.GetValue<string>("VirtualFolderPath")))
{
    Directory.CreateDirectory(builder.Configuration.GetValue<string>("VirtualFolderPath"));
}
app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(builder.Configuration.GetValue<string>("VirtualFolderPath")),
    RequestPath = "/Shopping.uploads" // 從磁碟C開始找檔案
});

app.UseSession();
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

// Area
app.MapAreaControllerRoute(
    name: "AccountArea",
    areaName: "Account",
    pattern: "Account/{controller=Home}/{action=Index}/{id?}");

app.MapAreaControllerRoute(
    name: "CategoriesArea",
    areaName: "Categories",
    pattern: "Categories/{controller=Home}/{action=Index}/{id?}");

// Banner
// app.MapControllerRoute(
//     name: "banner",
//     pattern: "{controller=Banner}/{action=Index}/{id?}");

// Home
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

// Login
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Login}/{action=Index}/{id?}");


app.Run();
