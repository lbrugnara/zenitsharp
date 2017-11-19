// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Fl.Engine.Symbols;
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
        private Scope _Env;

        public Func(Token name, AstParametersNode parameters, List<AstNode> body, Scope env = null)
            : base ()
        {
            _Identifier = name;
            _Params = parameters;
            _Body = body;
            _Env = env;
        }

        public override string Name => _Identifier.Type == TokenType.RightArrow ? "<lambda>" : _Identifier.Value.ToString();

        public AstParametersNode Parameters => _Params;

        public override FlObject Invoke(AstEvaluator evaluator, List<FlObject> args)
        {
            if (args.Count != _Params.Parameters.Count)
                throw new AstWalkerException($"Function {_Identifier.Value} expects {_Params.Parameters.Count} arguments but received {args.Count}");

            evaluator.Symtable.NewScope(ScopeType.Function, _Env);
            
            for (int i=0; i < _Params.Parameters.Count; i++)
            {
                evaluator.Symtable.AddSymbol(
                    _Params.Parameters[i].Value.ToString(),
                    // by-value
                    new Symbol(args[i].Type, StorageType.Variable, new FlObject(args[i].Type, args[i].Value))
                );
            }
            FlObject ret = null;
            try
            {                
                foreach (var decl in _Body)
                {
                    ret = decl.Exec(evaluator);
                    if (evaluator.Symtable.MustReturn)
                        return evaluator.Symtable.ReturnValue;
                }
            }
            finally
            {
                evaluator.Symtable.DestroyScope();
            }
            if (_Identifier.Type == TokenType.RightArrow && ret != null)
                return ret;
            return new FlObject(ObjectType.Null, null);
        }
    }

    class FuncDeclNodeEvaluator : INodeEvaluator<AstEvaluator, AstFuncDeclNode, FlObject>
    {
        public FlObject Evaluate(AstEvaluator evaluator, AstFuncDeclNode funcdecl)
        {
            var func = new Func(funcdecl.Identifier, funcdecl.Parameters, funcdecl.Body, evaluator.Symtable.IsFunctionEnv() ? evaluator.Symtable.GetCurrentFunctionEnv() : null);
            if (evaluator.Symtable.HasSymbol(func.Name))
            {
                evaluator.Symtable.UpdateSymbol(func.Name, func);
            }
            else
            {
                evaluator.Symtable.AddSymbol(func.Name, new Symbol(ObjectType.Function, StorageType.Variable, func));
            }
            return func;
        }
    }
}
