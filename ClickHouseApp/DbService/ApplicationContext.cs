using ClickHouse.EntityFrameworkCore.Extensions;
using ClickHouseApp.DbService.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClickHouseApp.DbService
{
    public class ApplicationContext : DbContext
    {
        public DbSet<TUser> Users => Set<TUser>();


        //public ApplicationContext() => Database.EnsureCreated();

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseClickHouse("Host=localhost;Port=8123;Database=default;Username=default");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TUser>().HasKey(k => k.UserId);
        }
    }
}
