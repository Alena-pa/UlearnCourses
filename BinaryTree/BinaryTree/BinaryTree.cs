using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BinaryTrees
{
    public class BinaryTree<T> : IEnumerable<T> where T : IComparable
    {
        T Value;
        int Weight = 1;
        BinaryTree<T> Left;
        BinaryTree<T> Right;
        bool isInitialized = false;
        public BinaryTree() { }
        BinaryTree(T value)
        {
            Value = value;
            isInitialized = true;
        }
        public void Add(T key)
        {
            if (!isInitialized)
            {
                Value = key;
                isInitialized = true;
                return;
            }
            BinaryTree<T> parentNodeToAdd = this;
            while (true)
            {
                parentNodeToAdd.Weight++;
                if (parentNodeToAdd.Value.CompareTo(key) > 0)
                {
                    if (parentNodeToAdd.Left != null)
                        parentNodeToAdd = parentNodeToAdd.Left;
                    else
                    {
                        parentNodeToAdd.Left = new BinaryTree<T>(key);
                        break;
                    }
                }
                else
                {
                    if (parentNodeToAdd.Right != null)
                        parentNodeToAdd = parentNodeToAdd.Right;
                    else
                    {
                        parentNodeToAdd.Right = new BinaryTree<T>(key);
                        break;
                    }
                }
            }
        }
        public bool Contains(T key)
        {
            if (!isInitialized)
                return false;
            BinaryTree<T> parentNodeToAdd = this;
            int compareResult;
            while (true)
            {
                compareResult = parentNodeToAdd.Value.CompareTo(key);
                if (compareResult == 0)
                    return true;
                if (compareResult > 0)
                {
                    if (parentNodeToAdd.Left != null)
                        parentNodeToAdd = parentNodeToAdd.Left;
                    else
                        return false;
                }
                else
                {
                    if (parentNodeToAdd.Right != null)
                        parentNodeToAdd = parentNodeToAdd.Right;
                    else
                        return false;
                }
            }
        }

        public T this[int i]
        {
            get
            {
                BinaryTree<T> root = this;
                int parentWeight = 0;
                while (true)
                {
                    int currentNodeIndex = (root.Left == null ? 0 : root.Left.Weight) + parentWeight;
                    if (i == currentNodeIndex)
                        return root.Value;
                    if (i < currentNodeIndex)
                        root = root.Left;
                    else
                    {
                        root = root.Right;
                        parentWeight = currentNodeIndex + 1;
                    }
                }
            }
        }

        public IEnumerator<T> GetEnumerator() => GetEnumeratorForNode(this);

        IEnumerator<T> GetEnumeratorForNode(BinaryTree<T> root)
        {
            if (root == null || !root.isInitialized)
                yield break;
            var enumeratorForTreeNode = GetEnumeratorForNode(root.Left);
            while (enumeratorForTreeNode.MoveNext())
                yield return enumeratorForTreeNode.Current;
            yield return root.Value;
            enumeratorForTreeNode = GetEnumeratorForNode(root.Right);
            while (enumeratorForTreeNode.MoveNext())
                yield return enumeratorForTreeNode.Current;
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}