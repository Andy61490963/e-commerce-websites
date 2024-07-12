using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.Data.SqlClient;
using ShoppingBackstage.BackstageService.Interface;
using ShoppingBackstage.BackstageService.Service;
using ShoppingBackstage.Areas.Account.Services.Interface;
using ShoppingBackstage.Areas.Account.Services.Service;

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


builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IAccountManagementService, AccountManagementService>();


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


app.UseSession();
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

// Area
app.MapAreaControllerRoute(
    name: "AccountArea",
    areaName: "Account",
    pattern: "Account/{controller=Home}/{action=Index}/{id?}");

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Login}/{action=Index}/{id?}");


app.Run();
