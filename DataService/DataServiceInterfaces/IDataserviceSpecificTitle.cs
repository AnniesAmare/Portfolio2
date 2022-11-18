using DataLayer.DataTransferModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer
{
    public interface IDataserviceSpecificTitle
    {
        //SPECIFIC TITLE METHODS
        SpecificTitle GetSpecificTitleByName(string name);
        SpecificTitle GetSpecificTitle(string tConst);
        IList<TitlePersons> GetTitleCastById(string tConst);
        IList<TitlePersons> GetTitleCrewById(string tConst);
        Titles GetTvShowById(string TConst);

    }
}
