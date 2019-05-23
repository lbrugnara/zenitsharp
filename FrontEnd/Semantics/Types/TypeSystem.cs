using System;
using System.Collections.Generic;

namespace Zenit.Semantics.Types
{
    public class TypeSystem
    {
        private static Dictionary<BuiltinType, BuiltinType> TypesHierarchy = new Dictionary<BuiltinType, BuiltinType>()
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

            if (t1 == BuiltinType.Object || t2 == BuiltinType.Object)
                return BuiltinType.Object;

            if (!TypesHierarchy.ContainsKey(t1) && !TypesHierarchy.ContainsKey(t2))
                return BuiltinType.Object;

            if (!TypesHierarchy.ContainsKey(t1))
                return TypesHierarchy[t2] == t1 ? t1 : BuiltinType.Object;

            if (!TypesHierarchy.ContainsKey(t2))
                return TypesHierarchy[t1] == t2 ? t2 : BuiltinType.Object;

            BuiltinType child = t1 < t2 ? t1 : t2;
            BuiltinType parent = t1 < t2 ? t2 : t1;

            while (child != parent && child != BuiltinType.Object && TypesHierarchy.ContainsKey(child))
                child = TypesHierarchy[child];

            return child;
        }
    }
}
