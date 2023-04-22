using System;
using System.Collections.Generic;
using System.Linq;

namespace Dungeon
{
    public class DungeonTask
    {
        public static MoveDirection[] FindShortestPath(Map map)
        {
            var start = map.InitialPosition;
            var exit = map.Exit;
            var chests = map.Chests;

            var pathsToExit = BfsTask.FindPaths(map, start, new Point[] { exit }).FirstOrDefault();
            if (pathsToExit == null)
                return Array.Empty<MoveDirection>();
            var moveToExit = pathsToExit.ToList();
            moveToExit.Reverse();
            if (chests.Any(chest => moveToExit.Contains(chest)))
                return ConvertPointsToDirection(moveToExit);
            var pathFromStartToChests = BfsTask.FindPaths(map, start, chests);
            var pathFromStartToExit = BfsTask.FindPaths(map, exit, chests).DefaultIfEmpty();
            if (pathFromStartToChests.FirstOrDefault() == null)
                return ConvertPointsToDirection(moveToExit);
            var bestPath = FindShortestPathFromStartToExit(pathFromStartToChests, pathFromStartToExit);
            return ConvertPointsToDirection(bestPath.ToList());
        }

        private static IEnumerable<Point> FindShortestPathFromStartToExit(
            IEnumerable<SinglyLinkedList<Point>> pathFromStartToChests,
            IEnumerable<SinglyLinkedList<Point>> pathFromStartToExit)
        {
            var routeStartToExit =
                pathFromStartToChests.Join(pathFromStartToExit, f => f.Value, s => s.Value, (f, s) => new
                {
                    Length = f.Length + s.Length,
                    ListFinish = f.ToList(),
                    ListStart = s.ToList()
                });

            var bestPath = routeStartToExit.OrderBy(l => l.Length).First();
            bestPath.ListFinish.Reverse();
            bestPath.ListFinish.AddRange(bestPath.ListStart.Skip(1));
            return bestPath.ListFinish;
        }

        private static MoveDirection[] ConvertPointsToDirection(List<Point> points)
        {
            var directionList = new LinkedList<MoveDirection>();
            for (var i = 0; i < points.Count - 1; i++)
            {
                var currentPoint = points.Skip(i).FirstOrDefault();
                var nextPoint = points.Skip(i + 1).FirstOrDefault();
                directionList.AddLast(OffsetToDirection(currentPoint, nextPoint));
            }

            return directionList.ToArray();
        }

        private static MoveDirection OffsetToDirection(Point currentPoint, Point nextPoint)
        {
            var point = new Point(currentPoint.X - nextPoint.X, currentPoint.Y - nextPoint.Y);
            if (point.X == 1) return MoveDirection.Left;
            if (point.X == -1) return MoveDirection.Right;
            if (point.Y == 1) return MoveDirection.Up;
            if (point.Y == -1) return MoveDirection.Down;
            throw new ArgumentException();
        }
    }
}