using DataLayer.DatabaseModel.SearchModel;
using DataLayer.DataTransferModel;

namespace DataLayer.DataServiceInterfaces
{
    public interface IDataserviceSearches
    {
        IList<PersonSearch>? GetSearchResultActors(string username, string search);
        IList<TitleSearch>? GetSearchResultTitles(string username, string search);
        IList<TitleSearch>? GetSearchResultGenres(string username, string search);

        bool ClearSearchHistory(string username);
    }

}

