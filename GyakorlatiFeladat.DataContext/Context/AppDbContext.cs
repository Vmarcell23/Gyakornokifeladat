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
        public DbSet<ShoppingItemVote> ShoppingItemVotes { get; set; }
        public DbSet<Menu> Menus { get; set; }
        public DbSet<Recipe> Recipes { get; set; }  
        public DbSet<MenuRecipe> MenuRecipes { get; set; }
        public DbSet<MenuVote> MenuVotes { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<MenuRecipe>()
                .HasKey(mr => new { mr.MenuId, mr.RecipeId });

            modelBuilder.Entity<MenuRecipe>()
                .HasOne(mr => mr.Menu)
                .WithMany(m => m.MenuRecipes)
                .HasForeignKey(mr => mr.MenuId);

            modelBuilder.Entity<MenuRecipe>()
                .HasOne(mr => mr.Recipe)
                .WithMany(r => r.MenuRecipes)
                .HasForeignKey(mr => mr.RecipeId);
        }
    }
}
