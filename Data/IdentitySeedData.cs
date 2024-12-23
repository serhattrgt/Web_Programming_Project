using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Web_Programming_Proje.Data;

namespace Web_Programming_Proje.Models
{
    public static class IdentitySeedData
    {
        private const string Serhat = "SerhatTurgut";
        private const string SerhatPassword = "Serhatturgut1$";
        private const string Yusuf = "YusufOkur";
        private const string YusufPassword = "Yusufokur1$";
        private const string Fatih = "FatihBozkurt";
        private const string FatihPassword = "FatihBozkurt1$";
        private const string Yasin = "YasinSebelek";
        private const string YasinPassword = "Yasinsebelek1$";
        private const string ADMIN_ROLE = "Admin";
        private const string SELLER_ROLE = "Seller";
        private const string CUSTOMER_ROLE = "Customer";

        public static async Task EnsurePopulatedAsync(IApplicationBuilder app)
        {
            using (var scope = app.ApplicationServices.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<AppIdentityDbContext>();
                var userManager = scope.ServiceProvider.GetRequiredService<UserManager<IdentityUser>>();
                var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

                if (context.Database.GetPendingMigrations().Any())
                {
                    context.Database.Migrate();
                }

                if (await roleManager.FindByNameAsync(ADMIN_ROLE) == null)
                {
                    await roleManager.CreateAsync(new IdentityRole(ADMIN_ROLE));
                }
                if (await roleManager.FindByNameAsync(SELLER_ROLE) == null)
                {
                    await roleManager.CreateAsync(new IdentityRole(SELLER_ROLE));
                }
                if (await roleManager.FindByNameAsync(CUSTOMER_ROLE) == null)
                {
                    await roleManager.CreateAsync(new IdentityRole(CUSTOMER_ROLE));
                }


                var userSerhat = await userManager.FindByNameAsync(Serhat);
                if (userSerhat == null)
                {
                    userSerhat = new IdentityUser
                    {
                        UserName = Serhat,
                        Email = "Serhat@gmail.com"
                    };

                    var result = await userManager.CreateAsync(userSerhat, SerhatPassword);
                    if (result.Succeeded)
                    {
                        await userManager.AddToRoleAsync(userSerhat, ADMIN_ROLE);
                    }
                    else
                    {
                        throw new Exception("Users Couldn't save to the Identity Database");
                    }
                }

                var userYusuf = await userManager.FindByNameAsync(Yusuf);
                if (userYusuf == null)
                {
                    userYusuf = new IdentityUser
                    {
                        UserName = Yusuf,
                        Email = "Yusuf@gmail.com"
                    };

                    var result = await userManager.CreateAsync(userYusuf, YusufPassword);
                    if (result.Succeeded)
                    {
                        await userManager.AddToRoleAsync(userYusuf, ADMIN_ROLE);
                    }
                    else
                    {
                        throw new Exception("Users Couldn't save to the Identity Database");
                    }
                }

                var userFatih = await userManager.FindByNameAsync(Fatih);
                if (userFatih == null)
                {
                    userFatih = new IdentityUser
                    {
                        UserName = Fatih,
                        Email = "Fatih@gmail.com"
                    };

                    var result = await userManager.CreateAsync(userFatih, FatihPassword);
                    if (result.Succeeded)
                    {
                        await userManager.AddToRoleAsync(userFatih, ADMIN_ROLE);
                    }
                    else
                    {
                        throw new Exception("Users Couldn't save to the Identity Database");
                    }
                }


                var userYasin = await userManager.FindByNameAsync(Yasin);
                if (userYasin == null)
                {
                    userYasin = new IdentityUser
                    {
                        UserName = Yasin,
                        Email = "Yasin@gmail.com"
                    };

                    var result = await userManager.CreateAsync(userYasin, YasinPassword);
                    if (result.Succeeded)
                    {
                        await userManager.AddToRoleAsync(userYasin, ADMIN_ROLE);
                    }
                    else
                    {
                        throw new Exception("Users Couldn't save to the Identity Database");
                    }
                }











            }
        }
    }
}
