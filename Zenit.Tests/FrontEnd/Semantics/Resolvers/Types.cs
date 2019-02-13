using Microsoft.VisualStudio.TestTools.UnitTesting;
using Zenit.FrontEnd;
using Zenit.Semantics;
using Zenit.Semantics.Symbols.Types.Specials;
using Zenit.Semantics.Types;
using Zenit.Syntax;

namespace Zenit.Tests
{
    [TestClass]
    public class Types
    {
        [TestMethod]
        public void Literals()
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
                var t = (1,'2', ""3"");
                var func = () => 1;
                var obj = { a: 1 };
            ";

            var zenitc = new TestCompiler();
            zenitc.Compile(source);

            Assert.IsTrue(this.ExpectSymbolType(zenitc, "c", BuiltinType.Char));
            Assert.IsTrue(this.ExpectSymbolType(zenitc, "b1", BuiltinType.Bool));
            Assert.IsTrue(this.ExpectSymbolType(zenitc, "b2", BuiltinType.Bool));
            Assert.IsTrue(this.ExpectSymbolType(zenitc, "i", BuiltinType.Int));
            Assert.IsTrue(this.ExpectSymbolType(zenitc, "f", BuiltinType.Float));
            Assert.IsTrue(this.ExpectSymbolType(zenitc, "d", BuiltinType.Double));
            Assert.IsTrue(this.ExpectSymbolType(zenitc, "d2", BuiltinType.Double));
            Assert.IsTrue(this.ExpectSymbolType(zenitc, "m", BuiltinType.Decimal));
            Assert.IsTrue(this.ExpectSymbolType(zenitc, "s", BuiltinType.String));
            Assert.IsTrue(this.ExpectSymbolType(zenitc, "t", BuiltinType.Tuple));
            Assert.IsTrue(this.ExpectSymbolType(zenitc, "func", BuiltinType.Function));
            Assert.IsTrue(this.ExpectSymbolType(zenitc, "obj", BuiltinType.Object));
        }

        [TestMethod]
        public void Numbers()
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
            zenitc.Compile(source);

            Assert.IsTrue(this.ExpectSymbolType(zenitc, "ii", BuiltinType.Int));
            Assert.IsTrue(this.ExpectSymbolType(zenitc, "ff", BuiltinType.Float));
            Assert.IsTrue(this.ExpectSymbolType(zenitc, "dd", BuiltinType.Double));
            Assert.IsTrue(this.ExpectSymbolType(zenitc, "dd2", BuiltinType.Double));
            Assert.IsTrue(this.ExpectSymbolType(zenitc, "mm", BuiltinType.Decimal));

            Assert.IsTrue(this.ExpectSymbolType(zenitc, "i_f", BuiltinType.Float));
            Assert.IsTrue(this.ExpectSymbolType(zenitc, "i_d", BuiltinType.Double));
            Assert.IsTrue(this.ExpectSymbolType(zenitc, "i_m", BuiltinType.Number));
            Assert.IsTrue(this.ExpectSymbolType(zenitc, "f_d", BuiltinType.Double));
            Assert.IsTrue(this.ExpectSymbolType(zenitc, "f_m", BuiltinType.Number));
            Assert.IsTrue(this.ExpectSymbolType(zenitc, "d_m", BuiltinType.Number));
        }

        private bool ExpectSymbolType(TestCompiler compiler, string symbol, BuiltinType builtinType)
        {
            return compiler.SymbolTable.GetBoundSymbol(symbol)?.TypeSymbol?.BuiltinType == builtinType;
        }
    }
}
