using System.Collections.Generic;
using System.Linq;
using Greedy.Architecture;

namespace Greedy;

public class GreedyPathFinder : IPathFinder
{
    public List<Point> FindPathToCompleteGoal(State state)
    {
        if (state.Goal == 0 || state.Goal > state.Chests.Count)
            return new List<Point>();
        var pathFinder = new DijkstraPathFinder();
        var chests = new HashSet<Point>(state.Chests);
        var result = new List<Point>();
        for (var i = 0; i < state.Goal; i++)
        {
            var pathToChest = pathFinder.GetPathsByDijkstra(state, state.Position, chests).FirstOrDefault();
            if (pathToChest == null)
                return new List<Point>();
            state.Position = pathToChest.End;
            state.Energy -= pathToChest.Cost;
            if (state.Energy < 0)
                return new List<Point>();
            chests.Remove(pathToChest.End);
            for (var j = 1; j < pathToChest.Path.Count; j++)
                result.Add(pathToChest.Path[j]);
        }
        return result;
    }
}