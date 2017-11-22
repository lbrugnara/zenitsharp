// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Fl.Engine.StdLib;
using Fl.Engine.Symbols.Exceptions;
using Fl.Engine.Symbols.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Fl.Engine.Symbols.Objects
{
    public abstract class FlObject
    {
        public abstract object RawValue { get; }

        public abstract bool IsPrimitive { get; }

        public abstract ObjectType ObjectType { get; }

        public override string ToString()
        {
            return RawValue.ToString();
        }

        public virtual string ToDebugStr()
        {
            return $"{RawValue} ({ObjectType})";
        }

        public abstract FlObject Clone();

        public abstract FlObject ConvertTo(ObjectType type);

        public virtual Symbol this[string membername]
        {
            get
            {
                throw new SymbolException($"{ObjectType} does not contain a definition of '{membername}'");
            }
        }
    }
}
