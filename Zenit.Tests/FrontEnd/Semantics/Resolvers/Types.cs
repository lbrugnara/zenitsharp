using Microsoft.VisualStudio.TestTools.UnitTesting;
using Zenit.FrontEnd;
using Zenit.Semantics;
using Zenit.Semantics.Symbols;
using Zenit.Semantics.Symbols.Containers;
using Zenit.Semantics.Symbols.Types;
using Zenit.Semantics.Symbols.Types.References;
using Zenit.Semantics.Symbols.Types.Specials;
using Zenit.Semantics.Symbols.Variables;
using Zenit.Semantics.Types;
using Zenit.Syntax;

namespace Zenit.Tests
{
    [TestClass]
    public class Types
    {
        [TestMethod]
        public void PrimitiveTypes()
        {
            var source = @"
                var c = 'c';
                var b1 = true;
                var b2 = false;
                var i = 1;
                var f = 1.0f;
                var d = 2.0;
                var d2 = 2.0d;
                var m = 3.0M;
                var s = ""string"";
            ";

            var zenitc = new TestCompiler();
            zenitc.ResolveSymbols(source);

            Assert.IsTrue(this.ExpectVariableSymbol(zenitc, "c", BuiltinType.Char));
            Assert.IsTrue(this.ExpectVariableSymbol(zenitc, "b1", BuiltinType.Bool));
            Assert.IsTrue(this.ExpectVariableSymbol(zenitc, "b2", BuiltinType.Bool));
            Assert.IsTrue(this.ExpectVariableSymbol(zenitc, "i", BuiltinType.Int));
            Assert.IsTrue(this.ExpectVariableSymbol(zenitc, "f", BuiltinType.Float));
            Assert.IsTrue(this.ExpectVariableSymbol(zenitc, "d", BuiltinType.Double));
            Assert.IsTrue(this.ExpectVariableSymbol(zenitc, "d2", BuiltinType.Double));
            Assert.IsTrue(this.ExpectVariableSymbol(zenitc, "m", BuiltinType.Decimal));
            Assert.IsTrue(this.ExpectVariableSymbol(zenitc, "s", BuiltinType.String));
        }

        [TestMethod]
        public void NumberTypes()
        {
            var source = @"
                var i = 1;
                var f = 1.0f;
                var d = 2.0;
                var d2 = 2.0d;
                var m = 3.0M;

                var ii = i + i;
                var ff = f + f;
                var dd = d + d;
                var dd2 = d2 + d2;
                var mm = m + m;

                var i_f = i + f;
                var i_d = i + d;
                var i_m = i + m;
                var f_d = f + d;
                var f_m = f + m;
                var d_m = d + m;
            ";

            var zenitc = new TestCompiler();
            zenitc.ResolveSymbols(source);

            Assert.IsTrue(this.ExpectVariableSymbol(zenitc, "ii", BuiltinType.Int));
            Assert.IsTrue(this.ExpectVariableSymbol(zenitc, "ff", BuiltinType.Float));
            Assert.IsTrue(this.ExpectVariableSymbol(zenitc, "dd", BuiltinType.Double));
            Assert.IsTrue(this.ExpectVariableSymbol(zenitc, "dd2", BuiltinType.Double));
            Assert.IsTrue(this.ExpectVariableSymbol(zenitc, "mm", BuiltinType.Decimal));

            Assert.IsTrue(this.ExpectVariableSymbol(zenitc, "i_f", BuiltinType.Float));
            Assert.IsTrue(this.ExpectVariableSymbol(zenitc, "i_d", BuiltinType.Double));
            Assert.IsTrue(this.ExpectVariableSymbol(zenitc, "i_m", BuiltinType.Number));
            Assert.IsTrue(this.ExpectVariableSymbol(zenitc, "f_d", BuiltinType.Double));
            Assert.IsTrue(this.ExpectVariableSymbol(zenitc, "f_m", BuiltinType.Number));
            Assert.IsTrue(this.ExpectVariableSymbol(zenitc, "d_m", BuiltinType.Number));
        }

