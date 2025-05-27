using System.Collections.Generic;
using System.Linq;
using Greedy.Architecture;

namespace Greedy
{
    public class GreedyPathFinder : IPathFinder
    {
        public List<Point> FindPathToCompleteGoal(State state)
        {
            if (state.Goal == 0)
                return new List<Point>();

            var chests = new HashSet<Point>(state.Chests);
            var finder = new DijkstraPathFinder();
            var result = new List<Point>();
            var position = state.Position;
            var currentCost = 0;

            for (int i = 0; i < state.Goal; i++)
            {
                var path = FindNextChestPath(state, position, chests, finder);
                if (path == null)
                    return new List<Point>();

                if (!TryAddPath(result, path, ref currentCost, state.Energy))
                    return new List<Point>();

                position = path.End;
                chests.Remove(path.End);
            }

            return result;
        }

        private PathWithCost FindNextChestPath(
        State state,
        Point current,
        HashSet<Point> chests,
        DijkstraPathFinder finder)
        {
            return finder.GetPathsByDijkstra(state, current, chests).FirstOrDefault();
        }

        private bool TryAddPath(List<Point> result, PathWithCost path, ref int currentCost, int maxEnergy)
        {
            currentCost += path.Cost;
            if (currentCost > maxEnergy)
                return false;

            for (int i = 1; i < path.Path.Count; i++)
                result.Add(path.Path[i]);

            return true;
        }
    }
}
