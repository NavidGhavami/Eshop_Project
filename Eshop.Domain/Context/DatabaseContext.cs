﻿using Eshop.Domain.Entities.Account.Role;
using Eshop.Domain.Entities.Account.User;
using Eshop.Domain.Entities.Contact;
using Eshop.Domain.Entities.Contact.Ticket;
using Eshop.Domain.Entities.Site;
using Microsoft.EntityFrameworkCore;

namespace Eshop.Domain.Context;

public class DatabaseContext : DbContext
{
    public DatabaseContext(DbContextOptions<DatabaseContext> options): base (options){}


    #region Account

    public DbSet<User> Users { get; set; }
    public DbSet<Role> Roles { get; set; }

    #endregion

    #region Site Setting

    public DbSet<SiteSetting> SiteSettings { get; set; }
    public DbSet<AboutUs> AboutUs { get; set; }

    #endregion

    #region Contact

    public DbSet<ContactUs> ContactUs { get; set; }

    #endregion

    #region Ticket

    public DbSet<Ticket> Tickets { get; set; }
    public DbSet<TicketMessage> TicketMessages { get; set; }

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