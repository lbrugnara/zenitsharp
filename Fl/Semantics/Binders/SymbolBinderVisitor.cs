// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Fl.Ast;
using Fl.Semantics.Inferrers;
using Fl.Semantics.Symbols;

namespace Fl.Semantics.Binders
{
    public class SymbolBinderVisitor : IAstWalker
    {
        private UnarySymbolBinder unarySymbolResolver;
        private BinarySymbolBinder binarySymbolResolver;
        private AssignmentSymbolBinder assignmentSymbolResolver;
        private ConstantSymbolBinder constantSymbolResolver;
        private VariableSymbolBinder variableSymbolResolver;
        private BlockSymbolBinder blockSymbolResolver;
        private DeclarationSymbolBinder declarationSymbolResolver;
        private LiteralSymbolBinder literalSymbolResolver;
        private AccessorSymbolBinder accessorSymbolResolver;
        private IfSymbolBinder ifSymbolResolver;
        private WhileSymbolBinder whileSymbolResolver;
        private ForSymbolBinder forSymbolResolver;
        private BreakSymbolBinder breakSymbolResolver;
        private ContinueSymbolBinder continueSymbolResolver;
        private ReturnSymbolBinder returnSymbolResolver;
        private CallableSymbolBinder callSymbolResolver;
        private FuncDeclSymbolBinder funcDeclSymbolResolver;
        private TupleSymbolBinder tupleSymbolResolver;
        private NullCoalescingSymbolBinder nullCoalescingSymbolResolver;

        public SymbolBinderVisitor(SymbolTable symtable, TypeInferrer inferrer)
        {
            this.SymbolTable = symtable;
            this.Inferrer = inferrer;
            this.unarySymbolResolver = new UnarySymbolBinder();
            this.binarySymbolResolver = new BinarySymbolBinder();
            this.assignmentSymbolResolver = new AssignmentSymbolBinder();
            this.constantSymbolResolver = new ConstantSymbolBinder();
            this.variableSymbolResolver = new VariableSymbolBinder();
            this.blockSymbolResolver = new BlockSymbolBinder();
            this.declarationSymbolResolver = new DeclarationSymbolBinder();
            this.literalSymbolResolver = new LiteralSymbolBinder();
            this.accessorSymbolResolver = new AccessorSymbolBinder();
            this.ifSymbolResolver = new IfSymbolBinder();
            this.whileSymbolResolver = new WhileSymbolBinder();
            this.forSymbolResolver = new ForSymbolBinder();
            this.breakSymbolResolver = new BreakSymbolBinder();
            this.continueSymbolResolver = new ContinueSymbolBinder();
            this.callSymbolResolver = new CallableSymbolBinder();
            this.funcDeclSymbolResolver = new FuncDeclSymbolBinder();
            this.tupleSymbolResolver = new TupleSymbolBinder();
            this.returnSymbolResolver = new ReturnSymbolBinder();
            this.nullCoalescingSymbolResolver = new NullCoalescingSymbolBinder();
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
