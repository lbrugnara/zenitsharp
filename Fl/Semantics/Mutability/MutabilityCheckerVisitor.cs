// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Fl.Ast;
using Fl.Semantics.Inferrers;
using Fl.Semantics.Symbols;
using System.Collections.Generic;

namespace Fl.Semantics.Mutability
{
    public class MutabilityCheckerVisitor : IAstWalker<MutabilityCheckResult>
    {
        private UnaryMutabilityChecker unaryMutabilityChecker;
        private BinaryMutabilityChecker binaryMutabilityChecker;
        private AssignmentMutabilityChecker assignmentMutabilityChecker;
        private ConstantMutabilityChecker constantMutabilityChecker;
        private VariableMutabilityChecker variableMutabilityChecker;
        private BlockMutabilityChecker blockMutabilityChecker;
        private DeclarationMutabilityChecker declarationMutabilityChecker;
        private LiteralMutabilityChecker literalMutabilityChecker;
        private AccessorMutabilityChecker accessorMutabilityChecker;
        private IfMutabilityChecker ifMutabilityChecker;
        private WhileMutabilityChecker whileMutabilityChecker;
        private ForMutabilityChecker forMutabilityChecker;
        private BreakMutabilityChecker breakMutabilityChecker;
        private ContinueMutabilityChecker continueMutabilityChecker;
        private ReturnMutabilityChecker returnMutabilityChecker;
        private CallableMutabilityChecker callMutabilityChecker;
        private FunctionMutabilityChecker funcDeclMutabilityChecker;
        private TupleMutabilityChecker tupleMutabilityChecker;
        private NullCoalescingMutabilityChecker nullCoalescingMutabilityChecker;
        private ClassMutabilityChecker classMutabilityChecker;
        private ClassPropertyMutabilityChecker classPropertyMutabilityChecker;
        private ClassConstantMutabilityChecker classConstantMutabilityChecker;
        private ClassMethodMutabilityChecker classMethodMutabilityChecker;

        public MutabilityCheckerVisitor(SymbolTable symtable)
        {
            this.SymbolTable = symtable;
            this.unaryMutabilityChecker = new UnaryMutabilityChecker();
            this.binaryMutabilityChecker = new BinaryMutabilityChecker();
            this.assignmentMutabilityChecker = new AssignmentMutabilityChecker();
            this.constantMutabilityChecker = new ConstantMutabilityChecker();
            this.variableMutabilityChecker = new VariableMutabilityChecker();
            this.blockMutabilityChecker = new BlockMutabilityChecker();
            this.declarationMutabilityChecker = new DeclarationMutabilityChecker();
            this.literalMutabilityChecker = new LiteralMutabilityChecker();
            this.accessorMutabilityChecker = new AccessorMutabilityChecker();
            this.ifMutabilityChecker = new IfMutabilityChecker();
            this.whileMutabilityChecker = new WhileMutabilityChecker();
            this.forMutabilityChecker = new ForMutabilityChecker();
            this.breakMutabilityChecker = new BreakMutabilityChecker();
            this.continueMutabilityChecker = new ContinueMutabilityChecker();
            this.callMutabilityChecker = new CallableMutabilityChecker();
            this.funcDeclMutabilityChecker = new FunctionMutabilityChecker();
            this.tupleMutabilityChecker = new TupleMutabilityChecker();
            this.returnMutabilityChecker = new ReturnMutabilityChecker();
            this.nullCoalescingMutabilityChecker = new NullCoalescingMutabilityChecker();
            this.classMutabilityChecker = new ClassMutabilityChecker();
            this.classPropertyMutabilityChecker = new ClassPropertyMutabilityChecker();
            this.classConstantMutabilityChecker = new ClassConstantMutabilityChecker();
            this.classMethodMutabilityChecker = new ClassMethodMutabilityChecker();
        }

        /// <summary>
        /// Tracks variables per blocks
        /// </summary>
        public SymbolTable SymbolTable { get; private set; }

        public MutabilityCheckResult Visit(AstNode node)
        {
            object n = node;
            switch (n)
            {
                case AstUnaryNode u:
                    return this.unaryMutabilityChecker.Visit(this, u);

                case AstBinaryNode b:
                    return this.binaryMutabilityChecker.Visit(this, b);

                case AstAssignmentNode a:
                    return this.assignmentMutabilityChecker.Visit(this, a);

                case AstConstantNode c:
                    return this.constantMutabilityChecker.Visit(this, c);

                case AstVariableNode v:
                    return this.variableMutabilityChecker.Visit(this, v);

                case AstBlockNode bl:
                    return this.blockMutabilityChecker.Visit(this, bl);

                case AstDeclarationNode d:
                    return this.declarationMutabilityChecker.Visit(this, d);

                case AstLiteralNode l:
                    return this.literalMutabilityChecker.Visit(this, l);

                case AstAccessorNode ivk:
                    return this.accessorMutabilityChecker.Visit(this, ivk);

                case AstIfNode i:
                    return this.ifMutabilityChecker.Visit(this, i);

                case AstWhileNode w:
                    return this.whileMutabilityChecker.Visit(this, w);

                case AstForNode f:
                    return this.forMutabilityChecker.Visit(this, f);

                case AstBreakNode brk:
                    return this.breakMutabilityChecker.Visit(this, brk);

                case AstContinueNode cont:
                    return this.continueMutabilityChecker.Visit(this, cont);

                case AstCallableNode call:
                    return this.callMutabilityChecker.Visit(this, call);

                case AstFunctionNode func:
                    return this.funcDeclMutabilityChecker.Visit(this, func);

                case AstTupleNode t:
                    return this.tupleMutabilityChecker.Visit(this, t);

                case AstReturnNode ret:
                    return this.returnMutabilityChecker.Visit(this, ret);

                case AstNullCoalescingNode nc:
                    return this.nullCoalescingMutabilityChecker.Visit(this, nc);

                case AstClassNode cn:
                    return this.classMutabilityChecker.Visit(this, cn);

                case AstClassPropertyNode cpn:
                    return this.classPropertyMutabilityChecker.Visit(this, cpn);

                case AstClassConstantNode ccn:
                    return this.classConstantMutabilityChecker.Visit(this, ccn);

                case AstClassMethodNode cmn:
                    return this.classMethodMutabilityChecker.Visit(this, cmn);

                case AstNoOpNode np:
                    return null;
            }
            throw new AstWalkerException($"Unhandled type {node.GetType()}");
        }
    }
}
