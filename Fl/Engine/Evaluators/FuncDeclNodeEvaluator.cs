// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Fl.Engine.StdLib;
using Fl.Parser.Ast;
using System;
using System.Collections.Generic;
using System.Text;

namespace Fl.Engine.Evaluators
{
    public class Func : FlCallable
    {
        private Token _Identifier;
        private AstParametersNode _Params;
        private List<AstNode> _Body;

        public Func(Token name, AstParametersNode parameters, List<AstNode> body)
        {
            _Identifier = name;
            _Params = parameters;
            _Body = body;
        }

        public override string Name => _Identifier.Value.ToString();

        public AstParametersNode Parameters => _Params;

        public override ScopeEntry Invoke(AstEvaluator evaluator, List<ScopeEntry> args)
        {
            if (args.Count != _Params.Parameters.Count)
                throw new AstWalkerException($"Function {_Identifier.Value} expects {_Params.Parameters.Count} arguments but received {args.Count}");

            evaluator.NewScope(ScopeType.Function);
            for (int i=0; i < _Params.Parameters.Count; i++)
            {
                evaluator.CurrentScope.NewSymbol(_Params.Parameters[i].Value.ToString(), args[i]);
            }
            try
            {
                foreach (var decl in _Body)
                {
                    decl.Exec(evaluator);
                    if (evaluator.CurrentScope.MustReturn)
                        return evaluator.CurrentScope.ReturnValue;
                }
            }
            finally
            {
                evaluator.DestroyScope();
            }
            return new ScopeEntry(ScopeEntryType.Null, null);
        }
    }

    class FuncDeclNodeEvaluator : INodeEvaluator<AstEvaluator, AstFuncDeclNode, ScopeEntry>
    {
        public ScopeEntry Evaluate(AstEvaluator evaluator, AstFuncDeclNode funcdecl)
        {
            var func = new Func(funcdecl.Identifier, funcdecl.Parameters, funcdecl.Body);
            evaluator.CurrentScope.NewSymbol(funcdecl.Identifier.Value.ToString(), func);
            return func;
        }
    }
}
