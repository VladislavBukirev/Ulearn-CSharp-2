using System.Collections.Generic;
using System.Linq;

namespace Dungeon
{
    public class BfsTask
    {
        public static IEnumerable<SinglyLinkedList<Point>> FindPaths(Map map, Point start, Point[] chests)
        {
            var road = new Dictionary<Point, SinglyLinkedList<Point>>();
            var visited = new HashSet<Point>();
            var queue = new Queue<Point>();
            var chestsSet = new HashSet<Point>(chests);

            queue.Enqueue(start);
            visited.Add(start);
            road.Add(start, new SinglyLinkedList<Point>(start));

            while (queue.Count != 0)
            {
                var point = queue.Dequeue();
                if (!map.InBounds(point) || map.Dungeon[point.X, point.Y] == MapCell.Wall)
                    continue;
                var incidentNodes = GetIncidentNodes(point);
                EnqueueUnvisitedNodes(queue, visited, incidentNodes);
                UpdateRoad(road, point, incidentNodes);
                if (!chestsSet.Contains(point) || !road.ContainsKey(point)) continue;
                chestsSet.Remove(point);
                yield return road[point];
            }
        }

        private static IEnumerable<Point> GetIncidentNodes(Point point)
        {
            for (var dx = -1; dx <= 1; dx++)
            for (var dy = -1; dy <= 1; dy++)
            {
                if ((dx + dy) % 2 == 0)
                    continue;
                var nextPoint = new Point { X = point.X + dx, Y = point.Y + dy };
                yield return nextPoint;
            }
        }

        private static void EnqueueUnvisitedNodes(Queue<Point> queue, HashSet<Point> visited, IEnumerable<Point> nodes)
        {
            foreach (var node in nodes)
            {
                if (visited.Contains(node)) continue;
                queue.Enqueue(node);
                visited.Add(node);
            }
        }

        private static void UpdateRoad
            (Dictionary<Point, SinglyLinkedList<Point>> road, Point point, IEnumerable<Point> incidentNodes)
        {
            foreach (var node in incidentNodes)
            {
                if (!road.ContainsKey(node))
                    road.Add(node, new SinglyLinkedList<Point>(node, road[point]));
            }
        }
    }
}