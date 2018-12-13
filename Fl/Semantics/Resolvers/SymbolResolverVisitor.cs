// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Fl.Ast;
using Fl.Semantics.Inferrers;
using Fl.Semantics.Symbols;

namespace Fl.Semantics.Resolvers
{
    public class SymbolResolverVisitor : IAstWalker
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

        public void Visit(Node node)
        {
            object n = node;

            switch (n)
            {
                case UnaryNode u:
                    this.unarySymbolResolver.Visit(this, u);
                    break;

                case BinaryNode b:
                    this.binarySymbolResolver.Visit(this, b);
                    break;

                case AssignmentNode a:
                    this.assignmentSymbolResolver.Visit(this, a);
                    break;

                case ConstantNode c:
                    this.constantSymbolResolver.Visit(this, c);
                    break;

                case VariableNode v:
                    this.variableSymbolResolver.Visit(this, v);
                    break;

                case BlockNode bl:
                    this.blockSymbolResolver.Visit(this, bl);
                    break;

                case DeclarationNode d:
                    this.declarationSymbolResolver.Visit(this, d);
                    break;

                case LiteralNode l:
                    this.literalSymbolResolver.Visit(this, l);
                    break;

                case AccessorNode ivk:
                    this.accessorSymbolResolver.Visit(this, ivk);
                    break;

                case IfNode i:
                    this.ifSymbolResolver.Visit(this, i);
                    break;

                case WhileNode w:
                    this.whileSymbolResolver.Visit(this, w);
                    break;

                case ForNode f:
                    this.forSymbolResolver.Visit(this, f);
                    break;

                case BreakNode brk:
                    this.breakSymbolResolver.Visit(this, brk);
                    break;

                case ContinueNode cont:
                    this.continueSymbolResolver.Visit(this, cont);
                    break;

                case CallableNode call:
                    this.callSymbolResolver.Visit(this, call);
                    break;

                case FunctionNode func:
                    this.funcDeclSymbolResolver.Visit(this, func);
                    break;

                case ObjectNode obj:
                    this.objectSymbolResolver.Visit(this, obj);
                    break;

                case ObjectPropertyNode objProp:
                    this.objectPropertySymbolResolver.Visit(this, objProp);
                    break;

                case TupleNode t:
                    this.tupleSymbolResolver.Visit(this, t);
                    break;

                case ReturnNode ret:
                    this.returnSymbolResolver.Visit(this, ret);
                    break;

                case NullCoalescingNode nc:
                    this.nullCoalescingSymbolResolver.Visit(this, nc);
                    break;

                case ClassNode cn:
                    this.classSymbolResolver.Visit(this, cn);
                    break;

                case ClassPropertyNode cpn:
                    this.classPropertySymbolResolver.Visit(this, cpn);
                    break;

                case ClassConstantNode ccn:
                    this.classConstantSymbolResolver.Visit(this, ccn);
                    break;

                case ClassMethodNode cmn:
                    this.classMethodSymbolResolver.Visit(this, cmn);
                    break;

                case NoOpNode np:
                    break;

                default:
                    throw new AstWalkerException($"Unhandled SymbolResolver type {node.GetType()}");
            }
        }
    }
}
