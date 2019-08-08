using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WeightedAndDirectedGraphs
{
    public class Graph<T>
    {
        internal List<Vertex<T>> vertices { get; set; }

        public int Count => Vertices.Count;

        public IReadOnlyList<Vertex<T>> Vertices => vertices;

        public IReadOnlyList<Edge<T>> Edges
        {
            get
            {
                var edges = new List<Edge<T>>();

                for (int i = 0; i < Count; i++)
                {
                    for (int j = 0; j < vertices[i].Count; j++)
                    {
                        edges.Add(vertices[i].Neighbors[j]);
                    }
                }

                return edges;
            }
        }

        public Vertex<T> this[int index]
        {
            get { return vertices[index]; }
            set { vertices[index] = value; }
        }


        public Graph()
        {
            vertices = new List<Vertex<T>>();
        }

        public void AddVertex(T value)
        {
            AddVertex(new Vertex<T>(value));
        }

        public void AddVertex(Vertex<T> vertex)
        {
            if (Search(vertex.Value) != null)
            {
                throw new Exception("This value already exists!");
            }

            vertices.Add(vertex);
        }

        public bool AddEdge(T a, T b, int distance = 0)
        {
            return AddEdge(Search(a), Search(b), distance);
        }

        public bool AddEdge(Vertex<T> a, Vertex<T> b, int distance = 0)
        {
            if (!(GetEdge(a, b) == null && vertices.Contains(a) && vertices.Contains(b)))
            {
                return false;
            }

            a.Neighbors.Add(new Edge<T>(a, b, distance));

            return true;
        }

        public bool RemoveVertex(T value)
        {
            return RemoveVertex(Search(value));
        }

        public bool RemoveVertex(Vertex<T> vertex)
        {
            if (!vertices.Contains(vertex))
            {
                return false;
            }

            foreach (var item in vertices)
            {
                for (int i = 0; i < item.Neighbors.Count; i++)
                {
                    if (item.Neighbors[i].EndingPoint.Equals(vertex))
                    {
                        RemoveEdge(item, vertex);
                    }
                }
            }

            vertices.Remove(vertex);
            return true;
        }

        public bool RemoveEdge(T a, T b)
        {
            return RemoveEdge(Search(a), Search(b));
        }

        public bool RemoveEdge(Vertex<T> a, Vertex<T> b)
        {
            var edge = GetEdge(a, b);

            if (edge == null)
            {
                return false;
            }

            a.Neighbors.Remove(edge);

            return true;
        }


        public Vertex<T> Search(T value)
        {
            foreach (var item in vertices)
            {
                if (item.Value.Equals(value))
                {
                    return item;
                }
            }

            return null;
        }

        public Edge<T> GetEdge(T a, T b)
        {
            return GetEdge(Search(a), Search(b));
        }

        public Edge<T> GetEdge(Vertex<T> a, Vertex<T> b)
        {
            if (!(vertices.Contains(a) && vertices.Contains(b)))
            {
                return null;
            }

            foreach (var item in a.Neighbors)
            {
                if (item.EndingPoint == b)
                {
                    return item;
                }
            }

            return null;
        }

        public IEnumerable<Vertex<T>> Dijkstra(T start, T end)
        {
            return Dijkstra(Search(start), Search(end));
        }

        public IEnumerable<Vertex<T>> Dijkstra(Vertex<T> start, Vertex<T> end)
        {
            if (!(vertices.Contains(start) && vertices.Contains(end)))
            {
                return null;
            }

            var info = new Dictionary<Vertex<T>, (Vertex<T> founder, float distance)>();

            var queue = new PriorityQue<Vertex<T>>(Comparer<Vertex<T>>.Create((a, b) => info[a].distance.CompareTo(info[b].distance)));

            for (int i = 0; i < Count; i++)
            {
                vertices[i].IsVisited = false;
                info.Add(vertices[i], (null, int.MaxValue));
            }

            info[start] = (null, 0);
            queue.Enqueu(start);

            while (!queue.IsEmpty())
            {
                var current = queue.Deqeue();
                current.IsVisited = true;

                foreach (var edge in current.Neighbors)
                {
                    var neighbor = edge.EndingPoint;
                    float tentative = edge.Distance + info[current].distance;

                    if (tentative < info[neighbor].distance)
                    {
                        info[neighbor] = (current, tentative);
                        neighbor.IsVisited = false;
                    }

                    if (!queue.Contains(neighbor) && !neighbor.IsVisited)
                    {
                        queue.Enqueu(neighbor);
                    }
                }

                if (current == end)
                {
                    break;
                }
            }

            var path = new Stack<Vertex<T>>();

            var currentV = info.Where(x => x.Key == end).First();
            while (true)
            {
                path.Push(currentV.Key);

                if (currentV.Value.founder == null)
                {
                    if (path.Count == 1)
                    {
                        return null;
                    }

                    break;
                }

                currentV = new KeyValuePair<Vertex<T>, (Vertex<T> founder, float distance)>(currentV.Value.founder, info[currentV.Value.founder]);

            }

            return path;
        }

        //public bool IfNegativeCycleExists()
        //{
        //    var ctrlA = new Queue<Vertex<T>>();

        //    for (int i = 0; i < Count; i++)
        //    {
        //        ctrlA.Enqueue(vertices[i]);
        //    }

        //    while (ctrlA.Count != 0)
        //    {
        //        var source = ctrlA.Dequeue();

        //        var info = new Dictionary<Vertex<T>, (Vertex<T> founder, float distance)>();

        //        for (int i = 0; i < Count; i++)
        //        {
        //            info.Add(vertices[i], (null, float.PositiveInfinity));
        //        }

        //        info[source] = (null, 0);

        //        for (int i = 0; i < Count; i++)
        //        {
        //            var start = info[vertices[i]];
        //            for (int j = 0; j < vertices[i].Count; j++)
        //            {
        //                var end = info[vertices[i].Neighbors[j].EndingPoint];
        //                if(end.distance > start.distance + vertices[i].Neighbors[j].Distance)
        //                {
        //                    info[vertices[i].Neighbors[j].EndingPoint] = (vertices[i], start.distance + vertices[i].Neighbors[j].Distance);
        //                }
        //            }
        //        }



        //    }

        //    return false;
        //}
    }
}
