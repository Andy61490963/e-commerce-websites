using Microsoft.Data.SqlClient;

var builder = WebApplication.CreateBuilder(args);

// ³s±µ¦r¦ê
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

app.UseAuthorization();

// Area
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
