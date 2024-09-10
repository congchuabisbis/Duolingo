using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using WebDuolingo.Areas.Identity.Data;
using WebDuolingo.Data;
var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("WebDuolingoContextConnection") ?? throw new InvalidOperationException("Connection string 'WebDuolingoContextConnection' not found.");

builder.Services.AddDbContext<WebDuolingoContext>(options => options.UseSqlServer(connectionString));

builder.Services.AddDbContext<WebDuolingo.Models.WebDuolingoContext>(options => options.UseSqlServer(connectionString));

builder.Services.AddDefaultIdentity<WebDuolingoUser>(options => options.SignIn.RequireConfirmedAccount = false).AddRoles<IdentityRole>().AddEntityFrameworkStores<WebDuolingoContext>();

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddControllersWithViews().AddRazorRuntimeCompilation();

builder.Services.AddCors(options =>
{
    options.AddPolicy("policy1",policy =>
    {
        policy.WithOrigins()
        .AllowAnyHeader()
        .AllowAnyMethod()
        .AllowAnyOrigin();
    });
});

var app = builder.Build();

/*// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    
}*/

app.UseExceptionHandler("/Home/Error");
// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
app.UseHsts();




app.UseStaticFiles();
app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapRazorPages();

app.UseCors();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
