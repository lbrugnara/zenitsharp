// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Fl.IL;
using Fl.IL.VM;
using Fl.Syntax;
using Fl.Semantics;
using System;

namespace Fl
{
    public class Compiler
    {
        public ILProgram Compile(string source)
        {
            var lexer = new Lexer(source);
            var parser = new Parser();

            // Lexical analysis and Parsing 
            var ast = parser.Parse(lexer.Tokenize());

            var errors = parser.ParsingErrors;

            if (errors.Count > 0)
                throw new Exception(string.Join("\n", errors));

            // Resolve symbols
            var symbolResolver = new SymbolBinder();
            symbolResolver.Resolve(ast);

            // Type checking
            var tc = new TypeChecker(symbolResolver.SymbolTable, symbolResolver.TypeInferrer);
            tc.Check(ast);

            // Generate IL program
            var ilGenerator = new ILGenerator();
            return ilGenerator.Build(ast);
        }
    }
}
