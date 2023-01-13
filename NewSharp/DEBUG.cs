using System.Diagnostics;

namespace NewSharp;

internal class DEBUG
{
    [ConditionalAttribute("DEBUG")]
    internal static void Print(string str)
    {
        Console.WriteLine(str);
    }

    [ConditionalAttribute("DEBUG")]
    internal static void Print(bool statement, string str)
    {
        if (!statement)
            return;
        Print(str);
    }

    [ConditionalAttribute("DEBUG")]
    internal static void Print(List<string> lines)
    {
        for (int i = 0; i < lines.Count; i++)
            Console.WriteLine(lines[i]);
    }

    [ConditionalAttribute("DEBUG")]
    internal static void Print(bool statement, List<string> lines)
    {
        if (!statement)
            Print(lines);
    }

    [ConditionalAttribute("DEBUG")]
    internal static void ReadLine()
    {
        Console.ReadLine();
    }

    [ConditionalAttribute("DEBUG")]
    internal static void ReadLine(bool statement)
    {
        if (!statement)
            ReadLine();
    }
}
