// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Fl.Ast;
using Fl.Symbols.Resolvers;

namespace Fl.Symbols
{
    public class AstVisitor
    {
        private SymbolResolver resolver;
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

        public AstVisitor(SymbolResolver resolver)
        {
            this.resolver = resolver;
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

        public void Visit(AstNode node)
        {
            object n = node;

            switch (n)
            {
                case AstUnaryNode u:
                    this.unarySymbolResolver.Visit(this.resolver, u);
                    break;

                case AstBinaryNode b:
                    this.binarySymbolResolver.Visit(this.resolver, b);
                    break;

                case AstAssignmentNode a:
                    this.assignmentSymbolResolver.Visit(this.resolver, a);
                    break;

                case AstConstantNode c:
                    this.constantSymbolResolver.Visit(this.resolver, c);
                    break;

                case AstVariableNode v:
                    this.variableSymbolResolver.Visit(this.resolver, v);
                    break;

                case AstBlockNode bl:
                    this.blockSymbolResolver.Visit(this.resolver, bl);
                    break;

                case AstDeclarationNode d:
                    this.declarationSymbolResolver.Visit(this.resolver, d);
                    break;

                case AstLiteralNode l:
                    this.literalSymbolResolver.Visit(this.resolver, l);
                    break;

                case AstAccessorNode ivk:
                    this.accessorSymbolResolver.Visit(this.resolver, ivk);
                    break;

                case AstIfNode i:
                    this.ifSymbolResolver.Visit(this.resolver, i);
                    break;

                case AstWhileNode w:
                    this.whileSymbolResolver.Visit(this.resolver, w);
                    break;

                case AstForNode f:
                    this.forSymbolResolver.Visit(this.resolver, f);
                    break;

                case AstBreakNode brk:
                    this.breakSymbolResolver.Visit(this.resolver, brk);
                    break;

                case AstContinueNode cont:
                    this.continueSymbolResolver.Visit(this.resolver, cont);
                    break;

                case AstCallableNode call:
                    this.callSymbolResolver.Visit(this.resolver, call);
                    break;

                case AstFuncDeclNode func:
                    this.funcDeclSymbolResolver.Visit(this.resolver, func);
                    break;

                case AstTupleNode t:
                    this.tupleSymbolResolver.Visit(this.resolver, t);
                    break;

                case AstReturnNode ret:
                    this.returnSymbolResolver.Visit(this.resolver, ret);
                    break;

                case AstNullCoalescingNode nc:
                    this.nullCoalescingSymbolResolver.Visit(this.resolver, nc);
                    break;

                case AstNoOpNode np:
                    break;

                default:
                    throw new AstWalkerException($"Unhandled SymbolResolver type {node.GetType()}");
            }
        }
    }
}
