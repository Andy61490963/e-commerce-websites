using Microsoft.Data.SqlClient;
using Microsoft.Extensions.FileProviders;
using Shopping.Service.Interface;
using Shopping.Service.Service;

var builder = WebApplication.CreateBuilder(args);

// �s���r��
builder.Services
       .AddScoped<SqlConnection, SqlConnection>(_ =>
       {
           var conn = new SqlConnection();
           conn.ConnectionString =
                   builder.Configuration.GetConnectionString("Connection");
           return conn;
       });

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddScoped<IDashBoardService, DashBoardService>();

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

app.UseRouting();

app.UseAuthorization();

// Area
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
