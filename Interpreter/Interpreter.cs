// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Fl;
using Fl.Engine;
using Fl.Engine.Evaluators;
using Fl.Engine.Symbols;
using Fl.Engine.Symbols.Exceptions;
using Fl.Engine.Symbols.Objects;
using Fl.Parser;
using Fl.Parser.Ast;
using System;
using System.Collections.Generic;

namespace FlInterpreter
{
    class Interpreter
    {

        public void Run()
        {
            AstEvaluator eval = new AstEvaluator();
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
                    FlObject result = eval.Process(ast);
                    if (result != null)
                        Console.WriteLine($"<= {result.ToDebugStr()}");
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