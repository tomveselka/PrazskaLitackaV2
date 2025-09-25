using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using PrazskaLitacka.Webapi.Interfaces;
using PrazskaLitacka.Infrastructure.Exceptions;
using PrazskaLitacka.Domain.Entities;
using Microsoft.Extensions.Logging;
using System.Net.Http;
using PidStops.Models;

namespace PrazskaLitacka.Infrastructure.Services.Impl;
public class GetPidDataService : IGetPidDataService
{
    private readonly HttpClient _httpClient;
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly ILogger<GetPidDataService> _logger;

    public GetPidDataService(IHttpClientFactory factory, ILogger<GetPidDataService> logger)
    {
        _httpClientFactory = factory;
        _httpClient = _httpClientFactory.CreateClient("XmlDataClient");
        _logger = logger;
    }

    public async Task<Stops> GetStationXmlAsync()
    {
        var response = await _httpClient.GetAsync(_httpClient.BaseAddress, CancellationToken.None);
        response.EnsureSuccessStatusCode();
        _logger.LogInformation("Retrieved XML from PID Data");
        await using var stream = await response.Content.ReadAsStreamAsync();
        var serializer = new XmlSerializer(typeof(Stops));
        var stops = (Stops?)serializer.Deserialize(stream);

        if (stops == null)
        {
            throw new StopsNotRetrievedException();
        }
        _logger.LogInformation("Successfully serialised XML into Stops object with {0} stations", stops.Groups.Count);
        return stops;
    }

    public List<Station> GetDataForDbInserts(Stops stopList)
    {
        List<Station> stationList = new List<Station>();
        var stopGroups = stopList.Groups;
        foreach (var stopGroup in stopGroups) 
        {
            var stops = stopGroup.Stops;
            Station station = new Station();
            station.AvgLat= stopGroup.AvgLat;
            station.AvgLon= stopGroup.AvgLon;
            station.Name = stopGroup.UniqueName;
            var zones = new StringBuilder();
            List<StationLine> lines = new List<StationLine>();
            foreach (var stop in stops)
            {
                foreach (var line in stop.Lines)
                {
                    var stationLine = new StationLine()
                    {
                        Name = line.Name,
                        Type = line.Type.ToString() 
                    };
                    lines.Add(stationLine);                    
                }
                zones.Append(stop.Zone).Append(","); ;
            }
            var uniqueLinesList = lines
                .GroupBy(x => x.Name)
                .Select(x => x.First())
                .ToList();
            station.Lines = uniqueLinesList;

            var uniqueZonesList = zones
                .ToString()
                .Split(',')
                .Where(z => !string.IsNullOrWhiteSpace(z))
                .Distinct()
                .ToList();
            station.Zones = SortZones(uniqueZonesList);
            stationList.Add(station);
        }

        _logger.LogInformation("Successfully created list of stations for DB insert. Number of stations: {0}", stationList.Count);

        return stationList;
    }

    private string SortZones(List<string> zones)
    {
        var sortedList =
        from x in zones
        orderby
            x == "P" ? 0 :
            x == "B" ? 1 :
            2,                       
            x == "P" || x == "B" ? 0 : int.Parse(x)
        select x;

        return string.Join(",", sortedList);
    }
}

