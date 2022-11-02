using System.Reflection.Emit;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataLayer.DatabaseModel;

namespace DataLayer
{

    public class PortfolioDBContext : DbContext
    {
        //ruc server 
        const string ConnectionString = "host=cit.ruc.dk;db=cit01;uid=cit01;pwd=0j4p2QVvDDgm";

        public DbSet<TitleBasics> TitleBasics { get; set; }


        //TITLE AKAS
        public DbSet<Attributes> Attributes { get; set; }
        public DbSet<Language> Languages { get; set; }
        public DbSet<Types> Types { get; set; }
        public DbSet<TitleAkas> TitleAkas { get; set; }

        //USER FRAMEWORK
        public DbSet<User> Users { get; set; }
        public DbSet<UserRating> UserRatings { get; set; }
        public DbSet<UserSearch> UserSearches { get; set; }
        public DbSet<BookmarkTitle> BookmarksTitles { get; set; }
        public DbSet<BookmarkName> BookmarksNames { get; set; }
        public DbSet<Review> Reviews { get; set; }


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            //optionsBuilder.LogTo(Console.WriteLine, LogLevel.Information);
            optionsBuilder.UseNpgsql(ConnectionString);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            //BASE
            /*
            modelBuilder.Entity<X>().ToTable("TABLENAME");
            modelBuilder.Entity<X>().HasKey(x => new { x.KEY }).HasName("KEYNAME");
            modelBuilder.Entity<X>().Property(x => x.PROPERTYNAME).HasColumnName("COLUMNNAME");
            */


            //TITLEBASICS MAPPING
            modelBuilder.Entity<TitleBasics>().ToTable("title_basics");
            modelBuilder.Entity<TitleBasics>().HasKey(x => new { x.TConst }).HasName("title_basics_pkey");
            modelBuilder.Entity<TitleBasics>().Property(x => x.TConst).HasColumnName("tconst");
            modelBuilder.Entity<TitleBasics>().Property(x => x.TitleType).HasColumnName("titletype");
            modelBuilder.Entity<TitleBasics>().Property(x => x.PrimaryTitle).HasColumnName("primarytitle");


            //ATTRIBUTES MAPPING
            modelBuilder.Entity<Attributes>().ToTable("attributes");
            modelBuilder.Entity<Attributes>().HasKey(x => new { x.TConst, x.Ordering }).HasName("attributes_pkey");
            modelBuilder.Entity<Attributes>().Property(x => x.TConst).HasColumnName("tconst");
            modelBuilder.Entity<Attributes>().Property(x => x.Ordering).HasColumnName("ordering");
            modelBuilder.Entity<Attributes>().Property(x => x.Attribute).HasColumnName("attributes");

        }

    }

}