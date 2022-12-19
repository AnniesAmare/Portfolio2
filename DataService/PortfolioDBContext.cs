using System.Reflection.Emit;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataLayer.DatabaseModel;
using DataLayer.DatabaseModel.MovieModel;
using DataLayer.DatabaseModel.SearchModel;
using DataLayer.DatabaseModel.WordCloudModel;
using Microsoft.Extensions.Logging;

namespace DataLayer
{

    public class PortfolioDBContext : DbContext
    {
        //ruc server 
        const string ConnectionString = "host=cit.ruc.dk;db=cit11;uid=cit11;pwd=nICrojAxtDeX";

        //siemje - localhost database
        //const string ConnectionString = "host=localhost;db=imdb;uid=postgres;pwd=postgres";

        //atru - localhost database
        //const string ConnectionString = "host=localhost;db=imdb;uid=postgres;pwd=Bqm33etj";

        /* MOVIE MODEL */
        //TITLEBASICS
        public DbSet<TitleBasic> TitleBasics { get; set; }
        public DbSet<TitleEpisode> TitleEpisodes { get; set; }
        public DbSet<TitleRating> TitleRatings { get; set; }
        public DbSet<Genre> Genres { get; set; }
        public DbSet<OmdbData> OmdbDatas { get; set; }
        public DbSet<Wi> Wis { get; set; }

        //INBETWEEN TITLE_ AND NAME_BASICS
        public DbSet<KnownFor> KnownFors { get; set; }
        public DbSet<TitlePrincipal> TitlePrincipals { get; set; }
        public DbSet<Job> Jobs { get; set; }
        public DbSet<Character> Characters { get; set; }

        //NAMEBASICS
        public DbSet<NameBasic> NameBasics { get; set; }
        public DbSet<Profession> Professions { get; set; }

        //TITLE AKAS
        public DbSet<Attributes> Attributes { get; set; }
        public DbSet<Language> Languages { get; set; }
        public DbSet<TType> Types { get; set; }
        public DbSet<TitleAka> TitleAkas { get; set; }

        /* USER FRAMEWORK */
        public DbSet<User>? Users { get; set; }
        public DbSet<UserRating>? UserRatings { get; set; }
        public DbSet<UserSearch>? UserSearches { get; set; }
        public DbSet<BookmarkTitle>? BookmarksTitles { get; set; }
        public DbSet<BookmarkName>? BookmarksNames { get; set; }
        public DbSet<Review>? Reviews { get; set; }

        /* SEARCH FRAMEWORK */
        public DbSet<PersonSearch> PersonSearches { get; set; }
        public DbSet<TitleSearch> TitleSearches { get; set; }

        /* WORD CLOUD FRAMEWORK */
        public DbSet<WordObject> WordObjects { get; set; }

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

            // MOVIE MODEL 
            /* TITLEBASICS & DEPENDENCIES*/
            //TITLEBASICS MAPPING
            modelBuilder.Entity<TitleBasic>().ToTable("title_basic");
            modelBuilder.Entity<TitleBasic>().HasKey(x => new { x.TConst }).HasName("title_basic_pkey");
            modelBuilder.Entity<TitleBasic>().Property(x => x.TConst).HasColumnName("tconst");
            modelBuilder.Entity<TitleBasic>().Property(x => x.TitleType).HasColumnName("titletype");
            modelBuilder.Entity<TitleBasic>().Property(x => x.PrimaryTitle).HasColumnName("primarytitle");
            modelBuilder.Entity<TitleBasic>().Property(x => x.OriginalTitle).HasColumnName("originaltitle");
            modelBuilder.Entity<TitleBasic>().Property(x => x.IsAdult).HasColumnName("isadult");
            modelBuilder.Entity<TitleBasic>().Property(x => x.StartYear).HasColumnName("startyear");
            modelBuilder.Entity<TitleBasic>().Property(x => x.EndYear).HasColumnName("endyear");
            modelBuilder.Entity<TitleBasic>().Property(x => x.RuntimeMinutes).HasColumnName("runtimeminutes");
            modelBuilder.Entity<TitleBasic>().Property(x => x.IsTvShow).HasColumnName("istvshow");
            modelBuilder.Entity<TitleBasic>().Property(x => x.IsMovie).HasColumnName("ismovie");
            modelBuilder.Entity<TitleBasic>().Property(x => x.IsEpisode).HasColumnName("isepisode");

