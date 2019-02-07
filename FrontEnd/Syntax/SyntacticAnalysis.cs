using Zenit.Ast;
using System;

namespace Zenit.Syntax
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

        public Node Run(string source)
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
