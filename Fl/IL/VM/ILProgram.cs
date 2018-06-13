// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file


namespace Fl.IL.VM
{
    public class ILProgram
    {
        //public SymbolTable SymbolTable { get; }
        //public ReadOnlyDictionary<string, Fragment> Fragments { get; }
        //public Stack<Frame> Frames { get; }
        //private readonly Dictionary<OpCode, Action> OpCodeHandler;

        //public ILProgram(SymbolTable symtable, List<Fragment> fragments)
        //{
        //    this.SymbolTable = SymbolTable.NewInstance;
        //    this.Fragments = new ReadOnlyDictionary<string, Fragment>(fragments.ToDictionary(fragment => fragment.Name, fragment => fragment));
        //    this.Frames = new Stack<Frame>();

        //    this.OpCodeHandler = new Dictionary<OpCode, Action>()
        //    {
        //        { OpCode.Add, OpCodeAdd },
        //        { OpCode.Sub, OpCodeSub },
        //        { OpCode.Mult, OpCodeMult },
        //        { OpCode.Div, OpCodeDiv },
        //        { OpCode.Ceq, OpCodeCeq },
        //        { OpCode.Cgt, OpCodeCgt },
        //        { OpCode.Cgte, OpCodeCgte },
        //        { OpCode.Clt, OpCodeClt },
        //        { OpCode.Clte, OpCodeClte },
        //        { OpCode.And, OpCodeAnd },
        //        { OpCode.Or, OpCodeOr },
        //        { OpCode.Not, OpCodeNot },
        //        { OpCode.Neg, OpCodeNeg },
        //        { OpCode.PreInc, OpCodePreInc },
        //        { OpCode.PreDec, OpCodePreDec },
        //        { OpCode.PostInc, OpCodePostInc },
        //        { OpCode.PostDec, OpCodePostDec },
        //        { OpCode.Store, OpCodeStore },
        //        { OpCode.Var, OpCodeVar },
        //        { OpCode.Const, OpCodeConst },
        //        { OpCode.Call, OpCodeCall },
        //        { OpCode.Param, OpCodeParam },
        //        { OpCode.Local, OpCodeLocal },
        //        { OpCode.IfFalse, OpCodeIfFalse },
        //        { OpCode.Goto, OpCodeGoto },
        //        { OpCode.Return, OpCodeReturn },
        //    };
        //}

        //public Frame Frame => Frames.Peek();

        //public void Run()
        //{
        //    this.Frames.Push(new Frame(".global"));

        //    while (true)
        //    {
        //        Instruction instr = this.FetchNextInstruction();

        //        if (instr == null)
        //            break;

        //        if (instr.OpCode == OpCode.Nop)
        //            continue;

        //        var opcodeHandler = this.OpCodeHandler[instr.OpCode];

        //        if (opcodeHandler == null)
        //            throw new Exception($"Unhandled opcode {instr.OpCode}");

        //        opcodeHandler.Invoke();
        //    }
        //}

        //private Instruction FetchNextInstruction()
        //{
        //    var instructions = this.Fragments[this.Frame.InstrPointer.FragmentName]?.Instructions;
        //    if (instructions == null || instructions.Count <= this.Frame.InstrPointer.IP)
        //        return null;

        //    Instruction instruction = instructions[this.Frame.InstrPointer.IP];
        //    this.Frame.InstrPointer.IP++;
        //    return instruction;
        //}

        //private T FetchCurrentInstruction<T>() where T : Instruction
        //{
        //    var instructions = this.Fragments[this.Frame.InstrPointer.FragmentName]?.Instructions;
        //    if (instructions == null || instructions.Count <= this.Frame.InstrPointer.IP-1)
        //        return null;

        //    Instruction instruction = instructions[this.Frame.InstrPointer.IP-1];
        //    return instruction as T ?? throw new Exception($"Expecting exception of type {typeof(T).FullName} but received {instruction?.GetType().FullName ?? "null"}");
        //}

        //protected FlObject GetFlObjectFromOperand(Operand o)
        //{
        //    FlObject value = FlNull.Value;

        //    if (o == null)
        //        return value;

        //    if (o is SymbolOperand)
        //    {
        //        var so = o as SymbolOperand;

        //        // Get the root value
        //        value = this.SymbolTable.GetSymbol(so.Name).Binding;
        //    }
        //    else
        //    {
        //        var io = (o as ImmediateOperand);
        //        /*FlType operandType = io.TypeResolver.Resolve(SymbolTable);                
        //        value = operandType.Activator(io.Value);*/
        //    }