            //TITLE_EPISODE MAPPING
            modelBuilder.Entity<TitleEpisode>().ToTable("title_episode");
            modelBuilder.Entity<TitleEpisode>().HasKey(x => new { x.TConst }).HasName("title_episode_pkey");
            modelBuilder.Entity<TitleEpisode>().Property(x => x.TConst).HasColumnName("tconst");
            modelBuilder.Entity<TitleEpisode>().Property(x => x.ParentTConst).HasColumnName("parenttconst");
            modelBuilder.Entity<TitleEpisode>().Property(x => x.SeasonNumber).HasColumnName("seasonnumber");
            modelBuilder.Entity<TitleEpisode>().Property(x => x.EpisodeNumber).HasColumnName("episodenumber");

            //TITLE_RATINGS
            modelBuilder.Entity<TitleRating>().ToTable("title_rating");
            modelBuilder.Entity<TitleRating>().HasKey(x => new { x.TConst }).HasName("title_rating_pkey");
            modelBuilder.Entity<TitleRating>()
              .HasOne(x => x.TitleBasic)
              .WithOne(x => x.TitleRating)
              .HasForeignKey<TitleRating>(x => x.TConst);
            modelBuilder.Entity<TitleRating>().Property(x => x.TConst).HasColumnName("tconst");
            modelBuilder.Entity<TitleRating>().Property(x => x.AverageRating).HasColumnName("averagerating");
            modelBuilder.Entity<TitleRating>().Property(x => x.NumVotes).HasColumnName("numvotes");

            //GENRES
            modelBuilder.Entity<Genre>().ToTable("genre");
            modelBuilder.Entity<Genre>().HasKey(x => new { x.TConst }).HasName("genre_pkey");
            modelBuilder.Entity<Genre>()
                .HasOne(x => x.TitleBasic)
                .WithMany(x => x.Genre)
                .HasForeignKey(x => x.TConst);
            modelBuilder.Entity<Genre>().Property(x => x.TConst).HasColumnName("tconst");
            modelBuilder.Entity<Genre>().Property(x => x.TGenre).HasColumnName("genre");

            //OMDB_DATA
            modelBuilder.Entity<OmdbData>().ToTable("omdb_data");
            modelBuilder.Entity<OmdbData>().HasKey(x => new { x.TConst }).HasName("omdb_data_pkey");
            modelBuilder.Entity<OmdbData>()
              .HasOne(x => x.TitleBasic)
              .WithOne(x => x.OmdbData)
              .HasForeignKey<OmdbData>(x => new { x.TConst });
            modelBuilder.Entity<OmdbData>().Property(x => x.TConst).HasColumnName("tconst");
            modelBuilder.Entity<OmdbData>().Property(x => x.Poster).HasColumnName("poster");
            modelBuilder.Entity<OmdbData>().Property(x => x.Plot).HasColumnName("plot");

            //WI
            modelBuilder.Entity<Wi>().ToTable("wi");
            modelBuilder.Entity<Wi>().HasKey(x => new { x.TConst }).HasName("wi_pkey");
            modelBuilder.Entity<Wi>().Property(x => x.TConst).HasColumnName("tconst");
            modelBuilder.Entity<Wi>().Property(x => x.Word).HasColumnName("word");
            modelBuilder.Entity<Wi>().Property(x => x.Field).HasColumnName("field");
            modelBuilder.Entity<Wi>().Property(x => x.Lexeme).HasColumnName("lexeme");

            /* NAMEBASICS & DEPENDENCIES */
            //NAMEBASICS MAPPING
            modelBuilder.Entity<NameBasic>().ToTable("name_basic");
            modelBuilder.Entity<NameBasic>().HasKey(x => new { x.NConst }).HasName("name_basic_pkey");
            modelBuilder.Entity<NameBasic>().Property(x => x.NConst).HasColumnName("nconst");
            modelBuilder.Entity<NameBasic>().Property(x => x.PrimaryName).HasColumnName("primaryname");
            modelBuilder.Entity<NameBasic>().Property(x => x.BirthYear).HasColumnName("birthyear");
            modelBuilder.Entity<NameBasic>().Property(x => x.DeathYear).HasColumnName("deathyear");
            modelBuilder.Entity<NameBasic>().Property(x => x.AVGNameRating).HasColumnName("avg_name_rating");
            modelBuilder.Entity<NameBasic>().Property(x => x.IsActor).HasColumnName("isactor");

