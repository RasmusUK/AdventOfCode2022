using System.Collections.Specialized;
using System.Text.RegularExpressions;

namespace Program
{
    class Program
    {
        static void Main(string[] args)
        {
            var input = Lib.Lib.ReadInputFile();
            var sensors = ParseInput(input);

            var part1 = Part1(sensors);
            System.Console.WriteLine($"Part 1 = {part1}");

            var part2 = Part2(sensors);
            System.Console.WriteLine($"Part 2 = {part2}");
        }       

        static int Part1(List<Sensor> sensors)
        {
            var y = 2000000;

            var minX = sensors.Min(s => s.SensorPos.x - s.Delta);
            var maxX = sensors.Max(s => s.SensorPos.x + s.Delta);

            var score = 0;

            for(int x = minX; x <= maxX; x++)
            {
                var pos = (x,y);
                if(sensors.Any(s => s.IsBeaconOrSensor(pos))) continue;
                
                foreach(var s in sensors)
                {
                    if(s.IsInside((pos)))
                    {
                        score++;
                        break;
                    }
                }
            }

            return score;
        }

        static long Part2(List<Sensor> sensors)
        {
            var max = 4000000;
            var min = 0;

            for(int y = 0; y <= max; y++)
            {
                var xRanges = sensors.Select(s => (s.MinXAtY(min, y), s.MaxXAtY(max, y))).Where(s => s.Item1 <= s.Item2).ToList();
                xRanges.Sort((s1, s2) => s1.Item1.CompareTo(s2.Item1));
                var xMin = xRanges.Min(s => s.Item1);
                var xMax = xRanges.Max(s => s.Item2);
                if(xMin != 0) return xMin; 
                else if(xMax != max) return xMax;

                for(int i = 0; i < xRanges.Count; i++)
                {
                    if(xRanges[0].Item1 <= xRanges[i].Item1 && xRanges[0].Item2 >= xRanges[i].Item1)
                        xRanges[0] = (xMin, Math.Max(xRanges[0].Item2, xRanges[i].Item2));
                    else return (long) (xRanges[0].Item2 + 1) * 4000000 + y;
                }
            }

            return 0;
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
            Delta = Math.Abs(sensorPos.x - beaconPos.x) + Math.Abs(sensorPos.y - beaconPos.y);
        }

        public (int x, int y) SensorPos { get; set; }
        public (int x, int y) BeaconPos { get; set; }    
        public int Delta { get; set; }

        private int MinXAtY(int y) => SensorPos.x - Delta + Math.Abs(SensorPos.y - y);
        private int MaxXAtY(int y) => SensorPos.x + Delta - Math.Abs(SensorPos.y - y);
        public bool IsInside((int x, int y) pos) => pos.x >= MinXAtY(pos.y) && pos.x <= MaxXAtY(pos.y);
        public bool IsBeaconOrSensor((int x, int y) pos) => pos == SensorPos || pos == BeaconPos;
        public int MinXAtY(int min, int y) => Math.Max(MinXAtY(y), min);
        public int MaxXAtY(int max, int y) => Math.Min(MaxXAtY(y), max);
    }
}