        //    if (o.Member != null)
        //    {
        //        FlObject curval = value;
        //        FlObject ancestor = value;
        //        SymbolOperand tmpmember = o.Member;
        //        while (tmpmember != null)
        //        {
        //            if (curval.Type == FlNamespaceType.Instance)
        //            {
        //                curval = (curval as FlNamespace)[tmpmember.Name].Binding;
        //            }
        //            else if (curval is FlType)
        //            {
        //                curval = (curval as FlType)[tmpmember.Name].Binding;
        //            }
        //            else
        //            {
        //                curval = curval.Type[tmpmember.Name].Binding;
        //            }

        //            if (curval is FlMethod)
        //            {
        //                curval = (curval as FlMethod).Bind(ancestor);
        //            }
        //            ancestor = curval;
        //            tmpmember = tmpmember.Member;
        //        }
        //        value = curval;
        //    }

        //    return value;
        //}


        //protected void OpCodeAdd()
        //{
        //    AddInstruction bi = this.FetchCurrentInstruction<AddInstruction>();
        //    FlObject left = this.GetFlObjectFromOperand(bi.Left);
        //    FlObject right = this.GetFlObjectFromOperand(bi.Right);
        //    FlObject result = left.Type.GetStaticMethod(FlType.OperatorAdd).Invoke(this.SymbolTable, left, right);
        //    this.SymbolTable.GetSymbol(bi.Destination.Name).UpdateBinding(result);
        //}

        //protected void OpCodeSub()
        //{
        //    SubInstruction bi = this.FetchCurrentInstruction<SubInstruction>();
        //    FlObject left = this.GetFlObjectFromOperand(bi.Left);
        //    FlObject right = this.GetFlObjectFromOperand(bi.Right);
        //    FlObject result = left.Type.GetStaticMethod(FlType.OperatorSub).Invoke(this.SymbolTable, left, right);
        //    this.SymbolTable.GetSymbol(bi.Destination.Name).UpdateBinding(result);
        //}

        //protected void OpCodeMult()
        //{
        //    MultInstruction bi = this.FetchCurrentInstruction<MultInstruction>();
        //    FlObject left = this.GetFlObjectFromOperand(bi.Left);
        //    FlObject right = this.GetFlObjectFromOperand(bi.Right);
        //    FlObject result = left.Type.GetStaticMethod(FlType.OperatorMult).Invoke(this.SymbolTable, left, right);
        //    this.SymbolTable.GetSymbol(bi.Destination.Name).UpdateBinding(result);
        //}

        //protected void OpCodeDiv()
        //{
        //    DivInstruction bi = this.FetchCurrentInstruction<DivInstruction>();
        //    FlObject left = this.GetFlObjectFromOperand(bi.Left);
        //    FlObject right = this.GetFlObjectFromOperand(bi.Right);
        //    FlObject result = left.Type.GetStaticMethod(FlType.OperatorDiv).Invoke(this.SymbolTable, left, right);
        //    this.SymbolTable.GetSymbol(bi.Destination.Name).UpdateBinding(result);
        //}

        //protected void OpCodeCeq()
        //{
        //    CeqInstruction bi = this.FetchCurrentInstruction<CeqInstruction>();
        //    FlObject left = this.GetFlObjectFromOperand(bi.Left);
        //    FlObject right = this.GetFlObjectFromOperand(bi.Right);
        //    FlObject result = left.Type.GetStaticMethod(FlType.OperatorEquals).Invoke(this.SymbolTable, left, right);
        //    this.SymbolTable.GetSymbol(bi.Destination.Name).UpdateBinding(result);
        //}

        //protected void OpCodeCgt()
        //{
        //    CgtInstruction bi = this.FetchCurrentInstruction<CgtInstruction>();
        //    FlObject left = this.GetFlObjectFromOperand(bi.Left);
        //    FlObject right = this.GetFlObjectFromOperand(bi.Right);
        //    FlObject result = left.Type.GetStaticMethod(FlType.OperatorGt).Invoke(this.SymbolTable, left, right);
        //    this.SymbolTable.GetSymbol(bi.Destination.Name).UpdateBinding(result);
        //}

