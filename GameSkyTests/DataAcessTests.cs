using Xunit;
using EFDataAccessLibrary.DataAccess;
using EFDataAccessLibrary.Models;
using System.Linq;
using Xunit.Abstractions;
using System.Collections.Generic;
using Moq;
using Microsoft.EntityFrameworkCore;
using System;
using Autofac.Extras.Moq;

namespace GameSkyTests
{
    public class DataAcessTests
    {
        private readonly ITestOutputHelper _output;
        private static bool dbSeeded = false;

        public DataAcessTests(ITestOutputHelper output)
        {
            _output = output;
        }

        #region SimpleTests
        [Fact]
        public void Add_SimpleValuesShouldMatchReturnFalse()
        {
            double expected = 3 + 1;

            double actual = 5;

            Assert.NotEqual(expected, actual);
        }

        [Fact]
        public void Add_SimpleValuesShouldMatchReturnTrue()
        {
            double expected = 3 + 2;

            double actual = 5;

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void DbContextShouldExists()
        {
            var context = GetDbContext();
            Assert.NotNull(context);
        }
#endregion

        #region NewsHeader and NewsContent Tests
        [Fact]
        public void GetFirstNews_ReturnNotNull()
        {
            /* using (var mock = AutoMock.GetLoose())
             {
                 mock.Mock<DataContext>()
                     .Setup(x => x.GetFirstNewsHeader())
                     .Returns(GetFakeData());

                 var cls = mock.Create<DataContext>();
                 var expected = GetFakeData();

                 var actual = cls.GetFirstNewsHeader();

                 Assert.NotNull(actual);
                 Assert.Equal(expected.NewsId, actual.NewsId);
             }*/


            var context = GetDbContext();

            var result = context.GetFirstNewsHeader();
            Assert.NotNull(result);
            Assert.Equal(1, result.NewsId);
        }

        [Fact]
        public void GetNewsList_ShouldReturnAllNews()
        {

            var context = GetDbContext();

            var result = context.GetNewsHeaderList();

            Assert.Equal(6, result.Count);
        }
        [Fact]
        public void GetNewsListInclud_ValidCall()
        {
            var context = GetDbContext();

            var result = context.GetNewsHeaderListIncludeContent();

            int[] expected = {2,3,4,6,8, 12};

            int[] actual = new int[6];

            int i = 0;

            foreach (var news in result)
            {
                if (news.NewsContent != null)
                {
                    int contentId = news.NewsContent.NewsContentId;
                    if (!actual.Contains(contentId))
                        actual[i++] = contentId;
                }
            }

            foreach (int index in expected)
            {
                Assert.Contains(index, actual);
            }
        }

        #endregion

        #region Player and PlayerPosition Tests

        [Fact]
        public void GetPlayerPosition_ValidCall()
        {
            var context = GetDbContext();

            var expected = "AWP";
            var actual = context.GetPlayerPosition(expected);

            Assert.Equal(actual.Name, expected);

        }

        [Fact]
        public void ListOfPlayers_ValidCall()
        {
            var context = GetDbContext();

            var expected = 3;

            var result = context.GetPlayers();

            Assert.Equal(expected, result.Count);
        }

        [Fact]
        public void ObjectPlayerShouldContainsPositionName()
        {
            var context = GetDbContext();

            var result = context.GetPlayersIncludePosition();

            foreach (var player in result)
            {
                Assert.NotNull(player.PlayerPosition.Name);
                Assert.NotEmpty(player.PlayerPosition.Name);
                Assert.NotEqual(String.Empty, player.PlayerPosition.Name);
            }
        }

        #endregion

        #region Team and PlayerTeam Tests
       [Theory]
       [InlineData("x-kom AGO", 2)]
        public List<Player> TakePlayersListShouldMatchExpectedCount(string teamName, int expected)
        {
            var context = GetDbContext();

            var players = context.Players.Include(t => t.Teams).ToList();

            Assert.True(players.Count > 0);

            List<Player> expectedPlayers = new();

            foreach (var player in players)
            {
                foreach (var team in player.Teams)
                {
                    if (team.TeamName.Equals(teamName))
                    {
                        expectedPlayers.Add(player);
                        break;
                    }
                }
            }
            Assert.Equal(expected, expectedPlayers.Count);
            return expectedPlayers;
        }
        
        [Fact]
        public void ShouldPlayerHaveTeamAfterAddingHimToTeam()
        {
            var context = GetDbContext();

            string expectedTeamName = "x-kom AGO";

            var players = context.Players.Include(t => t.Teams).ToList();

            int expected = 2;
            int actual = 0;

            Assert.True(players.Count > 0);

            foreach (var player in players)
            {
                foreach (var team in player.Teams)
                {
                    if (team.TeamName.Equals(expectedTeamName))
                    {
                        actual++;
                    }
                }
            }

            Assert.Equal(expected, actual);

        }

        [Theory]
        [InlineData("G2", 2)]
        public void AddingPlayersToTeamSavesThem(string teamName, int expectedAmount)
        {
            var context = GetDbContext();

            var players2 = TakePlayersListShouldMatchExpectedCount(teamName, expectedAmount);

            var team = context.Teams.Where(x => x.TeamName == teamName).Include(x => x.Players).SingleOrDefault();
            Assert.NotNull(team);
            var player = new Player
            {
                PlayerID = 98,
                FirstName = "Kacper",
                LastName = "Rusztowski",
                NickName = "sn0wgrill",
                BirthDate = DateTime.Now.AddYears(-9),
                Prize = 164f,
                Potencial = 7,
                Aim = 6,
                Knowledge = 4,
                PlayerLevel = 32,
                PlayerPosition = context.PlayerPosition.Where(x => x.Name == "AWP").SingleOrDefault(),
                Teams = new List<Team>(),
            };
            team.Players.Add(player);
            context.Players.Add(player);
            context.Teams.Update(team);
            context.SaveChanges();

            var players3 = TakePlayersListShouldMatchExpectedCount(teamName, ++expectedAmount);

            Assert.True(players2.Count < players3.Count);
            _output.WriteLine("Ilosc przed i po dodaniu: " + players2.Count + " < " + players3.Count);
        }
        #endregion

        #region Fake Data

        private static DataContext GetDbContext()
        {
            var options = new DbContextOptionsBuilder<DataContext>()
                .UseInMemoryDatabase(databaseName: "FakeDb")
                .Options;

            var context = new DataContext(options);

            SeedDbEntities(context);

            return context;
        }
        private static void SeedDbEntities(DataContext context)
        {
            //Seeds Fake DB only once
            if (dbSeeded)
                return;

            var content = new[]
            {
                new NewsContent {NewsContentId = 2, Content = "test content9" },
                new NewsContent {NewsContentId = 3, Content = "test content8" },
                new NewsContent {NewsContentId = 4, Content = "test content7" },
                new NewsContent {NewsContentId = 6, Content = "test content6" },
                new NewsContent {NewsContentId = 8, Content = "test content5" },
                new NewsContent {NewsContentId = 12, Content = "test content2" },
            };
            context.NewsContent.AddRange(content);
            context.SaveChanges();

            var news = new[]
                {
                new NewsHeader{ NewsId = 8, NewsTitle = "Test8", NewsDesc = "test test test8", NewsCreateDate = DateTime.Now, NewsPublishDate = DateTime.Now, isPublished = false, NewsContent = content[0]},
                new NewsHeader{ NewsId = 9, NewsTitle = "Test9", NewsDesc = "test test Test9", NewsCreateDate = DateTime.Now, NewsPublishDate = DateTime.Now, isPublished = false, NewsContent = content[1]},
                new NewsHeader{ NewsId = 1, NewsTitle = "Test10", NewsDesc = "test test Test10", NewsCreateDate = DateTime.Now, NewsPublishDate = DateTime.Now, isPublished = false, NewsContent = content[2]},
                new NewsHeader{ NewsId = 11, NewsTitle = "Test11", NewsDesc = "test test Test11", NewsCreateDate = DateTime.Now, NewsPublishDate = DateTime.Now, isPublished = false, NewsContent = content[3]},
                new NewsHeader{ NewsId = 12, NewsTitle = "Test12", NewsDesc = "test test Test12", NewsCreateDate = DateTime.Now, NewsPublishDate = DateTime.Now, isPublished = false, NewsContent = content[4]},
                new NewsHeader{ NewsId = 13, NewsTitle = "Test13", NewsDesc = "test test Test13", NewsCreateDate = DateTime.Now, NewsPublishDate = DateTime.Now, isPublished = false, NewsContent = content[5]},
            };
            context.NewsHeader.AddRange(news);
            context.SaveChanges();

            var playerPositions = new[]
            {
                new PlayerPosition{ PositionID = 1, Name = "AWP" },
                new PlayerPosition{ PositionID = 2, Name = "Rifler" },
                new PlayerPosition{ PositionID = 3, Name = "IGL" },
            }; 
            context.PlayerPosition.AddRange(playerPositions);
            context.SaveChanges();

            var players = new[]
             {
                new Player{PlayerID = 1, FirstName = "Marcin", LastName = "Klusek", NickName = "fr0y", BirthDate = DateTime.Now.AddYears(-16), Prize = 100f,
                                Potencial = 10, Aim = 10, Knowledge = 10, PlayerLevel = 94, PlayerPosition = playerPositions[0]},
                new Player{PlayerID = 2, FirstName = "Wojtek", LastName = "Sakowicz", NickName = "sako", BirthDate = DateTime.Now.AddYears(-21), Prize = 300f,  
                                Potencial = 10, Aim = 10, Knowledge = 10, PlayerLevel = 94, PlayerPosition = playerPositions[1]},
                new Player{PlayerID = 3, FirstName = "Tomek", LastName = "Cebulski", NickName = "tikson", BirthDate = DateTime.Now.AddYears(-32), Prize = 452f, 
                                Potencial = 10, Aim = 10, Knowledge = 10, PlayerLevel = 94,  PlayerPosition = playerPositions[2]},
            };
            context.Players.AddRange(players);

            var teams = new[]
            {
                new Team{TeamId = 1, TeamName = "x-kom AGO", Tag="AGO", Players = new List<Player>{ players[0], players[1]}},
                new Team{TeamId = 2, TeamName = "G2", Tag="G2", Players = new List<Player>{ players[1], players[2]}},
                new Team{TeamId = 3, TeamName = "piratesports", Tag="ARR", Players = new List<Player>{ players[2], players[0]}},
            };
            context.Teams.AddRange(teams);
            context.SaveChanges();

            dbSeeded = true;
        }

        #endregion
    }
}
