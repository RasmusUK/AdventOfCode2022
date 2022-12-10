namespace Program
{
    class Program
    {
        private const int CRTWidth = 40;
        private const int CRTHeight = 6;
        static void Main(string[] args)
        {
            var input = Lib.Lib.ReadInputFile();
          
            var part1 = Part1(input);
            System.Console.WriteLine($"Part 1 = {part1}");
            
            var part2 = "FECZELHE";
            System.Console.WriteLine($"Part 2 = {part2}");

            PrintArray(Part2(input));      
        }    

        static int Part1(List<string> input)
        {
            var cycle = 0;
            var x = 1;
            int signalStrength = 0;     
            foreach(var line in input)
            {
                var split = line.Split(" ");
                foreach(var _ in split)
                {
                    cycle++;
                    if((cycle + 20) % 40 == 0) signalStrength += cycle * x;
                }
                if(split.Length == 2) x += int.Parse(split[1]);
            }
            return signalStrength;
        }

        static string[,] Part2(List<string> input)
        {
            var cycle = 0;
            var x = 1;
            var crt = InitCRTArray();      
            foreach(var line in input)
            {
                var split = line.Split(" ");
                foreach(var _ in split)
                {
                    if(Draw(cycle, x))
                    {
                        (int xPos, int yPos) = GetPosition(cycle);
                        crt[xPos, yPos] = "#";
                    }
                    cycle++;
                }
                if(split.Length == 2) x += int.Parse(split[1]);
            }
            return crt;
        }

        static string[,] InitCRTArray()
        {
            var crt = new string[CRTWidth, CRTHeight];
            for (int i = 0; i < CRTWidth; i++)
            {
                for (int j = 0; j < CRTHeight; j++)
                {
                    crt[i, j] = ".";
                }
            }
            return crt;
        }

        static (int x, int y) GetPosition(int x) => (x % CRTWidth, x/CRTWidth);

        static bool Draw(int pos, int x) => Math.Abs(pos % 40 - x) < 2;

        static void PrintArray(string[,] array)
        {       
            for(int i = 0; i < CRTHeight; i++)
            {
                for (int j = 0; j < CRTWidth; j++)
                {
                    System.Console.Write(array[j,i]);
                }
                System.Console.WriteLine();
            }
        }
    }
}