        [TestMethod]
        public void FunctionType()
        {
            var source = @"
                var func = (a) => a+1;
                var func2 = (int a) => {  };
                fn func3 (a) => 2*a;
                fn func4 (a) => a*2;
                fn func5 (a, string b) {};
                fn func6 () => ""test"";
                fn func7 () { return '2'; };
                fn func8 () {};
            ";

            var zenitc = new TestCompiler();
            zenitc.ResolveSymbols(source);

            // func
            Assert.IsTrue(this.ExpectVariableSymbol(zenitc, "func", BuiltinType.Function));
            Assert.IsTrue(this.ExpectVariableSymbol(zenitc, new[] { "func", "a" }, BuiltinType.Anonymous));
            var func = zenitc.SymbolTable.GetVariableSymbol("func").TypeSymbol as Function;
            Assert.IsTrue(func.Return.TypeSymbol.BuiltinType == BuiltinType.Int);
            Assert.IsTrue(func.Parameters.Count == 1);

            // func2
            Assert.IsTrue(this.ExpectVariableSymbol(zenitc, "func2", BuiltinType.Function));
            Assert.IsTrue(this.ExpectVariableSymbol(zenitc, new[] { "func2", "a" }, BuiltinType.Int));
            var func2 = zenitc.SymbolTable.GetVariableSymbol("func2").TypeSymbol as Function;
            Assert.IsTrue(func2.Return.TypeSymbol.BuiltinType == BuiltinType.Void);
            Assert.IsTrue(func2.Parameters.Count == 1);

            // func3
            Assert.IsTrue(this.ExpectTypeSymbol(zenitc, "func3", BuiltinType.Function));
            Assert.IsTrue(this.ExpectVariableSymbol(zenitc, new[] { "func3", "a" }, BuiltinType.Anonymous));
            var func3 = zenitc.SymbolTable.CurrentScope.Get<IType>("func3") as Function;
            Assert.IsTrue(func3.Return.TypeSymbol.BuiltinType == BuiltinType.Int);
            Assert.IsTrue(func3.Parameters.Count == 1);

            // func4
            Assert.IsTrue(this.ExpectTypeSymbol(zenitc, "func4", BuiltinType.Function));
            Assert.IsTrue(this.ExpectVariableSymbol(zenitc, new[] { "func4", "a" }, BuiltinType.Anonymous));
            var func4 = zenitc.SymbolTable.CurrentScope.Get<IType>("func4") as Function;
            Assert.IsTrue(func4.Return.TypeSymbol.BuiltinType == BuiltinType.Int);
            Assert.IsTrue(func4.Parameters.Count == 1);

            // func5
            Assert.IsTrue(this.ExpectTypeSymbol(zenitc, "func5", BuiltinType.Function));
            Assert.IsTrue(this.ExpectVariableSymbol(zenitc, new[] { "func5", "a" }, BuiltinType.Anonymous));
            Assert.IsTrue(this.ExpectVariableSymbol(zenitc, new[] { "func5", "b" }, BuiltinType.String));
            var func5 = zenitc.SymbolTable.CurrentScope.Get<IType>("func5") as Function;
            Assert.IsTrue(func5.Return.TypeSymbol.BuiltinType == BuiltinType.Void);
            Assert.IsTrue(func5.Parameters.Count == 2);

            // func6
            Assert.IsTrue(this.ExpectTypeSymbol(zenitc, "func6", BuiltinType.Function));
            var func6 = zenitc.SymbolTable.CurrentScope.Get<IType>("func6") as Function;
            Assert.IsTrue(func6.Return.TypeSymbol.BuiltinType == BuiltinType.String);
            Assert.IsTrue(func6.Parameters.Count == 0);

            // func7
            Assert.IsTrue(this.ExpectTypeSymbol(zenitc, "func7", BuiltinType.Function));
            var func7 = zenitc.SymbolTable.CurrentScope.Get<IType>("func7") as Function;
            Assert.IsTrue(func7.Return.TypeSymbol.BuiltinType == BuiltinType.Char);
            Assert.IsTrue(func7.Parameters.Count == 0);

            // func8
            Assert.IsTrue(this.ExpectTypeSymbol(zenitc, "func8", BuiltinType.Function));
            var func8 = zenitc.SymbolTable.CurrentScope.Get<IType>("func8") as Function;
            Assert.IsTrue(func8.Return.TypeSymbol.BuiltinType == BuiltinType.Void);
            Assert.IsTrue(func8.Parameters.Count == 0);
        }

