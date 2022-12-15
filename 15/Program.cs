using System.Collections.Specialized;
using System.Text.RegularExpressions;

namespace Program
{
    class Program
    {
        static void Main(string[] args)
        {
            var input = Lib.Lib.ReadInputFile();
            
            var part1 = Part1(input);
            System.Console.WriteLine($"Part 1 = {part1}");

            var part2 = Part2(input);
            System.Console.WriteLine($"Part 2 = {part2}");
        }       

        static int Part1(List<string> input)
        {
            var sensors = ParseInput(input);
            var positions = new List<(int, int, int)>();
            var positionsOccupied = new List<int>();

            var row = 2000000;

            foreach(var s in sensors)
            {
                if(s.SensorPos.y == row) positionsOccupied.Add(s.SensorPos.x);
                if(s.BeaconPos.y == row) positionsOccupied.Add(s.BeaconPos.x);
                positions.AddRange(s.GetOccupiedPositionsAtRow(row));
            }
            
            var alreadyOccupied = positionsOccupied.Distinct().Count();

            positionsOccupied.Clear();

            foreach(var pos in positions)
            {
                positionsOccupied.AddRange(GetXPositions(pos));
            }

            positionsOccupied = positionsOccupied.Distinct().ToList();;

            var count = positionsOccupied.Count - alreadyOccupied;

            return count;
        }

        static int Part2(List<string> input)
        {
            var max = 4000000;
            var sensors = ParseInput(input);
            var xPossible = Enumerable.Range(0, 20).ToList();
            var yPossible = Enumerable.Range(0, 20).ToList();
            foreach(var s in sensors)
            {
                s.GetOccupiedPositions();
                xPossible = xPossible.Except(s.CannotX).ToList();;
                yPossible = yPossible.Except(s.CannotY).ToList();;
            }
       

            //var positionsOccupiedMap = new Dictionary<int, HashSet<int>>();
            //var fullRange = new HashSet<int>(Enumerable.Range(0, max));
            //var puzzle = new int[max+1,max+1];
//
            //foreach(var s in sensors)
            //{
            //    foreach(var pos in s.GetOccupiedPositions().Where(e => e.xStart >= 0 && e.xEnd <= max))
            //    {
            //        foreach(var p in GetXPositions(pos))
            //        {
            //            puzzle[p,pos.y] = 1;
            //        }
            //    } 
            //}
            //int j = 0;
            //foreach(var s in sensors)
            //{    
            //    System.Console.WriteLine(j);
//
            //    s.GetOccupiedPositions();
            //    for(int i = 0; i <= max; i++)
            //    {
            //        positionsOccupiedMap[i].UnionWith(s.PositionsMap[i]);
            //    }
            //    j++;
            //}
//
            //for(int row = 0; row <= max; row++)
            //{
            //    if(row % 10 == 0) System.Console.WriteLine(row);
            //    
            //    var result = fullRange.Except(positionsOccupiedMap[row]).ToList();
//
            //    if(result.Count != 0) 
            //    {
            //        return result[0] * 4000000 + row;
            //    } 
            //}         

            return 0;
        }

        static List<int> GetXPositions((int xStart, int xEnd, int _) pos)
        {
            var positions = new List<int>();
            for(int i = pos.xStart; i <= pos.xEnd; i++)
            {
                positions.Add(i);
            }
            return positions;
        }

        static List<Sensor> ParseInput(List<string> input)
        {
            var sensors = new List<Sensor>();
            for(int i = 0; i < input.Count; i++)
            {
                (int x, int y) sensorPos;
                (int x, int y) beaconPos;

                string pattern = @"x=(-?\d+), y=(-?\d+)";
                var matches = Regex.Matches(input[i], pattern);
                
                sensorPos.x = int.Parse(matches[0].Groups[1].Value);
                sensorPos.y = int.Parse(matches[0].Groups[2].Value);
                beaconPos.x = int.Parse(matches[1].Groups[1].Value);
                beaconPos.y = int.Parse(matches[1].Groups[2].Value);

                sensors.Add(new Sensor(sensorPos, beaconPos));
            }
            return sensors;
        }
    }

    class Sensor
    {
        public Sensor((int x, int y) sensorPos, (int x, int y) beaconPos)
        {
            SensorPos = sensorPos;
            BeaconPos = beaconPos;
            OccupiedPos = new List<(int, int, int)>();
            CannotX = new List<int>();
            CannotY = new List<int>();
        }

        public (int x, int y) SensorPos { get; set; }
        public (int x, int y) BeaconPos { get; set; }
        public List<int> CannotX { get; set; }
        public List<int> CannotY { get; set; }

        List<(int xStart, int xEnd, int y)> OccupiedPos { get; set; }
        public HashSet<int>[] PositionsMap {get; set;}

        public List<(int xStart, int xEnd, int y)> GetOccupiedPositions()
        {
            if(OccupiedPos.Count != 0) return OccupiedPos;

            var positions = new List<(int, int, int)>();
            var distance = GetManhattenDistance(SensorPos, BeaconPos);
            var start = SensorPos.y - distance;
            var end = SensorPos.y + distance;
            if(end <= 0 && start >= 20) CannotY.Remove(SensorPos.y);
            PositionsMap = new HashSet<int>[end+1];
            for(int y = start; y <= end; y++)
            {
                var distanceLeft = distance - (Math.Abs(SensorPos.y - y));

                int xStart = SensorPos.x - distanceLeft;
                int xEnd = SensorPos.x + distanceLeft;

                if(xEnd <= 0 && xStart >= 20) CannotX.Remove(SensorPos.x);
                positions.Add((xStart, xEnd, y));
                //PositionsMap[y] = Enumerable.Range(xStart, xEnd).ToHashSet();
            }

            OccupiedPos = positions;

            return OccupiedPos;
        }

        public List<(int xStart, int xEnd, int y)> GetOccupiedPositionsAtRow(int row)
        {
            if(OccupiedPos.Count == 0) GetOccupiedPositions();
            return OccupiedPos.Where(e => e.y == row).ToList();
        }

        private HashSet<int> GetXPositions((int xStart, int xEnd) pos)
        {
            var positions = new HashSet<int>();
            for(int i = pos.xStart; i <= pos.xEnd; i++)
            {
                positions.Add(i);
            }
            return positions;
        }

        private int GetManhattenDistance((int x, int y) pos1, (int x, int y) pos2)
        {
            var x = Math.Abs(pos1.x - pos2.x);
            var y = Math.Abs(pos1.y - pos2.y);
            return x + y;
        }
    }
}