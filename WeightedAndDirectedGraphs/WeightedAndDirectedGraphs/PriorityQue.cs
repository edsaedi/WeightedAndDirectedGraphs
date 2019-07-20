using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WeightedAndDirectedGraphs
{
    public class PriorityQue<T>
    {
        T[] tree = new T[30];
        IComparer<T> comparer;

        public bool IsEmpty() => Count == 0;
        public int Count { get; private set; }

        public bool Contains(T item) => tree.Contains(item);

        public T this[int i]
        {
            get
            {
                if (i > -1 && i < Count)
                {
                    return tree[i];
                }

                throw new ArgumentOutOfRangeException();
            }
        }
    }
}
