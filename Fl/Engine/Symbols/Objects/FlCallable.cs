﻿// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Fl.Engine.Evaluators;
using Fl.Engine.StdLib;
using Fl.Engine.Symbols.Exceptions;
using Fl.Engine.Symbols.Types;
using Fl.Parser.Ast;
using System;
using System.Collections.Generic;
using System.Text;

namespace Fl.Engine.Symbols.Objects
{
    public abstract class FlCallable : FlObject
    {
        public override ObjectType ObjectType => FunctionType.Value;

        public override bool IsPrimitive => true;
        
        public abstract string Name { get; }

        public abstract FlObject Invoke(SymbolTable symboltable, List<FlObject> args);

        public override object RawValue => Name;

        public override FlObject Clone()
        {
            throw new System.NotImplementedException();
        }

        public override FlObject ConvertTo(ObjectType type)
        {
            if (type == FunctionType.Value)
            {
                return this;
            }
            throw new CastException($"Cannot convert type {ObjectType} to {type}");
        }
    }
}
