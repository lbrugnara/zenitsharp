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

        public override FlObject Invoke(AstEvaluator evaluator, List<FlObject> args)
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
                ev.Symtable.GetSymbol("os").Binding.AsNamespace["cwd"].Binding.AsCallable.Invoke(ev, new List<FlObject>() { new FlObject(ObjectType.String, System.IO.Directory.GetParent(a.AsString)) });
                ev.Process(ast);
                evaluator.Symtable.Import(ev.Symtable.GlobalScope);
            });
            return null;
        }
    }

    public class UsingFunction : FlCallable
    {
        public override string Name => "using";

        public override FlObject Invoke(AstEvaluator evaluator, List<FlObject> args)
        {
            args.ForEach(e => {
                if (!e.IsNamespace)
                    throw new AstWalkerException($"{e.AsString} is not a namespace");
                evaluator.Symtable.Using(e.AsNamespace);
            });
            return null;
        }
    }
}
