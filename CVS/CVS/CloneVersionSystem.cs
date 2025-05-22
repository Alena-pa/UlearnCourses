using System;
using System.Collections.Generic;

namespace Clones
{
    public class CloneVersionSystem : ICloneVersionSystem
    {
        private class CustomStack
        {
            private class Node
            {
                public string Value { get; }
                public Node Next { get; }

                public Node(string value, Node next = null)
                {
                    Value = value;
                    Next = next;
                }
            }

            private readonly Node head;
            private readonly int count;

            public CustomStack()
            {
                head = null;
                count = 0;
            }

            public CustomStack(string value, CustomStack other)
            {
                head = new Node(value, other.head);
                count = other.count + 1;
            }

            private CustomStack(Node newHead, int newCount)
            {
                head = newHead;
                count = newCount;
            }

            public int Count => count;

            public CustomStack Push(string item)
            {
                return new CustomStack(item, this);
            }

            public CustomStack Pop(out string value)
            {
                if (count == 0)
                {
                    value = null;
                    return this;
                }

                value = head.Value;
                return new CustomStack(head.Next, count - 1);
            }

            public string Peek()
            {
                return count > 0 ? head.Value : null;
            }

            public CustomStack Clear()
            {
                return new CustomStack();
            }
        }

        private class Clone
        {
            public CustomStack Learned { get; }
            public CustomStack RolledBack { get; }

            public Clone(CustomStack learned, CustomStack rolledBack)
            {
                Learned = learned;
                RolledBack = rolledBack;
            }
        }

        private readonly List<Clone> clones;

        public CloneVersionSystem()
        {
            clones = new List<Clone> { new Clone(new CustomStack(), new CustomStack()) };
        }

        public string Execute(string query)
        {
            if (!TryParseQuery(query, out string command, out int id, out string argument))
                return null;

            switch (command)
            {
                case "learn": return Learn(id, argument);
                case "rollback": return Rollback(id);
                case "relearn": return Relearn(id);
                case "clone": return CloneNew(id);
                case "check": return Check(id);
                default: return null;
            }
        }

        private bool TryParseQuery(string query, out string command, out int id, out string argument)
        {
            command = null;
            id = -1;
            argument = null;

            if (string.IsNullOrWhiteSpace(query))
                return false;

            var parts = query.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            if (parts.Length < 2)
                return false;

            command = parts[0];
            if (!int.TryParse(parts[1], out id) || id < 1 || id > clones.Count)
                return false;

            id--; // приведение к индексации с 0
            if (parts.Length > 2)
                argument = parts[2];

            return true;
        }

        private string Learn(int id, string program)
        {
            if (program == null) return null;
            clones[id] = new Clone(
                clones[id].Learned.Push(program),
                clones[id].RolledBack.Clear());
            return null;
        }

        private string Rollback(int id)
        {
            if (clones[id].Learned.Count > 0)
            {
                var newLearned = clones[id].Learned.Pop(out string last);
                clones[id] = new Clone(newLearned, clones[id].RolledBack.Push(last));
            }
            return null;
        }

        private string Relearn(int id)
        {
            if (clones[id].RolledBack.Count > 0)
            {
                var newRolledBack = clones[id].RolledBack.Pop(out string prog);
                clones[id] = new Clone(clones[id].Learned.Push(prog), newRolledBack);
            }
            return null;
        }

        private string CloneNew(int id)
        {
            clones.Add(new Clone(clones[id].Learned, clones[id].RolledBack));
            return null;
        }

        private string Check(int id)
        {
            string top = clones[id].Learned.Peek();
            return top ?? "basic";
        }
    }
}