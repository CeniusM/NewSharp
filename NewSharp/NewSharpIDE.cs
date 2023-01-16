

using System.Diagnostics;

namespace NewSharp;

internal class NewSharpIDE
{
    private Interpreter _codeRunner = new Interpreter();
    private List<List<char>> _lines = new List<List<char>>();
    private string _filePath = "";
    private bool _running = false;
    private bool _justPressedEsc = false;
    private Stopwatch _timeAtPress = new Stopwatch();

    private List<char> _clipBoard = new List<char>();
    private (int X, int Y) _clipStart;
    private (int X, int Y) _clipEnd;


    private int _xCursor = 0; // Corusponse both the the _lines xy + offset and the console xy
    private int _yCursor = 0;
    private int _lineOffSet = 0; // The line number of the top veiweble line
    private int _windowWidth = 0;
    private int _windowHeight = 0;

    public NewSharpIDE()
    {

    }

    private string _GetCode()
    {
        string code = "";
        for (int i = 0; i < _lines.Count; i++)
        {
            code += _lines[i].ToArray() + "\n";
        }
        return code;
    }

    private void _RunCode()
    {
        _codeRunner.RunCode(_GetCode());
    }

    internal void Start(string inFilePath)
    {
        if (_running)
            return;
        _running = true;
        Console.ResetColor();
        Console.Clear();
        if (Helper.TryReadFile(inFilePath, out var lines))
        {
            for (int i = 0; i < lines.Count; i++)
                _lines.Add(lines[i].ToCharArray().ToList());
            _filePath = inFilePath;
        }
        else
        {
            _running = false;
            Console.WriteLine("where no able to read a file at " + inFilePath);
            return;
        }

        _Environment();
    }

    private void _ReplaceNotValidCharecters()
    {
        for (int i = 0; i < _lines.Count; i++)
        {
            int index = 0;
            index = _lines[i].FindIndex(s => s == '\t');
            while (index != -1)
            {
                _lines[i][index] = ' ';
                index = _lines[i].FindIndex(s => s == '\t');
            }
        }
    }

    private void _Environment()
    {
        _ReplaceNotValidCharecters();
        Console.CursorVisible = false;
        _PrintScreen(); // If screen move
        while (_running)
        {
            _PrintLine(_yCursor + _lineOffSet); // If only line have changed
            _PrintLine(_yCursor + _lineOffSet + 1); // To remove cursor background
            _PrintLine(_yCursor + _lineOffSet - 1); // To remove cursor background
            if (_windowWidth != Console.WindowWidth || _windowHeight != Console.WindowHeight)
                _PrintScreen();
            if (_justPressedEsc)
            {
                Console.WriteLine("Double click ESC to quit aplication");
            }
            var key = Console.ReadKey();
            int lineY = _yCursor + _lineOffSet;
            int lineX = _xCursor;
            char c = key.KeyChar;
            ConsoleKey keyVal = key.Key;
            ConsoleModifiers keyMod = key.Modifiers;
            if (key.Key == ConsoleKey.RightArrow)
            {
                if (_xCursor < _lines[_yCursor + _lineOffSet].Count)
                    _xCursor += 1;
            }
            else if (key.Key == ConsoleKey.LeftArrow)
            {
                if (_xCursor > 0)
                    _xCursor -= 1;
            }
            else if (key.Key == ConsoleKey.DownArrow)
            {
                if (_yCursor < _lines.Count - 1)
                    _yCursor += 1;
                if (_xCursor > _lines[_yCursor + _lineOffSet].Count)
                    _xCursor = _lines[_yCursor + _lineOffSet].Count;
            }
            else if (key.Key == ConsoleKey.UpArrow)
            {
                if (_yCursor > 0)
                    _yCursor -= 1;
                if (_xCursor > _lines[_yCursor + _lineOffSet].Count)
                    _xCursor = _lines[_yCursor + _lineOffSet].Count;
            }
            else if (key.Key == ConsoleKey.Enter)
            {

            }
            else if (key.Key == ConsoleKey.Backspace)
            {

            }
            else if (key.Key == ConsoleKey.Tab)
            {

            }
            else if (key.Key == ConsoleKey.Escape)
            {
                if (_justPressedEsc && _timeAtPress.Elapsed.TotalMilliseconds < 300)
                {

                    _running = false;
                    _CleanUp();
                    return;
                }
                else
                {
                    _timeAtPress.Restart();
                    _justPressedEsc = true;
                    continue;
                }
            }
            else if (keyVal == ConsoleKey.Oem2 && keyMod == ConsoleModifiers.Control)
            {
                if (_lines[lineY].Count > 1 && _lines[lineY][0] == '/' && _lines[lineY][1] == '/')
                {
                    _lines[lineY].RemoveAt(0);
                    _lines[lineY].RemoveAt(0);
                }
                else
                {
                    _lines[lineY].Insert(0, '/');
                    _lines[lineY].Insert(0, '/');
                }
            }
            else
            {
                if (lineY > -1 && lineY < _lines.Count)
                {
                    if (lineX > -1 && lineX < _lines[lineY].Count)
                    {
                        _lines[lineY].Insert(lineX, key.KeyChar);
                        lineX += 1;
                    }
                    else if (lineX == _lines[lineY].Count)
                    {
                        _lines[lineY].Add(key.KeyChar);
                        lineX += 1;
                    }
                }

            }

            _timeAtPress.Stop();
            _justPressedEsc = false;
        }
    }

    private void _CleanUp()
    {
        _codeRunner.Clear();
        _running = false;
        _lines.Clear();
        _filePath = "";
    }

    #region Rendering

    private ConsoleColor _comments = ConsoleColor.DarkGreen;
    private ConsoleColor _functions = ConsoleColor.Yellow;
    private ConsoleColor _varibles = ConsoleColor.Blue;
    private ConsoleColor _ifendif = ConsoleColor.Magenta;
    private ConsoleColor _error = ConsoleColor.Red;

    private void _PrintScreen()
    {
        // Buttons at some point?

        _windowWidth = Console.WindowWidth;
        _windowHeight = Console.WindowHeight;


        Console.ResetColor();
        // Only render lines that are veiweble
        int top = _lineOffSet;
        int bottom = Math.Min(_lineOffSet + Console.WindowHeight, _lines.Count - 1);
        for (int i = top; i < bottom; i++)
        {
            _PrintLine(i);
        }
    }

    private void _PrintLine(int lineNum, bool clearLine = true)
    {
        if (lineNum < 0 || lineNum > _lines.Count)
            return;
        var line = _lines[lineNum];
        Console.ForegroundColor = ConsoleColor.Green;
        Console.BackgroundColor = ConsoleColor.Black;
        if (clearLine)
        {
            Console.SetCursorPosition(0, lineNum);
            Console.WriteLine(new string(' ', Console.BufferWidth));
        }
        Console.SetCursorPosition(0, lineNum);
        Console.WriteLine(new string(line.ToArray()));

        if (lineNum == _yCursor + _lineOffSet)
        {
            Console.SetCursorPosition(_xCursor, _yCursor);
            Console.BackgroundColor = ConsoleColor.Gray;
            if (_lines.Count > lineNum && _lines[lineNum].Count > _xCursor)
                Console.Write(_lines[lineNum][_xCursor]);
            else
                Console.Write(' ');
        }
        Console.SetCursorPosition(_xCursor, _yCursor);
        Console.ResetColor();
    }

    #endregion
}
