using DataLayer.DatabaseModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer
{
    public interface IDataService
    {
        IList<TitleBasics> GetTitleBasics();
        //TitleBasics GetTitleBasics(int id);
        IList<TitleAkas> GetTitleAkas();

    }
}
