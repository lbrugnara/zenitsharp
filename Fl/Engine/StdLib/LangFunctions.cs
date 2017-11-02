// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Fl.Engine.Evaluators;
using Fl.Engine.Symbols;
using Fl.Parser.Ast;
using System;
using System.Collections.Generic;
using System.Text;

namespace Fl.Engine.StdLib
{
    public class ImportFunction : FlCallable
    {
        public override string Name => "import";

        public override Symbol Invoke(AstEvaluator evaluator, List<Symbol> args)
        {
            args.ForEach(a => {
                Lexer l = new Lexer(System.IO.File.ReadAllText(a.AsString));
                List<Token> tokens = new List<Token>();
                Token t = null;
                while ((t = l.NextToken()) != null)
                {
                    tokens.Add(t);
                }

                Parser.Parser p = new Fl.Parser.Parser();
                Parser.Ast.AstNode ast = p.Parse(tokens);
                AstEvaluator ev = new AstEvaluator();
                ev.Process(ast);
                evaluator.CurrentScope.Import(ev.CurrentScope);
            });
            return null;
        }
    }

    public class UsingFunction : FlCallable
    {
        public override string Name => "using";

        public override Symbol Invoke(AstEvaluator evaluator, List<Symbol> args)
        {
            args.ForEach(e => {
                if (!e.IsNamespace)
                    throw new AstWalkerException($"{e.AsString} is not a namespace");
                evaluator.CurrentScope.Using(e.AsNamespace);
            });
            return null;
        }
    }
}
