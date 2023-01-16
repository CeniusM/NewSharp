// Note: Should probely use IEnumerable<char> or StringBuilder for all of this...

using System.Diagnostics;
using System.Text;

namespace NewSharp;

public class Interpreter
{
    private Dictionary<(string Name, int Parameters, bool Returns), Func<int, int, int>> BuildIn = new Dictionary<(string Name, int Parameters, bool Returns), Func<int, int, int>>()
    {

    };

    private Dictionary<(string Name, int Parameters, bool Returns), (List<string> Func, int LineOffSet)> NameFunc = new Dictionary<(string Name, int Parameters, bool Returns), (List<string> Func, int LineOffSet)>();

    private Dictionary<string, int> NameVaribleIndex = new Dictionary<string, int>();

    private List<int> Varibles = new List<int>();

    private int[] Array1 = new int[262144]; // 1 MB worth of ints

    //private int[,] Array2 = new int[512, 512]; // 1 MB worth of 2d ints

    //private int[,,] Array3 = new int[64, 64, 64]; // 1 MB worth of 3d ints

    private Stack<int> Stack = new Stack<int>();

    private List<string> LinesInput = new List<string>();

    private Stopwatch sw = new Stopwatch();

    private Random rand = new Random();

    private bool running = false;
    public bool Running => running;

    public Interpreter()
    {
        BuildIn.Add(("Print", 1, false), __PRINT);
        BuildIn.Add(("Pop", 0, true), __POP);
        BuildIn.Add(("Peek", 0, true), __PEEK);
        BuildIn.Add(("Push", 1, false), __PUSH);
        BuildIn.Add(("Plus", 0, false), __PLUS);
        BuildIn.Add(("Minus", 0, false), __MINUS);
        BuildIn.Add(("Mul", 0, false), __MUL);
        BuildIn.Add(("Div", 0, false), __DIV);
        BuildIn.Add(("StackCount", 0, true), __STACKCOUNT);
        BuildIn.Add(("Sleep", 1, false), __SLEEP);
        BuildIn.Add(("Clear", 0, false), __CLEAR);
        BuildIn.Add(("StartTimer", 0, false), __STARTTIMER);
        BuildIn.Add(("StopTimer", 0, false), __STOPTIMER);
        BuildIn.Add(("Rand", 2, true), __RAND);
        BuildIn.Add(("ReadKey", 0, true), __READKEY);
        BuildIn.Add(("Array1", 2, false), __ARRAY1SET);
        BuildIn.Add(("Array1", 1, true), __ARRAY1GET);
    }

    private int __ARRAY1GET(int var1, int foo2)
    {
        return Array1[var1];
    }

    private int __ARRAY1SET(int var1, int var2)
    {
        //SetErrorIf(var1 < 0 || var1 > Array.Length - 1, );
        Array1[var1] = var2;
        return 0;
    }

    private int __READKEY(int foo1, int foo2)
    {
        return Console.ReadKey().KeyChar;
    }

    private int __RAND(int var1, int var2)
    {
        return rand.Next(var1, var2 + 1);
    }

    private int __STARTTIMER(int foo1, int foo2)
    {
        sw.Restart();
        return 0;
    }

    private int __STOPTIMER(int foo1, int foo2)
    {
        Stack.Push((int)sw.ElapsedMilliseconds);
        sw.Stop();
        return 0;
    }

    private int __CLEAR(int foo1, int foo2)
    {
        Console.Clear();
        return 0;
    }

    private int __SLEEP(int var1, int foo2)
    {
        Thread.Sleep(var1);
        return 0;
    }

    private int __MUL(int foo1, int foo2)
    {
        int var1 = Stack.Pop();
        int var2 = Stack.Pop();
        Stack.Push(var2 * var1);
        return 0;
    }

    private int __DIV(int foo1, int foo2)
    {
        int var1 = Stack.Pop();
        int var2 = Stack.Pop();
        Stack.Push(var2 / var1);
        return 0;
    }

    private int __STACKCOUNT(int var1, int foo2)
    {
        return Stack.Count;
    }

    private int __PRINT(int var1, int foo2)
    {
        Console.WriteLine(var1);
        return 0;
    }

    private int __PUSH(int var1, int foo2)
    {
        Stack.Push(var1);
        return 0;
    }

    private int __PEEK(int foo1, int foo2)
    {
        return Stack.Peek();
    }

