namespace Program
{
    class Program
    {
        static void Main(string[] args)
        {
            var input = Lib.Lib.ReadFileFromPath(Directory.GetCurrentDirectory()+"\\input.txt")[0].ToCharArray().ToList();
            Console.WriteLine($"Part 1 = {FindIndex(input, 4)}");
            Console.WriteLine($"Part 2 = {FindIndex(input, 14)}");
        }

        static int FindIndex(List<char> input, int count)
        {
            for(int i = 0; i < input.Count-(count-1); i++)
            {
                if(input.Skip(i).Take(count).Distinct().Count() == count) return i+count;
            }
            return 0;
        }
    }
}