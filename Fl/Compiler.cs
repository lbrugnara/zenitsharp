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
            var syntacticAnalysis = new SyntacticAnalysis();
            var ast = syntacticAnalysis.Run(source);

            var semanticAnalysis = new SemanticAnalysis();
            semanticAnalysis.Run(ast);

            // Generate IL program
            var ilGenerator = new ILGenerator();
            return ilGenerator.Build(ast);
        }
    }
}
