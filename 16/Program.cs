using System.Collections.Specialized;
using System.Text.RegularExpressions;

namespace Program
{
    class Program
    {
        private const string Start = "AA";
        static void Main(string[] args)
        {
            var input = Lib.Lib.ReadInputFile();
            var graph = ParseInput(input);

            var part1 = Part1(graph);
            System.Console.WriteLine($"Part 1 = {part1}");
//
            //var part2 = Part2(sensors);
            //System.Console.WriteLine($"Part 2 = {part2}");
        }       

        static int Part1(Graph g)
        {
            return g.MostPressure(g.Valves.First(v => v.Name == Start));
        }

        static long Part2(Graph g)
        {
            return 0;
        }

        static Graph ParseInput(List<string> input)
        {
            var valves = new List<Valve>();
            for(int i = 0; i < input.Count; i++)
            {

                string pattern = @"Valve (\w+) has flow rate=(\d+); tunnels? leads? to valves? (.+)";
                var matches = Regex.Matches(input[i], pattern);
                
                var name = matches[0].Groups[1].Value;
                var flowRate = int.Parse(matches[0].Groups[2].Value);
                var valve = new Valve(flowRate, name);
                matches[0].Groups[3].Value.Split(",").ToList().ForEach(s => valve.AdjacentValvesNames.Add(s.Trim()));
                valves.Add(valve);
            }
            valves = AddEdges(valves);
            valves = RemoveUselessValves(valves);
            return new Graph(valves);
        }

        static List<Valve> AddEdges(List<Valve> valves)
        {
            foreach(var v in valves)
            {
                var queue = new Queue<(Valve, int)>();
                var visited = new HashSet<string>();
                queue.Enqueue((v,0));
                var edges = new List<Edge>();
                while(queue.Count > 0)
                {
                    (Valve valve, int weight) = queue.Dequeue();
                    if(valve.Name != v.Name && valve.FlowRate > 0) edges.Add(new Edge(v, valve, weight));
                    foreach(var adj in valve.AdjacentValvesNames.Where(s => !visited.Contains(s)))
                    {
                        queue.Enqueue((valves.First(s => s.Name == adj), weight+1));
                        visited.Add(adj);
                    }
                }
                v.Edges.AddRange(edges);
            }
            return valves;
        }

        static List<Valve> RemoveUselessValves(List<Valve> valves) => valves.Where(e => e.FlowRate != 0 || e.Name == Start).ToList();

    }

    class Graph
    {
        private const int Minutes = 30;

        public Graph(List<Valve> valves)
        {
            Valves = valves;
        }
        public List<Valve> Valves { get; set; }

        public int MostPressure(Valve start)
        {
            return Pressure(Valves.First(v => v.Name == "AA"), 0, 0, 1, new HashSet<Valve>());
        }

        public int Pressure(Valve valve, int flowRate, int current, int time, HashSet<Valve> visited)
        {
            if(time > Minutes) return current;

            visited.Add(valve);
            flowRate += valve.FlowRate;

            if(valve.FlowRate != 0)
            {
                time++;
                current += flowRate;
            }

            int best = current;
            var edges = valve.Edges.Where(e => !visited.Contains(e.To)).Where(e => e.Weight <= Minutes - time) .ToList();

            if(edges.Count == 0) return (best + (flowRate * (Minutes - time)));

            foreach(var e in edges) best = Math.Max(best, Pressure(e.To, flowRate, current + flowRate*e.Weight, time + e.Weight, new HashSet<Valve>(visited)));

            return best;
        }
    }

    class Valve
    {
        public Valve(int flowRate, string name)
        {
            FlowRate = flowRate;
            Name = name;
            Edges = new List<Edge>();
            AdjacentValvesNames = new List<string>();
            ValveOpen = false;
        }

        public int FlowRate { get; set; }
        public string Name { get; set; }
        public List<Edge> Edges { get; set; }
        public List<string> AdjacentValvesNames { get; set; }
        public bool ValveOpen { get; set; }
    }

    class Edge
    {
        public Edge(Valve from, Valve to, int weight)
        {
            From = from;
            To = to;
            Weight = weight;
        }

        public Valve From { get; set; }
        public Valve To { get; set; }
        public int Weight { get; set; }
    }
}