            //PROFESSIONS
            modelBuilder.Entity<Profession>().ToTable("profession");
            modelBuilder.Entity<Profession>().HasKey(x => new { x.NConst }).HasName("profession_pkey");
            modelBuilder.Entity<Profession>().Property(x => x.NConst).HasColumnName("nconst");
            modelBuilder.Entity<Profession>().Property(x => x.TProfession).HasColumnName("profession");

            /* INBETWEEN TITLE_ AND NAME_BASICS */
            //KNOWN_FOR
            modelBuilder.Entity<KnownFor>().ToTable("known_for");
            modelBuilder.Entity<KnownFor>().HasKey(x => new { x.TConst, x.NConst }).HasName("known_for_pkey");
            modelBuilder.Entity<KnownFor>().Property(x => x.TConst).HasColumnName("tconst");
            modelBuilder.Entity<KnownFor>().Property(x => x.NConst).HasColumnName("nconst");
            modelBuilder.Entity<KnownFor>()
                .HasOne(x => x.NameBasic)
                .WithMany(x => x.KnownFor)
                .HasForeignKey(x => x.NConst);
            modelBuilder.Entity<KnownFor>()
                .HasOne(x => x.TitleBasic)
                .WithMany(x => x.KnownFor)
                .HasForeignKey(x => x.TConst);

            //TITLE_PRINCIPALS
            modelBuilder.Entity<TitlePrincipal>().ToTable("title_principal");
            modelBuilder.Entity<TitlePrincipal>().HasKey(x => new { x.TConst, x.NConst }).HasName("title_principal_pkey");
            modelBuilder.Entity<TitlePrincipal>()
                .HasOne(x => x.NameBasic)
                .WithMany(x => x.TitlePrincipal)
                .HasForeignKey(x => x.NConst);
            modelBuilder.Entity<TitlePrincipal>()
                .HasOne(x => x.TitleBasic)
                .WithMany(x => x.TitlePrincipal)
                .HasForeignKey(x => x.TConst);
            modelBuilder.Entity<TitlePrincipal>().Property(x => x.TConst).HasColumnName("tconst");
            modelBuilder.Entity<TitlePrincipal>().Property(x => x.NConst).HasColumnName("nconst");
            modelBuilder.Entity<TitlePrincipal>().Property(x => x.Category).HasColumnName("category");

            //JOBS
            modelBuilder.Entity<Job>().ToTable("job");
            modelBuilder.Entity<Job>().HasKey(x => new { x.TConst, x.NConst }).HasName("job_pkey");
            modelBuilder.Entity<Job>().Property(x => x.TConst).HasColumnName("tconst");
            modelBuilder.Entity<Job>().Property(x => x.NConst).HasColumnName("nconst");
            modelBuilder.Entity<Job>().Property(x => x.TJob).HasColumnName("job");

            //Characters
            modelBuilder.Entity<Character>().ToTable("character");
            modelBuilder.Entity<Character>().HasKey(x => new { x.TConst, x.NConst }).HasName("character_pkey");
            modelBuilder.Entity<Character>()
                .HasOne(x => x.NameBasic)
                .WithMany(x => x.Character)
                .HasForeignKey(x => x.NConst);
            modelBuilder.Entity<Character>()
                .HasOne(x => x.TitleBasic)
                .WithMany(x => x.Character)
                .HasForeignKey(x => x.TConst);
            modelBuilder.Entity<Character>().Property(x => x.TConst).HasColumnName("tconst");
            modelBuilder.Entity<Character>().Property(x => x.NConst).HasColumnName("nconst");
            modelBuilder.Entity<Character>().Property(x => x.TCharacter).HasColumnName("character");


            /* TITLE AKAS & DEPENDENCIES */
            //TITLEAKAS
            modelBuilder.Entity<TitleAka>().ToTable("title_aka");
            modelBuilder.Entity<TitleAka>().HasKey(x => new { x.TConst, x.Ordering }).HasName("title_aka_pkey");
            modelBuilder.Entity<TitleAka>().Property(x => x.TConst).HasColumnName("tconst");
            modelBuilder.Entity<TitleAka>().Property(x => x.Ordering).HasColumnName("ordering");
            modelBuilder.Entity<TitleAka>().Property(x => x.Title).HasColumnName("title");
            modelBuilder.Entity<TitleAka>().Property(x => x.Region).HasColumnName("region");
            modelBuilder.Entity<TitleAka>().Property(x => x.IsOriginalTitle).HasColumnName("isoriginaltitle");

