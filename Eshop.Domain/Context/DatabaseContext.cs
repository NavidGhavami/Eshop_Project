using Eshop.Domain.Entites.Account.Role;
using Eshop.Domain.Entites.Account.User;
using Microsoft.EntityFrameworkCore;

namespace Eshop.Domain.Context;

public class DatabaseContext : DbContext
{
    public DatabaseContext(DbContextOptions<DatabaseContext> options): base (options){}


    #region Account

    public DbSet<User> Users { get; set; }
    public DbSet<Role> Roles { get; set; }

    #endregion


    #region OnModelCreating

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        foreach (var relationship in modelBuilder.Model.GetEntityTypes().SelectMany(s => s.GetForeignKeys()))
        {
            relationship.DeleteBehavior = DeleteBehavior.Restrict;
        }

        base.OnModelCreating(modelBuilder);
    }

    #endregion
}