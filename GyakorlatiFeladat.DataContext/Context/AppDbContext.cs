using GyakorlatiFeladat.DataContext.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GyakorlatiFeladat.DataContext.Context
{
    public class AppDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<TaskItem> Tasks { get; set; }
        public DbSet<ShoppingItem> ShoppingItems { get; set; }
        public DbSet<Family> Families { get; set; }
        public DbSet<FamilyUsers> FamilyUsers { get; set; }
        public DbSet<FamilyInvite> FamilyInvites { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }
}
}
