// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using System;
using System.Collections.Generic;
using System.Text;

namespace Fl.Engine.Symbols.Types
{
    public abstract class ObjectType
    {
        public abstract string Name { get; }

        public abstract string ClassName { get; }

        public override string ToString()
        {
            return Name ?? "object";
        }
    }
}
