using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace linq_slideviews;

public static class ExtensionsTask
{
    public static double Median(this IEnumerable<double> items)
    {
        var sortedList = items.OrderBy(e => e).ToList();
        if (!sortedList.Any())
            throw new InvalidOperationException();
        if (sortedList.Count % 2 == 1)
            return sortedList[sortedList.Count / 2];
        return (sortedList[sortedList.Count / 2 - 1] + sortedList[sortedList.Count / 2]) / 2;
    }

    public static IEnumerable<(T First, T Second)> Bigrams<T>(this IEnumerable<T> items)
    {
        T previous = default;
        var isFirstOccurrence = true;
        foreach (var element in items)
        {
            if (isFirstOccurrence)
            {
                previous = element;
                isFirstOccurrence = false;
                continue;
            }
            yield return (previous, element);
            previous = element;
        }
    }
}