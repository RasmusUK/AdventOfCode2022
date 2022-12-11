namespace Program
{
    class Program
    {
        static void Main(string[] args)
        {
            var input = Lib.Lib.ReadInputFile();

            var part1 = Answer(input, 20, (x, _) => x / 3);
            System.Console.WriteLine($"Part 1 = {part1}");

            var part2 = Answer(input, 10000, (x, y) => x % y);
            System.Console.WriteLine($"Part 2 = {part2}");
        }    

        static long Answer(List<string> input, long roundCount, Func<long, long, long> operation)
        {
            Dictionary<long, Monkey> monkeys = new Dictionary<long, Monkey>();

            for(int i = 0; i < input.Count; i+=7)
            {
                var monkeyNumber = GetCurrentMonkey(input[i]);
                var items = GetItems(input[i+1]);
                var update = GetOperation(input[i+2]);
                var test = GetTest(input[i+3]);
                var monkeyTrue = GetMonkey(input[i+4]);
                var monkeyFalse = GetMonkey(input[i+5]);
                var monkey = new Monkey(items, monkeyTrue, monkeyFalse, update, test);
                monkeys.Add(monkeyNumber, monkey);
            }

            long modulo =  monkeys.Values.Select(e => e.Mod).Aggregate(1L, (x, y) => x*y);

            for(int i = 0; i < roundCount; i++)
            {
                for(int j = 0; j < monkeys.Count; j++)
                {
                    monkeys = monkeys[j].PerformAction(monkeys, x => operation(x, modulo));
                }
            }

            return monkeys.Values.OrderByDescending(e => e.Count).Select(e => e.Count).Take(2).Aggregate((x,y) => x * y);
        }

        static long GetCurrentMonkey(string line) => long.Parse(line.Replace(":", "").Trim().Split(" ")[1]);

        static List<long> GetItems(string line) => line.Replace(",", "").Replace(":","").Trim().Split(" ").Skip(2).Select(e => long.Parse(e.ToString())).ToList();

        static Func<long, long> GetOperation(string line)
        {
            var operationLine = line.Replace(":","").Replace("=","").Trim().Split(" ").Skip(3).ToList();
            if(operationLine[1] == "*")
            {
                if(operationLine[2] == "old") return x => x * x;
                return x => x * long.Parse(operationLine[2]);
            }
            else
            {
                if(operationLine[2] == "old") return x => x + x;
                return x => x + long.Parse(operationLine[2]);
            }
        }

        static long GetTest(string line) => long.Parse(line.Split(" ").Last());

        static long GetMonkey(string line) => long.Parse(line.Split(" ").Last());      
    }

    class Monkey
    {
        public Monkey(List<long> items, long monkeyTrue, long monkeyFalse, Func<long, long> operation, long test)
        {
            Items = items;
            MonkeyTrue = monkeyTrue;
            MonkeyFalse = monkeyFalse;
            Update = operation;
            Mod = test;
        }

        List<long> Items { get; set; }
        long MonkeyTrue { get; set; }
        long MonkeyFalse { get; set; }
        Func<long, long> Update { get; set; }
        public long Mod { get; set; }
        public long Count { get; set; }

        private bool PerformOperation(int index, Func<long, long> operation)
        {
            Count++;
            var item = Items[index];
            item = operation(Update(item)); 
            Items[index] = item;
            return item % Mod == 0; 
        }

        public Dictionary<long, Monkey> PerformAction(Dictionary<long, Monkey> monkeys, Func<long, long> operation)
        {
            for(int i = 0; i < Items.Count; i++)
            {
                var test = PerformOperation(i, operation);
                if(test) monkeys[MonkeyTrue].Items.Add(Items[i]);
                else monkeys[MonkeyFalse].Items.Add(Items[i]);             
            }
            Items.Clear();
            return monkeys;
        }      
    }
}