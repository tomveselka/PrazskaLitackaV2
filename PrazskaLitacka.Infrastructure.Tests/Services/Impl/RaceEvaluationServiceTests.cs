using Microsoft.Extensions.Logging;
using Moq;
using PrazskaLitacka.Domain.Entities;
using PrazskaLitacka.Domain.Interfaces;
using PrazskaLitacka.Infrastructure.Services.Impl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrazskaLitacka.Infrastructure.Tests.Services.Impl;
public class RaceEvaluationServiceTests
{
    private readonly Mock<IPointsRepository> _pointsRepositoryMock;
    private readonly Mock<IRaceRepository> _raceRepositoryMock;
    private readonly Mock<ILogger<RaceEvaluationService>> _loggerMock;

    private readonly RaceEvaluationService _sut;

    public RaceEvaluationServiceTests()
    {
        _pointsRepositoryMock = new Mock<IPointsRepository>();
        _raceRepositoryMock = new Mock<IRaceRepository>();
        _loggerMock = new Mock<ILogger<RaceEvaluationService>>();
        _sut = new RaceEvaluationService(_pointsRepositoryMock.Object, _raceRepositoryMock.Object, _loggerMock.Object);
    }
    
    private RaceEntry raceEntry = new RaceEntry()
    {
        Id = 1,
        RacerId = 1,
        RaceId = 5,
        TimeOfReturn = new DateTimeOffset(2025, 10, 10, 19, 00, 00, TimeSpan.Zero),
        Rows = new List<Row>()
        {
            new Row()
            {
                Id = 1,
                StationFromName = "StationFrom1",
                StationToName = "StationTo1",
                LineName = "Metro1",
                LineType = "metro",
            }
        }
    };
    
    private List<Points> pointsList = new List<Points>()
        {
            new Points()
            {
                Id = 1,
                Name = "metro",
                PointsValue = 2
            },
            new Points()
            {
                Id = 2,
                Name = "tram",
                PointsValue = 3
            },
            new Points()
            {
                Id = 3,
                Name = "bus",
                PointsValue = 4
            },
            new Points()
            {
                Id = 3,
                Name = "bonusStop",
                PointsValue = 20
            },
            new Points()
            {
                Id = 3,
                Name = "bonusLines",
                PointsValue = 25
            }
        };
    
    [Fact]
    public async Task EvaluateRace_ReturnsPoints_WhenRaceFinishedAndLate()
    {
        //Arrange
        raceEntry.TimeOfReturn = new DateTimeOffset(2025, 10, 10, 19, 05, 31, TimeSpan.Zero);

        _pointsRepositoryMock
            .Setup(x => x.GetAll())
            .ReturnsAsync(pointsList);
        _raceRepositoryMock
            .Setup(x => x.GetById(It.IsAny<int>()))
            .ReturnsAsync(new Race()
            {
                Id = 1,
                Date = new DateOnly(2025, 10, 10),
                Title = "Race1",
                StartTime = new DateTimeOffset(2025, 10, 10, 16, 00, 00, TimeSpan.Zero),
                EndTime = new DateTimeOffset(2025,10,10,19,00,00,TimeSpan.Zero),
                BonusStopDisplayTime = new DateTimeOffset(2025, 10, 10, 15, 55, 00, TimeSpan.Zero)
            });
        //Act
        var raceEntryEvaluated = await _sut.EvaluateRace(raceEntry);
    }


    [Fact]
    public void GetPenaltyPointsForBeingLate_Returns10_WhenFewSecondsLate()
    {
        //Arrange
        var timeOfReturn = new DateTimeOffset(2025, 01, 01, 19, 00, 30, TimeSpan.Zero);
        var raceEndTime = new DateTimeOffset(2025, 01, 01, 19, 00, 00, TimeSpan.Zero);

        //Act
        var penalty = _sut.GetPenaltyPointsForBeingLate(timeOfReturn, raceEndTime);

        //Assert
        Assert.Equal(10, penalty);            
    }

    [Fact]
    public void GetPenaltyPointsForBeingLate_Returns0_WhenArrivedInTime()
    {
        //Arrange
        var timeOfReturn = new DateTimeOffset(2025, 01, 01, 19, 00, 00, TimeSpan.Zero);
        var raceEndTime = new DateTimeOffset(2025, 01, 01, 19, 00, 00, TimeSpan.Zero);

        //Act
        var penalty = _sut.GetPenaltyPointsForBeingLate(timeOfReturn, raceEndTime);

        //Assert
        Assert.Equal(0, penalty);
    }

    [Fact]
    public void PointsToDictionary_ReturnsDictionary()
    {
        //Arrange
        var pointsList = new List<Points>()
        {
            new Points()
            {
                Id = 1,
                Name = "tram",
                PointsValue = 4
            },
            new Points()
            {
                Id = 2,
                Name = "bus",
                PointsValue = 3
            }
        };

        //Act
        var dict = _sut.PointsToDictionary(pointsList);

        //Assert
        Assert.Equal(2, dict.Count);
        Assert.Equal(3, dict["bus"]);
        Assert.Equal(4, dict["tram"]);
    }

    [Fact]
    public void IsInList_ReturnsTrue_WhenPresentInList()
    {
        //Arrange
        var list = new List<string>()
        {
            "Zastávečka1",
            "Zastávečka2",
            "Zastávečka3"
        };
        var comparedString = "zastavecka2";

        //Act
        var result = _sut.IsInlist(list, comparedString);

        //Assert
        Assert.True(result);
    }

    [Fact]
    public void IsInList_ReturnsFalse_WhenNotPresentInList()
    {
        //Arrange
        var list = new List<string>()
        {
            "Zastávečka1",
            "Zastávečka2",
            "Zastávečka3"
        };
        var comparedString = "zastavvecka2";

        //Act
        var result = _sut.IsInlist(list, comparedString);

        //Assert
        Assert.False(result);
    }
}
