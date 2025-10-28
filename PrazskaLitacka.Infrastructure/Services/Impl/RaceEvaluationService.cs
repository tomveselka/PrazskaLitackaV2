using Microsoft.Extensions.Logging;
using PrazskaLitacka.Domain.Entities;
using PrazskaLitacka.Domain.Interfaces;
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
        var race = await _raceRepository.GetById(raceEntry.RaceId);

        var pointsList = await _pointsRepository.GetAll();
        var pointsMap = PointsToDictionary(pointsList);


        var rows = raceEntry.Rows;
        var visitedStations = new List<string>();
        var visitedBonusStations = new List<string>();
        var visitedLines = new List<string>();
        var visitedBonusLines = new List<string>();

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

            if (!IsInlist(visitedBonusStations, row.StationFromName))
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

            if (!IsInlist(visitedBonusStations, row.StationToName))
            {
                visitedBonusStations.Add(row.StationToName);
                row.StationToPoints += pointsMap.GetValueOrDefault("bonusStop", 0);
                row.StationToBonus = true;
            }

            if (!IsInlist(visitedLines, row.LineName))
            {
                visitedStations.Add(row.StationToName);
                row.LinePoints = pointsMap.GetValueOrDefault(row.LineType, 0);
            }

            if (!IsInlist(visitedBonusLines, row.LineName))
            {
                visitedBonusLines.Add(row.LineName);
                row.LinePoints += pointsMap.GetValueOrDefault("bonusLine", 0);
                row.LineBonus = true;
            }

            raceEntry.StationLAndLinesPointsTotal += row.StationFromPoints + row.StationToPoints + row.LinePoints;
        }

        if(raceEntry.TimeOfReturn is DateTimeOffset timeOfReturn)
        {
            raceEntry.Penalties = GetPenaltyPointsForBeingLate(timeOfReturn, race!.EndTime);
        }

        raceEntry.PointsTotal = raceEntry.PointsTotal + raceEntry.Penalties;

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

    internal double GetPenaltyPointsForBeingLate(DateTimeOffset timeOfReturn, DateTimeOffset RaceEndTime)
    {
        if(timeOfReturn <= RaceEndTime)
        {
            return 0;
        }
        var difference = Math.Ceiling((timeOfReturn - RaceEndTime).TotalMinutes);
        return difference * 10;
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
}
