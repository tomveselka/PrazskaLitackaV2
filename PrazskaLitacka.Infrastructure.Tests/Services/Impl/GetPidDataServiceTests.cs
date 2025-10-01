using Microsoft.Extensions.Logging;
using Moq;
using Moq.Protected;
using PidStops.Models;
using PrazskaLitacka.Domain.Interfaces;
using PrazskaLitacka.Infrastructure.Services.Impl;
using PrazskaLitacka.Infrastructure.Tests.Extensions;
using System.Net;
using System.Text;
using Xunit;

namespace PrazskaLitacka.Infrastructure.Tests.Services.Impl;
public class GetPidDataServiceTests
{
    private readonly Mock<ILogger<GetPidDataService>> _loggerMock;
    private readonly Mock<IHttpClientFactory> _factoryMock;
    private readonly Mock<HttpMessageHandler> _handlerMock;
    private readonly Mock<IStationRepository> _stationRepositoryMock;
    private readonly HttpClient _httpClient;

    private readonly GetPidDataService _getPidData;
    public GetPidDataServiceTests()
    {
        _loggerMock = new Mock<ILogger<GetPidDataService>>();
        _factoryMock = new Mock<IHttpClientFactory>();
        _stationRepositoryMock = new Mock<IStationRepository>();
        _handlerMock = new Mock<HttpMessageHandler>();
        _handlerMock.Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = xmlResponse
            });

        _httpClient = new HttpClient(_handlerMock.Object)
        {
            BaseAddress = new Uri("https://data.pid.cz/stops/xml/StopsByName.xsd")
        };

        _factoryMock.Setup(f => f.CreateClient("XmlDataClient"))
                   .Returns(_httpClient);

        _getPidData = new GetPidDataService(_factoryMock.Object, _stationRepositoryMock.Object, _loggerMock.Object);
    }

    [Fact]
    public async Task GetStationXmlAsync_ReturnsStopsObjectAsync()
    {
        //Act
        var stops = await _getPidData.GetStationXmlAsync();

        //Assert
        Assert.IsType<Stops>(stops);
        Assert.Equal(3, stops.Groups.Count);

        _handlerMock.Protected().Verify(
            "SendAsync",
            Times.Once(),
            ItExpr.Is<HttpRequestMessage>(req =>
                req.Method == HttpMethod.Get
                && req.RequestUri == _httpClient.BaseAddress),
            ItExpr.IsAny<CancellationToken>()
        );
        _loggerMock.VerifyLogStartsWith(
            LogLevel.Information,
            "PID-DATA-DOWNLOAD-SUCCESS",
            Times.Once()
        );
        _loggerMock.VerifyLogStartsWith(
           LogLevel.Information,
           "PID-DATA-PARSE-SUCCESS",
           Times.Once()
       );
    }

    [Fact]
    public void GetDataForDbInserts_ShouldReturnList_WhenStopsObjectContainsData()
    {
        //Act
        var stationList = _getPidData.GetDataForDbInserts(stops);

        //Assert
        Assert.Equal(2, stationList.Count);
        Assert.Equal(2, stationList[0].Lines.Count);
        Assert.Equal(3, stationList[1].Lines.Count);
        Assert.Equal("UniqueName1", stationList[0].Name);
        Assert.Equal("UniqueName2", stationList[1].Name);
        Assert.Equal("8", stationList[0].Zones);
        Assert.Equal("P,B,0,1", stationList[1].Zones);
        _loggerMock.VerifyLogStartsWith(
           LogLevel.Information,
           "PID-DATA-CREATION-SUCCESS",
           Times.Once()
       );
    }

    private readonly Stops stops = new Stops()
    {
        DataFormatVersion = "1",
        GeneratedAt = DateTime.UtcNow,
        Groups = new List<Group>()
            {
                new Group()
                {
                    Name = "Name1",
                    DistrictCode = "DistCode1",
                    IdosCategory = 1,
                    IdosName = "IdosName1",
                    FullName = "FullName1",
                    UniqueName = "UniqueName1",
                    Node = 1,
                    Cis= 1,
                    AvgLat = 50.1F,
                    AvgLon = 15.0F,
                    AvgJtskX = -60000F,
                    AvgJtskY = -1000000F,
                    Municipality = "Mun1",
                    Stops = new List<Stop>()
                    {
                        new Stop()
                        {
                            Id = "Stop1/1",
                            Platform = "1",
                            AltIdosName = "AltIdosName",
                            Lat = 49.8581047F,
                            Lon = 15.4081345F,
                            JtskX = -675931.563F,
                            JtskY = -1077494.13F,
                            Zone = "8",
                            WheelchairAccess = WheelchairAccessType.unknown,
                            GtfsIds = "U7288Z1",
                            Lines = new List<Line>()
                            {
                                new Line()
                                {
                                    Id=740,
                                    Name="740",
                                    Type=TrafficType.bus,
                                    Direction="Okřesaneč",
                                    Direction2="Golčův Jeníkov,nám.TGM"
                                },
                                new Line()
                                {
                                    Id=740,
                                    Name="740",
                                    Type=TrafficType.bus,
                                    Direction="Čáslav,žel.st."
                                },
                                new Line()
                                {
                                    Id=741,
                                    Name="741",
                                    Type=TrafficType.bus,
                                    Direction="Čáslav,žel.st."
                                }
                            }
                        }
                    }
                },
                new Group()
                {
                    Name = "Name2",
                    DistrictCode = "DistCode2",
                    IdosCategory = 1,
                    IdosName = "IdosName2",
                    FullName = "FullName2",
                    UniqueName = "UniqueName2",
                    Node = 1,
                    Cis= 1,
                    AvgLat = 50.1F,
                    AvgLon = 15.0F,
                    AvgJtskX = -60000F,
                    AvgJtskY = -1000000F,
                    Municipality = "Mun2",
                    Stops = new List<Stop>()
                    {
                        new Stop()
                        {
                            Id = "Stop2/1",
                            Platform = "1",
                            AltIdosName = "AltIdosName2.1",
                            Lat = 49.8581047F,
                            Lon = 15.4081345F,
                            JtskX = -675931.563F,
                            JtskY = -1077494.13F,
                            Zone = "P,B,1",
                            WheelchairAccess = WheelchairAccessType.unknown,
                            GtfsIds = "U7288Z1",
                            Lines = new List<Line>()
                            {
                                new Line()
                                {
                                    Id=15,
                                    Name="15",
                                    Type=TrafficType.tram,
                                    Direction="Okřesaneč",
                                    Direction2="Golčův Jeníkov,nám.TGM"
                                },
                                new Line()
                                {
                                    Id=16,
                                    Name="16",
                                    Type=TrafficType.tram,
                                    Direction="Čáslav,žel.st."
                                }
                            }
                        },
                        new Stop()
                        {
                            Id = "Stop2/2",
                            Platform = "2",
                            AltIdosName = "AltIdosName2.2",
                            Lat = 49.8581047F,
                            Lon = 15.4081345F,
                            JtskX = -675931.563F,
                            JtskY = -1077494.13F,
                            Zone = "B,0",
                            WheelchairAccess = WheelchairAccessType.unknown,
                            GtfsIds = "U7288Z1",
                            Lines = new List<Line>()
                            {
                                new Line()
                                {
                                    Id=19,
                                    Name="19",
                                    Type=TrafficType.tram,
                                    Direction="Okřesaneč",
                                    Direction2="Golčův Jeníkov,nám.TGM"
                                },
                                new Line()
                                {
                                    Id=16,
                                    Name="16",
                                    Type=TrafficType.tram,
                                    Direction="Čáslav,žel.st."
                                }
                            }
                        }
                    } }
            }
    };

    private readonly StringContent xmlResponse = new StringContent(
    @"<stops xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xmlns:xsd=""http://www.w3.org/2001/XMLSchema"" generatedAt=""2025-09-23T03:52:25"" dataFormatVersion=""3"">
	<group name=""Adamov"" districtCode=""KH"" idosCategory=""301003"" idosName=""Adamov"" fullName=""Adamov"" uniqueName=""Adamov"" node=""7288"" cis=""33"" avgLat=""49.8581047"" avgLon=""15.4081345"" avgJtskX=""-675931.563"" avgJtskY=""-1077494.13"" municipality=""Adamov"" mainTrafficType=""bus"">
		<stop id=""7288/1"" platform=""1"" altIdosName=""Adamov"" lat=""49.8581047"" lon=""15.4081345"" jtskX=""-675931.563"" jtskY=""-1077494.13"" zone=""8"" mainTrafficType=""bus"" wheelchairAccess=""unknown"" gtfsIds=""U7288Z1"">
			<line id=""740"" name=""740"" type=""bus"" direction=""Okřesaneč"" direction2=""Golčův Jeníkov,nám.TGM""/>
			<line id=""740"" name=""740"" type=""bus"" direction=""Čáslav,žel.st.""/>
			<line id=""741"" name=""741"" type=""bus"" direction=""Čáslav,žel.st.""/>
		</stop>
	</group>
	<group name=""Albertov"" districtCode=""AB"" idosCategory=""301003"" idosName=""Albertov"" fullName=""Albertov"" uniqueName=""Albertov"" node=""876"" cis=""58936"" avgLat=""50.0679169"" avgLon=""14.4207993"" avgJtskX=""-743138.3"" avgJtskY=""-1045162.44"" municipality=""Praha"" mainTrafficType=""tram"">
		<stop id=""876/1"" platform=""A"" altIdosName=""Albertov"" lat=""50.0672455"" lon=""14.421648"" jtskX=""-743088.4"" jtskY=""-1045244.69"" zone=""P"" mainTrafficType=""tram"" wheelchairAccess=""possible"" gtfsIds=""U876Z1P"">
			<line id=""7"" name=""7"" type=""tram"" direction=""Radlická""/>
			<line id=""14"" name=""14"" type=""tram"" direction=""Vozovna Kobylisy""/>
			<line id=""18"" name=""18"" type=""tram"" direction=""Nádraží Podbaba""/>
			<line id=""24"" name=""24"" type=""tram"" direction=""Vozovna Kobylisy""/>
			<line id=""93"" name=""93"" type=""tram"" isNight=""true"" direction=""Sídliště Ďáblice""/>
			<line id=""95"" name=""95"" type=""tram"" isNight=""true"" direction=""Vozovna Kobylisy""/>
		</stop>
		<stop id=""876/2"" platform=""B"" altIdosName=""Albertov"" lat=""50.0686836"" lon=""14.4204521"" jtskX=""-743151.3"" jtskY=""-1045074.5"" zone=""P"" mainTrafficType=""tram"" wheelchairAccess=""possible"" gtfsIds=""U876Z2P"">
			<line id=""14"" name=""14"" type=""tram"" direction=""Spořilov""/>
			<line id=""18"" name=""18"" type=""tram"" direction=""Vozovna Pankrác""/>
			<line id=""24"" name=""24"" type=""tram"" direction=""Náměstí Bratří Synků""/>
			<line id=""93"" name=""93"" type=""tram"" isNight=""true"" direction=""Vozovna Pankrác""/>
			<line id=""95"" name=""95"" type=""tram"" isNight=""true"" direction=""Ústřední dílny DP""/>
		</stop>
		<stop id=""876/4"" platform=""D"" altIdosName=""Albertov"" lat=""50.06782"" lon=""14.4202976"" jtskX=""-743175.3"" jtskY=""-1045168.13"" zone=""P"" mainTrafficType=""tram"" wheelchairAccess=""possible"" gtfsIds=""U876Z4P"">
			<line id=""7"" name=""7"" type=""tram"" direction=""Lehovec""/>
		</stop>
	</group>
	<group name=""Ametystová"" districtCode=""AB"" idosCategory=""301003"" idosName=""Ametystová"" fullName=""Ametystová"" uniqueName=""Ametystová"" node=""1274"" cis=""58937"" avgLat=""49.9882"" avgLon=""14.3622169"" avgJtskX=""-748508.3"" avgJtskY=""-1053371.25"" municipality=""Praha"" mainTrafficType=""bus"">
		<stop id=""1274/1"" platform=""A"" altIdosName=""Ametystová"" lat=""49.9882"" lon=""14.3622169"" jtskX=""-748508.3"" jtskY=""-1053371.25"" zone=""P"" mainTrafficType=""bus"" wheelchairAccess=""unknown"" gtfsIds=""U1274Z1P"">
			<line id=""245"" name=""245"" type=""bus"" direction=""Lahovská""/>
			<line id=""269"" name=""269"" type=""bus"" direction=""Škola Radotín""/>
		</stop>
	</group>
</stops>",
    Encoding.UTF8,
    "application/xml");
}