    private int __POP(int foo1, int foo2)
    {
        return Stack.Pop();
    }

    private int __MINUS(int foo1, int foo2)
    {
        int var1 = Stack.Pop();
        int var2 = Stack.Pop();
        Stack.Push(var2 - var1);
        return 0;
    }

    private int __PLUS(int foo1, int foo2)
    {
        int var1 = Stack.Pop();
        int var2 = Stack.Pop();
        Stack.Push(var1 + var2);
        return 0;
    }

    private bool DoesStringContain(string input, string match, int offset)
    {
        return string.Join("", input.Take(new Range(offset, match.Length + offset))) == match;
        //return false;
    }

    private string RemoveEmpty(string code)
    {
        // Must ignore
        // PrintText("This part of the input")

        StringBuilder newStr = new StringBuilder(code);
        int funcLength = "PrintText(\"".Length;

        int lastIndex = 0;
        while (true)
        {
            // Change out the spaces with random non used char like 24(cancel) in ascii
            // * 25 for '\t'
            int index = code.IndexOf("PrintText(\"", lastIndex);
            int endIndex = code.IndexOf("\")", index + funcLength);
            if (index == -1 || endIndex == -1)
                break;

            lastIndex = endIndex;

            for (int i = index; i < endIndex; i++)
            {
                if (newStr[i] == ' ')
                    newStr[i] = (char)24;
                else if (newStr[i] == '\t')
                    newStr[i] = (char)25;
            }
        }

        newStr = newStr.Replace(" ", "");
        newStr = newStr.Replace("\t", "");
        newStr = newStr.Replace((char)24, ' ');
        newStr = newStr.Replace((char)25, '\t');
        return newStr.ToString();

        //bool doingText = false;
        //for (int i = 0; i < code.Length; i++)
        //{
        //    char c = code[i];
        //    if ()
        //}




        // Later it is important to throw error if the name is using spaces
        //return code.Replace(" ", "").Replace("\t", "");
    }

    private List<string> GetLines(string code)
    {
        return code.Split('\n').ToList();
    }

    private List<string> RemoveEmptyLines(List<string> lines)
    {
        for (int i = 0; i < lines.Count; i++)
        {
            if (lines[i].Count() == 0)
            {
                lines.RemoveAt(i);
                i--;
            }
        }
        return lines;
    }

    private List<string> RemoveComments(List<string> lines)
    {
        for (int i = 0; i < lines.Count; i++)
        {
            for (int j = 0; j < lines[i].Length - 1; j++)
            {
                if (lines[i][j] == '/' && lines[i][j + 1] == '/'
                    || lines[i][j] == ';'
                    || lines[i][j] == '#'
                    )
                {
                    if (j == 0)
                    {
                        lines[i] = lines[i].Remove(j, lines[i].Length - j);
                        i--;
                        break;
                    }
                    else // Part of line
                    {
                        lines[i] = lines[i].Remove(j, lines[i].Length - j);
                    }
                }
            }
        }
        return lines;
    }

