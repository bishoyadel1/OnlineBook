using Domin.Entity;
using Domin.ViewModel;
using Infrastructure;
using Infrastructure.IRepository;
using Infrastructure.IRepository.ServicesRepository;
using Infrastructure.Seeds;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

var connectionString = builder.Configuration.GetConnectionString("DefualtConnection");
builder.Services.AddDbContext<ElectroBookDbContext>(options =>
    options.UseSqlServer(connectionString));

builder.Services.AddIdentity<AppUserModel, IdentityRole>().AddEntityFrameworkStores<ElectroBookDbContext>();
builder.Services.Configure<IdentityOptions>(option =>
{
    option.Password.RequireNonAlphanumeric = false;
    option.Password.RequireLowercase = false;
    option.Password.RequireUppercase = false;
    option.Password.RequireDigit = false;
    option.Password.RequiredLength = 10;
});

builder.Services.ConfigureApplicationCookie(option =>
{
    option.LoginPath = "/admin/";
    option.AccessDeniedPath = "/admin/home/Denied";
});

builder.Services.AddScoped<IServicesCategory<Category>, ServicesRepository>();
builder.Services.AddScoped<IServicesCategoryLog<LogCategory>, ServicesLogRepository>();

var app = builder.Build();

// Add the following lines to seed the default data
using var scope = app.Services.CreateScope();
var services = scope.ServiceProvider;

try
{
    var userManager = services.GetRequiredService<UserManager<AppUserModel>>();
    var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();

    await DefaultRoles.DefaultRole(roleManager);
    await DefaultUser.SeedClaimsForSuperUser(roleManager);
    await DefaultUser.SeedClaimsForSuperUser(roleManager);
    await DefaultUser.DefaultAdmin(userManager, roleManager);
    await DefaultUser.DefaultUsers(userManager);
}
catch (Exception)
{
    throw;
}

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

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllerRoute(
      name: "admin",
      pattern: "{area:exists}/{controller=Account}/{action=login}/{id?}"
    );
    endpoints.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
});

app.Run();