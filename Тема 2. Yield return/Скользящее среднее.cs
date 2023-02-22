using System.Collections.Generic;

namespace yield;

public static class MovingAverageTask
{
    public static IEnumerable<DataPoint> MovingAverage(this IEnumerable<DataPoint> data, int windowWidth)
    {
        var queue = new Queue<DataPoint>();
        var sum = 0.0;
        foreach (var point in data)
        {
            var currentPoint = new DataPoint(point);
            queue.Enqueue(currentPoint);
            sum += point.OriginalY;
            yield return point.WithAvgSmoothedY(sum / queue.Count);
            if(queue.Count == windowWidth)
                sum -= queue.Dequeue().OriginalY;
        }
    }
}