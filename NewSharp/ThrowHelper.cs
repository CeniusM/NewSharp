

namespace NewSharp;

internal class ThrowHelper
{
    internal static void PrintException(Exception excep)
    {
        Console.WriteLine(excep.Message);
        Console.WriteLine(excep.StackTrace);
    }

    internal static void PrintException(bool statement, Exception excep)
    {
        if (!statement)
            return;
        Console.WriteLine(excep.Message);
        Console.WriteLine(excep.StackTrace);
    }

    internal static void PrintExceptionReadLine(Exception excep)
    {
        Console.WriteLine(excep.Message);
        Console.WriteLine(excep.StackTrace);
        Console.ReadLine();
    }

    internal static void PrintExceptionReadLine(bool statement, Exception excep)
    {
        if (!statement)
            return;
        Console.WriteLine(excep.Message);
        Console.WriteLine(excep.StackTrace);
        Console.ReadLine();
    }

    internal static void Throw(string message)
    {
        throw new Exception(message);
    }

    internal static void ThrowIf(bool statement, string message)
    {
        if (statement)
            Throw(message);
    }

    /// <summary>
    /// (val < low || val > heigh) throw
    /// </summary>
    internal static void ThrowIfOutOfRange(int val, int low, int heigh, string message)
    {
        ThrowIf(val < low || val > heigh, message);
    }
}
