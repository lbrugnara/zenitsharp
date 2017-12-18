﻿// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Fl.Engine.IL.Instructions.Operands;
using Fl.Engine.StdLib;
using Fl.Engine.Symbols;
using Fl.Engine.Symbols.Objects;
using Fl.Engine.Symbols.Types;
using Fl.Parser.Ast;
using System;
using System.Collections.Generic;
using System.Text;

namespace Fl.Engine.IL.Generators
{
    class NullCoalescingILGenerator : INodeVisitor<AstILGenerator, AstNullCoalescingNode, Operand>
    {
        public Operand Visit(AstILGenerator generator, AstNullCoalescingNode nullc)
        {
            return null;
        }
    }
}