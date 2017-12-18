// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Fl.Parser;

namespace Fl.Engine.IL.Instructions
{
    public enum OpCode
    {
        // No-Op instruction: 
        //  nop
        Nop,

        // Declare variable <a>. If <b> is provided, <a> is initialized with <b>'s value. If not, just null: 
        //  var <a> null
        //  var <a> <b>
        Var,

        // Declare constant <a> initialized with <b>'s value: 
        //  const <a> <b>
        Const,

        // Store value of <b> into <a>:
        //  store <a> <b>
        Store,

        // Call function <f> taking <n> params:
        //  call <f> <n>
        Call,

        // Prepare param <a> to be used by a call operation:
        //  param <a>
        Param,


        // Add b and c and save it into <a>:
        //  add <a> <b> <c>
        Add,

        // Subtract b and c and save it into <a>:
        //  sub <a> <b> <c>
        Sub,

        // Multiply b and c and save it into <a>:
        //  mult <a> <b> <c>
        Mult,

        // Divide b and c and save it into <a>:
        //  div <a> <b> <c>
        Div,

        // Negate logical expression <b> and save it into <a>:
        //  not <a> <b>
        Not,

        // Negate arithmetic expression <b> and save it into <a>:
        //  not <a> <b>
        Neg,

        // Pre Increment operand <b> and save it into <a>:
        //  prei <a> <b>
        PreInc,

        // Pre Decrement operand <b> and save it into <a>:
        //  pred <a> <b>
        PreDec,

        // Post Increment operand <b> and save <b>'s previous value into <a>:
        //  posti <a> <b>
        PostInc,

        // Post Decrement operand <b> and save <b>'s previous value into <a>:
        //  postd <a> <b>
        PostDec,

        // Compute a logical AND between <b> and <c> and save the result into <a>:
        //  and <a> <b> <c>
        And,

        // Compute a logical OR between <b> and <c> and save the result into <a>:
        //  or <a> <b> <c>
        Or,

        // Compare equality between <b> and <c> and save the result into <a>:
        //  ceq <a> <b> <c>
        Ceq,

        // Compare if <b> is greater than <c> and save the result into <a>:
        //  cgt <a> <b> <c>
        Cgt,

        // Compare if <b> is greater than or equals <c> and save the result into <a>:
        //  cgte <a> <b> <c>
        Cgte,

        // Compare if <b> is lesser than <c> and save the result into <a>:
        //  clt <a> <b> <c>
        Clt,

        // Compare if <b> is lesser than or equals <c> and save the result into <a>:
        //  clte <a> <b> <c>
        Clte
    }

    public static class OpCodeExtensions
    {
        public static string InstructionName(this OpCode self)
        {
            switch (self)
            {
                case OpCode.Nop:
                    return "nop";

                case OpCode.Var:
                    return "var";

                case OpCode.Const:
                    return "const";

                case OpCode.Store:
                    return "store";

                case OpCode.Call:
                    return "call";

                case OpCode.Param:
                    return "param";

                case OpCode.Add:
                    return "add";

                case OpCode.Sub:
                    return "sub";

                case OpCode.Mult:
                    return "mult";

                case OpCode.Div:
                    return "div";

                case OpCode.Not:
                    return "not";

                case OpCode.Neg:
                    return "neg";

                case OpCode.PreInc:
                    return "prei";

                case OpCode.PreDec:
                    return "pred";

                case OpCode.PostInc:
                    return "posti";

                case OpCode.PostDec:
                    return "postd";

                case OpCode.And:
                    return "and";

                case OpCode.Or:
                    return "or";

                case OpCode.Ceq:
                    return "ceq";

                case OpCode.Cgt:
                    return "cgt";

                case OpCode.Cgte:
                    return "cgte";

                case OpCode.Clt:
                    return "clt";

                case OpCode.Clte:
                    return "clte";
            }
            return "-";
        }
    }
}
