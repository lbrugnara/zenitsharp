// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Fl.Engine.IL.Instructions;
using Fl.Engine.IL.Instructions.Exceptions;
using Fl.Engine.IL.Instructions.Operands;
using Fl.Engine.Symbols;
using Fl.Engine.Symbols.Objects;
using Fl.Engine.Symbols.Types;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace Fl.Engine.IL.VM
{
    public class Frame
    {
        public Stack<FlObject> Parameters { get; }
        public FlObject ReturnValue { get; set; }

        public Frame()
        {
            Parameters = new Stack<FlObject>();
        }

        public Frame(Stack<FlObject> parameters)
        {
            Parameters = parameters;
        }
    }

    public class ILProgram
    {
        public SymbolTable SymbolTable { get; }
        public ReadOnlyDictionary<string, Fragment> Fragments { get; }
        public Stack<Frame> Frames { get; }

        public ILProgram(SymbolTable symtable, List<Fragment> fragments)
        {
            this.SymbolTable = SymbolTable.NewInstance;
            this.Fragments = new ReadOnlyDictionary<string, Fragment>(fragments.ToDictionary(fragment => fragment.Name, fragment => fragment));
            this.Frames = new Stack<Frame>();
        }

        public Frame Frame => Frames.Peek();

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            foreach (var fragment in Fragments.Values)
            {
                if (fragment.Type == FragmentType.Global)
                    continue;
                sb.AppendLine(fragment.ToString());
            }

            if (Fragments.ContainsKey(".global"))
                sb.AppendLine(Fragments[".global"].ToString());
            else
                sb.AppendLine(".global:");

            return sb.ToString();
        }

        public void Run()
        {
            var global = Fragments[".global"];
            RegisterFunctions();
            Frames.Push(new Frame());
            Run(global);
        }

        private void RegisterFunctions()
        {
            foreach (var fragment in Fragments.Values)
            {
                if (fragment.Type != FragmentType.Function)
                    continue;
                SymbolTable.AddSymbol(fragment.Name, new Symbol(SymbolType.Constant, StorageType.Static), new FlFunction(fragment.Name, (args) =>
                {
                    Run(fragment);
                    return Frame.ReturnValue;   
                }));
            }
        }

        private void Run(Fragment fragment)
        {
            for (int i = 0; i < fragment.Instructions.Count;)
            {
                Instruction inst = fragment.Instructions[i];
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

                    case LocalInstruction li:
                        if (fragment.Type != FragmentType.Function)
                            throw new InvalidInstructionException("Cannot declare local variable in a non-function fragment");
                        VisitLocal(li);
                        break;

                    case AddInstruction instr:
                        VisitAdd(instr);
                        break;

                    case SubInstruction instr:
                        VisitSub(instr);
                        break;

                    case MultInstruction instr:
                        VisitMult(instr);
                        break;

                    case DivInstruction instr:
                        VisitDiv(instr);
                        break;

                    case CeqInstruction instr:
                        VisitCeq(instr);
                        break;

                    case CgtInstruction instr:
                        VisitCgt(instr);
                        break;

                    case CgteInstruction instr:
                        VisitCgte(instr);
                        break;

                    case CltInstruction instr:
                        VisitClt(instr);
                        break;

                    case ClteInstruction instr:
                        VisitClte(instr);
                        break;

                    case AndInstruction instr:
                        VisitAnd(instr);
                        break;

                    case OrInstruction instr:
                        VisitOr(instr);
                        break;

                    case NotInstruction ins:
                        VisitNot(ins);
                        break;

                    case NegInstruction ins:
                        VisitNeg(ins);
                        break;

                    case PreIncInstruction ins:
                        VisitPreInc(ins);
                        break;

                    case PreDecInstruction ins:
                        VisitPreDec(ins);
                        break;

                    case PostIncInstruction ins:
                        VisitPostInc(ins);
                        break;

                    case PostDecInstruction ins:
                        VisitPostDec(ins);
                        break;

                    case IfFalseInstruction fi:
                        FlBool cond = GetFlObjectFromOperand(fi.Condition) as FlBool;
                        if (!cond.Value)
                        {
                            i = fi.Goto.Address;
                            continue;
                        }
                        break;

                    case GotoInstruction gi:
                        i = gi.Goto.Address;
                        continue;

                    case ReturnInstruction ri:
                        Frame.ReturnValue = GetFlObjectFromOperand(ri.DestSymbol);
                        i = fragment.Instructions.Count;
                        break;
                }

                i++;
            }
        }

        protected FlObject GetFlObjectFromOperand(Operand o)
        {
            FlObject value = FlNull.Value;

            if (o == null || o.TypeResolver?.TypeName == FlNullType.Instance.Name)
                return value;

            if (o is SymbolOperand)
            {
                var so = o as SymbolOperand;

                // Get the root value
                value = SymbolTable.GetSymbol(so.Name).Binding;
            }
            else
            {
                var io = (o as ImmediateOperand);
                FlType operandType = io.TypeResolver.Resolve(SymbolTable);                
                value = operandType.Activator(io.Value);
            }

            if (o.Member != null)
            {
                FlObject curval = value;
                FlObject ancestor = value;
                SymbolOperand tmpmember = o.Member;
                while (tmpmember != null)
                {
                    if (curval.Type == FlNamespaceType.Instance)
                    {
                        curval = (curval as FlNamespace)[tmpmember.Name].Binding;
                    }
                    else if (curval is FlType)
                    {
                        curval = (curval as FlType)[tmpmember.Name].Binding;
                    }
                    else
                    {
                        curval = curval.Type[tmpmember.Name].Binding;
                    }

                    if (curval is FlMethod)
                    {
                        curval = (curval as FlMethod).Bind(ancestor);
                    }
                    ancestor = curval;
                    tmpmember = tmpmember.Member;
                }
                value = curval;
            }

            return value;
        }


        protected void VisitAdd(AddInstruction bi)
        {
            FlObject left = this.GetFlObjectFromOperand(bi.Left);
            FlObject right = this.GetFlObjectFromOperand(bi.Right);
            FlObject result = left.Type.GetStaticMethod(FlType.OperatorAdd).Invoke(SymbolTable, left, right);
            SymbolTable.GetSymbol(bi.DestSymbol.Name).UpdateBinding(result);
        }

        protected void VisitSub(SubInstruction bi)
        {
            FlObject left = this.GetFlObjectFromOperand(bi.Left);
            FlObject right = this.GetFlObjectFromOperand(bi.Right);
            FlObject result = left.Type.GetStaticMethod(FlType.OperatorSub).Invoke(SymbolTable, left, right);
            SymbolTable.GetSymbol(bi.DestSymbol.Name).UpdateBinding(result);
        }

        protected void VisitMult(MultInstruction bi)
        {
            FlObject left = this.GetFlObjectFromOperand(bi.Left);
            FlObject right = this.GetFlObjectFromOperand(bi.Right);
            FlObject result = left.Type.GetStaticMethod(FlType.OperatorMult).Invoke(SymbolTable, left, right);
            SymbolTable.GetSymbol(bi.DestSymbol.Name).UpdateBinding(result);
        }

        protected void VisitDiv(DivInstruction bi)
        {
            FlObject left = this.GetFlObjectFromOperand(bi.Left);
            FlObject right = this.GetFlObjectFromOperand(bi.Right);
            FlObject result = left.Type.GetStaticMethod(FlType.OperatorDiv).Invoke(SymbolTable, left, right);
            SymbolTable.GetSymbol(bi.DestSymbol.Name).UpdateBinding(result);
        }

        protected void VisitCeq(CeqInstruction bi)
        {
            FlObject left = this.GetFlObjectFromOperand(bi.Left);
            FlObject right = this.GetFlObjectFromOperand(bi.Right);
            FlObject result = left.Type.GetStaticMethod(FlType.OperatorEquals).Invoke(SymbolTable, left, right);
            SymbolTable.GetSymbol(bi.DestSymbol.Name).UpdateBinding(result);
        }

        protected void VisitCgt(CgtInstruction bi)
        {
            FlObject left = this.GetFlObjectFromOperand(bi.Left);
            FlObject right = this.GetFlObjectFromOperand(bi.Right);
            FlObject result = left.Type.GetStaticMethod(FlType.OperatorGt).Invoke(SymbolTable, left, right);
            SymbolTable.GetSymbol(bi.DestSymbol.Name).UpdateBinding(result);
        }

        protected void VisitCgte(CgteInstruction bi)
        {
            FlObject left = this.GetFlObjectFromOperand(bi.Left);
            FlObject right = this.GetFlObjectFromOperand(bi.Right);
            FlObject result = left.Type.GetStaticMethod(FlType.OperatorGte).Invoke(SymbolTable, left, right);
            SymbolTable.GetSymbol(bi.DestSymbol.Name).UpdateBinding(result);
        }

        protected void VisitClt(CltInstruction bi)
        {
            FlObject left = this.GetFlObjectFromOperand(bi.Left);
            FlObject right = this.GetFlObjectFromOperand(bi.Right);
            FlObject result = left.Type.GetStaticMethod(FlType.OperatorLt).Invoke(SymbolTable, left, right);
            SymbolTable.GetSymbol(bi.DestSymbol.Name).UpdateBinding(result);
        }

        protected void VisitClte(ClteInstruction bi)
        {
            FlObject left = this.GetFlObjectFromOperand(bi.Left);
            FlObject right = this.GetFlObjectFromOperand(bi.Right);
            FlObject result = left.Type.GetStaticMethod(FlType.OperatorLte).Invoke(SymbolTable, left, right);
            SymbolTable.GetSymbol(bi.DestSymbol.Name).UpdateBinding(result);
        }

        protected void VisitAnd(AndInstruction bi)
        {
            FlObject left = this.GetFlObjectFromOperand(bi.Left);
            FlObject right = this.GetFlObjectFromOperand(bi.Right);
            bool l = left.Type != FlBoolType.Instance ? left.RawValue != null : (left as FlBool).Value;
            bool r = right.Type != FlBoolType.Instance ? right.RawValue != null : (right as FlBool).Value;
            FlObject result = new FlBool(l && r);
            SymbolTable.GetSymbol(bi.DestSymbol.Name).UpdateBinding(result);
        }

        protected void VisitOr(OrInstruction bi)
        {
            FlObject left = this.GetFlObjectFromOperand(bi.Left);
            FlObject right = this.GetFlObjectFromOperand(bi.Right);
            bool l = left.Type != FlBoolType.Instance ? left.RawValue != null : (left as FlBool).Value;
            bool r = right.Type != FlBoolType.Instance ? right.RawValue != null : (right as FlBool).Value;
            FlObject result = new FlBool(l || r);
            SymbolTable.GetSymbol(bi.DestSymbol.Name).UpdateBinding(result);
        }

        public void VisitNot(NotInstruction ui)
        {
            FlObject left = this.GetFlObjectFromOperand(ui.Left);
            FlObject result = left.Type.GetStaticMethod(FlType.OperatorNot).Invoke(SymbolTable, left);
            SymbolTable.GetSymbol(ui.DestSymbol.Name).UpdateBinding(result);
        }

        public void VisitNeg(NegInstruction ui)
        {
            FlObject left = this.GetFlObjectFromOperand(ui.Left);
            FlObject result = left.Type.GetStaticMethod(FlType.OperatorSub).Invoke(SymbolTable, left);
            SymbolTable.GetSymbol(ui.DestSymbol.Name).UpdateBinding(result);
        }

        public void VisitPreInc(PreIncInstruction ui)
        {
            FlObject left = this.GetFlObjectFromOperand(ui.DestSymbol);
            FlObject result = left.Type.GetStaticMethod(FlType.OperatorPreIncr).Invoke(SymbolTable, left);
            SymbolTable.GetSymbol(ui.DestSymbol.Name).UpdateBinding(result);
        }

        public void VisitPreDec(PreDecInstruction ui)
        {
            FlObject left = this.GetFlObjectFromOperand(ui.DestSymbol);
            FlObject result = left.Type.GetStaticMethod(FlType.OperatorPreDecr).Invoke(SymbolTable, left);
            SymbolTable.GetSymbol(ui.DestSymbol.Name).UpdateBinding(result);
        }

        public void VisitPostInc(PostIncInstruction ui)
        {
            FlObject left = this.GetFlObjectFromOperand(ui.DestSymbol);
            FlObject result = left.Type.GetStaticMethod(FlType.OperatorPostIncr).Invoke(SymbolTable, left);
            SymbolTable.GetSymbol(ui.DestSymbol.Name).UpdateBinding(result);
        }

        public void VisitPostDec(PostDecInstruction ui)
        {
            FlObject left = this.GetFlObjectFromOperand(ui.DestSymbol);
            FlObject result = left.Type.GetStaticMethod(FlType.OperatorPostDecr).Invoke(SymbolTable, left);
            SymbolTable.GetSymbol(ui.DestSymbol.Name).UpdateBinding(result);
        }

        public void VisitLoad(StoreInstruction li)
        {
            FlObject value = this.GetFlObjectFromOperand(li.Value);
            var symbol = SymbolTable.GetSymbol(li.DestSymbol.Name);

            // Check the SymbolType to see if it's a valid lvalue
            if (symbol.SymbolType == SymbolType.Constant)
            {
                if (symbol.Binding.Type == FlFuncType.Instance)
                    throw new System.Exception($"Left-hand side of an assignment must be a variable. '{symbol.Name}' is a function");

                if (symbol.Binding.Type == FlNamespaceType.Instance)
                    throw new System.Exception($"Left-hand side of an assignment must be a variable. '{symbol.Name}' is a namespace");

                throw new System.Exception($"Left-hand side of an assignment must be a variable. '{symbol.Name}' is a constant value");
            }

            symbol.UpdateBinding(value);
        }

        public void VisitVar(VarInstruction vi)
        {
            FlObject value = this.GetFlObjectFromOperand(vi.Value);
            if (vi.TypeResolver != null)
            {
                FlType type = vi.TypeResolver.Resolve(SymbolTable);
                Symbol symbol = new Symbol(SymbolType.Variable);
                SymbolTable.AddSymbol(vi.DestSymbol.Name, symbol, FlNull.Value);
                var defaultValue = type.Activator(null);
                if (value == null)
                {
                    if (defaultValue != null && defaultValue != FlNull.Value)
                        symbol.UpdateBinding(defaultValue);
                }
                else
                {
                    symbol.UpdateBinding(value);
                }                
            }
            else
            {
                SymbolTable.AddSymbol(vi.DestSymbol.Name, new Symbol(SymbolType.Variable), value);
            }
        }

        public void VisitConst(ConstInstruction ci)
        {
            FlObject value = this.GetFlObjectFromOperand(ci.Value);
            SymbolTable.AddSymbol(ci.DestSymbol.Name, new Symbol(SymbolType.Constant), value);
        }

        public void VisitCall(CallInstruction ci)
        {
            FlObject result = null;
            FlObject target = GetFlObjectFromOperand(ci.Func);

            Frame functionFrame = new Frame(Frame.Parameters);
            Frames.Push(functionFrame);

            /*if (node is AstIndexerNode)
            {
                Symbol clasz = evaluator.Symtable.GetSymbol(target.ObjectType.Name) ?? evaluator.Symtable.GetSymbol(target.ObjectType.Name);
                var claszobj = (clasz.Binding as FlClass);
                FlIndexer indexer = claszobj.GetIndexer(node.Arguments.Count);
                if (indexer == null)
                    throw new AstWalkerException($"{claszobj} does not contain an indexer that accepts {node.Arguments.Count} {(node.Arguments.Count == 1 ? "argument" : "arguments")}");
                return indexer.Bind(target).Invoke(evaluator.Symtable, node.Arguments.Expressions.Select(e => e.Exec(evaluator)).ToList());
            }
            */

            if (target is FlFunction)
            {
                var parameters = new List<FlObject>(functionFrame.Parameters);
                //parameters.Reverse();
                result = (target as FlFunction).Invoke(SymbolTable, parameters);
            }
            else if (target is FlType)
            {
                var opcall = (target as FlType).GetStaticMethod(FlType.OperatorCall);
                var parameters = new List<FlObject>(functionFrame.Parameters);
                //parameters.Reverse();
                result = opcall.Invoke(SymbolTable, parameters);
            }
            /*
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

            Frame.Parameters.Clear();

            this.SymbolTable.GetSymbol(ci.DestSymbol.Name).UpdateBinding(result);
            Frames.Pop();
        }

        public void VisitParam(ParamInstruction pi)
        {
            Frame.Parameters.Push(GetFlObjectFromOperand(pi.Parameter));
        }

        public void VisitLocal(LocalInstruction li)
        {
            SymbolTable.AddSymbol(li.DestSymbol.Name, new Symbol(SymbolType.Variable), Frame.Parameters.Pop());
        }
    }
}
