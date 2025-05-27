using System.Collections.Generic;
using System.Linq;
using Greedy.Architecture;

namespace Greedy
{
    public class NotGreedyPathFinder : IPathFinder
    {
        public List<Point> FindPathToCompleteGoal(State state)
        {
            var finder = new DijkstraPathFinder();
            var pathsThroughAllChests = new Stack<PathWithCost>();
            var notUsedChests = new HashSet<Point>(state.Chests);
            var dictionaryOfPaths = new Dictionary<(Point start, Point end), PathWithCost>();
            List<PathWithCost> bestPath = null;

            foreach (var chest in state.Chests)
            {
                var (result, updatedBestPath) = FindPathByFirstChest(
                    notUsedChests, state.Position, chest, 0,
                    state, pathsThroughAllChests, dictionaryOfPaths, finder, bestPath);
                bestPath = updatedBestPath;
                if (result != null)
                    return result;
            }

            return bestPath != null ? MakePath(bestPath) : new List<Point>();
        }

        private (List<Point> path, List<PathWithCost> bestPath) FindPathByFirstChest(
            HashSet<Point> notUsedChests, Point previousNode, Point currentChest,
            int cost, State state, Stack<PathWithCost> pathsThroughAllChests,
            Dictionary<(Point start, Point end), PathWithCost> dictionaryOfPaths,
            DijkstraPathFinder finder, List<PathWithCost> bestPath)
        {
            if (!TryMoveToChest(previousNode, currentChest, ref cost, notUsedChests,
                pathsThroughAllChests, dictionaryOfPaths, state, finder, out _))
                return (null, bestPath);

            if (ShouldContinue(cost, state, notUsedChests))
            {
                if (TryFindPathToAllChests(notUsedChests, currentChest, cost, state,
                    pathsThroughAllChests, dictionaryOfPaths, finder, ref bestPath, out var result))
                    return (result, bestPath);
            }

            if (IsSuccessfulPath(cost, state, notUsedChests))
                return (MakePath(pathsThroughAllChests.ToList()), bestPath);

            UndoMove(currentChest, notUsedChests, pathsThroughAllChests);
            bestPath = UpdateBestPath(bestPath, pathsThroughAllChests);
            return (null, bestPath);
        }

        private bool TryMoveToChest(
            Point previousNode, Point currentChest, ref int cost,
            HashSet<Point> notUsedChests, Stack<PathWithCost> pathsThroughAllChests,
            Dictionary<(Point start, Point end), PathWithCost> dictionaryOfPaths,
            State state, DijkstraPathFinder finder, out PathWithCost pathToCurrentChest)
        {
            pathToCurrentChest = PathToNode(previousNode, currentChest, dictionaryOfPaths, state, finder);
            if (pathToCurrentChest == null)
                return false;

            cost += pathToCurrentChest.Cost;
            notUsedChests.Remove(currentChest);
            pathsThroughAllChests.Push(pathToCurrentChest);
            return true;
        }

        private void UndoMove(Point chest,
            HashSet<Point> notUsedChests, Stack<PathWithCost> pathsThroughAllChests)
        {
            notUsedChests.Add(chest);
            pathsThroughAllChests.Pop();
        }

        private bool ShouldContinue(int cost, State state, HashSet<Point> notUsedChests)
        {
            return cost <= state.Energy && notUsedChests.Count > 0;
        }

        private bool IsSuccessfulPath(int cost, State state, HashSet<Point> notUsedChests)
        {
            return cost <= state.Energy && notUsedChests.Count == 0;
        }

        private bool TryFindPathToAllChests(
            HashSet<Point> notUsedChests, Point currentChest, int cost, State state,
            Stack<PathWithCost> pathsThroughAllChests,
            Dictionary<(Point start, Point end), PathWithCost> dictionaryOfPaths,
            DijkstraPathFinder finder, ref List<PathWithCost> bestPath,
            out List<Point> result)
        {
            foreach (var nextChest in notUsedChests.ToList())
            {
                var (path, updatedBestPath) = FindPathByFirstChest(
                    notUsedChests, currentChest, nextChest, cost,
                    state, pathsThroughAllChests, dictionaryOfPaths, finder, bestPath);
                bestPath = updatedBestPath;
                if (path != null)
                {
                    result = path;
                    return true;
                }
            }
            result = null;
            return false;
        }

        private List<PathWithCost> UpdateBestPath(
            List<PathWithCost> bestPath, Stack<PathWithCost> pathsThroughAllChests)
        {
            if (bestPath == null || bestPath.Count < pathsThroughAllChests.Count)
                return pathsThroughAllChests.ToList();
            return bestPath;
        }

        private List<Point> MakePath(List<PathWithCost> pathsThroughAllChests)
        {
            var result = new List<Point>();
            for (int i = pathsThroughAllChests.Count - 1; i >= 0; i--)
            {
                var path = pathsThroughAllChests[i];
                for (int j = 1; j < path.Path.Count; j++)
                    result.Add(path.Path[j]);
            }
            return result;
        }

        private PathWithCost PathToNode(
            Point previousNode, Point currentChest,
            Dictionary<(Point start, Point end), PathWithCost> dictionaryOfPaths,
            State state, DijkstraPathFinder finder)
        {
            if (dictionaryOfPaths.TryGetValue((previousNode, currentChest), out var path))
                return path;

            path = finder.GetPathsByDijkstra(state, previousNode, new[] { currentChest }).FirstOrDefault();
            dictionaryOfPaths[(previousNode, currentChest)] = path;
            return path;
        }
    }
}
