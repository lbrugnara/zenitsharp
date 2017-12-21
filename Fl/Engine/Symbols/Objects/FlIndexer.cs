// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using System;
using System.Collections.Generic;

namespace Fl.Engine.Symbols.Objects
{
    public class FlIndexer : FlMethod
    {
        private int _ParamsCount;
        private string _Name;

        public FlIndexer(int paramsCount, FlFunction.BoundFunction body)
            : base("indexer", body)
        {
            _ParamsCount = paramsCount;
            _Name = $"{base.Name}@{_ParamsCount}";
        }

        public override string Name => _Name;
    }
}
