using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using OnlinePreferance2_api.Model;
using ConfigurationManager = OnlinePreferance2_api.Configuration.ConfigurationManager;

namespace OnlinePreferance2_api.Database;
public class PreferanceDbContext : IdentityUserContext<IdentityUser>
{
    public PreferanceDbContext(DbContextOptions<PreferanceDbContext> options)
        : base(options)
    {
    }

    public DbSet<Game> Games { get; set; }
    protected override void OnConfiguring(DbContextOptionsBuilder options)
    {
        // It would be a good idea to move the connection string to user secrets
        options.UseNpgsql(ConfigurationManager.AppSetting.GetConnectionString("PreferanceDb"));
    }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
    }
}