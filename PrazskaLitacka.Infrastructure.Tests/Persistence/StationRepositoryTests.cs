using Microsoft.EntityFrameworkCore;
using PrazskaLitacka.Domain.DbContexts;
using PrazskaLitacka.Domain.Entities;
using PrazskaLitacka.Domain.PidStops.Models;
using PrazskaLitacka.Infrastructure.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrazskaLitacka.Infrastructure.Tests.Persistence;
public class StationRepositoryTests
{
    private readonly StationRepository _sut;

    public StationRepositoryTests()
    {
        var context = GetInMemoryDbContext();
        _sut = new StationRepository(context);
    }

    private ApplicationDbContext GetInMemoryDbContext()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        var context = new ApplicationDbContext(options);

        List<Station> fullList = new List<Station>()
        {
            new Station()
            {
                Id = 1,
                AvgLat = 1,
                AvgLon = 1,
                Lines=new List<StationLine>(){ },
                Zones="B",
                Name="Prazska"
            },
            new Station()
            {
                Id = 2,
                AvgLat = 1,
                AvgLon = 1,
                Lines=new List<StationLine>(){ },
                Zones="B",
                Name="Prastara"
            },
            new Station()
            {
                Id = 3,
                AvgLat = 1,
                AvgLon = 1,
                Lines=new List<StationLine>(){ },
                Zones="B",
                Name="Prosecka"
            },
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
        context.Stations.AddRange(fullList);
        context.SaveChanges();

        return context;
    }


    [Fact]
    public async Task GetByBeginningOfName_ReturnsStations_WhenInRange()
    {
        //Act
        var result = await _sut.GetByBeginningOfName("Pra", 2, 2);

        //Assert
        Assert.Equal(2, result.Count);
        Assert.Equal("Prastara", result[0].Name);
        Assert.Equal("Prazska", result[1].Name);
    }

    [Fact]
    public async Task GetByBeginningOfName_ReturnsEmpty_WhenNoMatch()
    {
        //Act
        var result = await _sut.GetByBeginningOfName("Brn", 2, 2);

        //Assert
        Assert.Empty(result);
    }

    [Fact]
    public async Task GetByBeginningOfName_ReturnsEmpty_WhenOutsideRange()
    {
        //Act
        var result = await _sut.GetByBeginningOfName("Brn", 10, 2);

        //Assert
        Assert.Empty(result);
    }
}
