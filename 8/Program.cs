namespace Program
{
    class Program
    {
        static void Main(string[] args)
        {
            var forrest = Lib.Lib.ReadInputFile().Select(e => e.Select(f => int.Parse(f.ToString())).ToArray()).ToArray();
            var part1 = Answer(forrest).Part1;
            System.Console.WriteLine($"Part 1 = {part1}");
            var part2 = Answer(forrest).Part2;
            System.Console.WriteLine($"Part 2 = {part2}");
        }

        static (int Part1, int Part2) Answer(int[][] forrest)
        {
            var scores = new List<int>();
            int count = forrest.Length * 2 + forrest[0].Length * 2 - 4;
            for(int i = 1; i < forrest.Length - 1; i++)
            {
                for(int j = 1; j < forrest[0].Length - 1; j++)
                {
                    if(IsVisable((i,j), forrest)) count++;
                    scores.Add(Score((i, j), forrest));
                }
            }
            return (count, scores.Max());
        }

        static int Score((int, int) pos, int[][] forrest) => 
        VisableAndScoreInDirection(pos, 0, 1, forrest).Score * 
        VisableAndScoreInDirection(pos, 1, 0, forrest).Score * 
        VisableAndScoreInDirection(pos, 0, -1, forrest).Score * 
        VisableAndScoreInDirection(pos, -1, 0, forrest).Score;
        
        static bool IsVisable((int, int) pos, int[][] forrest) => 
        VisableAndScoreInDirection(pos, 0, 1, forrest).Visable || 
        VisableAndScoreInDirection(pos, 1, 0, forrest).Visable || 
        VisableAndScoreInDirection(pos, 0, -1, forrest).Visable || 
        VisableAndScoreInDirection(pos, -1, 0, forrest).Visable;
        static (bool Visable, int Score) VisableAndScoreInDirection((int, int) pos, int hor, int ver, int[][] forrest)
        {
            int count = 0;
            int i = pos.Item1;
            int j = pos.Item2;
            int height = forrest[i][j];
            while(i > 0 && i < forrest.Length - 1 && j > 0 && j < forrest[0].Length - 1)
            {
                i += hor;
                j += ver;
                count++;
                if(forrest[i][j] >= height) return (false, count);
            }
            return (true, count);
        }
    }
}