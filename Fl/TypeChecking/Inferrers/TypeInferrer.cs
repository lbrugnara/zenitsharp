// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Fl.Symbols;
using Fl.Lang.Types;
using System.Collections.Generic;
using System.Linq;

namespace Fl.TypeChecking.Inferrers
{
    public class TypeInferrer
    {
        /// <summary>
        /// Assumptions of primitive types. When a complex type is assumed,
        /// it is broken down in its pirmitive types, and the symbols that have the complex
        /// type are all tied to the complex' primitive types
        /// </summary>
        private Dictionary<Anonymous, List<Symbol>> assumptions;

        private int[] names = new int[] { -1, -1, -1, -1, -1, -1, -1 };


        public TypeInferrer()
        {
            this.assumptions = new Dictionary<Anonymous, List<Symbol>>();
        }


        #region Public API

        /// <summary>
        /// Creates a constraint where symbol's type inference depends on the resolution
        /// of the type represented by the Type object
        /// </summary>
        /// <param name="symbol">Symbol that has the type to be inferred</param>
        /// <param name="type">Type to be inferred</param>
        public void AssumeSymbolTypeAs(Symbol symbol, Type type)
        {
            // If type is a primitive type other than anonymous type
            // the constraint is not needed as it is understood that
            // the type is already inferred
            if (type is Anonymous at)
                this.AssumeSymbolTypeAsAnonymousType(symbol, at);
            else if (type is ComplexType ct)
                this.AssumeSymbolTypeAsComplexType(symbol, ct);

            // TODO: If we want to be more strict regarding constraint checking, we could allow
            // the following check to detect unnecessary calls to the GenerateConstraint method 
            /*else if (type is PrimitiveType)
                throw new System.Exception($"Non-anonymous types like {type} cannot be used in constraints");*/
        }

        /// <summary>
        /// Creates a constraint where symbol's type inference depends on the resolution
        /// of an anonymous type that is generated to represent symbol's type
        /// ahead in time
        /// </summary>
        /// <param name="symbol">Symbol to assign the temporal type</param>
        /// <returns>New type that represents the undefined symbol's type</returns>
        public Anonymous AssumeSymbolType(Symbol symbol)
        {
            var tempType = new Anonymous(this.GetName());
            symbol.Type = tempType;

            this.AssumeSymbolTypeAs(symbol, tempType);

            return tempType;
        }

        /// <summary>
        /// If one of these types can be unified with the other, this method
        /// will update it and will return the unified type
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public Type UnifyTypesIfPossible(Type left, Type right)
        {
            // Can't unify null objects
            if (left == null || right == null)
                throw new System.ArgumentNullException(left == null ? nameof(left) : nameof(right) + " cannot be null");

            // If left is an anonymous type and right is already inferred,
            // then unify under right's type
            if (left is Anonymous && !(right is Anonymous))
            {
                this.UnifyTypes(left as Anonymous, right);
                return right;
            }
            else if (right is Anonymous)
            {
                // If both are Anonymous, or just the right type
                // then unify under left's type
                this.UnifyTypes(right as Anonymous, left);
            }
            else
            {
                // If neither of these types are anonymous, we just return
                // the left type.
                // TODO: Here we could use union types or throw
            }

            return left;
        }

        public bool TypeIsAssumed(Type t)
        {
            if (t is PrimitiveType pt)
                return (pt is Anonymous at) ? this.assumptions.ContainsKey(at) : false; // PrimitiveTypes that are not anonymous do not have constraints

            // Complex type
            if (t is Func ft)
                return ft.Parameters.Any(this.TypeIsAssumed) || this.TypeIsAssumed(ft.Return);

            if (t is Tuple tt)
                return tt.Types.Any(this.TypeIsAssumed);

            throw new System.Exception($"Unhandled type {t}");
        }

        #endregion

        #region Constraints

        private void AssumeSymbolTypeAsAnonymousType(Symbol symbol, Anonymous type)
        {
            if (!this.assumptions.ContainsKey(type))
                this.assumptions[type] = new List<Symbol>();

            if (!this.assumptions[type].Contains(symbol))
                this.assumptions[type].Add(symbol);
        }

        private void AssumeSymbolTypeAsComplexType(Symbol symbol, ComplexType type)
        {
            if (type is Func f)
            {
                f.Parameters.ToList().ForEach(paramType => this.AssumeSymbolTypeAs(symbol, paramType));
                this.AssumeSymbolTypeAs(symbol, f.Return);
            }
            else if (type is Tuple t)
            {
                t.Types.ToList().ForEach(paramType => this.AssumeSymbolTypeAs(symbol, paramType));
            }
            else
            {
                throw new System.Exception($"Unhandled ComplexType {type}");
            }
        }

