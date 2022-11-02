using System.Reflection.Emit;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataService;

public class PortfolioDBContext : DbContext 
{
    //siemje db
    const string ConnectionString = "host=cit.ruc.dk;db=cit01;uid=postgres;pwd=postgres";
    //atru db
    //const string ConnectionString = "host=cit.ruc.dk;db=cit01;uid=postgres;pwd=postgres";
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        base.OnConfiguring(optionsBuilder);
        //optionsBuilder.LogTo(Console.WriteLine, LogLevel.Information);
        optionsBuilder.UseNpgsql(ConnectionString);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        //TileBasics mapping
        modelBuilder.Entity<TitleBasics>().ToTable("categories");
        //modelBuilder.Entity<Category>().Property(x => x.Id).HasColumnName("categoryid");

    }

}