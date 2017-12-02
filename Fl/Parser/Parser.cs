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
        #region Private fields
        
        /// <summary>
        /// Stream of tokens
        /// </summary>
        private List<Token> _Tokens;

        /// <summary>
        /// Pointer to keep track of the current position
        /// </summary>
        private int _Pointer;

        #endregion

        #region Constructor

        public AstNode Parse(List<Token> tokens)
        {
            _Pointer = 0;
            _Tokens = tokens;
            return Program();
        }

        #endregion

        #region Parsing helpers

        private bool HasInput()
        {
            return _Pointer < _Tokens.Count;
        }

        /// <summary>
        /// Return true if the following tokens starting from the current position match in type with the provided array of types
        /// </summary>
        /// <param name="types">Target stream of types to match</param>
        /// <returns></returns>
        private bool Match(params TokenType[] types)
        {
            return MatchFrom(0, types);
        }

        /// <summary>
        /// Return true if the following tokens starting from the current position and applying an offset, match in type with the provided array of types
        /// </summary>
        /// <param name="offset">Offset to reach the target token</param>
        /// <param name="types">Types to check against the target token</param>
        /// <returns></returns>
        private bool MatchFrom(int offset, params TokenType[] types)
        {
            //if (offset < 0) throw new ArgumentOutOfRangeException(nameof(offset), "Offset cannot be negative");

            var l = types.Length;
            if (l + _Pointer + offset > _Tokens.Count || _Pointer + offset < 0)
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

        /// <summary>
        /// Return true if the following token starting from the current position, has its type in the types array
        /// </summary>
        /// <param name="types">List of expected types to match with the next token</param>
        /// <returns></returns>
        private bool MatchAny(params TokenType[] types)
        {
            var t = Peek();
            return t != null && types.Contains(t.Type);
        }

        /// <summary>
        /// Return true if the following token starting from the current position and applying an offset, has its type in the types array
        /// </summary>
        /// <param name="types">List of expected types to match with the next token</param>
        /// <returns></returns>
        private bool MatchAnyFrom(int offset, params TokenType[] types)
        {
            //if (offset < 0) throw new ArgumentOutOfRangeException(nameof(offset), "Offset cannot be negative");
            var t = PeekFrom(offset);
            return t != null && types.Contains(t.Type);
        }

        /// <summary>
        /// Return the number of tokens starting from the current position and applying an offset, that repeatedly match the
        /// types in order
        /// </summary>
        /// <param name="types">List of expected types to match with the next token</param>
        /// <returns></returns>
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

        /// <summary>
        /// Return the next token if available
        /// </summary>
        /// <returns></returns>
        private Token Peek()
        {
            return _Pointer < _Tokens.Count ? _Tokens[_Pointer] : null;
        }

        /// <summary>
        /// Return the next token based on the current position with a positive offset
        /// </summary>
        /// <param name="offset"></param>
        /// <returns></returns>
        private Token PeekFrom(int offset)
        {
            //if (offset < 0) throw new ArgumentOutOfRangeException(nameof(offset), "Offset cannot be negative");
            int i = _Pointer + offset;
            return i >= 0 && i < _Tokens.Count ? _Tokens[i] : null;
        }

        /// <summary>
        /// Consume the next token
        /// </summary>
        /// <returns></returns>
        private Token Consume()
        {
            if (!HasInput())
                return null;
            return _Tokens[_Pointer++];
        }

        /// <summary>
        /// Consume the next token making sure its type matches the provided type. If the next token does not match with the type
        /// throw an exception. If message is not null use it as the exception's message
        /// </summary>
        /// <param name="type">Expected type of the next token</param>
        /// <param name="message">Error message if the token's type does not match the provided type</param>
        /// <returns></returns>
        private Token Consume(TokenType type, string message = null)
        {
            if (!HasInput())
            {
                // ; is optional at the end of the input
                if (type == TokenType.Semicolon)
                {
                    var lt = _Tokens.LastOrDefault();
                    return new Token()
                    {
                        Line = lt?.Line ?? 0,
                        Col = lt?.Col+1 ?? 0,
                        Type = TokenType.Semicolon,
                        Value = ";"
                    };
                }
                Token t = _Tokens.ElementAtOrDefault(_Pointer - 1);
                if (t != null)
                    throw new ParserException(message ?? $"Expects {type} at line {t.Line}:{t.Col}");
                throw new ParserException(message ?? $"Expects {type} the end of the input");
            }

            if (!Match(type))
            {
                Token t = _Tokens.ElementAtOrDefault(_Pointer - 1);
                throw new ParserException(message ?? $"Expects {type} but received {Peek().Type} at line {t.Line}:{t.Col}");
            }

            return _Tokens[_Pointer++];
        }

        /// <summary>
        /// Move the pointer back one position
        /// </summary>
        /// <param name="t"></param>
        private void Restore(Token t)
        {
            _Pointer--;
        }

        #endregion

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

        private bool IsVarDeclaration()
        {
            // 'var' identifier ( '=' expression )?
            if (Match(TokenType.Variable))
                return true;

            if (Match(TokenType.Identifier))
            {
                // identifier identifier ( '=' expression )?
                if (MatchFrom(1, TokenType.Identifier))
                    return true;

                // identifier ( '[' ']' )+ identifier ( '=' expression )?
                int dimensions = CountRepeatedMatchesFrom(1, TokenType.LeftBracket, TokenType.RightBracket);
                return dimensions > 0 && MatchFrom(dimensions + 1, TokenType.Identifier);
            }
            return false;
        }

        // Rule:
        // declaration	-> func_declaration
        //               | variable_declaration
        //	             | constant_declaration
        //	             | statement
        //
        private AstNode Declaration()
        {
            if (IsVarDeclaration())
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
        // func_declaration -> "fn" IDENTIFIER "(" func_params? ")" "{" declaration* "}"
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
            AstTupleNode expr = null;
            if (!Match(TokenType.Semicolon))
                expr = new AstTupleNode(ExpressionList());
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
            (condition, body) = Match(TokenType.LeftParen) ? ParenthesizedExpression() : BracedExpression();
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
        // 				    | expression_list
        private AstNode ForInitializer()
        {
            if (IsVarDeclaration())
            {
                return ForDeclaration();
            }
            return ExpressionList();
        }

        // Rule:
        // for_declaration -> ( implicit_var_declaration | typed_var_declaration )
        private AstNode ForDeclaration()
        {
            AstNode variable = null;
            if (Match(TokenType.Variable))
            {
                variable = ImplicitVarDeclaration();
            }
            else
            {
                variable = TypedVarDeclaration();
            }
            return new AstDeclarationNode(new List<AstNode>() { variable });
        }

        // Rule:
        // for_iterator -> expression ( "," expression )*
        private AstNode ForIterator()
        {
            List<AstNode> exprs = new List<AstNode> {
                Expression()
            };
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

            (condition, thenbranch) = Match(TokenType.LeftParen) ? ParenthesizedExpression() : BracedExpression();

            // Parse the else branch if present
            if (Match(TokenType.Else))
            {
                Consume(TokenType.Else);
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
        // variable_declaration -> ( implicit_var_declaration | typed_var_declaration ) ';'
        private AstNode VarDeclaration()
        {
            AstNode variable = null;
            if (Match(TokenType.Variable))
            {
                variable = ImplicitVarDeclaration();
            }
            else
            {
                variable = TypedVarDeclaration();
            }
            Consume(TokenType.Semicolon);
            return variable;
        }

        // Rule:
        // implicit_var_declaration -> VAR IDENTIFIER '=' expression
        private AstVariableNode ImplicitVarDeclaration()
        {
            AstVariableTypeNode variableType = new AstVariableTypeNode(Consume(TokenType.Variable));
            Token identifier = Consume(TokenType.Identifier);
            Consume(TokenType.Assignment, "Implicitly typed variables must be initialized");
            AstNode expression = Expression();
            return new AstVariableNode(variableType, identifier, expression);
        }

        // Rule:
        // typed_var_declaration -> IDENTIFIER ( '[' ']' )* typed_var_definition
        private AstVariableNode TypedVarDeclaration()
        {
            Token varType = Consume(TokenType.Identifier);
            AstVariableTypeNode variableType = new AstVariableTypeNode(varType);

            if (Match(TokenType.LeftBracket))
            {
                List<Token> dimensions = new List<Token>();
                while (Match(TokenType.LeftBracket))
                {
                    dimensions.Add(Consume(TokenType.LeftBracket));
                    dimensions.Add(Consume(TokenType.RightBracket));
                }
                variableType = new AstVariableTypeNode(varType, dimensions);
            }

            return new AstVariableNode(variableType, TypedVarDefinition());
        }

        // Rule: 
        // typed_var_definition -> IDENTIFIER ( '=' expression ( ',' typed_var_definition )* )?
        private List<Tuple<Token,AstNode>> TypedVarDefinition()
        {
            List<Tuple<Token, AstNode>> vars = new List<Tuple<Token, AstNode>>();
            do
            {
                var id = Consume(TokenType.Identifier);
                if (Match(TokenType.Assignment))
                {
                    Consume(TokenType.Assignment);
                    vars.Add(new Tuple<Token, AstNode>(id, Expression()));
                }
                else
                {
                    vars.Add(new Tuple<Token, AstNode>(id, null));
                }
            } while (Match(TokenType.Comma) && Consume(TokenType.Comma) != null);
            return vars;
        }

        // Rule:
        // constant_declaration -> CONST IDENTIFIER "=" expression ";"
        private AstNode ConstDeclaration()
        {
            Consume(TokenType.Constant);
            Token type = null;
            Token identifier = Consume(TokenType.Identifier);
            if (Match(TokenType.Identifier))
            {
                type = identifier;
                identifier = Consume(TokenType.Identifier);
            }
            Consume(TokenType.Assignment, "A constant value needs to be defined when declared.");
            AstNode expression = Expression();
            Consume(TokenType.Semicolon);
            return new AstConstantNode(identifier, expression, type);
        }

        // Rule:
        // expression_list -> expression ( ',' expression )*
        private AstExpressionListNode ExpressionList()
        {
            List<AstNode> exprs = new List<AstNode> {
                Expression()
            };
            while (Match(TokenType.Comma))
            {
                Consume();
                exprs.Add(Expression());
            }
            return new AstExpressionListNode(exprs);
        }

        // Rule:
        // expression -> expression_assignment
        private AstNode Expression()
        {
            return ExpressionAssignment();
        }

        private AstNode LambdaExpression()
        {
            AstParametersNode lambdaParams = LambdaParams();
            var arrow = Consume(TokenType.RightArrow);
            AstNode expr = Match(TokenType.LeftBrace) ? Block() : Expression();
            return new AstFuncDeclNode(arrow, lambdaParams, new List<AstNode>() { expr });
        }

        private AstParametersNode LambdaParams()
        {
            List<Token> parameters = new List<Token>();
            bool parenthesis = false;
            if (Match(TokenType.LeftParen))
            {
                parenthesis = true;
                Consume();
            }
            while (Match(TokenType.Identifier))
            {
                parameters.Add(Consume(TokenType.Identifier));
                if (Match(TokenType.Comma))
                    Consume();
            }
            if (parenthesis)
                Consume(TokenType.RightParen);
            return new AstParametersNode(parameters);
        }

        // Rule:
        // left_hand_side_expr -> IDENTIFIER ( '.' IDENTIFIER | arguments )*
        private AstNode LeftHandSideExpression()
        {
            // Get the identifier token
            Token identifier = Consume(TokenType.Identifier);

            AstNode accessor = new AstAccessorNode(identifier, null);

            // Try to find member accessors or callable members
            //  member.property.property2
            //  member.property()
            while (true)
            {
                if (Match(TokenType.LeftParen))
                {
                    AstExpressionListNode arguments = Arguments();
                    accessor = new AstCallableNode(accessor, arguments);
                }
                else if (Match(TokenType.Dot))
                {
                    Consume();
                    accessor = new AstAccessorNode(Consume(TokenType.Identifier), accessor);
                }
                else break;
            }
            return accessor;
        }

        // Rule:
        // destructuring -> '(' IDENTIFIER ( '.' IDENTIFIER )* ( ',' destructuring )* ')'
        private AstTupleNode Destructuring()
        {
            List<AstNode> exprs = new List<AstNode>();
            Consume(TokenType.LeftParen);
            do
            {
                // Get the identifier token
                Token identifier = Consume(TokenType.Identifier);

                AstNode accessor = new AstAccessorNode(identifier, null);

                // Try to find member accessors or callable members
                //  member.property.property2
                while (true)
                {
                    if (Match(TokenType.Dot))
                    {
                        Consume();
                        accessor = new AstAccessorNode(Consume(TokenType.Identifier), accessor);
                    }
                    else break;
                }
                exprs.Add(accessor);
            } while (Match(TokenType.Comma) && Consume(TokenType.Comma) != null);
            Consume(TokenType.RightParen);
            return new AstTupleNode(exprs);
        }

        /// <summary>
        /// It is destructuring if it is wrapped by parenthesis and is a lvalue
        /// </summary>
        /// <returns></returns>
        private bool IsDestructuring()
        {
            return Match(TokenType.LeftParen, TokenType.Identifier) && MatchAnyFrom(2, TokenType.Dot, TokenType.Comma, TokenType.RightParen);
        }

        private bool IsAssignment()
        {
            return Match(TokenType.Identifier)
                && MatchAnyFrom(1, TokenType.Dot, TokenType.LeftParen, TokenType.Assignment, TokenType.IncrementAndAssign, TokenType.DecrementAndAssign, TokenType.DivideAndAssign, TokenType.MultAndAssign);
        }

        // Rule:
        // expression_assignment	-> ( destructuring | left_hand_side_expr ) ( ( '=' | '+=' | '-=' | '/=' | '*=' )  expression_assignment )?
        // 						     | lambda_expression
        //                           | tuple_initializer
        // 						     | conditional_expression
        private AstNode ExpressionAssignment()
        {
            if (IsDestructuring())
            {
                int checkpoint = _Pointer;

                AstTupleNode lvalue = Destructuring();

                // If we now match an assignment of any type, create an assignment node
                if (MatchAny(TokenType.Assignment, TokenType.IncrementAndAssign, TokenType.DecrementAndAssign, TokenType.DivideAndAssign, TokenType.MultAndAssign))
                {
                    Token assignmentop = Consume();
                    AstNode expression = Expression();
                    return new AstAssignmentNode(lvalue, assignmentop, expression);
                }

                // If we are not seeing an assignment operator, then try again with ConditionalExpression
                _Pointer = checkpoint;
            }
            
            // Check for:
            //  identifier ( = | += | -= | *= | /= )
            //  identifier.
            //  identifier(
            if (IsAssignment())
            {
                int checkpoint = _Pointer;

                AstNode lvalue = LeftHandSideExpression();

                // If we now match an assignment of any type, create an assignment node
                if (MatchAny(TokenType.Assignment, TokenType.IncrementAndAssign, TokenType.DecrementAndAssign, TokenType.DivideAndAssign, TokenType.MultAndAssign))
                {
                    Token assignmentop = Consume();
                    AstNode expression = Expression();

                    if (lvalue is AstCallableNode)
                        throw new ParserException($"Left-hand side of an assignment must be a variable.");

                    return new AstAssignmentNode(lvalue as AstAccessorNode, assignmentop, expression);
                }

                // If we are not seeing an assignment operator, then try again with ConditionalExpression
                _Pointer = checkpoint;
                return ConditionalExpression();
            }
            else if (MatchLambda())
            {
                return LambdaExpression();
            }
            else if (Match(TokenType.LeftParen))
            {
                int checkpoint = _Pointer;
                List<AstNode> args = new List<AstNode>();
                Consume(TokenType.LeftParen);
                if (!Match(TokenType.RightParen))
                {
                    args.Add(Expression());
                    while (Match(TokenType.Comma))
                    {
                        Consume();
                        args.Add(Expression());
                    }
                }
                Consume(TokenType.RightParen);
                return new AstTupleNode(args);
            }
            return ConditionalExpression();
        }

        private bool MatchLambda()
        {
            // =>
            bool arrow = Match(TokenType.RightArrow);
            // a =>
            bool paramArrow = (!Match(TokenType.LeftParen) && Match(TokenType.Unknown, TokenType.RightArrow));
            // a,b =>
            bool paramListArrow = MatchFrom(CountRepeatedMatchesFrom(0, TokenType.Unknown, TokenType.Comma), TokenType.RightArrow);
            // () =>
            bool parentArrow = Match(TokenType.LeftParen, TokenType.RightParen, TokenType.RightArrow);
            // (a) =>
            bool parentParamArrow = (!MatchFrom(1, TokenType.LeftParen) && Match(TokenType.LeftParen, TokenType.Unknown, TokenType.RightParen, TokenType.RightArrow));
            // (a,b) =>
            bool parentParamListArrow = (Match(TokenType.LeftParen) && MatchFrom(1 + CountRepeatedMatchesFrom(1, TokenType.Unknown, TokenType.Comma), TokenType.RightParen, TokenType.RightArrow));

            return arrow || paramArrow || paramListArrow || parentArrow || parentParamArrow || parentParamListArrow;
        }

        // Rule:
        // conditional_expression -> null_coalescing_expression ( '?' expression ':' expression )?
        private AstNode ConditionalExpression()
        {
            AstNode nullCoalescingExpr = NullCoalescingExpression();
            if (Match(TokenType.Question))
            {
                Token q = Consume();
                AstNode trueExpr = Expression();
                Consume(TokenType.Colon);
                AstNode falseExpr = Expression();
                return new AstIfNode(q, nullCoalescingExpr, trueExpr, falseExpr);
            }
            return nullCoalescingExpr;
        }

        // Rule:
        // null_coalescing_expression -> or_expression ( '??' null_coalescing_expression )?
        private AstNode NullCoalescingExpression()
        {
            AstNode orExpr = OrExpression();
            if (Match(TokenType.QuestionQuestion))
            {
                Token q = Consume();
                AstNode rightExpr = NullCoalescingExpression();
                return new AstNullCoalescingNode(q, orExpr, rightExpr);
            }
            return orExpr;
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

        private AstAccessorNode TryGetAccessor(AstNode primary)
        {
            AstNode tmp = primary;
            while (tmp != null)
            {
                if (tmp is AstAccessorNode)
                    return tmp as AstAccessorNode;

                if (tmp is AstCallableNode)
                {
                    tmp = (tmp as AstCallableNode).Callable;
                    continue;
                }
            }
            return null;
        }

        private AstLiteralNode TryGetLiteral(AstNode primary)
        {
            AstNode tmp = primary;
            while (tmp != null)
            {
                if (tmp is AstLiteralNode)
                    return tmp as AstLiteralNode;

                if (tmp is AstAccessorNode)
                {
                    tmp = (tmp as AstAccessorNode).Member;
                    continue;
                }
                else if (tmp is AstCallableNode)
                {
                    tmp = (tmp as AstCallableNode).Callable;
                    continue;
                }
            }
            return null;
        }

        // Rule:
        // primary_expression -> primary ( "." IDENTIFIER | "(" arguments? ")" )*
        private AstNode PrimaryExpression()
        {
            Token newt = null;
            if (Match(TokenType.New))
            {
                newt = Consume();
            }
            AstNode primary = Primary();
            int checkpoint = _Pointer;
            while (true)
            {
                if (Match(TokenType.LeftParen))
                {
                    if (TryGetAccessor(primary) == null && TryGetLiteral(primary)?.Primary?.Type != TokenType.Identifier)
                        throw new ParserException($"'{primary}' is not an invokable object");
                    AstExpressionListNode arguments = Arguments();
                    primary = new AstCallableNode(primary, arguments, newt);
                }
                else if (Match(TokenType.LeftBracket))
                {
                    primary = new AstIndexerNode(primary, Indexer());
                }
                else if (Match(TokenType.Dot))
                {
                    Consume();
                    primary = new AstAccessorNode(Consume(TokenType.Identifier), primary);
                }
                else
                {
                    if (_Pointer == checkpoint 
                        && (primary is AstLiteralNode && (primary as AstLiteralNode).Primary.Type != TokenType.Identifier)
                        && newt != null)
                        throw new ParserException($"Type expected");
                    if (_Pointer == checkpoint && (primary is AstAccessorNode) && newt != null)
                        primary = new AstCallableNode(primary, new AstExpressionListNode(new List<AstNode>()), newt);
                    break;
                }

            }
            return primary;
        }

        // Rule: (it is easier to check the expression_list manually here)
        // indexer -> '[' expression_list ']'
        private AstExpressionListNode Indexer()
        {
            List<AstNode> args = new List<AstNode>();
            Consume(TokenType.LeftBracket);
            do
            {
                args.Add(Expression());
                while (Match(TokenType.Comma))
                {
                    Consume();
                    args.Add(Expression());
                }
            } while ((!Match(TokenType.RightBracket)));
            Consume(TokenType.RightBracket);
            return new AstExpressionListNode(args);
        }

        // Rule: (it is easier to check the expression_list manually here)
        // arguments -> '(' expression_list? ')'
        private AstExpressionListNode Arguments()
        {
            List<AstNode> args = new List<AstNode>();
            Consume(TokenType.LeftParen);
            if (!Match(TokenType.RightParen))
            {
                args.Add(Expression());
                while (Match(TokenType.Comma))
                {
                    Consume();
                    args.Add(Expression());
                }
            }
            Consume(TokenType.RightParen);
            return new AstExpressionListNode(args);
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
                throw new ParserException($"Expects primary but received {(HasInput() ? Peek().Type.ToString() : "end of input")}");

            return Match(TokenType.Identifier) ? (AstNode)new AstAccessorNode(Consume(), null) : new AstLiteralNode(Consume());
        }
        #endregion
    }
}
