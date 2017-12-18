// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Fl.Engine.IL.Instructions;
using Fl.Engine.IL.Instructions.Operands;
using Fl.Engine.Symbols;
using Fl.Engine.Symbols.Objects;
using Fl.Engine.Symbols.Types;
using System.Collections.Generic;
using System.Linq;

namespace Fl.Engine.IL.EValuators
{
    public class ILProgram
    {
        public List<FlObject> Params { get; } = new List<FlObject>();
        public SymbolTable SymbolTable { get; }
        public List<Instruction> Instructions { get; }
        

        public ILProgram(SymbolTable symtable, List<Instruction> instructions)
        {
            this.SymbolTable = SymbolTable.NewInstance;
            this.Instructions = instructions;
        }
        
        public override string ToString()
        {
            return string.Join('\n', Instructions.Select(i => i.ToString()));
        }

        public void Run()
        {
            foreach (var inst in Instructions)
            {
                switch (inst)
                {
                    case NopInstruction ni:
                        break;

                    case VarInstruction vi:
                        VisitVar(vi);
                        break;

                    case ConstInstruction ci:
                        VisitConst(ci);
                        break;

                    case StoreInstruction li:
                        VisitLoad(li);
                        break;

                    case CallInstruction ci:
                        VisitCall(ci);
                        break;

                    case ParamInstruction pi:
                        VisitParam(pi);
                        break;

                    case BinaryInstruction bi:
                        VisitBinary(bi);
                        break;

                    case UnaryInstruction ui:
                        VisitUnary(ui);
                        break;
                }
            }
        }

        protected FlObject GetFlObjectFromOperand(Operand o)
        {
            FlObject value = FlNull.Value;

            if (o == null)
                return value;

            if (o is SymbolOperand)
            {
                var so = o as SymbolOperand;

                // Get the root value
                value = SymbolTable.GetSymbol(so.Name).Binding;

                if (so.Member != null)
                {
                    FlObject tmpval = value;
                    SymbolOperand tmpmember = (o as SymbolOperand).Member;
                    while (tmpmember != null)
                    {
                        if (tmpval.ObjectType == NamespaceType.Value)
                        {
                            tmpval = (tmpval as FlNamespace)[tmpmember.Name].Binding;
                        }
                        tmpmember = tmpmember.Member;
                    }
                    value = tmpval;
                }
            }
            else
            {
                var io = (o as ImmediateOperand);
                value = ObjectType.GetFromTypeName(io.TypeName).NewValue(io.Value);
            }
            return value;
        }

        protected void VisitBinary(BinaryInstruction bi)
        {
            FlObject left = this.GetFlObjectFromOperand(bi.Left);
            FlObject right = this.GetFlObjectFromOperand(bi.Right);

            FlObject result = null; 
            switch (bi.OpCode)
            {
                case OpCode.Add:
                    result = left.Add(right);
                    break;
                case OpCode.Sub:
                    result = left.Subtract(right);
                    break;
                case OpCode.Mult:
                    result = left.Multiply(right);
                    break;
                case OpCode.Div:
                    result = left.Divide(right);
                    break;

                case OpCode.And:
                case OpCode.Or:
                    {
                        bool l = left.ObjectType != BoolType.Value ? left.RawValue != null : (left as FlBool).Value;
                        bool r = right.ObjectType != BoolType.Value ? right.RawValue != null : (right as FlBool).Value;

                        switch (bi.OpCode)
                        {
                            case OpCode.And:
                                result = new FlBool(l && r);
                                break;
                            case OpCode.Or:
                                result = new FlBool(l || r);
                                break;
                        }
                        break;
                    }

                case OpCode.Ceq:
                    result = left.Equals(right);
                    break;
                case OpCode.Cgt:
                    result = left.GreatherThan(right);
                    break;
                case OpCode.Cgte:
                    result = left.GreatherThanEquals(right);
                    break;
                case OpCode.Clt:
                    result = left.LesserThan(right);
                    break;
                case OpCode.Clte:
                    result = left.LesserThanEquals(right);
                    break;
                default:
                    throw new System.Exception($"Unhandled binary operator");
            }
            SymbolTable.GetSymbol(bi.DestSymbol.Name).UpdateBinding(result);
        }

        public void VisitUnary(UnaryInstruction ui)
        {
            FlObject left = this.GetFlObjectFromOperand(ui.Left);

            FlObject result = null; 
            switch (ui.OpCode)
            {
                case OpCode.Not:
                    result = left.Not();
                    break;
                case OpCode.Neg:
                    result = left.Negate();
                    break;
                case OpCode.PreInc:
                    result = left.PreIncrement();
                    break;
                case OpCode.PreDec:
                    result = left.PreDecrement();
                    break;
                case OpCode.PostInc:
                    result = left.PostIncrement();
                    break;
                case OpCode.PostDec:
                    result = left.PostDecrement();
                    break;
                default:
                    throw new System.Exception($"Unhandled unary operator");
            }
            SymbolTable.GetSymbol(ui.DestSymbol.Name).UpdateBinding(result);
        }

        public void VisitLoad(StoreInstruction li)
        {
            FlObject value = this.GetFlObjectFromOperand(li.Value);
            SymbolTable.GetSymbol(li.DestSymbol.Name).UpdateBinding(value);
        }

        public void VisitVar(VarInstruction vi)
        {
            FlObject value = this.GetFlObjectFromOperand(vi.Value);
            SymbolTable.AddSymbol(vi.DestSymbol.Name, new Symbol(SymbolType.Variable), value);
        }

        public void VisitConst(ConstInstruction ci)
        {
            FlObject value = this.GetFlObjectFromOperand(ci.Value);
            SymbolTable.AddSymbol(ci.DestSymbol.Name, new Symbol(SymbolType.Constant), value);
        }

        public void VisitCall(CallInstruction ci)
        {
            FlObject target = GetFlObjectFromOperand(ci.Func);
            List<FlObject> parameters = new List<FlObject>(Params);
            parameters.Reverse();

            /*if (node is AstIndexerNode)
            {
                Symbol clasz = evaluator.Symtable.GetSymbol(target.ObjectType.ClassName) ?? evaluator.Symtable.GetSymbol(target.ObjectType.Name);
                var claszobj = (clasz.Binding as FlClass);
                FlIndexer indexer = claszobj.GetIndexer(node.Arguments.Count);
                if (indexer == null)
                    throw new AstWalkerException($"{claszobj} does not contain an indexer that accepts {node.Arguments.Count} {(node.Arguments.Count == 1 ? "argument" : "arguments")}");
                return indexer.Bind(target).Invoke(evaluator.Symtable, node.Arguments.Expressions.Select(e => e.Exec(evaluator)).ToList());
            }
            */

            if (target is FlFunction)
            {
                (target as FlFunction).Invoke(SymbolTable, Params);
            }/*
            else if (target is FlClass)
            {
                var clasz = (target as FlClass);

                if (node.New != null)
                {
                    return clasz.InvokeConstructor(node.Arguments.Expressions.Select(a => a.Exec(evaluator)).ToList());
                }
                else
                {
                    target = clasz.StaticConstructor ?? throw new AstWalkerException($"{target} does not contain a definition for the static constructor");
                }
            }*/
            //throw new AstWalkerException($"{target} is not a callable object");

            Params.Clear();
        }

        public void VisitParam(ParamInstruction pi)
        {
            Params.Add(GetFlObjectFromOperand(pi.Parameter));
        }
    }
}
