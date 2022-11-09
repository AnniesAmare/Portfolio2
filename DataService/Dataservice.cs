using DataLayer.DatabaseModel;
using DataLayer.DataTransferModel;
using Microsoft.EntityFrameworkCore;
using System.Xml.Linq;

namespace DataLayer
{
    public class DataService : IDataService
    {
        public IList<TitleBasic> GetTitleBasics()
        {
            using var db = new PortfolioDBContext();
            return db.TitleBasics.ToList();
        }

    }
}