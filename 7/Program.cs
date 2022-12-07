namespace Program
{
    class Program
    {
        static void Main(string[] args)
        {
            var input = Lib.Lib.ReadInputFile();
            var rootDirectory = GetFileSystem(input);
            var part1 = Part1(rootDirectory);
            var availSpace = 70000000;
            var spaceNeeded = 30000000;
            var spaceRequired = (availSpace - rootDirectory.Size - spaceNeeded) * -1;
            var part2 = Part2(rootDirectory, spaceRequired, int.MaxValue);
            Console.WriteLine($"Part 1 = {part1}");
            Console.WriteLine($"Part 2 = {part2}");
        }

        private static int Part2(Directory directory, int spaceRequired, int best)
        {
            if(directory.Size >= spaceRequired && directory.Size < best) best = directory.Size;
            if(directory.Children.Count == 0) return best;
            return directory.Children.Values.Select(e => Part2(e, spaceRequired, best)).Min();
        }

        private static int Part1(Directory directory)
        {
            int size = 0;
            if(directory.Size <= 100000) size = directory.Size;
            return size + directory.Children.Sum(e => Part1(e.Value));
        }

        private static Directory GetFileSystem(List<string> input)
        {
            var parent = new Directory();
            var currentDirectory = parent;
            for(int i = 1; i < input.Count; i++)
            {
                var line = input[i];
                if(line.Contains("$"))
                {
                    var lineSplit = line.Split(" ");
                    var command = lineSplit[1];
                    if(command == "cd")
                    {
                        var directoryName = lineSplit[2];
                        if(directoryName == ".." && currentDirectory.Parent != null) currentDirectory = currentDirectory.Parent;
                        else currentDirectory = currentDirectory.Children[directoryName];
                    }
                    else
                    {
                        var (directories, files, index) = GetContentOfDirectory(input, i + 1);
                        currentDirectory.Files.AddRange(files);
                        currentDirectory.AddDirectories(directories);
                        i = index - 1;
                    }
                }
            }
            return parent;
        }

        private static (List<string> directories, List<File> files, int index) GetContentOfDirectory(List<string> input, int index)
        {
            var directories = new List<string>();
            var files = new List<File>();
            int i;
            for(i = index; i < input.Count && !input[i].Contains("$"); i++)
            {
                var line = input[i];
                var lineSplit = line.Split(" ");
                var fst = lineSplit[0];
                var snd = lineSplit[1];
                if(fst == "dir") directories.Add(snd);
                else files.Add(new File(int.Parse(fst), snd));    
            }
            return (directories, files, i);
        }
    }

    class File
    {
        public File(int size, string name)
        {
            Size = size;
            Name = name;
        }

        public int Size { get; set; }
        public string Name { get; set; }
    }

    class Directory
    {
        public Directory()
        {
            Parent = null;
            Children = new Dictionary<string, Directory>();
            Files = new List<File>();
        }
        public Directory(Directory parent)
        {
            Parent = parent;
            Children = new Dictionary<string, Directory>();
            Files = new List<File>();
        }
        public Directory? Parent { get; set; }
        public Dictionary<string, Directory> Children { get; set; }
        public List<File> Files { get; set; }

        public int Size 
        {  
            get 
            {
                return CalculateSize();
            }
        }

        public void AddDirectories(List<string> directories) => directories.ForEach(directory => Children.Add(directory, new Directory(this)));

        private int CalculateSize() => Files.Sum(e => e.Size) + Children.Sum(e => e.Value.Size);
    }
}