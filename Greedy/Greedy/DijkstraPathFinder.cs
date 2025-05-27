using System;
using System.Collections.Generic;
using System.Linq;
using Greedy.Architecture;

namespace Greedy
{
    public class DijkstraData
    {
        public Point? Previous { get; set; }
        public int Price { get; set; }
    }

    public class DijkstraPathFinder
    {
        public IEnumerable<PathWithCost> GetPathsByDijkstra(
            State state,
            Point start,
            IEnumerable<Point> targets)
        {
            var chests = new HashSet<Point>(targets);
            var candidatesToOpen = new HashSet<Point> { start };
            var visitedNodes = new HashSet<Point>();
            var track = new Dictionary<Point, DijkstraData>
            {
                [start] = new DijkstraData { Price = 0, Previous = null }
            };

            while (true)
            {
                var toOpen = SelectNodeToOpen(candidatesToOpen, track);
                if (toOpen == null)
                    yield break;

                if (chests.Contains(toOpen.Value))
                    yield return MakePath(track, toOpen.Value);

                ProcessIncidentNodes(toOpen.Value, state, track, visitedNodes, candidatesToOpen);

                candidatesToOpen.Remove(toOpen.Value);
                visitedNodes.Add(toOpen.Value);
            }
        }

        private Point? SelectNodeToOpen(HashSet<Point> candidates, Dictionary<Point, DijkstraData> track)
        {
            Point? bestNode = null;
            int bestPrice = int.MaxValue;

            foreach (var point in candidates)
            {
                int price = track[point].Price;
                if (price < bestPrice)
                {
                    bestPrice = price;
                    bestNode = point;
                }
            }

            return bestNode;
        }

        private void ProcessIncidentNodes(
            Point current,
            State state,
            Dictionary<Point, DijkstraData> track,
            HashSet<Point> visited,
            HashSet<Point> candidates)
        {
            foreach (var neighbor in GetIncidentNodes(current, state))
            {
                int price = track[current].Price + state.CellCost[neighbor.X, neighbor.Y];
                if (!track.ContainsKey(neighbor) || price < track[neighbor].Price)
                {
                    track[neighbor] = new DijkstraData { Previous = current, Price = price };
                }

                if (!visited.Contains(neighbor))
                    candidates.Add(neighbor);
            }
        }

        public PathWithCost MakePath(Dictionary<Point, DijkstraData> track, Point end)
        {
            var result = new List<Point>();
            Point? current = end;

            while (current != null)
            {
                result.Add(current.Value);
                current = track[current.Value].Previous;
            }

            result.Reverse();
            return new PathWithCost(track[end].Price, result.ToArray());
        }

        public IEnumerable<Point> GetIncidentNodes(Point node, State state)
        {
            return new Point[]
            {
                new Point(node.X, node.Y + 1),
                new Point(node.X, node.Y - 1),
                new Point(node.X + 1, node.Y),
                new Point(node.X - 1, node.Y)
            }.Where(p => state.InsideMap(p) && !state.IsWallAt(p));
        }
    }
}
