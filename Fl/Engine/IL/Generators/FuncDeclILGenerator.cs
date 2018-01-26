// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Fl.Engine.IL.Instructions;
using Fl.Engine.IL.Instructions.Operands;
using Fl.Engine.IL.VM;
using Fl.Parser.Ast;

namespace Fl.Engine.IL.Generators
{
    class FuncDeclILGenerator : INodeVisitor<ILGenerator, AstFuncDeclNode, Operand>
    {
        public Operand Visit(ILGenerator generator, AstFuncDeclNode funcdecl)
        {
            generator.PushFragment(funcdecl.Identifier.Value.ToString(), FragmentType.Function);
            funcdecl.Parameters.Parameters.ForEach(p => generator.Emmit(new LocalInstruction(generator.SymbolTable.NewSymbol(p.Value.ToString()))));
            funcdecl.Body.ForEach(s => s.Exec(generator));
            generator.PopFragment();
            return null;
        }
    }
}
