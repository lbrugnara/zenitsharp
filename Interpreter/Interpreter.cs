// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Fl;
using Fl.Engine;
using Fl.Engine.Evaluators;
using Fl.Engine.Symbols;
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
                    Symbol result = eval.Process(ast);
                    if (result != null)
                        Console.WriteLine($":: {result.ToDebugStr()}");
                }
                catch (Exception e)
                {
                    string type = (e is AstWalkerException) ? "Runtime" : (e is ParsingException) ? "Parsing" : "Unknown";
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