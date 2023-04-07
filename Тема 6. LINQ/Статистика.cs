using System;
using System.Collections.Generic;
using System.Linq;

namespace linq_slideviews;

public class StatisticsTask
{
    public static double GetMedianTimePerSlide(List<VisitRecord> visits, SlideType slideType)
    {
        return visits.GroupBy(user => user.UserId)
            .Select(group => group.OrderBy(x => x.DateTime)
                .Bigrams())
            .SelectMany(x => x)
            .Where(user => user.First.SlideType.Equals(slideType) && !user.First.SlideId.Equals(user.Second.SlideId))
            .Select(user => (user.Second.DateTime - user.First.DateTime))
            .Where(minutes => 
                minutes >= TimeSpan.FromMinutes(1)
                && minutes <= TimeSpan.FromMinutes(120))
            .Select(minutes => minutes.TotalMinutes)
            .DefaultIfEmpty()
            .Median();
    }
}