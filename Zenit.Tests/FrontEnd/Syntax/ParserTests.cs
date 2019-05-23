using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Zenit.Ast;
using Zenit.FrontEnd;
using Zenit.Syntax;

namespace Zenit.Tests.FrontEnd.Syntax
{
    [TestClass]
    public class ParserTests
    {
        [TestMethod]
        public void Program()
        {
            // program	-> declaration*
            var ast = new TestParser().Parse("").ast;

            Assert.IsTrue(ast is DeclarationNode);

            var dn = ast as DeclarationNode;

            Assert.IsTrue(dn.Declarations.Count == 0);

            // this.Statement();
        }

        public void Statement()
        {
            this.ExpressionStatement();
        }

        public void ExpressionStatement()
        {
            this.Expression();
        }

        public void Expression()
        {
            this.ExpressionAssignment();
        }

        public void ExpressionAssignment()
        {
            this.ConditionalExpression();
        }

        public void ConditionalExpression()
        {
            this.NullCoalescingExpression();
        }

        public void NullCoalescingExpression()
        {
            this.OrExpression();
        }

        public void OrExpression()
        {
            this.AndExpression();
        }

        public void AndExpression()
        {
            this.EqualityExpression();
        }

        public void EqualityExpression()
        {
            this.ComparisonExpression();
        }

        public void ComparisonExpression()
        {
            this.AdditionExpression();
        }

        public void AdditionExpression()
        {
            this.MultiplicationExpression();
        }

        public void MultiplicationExpression()
        {
            this.UnaryExpression();
        }

