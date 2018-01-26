// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Fl.Parser.Ast;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

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

        private List<ParserException> _ParsingErrors;
        #endregion

        #region Constructor

        public AstNode Parse(List<Token> tokens)
        {
            _Pointer = 0;
            _Tokens = tokens;
            _ParsingErrors = new List<ParserException>();
            return Program();
        }

        #endregion

        public ReadOnlyCollection<ParserException> ParsingErrors => new ReadOnlyCollection<ParserException>(_ParsingErrors);

        #region Parser state

        /// <summary>
        /// Now it just contains a copy of the pointer. It is overkill but 
        /// in the future if the parser starts to keep track of states, this
        /// class will be helpful to keep track of them
        /// </summary>
        protected class ParserCheckpoint
        {
            public int Pointer { get; }

            public ParserCheckpoint(int pointer)
            {
                Pointer = pointer;
            }
        }

        /// <summary>
        /// Get a copy o the current parser's state
        /// </summary>
        /// <returns></returns>
        protected ParserCheckpoint SaveCheckpoint()
        {
            return new ParserCheckpoint(_Pointer);
        }

        /// <summary>
        /// Restore a previous parser's state
        /// </summary>
        /// <param name="checkpoint"></param>
        protected void RestoreCheckpoint(ParserCheckpoint checkpoint)
        {
            _Pointer = checkpoint.Pointer;
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
            if (offset < 0) throw new ArgumentOutOfRangeException(nameof(offset), "Offset cannot be negative");

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
            if (offset < 0) throw new ArgumentOutOfRangeException(nameof(offset), "Offset cannot be negative");
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

        private string GetSourceContext()
        {
            // TODO: Retrieve context from Lexer
            return "";
        }

        private string GetCurrentLineAndCol()
        {
            Token t = _Tokens.ElementAtOrDefault(_Pointer - 1);
            return t != null ? $"[Line {t.Line}:{t.Col}]" : "[Line 0:0]";
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
                
                throw new ParserException(message ?? $"{GetCurrentLineAndCol()} Expects {type} but received the end of the input: {GetSourceContext()}");
            }

            if (!Match(type))
            {
                throw new ParserException(message ?? $"{GetCurrentLineAndCol()} Expects {type} but received {Peek().Type}: {GetSourceContext()}");
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
                try
                {
                    if (Match(TokenType.Semicolon))
                    {
                        Consume();
                        continue;
                    }
                    statements.Add(Declaration());
                }
                catch (ParserException pe)
                {
                    _ParsingErrors.Add(pe);
                    while (HasInput() && !Match(TokenType.Semicolon))
                        Consume();
                }
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
        // variable_declaration -> ( implicit_var_declaration | typed_var_declaration ) ';'
        private AstVariableNode VarDeclaration()
        {
            AstVariableNode variable = null;
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
        // implicit_var_declaration -> VAR ( IDENTIFIER '=' expression | var_destructuring )';'
        private AstVariableNode ImplicitVarDeclaration()
        {
            AstVariableTypeNode variableType = new AstVariableTypeNode(Consume(TokenType.Variable));

            // If there is a left parent present, it is a destructuring declaration
            if (IsDestructuring())
                return VarDestructuring(variableType);
            
            // If not it is a common var declaration
            Token identifier = Consume(TokenType.Identifier);
            Consume(TokenType.Assignment, "Implicitly typed variables must be initialized");
            AstNode expression = Expression();
            return new AstVarDefinitionNode(variableType, new List<Tuple<Token, AstNode>>() { new Tuple<Token, AstNode>(identifier, expression) });
        }

        // Rule:
        // typed_var_declaration -> IDENTIFIER ( '[' ']' )* ( typed_var_definition | var_destructuring ) ';'
        private AstVariableNode TypedVarDeclaration()
        {
            // Get the variable type
            Token varType = Consume(TokenType.Identifier);
            AstVariableTypeNode variableType = new AstVariableTypeNode(varType);

            // If it contains a left bracket, it is an array variable
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

            // If there is a left parent present, it is a destructuring declaration
            if (IsDestructuring())
                return VarDestructuring(variableType);

            // If not, it is a simple typed var definition
            return new AstVarDefinitionNode(variableType, TypedVarDefinition());
        }

        // Rule: 
        // typed_var_definition -> IDENTIFIER ( '=' expression ( ',' typed_var_definition )* )?
        private List<Tuple<Token, AstNode>> TypedVarDefinition()
        {
            List<Tuple<Token, AstNode>> vars = new List<Tuple<Token, AstNode>>();
            do
            {
                // There could be multiple declarations and definitions, so consume the
                // identifier and the check if it is a definition or just a declaration
                var id = Consume(TokenType.Identifier);

                if (Match(TokenType.Assignment) && Consume(TokenType.Assignment) != null)
                    vars.Add(new Tuple<Token, AstNode>(id, Expression()));
                else
                    vars.Add(new Tuple<Token, AstNode>(id, null));

            } while (Match(TokenType.Comma) && Consume(TokenType.Comma) != null);

            return vars;
        }

        // Rule: 
        // var_destructuring -> '(' ( ',' | IDENTIFIER )+ ')' '=' expression
        private AstVarDestructuringNode VarDestructuring(AstVariableTypeNode varType)
        {
            List<Token> tokens = new List<Token>();

            // Consume the left hand side: (x,y,z) or (,y,) or (x,,) etc.
            bool hasParenthesis = false;

            if (Match(TokenType.LeftParen) && Consume(TokenType.LeftParen) != null)
                hasParenthesis = true;

            do
            {
                if (Match(TokenType.Comma))
                {
                    tokens.Add(null);
                }
                else if (Match(TokenType.Identifier))
                {
                    tokens.Add(Consume(TokenType.Identifier));
                }
            } while (Match(TokenType.Comma) && Consume(TokenType.Comma) != null);

            if (hasParenthesis)
                Consume(TokenType.RightParen);

            // Destructuring just work with assignment, it is a must
            Consume(TokenType.Assignment);

            // Get the expression that will need to return a Tuple value, let the runtime check that
            return new AstVarDestructuringNode(varType, tokens, Expression());
        }

        // Rule:
        // constant_declaration -> CONST IDENTIFIER IDENTIFIER '=' expression ( ',' IDENTIFIER '=' expression )* ) ';'
        private AstConstantNode ConstDeclaration()
        {
            // Consume the keyword
            Consume(TokenType.Constant);

            // Get the constant type
            Token type = Consume(TokenType.Identifier);

            // Consume multiple constants declarations and definitions
            List<Tuple<Token, AstNode>> constdefs = new List<Tuple<Token, AstNode>>();
            do
            {
                Token identifier = Consume(TokenType.Identifier);
                Consume(TokenType.Assignment, "A constant value needs to be defined when declared.");
                AstNode expression = Expression();

                constdefs.Add(new Tuple<Token, AstNode>(identifier, expression));

            } while (Match(TokenType.Comma) && Consume(TokenType.Comma) != null);

            Consume(TokenType.Semicolon);

            return new AstConstantNode(type, constdefs);
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
        // return_statement -> 'return' ( expression ( ',' expression )* )? ';'
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
        private (AstNode, AstNode) ParenthesizedStatement()
        {
            AstNode expression = ParenthesizedExpression();
            AstNode stmt = Match(TokenType.Semicolon) ? new AstNoOpNode(Consume(TokenType.Semicolon)) : Statement();
            return (expression, stmt);
        }

        private AstNode ParenthesizedExpression()
        {
            Consume(TokenType.LeftParen);
            AstNode expression = Expression();
            Consume(TokenType.RightParen);
            return expression;
        }

        // Rule:
        // braced_expr -> expression block
        private (AstNode, AstNode) BracedStatement()
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
            (condition, body) = Match(TokenType.LeftParen) ? ParenthesizedStatement() : BracedStatement();
            return new AstWhileNode(kw, condition, body);
        }

        #region for_statement

        // Rule:
        // for_statement -> "for" "(" for_initializer? ";" expression? ";" for_iterator? ")" statement
        // 			      | "for" for_initializer? ";" expression? ";" for_iterator? block
        private AstForNode ForStatement()
        {
            if (Match(TokenType.For, TokenType.LeftParen))
                return ParenthesizedForStatemet();

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
        private AstForNode ParenthesizedForStatemet()
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
        private AstDeclarationNode ForDeclaration()
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
        private AstDeclarationNode ForIterator()
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
        private AstIfNode IfStatement()
        {
            Token kw = Consume(TokenType.If);
            AstNode condition = null;
            AstNode thenbranch = null;
            AstNode elsebranch = null;

            (condition, thenbranch) = Match(TokenType.LeftParen) ? ParenthesizedStatement() : BracedStatement();

            // Parse the else branch if present
            if (Match(TokenType.Else))
            {
                Consume(TokenType.Else);
                elsebranch = Match(TokenType.Semicolon) ? new AstNoOpNode(Consume(TokenType.Semicolon)) : Statement();
            }
            return new AstIfNode(kw, condition, thenbranch, elsebranch);
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
        // expression_statement	-> expression ";"
        private AstNode ExpressionStatement()
        {
            AstNode expr = Expression();
            Consume(TokenType.Semicolon);
            return expr;
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

        // Rule:
        // lambda_expression -> lambda_params '=>' ( block | expression )
        private AstFuncDeclNode LambdaExpression()
        {
            AstParametersNode lambdaParams = LambdaParams();
            var arrow = Consume(TokenType.RightArrow);
            AstNode expr = Match(TokenType.LeftBrace) ? Block() : Expression();
            return new AstFuncDeclNode(arrow, lambdaParams, new List<AstNode>() { expr });
        }

        // Rule:
        // lambda_params -> '(' func_params ')' | func_params
        private AstParametersNode LambdaParams()
        {
            List<Token> parameters = new List<Token>();

            // Lambda params could be wrapped between parenthesis
            bool parenthesis = false;

            if (Match(TokenType.LeftParen))
            {
                parenthesis = true;
                Consume();
            }

            // Consume the identifiers separated by commas
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
        // member_access -> IDENTIFIER ( '.' IDENTIFIER | arguments )*
        private AstNode MemberAccess()
        {
            // Create an accessor node for the identifier
            AstNode accessor = new AstAccessorNode(Consume(TokenType.Identifier), null);

            // Try to find member accessors or callable members like:
            //  member.property
            //  member.property.property2
            //  member.property()
            //  member.property.property2()
            //  member.property.property2().property3
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

            bool hasParenthesis = false;

            if (Match(TokenType.LeftParen) && Consume(TokenType.LeftParen) != null)
                hasParenthesis = true;
            do
            {
                AstNode accessor = null;

                // Try to find member accessors or callable members
                //  member.property.property2
                if (Match(TokenType.Identifier))
                {
                    do
                    {
                        accessor = new AstAccessorNode(Consume(TokenType.Identifier), accessor);
                    } while (Match(TokenType.Dot) && Consume(TokenType.Dot) != null);
                }

                if (accessor == null)
                    accessor = new AstNoOpNode(null);

                exprs.Add(accessor);

            } while (Match(TokenType.Comma) && Consume(TokenType.Comma) != null);

            if (hasParenthesis)
                Consume(TokenType.RightParen);

            return new AstTupleNode(exprs);
        }

        // Rule:
        // tuple_initializer -> '(' expression_list? ')'
        private AstTupleNode TupleInitializer()
        {
            List<AstNode> args = new List<AstNode>();
            bool isValidTuple = false;

            Consume(TokenType.LeftParen);

            while (!Match(TokenType.RightParen))
            {
                args.Add(Expression());
                while (Match(TokenType.Comma))
                {
                    isValidTuple = true;
                    Consume();
                }
            }

            Consume(TokenType.RightParen);

            return isValidTuple ? new AstTupleNode(args) : null;
        }

        // Rule:
        // expression_assignment	-> ( destructuring | member_access ) ( ( '=' | '+=' | '-=' | '/=' | '*=' )  expression_assignment )?
        // 						     | lambda_expression
        // 						     | conditional_expression
        private AstNode ExpressionAssignment()
        {
            // Check for:
            //  identifier ( = | += | -= | *= | /= )
            //  identifier.
            //  identifier(
            if (IsAssignment())
            {
                var checkpoint = SaveCheckpoint();

                AstNode lvalue = MemberAccess();

                // If we now match an assignment of any type, create an assignment node
                if (MatchAny(TokenType.Assignment, TokenType.IncrementAndAssign, TokenType.DecrementAndAssign, TokenType.DivideAndAssign, TokenType.MultAndAssign))
                {
                    Token assignmentop = Consume();
                    AstNode expression = Expression();

                    if (lvalue is AstCallableNode)
                        throw new ParserException($"{GetCurrentLineAndCol()} Left-hand side of an assignment must be a variable.");

                    return new AstVariableAssignmentNode(lvalue as AstAccessorNode, assignmentop, expression);
                }

                // If we are not seeing an assignment operator, then try again with ConditionalExpression
                RestoreCheckpoint(checkpoint);
                return ConditionalExpression();
            }

            // Try to parse a lambda expression
            if (MatchLambda())
                return LambdaExpression();

            if (IsDestructuring())
            {
                ParserCheckpoint checkpoint = SaveCheckpoint();
                // First try if it is a left-hand side expression
                try
                {
                    AstTupleNode lvalue = Destructuring();

                    // If we now match an assignment of any type, create an assignment node
                    if (MatchAny(TokenType.Assignment, TokenType.IncrementAndAssign, TokenType.DecrementAndAssign, TokenType.DivideAndAssign, TokenType.MultAndAssign))
                    {
                        Token assignmentop = Consume();
                        AstNode expression = Expression();
                        return new AstDestructuringAssignmentNode(lvalue, assignmentop, expression);
                    }
                }
                catch
                {
                }
                // If we reach this point, we restore the checkpoint
                RestoreCheckpoint(checkpoint);
            }

            // Finally try with a conditional expression
            return ConditionalExpression();
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
        private AstUnaryNode UnaryExpression()
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
            Token newt = null;
            if (Match(TokenType.New))
            {
                newt = Consume();
            }
            AstNode primary = Primary();
            var checkpoint = SaveCheckpoint();
            while (true)
            {
                if (Match(TokenType.LeftParen))
                {
                    if (TryGetAccessor(primary) == null && TryGetLiteral(primary)?.Literal?.Type != TokenType.Identifier)
                        throw new ParserException($"{GetCurrentLineAndCol()} '{primary}' is not an invokable object");
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
                    if (_Pointer == checkpoint.Pointer
                        && (primary is AstLiteralNode && (primary as AstLiteralNode).Literal.Type != TokenType.Identifier)
                        && newt != null)
                        throw new ParserException($"{GetCurrentLineAndCol()} Type expected");
                    if (_Pointer == checkpoint.Pointer && (primary is AstAccessorNode) && newt != null)
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
        // primary	-> "true" | "false" | "null" | INTEGER | DOUBLE | DECIMAL | STRING | IDENTIFIER	| tuple_initializer | "(" expression ")"
        private AstNode Primary()
        {
            if (Match(TokenType.LeftParen))
            {
                var checkpoint = SaveCheckpoint();
                // Now we can try if it is a literal tuple initializer
                try
                {
                    // This call will return null if it is not a valid tuple initializer
                    var node = TupleInitializer();

                    // If we match a dot, it could be a primary expression followed by a member access,
                    // it is not a literal tuple initializer
                    if (node != null && !MatchAny(TokenType.Dot))
                        return node;
                }
                catch
                {
                }

                // If we didn't match destructuring or tuple initializer, restore and try
                // a parenthesized expression
                RestoreCheckpoint(checkpoint);
                return ParenthesizedExpression();
            }

            if (!Match(TokenType.Boolean)
                && !Match(TokenType.Null)
                && !Match(TokenType.Char)
                && !Match(TokenType.Integer)
                && !Match(TokenType.Float)
                && !Match(TokenType.Double)
                && !Match(TokenType.Decimal)
                && !Match(TokenType.String)
                && !Match(TokenType.Identifier))
                throw new ParserException($"{GetCurrentLineAndCol()} Expects primary but received {(HasInput() ? Peek().Type.ToString() : "end of input")}");

            return Match(TokenType.Identifier) ? (AstNode)new AstAccessorNode(Consume(), null) : new AstLiteralNode(Consume());
        }
        #endregion

        #region Parser lookahead helpers

        private bool IsVarDeclaration()
        {
            // 'var' identifier ( '=' expression )?
            if (Match(TokenType.Variable))
                return true;

            if (Match(TokenType.Identifier, TokenType.LeftParen, TokenType.Identifier))
            {
                // identifier (identifier, identifier, ..., identifier)
                if (MatchAnyFrom(3, TokenType.Identifier, TokenType.Comma))
                    return true;

                /*// identifier ( '[' ']' )+ (identifier, identifier, ..., identifier)
                int dimensions = CountRepeatedMatchesFrom(1, TokenType.LeftBracket, TokenType.RightBracket);
                return dimensions > 0 && MatchFrom(dimensions + 1, TokenType.Identifier);*/
            }

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

        /// <summary>
        /// It is destructuring if it is wrapped by parenthesis and is a lvalue
        /// </summary>
        /// <returns></returns>
        private bool IsDestructuring()
        {
            // Check for a left-hand side expression containing a Comma
            if (!Match(TokenType.LeftParen))
            {
                for (int i=0; i < _Tokens.Count; i++)
                {
                    var t = PeekFrom(i);
                    if (t == null || t.Type == TokenType.Assignment)
                        break;
                    if (t.Type == TokenType.Comma)
                        return true;
                }
            }

            // Check for parenthesized list of identifiers
            return (Match(TokenType.LeftParen, TokenType.Identifier) && MatchAnyFrom(2, TokenType.Dot, TokenType.Comma, TokenType.RightParen))
                || Match(TokenType.LeftParen, TokenType.Comma);
        }

        private bool IsAssignment()
        {
            return Match(TokenType.Identifier)
                && MatchAnyFrom(1, TokenType.Dot, TokenType.LeftParen, TokenType.Assignment, TokenType.IncrementAndAssign, TokenType.DecrementAndAssign, TokenType.DivideAndAssign, TokenType.MultAndAssign);
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
                    tmp = (tmp as AstAccessorNode).Enclosing;
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

        #endregion
    }
}
