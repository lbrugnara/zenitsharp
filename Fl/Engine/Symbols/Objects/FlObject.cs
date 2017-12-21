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

        public abstract FlType Type { get; }

        public abstract FlObject Clone();

        public override string ToString()
        {
            return this.RawValue.ToString();
        }

        public virtual string ToDebugStr()
        {
            return $"{this.RawValue} ({this.Type})";
        }
    }
}
