// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Fl.Parser.Ast;
using Fl.Engine.IL.Instructions.Operands;
using Fl.Engine.IL.Generators;

namespace Fl.Engine.IL
{
    public class AstVisitor
    {
        private ILGenerator _Generator;
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

        public AstVisitor(ILGenerator generator)
        {
            _Generator = generator;
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

        public Operand Visit(AstNode node)
        {
            object n = node;
            switch (n)
            {
                case AstUnaryNode u:
                    return _UnaryILGenerator.Visit(_Generator, u);
                case AstBinaryNode b:
                    return _BinaryILGenerator.Visit(_Generator, b);
                case AstAssignmentNode a:
                    return _AssignmentILGenerator.Visit(_Generator, a);
                case AstConstantNode c:
                    return _ConstantILGenerator.Visit(_Generator, c);
                case AstVariableNode v:
                    return _VariableILGenerator.Visit(_Generator, v);
                case AstBlockNode bl:
                    return _BlockILGenerator.Visit(_Generator, bl);
                case AstDeclarationNode d:
                    return _DeclarationILGenerator.Visit(_Generator, d);
                case AstLiteralNode l:
                    return _LiteralILGenerator.Visit(_Generator, l);
                case AstAccessorNode ivk:
                    return _AccessorILGenerator.Visit(_Generator, ivk);
                case AstIfNode i:
                    return _IfILGenerator.Visit(_Generator, i);
                case AstWhileNode w:
                    return _WhileILGenerator.Visit(_Generator, w);
                case AstForNode f:
                    return _ForILGenerator.Visit(_Generator, f);
                case AstBreakNode brk:
                    return _BreakILGenerator.Visit(_Generator, brk);
                case AstContinueNode cont:
                    return _ContinueILGenerator.Visit(_Generator, cont);
                case AstCallableNode call:
                    return _CallILGenerator.Visit(_Generator, call);
                case AstFuncDeclNode func:
                    return _FuncDeclILGenerator.Visit(_Generator, func);
                case AstTupleNode t:
                    return _TupleILGenerator.Visit(_Generator, t);
                case AstReturnNode ret:
                    return _ReturnILGenerator.Visit(_Generator, ret);
                case AstNullCoalescingNode nc:
                    return _NullCoalescingILGenerator.Visit(_Generator, nc);
                case AstNoOpNode np:
                    return null;
            }
            throw new AstWalkerException($"Unhandled type {node.GetType()}");
        }
    }
}
