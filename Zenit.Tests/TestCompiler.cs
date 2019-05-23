using System.Diagnostics;
using System.Text;
using Zenit.Semantics;
using Zenit.Semantics.Symbols;
using Zenit.Syntax;

namespace Zenit.FrontEnd
{
    class TestCompiler : IZenitCompiler
    {
        private SyntacticAnalysis syntacticAnalysis;
        private SemanticAnalysis semanticAnalysis;

        public TestCompiler()
        {
            this.syntacticAnalysis = new SyntacticAnalysis();
            this.semanticAnalysis = new SemanticAnalysis();
        }

        public SymbolTable SymbolTable => this.semanticAnalysis.SymbolTable;        

        public void Compile(string source)
        {
            var ast = syntacticAnalysis.Run(source);
            this.semanticAnalysis.Run(ast);
        }

        public void ResolveSymbols(string source)
        {
            var ast = syntacticAnalysis.Run(source);
            this.semanticAnalysis.ResolveSymbols(ast);
        }
    }
}
