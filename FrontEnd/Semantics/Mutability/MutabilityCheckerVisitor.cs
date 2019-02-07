// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Zenit.Ast;
using Zenit.Semantics.Symbols;

namespace Zenit.Semantics.Mutability
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

        public MutabilityCheckResult Visit(Node node)
        {
            object n = node;
            switch (n)
            {
                case UnaryNode u:
                    return this.unaryMutabilityChecker.Visit(this, u);

                case BinaryNode b:
                    return this.binaryMutabilityChecker.Visit(this, b);

                case AssignmentNode a:
                    return this.assignmentMutabilityChecker.Visit(this, a);

                case ConstantNode c:
                    return this.constantMutabilityChecker.Visit(this, c);

                case VariableNode v:
                    return this.variableMutabilityChecker.Visit(this, v);

                case BlockNode bl:
                    return this.blockMutabilityChecker.Visit(this, bl);

                case DeclarationNode d:
                    return this.declarationMutabilityChecker.Visit(this, d);

                case LiteralNode l:
                    return this.literalMutabilityChecker.Visit(this, l);

                case AccessorNode ivk:
                    return this.accessorMutabilityChecker.Visit(this, ivk);

                case IfNode i:
                    return this.ifMutabilityChecker.Visit(this, i);

                case WhileNode w:
                    return this.whileMutabilityChecker.Visit(this, w);

                case ForNode f:
                    return this.forMutabilityChecker.Visit(this, f);

                case BreakNode brk:
                    return this.breakMutabilityChecker.Visit(this, brk);

                case ContinueNode cont:
                    return this.continueMutabilityChecker.Visit(this, cont);

                case CallableNode call:
                    return this.callMutabilityChecker.Visit(this, call);

                case FunctionNode func:
                    return this.funcDeclMutabilityChecker.Visit(this, func);

                case TupleNode t:
                    return this.tupleMutabilityChecker.Visit(this, t);

                case ReturnNode ret:
                    return this.returnMutabilityChecker.Visit(this, ret);

                case NullCoalescingNode nc:
                    return this.nullCoalescingMutabilityChecker.Visit(this, nc);

                case ClassNode cn:
                    return this.classMutabilityChecker.Visit(this, cn);

                case ClassPropertyNode cpn:
                    return this.classPropertyMutabilityChecker.Visit(this, cpn);

                case ClassConstantNode ccn:
                    return this.classConstantMutabilityChecker.Visit(this, ccn);

                case ClassMethodNode cmn:
                    return this.classMethodMutabilityChecker.Visit(this, cmn);

                case NoOpNode np:
                    return null;
            }
            throw new AstWalkerException($"Unhandled type {node.GetType()}");
        }
    }
}
