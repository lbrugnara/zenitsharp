// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Fl.Semantics.Types;
using System.Collections.Generic;

namespace Fl.Semantics.Symbols
{
    public class FunctionSymbol : ComplexSymbol
    {
        public List<string> Parameters { get; set; }
        public Symbol ReturnSymbol { get; private set; }

        public FunctionSymbol(string name, ISymbolContainer parent)
            : base (name, new TypeInfo(new Function()), Access.Public, Storage.Constant, parent)
        {
            this.Parameters = new List<string>();

            // Create the @ret symbol and save it into the function's symbol table
            this.ReturnSymbol = new Symbol("@ret", null, Access.Public, Storage.Mutable, this);
            this.Insert(this.ReturnSymbol);
        }

        private Function FunctionType => this.TypeInfo.Type as Function;

        public void UpdateReturnType(TypeInfo typeInfo)
        {
            // Update the @ret symbol's type
            this.ReturnSymbol.TypeInfo = typeInfo ?? throw new System.ArgumentNullException(nameof(typeInfo), "Return type cannot be null");

            // Update the function's return type
            this.FunctionType.SetReturnType(typeInfo.Type);
        }

        public Symbol CreateParameter(string name, TypeInfo typeInfo, Storage storage)
        {
            // Update the function's type (with parameter type)
            this.FunctionType.DefineParameterType(typeInfo.Type);

            // Create the parameter symbol
            var symbol = new Symbol(name, typeInfo, Access.Private, storage, this);
            
            // Insert it into the Function's symbol table
            this.Insert(symbol);

            // Keep track of the parameter name (and position)
            this.Parameters.Add(symbol.Name);

            // Return a reference to the created symbol
            return symbol;
        }
    }
}
