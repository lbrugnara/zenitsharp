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
        /// <summary>
        /// Assumptions of primitive types. When a complex type is assumed,
        /// it is broken down into its primitive types, and the symbols that have the complex
        /// type are all tied to the complex' primitive types
        /// </summary>
        private readonly Dictionary<Anonymous, List<Symbol>> assumptions;

        private readonly SequenceGenerator namegen;

        public TypeInferrer()
        {
            this.assumptions = new Dictionary<Anonymous, List<Symbol>>();
            this.namegen = new SequenceGenerator();
        }


        #region Public API

        /// <summary>
        /// Creates a constraint where symbol's type inference depends on the resolution
        /// of the type represented by the Type object
        /// </summary>
        /// <param name="type">Type to be inferred</param>
        /// <param name="symbol">Symbol that has the type to be inferred</param>
        public void AddTypeDependency(Object type, Symbol symbol)
        {
            // If type is a primitive type other than anonymous type
            // the constraint is not needed as it is understood that
            // the type is already inferred
            if (type is Anonymous at)
                this.AddAnonymousTypeDependency(at, symbol);
            else if (type is Complex ct)
                this.AddComplexTypeDependency(ct, symbol);

            // TODO: If we want to be more strict regarding constraint checking, we could allow
            // the following check to detect unnecessary calls to the GenerateConstraint method 
            /*else if (type is PrimitiveType)
                throw new System.Exception($"Non-anonymous types like {type} cannot be used in constraints");*/
        }

        /// <summary>
        /// Create a new anonymous type
        /// </summary>
        /// <returns></returns>
        public Anonymous NewAnonymousType()
        {
            var type = new Anonymous(this.namegen.Generate());

            if (!this.assumptions.ContainsKey(type))
                this.assumptions[type] = new List<Symbol>();

            return type;
        }

        /// <summary>
        /// If one of these types can be unified with the other, this method
        /// will update it and will return the unified type
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public Object InferFromType(Object left, Object right)
        {
            // Can't unify null objects
            if (left == null || right == null)
                throw new System.ArgumentNullException(left == null ? nameof(left) : nameof(right) + " cannot be null");

            // If left is an anonymous type and right is already inferred,
            // then unify under right's type
            if (this.IsTypeAssumption(left) && !this.IsTypeAssumption(right))
            {
                this.UnifyTypes(left, right);
                return right;
            }
            else if (this.IsTypeAssumption(right))
            {
                // If both are Anonymous, or just the right type
                // then unify under left's type
                this.UnifyTypes(right, left);
            }
            else
            {
                // If neither of these types are anonymous, we just return
                // the left type.
                // TODO: Here we could use union types or throw
            }

            return left;
        }

        public bool IsTypeAssumption(Object t)
        {
            if (t is Primitive || t is Void)
                return false;

            if (t is Anonymous at)
                return this.assumptions.ContainsKey(at);

            // Complex type
            if (t is Function ft)
                return ft.Parameters.Any(p => this.IsTypeAssumption(p)) || !ft.IsCircularReference && ft.Return != null && this.IsTypeAssumption(ft.Return);

            if (t is Tuple tt)
                return tt.Types.Any(this.IsTypeAssumption);

            // TODO: This may change based on the Class type
            if (t is Class c)
                return false;

            throw new System.Exception($"Unhandled type {t}");
        }

        #endregion

        #region Constraints

        /// <summary>
        /// Add the symbol under the assumption of having an anonymous type defined
        /// by type object
        /// </summary>
        /// <param name="type">Anonymous type</param>
        /// <param name="symbol">Symbol to assume its type</param>
        private void AddAnonymousTypeDependency(Anonymous type, Symbol symbol)
        {
            if (!this.assumptions.ContainsKey(type))
                this.assumptions[type] = new List<Symbol>();

            if (!this.assumptions[type].Contains(symbol))
                this.assumptions[type].Add(symbol);
        }

        /// <summary>
        /// Add the symbol under the assumption of having a complex type where
        /// its primitive parts might be anonymous types
        /// </summary>
        /// <param name="type">Complex type that might contain anonymous types in its primitives members</param>
        /// <param name="symbol">Symbol to assume its type</param>
        private void AddComplexTypeDependency(Complex type, Symbol symbol)
        {
            if (type is Function f)
            {
                f.Parameters.ToList().ForEach(paramType => this.AddTypeDependency(paramType, symbol));

                // Assume Return type just if it is not equals to 'f' (circular reference)
                if (!f.IsCircularReference)
                    this.AddTypeDependency(f.Return, symbol);
            }
            else if (type is Tuple t)
            {
                t.Types.ToList().ForEach(paramType => this.AddTypeDependency(paramType, symbol));
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
        private bool IsComplexType(Object t) => t is Complex;

        /// <summary>
        /// Update previous inferred type in a symbol by changing the occurrences to the new
        /// inferred type.
        /// </summary>
        /// <param name="s">Symbol to be updated</param>
        /// <param name="prevType">Previous inferred type</param>
        /// <param name="newType">New inferred type</param>
        private void InferSymbolType(Symbol s, Anonymous prevType, Object newType)
        {
            if (this.IsComplexType(s.Type))
                this.UpdateComplexType(s.Type, prevType, newType);
            else
                s.Type = newType;
        }

        /// <summary>
        /// Check complex type structure, and change previous inferred type occurrences by assiging
        /// them to the new inferred type
        /// </summary>
        /// <param name="complexType">Type to be updated</param>
        /// <param name="prevType">Previous inferred type</param>
        /// <param name="newType">New inferred type</param>
        private void UpdateComplexType(Object complexType, Anonymous prevType, Object newType)
        {
            if (complexType is Function f)
                this.UpdateFunctionType(f, prevType, newType);
            else if (complexType is Tuple tu)
                this.UpdateTupleType(tu, prevType, newType);
        }

        /// <summary>
        /// Update previous inferred type in a function type by changing the occurrences to the
        /// new inferred type. This process does this by recursively checking the complext types
        /// </summary>
        /// <param name="f">Function type to be updated</param>
        /// <param name="prevType">Previous inferred type</param>
        /// <param name="newType">New inferred type</param>
        private void UpdateFunctionType(Function f, Anonymous prevType, Object newType)
        {
            for (int i = 0; i < f.Parameters.Count; i++)
            {
                // If type is a complex type, infer it as a complex type, otherwise just updated the value
                if (this.IsComplexType(f.Parameters[i]))
                    this.UpdateComplexType(f.Parameters[i], prevType, newType);
                else if (f.Parameters[i] == prevType)
                    f.Parameters[i] = newType;
            }

            // If 'f' is a circular reference, do not touch Return property
            if (f.IsCircularReference)
                return;
            
            // If return type is a complex type, infer it as a complex type, otherwise just updated the value
            if (this.IsComplexType(f.Return))
                this.UpdateComplexType(f.Return, prevType, newType);
            else if (f.Return == prevType)
                f.SetReturnType(newType);
        }

        /// <summary>
        /// Update previous inferred type in a tuple type by changing the occurrences to the
        /// new inferred type. This process does this by recursively checking the complext types
        /// </summary>
        /// <param name="t">Tuple type to be updated</param>
        /// <param name="prevType">Previous inferred type</param>
        /// <param name="newType">New inferred type</param>
        private void UpdateTupleType(Tuple t, Anonymous prevType, Object newType)
        {
            for (int i = 0; i < t.Types.Count; i++)
            {
                // If type is a complex type, infer it as a complex type, otherwise just updated the value
                if (this.IsComplexType(t.Types[i]))
                    this.UpdateComplexType(t.Types[i], prevType, newType);
                else if (t.Types[i] == prevType)
                    t.Types[i] = newType;
            }
        }

        /// <summary>
        /// Infer an anonymous previous type by unify it with the new inferred type
        /// </summary>
        /// <param name="prevType">Previous anonymous type</param>
        /// <param name="newType">New inferred type</param>
        private void UnifyTypes(Object prevType, Object newType)
        {
            if (prevType is Function prevFuncType)
            {
                if (newType is Anonymous)
                {
                    this.UnifyTypes(newType, prevType);
                }
                else
                {
                    var newFuncType = newType as Function;
                    for (int i = 0; i < prevFuncType.Parameters.Count; i++)
                    {
                        var oldParamSymbol = prevFuncType.Parameters[i];
                        var newParamSymbol = newFuncType.Parameters[i];

                        this.UnifyTypes(oldParamSymbol, newParamSymbol);
                    }

                    // If return type is a complex type, infer it as a complex type, otherwise just updated the value
                    this.UnifyTypes(prevFuncType.Return, newFuncType.Return);
                }
            }
            else if (prevType is Tuple prevTupleType)
            {
                if (newType is Anonymous)
                {
                    this.UnifyTypes(newType, prevType);
                }
                else
                {
                    var newTupleType = newType as Tuple;
                    for (int i = 0; i < prevTupleType.Types.Count; i++)
                    {
                        // If type is a complex type, infer it as a complex type, otherwise just updated the value
                        prevTupleType.Types[i] = newTupleType.Types[i];
                    }
                }
            }
            else if (prevType is Anonymous prevAnonType && prevType != newType)
            {
                // Infer each symbol type under the previous anonymous inferred type
                this.assumptions[prevAnonType].ForEach(s => this.InferSymbolType(s, prevAnonType, newType));

                // If the new inferred type is also an anonymous type, generate all the constraints for the new anonymous type
                if (this.IsTypeAssumption(newType))
                    this.assumptions[prevAnonType].ForEach(s => this.AddTypeDependency(newType, s));

                // Remove all the resolved types
                this.assumptions.Remove(prevAnonType);
            }
            else
            {

            }
        }

        #endregion
    }
}
