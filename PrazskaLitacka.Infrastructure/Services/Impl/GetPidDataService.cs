using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using prazska_litacka.webapi;
using System.Xml.Serialization;
using PrazskaLitacka.Webapi.Interfaces;
using PrazskaLitacka.Infrastructure.Exceptions;
using PrazskaLitacka.Webapi.XmlModels;
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
        return stops;
    }

    public PidDataDto GetDataForDbInserts(Stops stopList)
    {
        PidDataDto pidData = new PidDataDto();
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
                zones.Append(stop.Zone);
            }
            var uniqueLinesList = lines
                .GroupBy(x => x.Name)
                .Select(x => x.First())
                .ToList();
            station.Lines = uniqueLinesList;

            var uniqueZonesList = zones
                .ToString()
                .Split(',')
                .Distinct()
                .ToList();                
            station.Zones = string.Join(",", uniqueZonesList);
        }

        return pidData;
    }
}

