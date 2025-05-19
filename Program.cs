using Microsoft.EntityFrameworkCore;
using projetomvc_6gusta.Models;

var builder = WebApplication.CreateBuilder(args);

// Configurar a string de conexão com MySQL
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

// Adicionar DbContext com MySQL
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));

// Adicionar controllers com views
builder.Services.AddControllersWithViews();

var app = builder.Build();

// Pipeline de requisição HTTP
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();

app.Run();
