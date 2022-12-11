using DataLayer.DatabaseModel.WordCloudModel;
using DataLayer.DataServiceInterfaces;
using Microsoft.EntityFrameworkCore;

namespace DataLayer
{
    public class DataserviceWordCloud : IDataserviceWordCloud
    {
        public IList<WordObject>? GetRelatedWordsForWord(string word)
        {
            using var db = new PortfolioDBContext();
            var wordResult = db.WordObjects.FromSqlInterpolated($"select * from word_to_word({word})").ToList();
            return wordResult;
        }

        public IList<WordObject>? GetRelatedWordsForName(string name)
        {
            using var db = new PortfolioDBContext();
            var wordResult = db.WordObjects.FromSqlInterpolated($"select * from person_words_table({name})").ToList();
            return wordResult;
        }
    }
}
