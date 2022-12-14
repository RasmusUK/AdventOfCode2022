using System.Collections.Specialized;

namespace Program
{
    class Program
    {
        static void Main(string[] args)
        {
            var input = Lib.Lib.ReadInputFile();
            
            var part1 = Part1(input, false);
            System.Console.WriteLine($"Part 1 = {part1}");

            var part2 = Part2(input, false);
            System.Console.WriteLine($"Part 2 = {part2}");
        }     

        static List<List<(int, int)>> GetRocks(List<string> input)
        {
            var rocks = new List<List<(int, int)>>();
            for(int i = 0; i < input.Count; i++)
            {
                var split = input[i].Split("->");
                var list = new List<(int, int)>();
                foreach(var entry in split)
                {
                    var strip = entry.Trim().Split(",");
                    var x = int.Parse(strip[0]);
                    var y = int.Parse(strip[1]);
 
                    list.Add((x,y));
                }
                rocks.Add(list);
            }
            return rocks;
        }

        static string[,] FillCave(List<List<(int x, int y)>> rocks, string[,] cave)
        {
            for(int i = 0; i < rocks.Count; i++)
            {
                (int x, int y) prevPos = (0,0);

                for(int j = 0; j < rocks[i].Count; j++)
                {                    
                    var x = rocks[i][j].x;
                    var y = rocks[i][j].y;

                    cave[x,y] = "#";

                    if(prevPos != (0,0))
                    {
                        var positions = GetPositionsBetween(prevPos, (x,y));
                        foreach(var pos in positions)
                        {
                            cave[pos.x, pos.y] = "#";
                        }
                    }

                    prevPos = (x,y);
                }
            }
            cave[500,0] = "+";
            return cave;
        }

        static (string[,] cave, int count) FallingSand(string[,] cave, (int, int, int) bounds)
        {
            bool rest = false;
            var count = 0;

            while(!rest)
            {
                var pos = ReleaseSand(cave, bounds);
                if(FlowIntoAbyss(pos, bounds))
                {
                    rest = true;
                }
                else
                {
                    count++;
                    cave[pos.x,pos.y] = "O";
                }
                if(Blocked(pos)) rest = true;
            }

            return (cave, count);
        }

        static (int x, int y) GetMax(List<List<(int x, int y)>> rocks)
        {
            var flatten = rocks.SelectMany(e => e).ToList();
            var x = flatten.Select(e => e.x).Max() + 1;
            var y = flatten.Select(e => e.y).Max() + 1;
            return (x,y);
        }

        static (int x, int y) GetMin(List<List<(int x, int y)>> rocks)
        {
            var flatten = rocks.SelectMany(e => e).ToList();
            var x = flatten.Select(e => e.x).Min();
            var y = flatten.Select(e => e.y).Min();
            return (x,y);
        }

        static int Part1(List<string> input, bool print)
        {
            var rocks = GetRocks(input);
            
            (int x, int y) max = GetMax(rocks);
            (int x, int y) min = GetMin(rocks);
            min.y = 0;

            var cave = FillCave(rocks, new string[max.x,max.y]);     

            if(print)
            {
                PrintArray(cave, max, min);
                System.Console.WriteLine();
            }

            (cave, int count) = FallingSand(cave, (min.x, max.x, max.y));     
            
            if(print) PrintArray(cave, max, min);
            return count;
        }     

        static int Part2(List<string> input, bool print)
        {
            var rocks = GetRocks(input);
            
            (int x, int y) max = GetMax(rocks);
            (int x, int y) min = GetMin(rocks);
            min.y = 0;
            min.x = 0;
            max.y += 2;
            max.x += 500;

            rocks.Add(new List<(int, int)>{(min.x,max.y-1), (max.x-1,max.y-1)});

            var cave = FillCave(rocks, new string[max.x,max.y]);     

            if(print)
            {
                PrintArray(cave, max, min);
                System.Console.WriteLine();
            }
    
            (cave, int count) = FallingSand(cave, (min.x, max.x, max.y));     
            
            if(print) PrintArray(cave, max, min);
            return count;
        } 
        static bool Blocked((int x, int y) pos)
        {
            return pos == (500,0);
        }

        static bool FlowIntoAbyss((int x, int y) pos, (int xMin, int xMax, int yMax) bounds)
        {
            return pos.y > bounds.yMax || pos.x < bounds.xMin || pos.x >= bounds.xMax;
        }

        static (int x, int y) ReleaseSand(string[,] arr, (int, int, int) bounds)
        {
            bool rest = false;
            (int x, int y) pos = (500,0);
            while(!rest)
            {
                if(IsAvailable((pos.x, pos.y+1), arr, bounds)) pos.y++;
                else if(IsAvailable((pos.x-1, pos.y+1), arr, bounds)) 
                {
                    pos.x--;
                    pos.y++;
                }
                else if(IsAvailable((pos.x+1, pos.y+1), arr, bounds)) 
                {
                    pos.x++;
                    pos.y++;
                }
                else rest = true;
                if(FlowIntoAbyss(pos, bounds)) rest = true;
            }
            return pos;
        }

        static bool IsAvailable((int x, int y) pos, string[,] arr, (int, int, int) bounds)
        {
            if(FlowIntoAbyss(pos, bounds)) return true;
            return (arr[pos.x,pos.y] == "." || string.IsNullOrEmpty(arr[pos.x,pos.y]));
        }
        

        static void PrintArray(string[,] arr, (int x, int y) max, (int x, int y) min)
        {
            for(int i = min.y; i < max.y; i++)
            {
                for(int j = min.x; j < max.x; j++)
                {   
                    if(string.IsNullOrEmpty(arr[j,i])) arr[j,i] = ".";
                    System.Console.Write(arr[j,i]);
                }
                System.Console.WriteLine();
            }
        }

        static List<(int x,int y)> GetPositionsBetween((int x, int y) pos1, (int x, int y) pos2)
        {
            List<int> between;
            var positions = new List<(int, int)>();

            if(pos1.x != pos2.x)
            {
                between = GetPositionsBetween(pos1.x, pos2.x);
                foreach(var x in between)
                {
                    positions.Add((x, pos1.y));
                }
            }
            else
            {
                between = GetPositionsBetween(pos1.y, pos2.y);
                foreach(var y in between)
                {
                    positions.Add((pos1.x, y));
                }
            }
            return positions;
        }   


        static List<int> GetPositionsBetween(int pos1, int pos2)
        {
            var between = new List<int>();
            for(int i = Math.Min(pos1, pos2) + 1; i < Math.Max(pos1, pos2); i++)
            {
                between.Add(i);
            }
            return between;
        }   

        static int GetXIndex(int minX, int index)
        {
            return index - minX;
        }
    }
}