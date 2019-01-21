// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Fl.Semantics.Symbols;
using Fl.Semantics.Symbols.Types;
using Fl.Semantics.Symbols.Types.Specials;
using Fl.Semantics.Symbols.Values;
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
        public AnonymousSymbol NewAnonymousTypeFor(IBoundSymbol symbol = null)
        {
            var asym = new AnonymousSymbol();

            this.constraints[asym] = new Descendant();
            this.trackedSymbols[asym] = new List<IBoundSymbol>();

            if (symbol != null)
            {
                symbol.ChangeType(asym);
                this.trackedSymbols[asym].Add(symbol);
            }

            return asym;
        }

        private void UpdateConstraints(AnonymousSymbol asym, ITypeSymbol type)
        {
            var constraints = this.constraints.Where(kvp => kvp.Value.Left == asym || kvp.Value.Right == asym).ToList();

            foreach (var constraint in constraints)
            {
                ITypeSymbol t1 = constraint.Value.Left;
                ITypeSymbol t2 = constraint.Value.Right;

                if (constraint.Value.Left == asym)
                    this.inferredTypes[constraint.Value.Left] = t1 = type;
                else
                    this.inferredTypes[constraint.Value.Right] = t2 = type;

                if (!(t1 is AnonymousSymbol) && !(t2 is AnonymousSymbol))
                    this.inferredTypes[constraint.Key] = this.FindMostGeneralType(t1, t2);
                else if (constraint.Key is AnonymousSymbol)
                    this.UpdateConstraints(constraint.Key, type);

                // this.trackedSymbols[constraint.Key].ForEach(symbol => this.Unify(null, this.inferredTypes[constraint.Key], symbol));


                /*// If the constraint's descendants are inferred, find the most general type of it
                if (this.inferredTypes.ContainsKey(constraint.Value.Left) && this.inferredTypes.ContainsKey(constraint.Value.Right))
                {
                    this.inferredTypes[constraint.Key] = this.FindMostGeneralType(this.inferredTypes[constraint.Value.Left], this.inferredTypes[constraint.Value.Right]);
                    this.trackedSymbols[constraint.Key].ForEach(symbol => this.Unify(null, this.inferredTypes[constraint.Key], symbol));
                    this.UpdateConstraints(constraint.Key, type);
                }
                else
                {
                    this.inferredTypes[constraint.Key] = this.FindMostGeneralType(constraint.Value.Left, constraint.Value.Right);
                    this.trackedSymbols[constraint.Key].ForEach(symbol => this.Unify(null, type, symbol));
                    // Update the constraint to be of the mostGeneralType
                    this.UpdateConstraints(constraint.Key, type);
                    //this.UpdateConstraints(constraint.Key, type);
                    //this.inferredTypes[constraint.Key] = type;
                    //this.trackedSymbols[constraint.Key].ForEach(symbol => this.Unify(null, type, symbol));
                }*/
            }
        }

        /// <summary>
        /// This method tries to get the most general type between two types. This process can create
        /// new types, or just return one of the provided types based on the relationship of these types:
        /// <list type="number">
        ///     <item>t1 and t2 cannot be unified: return null, and let the caller handle the situation</item>
        ///     <item>t1 and t2 are the special type BuiltinType.None: return NoneSymbol</item>
        ///     <item>t1 or t2 are the special type BuiltinType.None: return the type that is not a NoneSymbol</item>
        ///     <item>t1 and t2 are primitives: return the common ancestor</item>
        ///     <item>t1 or t2 are anonymous (but not both): update the anonymous constraint with the non-anonymous type information (see <see cref="UpdateConstraints(AnonymousSymbol, ITypeSymbol)"/>)</item>
        ///     <item>t1 and t2 are anonymous: return a new anonymous type that will be the most general type that encloses both anonymous types, and track the relationship on <see cref="constraints"/></item>
        ///     <item>t1 and t2 are structural: FIXME</item>
        /// </list>
        /// </summary>
        /// <param name="t1">Type 1</param>
        /// <param name="t2">Type 2</param>
        /// <returns>Most general type that encloses both t1 and t2</returns>
        public ITypeSymbol FindMostGeneralType(ITypeSymbol t1, ITypeSymbol t2)
        {
            if (t1 == null || t2 == null)
                return null;

            // Cannot be unified
            if (!this.CanUnify(t1.BuiltinType, t2.BuiltinType))
                return null;

            // t1 and t2 are NoneSymbol
            if (t1.BuiltinType == BuiltinType.None && t2.BuiltinType == BuiltinType.None)
                return new NoneSymbol();

            // t1 or t2 are NoneSymbol
            if (t1.BuiltinType == BuiltinType.None)
                return t2;

            if (t2.BuiltinType == BuiltinType.None)
                return t1;

            // t1 and t2 are primitives
            if (t1 is IPrimitiveSymbol && t2 is IPrimitiveSymbol)
                return new PrimitiveSymbol(this.typeSystem.GetCommonAncestor(t1.BuiltinType, t2.BuiltinType), t1.Parent ?? t2.Parent);

            // t1 and t2 are anonymous types
            if (t1 is AnonymousSymbol at1 && t2 is AnonymousSymbol at2)
            {
                if (t1 == t2)
                    return t1;

                var newType = this.NewAnonymousTypeFor();

                this.constraints[newType] = new Descendant(at1, at2);
                this.trackedSymbols[newType] = new List<IBoundSymbol>();

                return newType;
            }

            // t1 or t2 are anonymous symbol
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

            // FIXME: Structural types
            return t1;

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
        }

        public void Unify(ISymbolTable symtable, ITypeSymbol generalType, params IBoundSymbol[] symbols)
        {
            foreach (var symbol in symbols)
            {
                if (symbol.TypeSymbol.Equals(generalType))
                    continue;

                var oldType = symbol.TypeSymbol as AnonymousSymbol;

                if (oldType != null)
                {
                    var oldSymbols = this.trackedSymbols[oldType];
                    this.trackedSymbols[oldType] = new List<IBoundSymbol>();

                    oldSymbols.Where(s => s != symbol).ToList().ForEach(os => {
                        os.ChangeType(generalType);
                        if (generalType is AnonymousSymbol anonsym)
                            this.trackedSymbols[anonsym].Add(os);
                    });
                }

                symbol.ChangeType(generalType);
                if (generalType is AnonymousSymbol asym)
                    this.trackedSymbols[asym].Add(symbol);

                /*// If type changing
                var oldType = symbol.TypeSymbol as AnonymousSymbol;

                if (oldType != null && generalType is AnonymousSymbol)
                {
                    this.trackedSymbols[oldType].RemoveAll(bs => bs == symbol);
                }

                symbol.ChangeType(generalType);

                if (generalType is AnonymousSymbol asym)
                {
                    this.trackedSymbols[asym].Add(symbol);
                }

                if (oldType != null)
                    this.trackedSymbols[oldType].ForEach(s => this.Unify(null, generalType, s));*/
            }
        }

        public string ToDebugString()
        {
            var sb = new StringBuilder();

            var titleIndent = "  ";
            var memberIndent = "    ";

            sb.AppendLine("[Inference]");

            // Resolved Constraints
            sb.AppendLine($"{titleIndent}Constraints {{");
            foreach (var constraint in this.constraints)
            {
                var key = this.inferredTypes.ContainsKey(constraint.Key) ? $"{constraint.Key} : {this.inferredTypes[constraint.Key]}" : constraint.Key.ToString();

                sb.Append($"{memberIndent}{key} => ");

                string leftStr = null;
                string rightStr = null;

                if (constraint.Value.Left != null)
                    leftStr = this.inferredTypes.ContainsKey(constraint.Value.Left) ? $"{constraint.Value.Left} : {this.inferredTypes[constraint.Value.Left]}" : constraint.Value.Left.ToString();

                if (constraint.Value.Right != null)
                    rightStr = this.inferredTypes.ContainsKey(constraint.Value.Right) ? $"{constraint.Value.Right} : {this.inferredTypes[constraint.Value.Right]}" : constraint.Value.Right.ToString();

                if (leftStr == null && rightStr == null)
                    sb.AppendLine("()");
                else if (leftStr != null && rightStr != null)
                    sb.AppendLine($"({leftStr}, {rightStr})");
                else
                    sb.AppendLine($"({leftStr ?? rightStr})");
            }
            sb.AppendLine($"{titleIndent}}}");

            // Tracked symbols
            sb.AppendLine($"{titleIndent}Tracked Symbols {{");
            foreach (var tracked in this.trackedSymbols.Where(ts => ts.Value.Any()))
            {
                sb.Append($"{memberIndent}{tracked.Key} => [ ");
                sb.Append($"{string.Join(", ", tracked.Value.Select(s => s.ToValueString()))}");
                sb.AppendLine($" ]");
            }
            sb.AppendLine($"{titleIndent}}}");

            // Inferred types
            sb.AppendLine($"{titleIndent}Inferred Types {{");
            foreach (var type in this.inferredTypes)
            {
                sb.AppendLine($"{memberIndent}{type.Key} => {type.Value.ToValueString()}");
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