    private void PrintError(List<string> lines, string message, int lineNum)
    {
        for (int i = 0; i < LinesInput.Count; i++)
        {
            if (i == lineNum)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(LinesInput[i] + " <-- " + message + " Line: " + (lineNum + 1));
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine(LinesInput[i]);
            }
        }
        Console.ResetColor();
        Clear();
    }

    /// <summary>
    /// Very ineficient
    /// </summary>
    private void ValidateName(string name, int lineOffSet)
    {
        string InvalidCharacters = "{[()]},.-+?*^¨><\"\\~|\\";
        SetErrorIf(name == "", lineOffSet, "A name cannot be empty");
        SetErrorIf("1234567890".Contains(name[0]), lineOffSet, "A name can not have its first charecter be a number");
        for (int i = 0; i < name.Length; i++)
        {
            if (InvalidCharacters.Contains(name[i]))
                SetError(lineOffSet, "A name can not contain \"" + name[i] + "\"");
        }
    }

    private void SetErrorIf(bool statement, int lineNum, string lineMessage)
    {
        if (statement)
            SetError(lineNum, lineMessage);
    }

    private void SetError(int lineNum, string lineMessage)
    {
        running = false;
        throw new Exception(GetErrorMessage(lineNum, lineMessage));
    }

    private string GetErrorMessage(int lineNum, string lineMessage)
    {
        return lineNum + ":" + lineMessage;
    }

    /// <summary>
    /// Used for definitions like
    /// Foo(x, name)
    /// {
    /// 
    /// }
    /// </summary>
    private string[] GetParameterNames(string line, int lineOffSet, bool validateNames = true, bool splitAtComma = true)
    {
        SetErrorIf(!(line.Contains('(') && line.Contains(')')), lineOffSet, "Function does not contain parameters");
        string str = line.Split('(', 2)[1];
        str = str.Remove(str.Length - 1);

        //int commasCount = str.Count(x => x == ',');
        int commasCount = 0;
        int parameterDepth = 0; // Tells us if we are inside a depper function parameters
        int index = -1;
        for (int i = 0; i < str.Length; i++)
        {
            if (str[i] == '(')
                parameterDepth++;
            else if (str[i] == ')')
                parameterDepth--;
            else if (str[i] == ',' && parameterDepth == 0)
            {
                index = i;
                commasCount++;
            }
        }


        SetErrorIf(commasCount > 1, lineOffSet, "There can not be more than 2 parameters");

        //SetErrorIf(commasCount < 0, lineOffSet, "There can not be less than 0 parameters"); // HOW WOULD THIS BE POSSIBLE
        string[] names = new string[0];
        if (splitAtComma && index != -1)
        {
            // IM LOSING MY MIND
            //str[index] = (char)24;
            // Idc anymore after 900 lines of code, efficiency is my last priority for now
            StringBuilder tempStr = new StringBuilder(str);
            tempStr[index] = (char)24;
            str = tempStr.ToString();
            names = str.Split((char)24);
        }
        else
            names = new string[1] { str };
        if (names.Length == 1)
            if (names[0] == "")
                return new string[0];
        if (validateNames)
            for (int i = 0; i < names.Length; i++)
                ValidateName(names[i], lineOffSet);
        if (names.Length == 2)
            SetErrorIf(names[0] == names[1], lineOffSet, "Both parameter names can not be the same");
        return names;
    }

    /// <summary>
    /// For example
    /// Set(var, 3)
    /// Foo(Get(var), 312)
    /// </summary>
    private int[] GetValuesFromParameters(string parameter, int lineOffSet)
    {
        int scopeLevel = 0;

        if (parameter == "")
            return new int[0];

        int MethodDepth = 0; // methods inside methods like "foo,RandomThing(13,re())"

        bool Parameter2 = false; // Used for telling if it is var1 or 2 being initelized
        StringBuilder var1 = new StringBuilder(parameter.Length);
        StringBuilder var2 = new StringBuilder(parameter.Length);

        void Switch()
        {
            if (Parameter2)
                SetError(lineOffSet, "To many parameters");
            else
                Parameter2 = true;
        }

        string newMethod = "";

        // Get parameters count
        for (int i = 0; i < parameter.Length; i++)
        {
            if (Parameter2)
                var2.Append(parameter[i]);
            else
                var1.Append(parameter[i]);
            if (parameter[i] == '(')
                MethodDepth++;
            else if (MethodDepth != 0)
            {
                //if (Parameter2)
                //    var2.Append(parameter[i]);
                //else
                //    var1.Append(parameter[i]);

                if (parameter[i] == ')')
                    MethodDepth--;
                //else
                //    newMethod += parameter[i];
                if (MethodDepth == 0)
                {
                    int result = 0;
                    if (Parameter2)
                        result = RunFunction(var2.ToString(), lineOffSet, true);
                    else
                        result = RunFunction(var1.ToString(), lineOffSet, true);
                    if (Parameter2)
                        var2 = new StringBuilder("" + result);
                    else
                        var1 = new StringBuilder("" + result);
                }

            }
            else if (parameter[i] == ',')
            {
                var1.Remove(var1.Length - 1, 1);
                Switch();
            }


            //if (parameter[i] == ',')
            //{
            //    SetErrorIf(Parameter2 && !BuildingMethod, lineOffSet, "To many parameters");

            //}
        }

        //DEBUG.Print(var1.ToString());
        //DEBUG.Print(var2.ToString());
        //DEBUG.ReadLine();

        string str1 = var1.ToString();
        string str2 = var2.ToString();
        int value1 = 0;
        int value2 = 0;

        SetErrorIf(str2 == "" && Parameter2, lineOffSet, "A Parameter can not be empty");

        if (int.TryParse(str1, out int outInt1))
            value1 = outInt1;
        else if (NameVaribleIndex.ContainsKey(str1))
            value1 = Varibles[NameVaribleIndex[str1]];
        else
            SetErrorIf(str1 != "", lineOffSet, "No varible called " + str1);
        if (int.TryParse(str2, out int outInt2))
            value2 = outInt2;
        else if (NameVaribleIndex.ContainsKey(str2))
            value2 = Varibles[NameVaribleIndex[str2]];
        else
            SetErrorIf(str2 != "", lineOffSet, "No varible called " + str2);

        if (str2 != "")
            return new int[] { value1, value2 };
        else if (str1 != "")
            return new int[] { value1 };
        else
            return new int[0];
    }

    /// <summary>
    /// Searches for brackets on current and next lines, if returns true, that can be used to say a function is declaring a function
    /// </summary>
    private bool SearchForCurly(List<string> lines, int lineIndex)
    {
        if (lines[lineIndex].Contains('{'))
        {
            SetErrorIf(lines[lineIndex][0] == '{', lineIndex, "Can not have a scope that isent used with a method declaration");
            return true;
        }
        else if (lines.Count > lineIndex + 1)
            if (lines[lineIndex + 1] == "{")
                return true;
        return false;
    }

    /// <summary>
    /// Starts at a if statement and ends at an end statement
    /// </summary>
    /// <param name="lines"></param>
    /// <param name="lineNum"></param>
    private int IfStatement(List<string> lines, int lineNum)
    {
        int scopeLength = 0;
        int scopeDepth = 0; // Indicates how many { and }we have gone pase
        bool ended = false;
        for (int i = lineNum; i < lines.Count; i++)
        {
            if (lines[i].Contains('{'))
                scopeDepth++;
            else if (lines[i].Contains('}'))
            {
                scopeDepth--;
                if (scopeDepth == 0)
                {
                    ended = true;
                    break;
                }
            }
            scopeLength++;
        }
        SetErrorIf(!ended, lineNum, "The if statement does not have an end");

        string statement = string.Join("", lines[lineNum].TakeLast(lines[lineNum].Length - 2));
        bool statementIsTrue = false;

        // Operations
        bool equalTo = false; // ==
        bool notEqualTo = false; // !=
        bool lessThan = false; // <
        bool moreThan = false; // >
        int operations = 0;

        // Check if the statement is true
        if (statement.Contains("=="))
        {
            equalTo = true;
            operations++;
        }
        if (statement.Contains("!="))
        {
            notEqualTo = true;
            operations++;
        }
        if (statement.Contains("<"))
        {
            lessThan = true;
            operations++;
        }
        if (statement.Contains(">"))
        {
            moreThan = true;
            operations++;
        }

        SetErrorIf(operations > 1, lineNum, "Can not have more than 1 operator");
        SetErrorIf(operations < 1, lineNum, "Can not have less than 1 operator");

        if (equalTo)
        {
            int[] values = GetValuesFromParameters(statement.Replace("==", ","), lineNum);
            SetErrorIf(values.Length != 2, lineNum, "Argument error");
            statementIsTrue = values[0] == values[1];
        }
        else if (notEqualTo)
        {
            int[] values = GetValuesFromParameters(statement.Replace("!=", ","), lineNum);
            SetErrorIf(values.Length != 2, lineNum, "Argument error");
            statementIsTrue = values[0] != values[1];
        }
        else if (lessThan)
        {
            int[] values = GetValuesFromParameters(statement.Replace("<", ","), lineNum);
            SetErrorIf(values.Length != 2, lineNum, "Argument error");
            statementIsTrue = values[0] < values[1];
        }
        else if (moreThan)
        {
            int[] values = GetValuesFromParameters(statement.Replace(">", ","), lineNum);
            SetErrorIf(values.Length != 2, lineNum, "Argument error");
            statementIsTrue = values[0] > values[1];
        }

        if (statementIsTrue)
        {
            var newLines = lines.Take(new Range(lineNum + 2, lineNum + scopeLength)).ToList();
            RunLinesInScope(newLines, lineNum + 1, true);
        }

        return scopeLength;
    }

    /// <summary>
    /// Runs the formatet code line by line. Returns false if it fails
    /// </summary>
    private int RunLinesInScope(List<string> lines, int linesOffSet, bool scoped)
    {
        // UnDefine this at end of function if it is scoped
        //List<string> NamesOfDefinedFunctions = new List<string>();

        for (int i = 0; i < lines.Count; i++)
        {
            if (lines[i].Length == 0)
                continue;
            if (!running)
                return 0;
            try
            {
                // Testing

                if (DoesStringContain(lines[i], "if", 0))
                    i += IfStatement(lines, i);
                else if (string.Join("", lines[i].Take(6)) == "return")
                    return GetValuesFromParameters(string.Join("", lines[i].TakeLast(lines[i].Length - 6)), i)[0];
                else if (SearchForCurly(lines, i))
                    i += DefFunction(lines, i, linesOffSet);
                else if (string.Join("", lines[i].Take(3)) == "def")
                    DefVarible(lines, i);
                else if (string.Join("", lines[i].Take(5)) == "undef")
                    UnDefVarible(lines, i);
                //else if (IsFunctionCall)
                //{
                //if (!RunLines(NameFunc[FuncName].List, NameFunc[FuncName].OffSet))
                // return false;
                //}
                else
                    RunFunction(lines[i], i + linesOffSet, false);
            }
            catch (Exception e)
            {
                string[] messageSplit = e.Message.Split(':');
                // Prints the code woth an error if there is given a line index in the message
                if (int.TryParse(messageSplit[0], out int var) && messageSplit.Length == 2)
                    PrintError(lines, messageSplit[1], var);
                else
                    throw;
                return int.MinValue;
            }
        }

        //foreach (var var in NameFunc)
        //{
        //    DEBUG.Print("Name " + var.Key.Name + " : Parameter count " + var.Key.Parameters + " : Returns " + var.Key.Returns);
        //    DEBUG.Print(var.Value.Func);
        //}

        return int.MinValue;
    }

    private void DefVarible(List<string> lines, int lineOffSet)
    {
        string line = lines[lineOffSet];
        SetErrorIf(line[3] != '(', lineOffSet, "Incorrect syntax, must use \"def(name)\"");
        string name = string.Join("", line.Take(new Range(4, line.Length - 1)));
        SetErrorIf(line[3] != '(', lineOffSet, "Incorrect syntax, must use \"def(name)\"");

        ValidateName(name, lineOffSet);
        SetErrorIf(NameVaribleIndex.ContainsKey(name), lineOffSet, "Varible has already been defined ");

        Varibles.Add(0);
        NameVaribleIndex.Add(name, Varibles.Count - 1);
    }

    private void UnDefVarible(List<string> lines, int lineOffSet)
    {
        string line = lines[lineOffSet];
        SetErrorIf(line[5] != '(', lineOffSet, "Incorrect syntax, must use \"undef(name)\"");
        string name = string.Join("", line.Take(new Range(6, line.Length - 1)));
        SetErrorIf(line[5] != '(', lineOffSet, "Incorrect syntax, must use \"undef(name)\"");

        ValidateName(name, lineOffSet);
        //SetErrorIf(!NameVaribleIndex.ContainsKey(name), lineOffSet, "Varible has not been defined");

        if (NameVaribleIndex.ContainsKey(name))
        {
            NameVaribleIndex.Remove(name, out int val);
            Varibles[val] = 0;
        }
    }

    private int RunFunction(string line, int lineOffSet, bool excpectReturn)
    {
        // Test
        string[] testStr = line.Split('(', 2);
        if (!line.Contains('(') || !line.Contains(')') || testStr[1][testStr[1].Length - 1] != ')')
            Console.WriteLine();

        SetErrorIf(!line.Contains('('), lineOffSet, "Incorrect parameters");
        SetErrorIf(!line.Contains(')'), lineOffSet, "Incorrect parameters");
        string[] str = line.Split('(', 2);
        SetErrorIf(str[1][str[1].Length - 1] != ')', lineOffSet, "Incorrect parameters");
        str[1] = str[1].Remove(str[1].Length - 1);

        string FuncName = str[0];

        if (FuncName == "Set")
        {
            SetErrorIf(excpectReturn, lineOffSet, "The Set() Function can not return a value");
            string[] parameters = GetParameterNames(line, lineOffSet, false);
            SetErrorIf(parameters.Length != 2, lineOffSet, "Incorrect parameters, example \"Set(name, 100)\"");
            string varName = parameters[0];
            if (int.TryParse(parameters[1], out int outInt))
            {
                SetErrorIf(!NameVaribleIndex.ContainsKey(varName), lineOffSet, $"The varible {varName} does not exist");
                Varibles[NameVaribleIndex[varName]] = outInt;
            }
            else
            {
                Varibles[NameVaribleIndex[varName]] = RunFunction(parameters[1], lineOffSet, true);
                //SetErrorIf(!NameVaribleIndex.ContainsKey(varName), lineOffSet, $"The varible {varName} does not exist");
                //Varibles[NameVaribleIndex[varName]] = GetValuesFromParameters(parameters[1], lineOffSet)[0];
            }
            //else
            //    SetError(lineOffSet, "Invalid number in second parameter");
            return int.MinValue;
        }
        else if (FuncName == "Get")
        {
            SetErrorIf(!excpectReturn, lineOffSet, "The Get() Function need to return a value");
            string[] parameters = GetParameterNames(line, lineOffSet, false);
            SetErrorIf(parameters.Length != 1, lineOffSet, "Incorrect parameters, example \"Get(name)\"");
            string varName = parameters[0];
            SetErrorIf(!NameVaribleIndex.ContainsKey(varName), lineOffSet, $"The varible {varName} does not exist");
            return Varibles[NameVaribleIndex[varName]];
        }
        else if (FuncName == "PrintText")
        {
            SetErrorIf(excpectReturn, lineOffSet, "The PrintText() Function can not return a value");
            string[] parameters = GetParameterNames(line, lineOffSet, false, false);
            SetErrorIf(parameters.Length != 1, lineOffSet, "Incorrect parameters, example PrintText(\"Hello world\")");
            SetErrorIf(parameters[0].Length < 2, lineOffSet, "Incorrect parameters, example PrintText(\"Hello world\")");
            SetErrorIf(parameters[0][0] != '\"', lineOffSet, "Incorrect parameters, example PrintText(\"Hello world\")");
            SetErrorIf(parameters[0][parameters[0].Length - 1] != '\"', lineOffSet, "Incorrect parameters, example PrintText(\"Hello world\")");
            Console.WriteLine(string.Join("", parameters[0].Take(new Range(1, parameters[0].Length - 1)))); ;
            return 0;
        }


        int[] Parameters = GetValuesFromParameters(str[1], lineOffSet);

        int var1 = Parameters.Length > 0 ? Parameters[0] : 0;
        int var2 = Parameters.Length > 1 ? Parameters[1] : 0;

        (string Name, int Parameters, bool Returns) key = (FuncName, Parameters.Length, excpectReturn);

        int value = 0;

        if (BuildIn.ContainsKey(key))
        {
            try
            {
                return BuildIn[key].Invoke(var1, var2);
            }
            catch (InvalidOperationException e)
            {
                //SetErrorIf(e.Message == "Stack empty.", lineOffSet, "StackUnderflow");
                SetError(lineOffSet, "StackUnderflow");
                throw;
            }
        }
        else if (NameFunc.ContainsKey(key))
        {
            string[] tempFunctions = new string[NameFunc[key].Func.Count];
            NameFunc[key].Func.CopyTo(tempFunctions);
            List<string> function = new List<string>(tempFunctions);
            string[] parameterNames = GetParameterNames(function[0], lineOffSet);
            string[] oldParameterNames = new string[parameterNames.Length];
            for (int i = 0; i < parameterNames.Length; i++)
                oldParameterNames[i] = parameterNames[i];
            for (int i = 0; i < parameterNames.Length; i++)
                while (NameVaribleIndex.ContainsKey(parameterNames[i]))
                    parameterNames[i] = parameterNames[i] + "a";
            for (int i = 0; i < function.Count; i++)
                for (int j = 0; j < parameterNames.Length; j++)
                    function[i] = function[i].Replace("(" + oldParameterNames[j] + ")", "(" + parameterNames[j] + ")");

            function.RemoveAt(0);
            // Define the varibles so they can be used by the function
            if (Parameters.Length == 2)
            {
                function.Insert(0, $"Set({parameterNames[1]},{var2})");
                function.Insert(0, $"Set({parameterNames[0]},{var1})");
                function.Insert(0, $"def({parameterNames[1]})");
                function.Insert(0, $"def({parameterNames[0]})");

                function.Add($"undef({parameterNames[1]})");
                function.Add($"undef({parameterNames[0]})");
                return RunLinesInScope(function, NameFunc[key].LineOffSet - 3, true);
            }
            else if (Parameters.Length == 1)
            {
                function.Insert(0, $"Set({parameterNames[0]},{var1})");
                function.Insert(0, $"def({parameterNames[0]})");

                function.Add($"undef({parameterNames[0]})");
                return RunLinesInScope(function, NameFunc[key].LineOffSet - 1, true);
            }
            else
            {
                return RunLinesInScope(function, NameFunc[key].LineOffSet + 1, true);
            }
        }
        else if (excpectReturn)
            SetError(lineOffSet, "The function \"" + FuncName + "\" could not be found with a return varible and with " + Parameters.Length + " Parameters");
        else
            SetError(lineOffSet, "The function \"" + FuncName + "\" could not be found without a return varible and with " + Parameters.Length + " Parameters");
        return value;
    }

    /// <summary>
    /// Uses the lines the define a function. Uses LineOffSet as the first line aka where the name is,
    /// and to tell where the function should start, and runs util a line contains }
    /// If an error acurs it will throw an error with the lineOffSet in the message, return the length of the function
    /// </summary>
    private int DefFunction(List<string> lines, int lineOffSet, int scopeOffSet)
    {
        string Name = lines[lineOffSet].Split('(')[0];
        ValidateName(Name, lineOffSet + scopeOffSet);
        // Check if there is lesss than 3 parameters [0,2]
        string[] str = GetParameterNames(lines[lineOffSet], lineOffSet + scopeOffSet);
        SetErrorIf(BuildIn.ContainsKey((Name, str.Length, false)), lineOffSet + scopeOffSet, "Function allready defined");
        SetErrorIf(NameFunc.ContainsKey((Name, str.Length, false)), lineOffSet + scopeOffSet, "Function allready defined");
        List<string> FuncLines = new List<string>();
        int LinesCount = 0;

        // Indicates how many brackets its been through
        /*
         * For example
         * hello()
         * {
         * fef()
         * {
         * }
         * push(1) 
         * }		string.Contains returned	false	bool

         * 
         */
        int depth = 0;
        bool foundReturn = false;
        //if (lines[lineOffSet].Contains('{'))
        //    depth++;

        for (int i = lineOffSet/* + 1*/; i < lines.Count; i++)
        {
            LinesCount++;

            if (lines[i].Contains('{'))
            {
                if (depth == 0)
                    FuncLines.Add(lines[i].Replace("{", ""));
                else
                    FuncLines.Add(lines[i]);
                //if (depth == 0)
                //    FuncLines.Add(lines[i].Replace("{", "").Replace("}", ""));
                depth++;
            }
            else if (lines[i].Contains('}'))
            {
                depth--;
                if (depth == 0)
                    FuncLines.Add(lines[i].Replace("}", ""));
                else
                    FuncLines.Add(lines[i]);
                if (depth == 0)
                {
                    // End
                    NameFunc.Add((Name, str.Length, foundReturn), (FuncLines, lineOffSet + scopeOffSet));
                    return FuncLines.Count - 1;
                }
            }
            else if (DoesStringContain(lines[i], "return", 0))
            {
                FuncLines.Add(lines[i]);
                if (depth == 1)
                    foundReturn = true;
            }
            else
            {
                SetErrorIf(foundReturn && lines[i] != "", lineOffSet + scopeOffSet + i, "Can not have code that runs after a return statement");
                FuncLines.Add(lines[i]);
            }
        }

        SetError(lineOffSet, "Function does not have an end");
        throw new NotImplementedException();
    }

    /// <summary>
    /// Converts the code from human code to code that the interpreter can go line by line through.
    /// If the program fails, the entire memory will be cleared to use as a blank slaid
    /// </summary>
    public void RunCode(string code)
    {
        if (running)
            return;
        running = true;
        LinesInput.AddRange(GetLines(code));
        code = RemoveEmpty(code);
        List<string> Lines = GetLines(code);
        //Lines = RemoveEmptyLines(Lines); // Dont remove beacous then you cant show errors on the correct line
        Lines = RemoveComments(Lines);

        //DEBUG.Print(Lines);

        RunLinesInScope(Lines, 0, false);

        //DEBUG.Print("Stack");
        //while (Stack.Count > 0)
        //DEBUG.Print(Stack.Pop() + "");
    }

    public void Stop()
    {
        running = false;
    }

    public void Clear()
    {
        Stop();
        NameFunc.Clear();
        NameVaribleIndex.Clear();
        Varibles.Clear();
        Stack.Clear();
        LinesInput.Clear();
    }
}
