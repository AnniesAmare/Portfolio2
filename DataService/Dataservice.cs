
using DataLayer.DatabaseModel;

using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace DataLayer
{
    public class DataService : IDataService 
    {

        public IList<TitleBasic> GetTitleBasics()
        {
            using var db = new PortfolioDBContext();

            return db.TitleBasics.ToList();
        }

        /*
        public IList<NameBasic> GetNameBasics()
        {
            using var db = new PortfolioDBContext();

            return db.NameBasics.ToList();
        }
        */
        public IList<TitleAka> GetTitleAkas()
        {
            using var db = new PortfolioDBContext();
            var titleAka = db
                .TitleAkas
                .ToList();
                
            return titleAka;
        }




        /*
        public TitleBasic GetTitleBasics(int id)
        {
            using var db = new PortfolioDBContext();

            return db.TitleBasics.Find(id);
        }*/
    }
}