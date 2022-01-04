using Microsoft.EntityFrameworkCore;
using MIST_infrastructure.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MIST.Context
{
    public class ApplicationContext:DbContext
    {
        public DbSet<User> Users => Set<User>();
        public DbSet<Post> Posts => Set<Post>();
        public DbSet<Comment> Comments => Set<Comment>();
        public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options)
        { }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<User>().HasIndex(u => u.Login).IsUnique();
        }
        public new void Update<TEntity>(TEntity entity) where TEntity : class
        {
            if (Entry(entity).State == EntityState.Detached)
            {
                Set<TEntity>().Attach(entity);
            }
            Entry(entity).State = EntityState.Modified;
        }

    }
}
