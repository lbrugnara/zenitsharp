// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Fl.Parser.Ast;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Fl.Parser
{
    public class Parser : IParser
    {
        private List<Token> _Tokens;
        private int _Pointer;

        public AstNode Parse(List<Token> tokens)
        {
            _Pointer = 0;
            _Tokens = tokens;
            return Program();
        }

        private bool HasInput()
        {
            return _Pointer < _Tokens.Count;
        }

        private bool Match(params TokenType[] type)
        {
            return MatchFrom(0, type);
        }

        private bool MatchFrom(int offset, params TokenType[] types)
        {
            if (offset < 0) throw new ArgumentOutOfRangeException(nameof(offset), "Offset cannot be negative");

            var l = types.Length;
            if (l + _Pointer + offset > _Tokens.Count)
                return false;
            for (int i = 0; i < types.Length; i++)
            {
                if (types[i] == TokenType.Unknown)
                    continue;
                if (_Tokens[_Pointer + i + offset].Type != types[i])
                    return false;
            }
            return true;
        }

        private bool MatchAny(params TokenType[] types)
        {
            var t = Peek();
            return t != null && types.Contains(t.Type);
        }

        private bool MatchAnyFrom(int offset, params TokenType[] types)
        {
            if (offset < 0) throw new ArgumentOutOfRangeException(nameof(offset), "Offset cannot be negative");
            var t = PeekFrom(offset);
            return t != null && types.Contains(t.Type);
        }

        private int CountRepeatedMatchesFrom(int offset, params TokenType[] types)
        {
            if (offset < 0) throw new ArgumentOutOfRangeException(nameof(offset), "Offset cannot be negative");

            int q = 0;
            var l = types.Length;
            if (l + _Pointer + offset > _Tokens.Count)
                return 0;

            int i = 0;
            bool valid = true;
            while (valid && i + offset + _Pointer < _Tokens.Count)
            {
                for (int j = 0; j < types.Length; j++, i++)
                {
                    if (types[j] == TokenType.Unknown)
                    {
                        q++;
                        continue;
                    }
                    if (_Pointer + i + offset >= _Tokens.Count || _Tokens[_Pointer + i + offset].Type != types[j])
                    {
                        valid = false;
                        break;
                    }
                    q++;
                }
            }
            return q >= types.Length ? q : 0;
        }

        private Token Peek()
        {
            return _Pointer < _Tokens.Count ? _Tokens[_Pointer] : null;
        }

        private Token PeekFrom(int offset)
        {
            if (offset < 0) throw new ArgumentOutOfRangeException(nameof(offset), "Offset cannot be negative");
            return _Pointer + offset < _Tokens.Count ? _Tokens[_Pointer + offset] : null;
        }

        private Token Consume(TokenType type, string message = null)
        {
            if (!HasInput())
                return null;

            if (!Match(type))
                throw new ParsingException(message ?? $"Expects {type} but received {Peek().Type}");

            return _Tokens[_Pointer++];
        }

        private Token Consume()
        {
            if (!HasInput())
                return null;
            return _Tokens[_Pointer++];
        }

        private void Restore(Token t)
        {
            _Pointer--;
        }

        #region Grammar

        // Rule:
        // program -> declaration*
        private AstDeclarationNode Program()
        {            
            List<AstNode> statements = new List<AstNode>();
            while (HasInput())
            {
                if (Match(TokenType.Semicolon))
                {
                    Consume();
                    continue;
                }
                statements.Add(Declaration());
            }
            return new AstDeclarationNode(statements);
        }

        // Rule:
        // declaration	-> func_declaration
        //               | variable_declaration
        //	             | constant_declaration
        //	             | statement
        //
        private AstNode Declaration()
        {
            if (Match(TokenType.Variable))
            {
                return VarDeclaration();
            }
            else if (Match(TokenType.Constant))
            {
                return ConstDeclaration();
            }
            else if (Match(TokenType.Function))
            {
                return FuncDeclaration();
            }
            return Statement();
        }

        // Rule:
        // func_declaration -> "func" IDENTIFIER "(" func_params? ")" "{" declaration* "}"
        private AstFuncDeclNode FuncDeclaration()
        {
            Consume(TokenType.Function);
            Token name = Consume(TokenType.Identifier);
            Consume(TokenType.LeftParen);
            AstParametersNode parameters = FuncParameters();
            Consume(TokenType.RightParen);
            
            List<AstNode> decls = new List<AstNode>();
            Consume(TokenType.LeftBrace);
            while (!Match(TokenType.RightBrace))
            {
                decls.Add(Declaration());
            }
            Consume(TokenType.RightBrace);
            return new AstFuncDeclNode(name, parameters, decls);
        }

        // Rule:
        // func_params -> IDENTIFIER ( "," IDENTIFIER )*
        private AstParametersNode FuncParameters()
        {
            List<Token> parameters = new List<Token>();
            while (Match(TokenType.Identifier))
            {
                parameters.Add(Consume(TokenType.Identifier));
                if (Match(TokenType.Comma))
                    Consume();
            }
            return new AstParametersNode(parameters);
        }

        // Rule:
        //  statement	-> expression_statement
        //	            | if_statement
        //	            | while_statement
        //	            | for_statement
        //	            | break_statement
        //	            | continue_statement
        //	            | return_statement
        //	            | block
        private AstNode Statement()
        {
            if (Match(TokenType.If))
                return IfStatement();
            if (Match(TokenType.LeftBrace))
                return Block();
            if (Match(TokenType.While))
                return WhileStatement();
            if (Match(TokenType.For))
                return ForStatement();
            if (Match(TokenType.Break))
                return BreakStatement();
            if (Match(TokenType.Continue))
                return ContinueStatement();
            if (Match(TokenType.Return))
                return ReturnStatement();
            return ExpressionStatement();
        }

        // Rule:
        // return_statement -> "return" expression? ";"
        private AstReturnNode ReturnStatement()
        {
            Token kw = Consume(TokenType.Return);
            AstNode expr = null;
            if (!Match(TokenType.Semicolon))
                expr = Expression();
            Consume(TokenType.Semicolon);
            return new AstReturnNode(kw, expr);
        }

        // Rule:
        // continue_statement -> "continue" ";"
        private AstContinueNode ContinueStatement()
        {
            var cont = new AstContinueNode(Consume(TokenType.Continue));
            Consume(TokenType.Semicolon);
            return cont;
        }

        // Rule:
        // break_statement -> "break" INTEGER? ";"
        private AstBreakNode BreakStatement()
        {
            AstNode nbreaks = null;
            Token kw = Consume(TokenType.Break);
            if (Match(TokenType.Integer))
                nbreaks = new AstLiteralNode(Consume(TokenType.Integer));
            Consume(TokenType.Semicolon);
            return new AstBreakNode(kw, nbreaks);
        }

        // Rule:
        // parenthesized_expr -> "(" expression ")" ( statement | ";" )
        private (AstNode, AstNode) ParenthesizedExpression()
        {
            Consume(TokenType.LeftParen);
            AstNode expression = Expression();
            Consume(TokenType.RightParen);
            AstNode stmt = Match(TokenType.Semicolon) ? new AstNoOpNode(Consume(TokenType.Semicolon)) : Statement();
            return (expression, stmt);
        }

        // Rule:
        // braced_expr -> expression block
        private (AstNode, AstNode) BracedExpression()
        {
            AstNode expression = Expression();
            AstNode block = Block();
            return (expression, block);
        }

        // Rule:
        // while_statement -> "while" ( parenthesized_expr | braced_expr )
        private AstNode WhileStatement()
        {
            Token kw = Consume(TokenType.While);

            AstNode condition = null;
            AstNode body = null;

            if (Match(TokenType.LeftParen))
                (condition, body) = ParenthesizedExpression();
            else
                (condition, body) = BracedExpression();
            return new AstWhileNode(kw, condition, body);
        }

        #region for_statement

        // Rule:
        // for_statement -> "for" "(" for_initializer? ";" expression? ";" for_iterator? ")" statement
        // 			      | "for" for_initializer? ";" expression? ";" for_iterator? block
        private AstNode ForStatement()
        {
            if (Match(TokenType.For, TokenType.LeftParen))
            {
                return ParenthesizedForStatemet();
            }
            Token kw = Consume(TokenType.For);

            AstNode forInitializer = null;
            AstNode expression = null;
            AstNode forIterator = null;
            AstNode body = null;

            // Initializer
            if (Match(TokenType.Semicolon))
            {
                forInitializer = new AstNoOpNode(Consume());
            }
            else
            {
                forInitializer = ForInitializer();
                Consume(TokenType.Semicolon);
            }
            // Expression
            if (Match(TokenType.Semicolon))
            {
                expression = new AstNoOpNode(Consume());
            }
            else
            {
                expression = Expression();
                Consume(TokenType.Semicolon);
            }

            // Iterator
            if (Match(TokenType.LeftBrace))
            {
                forIterator = new AstNoOpNode(Peek()); // Get a reference of the line/col
            }
            else
            {
                forIterator = ForIterator();
            }

            // Body
            body = Block();
            return new AstForNode(kw, forInitializer, expression, forIterator, body);
        }

        // Rule: (continuation)
        // for_statement -> "for" "(" for_initializer? ";" expression? ";" for_iterator? ")" statement
        private AstNode ParenthesizedForStatemet()
        {
            Token kw = Consume(TokenType.For);
            Consume(TokenType.LeftParen);

            AstNode forInitializer = null;
            AstNode expression = null;
            AstNode forIterator = null;
            AstNode body = null;

            // Initializer
            if (Match(TokenType.Semicolon))
            {
                forInitializer = new AstNoOpNode(Consume());
            }
            else
            {
                forInitializer = ForInitializer();
                Consume(TokenType.Semicolon);
            }

            // Expression
            if (Match(TokenType.Semicolon))
            {
                expression = new AstNoOpNode(Consume());
            }
            else
            {
                expression = Expression();
                Consume(TokenType.Semicolon);
            }

            // Iterator
            if (Match(TokenType.RightParen))
            {
                forIterator = new AstNoOpNode(Consume());
            }
            else
            {
                forIterator = ForIterator();
                Consume(TokenType.RightParen);
            }

            // Body
            body = Match(TokenType.Semicolon) ? new AstNoOpNode(Consume(TokenType.Semicolon)) : Statement();
            return new AstForNode(kw, forInitializer, expression, forIterator, body);
        }

        // Rule:
        // for_initializer -> for_declaration
        // 				    | expression ( "," expression )*
        private AstNode ForInitializer()
        {
            if (Match(TokenType.Variable))
            {
                return ForDeclaration();
            }
            List<AstNode> exprs = new List<AstNode>();
            exprs.Add(Expression());
            while (Match(TokenType.Comma))
            {
                Consume();
                exprs.Add(Expression());
            }
            // TODO: Check if DeclarationNode is correct
            return new AstDeclarationNode(exprs);
        }

        // Rule:
        // for_declaration -> VAR local_var_declaration ( "," local_var_declaration )*
        private AstNode ForDeclaration()
        {
            Token var = Consume(TokenType.Variable);
            List<AstNode> decls = new List<AstNode>();
            decls.Add(LocalVarDeclaration());
            while (Match(TokenType.Comma))
            {
                Consume();
                decls.Add(LocalVarDeclaration());
            }
            return new AstDeclarationNode(decls);
        }

        // Rule:
        // local_var_declaration -> IDENTIFIER ( "=" expression )
        private AstNode LocalVarDeclaration()
        {
            Token id = Consume(TokenType.Identifier);
            AstNode expr = null;
            if (Match(TokenType.Assignment))
            {
                Consume(TokenType.Assignment);
                expr = Expression();
            }
            return new AstVariableNode(id, expr);
        }

        // Rule:
        // for_iterator -> expression ( "," expression )*
        private AstNode ForIterator()
        {
            List<AstNode> exprs = new List<AstNode>();
            exprs.Add(Expression());
            while (Match(TokenType.Comma))
            {
                Consume();
                exprs.Add(Expression());
            }
            // TODO: Check if DeclarationNode is correct
            return new AstDeclarationNode(exprs);
        }
        #endregion

        // Rule:
        // if_statement -> "if" (parenthesized_expr | braced_expr ) ( "else" (statement | ";" ) )?
        // parenthesized_expr -> "(" expression ")" ( statement | ";" )
        // braced_expr -> expression block
        private AstNode IfStatement()
        {
            Token kw = Consume(TokenType.If);
            AstNode condition = null;
            AstNode thenbranch = null;
            AstNode elsebranch = null;

            if (Match(TokenType.LeftParen))
                (condition, thenbranch) = ParenthesizedExpression();
            else
                (condition, thenbranch) = BracedExpression();

            // Parse the else branch if present
            if (Match(TokenType.Else))
            {
                Consume(TokenType.Else);
                // Hack: Consume the Semicolon before the new AstNoOpNode
                elsebranch = Match(TokenType.Semicolon) ? new AstNoOpNode(Consume(TokenType.Semicolon)) : Statement();
            }
            return new AstIfNode(kw, condition, thenbranch, elsebranch);
        }

        // Rule:
        // expression_statement	-> expression ";"
        private AstNode ExpressionStatement()
        {
            AstNode expr = Expression();
            Consume(TokenType.Semicolon);
            return expr;
        }

        // Rule:
        // block -> "{" declaration* "}"
        private AstBlockNode Block()
        {
            List<AstNode> statements = new List<AstNode>();
            Consume(TokenType.LeftBrace);
            while (!Match(TokenType.RightBrace))
            {
                statements.Add(Declaration());
            }
            Consume(TokenType.RightBrace);
            return new AstBlockNode(statements);
        }

        // Rule:
        // variable_declaration -> VAR IDENTIFIER ( "=" expression )? ";"
        private AstNode VarDeclaration()
        {
            Consume(TokenType.Variable);
            Token identifier = Consume(TokenType.Identifier);
            AstNode expression = null;
            if (Match(TokenType.Assignment))
            {
                Consume(); // =
                expression = Expression();
            }
            Consume(TokenType.Semicolon);
            return new AstVariableNode(identifier, expression);
        }

        // Rule:
        // constant_declaration -> CONST IDENTIFIER "=" expression ";"
        private AstNode ConstDeclaration()
        {
            Consume(TokenType.Constant);
            Token identifier = Consume(TokenType.Identifier);
            Consume(TokenType.Assignment, "A constant value needs to be defined when declared.");
            AstNode expression = Expression();
            Consume(TokenType.Semicolon);
            return new AstConstantNode(identifier, expression);
        }

        // Rule:
        // expression -> expression_assignment
        private AstNode Expression()
        {
            return ExpressionAssignment();
        }

        // Rule:
        // expression_assignment	-> IDENTIFIER ( ( "=" | "+=" | "-=" | "/=" | "*=" )  expression_assignment )?
        //			                | or_expression
        private AstNode ExpressionAssignment()
        {
            if (Match(TokenType.Identifier))
            {
                Token identifier = Consume(TokenType.Identifier);
                if (MatchAny(TokenType.Assignment, TokenType.IncrementAndAssign, TokenType.DecrementAndAssign, TokenType.DivideAndAssign, TokenType.MultAndAssign))
                {
                    Token assignmentop = Consume();
                    AstNode expression = ExpressionAssignment();
                    return new AstAssignmentNode(identifier, assignmentop, expression);
                }
                Restore(identifier);
            }
            return OrExpression();
        }

        // Rule:
        // or_expression -> and_expression ( "||" and_expression )*
        private AstNode OrExpression()
        {
            AstNode orexpr = AndExpression();
            while (Match(TokenType.Or))
            {
                Token or = Consume();
                AstNode right = AndExpression();
                orexpr = new AstBinaryNode(or, orexpr, right);
            }
            return orexpr;
        }

        // Rule:
        // and_expression -> equality_expression ( "&&" equality_expression )*
        private AstNode AndExpression()
        {
            AstNode andexpr = EqualityExpression();
            while (Match(TokenType.And))
            {
                Token and = Consume();
                AstNode right = EqualityExpression();
                andexpr = new AstBinaryNode(and, andexpr, right);
            }
            return andexpr;
        }

        // Rule:
        // equality_expression -> comparison_expression ( ( "!=" | "==" ) comparison_expression )*
        private AstNode EqualityExpression()
        {
            AstNode compexpr = ComparisonExpression();
            while (Match(TokenType.Equal) || Match(TokenType.NotEqual))
            {
                Token equality = Consume();
                AstNode right = ComparisonExpression();
                compexpr = new AstBinaryNode(equality, compexpr, right);
            }
            return compexpr;
        }

        // Rule:
        // comparison_expression -> addition_expression(( ">" | ">=" | "<" | "<=" ) addition_expression )*
        private AstNode ComparisonExpression()
        {
            AstNode additionexpr = AdditionExpression();
            while (Match(TokenType.GreatThan)
                || Match(TokenType.GreatThanEqual)
                || Match(TokenType.LessThan)
                || Match(TokenType.LessThanEqual))
            {
                Token comp = Consume();
                AstNode right = AdditionExpression();
                additionexpr = new AstBinaryNode(comp, additionexpr, right);
            }
            return additionexpr;
        }

        // Rule:
        // addition_expression -> multiplication_expression(( "-" | "+" ) multiplication_expression )*
        private AstNode AdditionExpression()
        {
            AstNode multexpr = MultiplicationExpression();
            while (Match(TokenType.Minus) || Match(TokenType.Addition))
            {
                Token addition = Consume();
                AstNode right = MultiplicationExpression();
                multexpr = new AstBinaryNode(addition, multexpr, right);
            }
            return multexpr;
        }

        // Rule:
        // multiplication_expression -> unary_expression(( "/" | "*" ) unary_expression )*
        private AstNode MultiplicationExpression()
        {
            AstNode unaryexpr = UnaryExpression();
            while (Match(TokenType.Multiplication) || Match(TokenType.Division))
            {
                Token mult = Consume();
                AstNode right = UnaryExpression();
                unaryexpr = new AstBinaryNode(mult, unaryexpr, right);
            }
            return unaryexpr;
        }

        // Rule:
        // unary_expression	-> ( "!" | "-" ) unary_expression
        // 					| ( "++" | "--" ) primary_expression
        // 					| primary_expression ( "++" | "--" )?
        private AstNode UnaryExpression()
        {
            if (Match(TokenType.Not) || Match(TokenType.Minus))
            {
                Token op = Consume();
                return new AstUnaryNode(op, UnaryExpression());
            }
            else if (Match(TokenType.Increment) || Match(TokenType.Decrement))
            {
                Token op = Consume();
                return new AstUnaryPrefixNode(op, PrimaryExpression());
            }
            var expr = PrimaryExpression();
            if (Match(TokenType.Increment) || Match(TokenType.Decrement))
                return new AstUnaryPostfixNode(Consume(), expr);
            return new AstUnaryNode(null, expr);
        }

        // Rule:
        // primary_expression -> primary ( "." IDENTIFIER | "(" arguments? ")" )*
        private AstNode PrimaryExpression()
        {
            AstNode primary = Primary();
            while (true)
            {
                if (Match(TokenType.LeftParen))
                {
                    Consume();
                    AstArgumentsNode arguments = Arguments();
                    Consume(TokenType.RightParen);
                    primary = new AstCallableNode(primary, arguments);
                }
                else if (Match(TokenType.Dot))
                {
                    Consume();
                    primary = new AstAccessorNode(Consume(TokenType.Identifier), primary);
                }
                else break;

            }
            return primary;
        }

        private AstArgumentsNode Arguments()
        {
            List<AstNode> args = new List<AstNode>();
            if (!Match(TokenType.RightParen))
            {
                args.Add(Expression());
                while (Match(TokenType.Comma))
                {
                    Consume();
                    args.Add(Expression());
                }
            }
            return new AstArgumentsNode(args);
        }

        // Rule:
        // primary	-> "true" | "false" | "null" | INTEGER | DOUBLE | DECIMAL | STRING | IDENTIFIER	| "(" expression ")"
        private AstNode Primary()
        {
            if (Match(TokenType.LeftParen))
            {
                Consume();
                AstNode expression = Expression();
                Consume(TokenType.RightParen);
                return expression;
            }
            if (!Match(TokenType.Boolean)
                && !Match(TokenType.Null)
                && !Match(TokenType.Integer)
                && !Match(TokenType.Double)
                && !Match(TokenType.Decimal)
                && !Match(TokenType.String)
                && !Match(TokenType.Identifier))
                throw new ParsingException($"Expects primary but received {Peek().Type}");

            return Match(TokenType.Identifier) ? (AstNode)new AstAccessorNode(Consume(), null) : new AstLiteralNode(Consume());
        }
        #endregion
    }
}
