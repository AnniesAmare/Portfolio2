﻿using System.Reflection.Emit;
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

        /* MOVIE MODEL */
        //TITLEBASICS
        public DbSet<TitleBasics> TitleBasics { get; set; }
        public DbSet<TitleEpisode> TitleEpisodes { get; set; }
        public DbSet<TitleRatings> TitleRatings { get; set; }

        //TITLE AKAS
        public DbSet<Attributes> Attributes { get; set; }
        public DbSet<Languages> Languages { get; set; }
        public DbSet<Types> Types { get; set; }
        public DbSet<TitleAkas> TitleAkas { get; set; }

        /* USER FRAMEWORK */
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

            /* MOVIE MODEL */
            //TITLEBASICS MAPPING
            modelBuilder.Entity<TitleBasics>().ToTable("title_basics");
            modelBuilder.Entity<TitleBasics>().HasKey(x => new { x.TConst }).HasName("title_basics_pkey");
            modelBuilder.Entity<TitleBasics>().Property(x => x.TConst).HasColumnName("tconst");
            modelBuilder.Entity<TitleBasics>().Property(x => x.TitleType).HasColumnName("titletype");
            modelBuilder.Entity<TitleBasics>().Property(x => x.PrimaryTitle).HasColumnName("primarytitle");
            modelBuilder.Entity<TitleBasics>().Property(x => x.OriginalTitle).HasColumnName("originaltitle");
            modelBuilder.Entity<TitleBasics>().Property(x => x.IsAdult).HasColumnName("isadult");
            modelBuilder.Entity<TitleBasics>().Property(x => x.StartYear).HasColumnName("startyear");
            modelBuilder.Entity<TitleBasics>().Property(x => x.EndYear).HasColumnName("endyear");
            modelBuilder.Entity<TitleBasics>().Property(x => x.RuntimeMinutes   ).HasColumnName("runtimeminutes");

            //TITLE_EPISODE MAPPING
            modelBuilder.Entity<TitleEpisode>().ToTable("title_episode");
            modelBuilder.Entity<TitleBasics>().HasKey(x => new { x.TConst }).HasName("title_episode_pkey");
            modelBuilder.Entity<TitleEpisode>().Property(x => x.TConst).HasColumnName("tconst");
            modelBuilder.Entity<TitleEpisode>().Property(x => x.ParentTConst).HasColumnName("parenttconst");
            modelBuilder.Entity<TitleEpisode>().Property(x => x.SeasonNumber).HasColumnName("seasonnumber");
            modelBuilder.Entity<TitleEpisode>().Property(x => x.EpisodeNumber).HasColumnName("episodenumber");

            //TITLE_RATINGS
            modelBuilder.Entity<TitleRatings>().ToTable("title_ratings");
            modelBuilder.Entity<TitleRatings>().HasKey(x => new { x.TConst }).HasName("title_ratings_pkey");
            modelBuilder.Entity<TitleRatings>().Property(x => x.TConst).HasColumnName("tconst");
            modelBuilder.Entity<TitleRatings>().Property(x => x.AverageRating).HasColumnName("averagerating");
            modelBuilder.Entity<TitleRatings>().Property(x => x.NumVotes).HasColumnName("numvotes");

            //NAMEBASICS MAPPING
            modelBuilder.Entity<NameBasics>().ToTable("name_basics");
            modelBuilder.Entity<NameBasics>().HasKey(x => new { x.NConst }).HasName("name_basics_pkey");


            /* TITLE AKAS & DEPENDENCIES */
            //TITLEAKAS
            modelBuilder.Entity<TitleAkas>().ToTable("title_akas");
            modelBuilder.Entity<TitleAkas>().HasKey(x => new { x.TConst, x.Ordering }).HasName("title_akas_pkey");
            modelBuilder.Entity<TitleAkas>().Property(x => x.TConst).HasColumnName("tconst");
            modelBuilder.Entity<TitleAkas>().Property(x => x.Ordering).HasColumnName("ordering");
            modelBuilder.Entity<TitleAkas>().Property(x => x.Title).HasColumnName("title");
            modelBuilder.Entity<TitleAkas>().Property(x => x.Region).HasColumnName("region");
            modelBuilder.Entity<TitleAkas>().Property(x => x.IsOriginalTitle).HasColumnName("isoriginaltitle");

            //ATTRIBUTES MAPPING
            modelBuilder.Entity<Attributes>().ToTable("attributes");
            modelBuilder.Entity<Attributes>().HasKey(x => new { x.TConst, x.Ordering }).HasName("attributes_pkey");
            /*
            modelBuilder.Entity<Attributes>()
                .HasOne<TitleAkas>()
                .WithOne(Attributes)
                .HasForeignKey(x => new { x.TConst, x.Ordering })
                .HasConstraintName("attributes_tconst_ordering_fkey");
            */
            modelBuilder.Entity<Attributes>().Property(x => x.TConst).HasColumnName("tconst");
            modelBuilder.Entity<Attributes>().Property(x => x.Ordering).HasColumnName("ordering");
            modelBuilder.Entity<Attributes>().Property(x => x.Attribute).HasColumnName("attributes");

            //LANGUAGES MAPPING
            modelBuilder.Entity<Languages>().ToTable("language");
            modelBuilder.Entity<Languages>().HasKey(x => new { x.TConst, x.Ordering }).HasName("languages_pkey");
            modelBuilder.Entity<Languages>().Property(x => x.TConst).HasColumnName("tconst");
            modelBuilder.Entity<Languages>().Property(x => x.Ordering).HasColumnName("ordering");
            modelBuilder.Entity<Languages>().Property(x => x.Language).HasColumnName("language");

            //ATTRIBUTES MAPPING
            modelBuilder.Entity<Types>().ToTable("types");
            modelBuilder.Entity<Types>().HasKey(x => new { x.TConst, x.Ordering }).HasName("types_pkey");
            modelBuilder.Entity<Types>().Property(x => x.TConst).HasColumnName("tconst");
            modelBuilder.Entity<Types>().Property(x => x.Ordering).HasColumnName("ordering");
            modelBuilder.Entity<Types>().Property(x => x.Type).HasColumnName("types");

            /* USER FRAMEWORK */
            //USERS
            modelBuilder.Entity<User>().ToTable("users");
            modelBuilder.Entity<User>().HasKey(x => new { x.Username }).HasName("users_pkey");
            modelBuilder.Entity<User>().Property(x => x.Username).HasColumnName("username");
            modelBuilder.Entity<User>().Property(x => x.Password).HasColumnName("password");
            modelBuilder.Entity<User>().Property(x => x.BirthYear).HasColumnName("birthyear");
            modelBuilder.Entity<User>().Property(x => x.Email).HasColumnName("email");

            //USER RATING
            modelBuilder.Entity<UserRating>().ToTable("user_rating");
            modelBuilder.Entity<UserRating>().HasKey(x => new { x.Username }).HasName("user_rating_pkey");
            modelBuilder.Entity<UserRating>().Property(x => x.Username).HasColumnName("username");
            modelBuilder.Entity<UserRating>().Property(x => x.Date).HasColumnName("date");
            modelBuilder.Entity<UserRating>().Property(x => x.TConst).HasColumnName("tconst");
            modelBuilder.Entity<UserRating>().Property(x => x.Rating).HasColumnName("rating");

            //USER SEARCH
            modelBuilder.Entity<UserSearch>().ToTable("user_search");
            modelBuilder.Entity<UserSearch>().HasKey(x => new { x.Username }).HasName("user_search_pkey");
            modelBuilder.Entity<UserSearch>().Property(x => x.Username).HasColumnName("username");
            modelBuilder.Entity<UserSearch>().Property(x => x.SearchId).HasColumnName("search_id");
            modelBuilder.Entity<UserSearch>().Property(x => x.Date).HasColumnName("date");
            modelBuilder.Entity<UserSearch>().Property(x => x.Content).HasColumnName("search_content");
            modelBuilder.Entity<UserSearch>().Property(x => x.Category).HasColumnName("search_category");

            //BOOKMARK NAME
            modelBuilder.Entity<BookmarkName>().ToTable("bookmark_name");
            modelBuilder.Entity<BookmarkName>().HasKey(x => new { x.Username }).HasName("bookmark_name_pkey");
            modelBuilder.Entity<BookmarkName>().Property(x => x.Username).HasColumnName("username");
            modelBuilder.Entity<BookmarkName>().Property(x => x.NConst).HasColumnName("nconst");
            modelBuilder.Entity<BookmarkName>().Property(x => x.Annotation).HasColumnName("annotation");

            //BOOKMARK TITLE
            modelBuilder.Entity<BookmarkTitle>().ToTable("bookmark_title");
            modelBuilder.Entity<BookmarkTitle>().HasKey(x => new { x.Username }).HasName("bookmark_title_pkey");
            modelBuilder.Entity<BookmarkTitle>().Property(x => x.Username).HasColumnName("username");
            modelBuilder.Entity<BookmarkTitle>().Property(x => x.TConst).HasColumnName("tconst");
            modelBuilder.Entity<BookmarkTitle>().Property(x => x.Annotation).HasColumnName("annotation");

            //REVIEW
            modelBuilder.Entity<Review>().ToTable("review");
            modelBuilder.Entity<Review>().HasKey(x => new { x.Username }).HasName("review_pkey");
            modelBuilder.Entity<Review>().Property(x => x.Username).HasColumnName("username");
            modelBuilder.Entity<Review>().Property(x => x.TConst).HasColumnName("tconst");
            modelBuilder.Entity<Review>().Property(x => x.Date).HasColumnName("date");
            modelBuilder.Entity<Review>().Property(x => x.Content).HasColumnName("review_content");
            modelBuilder.Entity<Review>().Property(x => x.IsSpoiler).HasColumnName("isspoiler");

        }

    }

}