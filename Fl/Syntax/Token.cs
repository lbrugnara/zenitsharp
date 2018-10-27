// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

namespace Fl.Syntax
{
    public class Token
    {
        public TokenType Type;
        public string Value;
        public int Line;
        public int Col;

        public override string ToString()
        {
            return $"{this.Value}: {this.Type} (L:{this.Line},C:{this.Col})";
        }
    }
}
