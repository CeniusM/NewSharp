

namespace NewSharp;

internal class NewSharpIDE
{
    private Interpreter _codeRunner = new Interpreter();
    private List<List<char>> _lines = new List<List<char>>();
    private string _filePath = "";
    private bool _running = false;


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
        _PrintScreen(); // If screen move
        while (_running)
        {
            _PrintLine(_yCursor + _lineOffSet); // If only line have changed
            if (_windowWidth != Console.WindowWidth || _windowHeight != Console.WindowHeight)
                _PrintScreen();
            var key = Console.ReadKey();

            if (key.Key == ConsoleKey.RightArrow)
            {
                if (_xCursor < _lines[_yCursor + _lineOffSet].Count - 1)
                    _xCursor += 1;
            }
            else if (key.Key == ConsoleKey.LeftArrow)
            {
                if (_xCursor > 0)
                    _xCursor -= 1;
            }
            else if (key.Key == ConsoleKey.DownArrow)
            {
                if (_xCursor < _lines.Count - 1)
                    _yCursor += 1;
            }
            else if (key.Key == ConsoleKey.UpArrow)
            {
                if (_yCursor > 0)
                    _yCursor -= 1;
            }
            else if (key.KeyChar == '\n')
            {

            }
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
        var line = _lines[lineNum];
        Console.ForegroundColor = ConsoleColor.Green;
        if (clearLine)
        {
            Console.SetCursorPosition(0, lineNum);
            Console.WriteLine(new string(' ', Console.BufferWidth));
        }
        Console.SetCursorPosition(0, lineNum);
        Console.WriteLine(new string(line.ToArray()));

        Console.SetCursorPosition(_xCursor, _yCursor);
        Console.BackgroundColor = ConsoleColor.Black;
        if (_lines.Count > _yCursor + _lineOffSet && _lines[_yCursor + _lineOffSet].Count > _xCursor)
            Console.Write(_lines[_yCursor + _lineOffSet][_xCursor]);
        else
            Console.Write(' ');
        Console.ResetColor();
    }

    #endregion
}
