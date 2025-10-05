using Microsoft.Extensions.Logging;
using PrazskaLitacka.Domain.Entities;
using PrazskaLitacka.Domain.Interfaces;
using PrazskaLitacka.Webapi.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrazskaLitacka.Infrastructure.Services.Impl;
public class RaceEvaluationService : IRaceEvaluationService
{
    private readonly IPointsRepository _pointsRepository;
    private readonly IRaceRepository _raceRepository;
    private readonly ILogger<RaceEvaluationService> _logger;

    public RaceEvaluationService(IPointsRepository pointsRepository, IRaceRepository raceRepository, ILogger<RaceEvaluationService> logger)
    {
        _pointsRepository = pointsRepository;
        _raceRepository = raceRepository;
        _logger = logger;
    }

    public async Task<RaceEntry> EvaluateRace(RaceEntry raceEntry)
    {
        var pointsList = await _pointsRepository.GetAll();
        var pointsMap = PointsToDictionary(pointsList);
        var rows = raceEntry.Rows;
        var visitedStations = new List<string>();
        var visitedBonusStations = new List<string>();
        var visitedLines = new List<string>();
        var visitedBonusLines = new List<string>();
        foreach (var row in rows) 
        {
            row.StationFromPoints = 0;
            row.StationToPoints = 0;
            row.LinePoints = 0;

            if (!visitedStations.Contains(row.StationFromName))
            {
                visitedStations.Add(row.StationFromName);
                row.StationFromPoints = pointsMap.GetValueOrDefault("stop", 0);
            }
            if (!visitedBonusStations.Contains(row.StationFromName))
            {
                visitedBonusStations.Add(row.StationFromName);
                row.StationFromPoints += pointsMap.GetValueOrDefault("bonusStop", 0);
            }
            if (!visitedStations.Contains(row.StationToName))
            {
                visitedStations.Add(row.StationToName);
                row.StationToPoints = pointsMap.GetValueOrDefault("stop", 0);
            }
            if (!visitedBonusStations.Contains(row.StationToName))
            {
                visitedBonusStations.Add(row.StationToName);
                row.StationToPoints += pointsMap.GetValueOrDefault("bonusStop", 0);
            }
            if (!visitedLines.Contains(row.LineName))
            {
                visitedStations.Add(row.StationToName);
                row.LinePoints = pointsMap.GetValueOrDefault(row.LineType, 0);
            }
            if (!visitedBonusLines.Contains(row.LineName))
            {
                visitedBonusLines.Add(row.LineName);
                row.LinePoints += pointsMap.GetValueOrDefault("bonusLine", 0);
            }

            raceEntry.StationLAndLinesPointsTotal += row.StationFromPoints + row.StationToPoints + row.LinePoints;
        }

        if(raceEntry.TimeOfReturn is DateTimeOffset timeOfReturn)
        {
            raceEntry.Penalties = await GetPenaltyPointsForBeingLate(timeOfReturn, raceEntry.RaceId);
        }

        return raceEntry;
    }

    public Dictionary<string, int> PointsToDictionary(List<Points> pointsList)
    {
        var dict = new Dictionary<string, int>();
        foreach (var point in pointsList) 
        {
            dict.Add(point.Name, point.PointsValue);
        } 
        return dict;
    }

    public async Task<double> GetPenaltyPointsForBeingLate(DateTimeOffset timeOfReturn, int RaceId)
    {
        var race = await _raceRepository.GetById(RaceId);
        if(timeOfReturn <= race!.EndTime)
        {
            return 0;
        }
        var difference = Math.Ceiling((timeOfReturn - race!.EndTime).TotalMinutes);
        return difference * 10;
    }
}
