
using DataLayer.DatabaseModel;

using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace DataLayer
{
    public class DataService : IDataService 
    {

        public IList<TitleBasics> GetTitleBasics()
        {
            using var db = new PortfolioDBContext();

            return db.TitleBasics.ToList();
        }

        public IList<TitleAkas> GetTitleAkas()
        {
            using var db = new PortfolioDBContext();
            var titleAka = db.TitleAkas
                //.Include(x => x.Attributes)
                .ToList();
                
            return titleAka;
        }




        /*
        public TitleBasics GetTitleBasics(int id)
        {
            using var db = new PortfolioDBContext();

            return db.TitleBasics.Find(id);
        }*/
    }
}