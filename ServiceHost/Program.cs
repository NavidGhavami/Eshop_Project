using Eshop.Application.Utilities;
using Eshop.Domain.Context;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using ServiceHost.ContainerDI;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();



#region Dependency Injection Container

builder.Services.RegisterService();

#endregion


#region ConfigDatabase

var connectionString = builder.Configuration.GetConnectionString("Eshop_Project");

builder.Services.AddDbContext<DatabaseContext>(option =>
    option.UseSqlServer(connectionString), ServiceLifetime.Transient);


#endregion

#region Authentication

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;

}).AddCookie(options =>
{
    options.LoginPath = "/Login";
    options.LogoutPath = "/Logout";
    options.AccessDeniedPath = "/404-page-not-found";
    options.ExpireTimeSpan = TimeSpan.FromMinutes(43200);
});
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AdminArea",
        builders
            => builders.RequireRole(new List<string>
                { Roles.Administrator, Roles.AdminAssistant, Roles.ContentUploader }));

    options.AddPolicy("UserManagement",
        builders
            => builders.RequireRole(new List<string>
                { Roles.Administrator, Roles.AdminAssistant }));

});


#endregion

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
    name: "areas",
    pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}"
);

app.MapControllerRoute(

    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");



app.Run();