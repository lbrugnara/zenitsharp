// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Fl.Ast;
using Fl.Semantics.Inferrers;
using Fl.Semantics.Symbols;
using Fl.Semantics.Symbols;

namespace Fl.Semantics.Resolvers
{
    public class SymbolResolverVisitor : IAstWalker<ISymbol>
    {
        private UnarySymbolResolver unarySymbolResolver;
        private BinarySymbolResolver binarySymbolResolver;
        private AssignmentSymbolResolver assignmentSymbolResolver;
        private ConstantSymbolResolver constantSymbolResolver;
        private VariableSymbolResolver variableSymbolResolver;
        private BlockSymbolResolver blockSymbolResolver;
        private DeclarationSymbolResolver declarationSymbolResolver;
        private LiteralSymbolResolver literalSymbolResolver;
        private AccessorSymbolResolver accessorSymbolResolver;
        private IfSymbolResolver ifSymbolResolver;
        private WhileSymbolResolver whileSymbolResolver;
        private ForSymbolResolver forSymbolResolver;
        private BreakSymbolResolver breakSymbolResolver;
        private ContinueSymbolResolver continueSymbolResolver;
        private ReturnSymbolResolver returnSymbolResolver;
        private CallableSymbolResolver callSymbolResolver;
        private FunctionSymbolResolver funcDeclSymbolResolver;
        private ObjectSymbolResolver objectSymbolResolver;
        private ObjectPropertySymbolResolver objectPropertySymbolResolver;
        private TupleSymbolResolver tupleSymbolResolver;
        private NullCoalescingSymbolResolver nullCoalescingSymbolResolver;
        private ClassSymbolResolver classSymbolResolver;
        private ClassPropertySymbolResolver classPropertySymbolResolver;
        private ClassConstantSymbolResolver classConstantSymbolResolver;
        private ClassMethodSymbolResolver classMethodSymbolResolver;

        public SymbolResolverVisitor(SymbolTable symtable, TypeInferrer inferrer)
        {
            this.SymbolTable = symtable;
            this.Inferrer = inferrer;
            this.unarySymbolResolver = new UnarySymbolResolver();
            this.binarySymbolResolver = new BinarySymbolResolver();
            this.assignmentSymbolResolver = new AssignmentSymbolResolver();
            this.constantSymbolResolver = new ConstantSymbolResolver();
            this.variableSymbolResolver = new VariableSymbolResolver();
            this.blockSymbolResolver = new BlockSymbolResolver();
            this.declarationSymbolResolver = new DeclarationSymbolResolver();
            this.literalSymbolResolver = new LiteralSymbolResolver();
            this.accessorSymbolResolver = new AccessorSymbolResolver();
            this.ifSymbolResolver = new IfSymbolResolver();
            this.whileSymbolResolver = new WhileSymbolResolver();
            this.forSymbolResolver = new ForSymbolResolver();
            this.breakSymbolResolver = new BreakSymbolResolver();
            this.continueSymbolResolver = new ContinueSymbolResolver();
            this.callSymbolResolver = new CallableSymbolResolver();
            this.funcDeclSymbolResolver = new FunctionSymbolResolver();
            this.objectSymbolResolver = new ObjectSymbolResolver();
            this.objectPropertySymbolResolver = new ObjectPropertySymbolResolver();
            this.tupleSymbolResolver = new TupleSymbolResolver();
            this.returnSymbolResolver = new ReturnSymbolResolver();
            this.nullCoalescingSymbolResolver = new NullCoalescingSymbolResolver();
            this.classSymbolResolver = new ClassSymbolResolver();
            this.classPropertySymbolResolver = new ClassPropertySymbolResolver();
            this.classConstantSymbolResolver = new ClassConstantSymbolResolver();
            this.classMethodSymbolResolver = new ClassMethodSymbolResolver();
        }

        /// <summary>
        /// Tracks variables per blocks
        /// </summary>
        public SymbolTable SymbolTable { get; private set; }

        public TypeInferrer Inferrer { get; private set; }

        public ISymbol Visit(Node node)
        {
            object n = node;

            switch (n)
            {
                case UnaryNode u:
                    return this.unarySymbolResolver.Visit(this, u);

                case BinaryNode b:
                    return this.binarySymbolResolver.Visit(this, b);

                case AssignmentNode a:
                    return this.assignmentSymbolResolver.Visit(this, a);

                case ConstantNode c:
                    return this.constantSymbolResolver.Visit(this, c);

                case VariableNode v:
                    return this.variableSymbolResolver.Visit(this, v);

                case BlockNode bl:
                    return this.blockSymbolResolver.Visit(this, bl);

                case DeclarationNode d:
                    return this.declarationSymbolResolver.Visit(this, d);

                case LiteralNode l:
                    return this.literalSymbolResolver.Visit(this, l);

                case AccessorNode ivk:
                    return this.accessorSymbolResolver.Visit(this, ivk);

                case IfNode i:
                    return this.ifSymbolResolver.Visit(this, i);

                case WhileNode w:
                    return this.whileSymbolResolver.Visit(this, w);

                case ForNode f:
                    return this.forSymbolResolver.Visit(this, f);

                case BreakNode brk:
                    return this.breakSymbolResolver.Visit(this, brk);

                case ContinueNode cont:
                    return this.continueSymbolResolver.Visit(this, cont);

                case CallableNode call:
                    return this.callSymbolResolver.Visit(this, call);

                case FunctionNode func:
                    return this.funcDeclSymbolResolver.Visit(this, func);

                case ObjectNode obj:
                    return this.objectSymbolResolver.Visit(this, obj);

                case ObjectPropertyNode objProp:
                    return this.objectPropertySymbolResolver.Visit(this, objProp);

                case TupleNode t:
                    return this.tupleSymbolResolver.Visit(this, t);

                case ReturnNode ret:
                    return this.returnSymbolResolver.Visit(this, ret);

                case NullCoalescingNode nc:
                    return this.nullCoalescingSymbolResolver.Visit(this, nc);

                case ClassNode cn:
                    return this.classSymbolResolver.Visit(this, cn);

                case ClassPropertyNode cpn:
                    return this.classPropertySymbolResolver.Visit(this, cpn);

                case ClassConstantNode ccn:
                    return this.classConstantSymbolResolver.Visit(this, ccn);

                case ClassMethodNode cmn:
                    return this.classMethodSymbolResolver.Visit(this, cmn);

                case NoOpNode np:
                    return null;

                default:
                    throw new AstWalkerException($"Unhandled SymbolResolver type {node.GetType()}");
            }
        }
    }
}
