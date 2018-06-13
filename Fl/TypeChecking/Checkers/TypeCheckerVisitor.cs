// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Fl.Symbols;
using Fl.Ast;
using Fl.Symbols.Types;

namespace Fl.TypeChecking.Checkers
{
    public class TypeCheckerVisitor : IAstWalker<Type>
    {
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

        public TypeCheckerVisitor(SymbolTable symtable)
        {
            this.SymbolTable = symtable;
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

        /// <summary>
        /// Tracks variables per blocks
        /// </summary>
        public SymbolTable SymbolTable { get; private set; }

        // Adds a new block to the SymbolTable, it represents a new scope
        public void EnterBlock(ScopeType type, string name)
        {
            this.SymbolTable.EnterScope(type, name);
        }

        // Leave the current block in the SymbolTable
        public void LeaveBlock()
        {
            this.SymbolTable.LeaveScope();
        }

        // Returns true if the current fragment is a function fragment
        public bool InFunction => this.SymbolTable.InFunction;

        public Type Visit(AstNode node)
        {
            object n = node;
            switch (n)
            {
                case AstUnaryNode u:
                    return this.unaryTypeChecker.Visit(this, u);
                case AstBinaryNode b:
                    return this.binaryTypeChecker.Visit(this, b);
                case AstAssignmentNode a:
                    return this.assignmentTypeChecker.Visit(this, a);
                case AstConstantNode c:
                    return this.constantTypeChecker.Visit(this, c);
                case AstVariableNode v:
                    return this.variableTypeChecker.Visit(this, v);
                case AstBlockNode bl:
                    return this.blockTypeChecker.Visit(this, bl);
                case AstDeclarationNode d:
                    return this.declarationTypeChecker.Visit(this, d);
                case AstLiteralNode l:
                    return this.literalTypeChecker.Visit(this, l);
                case AstAccessorNode ivk:
                    return this.accessorTypeChecker.Visit(this, ivk);
                case AstIfNode i:
                    return this.ifTypeChecker.Visit(this, i);
                case AstWhileNode w:
                    return this.whileTypeChecker.Visit(this, w);
                case AstForNode f:
                    return this.forTypeChecker.Visit(this, f);
                case AstBreakNode brk:
                    return this.breakTypeChecker.Visit(this, brk);
                case AstContinueNode cont:
                    return this.continueTypeChecker.Visit(this, cont);
                case AstCallableNode call:
                    return this.callTypeChecker.Visit(this, call);
                case AstFuncDeclNode func:
                    return this.funcDeclTypeChecker.Visit(this, func);
                case AstTupleNode t:
                    return this.tupleTypeChecker.Visit(this, t);
                case AstReturnNode ret:
                    return this.returnTypeChecker.Visit(this, ret);
                case AstNullCoalescingNode nc:
                    return this.nullCoalescingTypeChecker.Visit(this, nc);
                case AstNoOpNode np:
                    return null;
            }
            throw new AstWalkerException($"Unhandled type {node.GetType()}");
        }
    }
}
