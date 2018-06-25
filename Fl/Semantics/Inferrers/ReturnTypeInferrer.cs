// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Fl.Semantics.Exceptions;
using Fl.Ast;
using Fl.Semantics.Types;
using System.Linq;
using Fl.Semantics.Symbols;

namespace Fl.Semantics.Inferrers
{
    class ReturnTypeInferrer : INodeVisitor<TypeInferrerVisitor, AstReturnNode, InferredType>
    {
        public InferredType Visit(TypeInferrerVisitor visitor, AstReturnNode rnode)
        {
            if (!visitor.SymbolTable.InFunction)
                throw new ScopeOperationException("Invalid return statement in a non-function block");

            var returnSymbol = visitor.SymbolTable.GetSymbol("@ret");

            Type type = null;
            Symbol symbol = null;

            var returnInferredType = rnode.ReturnTuple?.Visit(visitor);

            // void
            if (returnInferredType == null)
                type = Void.Instance;
            else
                type = returnInferredType?.Type;

            symbol = returnInferredType?.Symbol;

            // Just one lement is like
            //  return 1;
            //  return 2;
            //  etc
            if ((type is Tuple t) && t.Types.Count == 1)
                type = t.Types.First();            

            // TODO: We cannot infer recursive declarations, use this checks by now
            if (type is Function f)
            {
                var callingFunctionType = visitor.SymbolTable.CurrentScope.Parent.GetSymbol(visitor.SymbolTable.CurrentScope.Uid).Type as Function;

                if (f.Return == callingFunctionType.Return)
                    throw new SymbolException($"Function '{visitor.SymbolTable.CurrentScope.Uid}' cannot return its own type.");

                Scope s = visitor.SymbolTable.CurrentScope.Parent;
                while (s.IsFunction)
                {
                    var ctype = s.Parent.GetSymbol(s.Uid).Type as Function;

                    if (f.Return == ctype.Return)
                        throw new SymbolException($"Function '{visitor.SymbolTable.CurrentScope.Uid}' cannot return enclosing function '{s.Uid}' type.");

                    s = s.Parent;
                }
            }

            visitor.Inferrer.MakeConclusion(type, returnSymbol.Type);

            return new InferredType(type, symbol);
        }
    }
}
