﻿// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Fl.Ast;
using Fl.Semantics.Inferrers;
using Fl.Semantics.Symbols;

namespace Fl.Semantics.Checkers
{
    public class TypeCheckerVisitor : IAstWalker<CheckedType>
    {
        private UnaryTypeChecker unaryTypeChecker;
        private BinaryTypeChecker binaryTypeChecker;
        private AssignmentTypeChecker assignmentTypeChecker;
        private ConstantTypeChecker constantTypeChecker;
        private VariableTypeChecker variableTypeChecker;
        private BlockTypeChecker blockTypeChecker;
        private DeclarationTypeChecker declarationTypeChecker;
        private LiteralTypeChecker literalTypeChecker;
        private AccessorTypeChecker accessorTypeChecker;
        private IfTypeChecker ifTypeChecker;
        private WhileTypeChecker whileTypeChecker;
        private ForTypeChecker forTypeChecker;
        private BreakTypeChecker breakTypeChecker;
        private ContinueTypeChecker continueTypeChecker;
        private ReturnTypeChecker returnTypeChecker;
        private CallableTypeChecker callTypeChecker;
        private FunctionTypeChecker funcDeclTypeChecker;
        private TupleTypeChecker tupleTypeChecker;
        private NullCoalescingTypeChecker nullCoalescingTypeChecker;
        private ClassTypeChecker classTypeChecker;
        private ClassPropertyTypeChecker classPropertyTypeChecker;
        private ClassConstantTypeChecker classConstantTypeChecker;
        private ClassMethodTypeChecker classMethodTypeChecker;

        public TypeCheckerVisitor(SymbolTable symtable)
        {
            this.SymbolTable = symtable;
            this.unaryTypeChecker = new UnaryTypeChecker();
            this.binaryTypeChecker = new BinaryTypeChecker();
            this.assignmentTypeChecker = new AssignmentTypeChecker();
            this.constantTypeChecker = new ConstantTypeChecker();
            this.variableTypeChecker = new VariableTypeChecker();
            this.blockTypeChecker = new BlockTypeChecker();
            this.declarationTypeChecker = new DeclarationTypeChecker();
            this.literalTypeChecker = new LiteralTypeChecker();
            this.accessorTypeChecker = new AccessorTypeChecker();
            this.ifTypeChecker = new IfTypeChecker();
            this.whileTypeChecker = new WhileTypeChecker();
            this.forTypeChecker = new ForTypeChecker();
            this.breakTypeChecker = new BreakTypeChecker();
            this.continueTypeChecker = new ContinueTypeChecker();
            this.callTypeChecker = new CallableTypeChecker();
            this.funcDeclTypeChecker = new FunctionTypeChecker();
            this.tupleTypeChecker = new TupleTypeChecker();
            this.returnTypeChecker = new ReturnTypeChecker();
            this.nullCoalescingTypeChecker = new NullCoalescingTypeChecker();
            this.classTypeChecker = new ClassTypeChecker();
            this.classPropertyTypeChecker = new ClassPropertyTypeChecker();
            this.classConstantTypeChecker = new ClassConstantTypeChecker();
            this.classMethodTypeChecker = new ClassMethodTypeChecker();
        }

        /// <summary>
        /// Tracks variables per blocks
        /// </summary>
        public SymbolTable SymbolTable { get; private set; }

        public CheckedType Visit(AstNode node)
        {
            object n = node;
            switch (n)
            {
                case AstUnaryNode u:
                    return this.unaryTypeChecker.Visit(this, u);

                case AstBinaryNode b:
                    return this.binaryTypeChecker.Visit(this, b);

                case AstAssignmentNode a:
                    return this.assignmentTypeChecker.Visit(this, a);

                case AstConstantNode c:
                    return this.constantTypeChecker.Visit(this, c);

                case AstVariableNode v:
                    return this.variableTypeChecker.Visit(this, v);

                case AstBlockNode bl:
                    return this.blockTypeChecker.Visit(this, bl);

                case AstDeclarationNode d:
                    return this.declarationTypeChecker.Visit(this, d);

                case AstLiteralNode l:
                    return this.literalTypeChecker.Visit(this, l);

                case AstAccessorNode ivk:
                    return this.accessorTypeChecker.Visit(this, ivk);

                case AstIfNode i:
                    return this.ifTypeChecker.Visit(this, i);

                case AstWhileNode w:
                    return this.whileTypeChecker.Visit(this, w);

                case AstForNode f:
                    return this.forTypeChecker.Visit(this, f);

                case AstBreakNode brk:
                    return this.breakTypeChecker.Visit(this, brk);

                case AstContinueNode cont:
                    return this.continueTypeChecker.Visit(this, cont);

                case AstCallableNode call:
                    return this.callTypeChecker.Visit(this, call);

                case AstFunctionNode func:
                    return this.funcDeclTypeChecker.Visit(this, func);

                case AstTupleNode t:
                    return this.tupleTypeChecker.Visit(this, t);

                case AstReturnNode ret:
                    return this.returnTypeChecker.Visit(this, ret);

                case AstNullCoalescingNode nc:
                    return this.nullCoalescingTypeChecker.Visit(this, nc);

                case AstClassNode cn:
                    return this.classTypeChecker.Visit(this, cn);

                case AstClassPropertyNode cpn:
                    return this.classPropertyTypeChecker.Visit(this, cpn);

                case AstClassConstantNode ccn:
                    return this.classConstantTypeChecker.Visit(this, ccn);

                case AstClassMethodNode cmn:
                    return this.classMethodTypeChecker.Visit(this, cmn);

                case AstNoOpNode np:
                    return null;
            }
            throw new AstWalkerException($"Unhandled type {node.GetType()}");
        }
    }
}