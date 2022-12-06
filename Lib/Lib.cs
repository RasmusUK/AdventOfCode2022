namespace Lib;
public static class Lib
{
    public static List<string> ReadFileFromPath(string path) => new List<string>(File.ReadAllLines(path));
}
