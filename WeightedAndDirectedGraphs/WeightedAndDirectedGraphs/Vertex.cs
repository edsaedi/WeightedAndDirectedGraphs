using System;
using System.Collections.Generic;
using System.Text;

namespace WeightedAndDirectedGraphs
{
    public class Vertex<T>
    {
        public T Value { get; set; }
        public List<Edge<T>> Neighbors { get; set; }
        public bool IsVisited { get; set; }
        public int Count => Neighbors.Count;

        public Edge<T> this[int index]
        {
            get { return Neighbors[index]; }
            set { Neighbors[index] = value; }
        }

        public Vertex(T value)
        {
            Value = value;
            Neighbors = new List<Edge<T>>();
            IsVisited = false;
        }
    }
}
