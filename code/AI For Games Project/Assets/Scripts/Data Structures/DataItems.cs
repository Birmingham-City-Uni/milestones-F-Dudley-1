using System;
using System.Collections;
using System.Collections.Generic;

public interface IPQueueItem<T> : IComparable<T>
{
    int QueueIndex {
        get;
        set;
    }
}