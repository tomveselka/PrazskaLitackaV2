using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Time.Testing;
using Moq;
using PidStops.Models;
using PrazskaLitacka.Domain.Entities;
using PrazskaLitacka.Domain.Interfaces;
using PrazskaLitacka.Webapi.Handlers;
using PrazskaLitacka.Webapi.Interfaces;
using PrazskaLitacka.WebApi.Tests.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using static PrazskaLitacka.Webapi.Requests.StationRequests;

namespace PrazskaLitacka.WebApi.Tests.Handlers;
public class GetAllStationsLatestHandlerTests
{
    private readonly GetAllStationsLatestHandler _handler;
    private readonly Mock<ITechnicalVariablesRepository> _variablesRepositoryMock;
    private readonly Mock<IStationRepository> _stationRepositoryMock;
    private readonly Mock<IGetPidDataService> _pidDataServiceMock;
    private readonly Mock<ILogger<GetAllStationsLatestHandler>> _loggerMock;

    public GetAllStationsLatestHandlerTests()
    {
        _variablesRepositoryMock = new Mock<ITechnicalVariablesRepository>();
        _stationRepositoryMock = new Mock<IStationRepository>();
        _pidDataServiceMock = new Mock<IGetPidDataService>();
        _loggerMock = new Mock<ILogger<GetAllStationsLatestHandler>>();

        var fakeTime = new FakeTimeProvider();
        fakeTime.SetUtcNow(new DateTimeOffset(2025, 1, 1, 10, 0, 0, TimeSpan.Zero));
        fakeTime.SetLocalTimeZone(TimeZoneInfo.Utc);

        _stationRepositoryMock
            .Setup(x => x.GetAll())
            .ReturnsAsync(repoResponse);
        _pidDataServiceMock
            .Setup(x => x.GetDataForDbInserts(It.IsAny<Stops>()))
            .Returns(serviceResponse);
        _pidDataServiceMock
            .Setup(x => x.GetStationXmlAsync())
            .ReturnsAsync(stops);

        _handler = new GetAllStationsLatestHandler(_variablesRepositoryMock.Object, _pidDataServiceMock.Object, _loggerMock.Object, _stationRepositoryMock.Object, fakeTime);
    }

    [Fact]
    public async Task Handle_ReturnListDontUpdate_WhenTooEarlySinceLastUpdate()
    {
        //Arrange
        var query = new GetAllStationsLatestHandlerQuery(false);
        _variablesRepositoryMock
            .Setup(x => x.GetAll())
            .ReturnsAsync(new TechnicalVariables()
            {
                Id = 1,
                TimeOfLastStationUpdate = new DateTime(2025, 1, 1, 0, 0, 0)
            });

        //Act
        var stationList = await _handler.Handle(query, new CancellationToken());

        //Assert
        Assert.Single(stationList);
        Assert.IsType<List<Station>>(stationList);

        _loggerMock.VerifyLogStartsWith(
           LogLevel.Information,
           "STATION-LIST-REQUEST",
           Times.Once()
       ); 
       _loggerMock.VerifyLogStartsWith(
           LogLevel.Information,
           "STATION-LIST-RETURN",
           Times.Once()
       );
       _loggerMock.VerifyLogStartsWith(
          LogLevel.Information,
          "STATION-LIST-UPDATE-BEGIN",
          Times.Never()
      );
    }

