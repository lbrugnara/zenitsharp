// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Fl.Semantics.Types;
using Fl.Ast;
using Fl.Semantics.Symbols;

namespace Fl.Semantics.Inferrers
{
    public class CallableTypeInferrer : INodeVisitor<TypeInferrerVisitor, CallableNode, InferredType>
    {
        public InferredType Visit(TypeInferrerVisitor visitor, CallableNode node)
        {
            // Get the callable inferred type (and symbol)
            var inferredInfo = node.Target.Visit(visitor);

            // If the inferred type is an anonymous type, it means the target symbol's type has not
            // been defined yet, we need to infer the function type
            if (inferredInfo.TypeInfo.IsAnonymousType)
                return this.InferFromAnonymousCall(visitor, node, inferredInfo);

            // If the inferred type is a Function, we have more information about the target, we can infer
            // both parameter and arguments types
            if (inferredInfo.TypeInfo.Type is Function)
                return this.InferFromFunctionCall(visitor, node, inferredInfo);

            /*if (inferredInfo.Type is ClassMethod cm)
                return this.InferFromFunctionCall(visitor, node, cm.Type as Function);
            */
            /*if (inferredInfo.TypeInfo.Type is Class c)
                return this.InferFromConstructorCall(visitor, node, inferredInfo);*/

            throw new System.Exception($"Cannot invoke a non-function object");
        }
        /*
        private InferredType InferFromConstructorCall(TypeInferrerVisitor visitor, CallableNode node, InferredType inferredInfo)
        {
            Class classType = inferredInfo.TypeInfo.Type as Class;

            var classScope = visitor.SymbolTable.Global.GetNestedScope(ScopeType.Class, inferredInfo.Symbol.Name);

            var ctorSymbol = classScope.TryGetSymbol(inferredInfo.Symbol.Name);

            return new InferredType(new TypeInfo(new ClassInstance(classType)));
        }*/

        private InferredType InferFromAnonymousCall(TypeInferrerVisitor visitor, CallableNode node, InferredType inferred)
        {
            // The function we need to infer here is a Function type
            Function funcType = new Function();

            // Iterate over the function arguments and infer funcType's types
            for (var i = 0; i < node.Arguments.Count; i++)
            {
                // Get the inferred argument type for this call
                var inferredParamType = node.Arguments.Expressions[i].Visit(visitor);

                // Define a funcType parameter using the argument's inferred type
                funcType.DefineParameterType(inferredParamType.TypeInfo.Type);
            }

            // We also need to infer the target's return type
            var rettype = visitor.Inferrer.NewAnonymousType();

            // Set the return type in the Function object
            funcType.SetReturnType(rettype.Type);

            // Replace the symbol's anonymous type with the new inferred type
            visitor.Inferrer.FindMostGeneralType(new TypeInfo(funcType), inferred.Symbol.TypeInfo);

            // This invocation will have the function's return type
            return new InferredType(rettype);
        }

        private InferredType InferFromFunctionCall(TypeInferrerVisitor visitor, CallableNode node, InferredType inferredType)
        {
            var funcType = inferredType.TypeInfo.Type as Function;
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
                //visitor.Inferrer.Unify(inferredParamType.TypeInfo, funcType.Parameters[i]);
            }

            // This invocation will have the function's return type
            return new InferredType(new TypeInfo(funcType.Return), (inferredType.Symbol as FunctionSymbol).ReturnSymbol);
        }
    }
}
