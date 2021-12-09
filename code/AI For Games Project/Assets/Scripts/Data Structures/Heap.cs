using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IHeapItem<T> : System.IComparable<T>
{
    int HeapIndex {
        get;
        set;
    }
}

public class Heap<T> where T : IHeapItem<T>
{
    T[] items;
    int currentItemCount;

    public int Count
    {
        get {
            return currentItemCount;
        }
    }

    #region Main Functions
    public Heap(int _maxSize)
    {
        items = new T[_maxSize];
    }

    public void Add(T newItem)
    {
        newItem.HeapIndex = currentItemCount;
        items[currentItemCount] = newItem;
        SortUp(newItem);
        currentItemCount++;
    }

    public void UpdateItem(T item)
    {
        SortUp(item);
    }

    public T RemoveFirst()
    {
        T firstItem = items[0];
        currentItemCount--;

        items[0] = items[currentItemCount];
        items[0].HeapIndex = 0;
        SortDown(items[0]);

        return firstItem;
    }

    public bool Contains(T itemToLookFor)
    {
        return Equals(items[itemToLookFor.HeapIndex], itemToLookFor);
    }
    #endregion

    #region Sorting Functions
    private void SortUp(T item)
    {
        int parentIndex = (item.HeapIndex - 1) / 2;

        while (true)
        {
            T parentItem = items[parentIndex];
            if (item.CompareTo(parentItem) > 0)
            {
                Swap(item, parentItem);
            }
            else break;

            parentIndex = (item.HeapIndex - 1) / 2;
        }
    }

    private void SortDown(T item)
    {
        while(true)
        {
            int childIndexLeft = item.HeapIndex * 2 + 1;
            int childIndexRight = item.HeapIndex * 2 + 2;
            int swapIndex;

            if (childIndexLeft < currentItemCount)
            {
                swapIndex = childIndexLeft;

                if (childIndexRight < currentItemCount)
                {
                    if (items[childIndexLeft].CompareTo(items[childIndexRight]) < 0)
                    {
                        swapIndex = childIndexRight;
                    }
                }

                if (item.CompareTo(items[swapIndex]) < 0)
                {
                    Swap(item, items[swapIndex]);
                }
                else break;
            }
            else break;
        }
    }

    private void Swap(T itemA, T itemB)
    {
        items[itemA.HeapIndex] = itemB;
        items[itemB.HeapIndex] = itemA;

        int itemAIndex = itemA.HeapIndex;
        itemA.HeapIndex = itemB.HeapIndex;
        itemB.HeapIndex = itemAIndex;
    }
    #endregion
}