        //protected void OpCodeCgte()
        //{
        //    CgteInstruction bi = this.FetchCurrentInstruction<CgteInstruction>();
        //    FlObject left = this.GetFlObjectFromOperand(bi.Left);
        //    FlObject right = this.GetFlObjectFromOperand(bi.Right);
        //    FlObject result = left.Type.GetStaticMethod(FlType.OperatorGte).Invoke(this.SymbolTable, left, right);
        //    this.SymbolTable.GetSymbol(bi.Destination.Name).UpdateBinding(result);
        //}

        //protected void OpCodeClt()
        //{
        //    CltInstruction bi = this.FetchCurrentInstruction<CltInstruction>();
        //    FlObject left = this.GetFlObjectFromOperand(bi.Left);
        //    FlObject right = this.GetFlObjectFromOperand(bi.Right);
        //    FlObject result = left.Type.GetStaticMethod(FlType.OperatorLt).Invoke(this.SymbolTable, left, right);
        //    this.SymbolTable.GetSymbol(bi.Destination.Name).UpdateBinding(result);
        //}

        //protected void OpCodeClte()
        //{
        //    ClteInstruction bi = this.FetchCurrentInstruction<ClteInstruction>();
        //    FlObject left = this.GetFlObjectFromOperand(bi.Left);
        //    FlObject right = this.GetFlObjectFromOperand(bi.Right);
        //    FlObject result = left.Type.GetStaticMethod(FlType.OperatorLte).Invoke(this.SymbolTable, left, right);
        //    this.SymbolTable.GetSymbol(bi.Destination.Name).UpdateBinding(result);
        //}

        //protected void OpCodeAnd()
        //{
        //    AndInstruction bi = this.FetchCurrentInstruction<AndInstruction>();
        //    FlObject left = this.GetFlObjectFromOperand(bi.Left);
        //    FlObject right = this.GetFlObjectFromOperand(bi.Right);
        //    bool l = left.Type != FlBoolType.Instance ? left.RawValue != null : (left as FlBool).Value;
        //    bool r = right.Type != FlBoolType.Instance ? right.RawValue != null : (right as FlBool).Value;
        //    FlObject result = new FlBool(l && r);
        //    this.SymbolTable.GetSymbol(bi.Destination.Name).UpdateBinding(result);
        //}

        //protected void OpCodeOr()
        //{
        //    OrInstruction bi = this.FetchCurrentInstruction<OrInstruction>();
        //    FlObject left = this.GetFlObjectFromOperand(bi.Left);
        //    FlObject right = this.GetFlObjectFromOperand(bi.Right);
        //    bool l = left.Type != FlBoolType.Instance ? left.RawValue != null : (left as FlBool).Value;
        //    bool r = right.Type != FlBoolType.Instance ? right.RawValue != null : (right as FlBool).Value;
        //    FlObject result = new FlBool(l || r);
        //    this.SymbolTable.GetSymbol(bi.Destination.Name).UpdateBinding(result);
        //}

        //public void OpCodeNot()
        //{
        //    NotInstruction ui = this.FetchCurrentInstruction<NotInstruction>();
        //    FlObject left = this.GetFlObjectFromOperand(ui.Left);
        //    FlObject result = left.Type.GetStaticMethod(FlType.OperatorNot).Invoke(this.SymbolTable, left);
        //    this.SymbolTable.GetSymbol(ui.Destination.Name).UpdateBinding(result);
        //}

        //public void OpCodeNeg()
        //{
        //    NegInstruction ui = this.FetchCurrentInstruction<NegInstruction>();
        //    FlObject left = this.GetFlObjectFromOperand(ui.Left);
        //    FlObject result = left.Type.GetStaticMethod(FlType.OperatorSub).Invoke(this.SymbolTable, left);
        //    this.SymbolTable.GetSymbol(ui.Destination.Name).UpdateBinding(result);
        //}

        //public void OpCodePreInc()
        //{
        //    PreIncInstruction ui = this.FetchCurrentInstruction<PreIncInstruction>();
        //    FlObject left = this.GetFlObjectFromOperand(ui.Left);
        //    FlObject result = left.Type.GetStaticMethod(FlType.OperatorPreIncr).Invoke(this.SymbolTable, left);
        //    this.SymbolTable.GetSymbol(ui.Destination.Name).UpdateBinding(result);
        //}

        //public void OpCodePreDec()
        //{
        //    PreDecInstruction ui = this.FetchCurrentInstruction<PreDecInstruction>();
        //    FlObject left = this.GetFlObjectFromOperand(ui.Left);
        //    FlObject result = left.Type.GetStaticMethod(FlType.OperatorPreDecr).Invoke(this.SymbolTable, left);
        //    this.SymbolTable.GetSymbol(ui.Destination.Name).UpdateBinding(result);
        //}