            //ATTRIBUTES MAPPING
            modelBuilder.Entity<Attributes>().ToTable("attributes");
            modelBuilder.Entity<Attributes>().HasKey(x => new { x.TConst, x.Ordering }).HasName("attributes_pkey");
            modelBuilder.Entity<Attributes>()
                .HasOne(x => x.TitleAka)
                .WithOne(x => x.Attributes)
                .HasForeignKey<TitleAka>(x => new { x.TConst, x.Ordering });
            modelBuilder.Entity<Attributes>().Property(x => x.TConst).HasColumnName("tconst");
            modelBuilder.Entity<Attributes>().Property(x => x.Ordering).HasColumnName("ordering");
            modelBuilder.Entity<Attributes>().Property(x => x.Attribute).HasColumnName("attributes");

            //LANGUAGES MAPPING
            modelBuilder.Entity<Language>().ToTable("language");
            modelBuilder.Entity<Language>().HasKey(x => new { x.TConst, x.Ordering }).HasName("language_pkey");
            modelBuilder.Entity<Language>()
                .HasOne(x => x.TitleAka)
                .WithOne(x => x.Language)
                .HasForeignKey<TitleAka>(x => new { x.TConst, x.Ordering });
            modelBuilder.Entity<Language>().Property(x => x.TConst).HasColumnName("tconst");
            modelBuilder.Entity<Language>().Property(x => x.Ordering).HasColumnName("ordering");
            modelBuilder.Entity<Language>().Property(x => x.TLanguage).HasColumnName("language");

            //ATTRIBUTES MAPPING
            modelBuilder.Entity<TType>().ToTable("type");
            modelBuilder.Entity<TType>().HasKey(x => new { x.TConst, x.Ordering }).HasName("type_pkey");
            modelBuilder.Entity<TType>()
                .HasOne(x => x.TitleAka)
                .WithOne(x => x.TType)
                .HasForeignKey<TitleAka>(x => new { x.TConst, x.Ordering });
            modelBuilder.Entity<TType>().Property(x => x.TConst).HasColumnName("tconst");
            modelBuilder.Entity<TType>().Property(x => x.Ordering).HasColumnName("ordering");
            modelBuilder.Entity<TType>().Property(x => x.Type).HasColumnName("types");

            /* USER FRAMEWORK */
            //USERS
            modelBuilder.Entity<User>().ToTable("users");
            modelBuilder.Entity<User>().HasKey(x => new { x.Username }).HasName("users_pkey");
            modelBuilder.Entity<User>().Property(x => x.Username).HasColumnName("username");
            modelBuilder.Entity<User>().Property(x => x.Password).HasColumnName("password");
            modelBuilder.Entity<User>().Property(x => x.Salt).HasColumnName("salt");
            modelBuilder.Entity<User>().Property(x => x.BirthYear).HasColumnName("birthyear");
            modelBuilder.Entity<User>().Property(x => x.Email).HasColumnName("email");

            //USER RATING
            modelBuilder.Entity<UserRating>().ToTable("user_rating");
            modelBuilder.Entity<UserRating>().HasKey(x => new { x.Username }).HasName("user_rating_pkey");
            modelBuilder.Entity<UserRating>().Property(x => x.Username).HasColumnName("username");
            modelBuilder.Entity<UserRating>().Property(x => x.Date).HasColumnName("date");
            modelBuilder.Entity<UserRating>().Property(x => x.TConst).HasColumnName("tconst");
            modelBuilder.Entity<UserRating>().Property(x => x.Rating).HasColumnName("rating");
            modelBuilder.Entity<UserRating>()
                .HasOne(x => x.User)
                .WithMany(x => x.UserRating)
                .HasForeignKey(x => x.Username);
            modelBuilder.Entity<UserRating>()
                .HasOne(x => x.TitleBasic)
                .WithMany(x => x.UserRating)
                .HasForeignKey(x => x.TConst);

            //modelBuilder.Entity<BookmarkTitle>()
            //  .HasOne(x => x.User)
            //  .WithMany(x => x.BookmarkTitle)
            //  .HasForeignKey(x => x.Username);
            //modelBuilder.Entity<BookmarkTitle>()
            //    .HasOne(x => x.TitleBasic)
            //    .WithMany(x => x.BookmarkTitle)
            //    .HasForeignKey(x => x.TConst);

