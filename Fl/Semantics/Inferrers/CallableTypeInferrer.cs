// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Fl.Semantics.Types;
using Fl.Ast;
using Fl.Semantics.Symbols;

namespace Fl.Semantics.Inferrers
{
    public class CallableTypeInferrer : INodeVisitor<TypeInferrerVisitor, CallableNode, ITypeSymbol>
    {
        public ITypeSymbol Visit(TypeInferrerVisitor visitor, CallableNode node)
        {
            // Get the callable inferred type (and symbol)
            var typeSymbol = node.Target.Visit(visitor);

            // If the inferred type is an anonymous type, it means the target symbol's type has not
            // been defined yet, we need to infer the function type
            if (typeSymbol is AnonymousSymbol)
                return this.InferFromAnonymousCall(visitor, node, typeSymbol);

            // If the inferred type is a Function, we have more information about the target, we can infer
            // both parameter and arguments types
            if (typeSymbol is FunctionSymbol)
                return this.InferFromFunctionCall(visitor, node, typeSymbol);

            /*if (inferredInfo.Type is ClassMethod cm)
                return this.InferFromFunctionCall(visitor, node, cm.Type as Function);
            */
            /*if (inferredInfo.ITypeSymbol.Type is Class c)
                return this.InferFromConstructorCall(visitor, node, inferredInfo);*/

            throw new System.Exception($"Cannot invoke a non-function object");
        }
        /*
        private IValueSymbol InferFromConstructorCall(TypeInferrerVisitor visitor, CallableNode node, IValueSymbol inferredInfo)
        {
            Class classType = inferredInfo.ITypeSymbol.Type as Class;

            var classScope = visitor.SymbolTable.Global.GetNestedScope(ScopeType.Class, inferredInfo.Symbol.Name);

            var ctorSymbol = classScope.TryGetSymbol(inferredInfo.Symbol.Name);

            return new IValueSymbol(new ITypeSymbol(new ClassInstance(classType)));
        }*/

        private ITypeSymbol InferFromAnonymousCall(TypeInferrerVisitor visitor, CallableNode node, ITypeSymbol inferred)
        {
            // The function we need to infer here is a Function type
            FunctionSymbol funcType = new FunctionSymbol(inferred.Name, visitor.Inferrer.NewAnonymousType(), inferred.Parent);

            // Iterate over the function arguments and infer funcType's types
            for (var i = 0; i < node.Arguments.Count; i++)
            {
                // Get the inferred argument type for this call
                var inferredParamType = node.Arguments.Expressions[i].Visit(visitor);

                funcType.CreateParameter(inferredParamType.Name, inferredParamType, Storage.Constant);
                // Define a funcType parameter using the argument's inferred type
                //funcType.DefineParameterType(inferredParamType.TypeSymbol.Type);
            }

            // We also need to infer the target's return type
            var rettype = visitor.Inferrer.NewAnonymousType();

            // Set the return type in the Function object
            funcType.UpdateReturnType(rettype);

            // Replace the symbol's anonymous type with the new inferred type
            visitor.Inferrer.FindMostGeneralType(funcType, inferred);

            // This invocation will have the function's return type
            return rettype;
        }

        private ITypeSymbol InferFromFunctionCall(TypeInferrerVisitor visitor, CallableNode node, ITypeSymbol inferredType)
        {
            var funcType = inferredType as FunctionSymbol;
            // Check parameters count
            // TODO: This is not needed to be here
            if (funcType.Parameters.Count != node.Arguments.Expressions.Count)
                throw new System.Exception($"Function {funcType.Name} expects {funcType.Parameters.Count} arguments but received {node.Arguments.Expressions.Count}");

            // Iterate over the function parameters and infer types if needed
            for (var i = 0; i < funcType.Parameters.Count; i++)
            {
                // Get the inferred argument type for this call
                var inferredParamType = node.Arguments.Expressions[i].Visit(visitor);

                // If possible, make conclusions about the inferred argument type and the parameter type
                //visitor.Inferrer.Unify(inferredParamType.ITypeSymbol, funcType.Parameters[i]);
            }

            // This invocation will have the function's return type
            return funcType.Return.TypeSymbol;
        }
    }
}
