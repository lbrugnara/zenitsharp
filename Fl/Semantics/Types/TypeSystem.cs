using System;
using System.Collections.Generic;

namespace Fl.Semantics.Types
{
    public class TypeSystem
    {
        private static Dictionary<BuiltinType, BuiltinType> TypeHierarchy = new Dictionary<BuiltinType, BuiltinType>()
        {
            { BuiltinType.Char,     BuiltinType.Bool    },
            { BuiltinType.Bool,     BuiltinType.Int     },
            { BuiltinType.Int,      BuiltinType.Float   },
            { BuiltinType.Float,    BuiltinType.Double  },

            { BuiltinType.Double,   BuiltinType.Number  },
            { BuiltinType.Decimal,  BuiltinType.Number  }
        };

        public TypeSystem()
        {
        }

        public BuiltinType GetCommonAncestor(BuiltinType t1, BuiltinType t2)
        {
            if (t1 == BuiltinType.Void || t1 == BuiltinType.None)
                throw new Exception($"Type {t1} is an special type and cannot be explicitly used");

            if (t2 == BuiltinType.Void || t2 == BuiltinType.None)
                throw new Exception($"Type {t2} is an special type and cannot be explicitly used");

            if (t1 == t2)
                return t1;

            // Object if one of them does not have a parent in the dict or already is an Object
            if (!TypeHierarchy.ContainsKey(t1) || !TypeHierarchy.ContainsKey(t2) || t1 == BuiltinType.Object || t2 == BuiltinType.Object)
                return BuiltinType.Object;

            BuiltinType child = t1 < t2 ? t1 : t2;
            BuiltinType parent = t1 < t2 ? t2 : t1;

            while (child != parent && child != BuiltinType.Object && TypeHierarchy.ContainsKey(child))
                child = TypeHierarchy[child];

            return child;
        }
    }
}