            //USER SEARCH
            modelBuilder.Entity<UserSearch>().ToTable("user_search");
            modelBuilder.Entity<UserSearch>().HasKey(x => new { x.Username }).HasName("user_search_pkey");
            modelBuilder.Entity<UserSearch>().Property(x => x.Username).HasColumnName("username");
            modelBuilder.Entity<UserSearch>().Property(x => x.SearchId).HasColumnName("search_id");
            modelBuilder.Entity<UserSearch>().Property(x => x.Date).HasColumnName("date");
            modelBuilder.Entity<UserSearch>().Property(x => x.Content).HasColumnName("search_content");
            modelBuilder.Entity<UserSearch>().Property(x => x.Category).HasColumnName("search_category");
            modelBuilder.Entity<UserSearch>()
                .HasOne(x => x.User)
                .WithMany(x => x.UserSearch)
                .HasForeignKey(x => x.Username);

            //BOOKMARK NAME
            modelBuilder.Entity<BookmarkName>().ToTable("bookmark_name");
            modelBuilder.Entity<BookmarkName>().HasKey(x => new { x.Username, x.NConst }).HasName("bookmark_name_pkey");
            modelBuilder.Entity<BookmarkName>().Property(x => x.Username).HasColumnName("username");
            modelBuilder.Entity<BookmarkName>().Property(x => x.NConst).HasColumnName("nconst");
            modelBuilder.Entity<BookmarkName>().Property(x => x.Annotation).HasColumnName("annotation");
            modelBuilder.Entity<BookmarkName>()
                .HasOne(x => x.User)
                .WithMany(x => x.BookmarkName)
                .HasForeignKey(x => x.Username);
            modelBuilder.Entity<BookmarkName>()
                .HasOne(x => x.NameBasic)
                .WithMany(x => x.BookmarkName)
                .HasForeignKey(x => x.NConst);

            //BOOKMARK TITLE
            modelBuilder.Entity<BookmarkTitle>().ToTable("bookmark_title");
            modelBuilder.Entity<BookmarkTitle>().HasKey(x => new { x.Username, x.TConst }).HasName("bookmark_title_pkey");
            modelBuilder.Entity<BookmarkTitle>().Property(x => x.Username).HasColumnName("username");
            modelBuilder.Entity<BookmarkTitle>().Property(x => x.TConst).HasColumnName("tconst");
            modelBuilder.Entity<BookmarkTitle>().Property(x => x.Annotation).HasColumnName("annotation");
            modelBuilder.Entity<BookmarkTitle>()
                .HasOne(x => x.User)
                .WithMany(x => x.BookmarkTitle)
                .HasForeignKey(x => x.Username);
            modelBuilder.Entity<BookmarkTitle>()
                .HasOne(x => x.TitleBasic)
                .WithMany(x => x.BookmarkTitle)
                .HasForeignKey(x => x.TConst);

            //REVIEW
            modelBuilder.Entity<Review>().ToTable("review");
            modelBuilder.Entity<Review>().HasKey(x => new { x.Username }).HasName("review_pkey");
            modelBuilder.Entity<Review>().Property(x => x.Username).HasColumnName("username");
            modelBuilder.Entity<Review>().Property(x => x.TConst).HasColumnName("tconst");
            modelBuilder.Entity<Review>().Property(x => x.Date).HasColumnName("date");
            modelBuilder.Entity<Review>().Property(x => x.Content).HasColumnName("review_content");
            modelBuilder.Entity<Review>().Property(x => x.IsSpoiler).HasColumnName("isspoiler");
            modelBuilder.Entity<Review>()
                .HasOne(x => x.User)
                .WithMany(x => x.Review)
                .HasForeignKey(x => x.Username);


            /* SEARCH FRAMEWORK */
            //PERSON SEARCH
            modelBuilder.Entity<PersonSearch>().HasNoKey();
            modelBuilder.Entity<PersonSearch>().Property(x => x.NConst).HasColumnName("nconst");
            modelBuilder.Entity<PersonSearch>().Property(x => x.PrimaryName).HasColumnName("primaryname");

            //TITLE SEARCH
            modelBuilder.Entity<TitleSearch>().HasNoKey();
            modelBuilder.Entity<TitleSearch>().Property(x => x.TConst).HasColumnName("tconst");
            modelBuilder.Entity<TitleSearch>().Property(x => x.Rank).HasColumnName("rank");
            modelBuilder.Entity<TitleSearch>().Property(x => x.PrimaryTitle).HasColumnName("primarytitle");

            /* WORD CLOUD FRAMEWORK */
            //WORD OBJECT
            modelBuilder.Entity<WordObject>().HasNoKey();
            modelBuilder.Entity<WordObject>().Property(x => x.Word).HasColumnName("word");
            modelBuilder.Entity<WordObject>().Property(x => x.Rank).HasColumnName("rank");

        }

    }

}