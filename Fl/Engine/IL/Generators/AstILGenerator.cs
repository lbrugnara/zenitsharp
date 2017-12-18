// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Fl.Engine.IL.Instructions;
using Fl.Engine.Symbols;
using Fl.Engine.Symbols.Objects;
using Fl.Parser.Ast;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System;
using Fl.Engine.IL.Instructions.Operands;
using System.Collections.ObjectModel;

namespace Fl.Engine.IL.Generators
{
    public class AstILGenerator : IAstWalker<Operand>
    {
        private SymbolTable _SymbolTable;
        private List<Instruction> _Instructions;
        private UnaryILGenerator _UnaryILGenerator;
        private BinaryILGenerator _BinaryILGenerator;
        private AssignmentILGenerator _AssignmentILGenerator;
        private ConstantILGenerator _ConstantILGenerator;
        private VariableILGenerator _VariableILGenerator;
        private BlockILGenerator _BlockILGenerator;
        private DeclarationILGenerator _DeclarationILGenerator;
        private LiteralILGenerator _LiteralILGenerator;
        private AccessorILGenerator _AccessorILGenerator;
        private IfILGenerator _IfILGenerator;
        private WhileILGenerator _WhileILGenerator;
        private ForILGenerator _ForILGenerator;
        private BreakILGenerator _BreakILGenerator;
        private ContinueILGenerator _ContinueILGenerator;
        private ReturnILGenerator _ReturnILGenerator;
        private CallableILGenerator _CallILGenerator;
        private FuncDeclILGenerator _FuncDeclILGenerator;
        private TupleILGenerator _TupleILGenerator;
        private NullCoalescingILGenerator _NullCoalescingILGenerator;

        private int TemporalVarCounter = 0;
        private int CommonScopeCounter = 0;
        

        public AstILGenerator()
        {
            _Instructions = new List<Instruction>();
            _SymbolTable = SymbolTable.NewInstance;

            _UnaryILGenerator = new UnaryILGenerator();
            _BinaryILGenerator = new BinaryILGenerator();
            _AssignmentILGenerator = new AssignmentILGenerator();
            _ConstantILGenerator = new ConstantILGenerator();
            _VariableILGenerator = new VariableILGenerator();
            _BlockILGenerator = new BlockILGenerator();
            _DeclarationILGenerator = new DeclarationILGenerator();
            _LiteralILGenerator = new LiteralILGenerator();
            _AccessorILGenerator = new AccessorILGenerator();
            _IfILGenerator = new IfILGenerator();
            _WhileILGenerator = new WhileILGenerator();
            _ForILGenerator = new ForILGenerator();
            _BreakILGenerator = new BreakILGenerator();
            _ContinueILGenerator = new ContinueILGenerator();
            _CallILGenerator = new CallableILGenerator();
            _FuncDeclILGenerator = new FuncDeclILGenerator();
            _TupleILGenerator = new TupleILGenerator();
            _ReturnILGenerator = new ReturnILGenerator();
            _NullCoalescingILGenerator = new NullCoalescingILGenerator();
        }

        public SymbolTable SymbolTable => _SymbolTable;

        public ReadOnlyCollection<Instruction> Instructions => new ReadOnlyCollection<Instruction>(_Instructions);

        public SymbolOperand GenerateTemporalName() => new SymbolOperand($"@t{(TemporalVarCounter++)}");

        public string GenerateCommonScopeName() => $"<scope:{(CommonScopeCounter++)}>";

        public Instruction Emmit(Instruction i)
        {
            _Instructions.Add(i);
            return i;
        }

        public string GetILRepresentation()
        {
            return string.Join('\n', Instructions.Select(i => i.ToString()));
        }

        //public Instruction Process(AstNode node)
        //{
        //    /*
        //     * int i=2;
        //     * int j=3;
        //     * int k = i * j
        //     */
        //    ILProgram p = new ILProgram();
        //    // int i=2;
        //    string tmpName = p.EmmitLiteral(new FlInteger(2));
        //    p.EmmitLocalVar("i", tmpName);

        //    // int i=2;
        //    string tmpName2 = p.EmmitLiteral(new FlInteger(3));
        //    p.EmmitLocalVar("j", tmpName2);

        //    // int k = i * j;
        //    string tmpName3 = p.EmmitMult("i", "j");
        //    p.EmmitLocalVar("k", tmpName3);

        //    p.Run();

        //    return null;
        //}

        public Operand Visit(AstNode node)
        {
            object n = node;
            switch (n)
            {
                case AstUnaryNode u:
                    return _UnaryILGenerator.Visit(this, u);                
                case AstBinaryNode b:
                    return _BinaryILGenerator.Visit(this, b);
                case AstAssignmentNode a:
                    return _AssignmentILGenerator.Visit(this, a);
                case AstConstantNode c:
                    return _ConstantILGenerator.Visit(this, c);
                case AstVariableNode v:
                    return _VariableILGenerator.Visit(this, v);
                case AstBlockNode bl:
                    return _BlockILGenerator.Visit(this, bl);
                case AstDeclarationNode d:
                    return _DeclarationILGenerator.Visit(this, d);
                case AstLiteralNode l:
                    return _LiteralILGenerator.Visit(this, l);
                case AstAccessorNode ivk:
                    return _AccessorILGenerator.Visit(this, ivk);
                case AstIfNode i:
                    return _IfILGenerator.Visit(this, i);
                case AstWhileNode w:
                    return _WhileILGenerator.Visit(this, w);
                case AstForNode f:
                    return _ForILGenerator.Visit(this, f);
                case AstBreakNode brk:
                    return _BreakILGenerator.Visit(this, brk);
                case AstContinueNode cont:
                    return _ContinueILGenerator.Visit(this, cont);
                case AstCallableNode call:
                    return _CallILGenerator.Visit(this, call);
                case AstFuncDeclNode func:
                    return _FuncDeclILGenerator.Visit(this, func);
                case AstTupleNode t:
                    return _TupleILGenerator.Visit(this, t);
                case AstReturnNode ret:
                    return _ReturnILGenerator.Visit(this, ret);
                case AstNullCoalescingNode nc:
                    return _NullCoalescingILGenerator.Visit(this, nc);
                case AstNoOpNode np:
                    return null;
            }
            throw new AstWalkerException($"Unhandled type {node.GetType()}");
        }
    }
}
