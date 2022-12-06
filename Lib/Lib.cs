namespace Lib;
public static class Lib
{
    public static string GetPathRootFolder() => System.IO.Path.Combine(System.AppContext.BaseDirectory, "..\\..\\..");
    public static List<string> ReadFileFromPath(string path) => new List<string>(File.ReadAllLines(path));
    public static List<string> ReadInputFile() => ReadFileFromPath(Path.GetFullPath(GetPathRootFolder() + "\\input.txt"));
    public static List<string> ReadInputFile(string fileName) => ReadFileFromPath(Path.GetFullPath(GetPathRootFolder() + $"\\{fileName}"));
}
