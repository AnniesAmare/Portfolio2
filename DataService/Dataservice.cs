
using DataLayer.DatabaseModel;

using System.Collections.Generic;

namespace DataLayer
{
    public class DataService : IDataService 
    {

        public IList<TitleBasics> GetTitleBasics()
        {
            using var db = new PortfolioDBContext();

            return db.TitleBasic.ToList();
        }

        /*
        public TitleBasics GetTitleBasics(int id)
        {
            using var db = new PortfolioDBContext();

            return db.TitleBasic.Find(id);
        }*/
    }
}