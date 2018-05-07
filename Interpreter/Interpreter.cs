// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Fl.IL;
using Fl.IL.VM;
using Fl.IL.Instructions;
using Fl.Engine.Symbols.Exceptions;
using Fl.Engine.Symbols.Objects;
using Fl.Parser;
using Fl.Parser.Ast;
using System;
using System.Collections.Generic;
using System.Linq;
using Fl.Symbols;
using Fl.TypeChecker;

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
                    Lexer l = new Lexer(line);
                    List<Token> tokens = new List<Token>();
                    Token t = null;
                    while ((t = l.NextToken()) != null)
                    {
                        tokens.Add(t);
                    }

                    Parser p = new Parser();
                    AstNode ast = p.Parse(tokens);

                    var errors = p.ParsingErrors;
                    if (errors.Count > 0)
                    {
                        var list = errors.ToList();
                        var tmp = Console.ForegroundColor;
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("Errors:");
                        list.ForEach(e => Console.WriteLine($"{e.Message}"));
                        Console.ForegroundColor = tmp;
                        continue;
                    }

                    SymbolResolver sr = new SymbolResolver();

                    SymbolTable st = sr.Resolve(ast);

                    TypeChecker tc = new TypeChecker(st);
                    tc.Visit(ast);
                    
                    var g = new ILGenerator();

                    ILProgram ip = g.Build(ast);
                    Console.WriteLine(ip.ToString());
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
                    switch (e)
                    {
                        case AstWalkerException awe:
                        case SymbolException se:
                        case ScopeOperationException soe:
                            type = "Runtime";
                            break;
                        case ParserException pe:
                            type = "Parsing";
                            break;                        
                    }
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