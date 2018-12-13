// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using System.Collections;
using System.Collections.Generic;

namespace Fl.Semantics.Types
{
    public abstract class Complex : Object
    {
        public Complex(string name)
            : base(name)
        {
        }

        public override bool Equals(object obj)
        {
            return 
                obj is Complex                          // Is complex
                && base.Equals(obj)                     // Has structural equality
                && (obj as Complex).Name == this.Name;  // Names match
        }

        public virtual string ToSafeString(List<(Object type, string safestr)> safeTypes)
        {
            return this.ToString();
        }
    }
}
