using Fl.Engine.StdLib;
using System;
using System.Collections.Generic;
using System.Text;

namespace Fl.Engine.Symbols
{
    public class SymbolTable
    {
        private Scope _Global;
        private Stack<Scope> _Scopes;

        public SymbolTable()
        {
            _Scopes = new Stack<Scope>();
            _Global = new Scope();
            _Scopes.Push(_Global);
            StdLibInitializer.Import(CurrentScope);
        }

        public Scope CurrentScope => _Scopes.Peek();

        public void NewScope(ScopeType scopeType)
        {
            Scope enclosing = CurrentScope;
            _Scopes.Push(new Scope(scopeType, _Global, enclosing));
        }

        public void DestroyScope()
        {
            _Scopes.Pop();
        }
    }
}