        //public void OpCodePostInc()
        //{
        //    PostIncInstruction ui = this.FetchCurrentInstruction<PostIncInstruction>();
        //    FlObject left = this.GetFlObjectFromOperand(ui.Left);
        //    FlObject result = left.Type.GetStaticMethod(FlType.OperatorPostIncr).Invoke(this.SymbolTable, left);
        //    this.SymbolTable.GetSymbol(ui.Destination.Name).UpdateBinding(result);
        //}

        //public void OpCodePostDec()
        //{
        //    PostDecInstruction ui = this.FetchCurrentInstruction<PostDecInstruction>();
        //    FlObject left = this.GetFlObjectFromOperand(ui.Left);
        //    FlObject result = left.Type.GetStaticMethod(FlType.OperatorPostDecr).Invoke(this.SymbolTable, left);
        //    this.SymbolTable.GetSymbol(ui.Destination.Name).UpdateBinding(result);
        //}

        //public void OpCodeStore()
        //{
        //    StoreInstruction li = this.FetchCurrentInstruction<StoreInstruction>();

        //    FlObject value = this.GetFlObjectFromOperand(li.Value);
        //    var symbol = this.SymbolTable.GetSymbol(li.Destination.Name);

        //    // Check the SymbolType to see if it's a valid lvalue
        //    if (symbol.SymbolType == SymbolType.Constant)
        //    {
        //        if (symbol.Binding.Type == FlFuncType.Instance)
        //            throw new System.Exception($"Left-hand side of an assignment must be a variable. '{symbol.Name}' is a function");

        //        if (symbol.Binding.Type == FlNamespaceType.Instance)
        //            throw new System.Exception($"Left-hand side of an assignment must be a variable. '{symbol.Name}' is a namespace");

        //        throw new System.Exception($"Left-hand side of an assignment must be a variable. '{symbol.Name}' is a constant value");
        //    }

        //    symbol.UpdateBinding(value);
        //}

        //public void OpCodeVar()
        //{
        //    VarInstruction vi = this.FetchCurrentInstruction<VarInstruction>();

        //    FlObject value = this.GetFlObjectFromOperand(vi.Value);
        //    /*if (vi.TypeResolver != null)
        //    {
        //        FlType type = vi.TypeResolver.Resolve(this.SymbolTable);
        //        Symbol symbol = new Symbol(SymbolType.Variable);
        //        this.SymbolTable.AddSymbol(vi.Destination.Name, symbol, FlNull.Value);
        //        var defaultValue = type.Activator(null);
        //        if (value == null)
        //        {
        //            if (defaultValue != null && defaultValue != FlNull.Value)
        //                symbol.UpdateBinding(defaultValue);
        //        }
        //        else
        //        {
        //            symbol.UpdateBinding(value);
        //        }                
        //    }
        //    else*/
        //    {
        //        //this.SymbolTable.AddSymbol(vi.Destination.Name, new Symbol(SymbolType.Variable), value);
        //    }
        //}

        //public void OpCodeConst()
        //{
        //    ConstInstruction ci = this.FetchCurrentInstruction<ConstInstruction>();

        //    FlObject value = this.GetFlObjectFromOperand(ci.Value);
        //    //this.SymbolTable.AddSymbol(ci.Destination.Name, new Symbol(SymbolType.Constant), value);
        //}

        //private void OpCodeReturn()
        //{
        //    ReturnInstruction instr = this.FetchCurrentInstruction<ReturnInstruction>();

        //    if (instr.Value != null)
        //    {
        //        this.SymbolTable.ReturnValue = GetFlObjectFromOperand(instr.Value);
        //    }

        //    this.Frame.Parameters.Clear();
        //    this.Frames.Pop();
        //}

        //public void OpCodeCall()
        //{
        //    CallInstruction ci = this.FetchCurrentInstruction<CallInstruction>();

        //    if (!this.Fragments.ContainsKey(ci.Func.ToString()))
        //    {
        //        this.HandleNativeCall();
        //        return;
        //    }
        //    this.Frames.Push(new Frame(ci.Func.ToString(), new Stack<FlObject>(this.Frame.Parameters)));
        //}

        //private void HandleNativeCall()
        //{
        //    CallInstruction ci = this.FetchCurrentInstruction<CallInstruction>();

