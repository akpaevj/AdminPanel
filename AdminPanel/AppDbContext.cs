using AdminPanel.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdminPanel
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
            => Database.EnsureCreated();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<InfoBaseInfoBasesList>()
                .HasKey(c => new { c.InfoBaseId, c.InfoBasesListId });
        }

        public void ManyToMany<T>(List<T> newCollection, List<T> oldCollection)
        {
            newCollection
                    .Except(oldCollection)
                    .ToList()
                    .ForEach(x => newCollection.Remove(x));

            oldCollection
                .Except(newCollection)
                .ToList()
                .ForEach(x => newCollection.Add(x));
        }

        public DbSet<User> Users { get; set; }
        public DbSet<InfoBase> InfoBases { get; set; }
        public DbSet<InfoBasesList> InfoBasesLists { get; set; }
        public DbSet<InfoBaseInfoBasesList> InfoBaseInfoBasesLists { get; set; }
    }
}