        [TestMethod]
        public void TupleType()
        {
            var source = @"
                var t = (1,'2', ""3"");
                var t2 = (first: 1, second: '2', third: ""3"");
                var t3 = (1, second: '2', ""3"");
            ";

            var zenitc = new TestCompiler();
            zenitc.ResolveSymbols(source);

            // t
            Assert.IsTrue(this.ExpectVariableSymbol(zenitc, "t", BuiltinType.Tuple));
            Assert.IsTrue(this.ExpectVariableSymbol(zenitc, new[] { "t", "$0" }, BuiltinType.Int));
            Assert.IsTrue(this.ExpectVariableSymbol(zenitc, new[] { "t", "$1" }, BuiltinType.Char));
            Assert.IsTrue(this.ExpectVariableSymbol(zenitc, new[] { "t", "$2" }, BuiltinType.String));

            // t2
            Assert.IsTrue(this.ExpectVariableSymbol(zenitc, new[] { "t2", "first" }, BuiltinType.Int));
            Assert.IsTrue(this.ExpectVariableSymbol(zenitc, new[] { "t2", "second" }, BuiltinType.Char));
            Assert.IsTrue(this.ExpectVariableSymbol(zenitc, new[] { "t2", "third" }, BuiltinType.String));
            Assert.IsTrue(this.ExpectVariableSymbolToNotExist(zenitc, new[] { "t2", "$0" }));
            Assert.IsTrue(this.ExpectVariableSymbolToNotExist(zenitc, new[] { "t2", "$1" }));
            Assert.IsTrue(this.ExpectVariableSymbolToNotExist(zenitc, new[] { "t2", "$2" }));

            // t3
            Assert.IsTrue(this.ExpectVariableSymbol(zenitc, new[] { "t3", "$0" }, BuiltinType.Int));
            Assert.IsTrue(this.ExpectVariableSymbol(zenitc, new[] { "t3", "second" }, BuiltinType.Char));
            Assert.IsTrue(this.ExpectVariableSymbol(zenitc, new[] { "t3", "$2" }, BuiltinType.String));
            Assert.IsTrue(this.ExpectVariableSymbolToNotExist(zenitc, new[] { "t3", "$1" }));
        }

