﻿// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Fl.Engine.Symbols;
using Fl.Engine.Symbols.Objects;
using Fl.Engine.Symbols.Types;
using Fl.Parser.Ast;
using System;
using System.Collections.Generic;
using System.Text;

namespace Fl.Engine.Evaluators
{
    public class Func : FlFunction
    {
        private static int UnboundLambda = 1;
        private AstEvaluator _Evaluator;
        private Token _Identifier;
        private AstParametersNode _Params;
        private List<AstNode> _Body;
        private Scope _Env;
        protected override Func<FlObject, List<FlObject>, FlObject> Body { get; }

        public Func(AstEvaluator eval, Token name, AstParametersNode parameters, List<AstNode> body, Scope env = null)
            : base (name.Type == TokenType.RightArrow ? null : name.Value.ToString(), null, null)
        {
            Body = (self, args) => InternalInvoke(eval.Symtable, args);
            _Evaluator = eval;
            _Identifier = name;
            _Params = parameters;
            _Body = body;
            _Env = env;
        }

        public bool IsLambda => _Identifier.Type == TokenType.RightArrow;

        public AstParametersNode Parameters => _Params;

        public override FlObject Clone()
        {
            return new Func(_Evaluator, _Identifier, _Params, _Body, _Env);
        }

        protected FlObject InternalInvoke(SymbolTable symboltable, List<FlObject> args)
        {
            if (args.Count != _Params.Parameters.Count)
                throw new AstWalkerException($"Function {_Identifier.Value} expects {_Params.Parameters.Count} arguments but received {args.Count}");

            for (int i=0; i < _Params.Parameters.Count; i++)
            {
                symboltable.AddSymbol(
                    _Params.Parameters[i].Value.ToString(),
                    // by-value
                    new Symbol(SymbolType.Variable),
                    args[i].Clone()
                );
            }
            FlObject ret = null;
            try
            {                
                foreach (var decl in _Body)
                {
                    ret = decl.Exec(_Evaluator);
                    if (symboltable.MustReturn)
                        return symboltable.ReturnValue;
                }
            }
            catch
            {

            }
            if (_Identifier.Type == TokenType.RightArrow && ret != null)
                return ret;
            return FlNull.Value;
        }
    }

    class FuncDeclNodeEvaluator : INodeEvaluator<AstEvaluator, AstFuncDeclNode, FlObject>
    {
        public FlObject Evaluate(AstEvaluator evaluator, AstFuncDeclNode funcdecl)
        {
            var func = new Func(evaluator, funcdecl.Identifier, funcdecl.Parameters, funcdecl.Body, evaluator.Symtable.IsFunctionEnv() ? evaluator.Symtable.GetCurrentFunctionEnv() : null);

            if (func.IsLambda)
                return func;

            if (evaluator.Symtable.HasSymbol(func.Name))
            {
                evaluator.Symtable.UpdateSymbol(func.Name, func);
            }
            else
            {
                evaluator.Symtable.AddSymbol(func.Name, new Symbol(SymbolType.Variable), func);
            }
            return func;
        }
    }
}
