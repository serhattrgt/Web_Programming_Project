using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Web_Programming_Proje.Models;
using Web_Programming_Proje.Data;


var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllersWithViews();


builder.Services.AddDbContext<StoreDbContext>(opts => {
    opts.UseNpgsql(
        builder.Configuration.GetConnectionString("StoreDbConnection"));
});



builder.Services.AddDbContext<AppIdentityDbContext>(opts => 
    opts.UseNpgsql(
        builder.Configuration["ConnectionStrings:IdentityDBConnection"]));


builder.Services.AddIdentity<IdentityUser, IdentityRole>()
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<AppIdentityDbContext>();


builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;                     //BURASINI SEPET MANTIGI ICIN EKLEDIM. SESSIONDA BELIRLI BI SÜRE ICIN SEPET TUTUYORUZ.
    options.Cookie.IsEssential = true;
});

builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/Auth/Login"; // Mevcut sayfanızın yolunu yazın
    options.AccessDeniedPath = "/Auth/Login"; // Yetkisiz erişim sayfasını isteğe bağlı ekleyebilirsiniz
});




var app = builder.Build();


app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseSession();
app.UseRouting();

app.UseAuthorization();

SeedData.EnsurePopulated(app);
await IdentitySeedData.EnsurePopulatedAsync(app);



app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
