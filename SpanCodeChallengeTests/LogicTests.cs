using Moq;
using SPANCodingChallenge.Data;
using SPANCodingChallenge.Enums;
using SPANCodingChallenge.Logic;
using SPANCodingChallenge.Models;

namespace SpanCodeChallengeTests
{
    [TestClass]
    public sealed class LogicTests        
    {
        [TestMethod]
        public void IsDashboardCommand_Returns_True()
        {
            //Arrange
            var repository = new Mock<ITeamsRepository>();
            var logic = new MatchesLogic(repository.Object);

            //Act
            var result = logic.IsDashboardCommand("DASHBOARD");

            //Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void IsDashboardCommand_Returns_False()
        {
            //Arrange
            var repository = new Mock<ITeamsRepository>();
            var logic = new MatchesLogic(repository.Object);

            //Act
            var result = logic.IsDashboardCommand("Whatever");

            //Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void ToTeams_Success()
        {
            //Arrange
            var repository = new Mock<ITeamsRepository>();
            var logic = new MatchesLogic(repository.Object);

            //Act
            var (team1 , team2, error) = logic.ToTeams("Lions 3, Snakes 3");

            //Assert
            Assert.IsFalse(error);
            Assert.IsTrue(team1.Name.Equals("Lions"));
            Assert.IsTrue(team1.Score == 3);
            Assert.IsTrue(team2.Name.Equals("Snakes"));
            Assert.IsTrue(team2.Score == 3);
        }
        
        [TestMethod]
        public void ToTeams_Error()
        {
            //Arrange
            var repository = new Mock<ITeamsRepository>();            
            var logic = new MatchesLogic(repository.Object);

            //Act
            var (team1, team2, error) = logic.ToTeams("Lions , Snakes 3");

            //Assert
            Assert.IsTrue(error);;
        }

        [TestMethod]
        public void SavePoints_Draw()
        {
            //Arrange
            var repository = new Mock<ITeamsRepository>();
            var logic = new MatchesLogic(repository.Object);
            var team1 = new Team
            {
                Name = "Lions",
                Score = 3,
            };

            var team2= new Team
            {
                Name = "Snakes",
                Score = 3,
            };


            //Act
            logic.SavePoints(team1, team2);

            //Assert
            repository.Verify(x => x.UpsertTeam(team1, PointsEnum.DRAW), Times.Once);
            repository.Verify(x => x.UpsertTeam(team2, PointsEnum.DRAW), Times.Once);
        }

        [TestMethod]
        public void SavePoints_Win()
        {
            //Arrange
            var repository = new Mock<ITeamsRepository>();
            var logic = new MatchesLogic(repository.Object);
            var team1 = new Team
            {
                Name = "Lions",
                Score = 3,
            };

            var team2 = new Team
            {
                Name = "Snakes",
                Score = 2,
            };


            //Act
            logic.SavePoints(team1, team2);

            //Assert
            repository.Verify(x => x.UpsertTeam(team1, PointsEnum.WIN), Times.Once);
            repository.Verify(x => x.UpsertTeam(team2, PointsEnum.LOSE), Times.Once);
        }

        [TestMethod]
        public void OrderDashboard_Success()
        {
            //Arrange
            var repository = new Mock<ITeamsRepository>();
            var teamsDic = new Dictionary<string, int>()
            {
                {"Lions", 0 },
                {"Snakes", 3 },
                {"Grouches", 9},
                {"FC Awesome", 6 }
            };
            repository.Setup(x => x.GetTeamsData()).Returns(teamsDic);
            var logic = new MatchesLogic(repository.Object);


            //Act
            var result = logic.OrderDashboard();

            //Assert
            var topTeam = result.First();
            var loserTeam = result.Last();
            Assert.IsTrue(topTeam.Key.Equals("Grouches"));
            Assert.IsTrue(loserTeam.Key.Equals("Lions"));
        }

    }
}
