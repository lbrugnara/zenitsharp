// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Fl.Syntax;
using Fl.Semantics;

namespace Fl
{
    public class Compiler
    {
        public void Compile(string source)
        {
            var syntacticAnalysis = new SyntacticAnalysis();
            var ast = syntacticAnalysis.Run(source);

            var semanticAnalysis = new SemanticAnalysis();
            semanticAnalysis.Run(ast);

            // Generate IL program
            /*var ilGenerator = new ILGenerator();
            return ilGenerator.Build(ast);*/
        }
    }
}
