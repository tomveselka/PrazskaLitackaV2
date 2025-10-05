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

    [Fact]
    public async Task GetPenaltyPointsForBeingLate_Returns10_WhenFewSecondsLate()
    {
        //Assert
        var timeOfReturn = new DateTimeOffset(2025, 01, 01, 19, 00, 30, TimeSpan.Zero);
        var race = new Race()
        {
            Date = new DateOnly(2025, 10, 10),
            Title = "Test",
            EndTime = new DateTimeOffset(2025,01,01,19,00,00,TimeSpan.Zero),
        };
        var raceReturned = _raceRepositoryMock
            .Setup(r => r.GetById(It.IsAny<int>()))
            .ReturnsAsync(race);

        //Act
        var penalty = await _sut.GetPenaltyPointsForBeingLate(timeOfReturn, 5);

        //Assert
        _raceRepositoryMock
            .Verify(r => r.GetById(It.IsAny<int>()), Times.Once);
        Assert.Equal(10, penalty);            
    }

    [Fact]
    public async Task GetPenaltyPointsForBeingLate_Returns0_WhenArrivedInTime()
    {
        //Assert
        var timeOfReturn = new DateTimeOffset(2025, 01, 01, 19, 00, 00, TimeSpan.Zero);
        var race = new Race()
        {
            Date = new DateOnly(2025, 10, 10),
            Title = "Test",
            EndTime = new DateTimeOffset(2025, 01, 01, 19, 00, 00, TimeSpan.Zero),
        };
        var raceReturned = _raceRepositoryMock
            .Setup(r => r.GetById(It.IsAny<int>()))
            .ReturnsAsync(race);

        //Act
        var penalty = await _sut.GetPenaltyPointsForBeingLate(timeOfReturn, 5);

        //Assert
        _raceRepositoryMock
            .Verify(r => r.GetById(It.IsAny<int>()), Times.Once);
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
}
