// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Fl.Parser.Ast;
using System;
using System.Collections.Generic;
using System.Text;

namespace Fl.Parser.Ast
{
    public interface INodeEvaluator<W, N, R> 
        where N : AstNode 
        where R : class 
        where W : IAstWalker<R>
    {
        R Evaluate(W walker, N node);
    }
}