    [Fact]
    public async Task Handle_ReturnListUpdate_WhenEnoughTimeSinceLastUpdate()
    {
        //Arrange
        var query = new GetAllStationsLatestHandlerQuery(false);
        _variablesRepositoryMock
            .Setup(x => x.GetAll())
            .ReturnsAsync(new TechnicalVariables()
            {
                Id = 1,
                TimeOfLastStationUpdate = new DateTime(2024, 1, 1, 0, 0, 0)
            });
        _stationRepositoryMock
            .Setup(x => x.DropAllUploadNew(It.IsAny<List<Station>>()))
            .Returns(Task.CompletedTask);

        //Act
        var stationList = await _handler.Handle(query, new CancellationToken());

        //Assert
        Assert.Single(stationList);
        Assert.IsType<List<Station>>(stationList);

        _pidDataServiceMock.Verify(p => p.GetStationXmlAsync(), Times.Once());
        _pidDataServiceMock.Verify(p => p.GetDataForDbInserts(It.IsAny<Stops>()), Times.Once());
        _stationRepositoryMock.Verify(p => p.DropAllUploadNew(It.IsAny<List<Station>>()), Times.Once());

        _loggerMock.VerifyLogStartsWith(
           LogLevel.Information,
           "STATION-LIST-REQUEST",
           Times.Once()
       );
        _loggerMock.VerifyLogStartsWith(
           LogLevel.Information,
           "STATION-LIST-UPDATE-BEGIN",
           Times.Once()
       );
       _loggerMock.VerifyLogStartsWith(
           LogLevel.Information,
           "STATION-LIST-UPDATE-SUCCESS",
           Times.Once()
       );
       _loggerMock.VerifyLogStartsWith(
            LogLevel.Information,
            "STATION-LIST-RETURN",
            Times.Once()
        );
    }

    [Fact]
    public async Task Handle_ReturnListDontUpdate_WhenUpdateFails()
    {
        //Arrange
        var query = new GetAllStationsLatestHandlerQuery(false);
        _variablesRepositoryMock
            .Setup(x => x.GetAll())
            .ReturnsAsync(new TechnicalVariables()
            {
                Id = 1,
                TimeOfLastStationUpdate = new DateTime(2024, 1, 1, 0, 0, 0)
            });
        _stationRepositoryMock
           .Setup(x => x.DropAllUploadNew(It.IsAny<List<Station>>()))
           .Throws<Exception>();

        //Act
        var stationList = await _handler.Handle(query, new CancellationToken());

        //Assert
        Assert.Single(stationList);
        Assert.IsType<List<Station>>(stationList);

        _pidDataServiceMock.Verify(p => p.GetStationXmlAsync(), Times.Once());
        _pidDataServiceMock.Verify(p => p.GetDataForDbInserts(It.IsAny<Stops>()), Times.Once());
        _stationRepositoryMock.Verify(p => p.DropAllUploadNew(It.IsAny<List<Station>>()), Times.Once());

        _loggerMock.VerifyLogStartsWith(
           LogLevel.Information,
           "STATION-LIST-REQUEST",
           Times.Once()
       );
        _loggerMock.VerifyLogStartsWith(
           LogLevel.Information,
           "STATION-LIST-UPDATE-BEGIN",
           Times.Once()
       );
        _loggerMock.VerifyLogStartsWith(
            LogLevel.Information,
            "STATION-LIST-UPDATE-SUCCESS",
            Times.Never()
        );
        _loggerMock.VerifyLogStartsWith(
           LogLevel.Error,
           "STATION-LIST-UPDATE-FAILURE",
           Times.Once()
       );
        _loggerMock.VerifyLogStartsWith(
             LogLevel.Information,
             "STATION-LIST-RETURN",
             Times.Once()
         );
    }

    private readonly List<Station> repoResponse = new List<Station>()
    {
        new Station()
        {
            Id = 1,
            Name = "Station1",
            AvgLat = 50.10f,
            AvgLon = 14.5f,
            Zones = "P",
            Lines = new List<StationLine>()
            {
                new StationLine()
                {
                    Id = 1,
                    Name = "B",
                    Type = "metro"
                }
            }
        }
    };

    private readonly List<Station> serviceResponse = new List<Station>()
    {
        new Station()
        {
            Id = 1,
            Name = "Station1New",
            AvgLat = 51.10f,
            AvgLon = 15.5f,
            Zones = "B",
            Lines = new List<StationLine>()
            {
                new StationLine()
                {
                    Id = 1,
                    Name = "C",
                    Type = "metro"
                }
            }
        }
    };

    private readonly Stops stops = new Stops()
    {
        DataFormatVersion = "1",
        GeneratedAt = DateTime.Now,
        Groups = new List<Group>()
    };
}
