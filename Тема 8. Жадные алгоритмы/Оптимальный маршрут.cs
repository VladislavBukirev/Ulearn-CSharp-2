using Greedy.Architecture;
using System.Collections.Generic;
using System.Linq;

namespace Greedy
{
    public class NotGreedyPathFinder : IPathFinder
    {
        public List<Point> FindPathToCompleteGoal(State state)
        {
            var allEdges = GetAllEdges(state, state.Chests.Append(state.Position));
            var bestPath = FindBestPath(state, allEdges);
            return GetPathFromBestPath(bestPath, allEdges);
        }

        private Dictionary<(Point, Point), PathWithCost> GetAllEdges(State state, IEnumerable<Point> points)
        {
            var pathFinder = new DijkstraPathFinder();
            var allPaths = points.SelectMany(start =>
                pathFinder.GetPathsByDijkstra(state, start, points.Where(end => start != end)));
            return allPaths.ToDictionary(path => (path.Start, path.End), path => path);
        }

        private LinkedPath FindBestPath(State state, Dictionary<(Point, Point), PathWithCost> edges)
        {
            var stack = new Stack<LinkedPath>();
            stack.Push(new LinkedPath(null, state.Position, 0, state.Chests));
            var bestPath = stack.Peek();
            while (stack.Count > 0)
            {
                var currentPath = stack.Pop();
                if (currentPath.Cost > state.Energy)
                    continue;
                if (currentPath.Score > bestPath.Score)
                    bestPath = currentPath;
                foreach (var point in currentPath.NotVisited)
                    stack.Push(currentPath.MoveTo(edges[(currentPath.Position, point)]));
            }

            return bestPath;
        }

        private List<Point> GetPathFromBestPath(LinkedPath bestPath, Dictionary<(Point, Point), PathWithCost> edges)
        {
            var path = bestPath.GetPath();
            var track = new List<Point>();
            for (var i = 0; i < path.Count - 1; i++)
            {
                var start = path[i];
                var end = path[i + 1];
                var edge = edges[(start, end)];
                track.AddRange(edge.Path.Skip(1));
            }

            return track;
        }


        public class LinkedPath
        {
            public LinkedPath(LinkedPath previous, Point position, int cost, HashSet<Point> notVisited, int score = 0)
            {
                this.previous = previous;
                Position = position;
                Cost = cost;
                Score = score;
                NotVisited = notVisited;
            }

            private readonly LinkedPath previous;
            public readonly Point Position;
            public readonly HashSet<Point> NotVisited;
            public readonly int Cost;
            public readonly int Score;

            public LinkedPath MoveTo(PathWithCost edge)
            {
                var newVisited = new HashSet<Point>(NotVisited);
                newVisited.Remove(edge.End);
                var newPath = new LinkedPath(this, edge.End, Cost + edge.Cost, newVisited, Score + 1);
                return newPath;
            }

            public List<Point> GetPath()
            {
                var path = new List<Point>();
                var current = this;
                while (current != null)
                {
                    path.Add(current.Position);
                    current = current.previous;
                }

                path.Reverse();
                return path;
            }
        }
    }
}