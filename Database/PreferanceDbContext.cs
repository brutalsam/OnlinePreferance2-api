using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using OnlinePreferance2_api.Model;
using System;
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
        modelBuilder.Entity<Deal>()
            .Property(b => b.InitialCards)
            .HasColumnType("jsonb");
        modelBuilder.Entity<Deal>()
             .Property(b => b.Rounds)
             .HasColumnType("jsonb");      
        modelBuilder.Entity<Deal>()
             .Property(b => b.DealContract)
             .HasColumnType("jsonb");      
        modelBuilder.Entity<Game>()
             .Property(b => b.Players)
             .HasColumnType("jsonb");

        modelBuilder.Entity<Game>()
        .Property(p => p.CreationDate)
        .HasConversion
        (
            src => src.Kind == DateTimeKind.Utc ? src : DateTime.SpecifyKind(src, DateTimeKind.Utc),
            dst => dst.Kind == DateTimeKind.Utc ? dst : DateTime.SpecifyKind(dst, DateTimeKind.Utc)
        );
        base.OnModelCreating(modelBuilder);
    }
}