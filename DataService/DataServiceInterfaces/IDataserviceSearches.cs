using DataLayer.DatabaseModel.SearchModel;
using DataLayer.DataTransferModel;

namespace DataLayer.DataServiceInterfaces
{
    public interface IDataserviceSearches
    {
        IList<PersonSearch>? GetSearchResultActors(string username, string search);
        (IList<PersonSearch>? searchResult, int total ) GetSearchResultActors(string username, string search, int page, int pageSize);
        IList<TitleSearch>? GetSearchResultTitles(string username, string search);
        (IList<TitleSearch>? searchResult, int total ) GetSearchResultTitles(string username, string search, int page, int pageSize);
        IList<TitleSearch>? GetSearchResultGenres(string username, string search);
        (IList<TitleSearch>? searchResult, int total) GetSearchResultGenres(string username, string search, int page, int pageSize);

        bool ClearSearchHistory(string username);
    }

}

