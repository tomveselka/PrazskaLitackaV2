using MediatR;
using Microsoft.Extensions.Logging;
using PidStops.Models;
using PrazskaLitacka.Domain.Entities;
using PrazskaLitacka.Domain.Interfaces;
using PrazskaLitacka.Infrastructure.Persistence;
using PrazskaLitacka.Webapi.Interfaces;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrazskaLitacka.Infrastructure.Services.Impl;
public class RaceEvaluationService : IRaceEvaluationService
{
    private readonly IPointsRepository _pointsRepository;
    private readonly IBonusLineRepository _bonusLineRepository;
    private readonly IBonusStationRepository _bonusStationRepository;
    private readonly IRaceRepository _raceRepository;
    private readonly ILogger<RaceEvaluationService> _logger;

    public RaceEvaluationService(IPointsRepository pointsRepository, IRaceRepository raceRepository, ILogger<RaceEvaluationService> logger, IBonusLineRepository bonusLineRepository, IBonusStationRepository bonusStationRepository)
    {
        _pointsRepository = pointsRepository;
        _raceRepository = raceRepository;
        _logger = logger;
        _bonusLineRepository = bonusLineRepository;
        _bonusStationRepository = bonusStationRepository;
    }

    public async Task<RaceEntry> EvaluateRace(RaceEntry raceEntry)
    {
        var race = await _raceRepository.GetById(raceEntry.RaceId);

        var pointsList = await _pointsRepository.GetAll();
        var pointsMap = PointsToDictionary(pointsList);

        var bonusStations = await _bonusStationRepository.GetAllForRace(raceEntry.RaceId);
        var bonusStationsList = bonusStations.Select(x => x.Name).ToList();
        var bonusLines = await _bonusLineRepository.GetAllForRace(raceEntry.RaceId);
        var bonusLinesList = bonusLines.Select(x => x.Name).ToList();

        var rows = raceEntry.Rows;
        var visitedStations = new List<string>();
        var visitedBonusStations = new List<string>();
        var visitedLines = new List<string>();
        var visitedBonusLines = new List<string>();
        var visitedZones = new List<string>();

        _logger.LogInformation("RACE-ENTRY-EVAL-BEGIN Began evaluating race entry {raceEntry}", raceEntry.Id);

        foreach (var row in rows) 
        {
            row.StationFromPoints = 0;
            row.StationToPoints = 0;
            row.LinePoints = 0;
            row.StationFromDuplicate = true;
            row.StationToDuplicate = true;
            row.LineDuplicate = true;
            row.StationFromBonus = false;
            row.StationToBonus = false;
            row.LineBonus = false;

            if (!IsInlist(visitedStations,row.StationFromName))
            {
                visitedStations.Add(row.StationFromName);
                row.StationFromPoints = pointsMap.GetValueOrDefault("stop", 0);
                row.StationFromDuplicate = false;
            }

            if (IsInlist(bonusStationsList, row.StationFromName) && !IsInlist(visitedBonusStations, row.StationFromName))
            {
                visitedBonusStations.Add(row.StationFromName);
                row.StationFromPoints += pointsMap.GetValueOrDefault("bonusStop", 0);
                row.StationFromBonus = true;
            }

            if (!IsInlist(visitedStations, row.StationToName))
            {
                visitedStations.Add(row.StationToName);
                row.StationToPoints = pointsMap.GetValueOrDefault("stop", 0);
            }

            if (IsInlist(bonusStationsList, row.StationToName) && !IsInlist(visitedBonusStations, row.StationToName))
            {
                visitedBonusStations.Add(row.StationToName);
                row.StationToPoints += pointsMap.GetValueOrDefault("bonusStop", 0);
                row.StationToBonus = true;
            }

            if (!IsInlist(visitedLines, row.LineName))
            {
                visitedStations.Add(row.StationToName);
                row.LinePoints = pointsMap.GetValueOrDefault(row.LineType.ToString(), 0);
            }

            if (IsInlist(bonusLinesList, row.LineName) && !IsInlist(visitedBonusLines, row.LineName))
            {
                visitedBonusLines.Add(row.LineName);
                row.LinePoints += pointsMap.GetValueOrDefault("bonusLine", 0);
                row.LineBonus = true;
            }

            visitedZones.AddRange(row.StationFromZones.Split(",", StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries));
            visitedZones.AddRange(row.StationToZones.Split(",", StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries));

            raceEntry.PointsForStationsAndLinesTotal += row.StationFromPoints + row.StationToPoints + row.LinePoints;
        }

        raceEntry.PointsForZones = CalculatePointsForZones(visitedZones, pointsMap.GetValueOrDefault("zone", 0));

        if(raceEntry.TimeOfReturn is DateTimeOffset timeOfReturn)
        {
            raceEntry.PointsForPenaltiesNegative = GetPenaltyPointsForBeingLate(timeOfReturn, race!.EndTime, pointsMap.GetValueOrDefault("late", 0));
        }

        raceEntry.PointsTotal = raceEntry.PointsForStationsAndLinesTotal + raceEntry.PointsForZones + raceEntry.PointsForGoodDeeds - raceEntry.PointsForPenaltiesNegative;

        _logger.LogInformation("RACE-ENTRY-EVAL-FINISH Finished evaluating race entry {raceEntry} with result of {points} points", raceEntry.Id, raceEntry.PointsTotal);

        return raceEntry;
    }

    internal Dictionary<string, int> PointsToDictionary(List<Points> pointsList)
    {
        var dict = new Dictionary<string, int>();
        foreach (var point in pointsList) 
        {
            dict.Add(point.Name, point.PointsValue);
        } 
        return dict;
    }

    internal double GetPenaltyPointsForBeingLate(DateTimeOffset timeOfReturn, DateTimeOffset RaceEndTime, int pointsForMinuteDelay)
    {
        if (timeOfReturn <= RaceEndTime)
        {
            return 0;
        }
        var difference = Math.Ceiling((timeOfReturn - RaceEndTime).TotalMinutes);
        return difference * pointsForMinuteDelay;
    }

    internal bool IsInlist(List<string> list, string comparedString) 
    {
        foreach (var entry in list)
        {
            if(NormalizeForComparison(entry) == NormalizeForComparison(comparedString))
            {
                return true;
            }
        }
        return false;
    }

    private string NormalizeForComparison(string input)
    {
        if (input == null) return string.Empty;

        string normalized = input.Normalize(NormalizationForm.FormD);

        var sb = new StringBuilder();
        foreach (var c in normalized)
        {
            var category = CharUnicodeInfo.GetUnicodeCategory(c);
            if (category != UnicodeCategory.NonSpacingMark)
                sb.Append(c);
        }

        return sb.ToString().Normalize(NormalizationForm.FormC)
                 .ToLowerInvariant();
    }

    internal double CalculatePointsForZones(List<string> visitedZones, int pointsForZone)
    {
        var numberOfZones = visitedZones
            .Where(x => x != "P" && x != "B" && x != "0")
            .Distinct()
            .ToList()
            .Count();

        return numberOfZones * pointsForZone;
    }
}
