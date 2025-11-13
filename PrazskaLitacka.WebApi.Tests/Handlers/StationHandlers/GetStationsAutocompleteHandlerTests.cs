using Microsoft.Extensions.Logging;
using Moq;
using PrazskaLitacka.Domain.Entities;
using PrazskaLitacka.Domain.Interfaces;
using PrazskaLitacka.Infrastructure.Persistence;
using PrazskaLitacka.Webapi.Handlers.StationHandlers;
using PrazskaLitacka.WebApi.Tests.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static PrazskaLitacka.Webapi.Requests.StationRequests;

namespace PrazskaLitacka.WebApi.Tests.Handlers.StationHandlers;
public class GetStationsAutocompleteHandlerTests
{
    private readonly Mock<IStationRepository> _stationRepositoryMock;
    private readonly Mock<ILogger<GetStationsAutocompleteHandler>> _loggerMock;
    private readonly GetStationsAutocompleteHandler _sut;

    public GetStationsAutocompleteHandlerTests()
    {
        _stationRepositoryMock = new Mock<IStationRepository>();
        _loggerMock = new Mock<ILogger<GetStationsAutocompleteHandler>>();
        _sut = new GetStationsAutocompleteHandler(_stationRepositoryMock.Object, _loggerMock.Object);
    }

    [Fact]
    public async Task Handle_ReturnsList_WhenNameMatchesAndInRange()
    {
        //Arrange
        var query = new GetStationsAutocompleteQuery("Start",2,10);

        var returnList = new List<Station>()
        {
            new Station()
            {
                Id = 4,
                AvgLat = 1,
                AvgLon = 1,
                Lines=new List<StationLine>(){ },
                Zones="B",
                Name="Prachaticka"
            },
            new Station()
            {
                Id = 5,
                AvgLat = 1,
                AvgLon = 1,
                Lines=new List<StationLine>(){ },
                Zones="B",
                Name="Prachenska"
            }
        };

        _stationRepositoryMock
            .Setup(s => s.GetByBeginningOfName(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int>()))
            .ReturnsAsync(returnList);

        //Act
        var result = await _sut.Handle(query, CancellationToken.None);

        //Assert
        Assert.Equal(2, result.Count);
        _loggerMock.VerifyLogStartsWith(
           LogLevel.Information,
           "STATION-AUTOCOMPLETE-REQUEST Received request for stations starting with Start",
           Times.Once()
       );
       _loggerMock.VerifyLogStartsWith(
           LogLevel.Information,
           "STATION-AUTOCOMPLETE-RETURN Returning list of 2 stations",
           Times.Once()
       );
    }


}
