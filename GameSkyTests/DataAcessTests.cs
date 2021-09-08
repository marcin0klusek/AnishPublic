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
using System.Threading.Tasks;

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
        public async Task GetFirstNews_ReturnNotNull()
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

            NewsHeader result = await context.GetFirstNewsHeader();
            Assert.NotNull(result);
            Assert.Equal(1, result.NewsId);
        }

       [Fact]
        public async Task GetNewsList_ShouldReturnAllNews()
        {

            var context = GetDbContext();

            List<NewsHeader> result = await context.GetNewsHeaderList();

            Assert.Equal(6, result.Count);
        }
        [Fact]
        public void GetNewsListInclud_ValidCall()
        {
            var context = GetDbContext();

            var result = context.GetNewsHeaderListIncludeContent().Result;

            int[] expected = {1,2,3,4,5,6};

            int[] actual = new int[6];

            int i = 0;

            foreach (var news in result)
            {
                if (news.NewsContent != null)
                {
                    int contentId = news.NewsContent.NewsContentID;
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
            PlayerPosition actual = context.GetPositionByName(expected).Result;

            Assert.Equal(actual.Name, expected);

        }

        [Fact]
        public void ListOfPlayers_ValidCall()
        {
            var context = GetDbContext();

            var expected = 3;

            List<Player> result = context.GetPlayers().Result;

            Assert.Equal(expected, result.Count);
        }

        [Fact]
        public void ObjectPlayerShouldContainsPositionName()
        {
            var context = GetDbContext();

            var result = context.GetPlayersIncludePosition().Result;

            foreach (var player in result)
            {
                Assert.NotNull(player.PlayerPosition.Name);
                Assert.NotEmpty(player.PlayerPosition.Name);
                Assert.NotEqual(String.Empty, player.PlayerPosition.Name);
            }
        }

        #endregion

        #region Team and PlayerTeam Tests
        //removed due to error when chaning db
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
                new NewsContent {Content = "test content9" },
                new NewsContent {Content = "test content8" },
                new NewsContent {Content = "test content7" },
                new NewsContent {Content = "test content6" },
                new NewsContent {Content = "test content5" },
                new NewsContent {Content = "test content2" },
            };
            context.NewsContent.AddRange(content);
            context.SaveChanges();

            var news = new[]
                {
                new NewsHeader{ NewsTitle = "Test8", NewsDesc = "test test test8", NewsCreateDate = DateTime.Now, NewsPublishDate = DateTime.Now, IsPublished = false, NewsContent = content[0]},
                new NewsHeader{ NewsTitle = "Test9", NewsDesc = "test test Test9", NewsCreateDate = DateTime.Now, NewsPublishDate = DateTime.Now, IsPublished = false, NewsContent = content[1]},
                new NewsHeader{ NewsTitle = "Test10", NewsDesc = "test test Test10", NewsCreateDate = DateTime.Now, NewsPublishDate = DateTime.Now, IsPublished = false, NewsContent = content[2]},
                new NewsHeader{ NewsTitle = "Test11", NewsDesc = "test test Test11", NewsCreateDate = DateTime.Now, NewsPublishDate = DateTime.Now, IsPublished = false, NewsContent = content[3]},
                new NewsHeader{ NewsTitle = "Test12", NewsDesc = "test test Test12", NewsCreateDate = DateTime.Now, NewsPublishDate = DateTime.Now, IsPublished = false, NewsContent = content[4]},
                new NewsHeader{ NewsTitle = "Test13", NewsDesc = "test test Test13", NewsCreateDate = DateTime.Now, NewsPublishDate = DateTime.Now, IsPublished = false, NewsContent = content[5]},
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
                new Player{FirstName = "Marcin", LastName = "Klusek", NickName = "fr0y", BirthDate = DateTime.Now.AddYears(-16), Prize = 100f,
                                Potencial = 10, Aim = 10, Knowledge = 10, PlayerLevel = 94, PositionID = context.GetPositionByName("AWP").Result.PositionID, PlayerPosition = context.GetPositionByName("AWP").Result},
                new Player{FirstName = "Wojtek", LastName = "Sakowicz", NickName = "sako", BirthDate = DateTime.Now.AddYears(-21), Prize = 300f,  
                                Potencial = 10, Aim = 10, Knowledge = 10, PlayerLevel = 94, PositionID = context.GetPositionByName("IGL").Result.PositionID, PlayerPosition = context.GetPositionByName("IGL").Result},
                new Player{FirstName = "Tomek", LastName = "Cebulski", NickName = "tikson", BirthDate = DateTime.Now.AddYears(-32), Prize = 452f, 
                                Potencial = 10, Aim = 10, Knowledge = 10, PlayerLevel = 94,  PositionID = context.GetPositionByName("Rifler").Result.PositionID, PlayerPosition = context.GetPositionByName("Rifler").Result},
            };
            context.Player.AddRange(players);

            var teams = new[]
            {
                new Team{TeamName = "x-kom AGO", Tag="AGO"},
                new Team{TeamName = "G2", Tag="G2"},
                new Team{TeamName = "piratesports", Tag="ARR"},
            };
            context.Team.AddRange(teams);
            context.SaveChanges();

            dbSeeded = true;
        }

        #endregion
    }
}
