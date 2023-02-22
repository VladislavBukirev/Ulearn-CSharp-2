using System;
using System.Collections.Generic;
using System.Linq;

namespace yield;

public static class MovingMaxTask
{
    public static IEnumerable<DataPoint> MovingMax(this IEnumerable<DataPoint> data, int windowWidth)
    {
        var window = new Queue<DataPoint>();
        var potentialMaximums = new LinkedList<DataPoint>();
        foreach (var point in data)
        {
            var currentPoint = new DataPoint(point);
            if (window.Count == windowWidth)
            {
                var deletedValue = window.Dequeue();
                if (potentialMaximums.Contains(deletedValue))
                    potentialMaximums.Remove(deletedValue);
            }
            window.Enqueue(currentPoint);
            while(potentialMaximums.Count != 0 && currentPoint.OriginalY > potentialMaximums.Last.Value.OriginalY)
                potentialMaximums.RemoveLast();
            potentialMaximums.AddLast(currentPoint);
            yield return currentPoint.WithMaxY(potentialMaximums.First.Value.OriginalY);
        }
    }
}