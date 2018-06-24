using System;
using System.Collections.Generic;
using System.Text;

namespace Fl.Semantics.Types
{
    public class ClassInstance : Primitive
    {
        public virtual Class Class { get; }

        public ClassInstance(Class clasz)
            : base("instance")
        {
            this.Class = clasz;
        }

        public override string ToString()
        {
            return this.Class.Name;
        }
    }
}
