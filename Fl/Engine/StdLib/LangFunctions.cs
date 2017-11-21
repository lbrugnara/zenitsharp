// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Fl.Engine.Evaluators;
using Fl.Engine.Symbols;
using Fl.Engine.Symbols.Types;
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
                var flString = a as FlString;
                Lexer l = new Lexer(System.IO.File.ReadAllText(flString.Value));
                List<Token> tokens = new List<Token>();
                Token t = null;
                while ((t = l.NextToken()) != null)
                {
                    tokens.Add(t);
                }

                Parser.Parser p = new Fl.Parser.Parser();
                Parser.Ast.AstNode ast = p.Parse(tokens);
                AstEvaluator ev = new AstEvaluator();
                FlNamespace os = (ev.Symtable.GetSymbol("os").Binding as FlNamespace);
                (os["cwd"].Binding as FlCallable).Invoke(ev, new List<FlObject>() { new FlString(System.IO.Directory.GetParent(flString.Value).FullName) });
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
                if (e.ObjectType != NamespaceType.Value)
                    throw new AstWalkerException($"{e} is not a namespace");
                evaluator.Symtable.Using((e as FlNamespace));
            });
            return null;
        }
    }
}
