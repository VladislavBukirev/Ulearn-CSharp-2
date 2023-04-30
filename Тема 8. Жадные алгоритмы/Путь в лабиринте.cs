using System;
using System.Collections.Generic;
using System.Linq;
using Greedy.Architecture;

namespace Greedy;

public class DijkstraData
{
    public Point Previous { get; set; }
    public int Price { get; set; }
}

public class DijkstraPathFinder
{
    public IEnumerable<PathWithCost> GetPathsByDijkstra(State state, Point start,
        IEnumerable<Point> targets)
    {
        var chestsCoordinates = new HashSet<Point>(targets);
        var track = new Dictionary<Point, DijkstraData>();
        track[start] = new DijkstraData { Previous = new Point(0, 0), Price = 0 };

        var visited = new List<Point>();
        while (chestsCoordinates.Count > 0)
        {
            var pointOutOfPath = new Point(state.MapWidth + 1, state.MapHeight + 1);
            var toOpen = UnpackNode(state, track, visited, pointOutOfPath);
            if (toOpen == pointOutOfPath)
                break;
            if (chestsCoordinates.Contains(toOpen))
            {
                yield return MakePath(track, toOpen, start);
                chestsCoordinates.Remove(toOpen);
            }
            var incidentNodes = GetIncidentNodes(toOpen, state);
            AddIncidentNodes(incidentNodes, track, toOpen, state, visited);
        }
    }

    private Point UnpackNode(State state, Dictionary<Point, DijkstraData> track, List<Point> visited, Point toOpen)
    {
        var bestPrice = double.PositiveInfinity;
        foreach (var point in track
                     .Where(point 
                         => !visited.Contains(point.Key) && point.Value.Price < bestPrice))
        {
            bestPrice = point.Value.Price;
            toOpen = point.Key;
        }

        return toOpen;
    }

    private static void AddIncidentNodes(IEnumerable<Point> incidentNodes, Dictionary<Point, DijkstraData> track
        ,Point toOpen, State state, List<Point> visited)
    {
        foreach (var incidentNode in incidentNodes)
        {
            var price = track[toOpen].Price + state.CellCost[incidentNode.X, incidentNode.Y];
            if (!track.ContainsKey(incidentNode) || track[incidentNode].Price > price)
                track[incidentNode] = new DijkstraData { Previous = toOpen, Price = price };
        }
        visited.Add(toOpen);
    }

    private PathWithCost MakePath(Dictionary<Point, DijkstraData> track, Point end, Point start)
    {
        var result = new List<Point>();
        var currentPoint = end;
        while (currentPoint != start)
        {
            result.Add(currentPoint);
            currentPoint = track[currentPoint].Previous;
        }

        result.Add(start);
        result.Reverse();
        var pathResult = new PathWithCost(track[end].Price, result.ToArray());
        return pathResult;
    }

    private static IEnumerable<Point> GetIncidentNodes(Point node, State state)
    {
        return new[]
        {
            new Point(node.X, node.Y + 1),
            new Point(node.X, node.Y - 1),
            new Point(node.X + 1, node.Y),
            new Point(node.X - 1, node.Y)
        }.Where(point => state.InsideMap(point) && !state.IsWallAt(point));
    }
}