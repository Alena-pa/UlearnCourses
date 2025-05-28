using System;
using System.Collections.Generic;
using System.Linq;

namespace DiskTree
{
    public class Node
    {
        public Dictionary<string, Node> Children = new Dictionary<string, Node>();
    }

    public class DiskTreeTask
    {
        public static List<string> Solve(List<string> input)
        {
            Node roots = new Node();

            foreach (var path in input)
            {
                string[] folders = path.Split(new[] { '\\' }, StringSplitOptions.RemoveEmptyEntries);
                Node root = roots;
                for (int i = 0; i < folders.Length; i++)
                {
                    if (!root.Children.ContainsKey(folders[i]))
                        root.Children[folders[i]] = new Node();
                    root = root.Children[folders[i]];
                }
            }

            List<string> result = new List<string>();
            CreateListFromNode(roots, result, "");
            return result;
        }

        public static void CreateListFromNode(Node roots, List<string> result, string prefix)
        {
            if (roots.Children.Count == 0)
                return;

            var listOfRoots = roots.Children.Keys.ToList();
            listOfRoots.Sort((first, second) => string.CompareOrdinal(first, second));
            foreach (var root in listOfRoots)
            {
                result.Add(prefix + root);
                CreateListFromNode(roots.Children[root], result, prefix + " ");
            }
        }
    }
}
