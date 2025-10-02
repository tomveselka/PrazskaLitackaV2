using Microsoft.Extensions.Logging;
using Moq;
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
}
