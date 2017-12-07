// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Fl.Engine.StdLib;
using Fl.Engine.Symbols;
using Fl.Engine.Symbols.Exceptions;
using Fl.Engine.Symbols.Objects;
using Fl.Engine.Symbols.Types;
using Fl.Parser.Ast;
using System;
using System.Collections.Generic;
using System.Text;

namespace Fl.Engine.Evaluators
{
    class ConstantNodeEvaluator : INodeEvaluator<AstEvaluator, AstConstantNode, FlObject>
    {
        public FlObject Evaluate(AstEvaluator evaluator, AstConstantNode constdec)
        {
            foreach (var constant in constdec.Constants)
            {
                // Get the constant name
                string constname = constant.Item1.Value.ToString();

                // Run the initializer of the constant 
                FlObject init = constant.Item2.Exec(evaluator);

                // If the constant initializer is not a primitive type, throw an exception
                if (!init.IsPrimitive)
                    throw new SymbolException($"The expression being assigned to '{constname}' must be constant");

                // If the constant declaration contains a type, enforce it
                if (constdec.Type != null)
                {
                    // Get the class of the type
                    var typeClass = evaluator.Symtable.GetSymbol(constdec.Type.Value).Binding as FlClass;

                    // Get the default value of the primitive
                    FlObject defaultValue = typeClass.InvokeConstructor();

                    // Create a new symbol and bind the default value to it
                    var constsymbol = new Symbol(SymbolType.Constant);
                    evaluator.Symtable.AddSymbol(constname, constsymbol, defaultValue);

                    // Try to update the value, if the type differs this call will raise an exception
                    constsymbol.UpdateBinding(init);
                }
                else
                {
                    // If the constant declaration does not contain a type, is safe to directly initialize it
                    evaluator.Symtable.AddSymbol(constname, new Symbol(SymbolType.Constant), init);
                }
            }
            return FlNull.Value;
        }
    }
}
