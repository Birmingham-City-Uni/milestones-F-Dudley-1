using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// An Interface That Specifies A Heap's Item, and What is Should Contain.
/// </summary>
/// <typeparam name="T">The Assigned Type of The Object The Interface is Assigned To.</typeparam>
public interface IHeapItem<T> : System.IComparable<T>
{
    int HeapIndex
    {
        get;
        set;
    }
}

public class Heap<T> where T : IHeapItem<T>
{
    /// <summary>
    /// The Heap's Contents.
    /// </summary>
    T[] items;

    /// <summary>
    /// The Current Item Count of The Heap.
    /// </summary>
    int currentItemCount;

    /// <summary>
    /// The Current Item Count of The Heap.
    /// </summary>
    /// <value></value>
    public int Count
    {
        get
        {
            return currentItemCount;
        }
    }

    #region Main Functions

    /// <summary>
    /// The Heaps Constructor.
    /// </summary>
    /// <param name="_maxSize">The Max Overall Size The Heap Can Go To.</param>
    public Heap(int _maxSize)
    {
        items = new T[_maxSize];
    }

    /// <summary>
    /// Adds A New Item To The Heaps Items.
    /// </summary>
    /// <param name="newItem">The New Item To Add To The Heap.</param>
    public void Add(T newItem)
    {
        newItem.HeapIndex = currentItemCount;
        items[currentItemCount] = newItem;
        SortUp(newItem);
        currentItemCount++;
    }

    /// <summary>
    /// Updates an Item Currently On The Heap.
    /// </summary>
    /// <param name="item">The Item To Update On The Heap.</param>
    public void UpdateItem(T item)
    {
        SortUp(item);
    }

    /// <summary>
    /// Removes The Item On The Heap With The Lowest Index.
    /// </summary>
    /// <returns></returns>
    public T RemoveFirst()
    {
        T firstItem = items[0];
        currentItemCount--;

        items[0] = items[currentItemCount];
        items[0].HeapIndex = 0;
        SortDown(items[0]);

        return firstItem;
    }

    /// <summary>
    /// Checks if The Heap Contains a Certain Item.
    /// </summary>
    /// <param name="itemToLookFor">The Item To Check If Is Currently In The Heap.</param>
    /// <returns>True if The Item is On The Heap, False if It is Not.</returns>
    public bool Contains(T itemToLookFor)
    {
        return Equals(items[itemToLookFor.HeapIndex], itemToLookFor);
    }
    #endregion

    #region Sorting Functions

    /// <summary>
    /// Sorts The Heaps' Items Up Depending On Their Comparison Items.
    /// </summary>
    /// <param name="item">The Item To Sort Up The Heap.</param>
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

    /// <summary>
    /// Sorts The Heaps' Items Up Depends On Their Comparison Items.
    /// </summary>
    /// <param name="item">The Item To Sort Down The Heap.</param>
    private void SortDown(T item)
    {
        while (true)
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

    /// <summary>
    /// Swaps The Items Around In The Heap.
    /// </summary>
    /// <param name="itemA">The First Item To Swap In The Heap.</param>
    /// <param name="itemB">The Second Item To Swap In The Heap.</param>
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