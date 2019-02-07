// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Zenit.Ast;
using Zenit.Semantics.Symbols;

namespace Zenit.Semantics.Checkers
{
    public class TypeCheckerVisitor : IAstWalker<CheckedType>
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
        private FunctionTypeChecker funcDeclTypeChecker;
        private TupleTypeChecker tupleTypeChecker;
        private NullCoalescingTypeChecker nullCoalescingTypeChecker;
        private ClassTypeChecker classTypeChecker;
        private ClassPropertyTypeChecker classPropertyTypeChecker;
        private ClassConstantTypeChecker classConstantTypeChecker;
        private ClassMethodTypeChecker classMethodTypeChecker;

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
            this.funcDeclTypeChecker = new FunctionTypeChecker();
            this.tupleTypeChecker = new TupleTypeChecker();
            this.returnTypeChecker = new ReturnTypeChecker();
            this.nullCoalescingTypeChecker = new NullCoalescingTypeChecker();
            this.classTypeChecker = new ClassTypeChecker();
            this.classPropertyTypeChecker = new ClassPropertyTypeChecker();
            this.classConstantTypeChecker = new ClassConstantTypeChecker();
            this.classMethodTypeChecker = new ClassMethodTypeChecker();
        }

        /// <summary>
        /// Tracks variables per blocks
        /// </summary>
        public SymbolTable SymbolTable { get; private set; }

        public CheckedType Visit(Node node)
        {
            object n = node;
            switch (n)
            {
                case UnaryNode u:
                    return this.unaryTypeChecker.Visit(this, u);

                case BinaryNode b:
                    return this.binaryTypeChecker.Visit(this, b);

                case AssignmentNode a:
                    return this.assignmentTypeChecker.Visit(this, a);

                case ConstantNode c:
                    return this.constantTypeChecker.Visit(this, c);

                case VariableNode v:
                    return this.variableTypeChecker.Visit(this, v);

                case BlockNode bl:
                    return this.blockTypeChecker.Visit(this, bl);

                case DeclarationNode d:
                    return this.declarationTypeChecker.Visit(this, d);

                case LiteralNode l:
                    return this.literalTypeChecker.Visit(this, l);

                case AccessorNode ivk:
                    return this.accessorTypeChecker.Visit(this, ivk);

                case IfNode i:
                    return this.ifTypeChecker.Visit(this, i);

                case WhileNode w:
                    return this.whileTypeChecker.Visit(this, w);

                case ForNode f:
                    return this.forTypeChecker.Visit(this, f);

                case BreakNode brk:
                    return this.breakTypeChecker.Visit(this, brk);

                case ContinueNode cont:
                    return this.continueTypeChecker.Visit(this, cont);

                case CallableNode call:
                    return this.callTypeChecker.Visit(this, call);

                case FunctionNode func:
                    return this.funcDeclTypeChecker.Visit(this, func);

                case TupleNode t:
                    return this.tupleTypeChecker.Visit(this, t);

                case ReturnNode ret:
                    return this.returnTypeChecker.Visit(this, ret);

                case NullCoalescingNode nc:
                    return this.nullCoalescingTypeChecker.Visit(this, nc);

                case ClassNode cn:
                    return this.classTypeChecker.Visit(this, cn);

                case ClassPropertyNode cpn:
                    return this.classPropertyTypeChecker.Visit(this, cpn);

                case ClassConstantNode ccn:
                    return this.classConstantTypeChecker.Visit(this, ccn);

                case ClassMethodNode cmn:
                    return this.classMethodTypeChecker.Visit(this, cmn);

                case NoOpNode np:
                    return null;
            }
            throw new AstWalkerException($"Unhandled type {node.GetType()}");
        }
    }
}
