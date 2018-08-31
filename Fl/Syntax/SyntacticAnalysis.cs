using Fl.Ast;
using System;
using System.Collections.Generic;
using System.Text;

namespace Fl.Syntax
{
    class SyntacticAnalysis
    {
        private Lexer lexer;
        private Parser parser;

        public SyntacticAnalysis()
        {
            this.lexer = new Lexer();
            this.parser = new Parser();
        }

        public AstNode Run(string source)
        {
            var tokens = this.lexer.Tokenize(source);

            // tokens.ForEach(t => System.Diagnostics.Trace.WriteLine($"{t.Type}('{t.Value}') {t.Line}:{t.Col}"));

            var ast = this.parser.Parse(tokens);

            var errors = parser.ParsingErrors;

            if (errors.Count > 0)
                throw new Exception(string.Join("\n", errors));

            return ast;
        }
    }
}
