using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Web_Programming_Proje.Data{
    public class AppIdentityDbContext:IdentityDbContext<IdentityUser>{
        public AppIdentityDbContext(DbContextOptions<AppIdentityDbContext> options):base(options){}
    }
}