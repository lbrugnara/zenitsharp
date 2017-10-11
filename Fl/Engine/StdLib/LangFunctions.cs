// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using System;
using System.Collections.Generic;
using System.Text;

namespace Fl.Engine.StdLib
{
    public class ImportFunction : FlCallable
    {
        public override string Name => "import";

        public override ScopeEntry Invoke(AstEvaluator evaluator, List<ScopeEntry> args)
        {
            args.ForEach(a => {
                Lexer l = new Lexer(System.IO.File.ReadAllText(a.StrValue));
                List<Token> tokens = new List<Token>();
                Token t = null;
                while ((t = l.NextToken()) != null)
                {
                    tokens.Add(t);
                }

                Parser.Parser p = new Fl.Parser.Parser();
                Parser.Ast.AstNode ast = p.Parse(tokens);
                evaluator.Process(ast);
            });
            return null;
        }
    }
}
