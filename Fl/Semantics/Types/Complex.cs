// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using System.Collections;
using System.Collections.Generic;

namespace Fl.Semantics.Types
{
    public abstract class Complex : Struct
    {
        public Complex(string name)
            : base(name)
        {
        }

        public virtual string ToSafeString(List<(Struct type, string safestr)> safeTypes)
        {
            return this.ToString();
        }
    }
}
