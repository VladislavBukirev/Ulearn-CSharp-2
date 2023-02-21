using System;
using System.Collections.Generic;

namespace LimitedSizeStack;

public class ListModel<TItem>
{
    public List<TItem> Items { get; }
    private LimitedSizeStack<Tuple<TItem, int, OperationType>> limitedItems;
    
    private enum OperationType
    {
        Add,
        Remove
    }

    public ListModel(int undoLimit) : this(new List<TItem>(), undoLimit)
    {
    }

    public ListModel(List<TItem> items, int undoLimit)
    {
        Items = items;
        limitedItems = new LimitedSizeStack<Tuple<TItem, int, OperationType>>(undoLimit);
    }

    public void AddItem(TItem item)
    {
        var tuple = new Tuple<TItem, int, OperationType>(item, Items.Count, OperationType.Add);
        limitedItems.Push(tuple);
        Items.Add(item);
    }

    public void RemoveItem(int index)
    {
        var tuple = new Tuple<TItem, int, OperationType>(Items[index], index, OperationType.Remove);
        limitedItems.Push(tuple);
        Items.RemoveAt(index);
    }

    public bool CanUndo()
    {
        return limitedItems.Count > 0;
    }

    public void Undo()
    {
        var lastExtension = limitedItems.Pop();
        switch (lastExtension.Item3)
        {
            case OperationType.Add:
                Items.RemoveAt(lastExtension.Item2);
                break;
            case OperationType.Remove:
                Items.Insert(lastExtension.Item2, lastExtension.Item1);
                break;
        }
    }
}