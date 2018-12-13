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

        public TypeInferrer()
        {
            this.namegen = new SequenceGenerator();
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

        public void Unify(TypeInfo left, TypeInfo right)
        {
            if (!left.IsAnonymousType && !right.IsAnonymousType)
            {
                // TODO: Here we need to get the "parent" type that encloses the two types (if available)
            }
            else if (left.IsAnonymousType && !right.IsAnonymousType)
            {
                // Left is anonymous, we will update it accordingly with right's type
                // TODO: Check structural compatibility between the anonymous type and right's type
                left.Type = right.Type;
            }
            else if (!left.IsAnonymousType && right.IsAnonymousType)
            {
                // Right is anonymous, we will update it accordingly with left's type
                // TODO: Check structural compatibility between the anonymous type and left's type
                right.Type = left.Type;
            }
            else
            {
                // Both types are anonymous, we will merge them:
                // Left -> Right
                foreach (var prop in left.Type.Properties)
                    if (!right.Type.Properties.ContainsKey(prop.Key))
                        right.Type.Properties[prop.Key] = prop.Value;

                foreach (var func in left.Type.Functions)
                    if (!right.Type.Functions.ContainsKey(func.Key))
                        right.Type.Functions[func.Key] = func.Value;

                // Right -> Left
                /*foreach (var prop in right.Type.Properties)
                    if (!left.Type.Properties.ContainsKey(prop.Key))
                        left.Type.Properties[prop.Key] = prop.Value;

                foreach (var func in right.Type.Functions)
                    if (!left.Type.Functions.ContainsKey(func.Key))
                        left.Type.Functions[func.Key] = func.Value;
                */
                left.Type = right.Type;
            }
        }

        public void Unify(Object left, TypeInfo right)
        {
            if (right.IsAnonymousType)
            {
                // Right is anonymous, we will update it accordingly with left's type
                right.Type = left;
            }
            else
            {
                // TODO: Here we need to get the "parent" type that encloses the two types (if available)
            }
        }

        public void Unify(TypeInfo left, Object right)
        {
            if (left.IsAnonymousType)
            {
                // Left is anonymous, we will update it accordingly with right's type
                left.Type = right;
            }
            else
            {
                // TODO: Here we need to get the "parent" type that encloses the two types (if available)
            }
        }

        #endregion
    }
}
