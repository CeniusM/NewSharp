

namespace NewSharp;

internal class Helper
{
    internal static bool TryReadFile(string inFilePath, out List<string> outLines)
    {
        try
        {
            outLines = File.ReadAllLines(inFilePath).ToList();
            return true;
        }
        catch (Exception)
        {
            outLines = new List<string>();
            return false;
        }
    }
}
