namespace Portfolio2.Tests
{
    public class DataServiceTests
    {
        [Fact]
        public void GetSpecificTitleReturnsCompleteRecordOnValidTConst()
        {
            var service = new DataService();
            var specificTitle = service.GetSpecificTitle("tt0052520");
            Assert.Equal("The Twilight Zone", specificTitle.Title);
            Assert.Equal(53, specificTitle.DirectorList.Count);
            Assert.Contains("Abner Biberman", specificTitle.DirectorList.First().Name);
            Assert.Equal(30, specificTitle.ActorList.Count);
            Assert.Contains("Burgess Meredith", specificTitle.ActorList.First().Name);
            Assert.Contains("Horror", specificTitle.Genre.Last());
        }
    }
}