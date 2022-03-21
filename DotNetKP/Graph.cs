using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace GraphPainterNs
{
    class Graph
    {
        List<Node> nodes { get ; set ; }
        public int zeroDegreesCounter { get; set; }
        public int[] _degrees { get; set; }
        Random rand;
        public List<Node> Nodes { get { return nodes; } }
        public Graph()
        {
            nodes = new List<Node>();
            zeroDegreesCounter = 0;
            rand = new Random((int)DateTime.Now.Ticks);
        }
        public Graph(int zeroDegreesCounter, List<List<Point>> links)
        {
            this.zeroDegreesCounter = zeroDegreesCounter;
            this.nodes = listToNodes(links);
            rand = new Random((int)DateTime.Now.Ticks);
        }
        public Graph(int zeroDegreesCounter)
        {
            this.zeroDegreesCounter = zeroDegreesCounter;
            nodes = new List<Node>();
            rand = new Random((int)DateTime.Now.Ticks);
        }
        public Graph(int[] _degrees, bool isSimple)
        {
            zeroDegreesCounter = 0;
            nodes = new List<Node>();
            rand = new Random((int)DateTime.Now.Ticks);
            generateLinks(_degrees, isSimple);
        }
        public Graph(List<List<Point>> links)
        {
            nodes = listToNodes(links);
            zeroDegreesCounter = 0;
            rand = new Random((int)DateTime.Now.Ticks);
        }
        public Graph(List<Node> nodes)
        {
            this.nodes = nodes;
            zeroDegreesCounter = 0;
            rand = new Random((int)DateTime.Now.Ticks);
        }
        public Graph(Graph graph)
        {
            _degrees = graph._degrees;
            nodes = graph.nodes;
            zeroDegreesCounter = graph.zeroDegreesCounter;
        }
        public Node this[int index]
        {
            get
            {
                if (nodes.Count - 1 >= index)
                    return nodes[index];
                else
                    return null;
            }
            set
            {
                if (nodes.Count - 1 >= index)
                    nodes[index] = value;
                else
                    throw new IndexOutOfRangeException();
            }
        }
        public PointClass this[int index1, int index2]
        {
            get 
            {
                if (nodes.Count - 1 >= index1 && nodes[index1].Count - 1 >= index2)
                    return nodes[index1][index2];
                else
                    throw new IndexOutOfRangeException(); 
            }
            set
            {
                if (nodes.Count - 1 >= index1 && nodes[index1].Count - 1 >= index2)
                    nodes[index1][index2] = value;
                else
                    throw new IndexOutOfRangeException();
            }
        }
        public int Count()
        {
            return nodes.Count;
        }
        public int isReal(int[] degrees, bool isSimple)
        {
            int sum = 0;
            int sum2;
            
            foreach (int degree in degrees)
            {
                sum += degree;
            }
            if (sum % 2 == 1) return 0;

            if (!isSimple)
                for (int i = 0; i < degrees.Count(); i++)
                {
                    sum = 0;
                    for (int k = 0; k <= i; k++)
                    {
                        sum += degrees[k];
                    }
                    sum2 = (i + 1) * i;
                    for (int k = i + 1; k < degrees.Count(); k++)
                    {
                        sum2 += Math.Min(degrees[k], i + 1);
                    }
                    if (sum2 < sum) return -1;
                }
            return 1;
        }
        public List<Node> generateLinks(int[] degrees, bool isSimple)
        {
            if (isReal(degrees, isSimple) != 1) return null;
            _degrees = degrees;
            bool isComplete = false;
            int counter = 0;
            int distance = 0;
            int stepX = 500 / degrees.Length;
            for (int i = 0; i < degrees.GetLength(0); i++)
            {
                nodes.Add(new Node());
                nodes[i].StartPoint = new PointClass(distance + 50, rand.Next(0, 400) + 40, i + 1);
                distance += stepX;
            }

            for (int i = 0; i < degrees.GetLength(0); i++)
            {
                if (_degrees[i] == 0) zeroDegreesCounter++;
            }
            //bool temp = false;

            //bool allNoneOdd = false;
            //for (int i = 0; i < _degrees.Count(); i++)
            //{
            //    counter = 1;
            //    temp = true;
            //    for (int j = 0; j < _degrees.Count(); j++)
            //    {
            //        if (_degrees[i] == _degrees[j] && i != j)
            //        {
            //            counter++;
            //            temp = true;
            //            if (_degrees[i] % 2 == 0) temp = false;
            //            else { allNoneOdd = true; }
            //        }
            //    }
            //    if (counter >= 4) break;
            //}
            //if (counter >= 4)
            //    for (int i = 0; i < _degrees.Count(); i++)
            //    {
            //        if (_degrees[i] % 2 == Convert.ToInt32(temp))
            //            for (int j = i + 1; j < _degrees.Count(); j++)
            //            {
            //                if (_degrees[j] % 2 == Convert.ToInt32(!temp || allNoneOdd))
            //                {
            //                    _degrees[i]--;
            //                    _degrees[j]--;
            //                    nodes[i] += nodes[j].StartPoint;
            //                    nodes[j] += nodes[i].StartPoint;
            //                }
            //                break;
            //            }
            //    }
            while (!isComplete)
            {
                isComplete = false;
                for (int i = 0; i < _degrees.Count(); i++)
                {
                    for (int j = i + 1; j < _degrees.Count(); j++)
                    {
                        if (_degrees[i] > 0 && _degrees[j] > 0)
                        {
                            _degrees[i]--;
                            _degrees[j]--;
                            nodes[i] += nodes[j].StartPoint;
                            nodes[j] += nodes[i].StartPoint;
                        }
                    }
                }

                for (int i = 0; i < _degrees.Count(); i++)
                    if (_degrees[i] > 2)
                    {
                        isComplete = false;
                        break;
                    }
                    else
                    {
                        isComplete = true;
                    }

            }
            return nodes;
        }
        public List<Node> listToNodes(List<List<Point>> links)
        {
            nodes = new List<Node>();
            for (int i = 0; i < links.Count; i++)
            {
                Node tempNode = new Node(new PointClass(links[i][0]));
                for (int j = 1; j < links[i].Count; j++)
                {
                    tempNode += new PointClass(links[i][j]);
                }
                nodes.Add(tempNode);
            }
            return nodes;
        }
        public List<Node> uniqueNodes()
        {
            List<Node> uniqueNodes = nodes;
            for (int i = 0; i < uniqueNodes.Count(); i++)
            {
                for (int k = i; k < uniqueNodes.Count(); k++)
                {
                    for (int j = 0; j < uniqueNodes[k].Count; j++)
                    {
                        if (uniqueNodes[i].StartPoint.Equals(uniqueNodes[k][j].toStruct()))
                        {
                            uniqueNodes[k].Remove(uniqueNodes[k][j]);
                            break;
                        }
                    }
                }
            }
            return uniqueNodes;
        }
        int add_cycle(int cycle_end, int cycle_st)
        {
            cycle[ncycle].Clear();

            cycle[ncycle].Add(cycle_st);
            for (int v = cycle_end; v != cycle_st; v = p[v])
                cycle[ncycle].Add(v);

            cycle[ncycle].Add(cycle_st);
            cycle[ncycle].Reverse();

            return cycle[ncycle].Count;
        }
        void dfs(int v)
        {
            color[v] = 1;
            for (int i = 0; i < g[v].Count; i++)
            {
                int to = g[v][i];
                if (color[to] == 0)
                {
                    p[to] = v;
                    dfs(to);
                }
                else if (color[to] == 1)
                {
                    if (add_cycle(v, to) > 3) // Исключение вырожденных случаев, н: 1 2 1
                    {
                        cycle.Add(new List<int>());
                        ncycle++;
                    }

                }
            }
            color[v] = 0;
        }
        public List<List<int>> getCycles()
        {
            ncycle = 0; 
            cycle.Clear();
            g.Clear();
            p.Clear();
            color.Clear();
            int n = nodes.Count + 1;
            cycle = new List<List<int>>(nodes.Count);
            cycle.Add(new List<int>());
            g = new List<List<int>>();
            for (int i = 0; i < nodes.Count + 2; g.Add(new List<int>()), i++);
            for (int i = 0; i < nodes.Count; i++)
            {
                for (int j = 0; j < nodes[i].Count; j++)
                {
                    if (nodes[i].outGoingLinks[j])
                    {
                        g[nodes[i].StartPoint.getNumber].Add(nodes[i][j].getNumber);
                    }
                    else
                    {
                        g[nodes[i][j].getNumber].Add(nodes[i].StartPoint.getNumber);
                    }
                }
            }
            for (int i = 0; i < n + 1; color.Add(0), p.Add(-1), i++) ;
            for (int i = 0; i < n; i++)
                if (color[i] == 0)
                    dfs(i);
            return cycle;
        }
        int ncycle = 0;
        List<List<int>> cycle = new List<List<int>>();
        List<List<int>> g = new List<List<int>>();
        List<int> color = new List<int>();
        List<int> p = new List<int>();
        
    }

}
