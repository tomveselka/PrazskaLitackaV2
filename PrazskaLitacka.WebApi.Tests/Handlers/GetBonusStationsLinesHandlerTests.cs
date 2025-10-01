using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Time.Testing;
using Moq;
using PrazskaLitacka.Domain.Entities;
using PrazskaLitacka.Domain.Interfaces;
using PrazskaLitacka.Webapi.Handlers;
using PrazskaLitacka.WebApi.Tests.Extensions;
using static PrazskaLitacka.Webapi.Requests.StationRequests;

namespace PrazskaLitacka.WebApi.Tests.Handlers;
public class GetBonusStationsLinesHandlerTests
{
    private readonly GetBonusStationsLinesHandler _handler;
    private readonly Mock<IBonusLineRepository> _lineRepositoryMock;
    private readonly Mock<IBonusStationRepository> _stationRepositoryMock;
    private readonly Mock<IRaceRepository> _raceRepositoryMock;
    private readonly Mock<ILogger<GetBonusStationsLinesHandler>> _loggerMock;


    public GetBonusStationsLinesHandlerTests()
    {
        _lineRepositoryMock = new Mock<IBonusLineRepository>();
        _stationRepositoryMock = new Mock<IBonusStationRepository>();
        _raceRepositoryMock = new Mock<IRaceRepository>();
        _loggerMock = new Mock<ILogger<GetBonusStationsLinesHandler>>();

        var fakeTime = new FakeTimeProvider();
        fakeTime.SetUtcNow(new DateTimeOffset(2025, 1, 1, 10, 0, 0, TimeSpan.Zero));
        fakeTime.SetLocalTimeZone(TimeZoneInfo.Utc);

        _handler = new GetBonusStationsLinesHandler(_stationRepositoryMock.Object, _lineRepositoryMock.Object, _raceRepositoryMock.Object, _loggerMock.Object, fakeTime);
    }

    [Fact]
    public async Task Handle_ReturnsEmptyLists_WhenTimeTooEarlyToDisplayStations()
    {
        //Arrange
        var race = new Race
        {
            Id = 1,
            Date = DateOnly.MaxValue,
            Title = "Title",
            BonusStopDisplayTime = new DateTimeOffset(2025, 1, 1, 10, 0, 1, TimeSpan.Zero),
            RegistrationOpen = false,
            AcceptsResults = false
        };
        var query = new GetBonusStationsLinesQuery(5);

        _raceRepositoryMock
            .Setup(r => r.GetById(It.IsAny<int>()))
            .ReturnsAsync(race);

        //Act
        var response = await _handler.Handle(query, new CancellationToken());

        //Assert
        Assert.Empty(response.BonusStations);
        Assert.Empty(response.BonusLines);
        Assert.False(response.Visible);
        Assert.Equal(new DateTimeOffset(2025, 1, 1, 10, 0, 1, TimeSpan.Zero), response.VisibleFrom);
        Assert.Equal(query.raceId, response.RaceId);
        _loggerMock.VerifyLogStartsWith(
           LogLevel.Information,
           "BONUS-LIST-REQUEST",
           Times.Once()
       );
       _loggerMock.VerifyLogStartsWith(
           LogLevel.Information,
           "BONUS-LIST-TOO-EARLY",
           Times.Once()
       );
    }

    [Fact]
    public async Task Handle_ReturnsLists_WhenTimeToDisplayInPast()
    {
        //Arrange
        var race = new Race
        {
            Id = 1,
            Date = DateOnly.MaxValue,
            Title = "Title",
            BonusStopDisplayTime = new DateTime(2025, 1, 1, 10, 0, 0),
            RegistrationOpen = false,
            AcceptsResults = false
        };
        var bonusStationList = new List<BonusStation>()
        {
            new BonusStation()
            {
                RaceId = 1,
                Id = 1,
                Name = "Name",
                StationId = "StationId",
                Zone = "B,0"
            }
        };

        var bonusLineList = new List<BonusLine>()
        {
            new BonusLine()
            {
                RaceId = 1,
                Id = 1,
                Name = "Name",
                Type = "tram"
            }
        };

        var query = new GetBonusStationsLinesQuery(5);

        _raceRepositoryMock
            .Setup(r => r.GetById(It.IsAny<int>()))
            .ReturnsAsync(race);
        _lineRepositoryMock
            .Setup(r => r.GetAllForRace(It.IsAny<int>()))
            .ReturnsAsync(bonusLineList);
        _stationRepositoryMock
            .Setup(r => r.GetAllForRace(It.IsAny<int>()))
            .ReturnsAsync(bonusStationList);

        //Act
        var response = await _handler.Handle(query, new CancellationToken());

        //Assert
        Assert.Single(response.BonusStations);
        Assert.Single(response.BonusLines);
        Assert.True(response.Visible);
        Assert.Equal(new DateTime(2025, 1, 1, 10, 0, 0), response.VisibleFrom);
        Assert.Equal(query.raceId, response.RaceId);
        _loggerMock.VerifyLogStartsWith(
          LogLevel.Information,
          "BONUS-LIST-REQUEST",
          Times.Once()
      );
        _loggerMock.VerifyLogStartsWith(
            LogLevel.Information,
            "BONUS-LIST-RETRIEVED",
            Times.Once()
        );
    }
}
