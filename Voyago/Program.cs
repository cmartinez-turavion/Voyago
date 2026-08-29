using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Voyago.Data;
using Voyago.Data.Seed;
using Voyago.Models.Entities;
using Voyago.Services;
using Voyago.Services.Queries;
var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' was not found.");
builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlite(connectionString));
builder.Services.AddDefaultIdentity<ApplicationUser>(options => { options.SignIn.RequireConfirmedAccount=false; options.Password.RequiredLength=8; options.Password.RequireDigit=true; options.Password.RequireLowercase=true; options.Password.RequireUppercase=true; options.Password.RequireNonAlphanumeric=true; options.Lockout.MaxFailedAccessAttempts=5; options.User.RequireUniqueEmail=true; }).AddRoles<IdentityRole>().AddEntityFrameworkStores<ApplicationDbContext>();
builder.Services.ConfigureApplicationCookie(options => { options.Cookie.HttpOnly=true; options.Cookie.SecurePolicy=CookieSecurePolicy.SameAsRequest; options.Cookie.SameSite=SameSiteMode.Lax; options.LoginPath="/Identity/Account/Login"; options.AccessDeniedPath="/Identity/Account/AccessDenied"; });
builder.Services.AddAuthorization(options => options.AddPolicy(AuthorizationPolicies.AdminOnly, policy => policy.RequireRole(ApplicationRoles.Administrator)));
builder.Services.AddRazorPages(options => options.Conventions.AuthorizeAreaFolder("Admin", "/", AuthorizationPolicies.AdminOnly));
builder.Services.AddControllersWithViews();
builder.Services.AddScoped<IIdentityInitializer, IdentityInitializer>();
builder.Services.AddScoped<IDataSeeder, DemoDataSeeder>();
builder.Services.AddScoped<IDestinationQueryService, DestinationQueryService>();
builder.Services.AddScoped<IPackageQueryService, PackageQueryService>();
builder.Services.AddScoped<IHotelQueryService, HotelQueryService>();
builder.Services.AddScoped<IFlightQueryService, FlightQueryService>();
builder.Services.AddScoped<IReviewQueryService, ReviewQueryService>();
var app=builder.Build();
if(!app.Environment.IsDevelopment()){app.UseExceptionHandler("/Error");app.UseHsts();}
app.UseHttpsRedirection(); app.UseStaticFiles(); app.UseRouting(); app.UseAuthentication(); app.UseAuthorization(); app.MapRazorPages(); app.MapControllers();
await using(var scope=app.Services.CreateAsyncScope())
{
    await scope.ServiceProvider.GetRequiredService<IIdentityInitializer>().InitializeAsync(CancellationToken.None);
    if(app.Environment.IsDevelopment()) await scope.ServiceProvider.GetRequiredService<IDataSeeder>().SeedAsync(CancellationToken.None);
}
await app.RunAsync();
