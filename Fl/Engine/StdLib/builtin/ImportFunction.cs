// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Fl.Engine.Evaluators;
using Fl.Engine.Symbols;
using Fl.Engine.Symbols.Objects;
using System.Collections.Generic;

namespace Fl.Engine.StdLib.builtin
{
    public class ImportFunction : FlFunction
    {
        public override string Name => "import";

        public override FlObject Invoke(SymbolTable symboltable, List<FlObject> args)
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
                (os["cwd"].Binding as FlFunction).Invoke(symboltable, new List<FlObject>() { new FlString(System.IO.Directory.GetParent(flString.Value).FullName) });
                ev.Process(ast);
                symboltable.Import(ev.Symtable.GlobalScope);
            });
            return null;
        }
    }
}
