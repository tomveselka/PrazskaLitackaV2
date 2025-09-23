using Moq;
using Xunit;
using Microsoft.Extensions.Logging;
using PrazskaLitacka.Infrastructure.Services.Impl;
using PrazskaLitacka.Webapi.XmlModels;
using Moq.Protected;
using System.Net;
using System.Text;
using PidStops.Models;

namespace PrazskaLitacka.Infrastructure.Tests.Services.Impl;
public class GetPidDataTests
{
    private readonly Mock<ILogger<GetPidDataService>> _loggerMock;
    private readonly Mock<IHttpClientFactory> _factoryMock;
    private readonly HttpClient _httpClient;

    private readonly GetPidDataService _getPidData;
    public GetPidDataTests()
    {
        _loggerMock = new Mock<ILogger<GetPidDataService>>();
        _factoryMock = new Mock<IHttpClientFactory>();

        var handlerMock = new Mock<HttpMessageHandler>();
        handlerMock.Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = xmlResponse
            });

        var fakeClient = new HttpClient(handlerMock.Object)
        {
            BaseAddress = new Uri("https://data.pid.cz/stops/xml/StopsByName.xsd")
        };

        _factoryMock.Setup(f => f.CreateClient("XmlDataClient"))
                   .Returns(fakeClient);

        _getPidData = new GetPidDataService(_factoryMock.Object, _loggerMock.Object);
    }

    [Fact]
    public async Task GetStationXmlAsync_ReturnsStopsObjectAsync()
    {
        //Act
        var stops = await _getPidData.GetStationXmlAsync();

        //Assert
        Assert.IsType<Stops>(stops);
    }

    [Fact]
    public void GetDataForDbInserts_ShouldReturnDto_WhenStopsObjectContainsData()
    {

    }

    StringContent xmlResponse = new StringContent(
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

