// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Fl.Ast;
using Fl.TypeChecking.Inferrers;

namespace Fl.Symbols.Resolvers
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
        private FuncDeclSymbolResolver funcDeclSymbolResolver;
        private TupleSymbolResolver tupleSymbolResolver;
        private NullCoalescingSymbolResolver nullCoalescingSymbolResolver;

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
            this.funcDeclSymbolResolver = new FuncDeclSymbolResolver();
            this.tupleSymbolResolver = new TupleSymbolResolver();
            this.returnSymbolResolver = new ReturnSymbolResolver();
            this.nullCoalescingSymbolResolver = new NullCoalescingSymbolResolver();
        }

        /// <summary>
        /// Tracks variables per blocks
        /// </summary>
        public SymbolTable SymbolTable { get; private set; }

        public TypeInferrer Inferrer { get; private set; }

        public void Visit(AstNode node)
        {
            object n = node;

            switch (n)
            {
                case AstUnaryNode u:
                    this.unarySymbolResolver.Visit(this, u);
                    break;

                case AstBinaryNode b:
                    this.binarySymbolResolver.Visit(this, b);
                    break;

                case AstAssignmentNode a:
                    this.assignmentSymbolResolver.Visit(this, a);
                    break;

                case AstConstantNode c:
                    this.constantSymbolResolver.Visit(this, c);
                    break;

                case AstVariableNode v:
                    this.variableSymbolResolver.Visit(this, v);
                    break;

                case AstBlockNode bl:
                    this.blockSymbolResolver.Visit(this, bl);
                    break;

                case AstDeclarationNode d:
                    this.declarationSymbolResolver.Visit(this, d);
                    break;

                case AstLiteralNode l:
                    this.literalSymbolResolver.Visit(this, l);
                    break;

                case AstAccessorNode ivk:
                    this.accessorSymbolResolver.Visit(this, ivk);
                    break;

                case AstIfNode i:
                    this.ifSymbolResolver.Visit(this, i);
                    break;

                case AstWhileNode w:
                    this.whileSymbolResolver.Visit(this, w);
                    break;

                case AstForNode f:
                    this.forSymbolResolver.Visit(this, f);
                    break;

                case AstBreakNode brk:
                    this.breakSymbolResolver.Visit(this, brk);
                    break;

                case AstContinueNode cont:
                    this.continueSymbolResolver.Visit(this, cont);
                    break;

                case AstCallableNode call:
                    this.callSymbolResolver.Visit(this, call);
                    break;

                case AstFuncDeclNode func:
                    this.funcDeclSymbolResolver.Visit(this, func);
                    break;

                case AstTupleNode t:
                    this.tupleSymbolResolver.Visit(this, t);
                    break;

                case AstReturnNode ret:
                    this.returnSymbolResolver.Visit(this, ret);
                    break;

                case AstNullCoalescingNode nc:
                    this.nullCoalescingSymbolResolver.Visit(this, nc);
                    break;

                case AstNoOpNode np:
                    break;

                default:
                    throw new AstWalkerException($"Unhandled SymbolResolver type {node.GetType()}");
            }
        }
    }
}
