using Fl.IL;
using Fl.IL.VM;
using Fl.Parser;
using Fl.Symbols;
using Fl.TypeChecking;
using System;

namespace Fl
{
    public class Compiler
    {
        public ILProgram Compile(string source)
        {
            var lexer = new Lexer(source);
            var parser = new Parser.Parser();
            var symbolResolver = new SymbolResolver();
            var ilGenerator = new ILGenerator();

            // Lexical analysis and Parsing 
            var ast = parser.Parse(lexer.Tokenize());


            var errors = parser.ParsingErrors;

            if (errors.Count > 0)
                throw new Exception(string.Join("\n", errors));

            // Resolve symbols
            var st = symbolResolver.Resolve(ast);

            // Type checking
            var tc = new TypeChecker(st);
            tc.Check(ast);

            // Generate IL program
            return ilGenerator.Build(ast);
        }
    }
}
