using System;
using System.Collections.Generic;

namespace LimitedSizeStack;

public class LimitedSizeStack<T>
{
    private readonly LinkedList<T> items;
    private readonly int maxSize;

    public LimitedSizeStack(int undoLimit)
    {
        items = new LinkedList<T>();
        maxSize = Math.Max(0, undoLimit);
    }

    public void Push(T item)
    {
        if (maxSize == 0) return;

        if (items.Count >= maxSize)
            items.RemoveFirst(); // O(1)

        items.AddLast(item); // O(1)
    }

    public T Pop()
    {
        if (items.Count == 0)
            throw new InvalidOperationException("Stack is empty");

        T item = items.Last.Value;
        items.RemoveLast();
        return item;
    }

    public int Count => items.Count;
}
