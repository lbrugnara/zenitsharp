// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file


namespace Fl.Engine.IL.Instructions
{
    public class Label
    {
        public string Name { get; }

        public Label(string name)
        {
            this.Name = name;
        }

        public override string ToString()
        {
            return $"{this.Name}:";
        }
    }
}