        #endregion

        #region Type inference

        /// <summary>
        /// Check if a type is a complex type, like function, tuple, etc
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        private bool IsComplexType(Type t) => t is ComplexType;

        /// <summary>
        /// Update previous inferred type in a symbol by changing the occurrences to the new
        /// inferred type.
        /// </summary>
        /// <param name="s">Symbol to be updated</param>
        /// <param name="prevInferredType">Previous inferred type</param>
        /// <param name="newInferredType">New inferred type</param>
        private void UpdateSymbolType(Symbol s, Anonymous prevInferredType, Type newInferredType)
        {
            if (this.IsComplexType(s.Type))
                this.UpdateComplexType(s.Type, prevInferredType, newInferredType);
            else
                s.Type = newInferredType;
        }

        /// <summary>
        /// Check complex type structure, and change previous inferred type occurrences by assiging
        /// them to the new inferred type
        /// </summary>
        /// <param name="complexType">Type to be updated</param>
        /// <param name="prevInferredType">Previous inferred type</param>
        /// <param name="newInferredType">New inferred type</param>
        private void UpdateComplexType(Type complexType, Anonymous prevInferredType, Type newInferredType)
        {
            if (complexType is Func f)
                this.UpdateFunctionType(f, prevInferredType, newInferredType);
            else if (complexType is Tuple tu)
                this.UpdateTupleType(tu, prevInferredType, newInferredType);
        }

        /// <summary>
        /// Update previous inferred type in a function type by changing the occurrences to the
        /// new inferred type. This process does this by recursively checking the complext types
        /// </summary>
        /// <param name="f">Function type to be updated</param>
        /// <param name="prevInferredType">Previous inferred type</param>
        /// <param name="newInferredType">New inferred type</param>
        private void UpdateFunctionType(Func f, Anonymous prevInferredType, Type newInferredType)
        {
            for (int i = 0; i < f.Parameters.Length; i++)
            {
                // If type is a complex type, infer it as a complex type, otherwise just updated the value
                if (this.IsComplexType(f.Parameters[i]))
                    this.UpdateComplexType(f.Parameters[i], prevInferredType, newInferredType);
                else if (f.Parameters[i] == prevInferredType)
                    f.Parameters[i] = newInferredType;
            }

            // If return type is a complex type, infer it as a complex type, otherwise just updated the value
            if (this.IsComplexType(f.Return))
                this.UpdateComplexType(f.Return, prevInferredType, newInferredType);
            else if (f.Return == prevInferredType)
                f.Return = newInferredType;
        }

        /// <summary>
        /// Update previous inferred type in a tuple type by changing the occurrences to the
        /// new inferred type. This process does this by recursively checking the complext types
        /// </summary>
        /// <param name="t">Tuple type to be updated</param>
        /// <param name="prevInferredType">Previous inferred type</param>
        /// <param name="newInferredType">New inferred type</param>
        private void UpdateTupleType(Tuple t, Anonymous prevInferredType, Type newInferredType)
        {
            for (int i = 0; i < t.Types.Count; i++)
            {
                // If type is a complex type, infer it as a complex type, otherwise just updated the value
                if (this.IsComplexType(t.Types[i]))
                    this.UpdateComplexType(t.Types[i], prevInferredType, newInferredType);
                else if (t.Types[i] == prevInferredType)
                    t.Types[i] = newInferredType;
            }
        }

        /// <summary>
        /// Infer an anonymous previous type by unify it with the new inferred type
        /// </summary>
        /// <param name="prevInferredType">Previous anonymous type</param>
        /// <param name="newInferredType">New inferred type</param>
        private void UnifyTypes(Anonymous prevInferredType, Type newInferredType)
        {
            // Infer each symbol type under the previous anonymous inferred type
            this.assumptions[prevInferredType].ForEach(s => this.UpdateSymbolType(s, prevInferredType, newInferredType));

            // If the new inferred type is also an anonymous type, generate all the constraints for the new anonymous type
            if (this.TypeIsAssumed(newInferredType))
                this.assumptions[prevInferredType].ForEach(s => this.AssumeSymbolTypeAs(s, newInferredType));

            // Remove all the resolved types
            this.assumptions.Remove(prevInferredType);
        }

        #endregion

        private string GetName()
        {
            // Poor's man type name generator
            for (var i=0; i < this.names.Length; i++)
            {
                if (this.names[i] < 25)
                {
                    this.names[i] += 1;
                    return string.Join("", this.names.Where(n => n > -1).Select(n => (char)('A' + n)));
                }
                this.names[i] = 0;
            }
            throw new System.Exception("Carry Overflow");
        }
    }
}
