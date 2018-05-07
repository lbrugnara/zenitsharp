// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Fl.Parser.Ast;
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

        public Symbol Visit(AstNode node)
        {
            object n = node;
            switch (n)
            {
                case AstUnaryNode u:
                    return this.unarySymbolResolver.Visit(this.resolver, u);
                case AstBinaryNode b:
                    return this.binarySymbolResolver.Visit(this.resolver, b);
                case AstAssignmentNode a:
                    return this.assignmentSymbolResolver.Visit(this.resolver, a);
                case AstConstantNode c:
                    return this.constantSymbolResolver.Visit(this.resolver, c);
                case AstVariableNode v:
                    return this.variableSymbolResolver.Visit(this.resolver, v);
                case AstBlockNode bl:
                    return this.blockSymbolResolver.Visit(this.resolver, bl);
                case AstDeclarationNode d:
                    return this.declarationSymbolResolver.Visit(this.resolver, d);
                case AstLiteralNode l:
                    return this.literalSymbolResolver.Visit(this.resolver, l);
                case AstAccessorNode ivk:
                    return this.accessorSymbolResolver.Visit(this.resolver, ivk);
                case AstIfNode i:
                    return this.ifSymbolResolver.Visit(this.resolver, i);
                case AstWhileNode w:
                    return this.whileSymbolResolver.Visit(this.resolver, w);
                case AstForNode f:
                    return this.forSymbolResolver.Visit(this.resolver, f);
                case AstBreakNode brk:
                    return this.breakSymbolResolver.Visit(this.resolver, brk);
                case AstContinueNode cont:
                    return this.continueSymbolResolver.Visit(this.resolver, cont);
                case AstCallableNode call:
                    return this.callSymbolResolver.Visit(this.resolver, call);
                case AstFuncDeclNode func:
                    return this.funcDeclSymbolResolver.Visit(this.resolver, func);
                case AstTupleNode t:
                    return this.tupleSymbolResolver.Visit(this.resolver, t);
                case AstReturnNode ret:
                    return this.returnSymbolResolver.Visit(this.resolver, ret);
                case AstNullCoalescingNode nc:
                    return this.nullCoalescingSymbolResolver.Visit(this.resolver, nc);
                case AstNoOpNode np:
                    return null;
            }
            throw new AstWalkerException($"Unhandled type {node.GetType()}");
        }
    }
}
