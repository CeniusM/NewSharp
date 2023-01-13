# NewSharp
This is a simple stack-based interpreter that is capable of performing basic arithmetic operations such as addition and subtraction, as well as handling variables, functions and conditional statements.

The NS scripting languages contains the following tools

- Push(value): pushes the value of x onto the stack
- Plus():pops the last two values from the stack, adds them together, and pushes the result onto the stack
- Mul(): multiplies the two top stack numbers
- Div(): divide the two top stack numbers
- Minus(): pops the last two values from the stack, subtracts the second popped value from the first, and pushes the result onto the stack
- Print(x): prints an argument
- StackCount(): Returns the Stack elements count
- def(varName); Used to define a varible that can be used later (init to 0)
- Set(varName, value): sets the value of the variable
- Get(varName): returns the value of the variable
- undef(varName): undefines the varible 

The interpreter also has the ability to skip the rest of a line at ("//", "#" and ";"), which are treated as comments.

You can create a function using the underlying syntax, it is all on a line to line basis

Note* All functions can have 0-2 parameters with function overloading

```
AddOne(var)
{
    // Example
    Push(Get(var))
    Push(1)
    Plus()
}
```

now if it contains a return statement at the end, it can return a value that other Functions can use
if it returns a value, it has to be used as a parameter

example
```
PlusVarWithOne(var)
{
    Push(Get(var))   
    Push(1)
    Plus()
    return Pop()
}
```

You are also able to use if statements
example

```
if (Pop() == 4)
Print(4)
endif
```

endif is used to indicate where to stop the scope

To run the interpreter, simply run the script with a full path in source code because i don't know how to do it any other way :D



## Todo
- [ ] Implement return
- [ ] Implement if statements
- [ ] Implement Read line to integer
- [ ] Implement NewLine() and Write(num)
- [ ] Implement undefine functions and varibles at end of scope
- [ ] Other than that there is no mistakes *cough
