//#define RunIDEThingy

using NewSharp;

#if RunIDEThingy
NewSharpIDE ide = new NewSharpIDE();
ide.Start("C:\\Users\\ceniu\\source\\repos\\NewSharp\\NewSharp\\MyProgram.NS");

#else

Interpreter programRunner = new Interpreter();

string code = "";
try
{
    code = string.Join("\n", File.ReadAllLines("C:\\Users\\ceniu\\source\\repos\\NewSharp\\NewSharp\\MyProgram.NS"));
}
catch (Exception)
{
    Console.WriteLine("I use full path sorry, im not THAT good at programing");
    return;
    //throw;
}

programRunner.RunCode(code);

//Console.ReadLine();

#endif