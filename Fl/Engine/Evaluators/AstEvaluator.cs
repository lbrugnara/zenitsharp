// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Fl.Engine.Symbols;
using Fl.Engine.Symbols.Objects;
using Fl.Parser.Ast;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Fl.Engine.Evaluators
{
    public class AstEvaluator : IAstWalker<FlObject>
    {
        private AstNodeEvaluator _AstNodeEvaluator;
        private UnaryNodeEvaluator _UnaryNodeEvaluator;
        private UnaryPrefixNodeEvaluator _UnaryPrefixNodeEvaluator;
        private UnaryPostfixNodeEvaluator _UnaryPostfixNodeEvaluator;
        private BinaryNodeEvaluator _BinaryNodeEvaluator;
        private AssignmentNodeEvaluator _AssignmentNodeEvaluator;
        private ConstantNodeEvaluator _ConstantNodeEvaluator;
        private VariableNodeEvaluator _VariableNodeEvaluator;
        private BlockNodeEvaluator _BlockNodeEvaluator;
        private DeclarationNodeEvaluator _DeclarationNodeEvaluator;
        private LiteralNodeEvaluator _LiteralNodeEvaluator;
        private AccessorNodeEvaluator _AccessorNodeEvaluator;
        private IfNodeEvaluator _IfNodeEvaluator;
        private WhileNodeEvaluator _WhileNodeEvaluator;
        private ForNodeEvaluator _ForNodeEvaluator;
        private BreakNodeEvaluator _BreakNodeEvaluator;
        private ContinueNodeEvaluator _ContinueNodeEvaluator;
        private ReturnNodeEvaluator _ReturnNodeEvaluator;
        private CallableNodeEvaluator _CallNodeEvaluator;
        private FuncDeclNodeEvaluator _FuncDeclNodeEvaluator;
        private TupleNodeEvaluator _TupleNodeEvaluator;
        private NullCoalescingNodeEvaluator _NullCoalescingNodeEvaluator;

        public AstEvaluator()
        {
            _AstNodeEvaluator = new AstNodeEvaluator();
            _UnaryNodeEvaluator = new UnaryNodeEvaluator();
            _UnaryPrefixNodeEvaluator = new UnaryPrefixNodeEvaluator();
            _UnaryPostfixNodeEvaluator = new UnaryPostfixNodeEvaluator();
            _BinaryNodeEvaluator = new BinaryNodeEvaluator();
            _AssignmentNodeEvaluator = new AssignmentNodeEvaluator();
            _ConstantNodeEvaluator = new ConstantNodeEvaluator();
            _VariableNodeEvaluator = new VariableNodeEvaluator();
            _BlockNodeEvaluator = new BlockNodeEvaluator();
            _DeclarationNodeEvaluator = new DeclarationNodeEvaluator();
            _LiteralNodeEvaluator = new LiteralNodeEvaluator();
            _AccessorNodeEvaluator = new AccessorNodeEvaluator();
            _IfNodeEvaluator = new IfNodeEvaluator();
            _WhileNodeEvaluator = new WhileNodeEvaluator();
            _ForNodeEvaluator = new ForNodeEvaluator();
            _BreakNodeEvaluator = new BreakNodeEvaluator();
            _ContinueNodeEvaluator = new ContinueNodeEvaluator();
            _CallNodeEvaluator = new CallableNodeEvaluator();
            _FuncDeclNodeEvaluator = new FuncDeclNodeEvaluator();
            _TupleNodeEvaluator = new TupleNodeEvaluator();
            _ReturnNodeEvaluator = new ReturnNodeEvaluator();
            _NullCoalescingNodeEvaluator = new NullCoalescingNodeEvaluator();
        }

        public SymbolTable Symtable => SymbolTable.Instance;

        public FlObject Process(AstNode node)
        {
            object n = node;
            switch (n)
            {
                case AstUnaryPrefixNode uprefix:
                    return _UnaryPrefixNodeEvaluator.Visit(this, uprefix);
                case AstUnaryPostfixNode upostfix:
                    return _UnaryPostfixNodeEvaluator.Visit(this, upostfix);
                case AstUnaryNode u:
                    return _UnaryNodeEvaluator.Visit(this, u);                
                case AstBinaryNode b:
                    return _BinaryNodeEvaluator.Visit(this, b);
                case AstAssignmentNode a:
                    return _AssignmentNodeEvaluator.Visit(this, a);
                case AstConstantNode c:
                    return _ConstantNodeEvaluator.Visit(this, c);
                case AstVariableNode v:
                    return _VariableNodeEvaluator.Visit(this, v);
                case AstBlockNode bl:
                    return _BlockNodeEvaluator.Visit(this, bl);
                case AstDeclarationNode d:
                    return _DeclarationNodeEvaluator.Visit(this, d);
                case AstLiteralNode l:
                    return _LiteralNodeEvaluator.Visit(this, l);
                case AstAccessorNode ivk:
                    return _AccessorNodeEvaluator.Visit(this, ivk);
                case AstIfNode i:
                    return _IfNodeEvaluator.Visit(this, i);
                case AstWhileNode w:
                    return _WhileNodeEvaluator.Visit(this, w);
                case AstForNode f:
                    return _ForNodeEvaluator.Visit(this, f);
                case AstBreakNode brk:
                    return _BreakNodeEvaluator.Visit(this, brk);
                case AstContinueNode cont:
                    return _ContinueNodeEvaluator.Visit(this, cont);
                case AstCallableNode call:
                    return _CallNodeEvaluator.Visit(this, call);
                case AstFuncDeclNode func:
                    return _FuncDeclNodeEvaluator.Visit(this, func);
                case AstTupleNode t:
                    return _TupleNodeEvaluator.Visit(this, t);
                case AstReturnNode ret:
                    return _ReturnNodeEvaluator.Visit(this, ret);
                case AstNullCoalescingNode nc:
                    return _NullCoalescingNodeEvaluator.Visit(this, nc);
                case AstNoOpNode np:
                    return null;
            }
            throw new AstWalkerException($"Unhandled type {node.GetType()}");
        }
    }
}
