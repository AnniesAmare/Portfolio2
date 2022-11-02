
using System.Collections.Generic;

namespace DataService
{
    public class DataService : IDataService 
    {
        public TitleBasics? GetTitleBasics(int id)
        {
            using var db = new PortfolioDBContext();

            return db.Categories.Find(id);
        }
    }
}