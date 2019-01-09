// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Fl.Helpers;
using Fl.Semantics.Symbols;
using Fl.Semantics.Types;
using System.Collections.Generic;
using System.Linq;

namespace Fl.Semantics.Inferrers
{
    public class TypeInferrer
    {
        private readonly SequenceGenerator namegen;
        private TypeSystem typeSystem;

        public TypeInferrer()
        {
            this.namegen = new SequenceGenerator();
            this.typeSystem = new TypeSystem();
        }


        #region Public API

        /// <summary>
        /// Create a new anonymous type
        /// </summary>
        /// <returns></returns>
        public TypeInfo NewAnonymousType()
        {
            return new TypeInfo(new Anonymous(this.namegen.Generate()));
        }

        public TypeInfo FindMostGeneralType(TypeInfo left, TypeInfo right)
        {
            if (!this.CanUnify(left.Type.BuiltinType, right.Type.BuiltinType))
                throw new System.Exception($"Cannot unify types {left} and {right}");

            if (left.IsAnonymousType && !right.IsAnonymousType)
                return new TypeInfo(right.Type);

            if (right.IsAnonymousType && !left.IsAnonymousType)
                return new TypeInfo(left.Type);

            if (left.IsPrimitiveType && right.IsPrimitiveType)
                return new TypeInfo(new Object(this.typeSystem.GetCommonAncestor(left.Type.BuiltinType, right.Type.BuiltinType)));

            // If the types are structural types and are the same built-in type
            // they can be "merged"
            if (left.IsStructuralType && right.IsStructuralType)
            {
                Object type = null;

                switch (left.Type.BuiltinType)
                {
                    case BuiltinType.Function:
                        type = new Function();
                        break;
                    case BuiltinType.Tuple:
                        type = new Tuple();
                        break;
                    case BuiltinType.Object:
                        type = new Object(left.Type.BuiltinType);
                        break;
                }
                
                foreach (var prop in left.Type.Properties)
                    type.Properties[prop.Key] = prop.Value;

                foreach (var func in left.Type.Functions)
                    type.Functions[func.Key] = func.Value;
                
                foreach (var prop in right.Type.Properties)
                    if (!type.Properties.ContainsKey(prop.Key))
                        type.Properties[prop.Key] = prop.Value;

                foreach (var func in right.Type.Functions)
                    if (!type.Functions.ContainsKey(func.Key))
                        type.Functions[func.Key] = func.Value;

                return new TypeInfo(type);
            }

            return this.NewAnonymousType();
        }

        public TypeInfo FindMostGeneralType(TypeInfo left, Object right)
        {
            if (!this.CanUnify(left.Type.BuiltinType, right.BuiltinType))
                throw new System.Exception($"Cannot unify types {left} and {right}");

            if (left.IsAnonymousType)
                return new TypeInfo(right);

            return new TypeInfo(left.Type);
        }

        public void ExpectsToUnifyWith(TypeInfo right, BuiltinType left)
        {
            if (!this.CanUnify(left, right.Type.BuiltinType))
                throw new System.Exception($"Cannot unify types {left} and {right}");

            if (right.IsAnonymousType)
                right.ChangeType(new Object(left));
        }

        public void Unify(TypeInfo generalType, params TypeInfo[] constructs)
        {
            foreach (var construct in constructs.Where(c => c != null))
            {
                // In variable assignment we try to infer
                if (!construct.Equals(generalType))
                    construct.ChangeType(generalType.Type);
            }
        }

        #endregion

        private bool CanUnify(BuiltinType t1, BuiltinType t2)
        {
            if (t1 == t2)
                return true;

            if (t1.IsPrimitive() && t2.IsStructuralType())
                return false;

            if (t1.IsStructuralType() && t2.IsStructuralType() && t1 != t2)
                return false;

            return true;
        }
    }
}
