// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Fl.Symbols;
using Fl.Parser.Ast;
using Fl.TypeChecker.Checkers;
using Fl.Lang.Types;

namespace Fl.TypeChecker
{
    public class AstVisitor
    {
        private TypeChecker checker;
        private UnaryTypeChecker unaryTypeChecker;
        private BinaryTypeChecker binaryTypeChecker;
        private AssignmentTypeChecker assignmentTypeChecker;
        private ConstantTypeChecker constantTypeChecker;
        private VariableTypeChecker variableTypeChecker;
        private BlockTypeChecker blockTypeChecker;
        private DeclarationTypeChecker declarationTypeChecker;
        private LiteralTypeChecker literalTypeChecker;
        private AccessorTypeChecker accessorTypeChecker;
        private IfTypeChecker ifTypeChecker;
        private WhileTypeChecker whileTypeChecker;
        private ForTypeChecker forTypeChecker;
        private BreakTypeChecker breakTypeChecker;
        private ContinueTypeChecker continueTypeChecker;
        private ReturnTypeChecker returnTypeChecker;
        private CallableTypeChecker callTypeChecker;
        private FuncDeclTypeChecker funcDeclTypeChecker;
        private TupleTypeChecker tupleTypeChecker;
        private NullCoalescingTypeChecker nullCoalescingTypeChecker;

        public AstVisitor(TypeChecker checker)
        {
            this.checker = checker;
            this.unaryTypeChecker = new UnaryTypeChecker();
            this.binaryTypeChecker = new BinaryTypeChecker();
            this.assignmentTypeChecker = new AssignmentTypeChecker();
            this.constantTypeChecker = new ConstantTypeChecker();
            this.variableTypeChecker = new VariableTypeChecker();
            this.blockTypeChecker = new BlockTypeChecker();
            this.declarationTypeChecker = new DeclarationTypeChecker();
            this.literalTypeChecker = new LiteralTypeChecker();
            this.accessorTypeChecker = new AccessorTypeChecker();
            this.ifTypeChecker = new IfTypeChecker();
            this.whileTypeChecker = new WhileTypeChecker();
            this.forTypeChecker = new ForTypeChecker();
            this.breakTypeChecker = new BreakTypeChecker();
            this.continueTypeChecker = new ContinueTypeChecker();
            this.callTypeChecker = new CallableTypeChecker();
            this.funcDeclTypeChecker = new FuncDeclTypeChecker();
            this.tupleTypeChecker = new TupleTypeChecker();
            this.returnTypeChecker = new ReturnTypeChecker();
            this.nullCoalescingTypeChecker = new NullCoalescingTypeChecker();
        }

        public Symbol Visit(AstNode node)
        {
            object n = node;
            switch (n)
            {
                case AstUnaryNode u:
                    return this.unaryTypeChecker.Visit(this.checker, u);
                case AstBinaryNode b:
                    return this.binaryTypeChecker.Visit(this.checker, b);
                case AstAssignmentNode a:
                    return this.assignmentTypeChecker.Visit(this.checker, a);
                case AstConstantNode c:
                    return this.constantTypeChecker.Visit(this.checker, c);
                case AstVariableNode v:
                    return this.variableTypeChecker.Visit(this.checker, v);
                case AstBlockNode bl:
                    return this.blockTypeChecker.Visit(this.checker, bl);
                case AstDeclarationNode d:
                    return this.declarationTypeChecker.Visit(this.checker, d);
                case AstLiteralNode l:
                    return this.literalTypeChecker.Visit(this.checker, l);
                case AstAccessorNode ivk:
                    return this.accessorTypeChecker.Visit(this.checker, ivk);
                case AstIfNode i:
                    return this.ifTypeChecker.Visit(this.checker, i);
                case AstWhileNode w:
                    return this.whileTypeChecker.Visit(this.checker, w);
                case AstForNode f:
                    return this.forTypeChecker.Visit(this.checker, f);
                case AstBreakNode brk:
                    return this.breakTypeChecker.Visit(this.checker, brk);
                case AstContinueNode cont:
                    return this.continueTypeChecker.Visit(this.checker, cont);
                case AstCallableNode call:
                    return this.callTypeChecker.Visit(this.checker, call);
                case AstFuncDeclNode func:
                    return this.funcDeclTypeChecker.Visit(this.checker, func);
                case AstTupleNode t:
                    return this.tupleTypeChecker.Visit(this.checker, t);
                case AstReturnNode ret:
                    return this.returnTypeChecker.Visit(this.checker, ret);
                case AstNullCoalescingNode nc:
                    return this.nullCoalescingTypeChecker.Visit(this.checker, nc);
                case AstNoOpNode np:
                    return null;
            }
            throw new AstWalkerException($"Unhandled type {node.GetType()}");
        }
    }
}
