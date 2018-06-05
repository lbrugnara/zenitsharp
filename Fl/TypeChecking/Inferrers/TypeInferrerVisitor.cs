// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Fl.Symbols;
using Fl.Ast;
using Fl.Lang.Types;
using System.Collections.Generic;
using System.Linq;

namespace Fl.TypeChecking.Inferrers
{
    public class Constraints
    {
        private Dictionary<string, List<Symbol>> constraints;

        private int[] names = new int[] { -1, -1, -1, -1, -1, -1, -1 };

        public Constraints()
        {
            this.constraints = new Dictionary<string, List<Symbol>>();
        }

        public void AddConstraint(Symbol s)
        {
            var name = s.Type.ToString();

            if (!this.constraints.ContainsKey(name))
                this.constraints[s.Type.ToString()] = new List<Symbol>();

            this.constraints[name].Add(s);
        }

        public bool HasConstraints(Symbol s) => this.constraints.ContainsKey(s.Type.ToString());

        public void ResolveConstraint(Symbol symbol, Type t)
        {
            var prevInferredType = symbol.Type.ToString();

            this.constraints[prevInferredType].ForEach(s => s.Type = t);

            if (t is Anonymous)
                this.constraints[t.ToString()].AddRange(this.constraints[prevInferredType]);

            this.constraints.Remove(prevInferredType);
        }

        public Anonymous AssignTemporalType(Symbol s)
        {            
            var tempType = new Anonymous(this.GetName());
            s.Type = tempType;

            var tempName = tempType.ToString();

            if (!this.constraints.ContainsKey(tempName))
                this.constraints[tempName] = new List<Symbol>();

            this.constraints[tempName].Add(s);

            return tempType;
        }

        public void InferTypeAs(Anonymous prevInferredType, Type newInferredType)
        {
            this.constraints[prevInferredType.ToString()].ForEach(s => s.Type = newInferredType);

            if (newInferredType is Anonymous)
                this.constraints[newInferredType.ToString()].AddRange(this.constraints[prevInferredType.ToString()]);

            this.constraints.Remove(prevInferredType.ToString());
        }

        private void RestoreName()
        {
            // Poor's man type name generator
            for (var i = this.names.Length - 1; i >= 0; i--)
            {
                if (this.names[i] >= 0)
                {
                    this.names[i] -= 1;
                    return;
                }
                continue;
            }
            throw new System.Exception("Carry Underflow");
        }

        private string GetName()
        {
            // Poor's man type name generator
            for (var i=0; i < this.names.Length; i++)
            {
                if (this.names[i] < 25)
                {
                    this.names[i] += 1;
                    return string.Join("", this.names.Where(n => n > -1).Select(n => (char)('A' + n)));
                }
                this.names[i] = 0;
            }
            throw new System.Exception("Carry Overflow");
        }
    }

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
            this.Constraints = new Constraints();
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

        public Constraints Constraints { get; private set; }

        // Adds a new block to the SymbolTable, it represents a new scope
        public void EnterBlock(BlockType type, string name)
        {
            this.SymbolTable.EnterBlock(type, name);
        }

        // Leave the current block in the SymbolTable
        public void LeaveBlock()
        {
            this.SymbolTable.LeaveBlock();
        }

        // Returns true if the current fragment is a function fragment
        public bool InFunction => this.SymbolTable.CurrentBlock.Type == BlockType.Function;

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
