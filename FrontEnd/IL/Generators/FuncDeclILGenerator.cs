// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Zenit.IL.Instructions;
using Zenit.IL.Instructions.Operands;
using Zenit.IL.VM;
using Zenit.Engine.Symbols.Types;
using Zenit.Ast;
using System.Linq;

namespace Zenit.IL.Generators
{
    class FuncDeclILGenerator : INodeVisitor<ILGenerator, FunctionNode, Operand>
    {
        public Operand Visit(ILGenerator generator, FunctionNode funcdecl)
        {
            generator.PushFragment(funcdecl.Name, FragmentType.Function);
            funcdecl.Parameters.ForEach(p => generator.Emmit(new LocalInstruction(generator.SymbolTable.NewSymbol(p.Name.Value, OperandType.Auto))));
            funcdecl.Body.ForEach(s => s.Visit(generator));
            if (!funcdecl.Body.Any(n => n is ReturnNode))
                generator.Emmit(new ReturnInstruction());
            generator.PopFragment();
            return null;
        }
    }
}
