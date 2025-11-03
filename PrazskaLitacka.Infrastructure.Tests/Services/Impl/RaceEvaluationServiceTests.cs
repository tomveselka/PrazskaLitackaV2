using Microsoft.Extensions.Logging;
using Moq;
using PrazskaLitacka.Domain.Entities;
using PrazskaLitacka.Domain.Enums;
using PrazskaLitacka.Domain.Interfaces;
using PrazskaLitacka.Infrastructure.Services.Impl;
using PrazskaLitacka.Infrastructure.Tests.Extensions;

namespace PrazskaLitacka.Infrastructure.Tests.Services.Impl;
public class RaceEvaluationServiceTests
{
    private readonly Mock<IPointsRepository> _pointsRepositoryMock;
    private readonly Mock<IRaceRepository> _raceRepositoryMock;
    private readonly Mock<ILogger<RaceEvaluationService>> _loggerMock;
    private readonly Mock<IBonusLineRepository> _bonusLineRepositoryMock;
    private readonly Mock<IBonusStationRepository> _bonusStationRepositoryMock;

    private readonly RaceEvaluationService _sut;

    public RaceEvaluationServiceTests()
    {
        _pointsRepositoryMock = new Mock<IPointsRepository>();
        _raceRepositoryMock = new Mock<IRaceRepository>();
        _loggerMock = new Mock<ILogger<RaceEvaluationService>>();
        _bonusLineRepositoryMock = new Mock<IBonusLineRepository>();
        _bonusStationRepositoryMock = new Mock<IBonusStationRepository>();
        _sut = new RaceEvaluationService(_pointsRepositoryMock.Object, _raceRepositoryMock.Object, _loggerMock.Object, _bonusLineRepositoryMock.Object, _bonusStationRepositoryMock.Object);
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
                StationFromZones="B,0",
                StationToName = "StationTo1",
                StationToZones="B",
                LineName = "Metro1",
                LineType = TrafficType.Metro,
            },
             new Row()
            {
                Id = 2,
                StationFromName = "StationFrom2",
                StationFromZones="3",
                StationToName = "BonusStation2",
                StationToZones="4",
                LineName = "trainR1",
                LineType = TrafficType.TrainR,
            }
        }
    };
    
    private List<Points> pointsList = new List<Points>()
        {
            new Points()
            {
                Id = 1,
                Name = "Metro",
                PointsValue = 2
            },
            new Points()
            {
                Id = 2,
                Name = "Tram",
                PointsValue = 3
            },
            new Points()
            {
                Id = 3,
                Name = "TrainR",
                PointsValue = 4
            },
            new Points()
            {
                Id = 4,
                Name = "bonusStop",
                PointsValue = 20
            },
            new Points()
            {
                Id = 5,
                Name = "bonusLine",
                PointsValue = 25
            },
            new Points()
            {
                Id = 6,
                Name = "stop",
                PointsValue = 3
            },
            new Points()
            {
                Id = 6,
                Name = "zone",
                PointsValue = 15
            },
            new Points()
            {
                Id = 6,
                Name = "late",
                PointsValue = 10
            }

        };
    private Race race = new Race()
    {
        Id = 1,
        Date = new DateOnly(2025, 10, 10),
        Title = "Race1",
        StartTime = new DateTimeOffset(2025, 10, 10, 16, 00, 00, TimeSpan.Zero),
        EndTime = new DateTimeOffset(2025, 10, 10, 19, 00, 00, TimeSpan.Zero),
        BonusStopDisplayTime = new DateTimeOffset(2025, 10, 10, 15, 55, 00, TimeSpan.Zero)
    };

    private List<BonusLine> bonusLines = new List<BonusLine>()
    {
        new BonusLine()
        {
            Id = 1,
            Name = "trainR1",
            Type = TrafficType.TrainR,
            RaceId = 1
        }
    };

    private List<BonusStation> bonusStations = new List<BonusStation>()
    {
        new BonusStation()
        {
            Id = 1,
            Name = "BonusStation1",
            Zone = "1",
            StationId = "stationId1",
            RaceId= 1,
        },
         new BonusStation()
        {
            Id = 2,
            Name = "BonusStation2",
            Zone = "0,1",
            StationId = "stationId2",
            RaceId= 1,
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
            .ReturnsAsync(race);
        _bonusLineRepositoryMock
            .Setup(x => x.GetAllForRace(It.IsAny<int>()))
            .ReturnsAsync(bonusLines);
        _bonusStationRepositoryMock
            .Setup(x => x.GetAllForRace(It.IsAny<int>()))
            .ReturnsAsync(bonusStations);

        //Act
        var raceEntryEvaluated = await _sut.EvaluateRace(raceEntry);

        //Assert
        Assert.Equal(30, raceEntryEvaluated.PointsForZones);
        Assert.Equal(60, raceEntryEvaluated.PointsForPenaltiesNegative);
        Assert.Equal(63, raceEntryEvaluated.PointsForStationsAndLinesTotal);
        Assert.Equal(33, raceEntryEvaluated.PointsTotal);

        _pointsRepositoryMock.Verify(x => x.GetAll(), Times.Once);
        _raceRepositoryMock.Verify(x => x.GetById(It.IsAny<int>()), Times.Once);
        _bonusLineRepositoryMock.Verify(x => x.GetAllForRace(It.IsAny<int>()), Times.Once);
        _bonusStationRepositoryMock.Verify(x => x.GetAllForRace(It.IsAny<int>()), Times.Once);

        _loggerMock.VerifyLogStartsWith(
           LogLevel.Information,
           "RACE-ENTRY-EVAL-BEGIN Began evaluating race entry 1",
           Times.Once()
       );
    }

    [Fact]
    public void GetPenaltyPointsForBeingLate_Returns10_WhenFewSecondsLate()
    {
        //Arrange
        var timeOfReturn = new DateTimeOffset(2025, 01, 01, 19, 00, 30, TimeSpan.Zero);
        var raceEndTime = new DateTimeOffset(2025, 01, 01, 19, 00, 00, TimeSpan.Zero);

        //Act
        var penalty = _sut.GetPenaltyPointsForBeingLate(timeOfReturn, raceEndTime, 10);

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
        var penalty = _sut.GetPenaltyPointsForBeingLate(timeOfReturn, raceEndTime, 10);

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

    [Fact]
    public void CalculatePointsForZones_ReturnsSixty()
    {
        //Arrange
        var zoneList = new List<string> { "P", "B", "0", "1", "2", "3", "P"};

        //Act
        var result = _sut.CalculatePointsForZones(zoneList, 20);

        //Assert
        Assert.Equal(60, result);
    }
}
