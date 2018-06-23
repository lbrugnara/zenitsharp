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
            var ast = this.parser.Parse(this.lexer.Tokenize(source));

            var errors = parser.ParsingErrors;

            if (errors.Count > 0)
                throw new Exception(string.Join("\n", errors));

            return ast;
        }
    }
}
