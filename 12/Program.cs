using System.Collections.Specialized;

namespace Program
{
    class Program
    {
        static void Main(string[] args)
        {
            var input = Lib.Lib.ReadInputFile();
            
            var answer = Answer(input);

            var part1 = answer.part1;
            System.Console.WriteLine($"Part 1 = {part1}");

            var part2 = answer.part2;
            System.Console.WriteLine($"Part 2 = {part2}");
        }         

        static (int part1, int part2) Answer(List<string> input)
        {
            var xCount = input[0].ToArray().Length;
            var yCount = input.Count();
            var nodes = new Node[yCount,xCount];
            Node startNode = new Node('S');
            Node endNode = new Node('E');

            for(int i = 0; i < yCount; i++)
            {
                var line = input[i].ToArray();
                for(int j = 0; j < xCount; j++)
                {   
                    var node = new Node(line[j]);
                    if(line[j] == 'S') startNode = node;
                    else if(line[j] == 'E') endNode = node;
                    nodes[i,j] = node;
                }                
            }

            var nodeList = new List<Node>();

            for(int i = 0; i < yCount; i++)
            {
                for(int j = 0; j < xCount; j++)
                {   
                    nodes[i,j].AddAdjacents(nodes, (i,j));
                    nodeList.Add(nodes[i,j]);
                }                
            }

            var graph = new Graph(startNode, endNode);
            var part1Answer = graph.FindDistance(nodeList);

            var distances = new List<int>();

            foreach(Node n in nodeList.Where(e => e.Elevation == 'a'))
            {
                graph = new Graph(n, endNode);
                distances.Add(graph.FindDistance(nodeList));
            }

            var part2Answer = distances.Min();

            return (part1Answer, part2Answer);
        }
    }

    class Graph
    {
        public Graph(Node start, Node end)
        {
            Start = start;
            End = end;
        }

        private Node Start { get; set; }
        private Node End { get; set; }

        public int FindDistance(List<Node> nodes)
        {
            var priorityQueue = new PriorityQueue<Node,int>();
            var dist = new Dictionary<Node, int>();

            priorityQueue.Enqueue(Start, 0);
            
            foreach(Node node in nodes)
            {
                if(node != Start) dist.Add(node, int.MaxValue);
            }

            dist.Add(Start, 0);
            
            while(priorityQueue.Count > 0)
            {
                var node = priorityQueue.Dequeue();

                if(node == End) return dist[End]; 

                foreach(var adj in node.Adjacents)
                {
                    var distance = dist[node] + 1;
                    if(distance < dist[adj])
                    {
                        dist[adj] = distance;
                        priorityQueue.Enqueue(adj, dist[adj]);
                    }
                }
            }
            return dist[End];
        }
    }

    class Node
    {
        public Node(char elevation)
        {
            Elevation = elevation;
            Adjacents = new List<Node>();
        }
        public char Elevation { get; set; }
        public List<Node> Adjacents { get; set; }
        public void AddAdjacents(Node[,] nodes, (int x, int y) index)
        {
            var node = nodes[index.x, index.y];
            var i1 = (index.x - 1, index.y);
            var i2 = (index.x + 1, index.y);
            var i3 = (index.x, index.y - 1);
            var i4 = (index.x, index.y + 1);

            if(IndexIsInside(i1, nodes) && ElevationIsInsideBounds(nodes[i1.Item1, i1.Item2], node)) Adjacents.Add(nodes[i1.Item1, i1.Item2]);
            if(IndexIsInside(i2, nodes) && ElevationIsInsideBounds(nodes[i2.Item1, i2.Item2], node)) Adjacents.Add(nodes[i2.Item1, i2.Item2]);
            if(IndexIsInside(i3, nodes) && ElevationIsInsideBounds(nodes[i3.Item1, i3.Item2], node)) Adjacents.Add(nodes[i3.Item1, i3.Item2]);
            if(IndexIsInside(i4, nodes) && ElevationIsInsideBounds(nodes[i4.Item1, i4.Item2], node)) Adjacents.Add(nodes[i4.Item1, i4.Item2]);
        }

        private bool IndexIsInside((int xIndex, int yIndex) index, Node[,] array)
        {
            return index.xIndex >= 0 && index.yIndex >= 0 && index.xIndex < array.GetLength(0) && index.yIndex < array.GetLength(1);
        }

        private bool ElevationIsInsideBounds(Node node1, Node node2)
        {   
            return (FormatChar(node2.Elevation) - FormatChar(node1.Elevation)) >= -1;
        }

        private char FormatChar(Char c)
        {
            if(c == 'S') return 'a';
            else if(c == 'E') return 'z';
            return c;
        }
    }
}