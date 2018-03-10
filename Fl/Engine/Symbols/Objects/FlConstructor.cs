// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using System;
using System.Collections.Generic;

namespace Fl.Engine.Symbols.Objects
{
    public class FlConstructor: FlInstanceMethod
    {
        private int paramsCount;
        private Action<FlObject, List<FlObject>> body;
        private string name;

        public FlConstructor(Action<FlObject, List<FlObject>> body)
            : base("constructor", (self, args) => { body(self, args); return self; })
        {
            this.body = body;
            this.paramsCount = -1;
            this.name = $"{base.Name}@params";
        }

        public FlConstructor(int paramsCount, Action<FlObject, List<FlObject>> body)
            : base("constructor", (self, args) => { body(self, args); return self; })
        {
            this.body = body;
            this.paramsCount = paramsCount;
            this.name = $"{base.Name}@{this.paramsCount}";
        }

        public override string Name => this.name;
    }
}
