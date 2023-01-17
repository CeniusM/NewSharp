# NewSharp
The NS scripting languages contains the following tools

- Plus(var1, var2): returns var1 + var2
- Mul(var1, var2): returns var1 * var2
- Minus(var1, var2): returns var1 - var2
- Div(var1, var2): returns var1 / var2
- Print(var1): prints the input number
- PrintLine(var1): prints the input number and a new line
- PrintText("This text will be printed"): prints the text you put in the "", does not create a new line
- PrintTextLine("This text will be printed"): prints the text you put in the "", and creates a new line
- NewLine(): prints new line
- Clear(): clears the terminal
- Sleep(x): sleeps for x amount of milliseconds
- def(varName): defines a varible
- Set(varName, value): sets a varible
- varnName = value: sets a varible
- Get(varName): returns a varible value
- varName: returns a varible value 
- undef(varName): undefines the varible
- StartTimer(): starts/restarts an internal timer
- StopTimer(): returns the time since start and stops the watch, if the timer was not started, it returns -1
- Rand(low, heigh): random number [low, heigh] 
- ReadKey(): returns a client key input
- Array1(var1): returns the value of a global array with var1 as index, the array is a MB with of bytes, Length of 262144 
- Array1(var1, var2): sets the value of the global array with var1 as index and var2 as the value, the array is a MB with of bytes, Length of 262144 
- SetCursor(var1, var2): set cursor at given coord (var1:x, var2:y)
- MoveCursor(var1, var2): move the cursor (var1:x, var2:y) from original coord
- Abs(var1): returns Math.Abs(var1)
- Max(var1, var2): returns Math.Max(var1, var2)
- Min(var1, var2): returns Math.Min(var1, var2)
- Sqrt(var1): returns Math.Sqrt(var1)
- Pow(var1, var2): returns Math.Pow(var1, var2)
- Scale(var1, var2): returns Math.Scale(var1, var2)


The interpreter also has the ability to skip the rest of a line at ("//", "#" and ";"), which are treated as comments.

You can create a function using the underlying syntax, it is all on a line to line basis

Note* All functions can have [0,2] parameters with function overloading

```
AddOne(var)
{
    // Example
    var = Plus(var, 1)
    return
}
```

Now if it contains a return statement at the end, it can return a value that other Functions can use
If it returns a value, it has to be used as a parameter

example
```
PlusVarWithOne(var)
{
    return Plus(var, 1)
}
```

You are also able to use if statements
example

```
def(i)
i = 4
if i == 4
{
PrintText("The given number is ")
Print(i)
NewLine()
}
```

To run the interpreter, simply run the script with a full path in source code because i don't know how to do it any other way :D



## Todo
- [ ] Implement undefine functions and varibles at end of scope
- [ ] Other than that there is no mistakes *cough