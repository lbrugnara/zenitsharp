// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Fl.IL.Instructions;
using Fl.IL.Instructions.Operands;
using Fl.IL.VM;
using Fl.Engine.Symbols.Types;
using Fl.Parser.Ast;
using System.Linq;

namespace Fl.IL.Generators
{
    class FuncDeclILGenerator : INodeVisitor<ILGenerator, AstFuncDeclNode, Operand>
    {
        public Operand Visit(ILGenerator generator, AstFuncDeclNode funcdecl)
        {
            generator.PushFragment(funcdecl.Identifier.Value.ToString(), FragmentType.Function);
            funcdecl.Parameters.Parameters.ForEach(p => generator.Emmit(new LocalInstruction(generator.SymbolTable.NewSymbol(p.Value.ToString(), OperandType.Auto))));
            funcdecl.Body.ForEach(s => s.Visit(generator));
            if (!funcdecl.Body.Any(n => n is AstReturnNode))
                generator.Emmit(new ReturnInstruction());
            generator.PopFragment();
            return null;
        }
    }
}
