// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Fl.Helpers;
using Fl.Semantics.Symbols;
using Fl.Semantics.Types;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Fl.Semantics.Inferrers
{
    public class TypeInferrer
    {
        #region Private fields

        /// <summary>
        /// Type system's information
        /// </summary>
        private readonly TypeSystem typeSystem;

        /// <summary>
        /// Constraints between anonymous types. The symbol used as key, is the
        /// common ancestor of the 2 descendant anonymous types. If any of the
        /// descendants of a type is resolved to be a non-anonymous type, the
        /// hierarchy must be updated correctly
        /// </summary>
        private readonly Dictionary<AnonymousSymbol, Descendant> constraints;

        /// <summary>
        /// All the symbols that are assumed to be of a certain anonymous type, must be
        /// tracked in order to be updated accordingly once the anonymous type is resolved 
        /// </summary>
        private readonly Dictionary<AnonymousSymbol, List<IBoundSymbol>> trackedSymbols;

        /// <summary>
        /// Inferred types
        /// </summary>
        private readonly Dictionary<AnonymousSymbol, ITypeSymbol> inferredTypes;

        #endregion

        #region Constructors

        public TypeInferrer()
        {
            this.typeSystem = new TypeSystem();
            this.constraints = new Dictionary<AnonymousSymbol, Descendant>();
            this.trackedSymbols = new Dictionary<AnonymousSymbol, List<IBoundSymbol>>();
            this.inferredTypes = new Dictionary<AnonymousSymbol, ITypeSymbol>();
        }

        #endregion

        #region Public members

        /// <summary>
        /// Create a new anonymous type
        /// </summary>
        /// <returns></returns>
        public AnonymousSymbol NewAnonymousType(IBoundSymbol boundSymbol = null)
        {
            var asym = new AnonymousSymbol();

            this.constraints[asym] = new Descendant();
            this.trackedSymbols[asym] = new List<IBoundSymbol>();

            if (boundSymbol != null)
                this.trackedSymbols[asym].Add(boundSymbol);

            return asym;
        }

        public void TrackSymbol(IBoundSymbol symbol, AnonymousSymbol asym)
        {
            this.trackedSymbols[asym].Add(symbol);
        }

        private void UpdateConstraints(AnonymousSymbol asym, ITypeSymbol type)
        {
            var constraints = this.constraints.Where(kvp => kvp.Value.Left == asym || kvp.Value.Right == asym).ToList();

            foreach (var constraint in constraints)
            {
                if (constraint.Value.Left == asym)
                    this.inferredTypes[constraint.Value.Left] = type;
                else
                    this.inferredTypes[constraint.Value.Right] = type;

                if (!(constraint.Value.Left is AnonymousSymbol) && !(constraint.Value.Right is AnonymousSymbol))
                {
                    this.inferredTypes[constraint.Key] = this.FindMostGeneralType(constraint.Value.Left, constraint.Value.Right);
                    this.trackedSymbols[constraint.Key].ForEach(symbol => this.Unify(null, this.inferredTypes[constraint.Key], symbol));
                    this.UpdateConstraints(constraint.Key, type);
                }
                else
                {
                    this.UpdateConstraints(constraint.Key, type);
                    this.inferredTypes[constraint.Key] = type;
                    this.trackedSymbols[constraint.Key].ForEach(symbol => this.Unify(null, type, symbol));
                }
            }
        }

        public ITypeSymbol FindMostGeneralType(ITypeSymbol t1, ITypeSymbol t2)
        {
            if (t1 == null && t2 == null)
                return null;

            if (!this.CanUnify(t1.BuiltinType, t2.BuiltinType))
                return null;

            if (t1 is AnonymousSymbol anonT1 && !(t2 is AnonymousSymbol))
            {
                this.UpdateConstraints(anonT1, t2);

                return t2;
            }

            if (t2 is AnonymousSymbol anonT2 && !(t1 is AnonymousSymbol))
            {
                this.UpdateConstraints(anonT2, t1);

                return t1;
            }

            if (t1 is IPrimitiveSymbol && t2 is IPrimitiveSymbol)
                return new PrimitiveSymbol(this.typeSystem.GetCommonAncestor(t1.BuiltinType, t2.BuiltinType), t1.Parent ?? t2.Parent);

            // If the types are structural types and are the same built-in type
            // they can be "merged"
            if (t1 is IComplexSymbol && t2 is IComplexSymbol)
            {
                /*IComplexSymbol type = null;

                switch (left.BuiltinType)
                {
                    case BuiltinType.Function:
                        type = new FunctionSymbol(this.namegen.Generate(), this.NewAnonymousType(), left.Parent ?? right.Parent);
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

                return type;*/
            }

            var newType = this.NewAnonymousType();

            this.constraints[newType] = new Descendant((AnonymousSymbol)t1, (AnonymousSymbol)t2);
            this.trackedSymbols[newType] = new List<IBoundSymbol>();

            return newType;

        }

        public void Unify(ISymbolTable symtable, ITypeSymbol generalType, params IBoundSymbol[] symbols)
        {
            foreach (var symbol in symbols)
            {
                if (symbol.TypeSymbol.Equals(generalType))
                    continue;

                symbol.ChangeType(generalType);

                if (generalType is AnonymousSymbol asym)
                    this.trackedSymbols[asym].Add(symbol);
            }
        }

        public string ToDebugString()
        {
            var sb = new StringBuilder();

            var titleIndent = "  ";
            var memberIndent = "    ";

            sb.AppendLine("[Inference]");

            // Constraints
            sb.AppendLine($"{titleIndent}Constraints {{");
            foreach (var constraint in this.constraints)
            {
                sb.AppendLine($"{memberIndent}{constraint.Key}: {constraint.Value}");
            }
            sb.AppendLine($"{titleIndent}}}");

            // Tracked symbols
            sb.AppendLine($"{titleIndent}Tracked Symbols {{");
            foreach (var tracked in this.trackedSymbols.Where(ts => ts.Value.Any()))
            {
                sb.Append($"{memberIndent}{tracked.Key}: [ ");
                sb.Append($"{string.Join(", ", tracked.Value.Select(s => s.ToValueString()))}");
                sb.AppendLine($" ]");
            }
            sb.AppendLine($"{titleIndent}}}");

            // Inferred types
            sb.AppendLine($"{titleIndent}Inferred Types {{");
            foreach (var type in this.inferredTypes)
            {
                sb.AppendLine($"{memberIndent}{type.Key}: {type.Value.ToValueString()}");
            }
            sb.AppendLine($"{titleIndent}}}");

            return sb.ToString();
        }

        #endregion

        #region Private members

        private class Descendant
        {
            public AnonymousSymbol Left { get; set; }
            public AnonymousSymbol Right { get; set; }

            public Descendant()
            {
            }

            public Descendant(AnonymousSymbol left, AnonymousSymbol right = null)
            {
                this.Left = left;
                this.Right = right;
            }

            public override string ToString()
            {
                if (this.Left == null && this.Right == null)
                    return "()";

                if (this.Left != null && this.Right != null)
                    return $"({this.Left}, {this.Right})";

                return $"({this.Left ?? this.Right})";
            }
        }

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

        #endregion
    }
}
