// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using System.Collections;
using System.Collections.Generic;

namespace Fl.Semantics.Types
{
    public abstract class Struct : Type
    {
        public Struct(string name)
            : base(name)
        {
        }

        public virtual string ToSafeString(List<(Type type, string safestr)> safeTypes)
        {
            return this.ToString();
        }
    }
}
