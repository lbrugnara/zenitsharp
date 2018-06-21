// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Fl.Ast;
using Fl.IL.Instructions.Operands;
using Fl.IL.Generators;

namespace Fl.IL
{
    public class AstVisitor
    {
        private ILGenerator generator;
        private UnaryILGenerator unaryILGenerator;
        private BinaryILGenerator binaryILGenerator;
        private AssignmentILGenerator assignmentILGenerator;
        private ConstantILGenerator constantILGenerator;
        private VariableILGenerator variableILGenerator;
        private BlockILGenerator blockILGenerator;
        private DeclarationILGenerator declarationILGenerator;
        private LiteralILGenerator literalILGenerator;
        private AccessorILGenerator accessorILGenerator;
        private IfILGenerator ifILGenerator;
        private WhileILGenerator whileILGenerator;
        private ForILGenerator forILGenerator;
        private BreakILGenerator breakILGenerator;
        private ContinueILGenerator continueILGenerator;
        private ReturnILGenerator returnILGenerator;
        private CallableILGenerator callILGenerator;
        private FuncDeclILGenerator funcDeclILGenerator;
        private TupleILGenerator tupleILGenerator;
        private NullCoalescingILGenerator nullCoalescingILGenerator;

        public AstVisitor(ILGenerator generator)
        {
            this.generator = generator;
            this.unaryILGenerator = new UnaryILGenerator();
            this.binaryILGenerator = new BinaryILGenerator();
            this.assignmentILGenerator = new AssignmentILGenerator();
            this.constantILGenerator = new ConstantILGenerator();
            this.variableILGenerator = new VariableILGenerator();
            this.blockILGenerator = new BlockILGenerator();
            this.declarationILGenerator = new DeclarationILGenerator();
            this.literalILGenerator = new LiteralILGenerator();
            this.accessorILGenerator = new AccessorILGenerator();
            this.ifILGenerator = new IfILGenerator();
            this.whileILGenerator = new WhileILGenerator();
            this.forILGenerator = new ForILGenerator();
            this.breakILGenerator = new BreakILGenerator();
            this.continueILGenerator = new ContinueILGenerator();
            this.callILGenerator = new CallableILGenerator();
            this.funcDeclILGenerator = new FuncDeclILGenerator();
            this.tupleILGenerator = new TupleILGenerator();
            this.returnILGenerator = new ReturnILGenerator();
            this.nullCoalescingILGenerator = new NullCoalescingILGenerator();
        }

        public Operand Visit(AstNode node)
        {
            object n = node;
            switch (n)
            {
                case AstUnaryNode u:
                    return this.unaryILGenerator.Visit(this.generator, u);
                case AstBinaryNode b:
                    return this.binaryILGenerator.Visit(this.generator, b);
                case AstAssignmentNode a:
                    return this.assignmentILGenerator.Visit(this.generator, a);
                case AstConstantNode c:
                    return this.constantILGenerator.Visit(this.generator, c);
                case AstVariableNode v:
                    return this.variableILGenerator.Visit(this.generator, v);
                case AstBlockNode bl:
                    return this.blockILGenerator.Visit(this.generator, bl);
                case AstDeclarationNode d:
                    return this.declarationILGenerator.Visit(this.generator, d);
                case AstLiteralNode l:
                    return this.literalILGenerator.Visit(this.generator, l);
                case AstAccessorNode ivk:
                    return this.accessorILGenerator.Visit(this.generator, ivk);
                case AstIfNode i:
                    return this.ifILGenerator.Visit(this.generator, i);
                case AstWhileNode w:
                    return this.whileILGenerator.Visit(this.generator, w);
                case AstForNode f:
                    return this.forILGenerator.Visit(this.generator, f);
                case AstBreakNode brk:
                    return this.breakILGenerator.Visit(this.generator, brk);
                case AstContinueNode cont:
                    return this.continueILGenerator.Visit(this.generator, cont);
                case AstCallableNode call:
                    return this.callILGenerator.Visit(this.generator, call);
                case AstFunctionNode func:
                    return this.funcDeclILGenerator.Visit(this.generator, func);
                case AstTupleNode t:
                    return this.tupleILGenerator.Visit(this.generator, t);
                case AstReturnNode ret:
                    return this.returnILGenerator.Visit(this.generator, ret);
                case AstNullCoalescingNode nc:
                    return this.nullCoalescingILGenerator.Visit(this.generator, nc);
                case AstNoOpNode np:
                    return null;
            }
            throw new AstWalkerException($"Unhandled type {node.GetType()}");
        }
    }
}
