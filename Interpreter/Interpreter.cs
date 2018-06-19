// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using System;
using Fl;

namespace FlInterpreter
{
    class Interpreter
    {

        public void Run()
        {
            while (true)
            {
                Console.Write(">>> ");
                try
                {
                    string line = Console.ReadLine();

                    var compiler = new Compiler();
                    var ilProgram = compiler.Compile(line);

                    System.Diagnostics.Trace.WriteLine($"Source: {line}");

                    if (ilProgram == null)
                        continue;

                    Console.WriteLine(ilProgram.ToString());

                    /*ip.Run();

                    AssignInstruction instr = ip.Fragments[".global"].Instructions.LastOrDefault() as AssignInstruction;
                    if (instr == null || instr.Destination == null || !ip.SymbolTable.HasSymbol(instr.Destination.ToString()))
                        continue;

                    FlObject result = ip.SymbolTable.GetSymbol(instr.Destination).Binding;
                    if (result == null)
                        continue;

                    Console.WriteLine($"<= {result.ToDebugStr()}");*/
                }
                catch (Exception e)
                {
                    string type = "Unknown";
                    var tmp = Console.ForegroundColor;
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine($"{type} Error: {e.Message}");
                    if (type == "Unknown")
                        Console.WriteLine(e.StackTrace);
                    Console.ForegroundColor = tmp;
                }
            }
        }
    }
}