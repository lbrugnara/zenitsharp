// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Fl.Engine.Symbols;
using Fl.Parser.Ast;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Fl.Engine.Evaluators
{
    public class AstEvaluator : IAstWalker<FlObject>
    {
        private SymbolTable _Symbols;
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
        private NullCoalescingNodeEvaluator _NullCoalescingNodeEvaluator;

        public AstEvaluator()
        {
            _Symbols = new SymbolTable();
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
            _ReturnNodeEvaluator = new ReturnNodeEvaluator();
            _NullCoalescingNodeEvaluator = new NullCoalescingNodeEvaluator();
        }

        public SymbolTable Symtable => _Symbols;

        public FlObject Process(AstNode node)
        {
            object n = node;
            switch (n)
            {
                case AstUnaryPrefixNode uprefix:
                    return _UnaryPrefixNodeEvaluator.Evaluate(this, uprefix);
                case AstUnaryPostfixNode upostfix:
                    return _UnaryPostfixNodeEvaluator.Evaluate(this, upostfix);
                case AstUnaryNode u:
                    return _UnaryNodeEvaluator.Evaluate(this, u);                
                case AstBinaryNode b:
                    return _BinaryNodeEvaluator.Evaluate(this, b);
                case AstAssignmentNode a:
                    return _AssignmentNodeEvaluator.Evaluate(this, a);
                case AstConstantNode c:
                    return _ConstantNodeEvaluator.Evaluate(this, c);
                case AstVariableNode v:
                    return _VariableNodeEvaluator.Evaluate(this, v);
                case AstBlockNode bl:
                    return _BlockNodeEvaluator.Evaluate(this, bl);
                case AstDeclarationNode d:
                    return _DeclarationNodeEvaluator.Evaluate(this, d);
                case AstLiteralNode l:
                    return _LiteralNodeEvaluator.Evaluate(this, l);
                case AstAccessorNode ivk:
                    return _AccessorNodeEvaluator.Evaluate(this, ivk);
                case AstIfNode i:
                    return _IfNodeEvaluator.Evaluate(this, i);
                case AstWhileNode w:
                    return _WhileNodeEvaluator.Evaluate(this, w);
                case AstForNode f:
                    return _ForNodeEvaluator.Evaluate(this, f);
                case AstBreakNode brk:
                    return _BreakNodeEvaluator.Evaluate(this, brk);
                case AstContinueNode cont:
                    return _ContinueNodeEvaluator.Evaluate(this, cont);
                case AstCallableNode call:
                    return _CallNodeEvaluator.Evaluate(this, call);
                case AstFuncDeclNode func:
                    return _FuncDeclNodeEvaluator.Evaluate(this, func);
                case AstReturnNode ret:
                    return _ReturnNodeEvaluator.Evaluate(this, ret);
                case AstNullCoalescingNode nc:
                    return _NullCoalescingNodeEvaluator.Evaluate(this, nc);
                case AstNoOpNode np:
                    return null;
            }
            throw new AstWalkerException($"Unhandled type {node.GetType()}");
        }
    }
}
