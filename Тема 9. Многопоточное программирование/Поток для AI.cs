using System;
using System.Collections.Generic;
using System.Linq;

namespace rocket_bot;

public class Channel<T> where T : class
{
    private readonly object lockObj = new();
    private List<T> elementsList = new();
    public T this[int index]
    {
        get
        {
            lock (lockObj)
            {
                if (index > elementsList.Count - 1 || elementsList.Count == 0)
                    return null;
                return elementsList[index];
            }
        }

        set
        {
            lock (lockObj)
            {
                if (index == elementsList.Count)
                {
                    elementsList.Add(value);
                }
                else if (index < elementsList.Count)
                {
                    elementsList.RemoveRange(index, elementsList.Count - index);
                    elementsList.Add(value);
                }
            }
        }
    }

    public T LastItem()
    {
        lock (lockObj)
        {
            if (elementsList.Count == 0)
                return null;
            return elementsList.Last();
        }
    }

    public void AppendIfLastItemIsUnchanged(T item, T knownLastItem)
    {
        lock (lockObj)
        {
            if (elementsList.Count != 0 && knownLastItem == elementsList.Last() || elementsList.Count == 0)
            {
                elementsList.Add(item);
            }
        }
    }

    public int Count
    {
        get
        {
            lock (lockObj)
            {
                return elementsList.Count;
            }
        }
    }
}