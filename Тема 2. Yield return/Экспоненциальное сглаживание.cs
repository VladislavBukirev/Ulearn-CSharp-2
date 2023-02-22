using System.Collections.Generic;

namespace yield;

public static class ExpSmoothingTask
{
    public static IEnumerable<DataPoint> SmoothExponentialy(this IEnumerable<DataPoint> data, double alpha)
    {
        DataPoint previousPoint = null;
        foreach (var point in data)
        {
            previousPoint ??= point.WithExpSmoothedY(point.OriginalY);
            var averageValue = alpha * point.OriginalY + (1 - alpha) * previousPoint.ExpSmoothedY;
            previousPoint = point.WithExpSmoothedY(averageValue);
            yield return previousPoint;
        }
    }
}