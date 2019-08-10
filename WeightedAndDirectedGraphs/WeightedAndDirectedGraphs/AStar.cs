using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Linq;

namespace WeightedAndDirectedGraphs
{
    public static class AStar
    {
        public enum Heuristics
        {
            Manhattan,
            Euclidean
        }

        public static IEnumerable<Vertex<Point>> AStarAlgo(Graph<Point> graph, Point start, Point end, Heuristics heuristic = Heuristics.Manhattan)
        {
            return AStarAlgo(graph, graph.Search(start), graph.Search(end));
        }

        public static IEnumerable<Vertex<Point>> AStarAlgo(Graph<Point> graph, Vertex<Point> start, Vertex<Point> end, Heuristics heuristic = Heuristics.Manhattan)
        {
            if (!(graph.vertices.Contains(start) && graph.vertices.Contains(end)))
            {
                return null;
            }

            var info = new Dictionary<Vertex<Point>, (Vertex<Point> founder, float distance, float finalDistance)>();

            var queu = new PriorityQue<Vertex<Point>>(Comparer<Vertex<Point>>.Create((a, b) => info[a].finalDistance.CompareTo(info[b].finalDistance)));

            foreach (var vertex in graph.vertices)
            {
                vertex.IsVisited = false;
                info.Add(vertex, (null, float.PositiveInfinity, float.PositiveInfinity));
            }

            info[start] = (null, 0, HeuristicsFunc(heuristic, start.Value, end.Value));
            queu.Enqueu(start);

            while (queu.Count > 0)
            {
                var current = queu.Deqeue();
                current.IsVisited = false;

                foreach (var edge in current.Neighbors)
                {
                    var neighbor = edge.EndingPoint;
                    float tentative = edge.Distance + info[start].distance;

                    if (tentative < info[neighbor].distance)
                    {
                        neighbor.IsVisited = false;
                        info[neighbor] = (current, tentative, HeuristicsFunc(heuristic, current.Value, neighbor.Value));
                    }

                    if (!queu.Contains(neighbor) && !neighbor.IsVisited)
                    {
                        queu.Enqueu(neighbor);
                    }
                }

                if (current == end)
                {
                    break;
                }
            }

            var path = new Stack<Vertex<Point>>();

            var currentV = info.Where(x => x.Key == end).First();

            while(true)
            {
                path.Push(currentV.Key);
                
                if(currentV.Value.founder == null)
                {
                    if(path.Count == 1)
                    {
                        return null;
                    }
                    break;
                }

                currentV = new KeyValuePair<Vertex<Point>, (Vertex<Point> founder, float distance, float finalDistance)>();
            }

            return path;
        }

        public static float HeuristicsFunc(Heuristics heuristics, Point start, Point end, float D = 1)
        {
            float dx = Math.Abs(start.X - end.X);
            float dy = Math.Abs(start.Y - end.Y);

            switch (heuristics)
            {
                case Heuristics.Manhattan:
                    return D * (dx + dy);

                case Heuristics.Euclidean:
                    return D * (float)Math.Sqrt(dx * dx + dy * dy);

                default:
                    throw new Exception("Invalid Heuristic Function");
            }
        }
    }
}
