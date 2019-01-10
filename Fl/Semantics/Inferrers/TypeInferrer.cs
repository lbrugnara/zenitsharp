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
        public ITypeSymbol NewAnonymousType()
        {
            return new AnonymousSymbol($"'{this.namegen.Generate()}");
        }

        public ITypeSymbol FindMostGeneralType(ITypeSymbol left, ITypeSymbol right)
        {
            if (!this.CanUnify(left.BuiltinType, right.BuiltinType))
                throw new System.Exception($"Cannot unify types {left} and {right}");

            if (left is AnonymousSymbol && !(right is AnonymousSymbol))
                return right;

            if (right is AnonymousSymbol && !(left is AnonymousSymbol))
                return left;

            if (left is IPrimitiveSymbol && right is IPrimitiveSymbol)
                return new PrimitiveSymbol(this.typeSystem.GetCommonAncestor(left.BuiltinType, right.BuiltinType), left.Parent ?? right.Parent);

            // If the types are structural types and are the same built-in type
            // they can be "merged"
            if (left is IComplexSymbol && right is IComplexSymbol)
            {
                IComplexSymbol type = null;

                switch (left.BuiltinType)
                {
                    case BuiltinType.Function:
                        type = new FunctionSymbol(this.namegen.Generate(), left.Parent ?? right.Parent);
                        break;
                    case BuiltinType.Tuple:
                        type = new TupleSymbol(this.namegen.Generate(), left.Parent ?? right.Parent);
                        break;
                    case BuiltinType.Object:
                        type = new ObjectSymbol(this.namegen.Generate(), left.Parent ?? right.Parent);
                        break;
                }
                
                foreach (var prop in (left as IComplexSymbol).Properties)
                    type.Properties[prop.Key] = prop.Value;

                foreach (var func in (left as IComplexSymbol).Functions)
                    type.Functions[func.Key] = func.Value;
                
                foreach (var prop in (right as IComplexSymbol).Properties)
                    if (!type.Properties.ContainsKey(prop.Key))
                        type.Properties[prop.Key] = prop.Value;

                foreach (var func in (right as IComplexSymbol).Functions)
                    if (!type.Functions.ContainsKey(func.Key))
                        type.Functions[func.Key] = func.Value;

                return type;
            }

            return this.NewAnonymousType();
        }

        public void Unify(ITypeSymbol generalType, params ITypeSymbol[] constructs)
        {
            foreach (var construct in constructs.Where(c => c != null))
            {
                // In variable assignment we try to infer
                //if (!construct.Equals(generalType))
                    //construct.ChangeType(generalType.Type);
            }
        }

        public void Unify(ISymbolTable symtable, ITypeSymbol generalType, params IBoundSymbol[] symbols)
        {
            foreach (var symbol in symbols)
            {
                if (symbol.TypeSymbol.Equals(generalType))
                    continue;

                symtable.Remove(symbol.Name);
                symtable.Insert(symbol.Name, generalType, symbol.Access, symbol.Storage);
            }
        }

        #endregion

        public bool CanUnify(ITypeSymbol t1, ITypeSymbol t2) => this.CanUnify(t1.BuiltinType, t2.BuiltinType);

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
