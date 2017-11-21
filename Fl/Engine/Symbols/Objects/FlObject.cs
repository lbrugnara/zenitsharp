// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Fl.Engine.StdLib;
using Fl.Engine.Symbols.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Fl.Engine.Symbols
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
    }
}
