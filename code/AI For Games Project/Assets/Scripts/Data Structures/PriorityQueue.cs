using System.Collections;
using System.Collections.Generic;

public class PriorityQueue<T> where T : IPQueueItem<T>
{
    T[] queueItems;
    private int currentItemCount;

    #region Queuing Functions
    public PriorityQueue(int _maxSize)
    {
        queueItems = new T[_maxSize];
    }

    public void Enqueue(T _newItem)
    {
        _newItem.QueueIndex = currentItemCount;
        queueItems[currentItemCount] = _newItem;
        SortUp(_newItem);

        currentItemCount++;
    }

    public T Dequeue()
    {
        T dequeuedItem = queueItems[0];
        currentItemCount--;

        queueItems[0] = queueItems[currentItemCount];
        queueItems[0].QueueIndex = 0;
        SortDown(queueItems[0]);

        return dequeuedItem;
    }

    public bool Contains(T _itemToCheck)
    {
        return Equals(queueItems[_itemToCheck.QueueIndex], _itemToCheck);
    }

    public int Count => currentItemCount;
    #endregion

    #region Sorting Functions
    public void UpdateItem(T _itemToUpdate)
    {
        SortUp(_itemToUpdate);
    }

    private void SortUp(T _itemToSort)
    {
        int parentIndex = (_itemToSort.QueueIndex - 1) / 2;

        while (true)
        {
            T parentItem = queueItems[parentIndex];

            if (_itemToSort.CompareTo(parentItem) > 0)
            {
                Swap(_itemToSort, parentItem);
            }
            else break;

            parentIndex = (_itemToSort.QueueIndex - 1) / 2;
        }
    }

    private void SortDown(T _itemToSort)
    {
        while (true)
        {
            int childIndexLeft = _itemToSort.QueueIndex * 2 + 1;
            int childIndexRight = _itemToSort.QueueIndex * 2 + 2;
            int swapIndex = 0;

            if (childIndexLeft < currentItemCount)
            {
                swapIndex = childIndexLeft;

                if (childIndexRight < currentItemCount)
                {
                    if (queueItems[childIndexLeft].CompareTo(queueItems[childIndexRight]) < 0)
                    {
                        swapIndex = childIndexRight;
                    }
                }

                if (_itemToSort.CompareTo(queueItems[swapIndex]) < 0) Swap(_itemToSort, queueItems[swapIndex]);
                else return;
            }
            else return;
        }
    }

    private void Swap(T _itemA, T _itemB)
    {
        int itemAIndex = _itemA.QueueIndex;
        _itemA.QueueIndex = _itemB.QueueIndex;
        _itemB.QueueIndex = itemAIndex;

        queueItems[_itemA.QueueIndex] = _itemB;
        queueItems[_itemB.QueueIndex] = _itemB;
    }
    #endregion
}