        [TestMethod]
        public void ObjectType()
        {
            var source = @"
                var obj = { 
                    a: { 
                        b: { 
                            c: ""test"",
                            d: (a,b) => a+b
                        } 
                    },
                    x: 2,
                    y: (1,'2', func: () => 1),
                    z: forward_referenced_function
                };

                fn forward_referenced_function (int x) { return x + 1; }
            ";

            var zenitc = new TestCompiler();
            zenitc.ResolveSymbols(source);

            Assert.IsTrue(this.ExpectVariableSymbol(zenitc, "obj", BuiltinType.Object));

            Assert.IsTrue(this.ExpectVariableSymbol(zenitc, new[] { "obj", "a" }, BuiltinType.Object));
            Assert.IsTrue(this.ExpectVariableSymbol(zenitc, new[] { "obj", "a", "b" }, BuiltinType.Object));
            Assert.IsTrue(this.ExpectVariableSymbol(zenitc, new[] { "obj", "a", "b", "c" }, BuiltinType.String));
            Assert.IsTrue(this.ExpectVariableSymbol(zenitc, new[] { "obj", "a", "b", "d" }, BuiltinType.Function));

            Assert.IsTrue(this.ExpectVariableSymbol(zenitc, new[] { "obj", "x" }, BuiltinType.Int));

            Assert.IsTrue(this.ExpectVariableSymbol(zenitc, new[] { "obj", "y" }, BuiltinType.Tuple));
            Assert.IsTrue(this.ExpectVariableSymbol(zenitc, new[] { "obj", "y", "$0" }, BuiltinType.Int));
            Assert.IsTrue(this.ExpectVariableSymbol(zenitc, new[] { "obj", "y", "$1" }, BuiltinType.Char));
            Assert.IsTrue(this.ExpectVariableSymbol(zenitc, new[] { "obj", "y", "func" }, BuiltinType.Function));
            Assert.IsTrue(this.ExpectVariableSymbolToNotExist(zenitc, new[] { "obj", "y", "$2" }));

            Assert.IsTrue(this.ExpectVariableSymbol(zenitc, new[] { "obj", "z" }, BuiltinType.Function));
            var frf = (zenitc.SymbolTable.CurrentScope.Get<IVariable>("obj").TypeSymbol as Object).Get<IVariable>("z").TypeSymbol as Function;
            Assert.IsTrue(frf.Parameters.Count == 1);
            Assert.IsTrue(frf.Parameters[0].TypeSymbol.BuiltinType == BuiltinType.Int);
            Assert.IsTrue(frf.Return.TypeSymbol.BuiltinType == BuiltinType.Int);            
        }

        private bool ExpectTypeSymbol(TestCompiler compiler, string symbol, BuiltinType builtinType)
        {
            return compiler.SymbolTable.CurrentScope.Get<IType>(symbol)?.BuiltinType == builtinType;
        }

        private bool ExpectVariableSymbol(TestCompiler compiler, string symbol, BuiltinType builtinType)
        {
            return compiler.SymbolTable.GetVariableSymbol(symbol)?.TypeSymbol?.BuiltinType == builtinType;
        }        

        private bool ExpectVariableSymbol(TestCompiler compiler, string[] symbols, BuiltinType builtinType)
        {
            BuiltinType? type = null;
            int i = 0;
            IContainer scope = compiler.SymbolTable.CurrentScope;
            ISymbol tmp = null;

            while (i < symbols.Length && ((tmp = scope.TryGet<IVariable>(symbols[i])) != null || (tmp = scope.TryGet<IType>(symbols[i])) != null))
            {
                i++;

                if (tmp is IVariable tmpv && tmpv.TypeSymbol is IContainer)
                {
                    scope = tmpv.TypeSymbol as IContainer;
                    type = scope.GetTypeSymbol()?.BuiltinType;
                    continue;
                }
                else if (tmp is IContainer)
                {
                    scope = tmp as IContainer;
                    type = scope.GetTypeSymbol()?.BuiltinType;
                    continue;
                }

                type = tmp.GetTypeSymbol()?.BuiltinType;
                break;
            }


            return type == builtinType;
        }

        private bool ExpectVariableSymbolToNotExist(TestCompiler compiler, string[] symbols)
        {
            ISymbol symbol = null;
            int i = 0;
            IContainer scope = compiler.SymbolTable.CurrentScope;
            ISymbol tmp = null;

            while (i < symbols.Length && ((tmp = scope.TryGet<IVariable>(symbols[i])) != null || (tmp = scope.TryGet<IType>(symbols[i])) != null))
            {
                i++;

                if (tmp is IVariable tmpv && tmpv.TypeSymbol is IContainer)
                {
                    scope = tmpv.TypeSymbol as IContainer;
                    symbol = scope.GetTypeSymbol();
                    continue;
                }
                else if (tmp is IContainer)
                {
                    scope = tmp as IContainer;
                    symbol = scope.GetTypeSymbol();
                    continue;
                }

                symbol = tmp.GetTypeSymbol();
                break;
            }

            // We must traverse all the path and the symbol must be null
            return i < symbols.Length || symbol == null;
        }
    }
}
