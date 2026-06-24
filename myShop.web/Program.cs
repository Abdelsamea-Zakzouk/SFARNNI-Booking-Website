using Microsoft.EntityFrameworkCore;
using myShop.DataAccess.Data;
using myShop.DataAccess.ImplementationRepo;
using myShop.Entities.Repositories;
using Microsoft.AspNetCore.Identity;
using myShop.Utilities;
using Microsoft.AspNetCore.Identity.UI.Services;
using Stripe;

var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("AppDbContextConnection")
                       ?? throw new InvalidOperationException("Connection string 'AppDbContextConnection' not found.");

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages().AddRazorRuntimeCompilation();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

// Register AppDbContext with the DI container
builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseSqlServer(Connection.SqlConStr); // Using the connection string from Connection class
    options.UseLazyLoadingProxies(); // Enable lazy loading if needed
});
builder.Services.Configure<StripeData>(builder.Configuration.GetSection("stripe"));
// Register Identity services with roles and Entity Framework support
builder.Services.AddIdentity<IdentityUser, IdentityRole>(options =>
    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromDays(4))
    .AddEntityFrameworkStores<AppDbContext>()
    .AddDefaultTokenProviders().AddDefaultUI()
    .AddDefaultUI(); // Generates tokens for reset passwords, etc.

// Register email sender service
builder.Services.AddSingleton<IEmailSender, EmailSender>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
StripeConfiguration.ApiKey = builder.Configuration.GetSection("stripe:Secretkey").Get<string>();
app.UseAuthentication(); // Add authentication middleware
app.UseAuthorization();

// Map Razor Pages and MVC routes
app.MapRazorPages();
////// HomePage
app.MapControllerRoute(
    name: "Home",
    pattern: "{area=Home}/{controller=Home}/{action=Index}/{ProductId?}");
// Define the area route first
app.MapControllerRoute(
    name: "Customer",
    pattern: "{area=Customer}/{controller=Home}/{action=Index}/{ProductId?}");

////////////////
app.MapControllerRoute(
name: "Flight",
pattern: "{area=Flight}/{controller=Home}/{action=Index}/{FlightProductId?}");

// Define the default route to open the landing page
// app.MapControllerRoute(
//     name: "default",
//     pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
