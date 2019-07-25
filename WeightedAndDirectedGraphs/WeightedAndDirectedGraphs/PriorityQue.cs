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

        public PriorityQue(IComparer<T> comparer)
        {
            this.comparer = comparer ?? Comparer<T>.Default;
            Count = 0;
        }

        public void Enqueu(T value)
        {
            Count++;
            if (Count >= tree.Length)
            {
                IncreaseTree();
            }

            tree[Count] = value;

            HeapifyUp(Count);
        }

        public T Deqeue()
        {
            Sort();
            T root = tree[1];

            tree[1] = tree[Count];
            tree[Count] = default(T);

            Count--;

            HeapifyDown(1);

            return root;
        }

        public void Sort()
        {
            for (int i = Count / 2; i > 0; i--)
            {
                HeapifyDown(i);
            }
        }

        private void HeapifyUp(int index)
        {
            int parent = index / 2;

            if (parent < 0)
            {
                return;
            }

            if (comparer.Compare(tree[index], tree[parent]) < 0)
            {
                T temp = tree[index];
                tree[index] = tree[parent];
                tree[parent] = temp;
            }

            HeapifyUp(parent);
        }

        private void HeapifyDown(int index)
        {
            int leftChild = index * 2;
            int rightChild = index * 2 + 1;

            int swapIndex = 0;

            if (leftChild > Count)
            {
                return;
            }

            if (comparer.Compare(tree[leftChild], tree[rightChild]) < 0)
            {
                swapIndex = leftChild;
            }
            else
            {
                swapIndex = rightChild;
            }

            if (comparer.Compare(tree[swapIndex], tree[leftChild]) < 0)
            {
                var temp = tree[swapIndex];
                tree[swapIndex] = tree[leftChild];
                tree[swapIndex] = temp;
            }

            HeapifyDown(swapIndex);
        }

        public void IncreaseTree()
        {
            T[] temp = new T[tree.Length * 2];
            tree.CopyTo(temp, 0);
            tree = temp;
        }
    }
}
