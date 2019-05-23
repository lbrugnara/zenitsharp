using Zenit.Ast;
using System;
using System.Linq;

namespace Zenit.Syntax
{
    public class SyntacticAnalysis
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

            if (parser.ParsingErrors.Any())
                throw new AggregateException(parser.ParsingErrors);

            return ast;
        }
    }
}
