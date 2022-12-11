using DataLayer.DatabaseModel.WordCloudModel;

namespace DataLayer.DataServiceInterfaces;

public interface IDataserviceWordCloud
{
    IList<WordObject>? GetRelatedWordsForWord(string word);

    IList<WordObject>? GetRelatedWordsForName(string name);

}