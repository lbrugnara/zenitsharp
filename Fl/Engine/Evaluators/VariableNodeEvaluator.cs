// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Fl.Engine.Symbols;
using Fl.Engine.Symbols.Exceptions;
using Fl.Engine.Symbols.Objects;
using Fl.Engine.Symbols.Types;
using Fl.Parser;
using Fl.Parser.Ast;
using System;
using System.Linq;

namespace Fl.Engine.Evaluators
{
    class VariableNodeEvaluator : INodeEvaluator<AstEvaluator, AstVariableNode, FlObject>
    {
        public FlObject Evaluate(AstEvaluator evaluator, AstVariableNode vardecl)
        {
            switch (vardecl)
            {
                case AstVarDefinitionNode vardefnode:
                    return VarDefinitionNode(evaluator, vardefnode);

                case AstVarDestructuringNode vardestnode:
                    return VarDestructuringNode(evaluator, vardestnode);
            }
            throw new AstWalkerException($"Invalid variable declaration of type {vardecl.GetType().FullName}");
        }

        protected FlObject VarDefinitionNode(AstEvaluator evaluator, AstVarDefinitionNode vardecl)
        {
            // Get the variable type
            var type = vardecl.VarType;
            Func<FlObject> typeActivator = null;

            // If type is not null, get the activator for the type
            if (type != null && type.TypeToken.Type != TokenType.Variable)
            {
                // This will raise an exception if the type is not registered
                var clasz = evaluator.Symtable.GetSymbol(vardecl.VarType.TypeToken.Value.ToString()).Binding as FlClass;
                typeActivator = clasz.Activator;
            }

            // attributes
            bool isArray = type.Dimensions?.Count > 0; // By now allow 1-dimension arrays
            Symbol varsymbol = null;

            foreach (var tuple in vardecl.VarDefinitions)
            {
                var id = tuple.Item1;
                var initializer = tuple.Item2;

                // Ignore empty items, it could be a declaration of type var (a,,c) = (1,2,3)
                if (id == null)
                    continue;

                // Get the variable name                
                string varname = id.Value.ToString();

                // Build the Symbol information from an initializer or use null
                varsymbol = new Symbol(SymbolType.Variable);

                // If the activator is present (it is a typed declaration) initialize to the default value
                // If it is an implicit declaration, let it null
                evaluator.Symtable.AddSymbol(varname, varsymbol, typeActivator?.Invoke() ?? FlNull.Value);

                // If the declaration has a definition, update the binding. This will raise an exception if types do not match
                if (initializer != null)
                {
                    varsymbol.UpdateBinding(initializer.Exec(evaluator));
                }
            }
            return FlNull.Value;
        }

        protected FlObject VarDestructuringNode(AstEvaluator evaluator, AstVarDestructuringNode vardecl)
        {
            // Get the variable type
            var type = vardecl.VarType;
            ObjectType otype = null;

            // If type is not null, get the class to make sure the tuple initializer matches it
            if (type != null && type.TypeToken.Type != TokenType.Variable)
            {
                var clasz = evaluator.Symtable.GetSymbol(vardecl.VarType.TypeToken.Value.ToString()).Binding as FlClass;
                FlObject obj = clasz.Activator.Invoke();
                otype = obj.ObjectType;
            }

            // attributes
            bool isArray = type.Dimensions?.Count > 0; // By now allow 1-dimension arrays
            
            var initres = vardecl.DestructInit.Exec(evaluator);

            if (!(initres is FlTuple))
                throw new AstWalkerException($"Var destructuring cannot be initialized with object of type '{initres}'");

            FlTuple tupleinit = initres as FlTuple;

            if (otype != null && tupleinit.Value.Any(t => t.ObjectType != otype))
                throw new SymbolException($"Cannot implicitly convert type '{tupleinit.Value.First(t => t.ObjectType != otype).ObjectType}' to '{otype}'");

            //foreach (var variable in vardecl.Variables)
            for (int i=0; i < vardecl.Variables.Count; i++)
            {
                var varname = vardecl.Variables[i]?.Value?.ToString();

                // Ignore empty items, it could be a declaration of type var (a,,c) = (1,2,3)
                if (varname == null)
                    continue;

                // Build the Symbol information from an initializer or use null
                Symbol varsymbol = new Symbol(SymbolType.Variable);

                // Initialize
                evaluator.Symtable.AddSymbol(varname, varsymbol, tupleinit.Value.ElementAtOrDefault(i) ?? FlNull.Value);
            }
            return FlNull.Value;
        }
    }
}
