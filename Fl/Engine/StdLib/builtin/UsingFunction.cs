// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Fl.Engine.Symbols;
using Fl.Engine.Symbols.Objects;
using System.Collections.Generic;

namespace Fl.Engine.StdLib.builtin
{
    public class UsingFunction : FlFunction
    {
        public override string Name => "using";

        public override FlObject Invoke(SymbolTable symboltable, List<FlObject> args)
        {
            /*args.ForEach(e => {
                if (e.ObjectType != NamespaceType.Value)
                    throw new AstWalkerException($"{e} is not a namespace");
                symboltable.Using((e as FlNamespace));
            });*/
            return null;
        }
    }
}
