// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using System;
using Zenit;

namespace FlInterpreter
{
    class Interpreter
    {

        public void Run()
        {
            while (true)
            {
                try
                {
                    string line = "";
                    while (true)
                    {
                        Console.Write(line == "" ? ">>> " : "  | ");
                        string tmp = Console.ReadLine();

                        if (string.IsNullOrEmpty(tmp))
                        {
                            Console.WriteLine(" <- ");
                            break;
                        }

                        line += tmp;
                    }

                    var compiler = new Compiler();
                    compiler.Compile(line);

                    System.Diagnostics.Trace.WriteLine($"Source: {line}");

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