// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Fl.Engine.Evaluators;
using Fl.Engine.Symbols;
using Fl.Engine.Symbols.Objects;
using Fl.Engine.Symbols.Types;
using System;
using System.Collections.Generic;
using System.Text;

namespace Fl.Engine.StdLib.builtin
{
    public class CastFunction : FlFunction
    {
        private string _Name;
        private ObjectType _Type;

        public CastFunction(string name, ObjectType type)
        {
            _Name = name;
            _Type = type;
        }

        public override string Name => _Name;

        public override FlObject Invoke(SymbolTable symboltable, List<FlObject> args)
        {
            var obj = args[0];
            return obj.ConvertTo(_Type);
        }
    }
}
