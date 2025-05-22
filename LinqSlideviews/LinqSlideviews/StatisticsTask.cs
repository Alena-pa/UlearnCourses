using System;
using System.Collections.Generic;
using System.Linq;

namespace linq_slideviews;

public class StatisticsTask
{
    public static double GetMedianTimePerSlide(List<VisitRecord> visits, SlideType slideType)
    {
        const double minMinutes = 1;
        const double maxMinutes = 120; // 2 часа = 120 минут

        var timeDifferences = visits
            .GroupBy(v => v.UserId)
            .SelectMany(g => g
                .OrderBy(v => v.DateTime)
                .Select((v, i) => new { Visit = v, Index = i })
                .Where(x => x.Visit.SlideType == slideType)
                .Select(x =>
                {
                    var nextVisit = g
                        .OrderBy(v => v.DateTime)
                        .Skip(x.Index + 1)
                        .FirstOrDefault(v => v.SlideId != x.Visit.SlideId);
                    return nextVisit != null
                        ? (nextVisit.DateTime - x.Visit.DateTime).TotalMinutes
                        : (double?)null;
                })
                .Where(t => t.HasValue && t >= minMinutes && t <= maxMinutes)
                .Select(t => t.Value))
            .ToList();

        return timeDifferences.Any() ? timeDifferences.Median() : 0;
    }
}