        [TestMethod]
        // unary_expression	-> ( '!' | '-' ) unary_expression                                   (a)
        // 					| ( '++' | '--' ) symbol_expression symbol_operator*     (b)
        // 					| symbol_expression symbol_operator* ( '++' | '--' )?    (c)
        public void UnaryExpression()
        {
            var parser = new TestParser(true);

            /*var unaryExpressions = parser.LoadFrom("./FrontEnd/Syntax/Grammar/unary_expressions.z");

            // If the unary_expressions.z file exists and has entries, use it.
            if (unaryExpressions.Any())
            {
                unaryExpressions.ForEach(ue => parser.Parse(ue));
                return;
            }*/

            // If the file is empty or does not exist at all, generate a set of unary_expressions using the
            // symbol_expression and symbol_operator files
            var symbolExpressions = parser.LoadFrom("./FrontEnd/Syntax/Grammar/symbol_expression.z");
            var operators = parser.LoadFrom("./FrontEnd/Syntax/Grammar/symbol_operator.z");

            // (a)
            symbolExpressions.ForEach(symbol => {
                parser.Parse($"!{symbol}");
                parser.Parse($"-{symbol}");

                parser.Parse($"!({symbol})");
                parser.Parse($"-({symbol})");

                parser.Parse($"!{symbol}++");
                parser.Parse($"!{symbol}--");
                parser.Parse($"!++{symbol}");
                parser.Parse($"!--{symbol}");
                parser.Parse($"!({symbol})++");
                parser.Parse($"!({symbol})--");
                parser.Parse($"!++({symbol})");
                parser.Parse($"!--({symbol})");

                parser.Parse($"-{symbol}++");
                parser.Parse($"-{symbol}--");
                parser.Parse($"-++{symbol}");
                parser.Parse($"- --{symbol}");
                parser.Parse($"-++({symbol})");
                parser.Parse($"- --({symbol})");

                operators.ForEach(@operator => {
                    parser.Parse($"!{symbol}{@operator}");
                    parser.Parse($"!{symbol}{@operator}++");
                    parser.Parse($"!({symbol}){@operator}");
                    parser.Parse($"!({symbol}{@operator})");
                    parser.Parse($"!({symbol}){@operator}++");
                    parser.Parse($"!({symbol}{@operator})++");
                    parser.Parse($"-{symbol}{@operator}");
                    parser.Parse($"-{symbol}{@operator}++");
                    parser.Parse($"-({symbol}){@operator}");
                    parser.Parse($"-({symbol}{@operator})");
                    parser.Parse($"-({symbol}){@operator}++");
                    parser.Parse($"-({symbol}{@operator})++");

                    parser.Parse($"!++{symbol}{@operator}");                                        
                    parser.Parse($"!++({symbol}){@operator}");
                    parser.Parse($"!++({symbol}{@operator})");
                    parser.Parse($"-++{symbol}{@operator}");                    
                    parser.Parse($"-++({symbol}){@operator}");
                    parser.Parse($"-++({symbol}{@operator})");

                    operators.ForEach(operator2 =>
                    {
                        parser.Parse($"!{symbol}{@operator}{operator2}");
                        parser.Parse($"!({symbol}){@operator}{operator2}");
                        parser.Parse($"!({symbol}{@operator}){operator2}");
                        parser.Parse($"!({symbol}{@operator}{operator2})");
                        parser.Parse($"!{symbol}{@operator}{operator2}++");
                        parser.Parse($"!({symbol}){@operator}{operator2}++");
                        parser.Parse($"!({symbol}{@operator}){operator2}++");
                        parser.Parse($"!({symbol}{@operator}{operator2})++");
                        parser.Parse($"!++{symbol}{@operator}{operator2}");
                        parser.Parse($"!++({symbol}){@operator}{operator2}");
                        parser.Parse($"!++({symbol}{@operator}){operator2}");
                        parser.Parse($"!++({symbol}{@operator}{operator2})");

                        parser.Parse($"-{symbol}{@operator}{operator2}");
                        parser.Parse($"-({symbol}){@operator}{operator2}");
                        parser.Parse($"-({symbol}{@operator}){operator2}");
                        parser.Parse($"-({symbol}{@operator}{operator2})");
                        parser.Parse($"-{symbol}{@operator}{operator2}++");
                        parser.Parse($"-({symbol}){@operator}{operator2}++");
                        parser.Parse($"-({symbol}{@operator}){operator2}++");
                        parser.Parse($"-({symbol}{@operator}{operator2})++");
                        parser.Parse($"-++{symbol}{@operator}{operator2}");
                        parser.Parse($"-++({symbol}){@operator}{operator2}");
                        parser.Parse($"-++({symbol}{@operator}){operator2}");
                        parser.Parse($"-++({symbol}{@operator}{operator2})");
                    });
                });

                operators.ForEach(@operator => {
                    parser.Parse($"!{symbol}{@operator}");
                    parser.Parse($"!({symbol}){@operator}");
                    parser.Parse($"!({symbol}{@operator})");
                    parser.Parse($"!{symbol}{@operator}--");
                    parser.Parse($"!({symbol}){@operator}--");
                    parser.Parse($"!({symbol}{@operator})--");
                    parser.Parse($"!--{symbol}{@operator}");
                    parser.Parse($"!--({symbol}){@operator}");
                    parser.Parse($"!--({symbol}{@operator})");

                    parser.Parse($"-{symbol}{@operator}");
                    parser.Parse($"-({symbol}){@operator}");
                    parser.Parse($"-({symbol}{@operator})");
                    parser.Parse($"-{symbol}{@operator}--");
                    parser.Parse($"-({symbol}){@operator}--");
                    parser.Parse($"-({symbol}{@operator})--");
                    parser.Parse($"- --{symbol}{@operator}");
                    parser.Parse($"- --({symbol}){@operator}");
                    parser.Parse($"- --({symbol}{@operator})");

                    operators.ForEach(operator2 =>
                    {
                        parser.Parse($"!{symbol}{@operator}{operator2}");
                        parser.Parse($"!({symbol}){@operator}{operator2}");
                        parser.Parse($"!({symbol}{@operator}){operator2}");
                        parser.Parse($"!({symbol}{@operator}{operator2})");
                        parser.Parse($"!{symbol}{@operator}{operator2}--");
                        parser.Parse($"!({symbol}){@operator}{operator2}--");
                        parser.Parse($"!({symbol}{@operator}){operator2}--");
                        parser.Parse($"!({symbol}{@operator}{operator2})--");
                        parser.Parse($"!--{symbol}{@operator}{operator2}");
                        parser.Parse($"!--({symbol}){@operator}{operator2}");
                        parser.Parse($"!--({symbol}{@operator}){operator2}");
                        parser.Parse($"!--({symbol}{@operator}{operator2})");

                        parser.Parse($"-{symbol}{@operator}{operator2}");
                        parser.Parse($"-({symbol}){@operator}{operator2}");
                        parser.Parse($"-({symbol}{@operator}){operator2}");
                        parser.Parse($"-({symbol}{@operator}{operator2})");
                        parser.Parse($"-{symbol}{@operator}{operator2}--");
                        parser.Parse($"-({symbol}){@operator}{operator2}--");
                        parser.Parse($"-({symbol}{@operator}){operator2}--");
                        parser.Parse($"-({symbol}{@operator}{operator2})--");
                        parser.Parse($"- --{symbol}{@operator}{operator2}");
                        parser.Parse($"- --({symbol}){@operator}{operator2}");
                        parser.Parse($"- --({symbol}{@operator}){operator2}");
                        parser.Parse($"- --({symbol}{@operator}{operator2})");
                    });
                });
            });

            // (b);
            symbolExpressions.ForEach(symbol => {
                // '++' symbol_expression
                parser.Parse($"++{symbol}");
                parser.Parse($"++({symbol})");

                // '--' symbol_expression
                parser.Parse($"--{symbol}");
                parser.Parse($"--({symbol})");

                // '++' symbol_expression symbol_operator
                operators.ForEach(@operator => {
                    parser.Parse($"++{symbol}{@operator}");
                    parser.Parse($"++({symbol}){@operator}");
                    parser.Parse($"++({symbol}{@operator})");

                    // '++' symbol_expression symbol_operator symbol_operator
                    operators.ForEach(operator2 =>
                    {
                        parser.Parse($"++{symbol}{@operator}{operator2}");
                        parser.Parse($"++({symbol}){@operator}{operator2}");
                        parser.Parse($"++({symbol}{@operator}){operator2}");
                        parser.Parse($"++({symbol}{@operator}{operator2})");
                    });
                });

                // '--' symbol_expression symbol_operator
                operators.ForEach(@operator => {
                    parser.Parse($"--{symbol}{@operator}");
                    parser.Parse($"--({symbol}){@operator}");
                    parser.Parse($"--({symbol}{@operator})");

                    // '++' symbol_expression symbol_operator symbol_operator
                    operators.ForEach(operator2 =>
                    {
                        parser.Parse($"--{symbol}{@operator}{operator2}");
                        parser.Parse($"--({symbol}){@operator}{operator2}");
                        parser.Parse($"--({symbol}{@operator}){operator2}");
                        parser.Parse($"--({symbol}{@operator}{operator2})");
                    });
                });
            });

            // (c);
            symbolExpressions.ForEach(symbol => {
                parser.Parse($"{symbol}++");
                parser.Parse($"({symbol})++");

                parser.Parse($"{symbol}--");
                parser.Parse($"({symbol})--");

                operators.ForEach(accessor => {
                    parser.Parse($"{symbol}{accessor}++");
                    parser.Parse($"({symbol}){accessor}++");
                    parser.Parse($"({symbol}{accessor})++");

                    operators.ForEach(accessor2 =>
                    {
                        parser.Parse($"{symbol}{accessor}{accessor2}++");
                        parser.Parse($"({symbol}){accessor}{accessor2}++");
                        parser.Parse($"({symbol}{accessor}){accessor2}++");
                        parser.Parse($"({symbol}{accessor}{accessor2})++");
                    });
                });

                operators.ForEach(accessor => {
                    parser.Parse($"{symbol}{accessor}--");
                    parser.Parse($"({symbol}){accessor}--");
                    parser.Parse($"({symbol}{accessor})--");

                    operators.ForEach(accessor2 =>
                    {
                        parser.Parse($"{symbol}{accessor}{accessor2}--");
                        parser.Parse($"({symbol}){accessor}{accessor2}--");
                        parser.Parse($"({symbol}{accessor}){accessor2}--");
                        parser.Parse($"({symbol}{accessor}{accessor2})--");
                    });
                });
            });

            // Save the file
            parser.SaveTo("./FrontEnd/Syntax/Grammar/unary_expressions.z");
        }

        [TestMethod]
        public void SymbolExpression()
        {
            var parser = new TestParser();
            parser.LoadFrom("./FrontEnd/Syntax/Grammar/symbol_expression.z").ForEach(sexpr => {
                parser.Parse(sexpr);
                parser.Parse($"({sexpr})");
            });
        }

        [TestMethod]
        public void ObjectExpression()
        {
            var parser = new TestParser();
            parser.LoadFrom("./FrontEnd/Syntax/Grammar/object_expression.z").ForEach(l => parser.Parse(l));
        }

        [TestMethod]
        public void ArrayInitializer()
        {
            var parser = new TestParser();
            parser.LoadFrom("./FrontEnd/Syntax/Grammar/array_expression.z").ForEach(l => parser.Parse(l));
        }

        [TestMethod]
        public void TupleExpression()
        {
            var parser = new TestParser();
            parser.LoadFrom("./FrontEnd/Syntax/Grammar/tuple_expression.z").ForEach(l => parser.Parse(l));
        }

        [TestMethod]
        public void Primary()
        {
            var parser = new TestParser();
            parser.LoadFrom("./FrontEnd/Syntax/Grammar/primary.z").ForEach(l => parser.Parse(l));
        }
    }
}
