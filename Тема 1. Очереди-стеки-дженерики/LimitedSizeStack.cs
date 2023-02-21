using System;
using System.Collections.Generic;
using System.Net.Sockets;

namespace LimitedSizeStack;

public class LimitedSizeStack<T>
{
    private LinkedList<T> list = new();
    private int stackSize;

    public LimitedSizeStack(int undoLimit)
    {
        stackSize = undoLimit;
    }

    public void Push(T item)
    {
        list.AddLast(item);
        if (list.Count > stackSize)
            list.RemoveFirst();
    }

    public T Pop()
    {
        if (list.Count == 0)
            throw new InvalidOperationException();
        var result = list.Last;
        list.RemoveLast();
        return result.Value;
    }

    public int Count => list.Count;
}