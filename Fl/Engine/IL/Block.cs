// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using System.Collections.Generic;
using Fl.Engine.IL.Instructions.Operands;

namespace Fl.Engine.IL
{

    public class Block
    {
        public BlockType Type { get; }
        Dictionary<string, SymbolOperand> Symbols { get; }
        public string Name { get; }
        public Label EntryPoint { get; }
        public Label ExitPoint { get; }

        public Block(BlockType type, string name, Label entryPoint, Label exitPoint)
        {
            this.Type = type;
            this.Symbols = new Dictionary<string, SymbolOperand>();
            this.Name = name;
            this.EntryPoint = entryPoint;
            this.ExitPoint = exitPoint;
        }

        public bool HasSymbol(string name) => Symbols.ContainsKey(name);

        public SymbolOperand this[string name]
        {
            get
            {
                return Symbols[name];
            }
            set
            {
                Symbols[name] = value;
            }
        }
    }
}
