// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Fl.Ast;
using Fl.Semantics.Symbols;

namespace Fl.Semantics.Inferrers
{
    public class TypeInferrerVisitor : IAstWalker<InferredType>
    {
        private UnaryTypeInferrer unaryTypeInferrer;
        private BinaryTypeInferrer binaryTypeInferrer;
        private AssignmentTypeInferrer assignmentTypeInferrer;
        private ConstantTypeInferrer constantTypeInferrer;
        private VariableTypeInferrer variableTypeInferrer;
        private BlockTypeInferrer blockTypeInferrer;
        private DeclarationTypeInferrer declarationTypeInferrer;
        private LiteralTypeInferrer literalTypeInferrer;
        private AccessorTypeInferrer accessorTypeInferrer;
        private IfTypeInferrer ifTypeInferrer;
        private WhileTypeInferrer whileTypeInferrer;
        private ForTypeInferrer forTypeInferrer;
        private BreakTypeInferrer breakTypeInferrer;
        private ContinueTypeInferrer continueTypeInferrer;
        private ReturnTypeInferrer returnTypeInferrer;
        private CallableTypeInferrer callTypeInferrer;
        private FunctionTypeInferrer funcDeclTypeInferrer;
        private TupleTypeInferrer tupleTypeInferrer;
        private NullCoalescingTypeInferrer nullCoalescingTypeInferrer;
        private ClassTypeInferrer classTypeInferrer;
        private ClassPropertyTypeInferrer classPropertyTypeInferrer;
        private ClassConstantTypeInferrer classConstantTypeInferrer;
        private ClassMethodTypeInferrer classMethodTypeInferrer;

        public TypeInferrerVisitor(SymbolTable symtable, TypeInferrer inferrer)
        {
            this.SymbolTable = symtable;
            this.Inferrer = inferrer;
            this.unaryTypeInferrer = new UnaryTypeInferrer();
            this.binaryTypeInferrer = new BinaryTypeInferrer();
            this.assignmentTypeInferrer = new AssignmentTypeInferrer();
            this.constantTypeInferrer = new ConstantTypeInferrer();
            this.variableTypeInferrer = new VariableTypeInferrer();
            this.blockTypeInferrer = new BlockTypeInferrer();
            this.declarationTypeInferrer = new DeclarationTypeInferrer();
            this.literalTypeInferrer = new LiteralTypeInferrer();
            this.accessorTypeInferrer = new AccessorTypeInferrer();
            this.ifTypeInferrer = new IfTypeInferrer();
            this.whileTypeInferrer = new WhileTypeInferrer();
            this.forTypeInferrer = new ForTypeInferrer();
            this.breakTypeInferrer = new BreakTypeInferrer();
            this.continueTypeInferrer = new ContinueTypeInferrer();
            this.callTypeInferrer = new CallableTypeInferrer();
            this.funcDeclTypeInferrer = new FunctionTypeInferrer();
            this.tupleTypeInferrer = new TupleTypeInferrer();
            this.returnTypeInferrer = new ReturnTypeInferrer();
            this.nullCoalescingTypeInferrer = new NullCoalescingTypeInferrer();
            this.classTypeInferrer = new ClassTypeInferrer();
            this.classPropertyTypeInferrer = new ClassPropertyTypeInferrer();
            this.classConstantTypeInferrer = new ClassConstantTypeInferrer();
            this.classMethodTypeInferrer = new ClassMethodTypeInferrer();
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
                    return this.unaryTypeInferrer.Visit(this, u);

                case AstBinaryNode b:
                    return this.binaryTypeInferrer.Visit(this, b);

                case AstAssignmentNode a:
                    return this.assignmentTypeInferrer.Visit(this, a);

                case AstConstantNode c:
                    return this.constantTypeInferrer.Visit(this, c);

                case AstVariableNode v:
                    return this.variableTypeInferrer.Visit(this, v);

                case AstBlockNode bl:
                    return this.blockTypeInferrer.Visit(this, bl);

                case AstDeclarationNode d:
                    return this.declarationTypeInferrer.Visit(this, d);

                case AstLiteralNode l:
                    return this.literalTypeInferrer.Visit(this, l);

                case AstAccessorNode ivk:
                    return this.accessorTypeInferrer.Visit(this, ivk);

                case AstIfNode i:
                    return this.ifTypeInferrer.Visit(this, i);

                case AstWhileNode w:
                    return this.whileTypeInferrer.Visit(this, w);

                case AstForNode f:
                    return this.forTypeInferrer.Visit(this, f);

                case AstBreakNode brk:
                    return this.breakTypeInferrer.Visit(this, brk);

                case AstContinueNode cont:
                    return this.continueTypeInferrer.Visit(this, cont);

                case AstCallableNode call:
                    return this.callTypeInferrer.Visit(this, call);

                case AstFunctionNode func:
                    return this.funcDeclTypeInferrer.Visit(this, func);

                case AstTupleNode t:
                    return this.tupleTypeInferrer.Visit(this, t);

                case AstReturnNode ret:
                    return this.returnTypeInferrer.Visit(this, ret);

                case AstNullCoalescingNode nc:
                    return this.nullCoalescingTypeInferrer.Visit(this, nc);

                case AstClassNode cn:
                    return this.classTypeInferrer.Visit(this, cn);

                case AstClassPropertyNode cpn:
                    return this.classPropertyTypeInferrer.Visit(this, cpn);

                case AstClassConstantNode ccn:
                    return this.classConstantTypeInferrer.Visit(this, ccn);

                case AstClassMethodNode cmn:
                    return this.classMethodTypeInferrer.Visit(this, cmn);

                case AstNoOpNode np:
                    return null;
            }
            throw new AstWalkerException($"Unhandled type {node.GetType()}");
        }
    }
}
