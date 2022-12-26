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

            var rocks = new List<Rock>()
            {
                new Rock(),
                new Rock(),
                new Rock(),
                new Rock(),
                new Rock()
            };
            rocks[0].Points.Add(new Point(0,0));
            rocks[0].Points.Add(new Point(1,0));
            rocks[0].Points.Add(new Point(2,0));
            rocks[0].Points.Add(new Point(3,0));

            rocks[1].Points.Add(new Point(0,1));
            rocks[1].Points.Add(new Point(1,0));
            rocks[1].Points.Add(new Point(1,1));
            rocks[1].Points.Add(new Point(1,2));
            rocks[1].Points.Add(new Point(2,1));

            rocks[2].Points.Add(new Point(0,0));
            rocks[2].Points.Add(new Point(1,0));
            rocks[2].Points.Add(new Point(2,0));
            rocks[2].Points.Add(new Point(2,1));
            rocks[2].Points.Add(new Point(2,2));

            rocks[3].Points.Add(new Point(0,0));
            rocks[3].Points.Add(new Point(0,1));
            rocks[3].Points.Add(new Point(0,2));
            rocks[3].Points.Add(new Point(0,3));

            rocks[4].Points.Add(new Point(0,0));
            rocks[4].Points.Add(new Point(0,1));
            rocks[4].Points.Add(new Point(1,0));
            rocks[4].Points.Add(new Point(1,1));

            var cave = new Cave(rocks, input[0].ToList());
            var part1 = Part1(cave);
            System.Console.WriteLine($"Part 1 = {part1}");
            
            //var part2 = Part2(sensors);
            //System.Console.WriteLine($"Part 2 = {part2}");
        }       

        static int Part1(Cave cave) => cave.Move(2022);

        static long Part2()
        {
            return 0;
        }
    }

    class Cave
    {
        public Cave(List<Rock> rocks, List<char> jets)
        {
            Rocks = rocks;
            Jets = jets;
            Occupied = new HashSet<(int, int)>();
            Top = 0;
        }
        List<Rock> Rocks { get; set; }
        List<char> Jets { get; set; }
        HashSet<(int, int)> Occupied { get; set; }
        int Top { get; set; }
        public int Move(long number)
        {
            int i = 0;
            int j = 0;
            while(number > 0)
            {
                var rock = new Rock(Rocks[i % Rocks.Count]);
                rock.SetPositionFromStart(Top);
                bool rest = false;
                while(!rest)
                {
                    var s = Jets[j % Jets.Count];
                    j++;

                    var rockMove = new Rock(rock);

                    if(s == '<')  rockMove.MoveLeft();
                    else rockMove.MoveRight();

                    if(IsValid(rockMove)) rock = rockMove;

                    var rockDown = new Rock(rock);
                    rockDown.MoveDown();

                    if(IsValid(rockDown)) rock = rockDown;
                    else rest = true;
                }
                var rockTop = rock.GetTop() + 1;

                if(rockTop > Top) Top = rockTop;

                AddOccupied(rock);

                i++;
                number--;
                //Draw();
            }
            return Top;
        }
        void AddOccupied(Rock r) => r.Points.ForEach(p => Occupied.Add((p.X, p.Y)));
        void Draw()
        {
            System.Console.WriteLine("==================");
            for(int i = Top; i >= 0; i--)
            {   
                for(int j = 0; j < 7; j++)
                {
                    if(Occupied.Contains((j,i))) System.Console.Write("#");
                    else System.Console.Write(".");
                }
                System.Console.WriteLine();
            }
        }
        bool IsValid(Rock r) => IsFree(r) && IsInside(r);
        bool IsFree(Rock r)
        {
            foreach(var p in r.Points)
            {
                if(!IsFree(p)) return false;
            }
            return true;
        }
        bool IsFree(Point p) => !Occupied.Contains((p.X, p.Y));
        bool IsInside(Rock r)
        {
            foreach(var p in r.Points)
            {
                if(!IsInside(p)) return false;
            }
            return true;
        }
        bool IsInside(Point p) => p.X >= 0 && p.X < 7 && p.Y >= 0;
    }

    class Rock
    {
        public Rock(Rock rock)
        {
            Points = new List<Point>();
            foreach(Point p in rock.Points)
            {
                Points.Add(new Point(p.X, p.Y));
            }
            Top = 0;
        }
        public Rock()
        {
            Points = new List<Point>();
            Top = 0;
        }
        private int Top { get; set; }
        public void MoveLeft() => Points.ForEach(p => p.MoveLeft());
        public void MoveRight() => Points.ForEach(p => p.MoveRight());
        public void MoveDown() => Points.ForEach(p => p.MoveDown());
        public List<Point> Points { get; set; }
        public int GetTop() => Points.Max(p => p.Y);
        public void SetPositionFromStart(int currentTop)
        {
            foreach(var p in Points)
            {
                p.X += 2;
                p.Y += currentTop + 3;
            }
        }
    }

    class Point
    {
        public Point(int x, int y)
        {
            X = x;
            Y = y;
        }
        public int X { get; set; }
        public int Y { get; set; }
        public void MoveLeft() => X--;
        public void MoveRight() => X++;
        public void MoveDown() => Y--;
    }
}