        //    FlObject target = this.GetFlObjectFromOperand(ci.Func);

        //    Frame functionFrame = new Frame(ci.Func.ToString(), this.Frame.Parameters);
        //    this.Frames.Push(functionFrame);

        //    /*if (node is AstIndexerNode)
        //    {
        //        Symbol clasz = evaluator.Symtable.GetSymbol(target.ObjectType.Name) ?? evaluator.Symtable.GetSymbol(target.ObjectType.Name);
        //        var claszobj = (clasz.Binding as FlClass);
        //        FlIndexer indexer = claszobj.GetIndexer(node.Arguments.Count);
        //        if (indexer == null)
        //            throw new AstWalkerException($"{claszobj} does not contain an indexer that accepts {node.Arguments.Count} {(node.Arguments.Count == 1 ? "argument" : "arguments")}");
        //        return indexer.Bind(target).Invoke(evaluator.Symtable, node.Arguments.Expressions.Select(e => e.Exec(evaluator)).ToList());
        //    }
        //    */

        //    if (target is FlFunction)
        //    {
        //        var parameters = new List<FlObject>(functionFrame.Parameters);
        //        var result = (target as FlFunction).Invoke(this.SymbolTable, parameters);
        //        if (result != FlNull.Value)
        //            this.SymbolTable.ReturnValue = result;
        //    }
        //    else if (target is FlType)
        //    {
        //        var opcall = (target as FlType).GetStaticMethod(FlType.OperatorCall);
        //        var parameters = new List<FlObject>(functionFrame.Parameters);
        //        var result = opcall.Invoke(this.SymbolTable, parameters);
        //        if (result != FlNull.Value)
        //            this.SymbolTable.ReturnValue = result;
        //    }
        //    /*
        //    else if (target is FlClass)
        //    {
        //        var clasz = (target as FlClass);

        //        if (node.New != null)
        //        {
        //            return clasz.InvokeConstructor(node.Arguments.Expressions.Select(a => a.Exec(evaluator)).ToList());
        //        }
        //        else
        //        {
        //            target = clasz.StaticConstructor ?? throw new AstWalkerException($"{target} does not contain a definition for the static constructor");
        //        }
        //    }*/
        //    //throw new AstWalkerException($"{target} is not a callable object");

        //    this.Frame.Parameters.Clear();
        //    this.Frames.Pop();
        //}

        //public void OpCodeParam()
        //{
        //    ParamInstruction pi = this.FetchCurrentInstruction<ParamInstruction>();
        //    this.Frame.Parameters.Push(this.GetFlObjectFromOperand(pi.Parameter));
        //}

        //public void OpCodeLocal()
        //{
        //    if (this.Fragments[this.Frame.InstrPointer.FragmentName].Type != FragmentType.Function)
        //        throw new InvalidInstructionException("Cannot declare local variable in a non-function fragment");

        //    LocalInstruction li = this.FetchCurrentInstruction<LocalInstruction>();
        //    //this.SymbolTable.AddSymbol(li.Destination.Name, new Symbol(SymbolType.Variable), this.Frame.Parameters.Pop());
        //}

        //private void OpCodeIfFalse()
        //{
        //    IfFalseInstruction instr = this.FetchCurrentInstruction<IfFalseInstruction>();
        //    FlBool cond = this.GetFlObjectFromOperand(instr.Condition) as FlBool;
        //    if (!cond.Value)
        //        this.Frame.InstrPointer.IP = this.Fragments[this.Frame.InstrPointer.FragmentName].Labels[instr.Goto.Name];
        //}

        //private void OpCodeGoto()
        //{
        //    GotoInstruction instr = this.FetchCurrentInstruction<GotoInstruction>();
        //    this.Frame.InstrPointer.IP = this.Fragments[this.Frame.InstrPointer.FragmentName].Labels[instr.Goto.Name];
        //}

        //public override string ToString()
        //{
        //    StringBuilder sb = new StringBuilder();
        //    foreach (var fragment in this.Fragments.Values)
        //    {
        //        if (fragment.Type == FragmentType.Global)
        //            continue;
        //        sb.AppendLine(fragment.ToString());
        //    }

        //    if (this.Fragments.ContainsKey(".global"))
        //        sb.AppendLine(this.Fragments[".global"].ToString());
        //    else
        //        sb.AppendLine(".global:");

        //    return sb.ToString();
        //}
    }
}
