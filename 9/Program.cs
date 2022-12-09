namespace Program
{
    class Program
    {
        static void Main(string[] args)
        {
            var input = Lib.Lib.ReadInputFile().Select(e => (e.Split(" ")[0], int.Parse(e.Split(" ")[1]))).ToList();
          
            var part1 = Answer(input, 2);
            System.Console.WriteLine($"Part 1 = {part1}");

            var part2 = Answer(input, 10);
            System.Console.WriteLine($"Part 2 = {part2}");
        }
        
        static int Answer(List<(string, int)> input, int count)
        {
            var tailPositions = new HashSet<(int,int)>();
            tailPositions.Add((0,0));

            var pos = new Dictionary<int, (int, int)>();

            for(int i = 0; i < count; i++)
            {
                pos.Add(i,(0,0));
            }

            foreach(var line in input)
            {
                for(int i = 0; i < line.Item2; i++)
                {
                    pos = Move(line.Item1, pos);
                    tailPositions.Add(pos[count-1]);
                }  
            }

            return tailPositions.Count();
        }
        static Dictionary<int, (int, int)> Move(string direction, Dictionary<int, (int, int)> pos)
        {
            pos[0] = MoveHead(direction, pos[0]);

            for(int i = 1; i < pos.Count; i++)
            {
                pos[i] = MoveTail(pos[i-1], pos[i]);
            }

            return pos;
        }

        static (int,int) MoveHead(string direction, (int x, int y) pos)
        {
            switch(direction)
            {
                case "R":
                    pos.x++;
                    break;
                case "L":
                    pos.x--;
                    break;
                case "U":
                    pos.y++;
                    break;
                case "D":
                    pos.y--;
                    break;  
            }          

            return pos;
        }

        static (int,int) MoveTail((int x, int y) headPos, (int x, int y) tailPos)
        {
            var xDif = headPos.x - tailPos.x;
            var yDif = headPos.y - tailPos.y;

            if(IsDiagonal(headPos, tailPos) && (Math.Abs(xDif) > 1 || Math.Abs(yDif) > 1))
            {
                tailPos.x += Math.Sign(xDif); 
                tailPos.y += Math.Sign(yDif);
            }
            else
            {
                if(Math.Abs(xDif) > 1) tailPos.x += Math.Sign(xDif); 
                else if(Math.Abs(yDif) > 1)tailPos.y += Math.Sign(yDif);
            }

            return tailPos;
        }

        static bool IsDiagonal((int x, int y) headPos, (int x, int y) tailPos) => headPos.x != tailPos.x && headPos.y != tailPos.y;
    }
}