using System;
using System.Collections.Generic;

namespace LimitedSizeStack;

public class ListModel<TItem>
{
    private enum ActionType { Add, Remove };

    private class Action
    {
        public ActionType Type { get; }
        public TItem Item { get; }
        public int Index { get; }

        public Action(ActionType type, TItem item, int index)
        {
            Type = type;
            Item = item;
            Index = index;
        }
    }

    public List<TItem> Items { get; }
    public int UndoLimit;

    private readonly LimitedSizeStack<Action> history;

    public ListModel(int undoLimit) : this(new List<TItem>(), undoLimit)
    {
    }

    public ListModel(List<TItem> items, int undoLimit)
    {
        Items = items;
        UndoLimit = undoLimit;
        history = new LimitedSizeStack<Action>(undoLimit);
    }

    public void AddItem(TItem item)
    {
        Items.Add(item);
        history.Push(new Action(ActionType.Add, item, Items.Count - 1));
    }

    public void RemoveItem(int index)
    {
        TItem removedItem = Items[index];
        Items.RemoveAt(index);
        history.Push(new Action(ActionType.Remove, removedItem, index));
    }

    public bool CanUndo()
    {
        return history.Count > 0;
    }

    public void Undo()
    {
        if (!CanUndo())
        {
            throw new InvalidOperationException("Nothing to undo");
        }
        var action = history.Pop();

        if (action.Type == ActionType.Add)
        {
            Items.RemoveAt(action.Index);
        }
        else if (action.Type == ActionType.Remove)
        {
            Items.Insert(action.Index, action.Item);
        }
    }
}