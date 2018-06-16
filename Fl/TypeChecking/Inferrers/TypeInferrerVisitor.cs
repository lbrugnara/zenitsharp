// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Fl.Symbols;
using Fl.Ast;

namespace Fl.TypeChecking.Inferrers
{
    public class TypeInferrerVisitor : IAstWalker<InferredType>
    {
        private UnaryTypeInferrer unaryTypeInferencer;
        private BinaryTypeInferrer binaryTypeInferencer;
        private AssignmentTypeInferrer assignmentTypeInferencer;
        private ConstantTypeInferrer constantTypeInferencer;
        private VariableTypeInferrer variableTypeInferencer;
        private BlockTypeInferrer blockTypeInferencer;
        private DeclarationTypeInferrer declarationTypeInferencer;
        private LiteralTypeInferrer literalTypeInferencer;
        private AccessorTypeInferrer accessorTypeInferencer;
        private IfTypeInferrer ifTypeInferencer;
        private WhileTypeInferrer whileTypeInferencer;
        private ForTypeInferrer forTypeInferencer;
        private BreakTypeInferrer breakTypeInferencer;
        private ContinueTypeInferrer continueTypeInferencer;
        private ReturnTypeInferrer returnTypeInferencer;
        private CallableTypeInferrer callTypeInferencer;
        private FuncDeclTypeInferrer funcDeclTypeInferencer;
        private TupleTypeInferrer tupleTypeInferencer;
        private NullCoalescingTypeInferrer nullCoalescingTypeInferencer;

        public TypeInferrerVisitor(SymbolTable symtable)
        {
            this.SymbolTable = symtable;
            this.Inferrer = new TypeInferrer();
            this.unaryTypeInferencer = new UnaryTypeInferrer();
            this.binaryTypeInferencer = new BinaryTypeInferrer();
            this.assignmentTypeInferencer = new AssignmentTypeInferrer();
            this.constantTypeInferencer = new ConstantTypeInferrer();
            this.variableTypeInferencer = new VariableTypeInferrer();
            this.blockTypeInferencer = new BlockTypeInferrer();
            this.declarationTypeInferencer = new DeclarationTypeInferrer();
            this.literalTypeInferencer = new LiteralTypeInferrer();
            this.accessorTypeInferencer = new AccessorTypeInferrer();
            this.ifTypeInferencer = new IfTypeInferrer();
            this.whileTypeInferencer = new WhileTypeInferrer();
            this.forTypeInferencer = new ForTypeInferrer();
            this.breakTypeInferencer = new BreakTypeInferrer();
            this.continueTypeInferencer = new ContinueTypeInferrer();
            this.callTypeInferencer = new CallableTypeInferrer();
            this.funcDeclTypeInferencer = new FuncDeclTypeInferrer();
            this.tupleTypeInferencer = new TupleTypeInferrer();
            this.returnTypeInferencer = new ReturnTypeInferrer();
            this.nullCoalescingTypeInferencer = new NullCoalescingTypeInferrer();
        }

        /// <summary>
        /// Tracks variables per blocks
        /// </summary>
        public SymbolTable SymbolTable { get; private set; }

        public TypeInferrer Inferrer { get; private set; }

        public InferredType Visit(AstNode node)
        {
            object n = node;
            switch (n)
            {
                case AstUnaryNode u:
                    return this.unaryTypeInferencer.Visit(this, u);
                case AstBinaryNode b:
                    return this.binaryTypeInferencer.Visit(this, b);
                case AstAssignmentNode a:
                    return this.assignmentTypeInferencer.Visit(this, a);
                case AstConstantNode c:
                    return this.constantTypeInferencer.Visit(this, c);
                case AstVariableNode v:
                    return this.variableTypeInferencer.Visit(this, v);
                case AstBlockNode bl:
                    return this.blockTypeInferencer.Visit(this, bl);
                case AstDeclarationNode d:
                    return this.declarationTypeInferencer.Visit(this, d);
                case AstLiteralNode l:
                    return this.literalTypeInferencer.Visit(this, l);
                case AstAccessorNode ivk:
                    return this.accessorTypeInferencer.Visit(this, ivk);
                case AstIfNode i:
                    return this.ifTypeInferencer.Visit(this, i);
                case AstWhileNode w:
                    return this.whileTypeInferencer.Visit(this, w);
                case AstForNode f:
                    return this.forTypeInferencer.Visit(this, f);
                case AstBreakNode brk:
                    return this.breakTypeInferencer.Visit(this, brk);
                case AstContinueNode cont:
                    return this.continueTypeInferencer.Visit(this, cont);
                case AstCallableNode call:
                    return this.callTypeInferencer.Visit(this, call);
                case AstFuncDeclNode func:
                    return this.funcDeclTypeInferencer.Visit(this, func);
                case AstTupleNode t:
                    return this.tupleTypeInferencer.Visit(this, t);
                case AstReturnNode ret:
                    return this.returnTypeInferencer.Visit(this, ret);
                case AstNullCoalescingNode nc:
                    return this.nullCoalescingTypeInferencer.Visit(this, nc);
                case AstNoOpNode np:
                    return null;
            }
            throw new AstWalkerException($"Unhandled type {node.GetType()}");
        }
    }
}
