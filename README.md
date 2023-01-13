# NewSharp
This is a simple stack-based interpreter that is capable of performing basic arithmetic operations such as addition and subtraction, as well as handling variables.

The interpreter takes in a file called "prog.NS" as input, which contains a list of operations to be executed. The file can contain the following operations:

Push(x): pushes the value of x onto the stack
Plus():pops the last two values from the stack, adds them together, and pushes the result onto the stack
Mul(): multiplies the two top stack numbers
Div(): divide the two top stack numbers
Minus():pops the last two values from the stack, subtracts the second popped value from the first, and pushes the result onto the stack
// Dump():pops the last value from the stack and prints it
Print(x): prints an argument
StackCount(): Returns the Stack elements count
def(var); Used to define a varible that can be used later (init to 0)
Set(var, value): sets the value of the variable
Get(var): returns the value of the variable
undef(var): undefines the varible 
The interpreter also has the ability to skip the rest of a line at "//", which are treated as comments.

The function simulate_program(program) takes in the program as input and executes the operations. The function compileProgram(program) is not implemented in this version.

You can create a function using the underlying syntax
add_one(varName)
{
    // Example
    Push(Get(varName))
    Push(1)
    Plus()
}
now if it contains a return statement, it can return a value that other Functions can use
example
get_top_plus_var(var1)
{
    Push(var1)
    Plus()
    return Pop()
}

You are also able to use if statements
example
if (Pop() == 4)


endif
end if is used to indicate where to stop the scope

To run the interpreter, simply run the script with a properly formatted "prog.nl" file in the same directory.



## Todo
- [ ] Implement return
- [ ] Implement if statements
- [ ] Implement Read line to integer
- [ ] Implement NewLine() and Write(num)
- [ ] Implement undefine functions and varibles at end of scope
- [ ] Other than that there is no mistakes *cough