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
        private List<Token> tokens;

        /// <summary>
        /// Pointer to keep track of the current position
        /// </summary>
        private int pointer;

        private List<ParserException> parsingErrors;
        #endregion

        #region Constructor

        public AstNode Parse(List<Token> tokens)
        {
            this.pointer = 0;
            this.tokens = tokens;
            this.parsingErrors = new List<ParserException>();
            return this.Program();
        }

        #endregion

        public ReadOnlyCollection<ParserException> ParsingErrors => new ReadOnlyCollection<ParserException>(this.parsingErrors);

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
                this.Pointer = pointer;
            }
        }

        /// <summary>
        /// Get a copy o the current parser's state
        /// </summary>
        /// <returns></returns>
        protected ParserCheckpoint SaveCheckpoint()
        {
            return new ParserCheckpoint(this.pointer);
        }

        /// <summary>
        /// Restore a previous parser's state
        /// </summary>
        /// <param name="checkpoint"></param>
        protected void RestoreCheckpoint(ParserCheckpoint checkpoint)
        {
            this.pointer = checkpoint.Pointer;
        }

        #endregion

        #region Parsing helpers

        private bool HasInput()
        {
            return this.pointer < this.tokens.Count;
        }

        /// <summary>
        /// Return true if the following tokens starting from the current position match in type with the provided array of types
        /// </summary>
        /// <param name="types">Target stream of types to match</param>
        /// <returns></returns>
        private bool Match(params TokenType[] types)
        {
            return this.MatchFrom(0, types);
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
            if (l + this.pointer + offset > this.tokens.Count || this.pointer + offset < 0)
                return false;
            for (int i = 0; i < types.Length; i++)
            {
                if (types[i] == TokenType.Unknown)
                    continue;
                if (this.tokens[this.pointer + i + offset].Type != types[i])
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
            var t = this.Peek();
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
            var t = this.PeekFrom(offset);
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
            if (l + this.pointer + offset > this.tokens.Count)
                return 0;

            int i = 0;
            bool valid = true;
            while (valid && i + offset + this.pointer < this.tokens.Count)
            {
                for (int j = 0; j < types.Length; j++, i++)
                {
                    if (types[j] == TokenType.Unknown)
                    {
                        q++;
                        continue;
                    }
                    if (this.pointer + i + offset >= this.tokens.Count || this.tokens[this.pointer + i + offset].Type != types[j])
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
            return this.pointer < this.tokens.Count ? this.tokens[this.pointer] : null;
        }

        /// <summary>
        /// Return the next token based on the current position with a positive offset
        /// </summary>
        /// <param name="offset"></param>
        /// <returns></returns>
        private Token PeekFrom(int offset)
        {
            //if (offset < 0) throw new ArgumentOutOfRangeException(nameof(offset), "Offset cannot be negative");
            int i = this.pointer + offset;
            return i >= 0 && i < this.tokens.Count ? this.tokens[i] : null;
        }

        /// <summary>
        /// Consume the next token
        /// </summary>
        /// <returns></returns>
        private Token Consume()
        {
            if (!this.HasInput())
                return null;
            return this.tokens[this.pointer++];
        }

        private string GetSourceContext()
        {
            // TODO: Retrieve context from Lexer
            return "";
        }

        private string GetCurrentLineAndCol()
        {
            Token t = this.tokens.ElementAtOrDefault(this.pointer - 1);
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
            if (!this.HasInput())
            {
                // ; is optional at the end of the input
                if (type == TokenType.Semicolon)
                {
                    var lt = this.tokens.LastOrDefault();
                    return new Token()
                    {
                        Line = lt?.Line ?? 0,
                        Col = lt?.Col+1 ?? 0,
                        Type = TokenType.Semicolon,
                        Value = ";"
                    };
                }
                
                throw new ParserException(message ?? $"{this.GetCurrentLineAndCol()} Expects {type} but received the end of the input: {this.GetSourceContext()}");
            }

            if (!this.Match(type))
            {
                throw new ParserException(message ?? $"{this.GetCurrentLineAndCol()} Expects {type} but received {this.Peek().Type}: {this.GetSourceContext()}");
            }

            return this.tokens[this.pointer++];
        }

        /// <summary>
        /// Move the pointer back one position
        /// </summary>
        /// <param name="t"></param>
        private void Restore(Token t)
        {
            this.pointer--;
        }

        #endregion

        #region Grammar

        // Rule:
        // program -> declaration*
        private AstDeclarationNode Program()
        {            
            List<AstNode> statements = new List<AstNode>();
            while (this.HasInput())
            {
                try
                {
                    if (this.Match(TokenType.Semicolon))
                    {
                        this.Consume();
                        continue;
                    }
                    statements.Add(this.Declaration());
                }
                catch (ParserException pe)
                {
                    this.parsingErrors.Add(pe);
                    while (this.HasInput() && !this.Match(TokenType.Semicolon))
                        this.Consume();
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
            if (this.IsVarDeclaration())
            {
                return this.VarDeclaration();
            }
            else if (this.Match(TokenType.Constant))
            {
                return this.ConstDeclaration();
            }
            else if (this.Match(TokenType.Function))
            {
                return this.FuncDeclaration();
            }
            return this.Statement();
        }

        // Rule:
        // variable_declaration -> ( implicit_var_declaration | typed_var_declaration ) ';'
        private AstVariableNode VarDeclaration()
        {
            AstVariableNode variable = null;
            if (this.Match(TokenType.Variable))
            {
                variable = this.ImplicitVarDeclaration();
            }
            else
            {
                variable = this.TypedVarDeclaration();
            }
            this.Consume(TokenType.Semicolon);
            return variable;
        }

        // Rule:
        // implicit_var_declaration -> VAR ( IDENTIFIER '=' expression | var_destructuring )';'
        private AstVariableNode ImplicitVarDeclaration()
        {
            AstVariableTypeNode variableType = new AstVariableTypeNode(this.Consume(TokenType.Variable));

            // If there is a left parent present, it is a destructuring declaration
            if (this.IsDestructuring())
                return this.VarDestructuring(variableType);
            
            // If not it is a common var declaration
            Token identifier = this.Consume(TokenType.Identifier);
            this.Consume(TokenType.Assignment, "Implicitly typed variables must be initialized");
            AstNode expression = this.Expression();
            return new AstVarDefinitionNode(variableType, new List<Tuple<Token, AstNode>>() { new Tuple<Token, AstNode>(identifier, expression) });
        }

        // Rule:
        // typed_var_declaration -> IDENTIFIER ( '[' ']' )* ( typed_var_definition | var_destructuring ) ';'
        private AstVariableNode TypedVarDeclaration()
        {
            // Get the variable type
            Token varType = this.Consume(TokenType.Identifier);
            AstVariableTypeNode variableType = new AstVariableTypeNode(varType);

            // If it contains a left bracket, it is an array variable
            if (this.Match(TokenType.LeftBracket))
            {
                List<Token> dimensions = new List<Token>();
                while (this.Match(TokenType.LeftBracket))
                {
                    dimensions.Add(this.Consume(TokenType.LeftBracket));
                    dimensions.Add(this.Consume(TokenType.RightBracket));
                }
                variableType = new AstVariableTypeNode(varType, dimensions);
            }

            // If there is a left parent present, it is a destructuring declaration
            if (this.IsDestructuring())
                return this.VarDestructuring(variableType);

            // If not, it is a simple typed var definition
            return new AstVarDefinitionNode(variableType, this.TypedVarDefinition());
        }

        // Rule: 
        // typed_var_definition -> IDENTIFIER ( '=' expression ( ',' typed_var_definition )* )?
        private List<Tuple<Token, AstNode>> TypedVarDefinition()
        {
            List<Tuple<Token, AstNode>> vars = new List<Tuple<Token, AstNode>>();
            do
            {
                // There could be multiple declarations and definitions, so consume the
                // identifier and then check if it is a definition or just a declaration
                var id = this.Consume(TokenType.Identifier);

                if (this.Match(TokenType.Assignment) && this.Consume(TokenType.Assignment) != null)
                    vars.Add(new Tuple<Token, AstNode>(id, this.Expression()));
                else
                    vars.Add(new Tuple<Token, AstNode>(id, null));

            } while (this.Match(TokenType.Comma) && this.Consume(TokenType.Comma) != null);

            return vars;
        }

        // Rule: 
        // var_destructuring -> '(' ( ',' | IDENTIFIER )+ ')' '=' expression
        private AstVarDestructuringNode VarDestructuring(AstVariableTypeNode varType)
        {
            List<Token> tokens = new List<Token>();

            // Consume the left hand side: (x,y,z) or (,y,) or (x,,) etc.
            bool hasParenthesis = false;

            if (this.Match(TokenType.LeftParen) && this.Consume(TokenType.LeftParen) != null)
                hasParenthesis = true;

            do
            {
                if (this.Match(TokenType.Comma))
                {
                    tokens.Add(null);
                }
                else if (this.Match(TokenType.Identifier))
                {
                    tokens.Add(this.Consume(TokenType.Identifier));
                }
            } while (this.Match(TokenType.Comma) && this.Consume(TokenType.Comma) != null);

            if (hasParenthesis)
                this.Consume(TokenType.RightParen);

            // Destructuring just work with assignment, it is a must
            this.Consume(TokenType.Assignment);

            // Get the expression that will need to return a Tuple value, let the runtime check that
            return new AstVarDestructuringNode(varType, tokens, this.Expression());
        }

        // Rule:
        // constant_declaration -> CONST IDENTIFIER IDENTIFIER '=' expression ( ',' IDENTIFIER '=' expression )* ) ';'
        private AstConstantNode ConstDeclaration()
        {
            // Consume the keyword
            this.Consume(TokenType.Constant);

            // Get the constant type
            Token type = this.Consume(TokenType.Identifier);

            // Consume multiple constants declarations and definitions
            List<Tuple<Token, AstNode>> constdefs = new List<Tuple<Token, AstNode>>();
            do
            {
                Token identifier = this.Consume(TokenType.Identifier);
                this.Consume(TokenType.Assignment, "A constant value needs to be defined when declared.");
                AstNode expression = this.Expression();

                constdefs.Add(new Tuple<Token, AstNode>(identifier, expression));

            } while (this.Match(TokenType.Comma) && this.Consume(TokenType.Comma) != null);

            this.Consume(TokenType.Semicolon);

            return new AstConstantNode(type, constdefs);
        }

        // Rule:
        // func_declaration -> "fn" IDENTIFIER "(" func_params? ")" "{" declaration* "}"
        private AstFuncDeclNode FuncDeclaration()
        {
            this.Consume(TokenType.Function);

            Token name = this.Consume(TokenType.Identifier);

            this.Consume(TokenType.LeftParen);

            AstParametersNode parameters = this.FuncParameters();

            this.Consume(TokenType.RightParen);
            
            List<AstNode> decls = new List<AstNode>();
            this.Consume(TokenType.LeftBrace);
            while (!this.Match(TokenType.RightBrace))
            {
                decls.Add(this.Declaration());
            }
            this.Consume(TokenType.RightBrace);

            return new AstFuncDeclNode(name, parameters, decls);
        }

        // Rule:
        // func_params -> IDENTIFIER ( "," IDENTIFIER )*
        private AstParametersNode FuncParameters()
        {
            List<Token> parameters = new List<Token>();
            while (this.Match(TokenType.Identifier))
            {
                parameters.Add(this.Consume(TokenType.Identifier));
                if (this.Match(TokenType.Comma))
                    this.Consume();
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
            if (this.Match(TokenType.If))
                return this.IfStatement();

            if (this.Match(TokenType.LeftBrace))
                return this.Block();

            if (this.Match(TokenType.While))
                return this.WhileStatement();

            if (this.Match(TokenType.For))
                return this.ForStatement();

            if (this.Match(TokenType.Break))
                return this.BreakStatement();

            if (this.Match(TokenType.Continue))
                return this.ContinueStatement();

            if (this.Match(TokenType.Return))
                return this.ReturnStatement();

            return this.ExpressionStatement();
        }

        // Rule:
        // return_statement -> 'return' ( expression ( ',' expression )* )? ';'
        private AstReturnNode ReturnStatement()
        {
            Token kw = this.Consume(TokenType.Return);

            AstTupleNode expr = null;
            if (!this.Match(TokenType.Semicolon))
                expr = new AstTupleNode(this.ExpressionList());

            this.Consume(TokenType.Semicolon);

            return new AstReturnNode(kw, expr);
        }

        // Rule:
        // continue_statement -> "continue" ";"
        private AstContinueNode ContinueStatement()
        {
            var cont = new AstContinueNode(this.Consume(TokenType.Continue));
            this.Consume(TokenType.Semicolon);
            return cont;
        }

        // Rule:
        // break_statement -> "break" INTEGER? ";"
        private AstBreakNode BreakStatement()
        {
            AstNode nbreaks = null;
            Token kw = this.Consume(TokenType.Break);
            if (this.Match(TokenType.Integer))
                nbreaks = new AstLiteralNode(this.Consume(TokenType.Integer));
            this.Consume(TokenType.Semicolon);
            return new AstBreakNode(kw, nbreaks);
        }

        // Rule:
        // parenthesized_expr -> "(" expression ")" ( statement | ";" )
        private (AstNode, AstNode) ParenthesizedStatement()
        {
            AstNode expression = this.ParenthesizedExpression();
            AstNode stmt = this.Match(TokenType.Semicolon) ? new AstNoOpNode(this.Consume(TokenType.Semicolon)) : this.Statement();
            return (expression, stmt);
        }

        private AstNode ParenthesizedExpression()
        {
            this.Consume(TokenType.LeftParen);
            AstNode expression = this.Expression();
            this.Consume(TokenType.RightParen);
            return expression;
        }

        // Rule:
        // braced_expr -> expression block
        private (AstNode, AstNode) BracedStatement()
        {
            AstNode expression = this.Expression();
            AstNode block = this.Block();
            return (expression, block);
        }

        // Rule:
        // while_statement -> "while" ( parenthesized_expr | braced_expr )
        private AstNode WhileStatement()
        {
            Token kw = this.Consume(TokenType.While);

            AstNode condition = null;
            AstNode body = null;
            (condition, body) = this.Match(TokenType.LeftParen) ? this.ParenthesizedStatement() : this.BracedStatement();
            return new AstWhileNode(kw, condition, body);
        }

        #region for_statement

        // Rule:
        // for_statement -> "for" "(" for_initializer? ";" expression? ";" for_iterator? ")" statement
        // 			      | "for" for_initializer? ";" expression? ";" for_iterator? block
        private AstForNode ForStatement()
        {
            if (this.Match(TokenType.For, TokenType.LeftParen))
                return this.ParenthesizedForStatemet();

            Token kw = this.Consume(TokenType.For);
            AstNode forInitializer = null;
            AstNode expression = null;
            AstNode forIterator = null;
            AstNode body = null;

            // Initializer
            if (this.Match(TokenType.Semicolon))
            {
                forInitializer = new AstNoOpNode(this.Consume());
            }
            else
            {
                forInitializer = this.ForInitializer();
                this.Consume(TokenType.Semicolon);
            }
            // Expression
            if (this.Match(TokenType.Semicolon))
            {
                expression = new AstNoOpNode(this.Consume());
            }
            else
            {
                expression = this.Expression();
                this.Consume(TokenType.Semicolon);
            }

            // Iterator
            if (this.Match(TokenType.LeftBrace))
            {
                forIterator = new AstNoOpNode(this.Peek()); // Get a reference of the line/col
            }
            else
            {
                forIterator = this.ForIterator();
            }

            // Body
            body = this.Block();
            return new AstForNode(kw, forInitializer, expression, forIterator, body);
        }

        // Rule: (continuation)
        // for_statement -> "for" "(" for_initializer? ";" expression? ";" for_iterator? ")" statement
        private AstForNode ParenthesizedForStatemet()
        {
            Token kw = this.Consume(TokenType.For);
            this.Consume(TokenType.LeftParen);

            AstNode forInitializer = null;
            AstNode expression = null;
            AstNode forIterator = null;
            AstNode body = null;

            // Initializer
            if (this.Match(TokenType.Semicolon))
            {
                forInitializer = new AstNoOpNode(this.Consume());
            }
            else
            {
                forInitializer = this.ForInitializer();
                this.Consume(TokenType.Semicolon);
            }

            // Expression
            if (this.Match(TokenType.Semicolon))
            {
                expression = new AstNoOpNode(this.Consume());
            }
            else
            {
                expression = this.Expression();
                this.Consume(TokenType.Semicolon);
            }

            // Iterator
            if (this.Match(TokenType.RightParen))
            {
                forIterator = new AstNoOpNode(this.Consume());
            }
            else
            {
                forIterator = this.ForIterator();
                this.Consume(TokenType.RightParen);
            }

            // Body
            body = this.Match(TokenType.Semicolon) ? new AstNoOpNode(this.Consume(TokenType.Semicolon)) : this.Statement();
            return new AstForNode(kw, forInitializer, expression, forIterator, body);
        }

        // Rule:
        // for_initializer -> for_declaration
        // 				    | expression_list
        private AstNode ForInitializer()
        {
            if (this.IsVarDeclaration())
            {
                return this.ForDeclaration();
            }
            return this.ExpressionList();
        }

        // Rule:
        // for_declaration -> ( implicit_var_declaration | typed_var_declaration )
        private AstDeclarationNode ForDeclaration()
        {
            AstNode variable = null;
            if (this.Match(TokenType.Variable))
            {
                variable = this.ImplicitVarDeclaration();
            }
            else
            {
                variable = this.TypedVarDeclaration();
            }
            return new AstDeclarationNode(new List<AstNode>() { variable });
        }

        // Rule:
        // for_iterator -> expression ( "," expression )*
        private AstDeclarationNode ForIterator()
        {
            List<AstNode> exprs = new List<AstNode> {
                this.Expression()
            };
            while (this.Match(TokenType.Comma))
            {
                this.Consume();
                exprs.Add(this.Expression());
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
            Token kw = this.Consume(TokenType.If);
            AstNode condition = null;
            AstNode thenbranch = null;
            AstNode elsebranch = null;

            (condition, thenbranch) = this.Match(TokenType.LeftParen) ? this.ParenthesizedStatement() : this.BracedStatement();

            // Parse the else branch if present
            if (this.Match(TokenType.Else))
            {
                this.Consume(TokenType.Else);
                elsebranch = this.Match(TokenType.Semicolon) ? new AstNoOpNode(this.Consume(TokenType.Semicolon)) : this.Statement();
            }
            return new AstIfNode(kw, condition, thenbranch, elsebranch);
        }

        // Rule:
        // block -> "{" declaration* "}"
        private AstBlockNode Block()
        {
            List<AstNode> statements = new List<AstNode>();
            this.Consume(TokenType.LeftBrace);
            while (!this.Match(TokenType.RightBrace))
            {
                statements.Add(this.Declaration());
            }
            this.Consume(TokenType.RightBrace);
            return new AstBlockNode(statements);
        }

        // Rule:
        // expression_statement	-> expression ";"
        private AstNode ExpressionStatement()
        {
            AstNode expr = this.Expression();
            this.Consume(TokenType.Semicolon);
            return expr;
        }

        // Rule:
        // expression_list -> expression ( ',' expression )*
        private AstExpressionListNode ExpressionList()
        {
            List<AstNode> exprs = new List<AstNode> {
                this.Expression()
            };
            while (this.Match(TokenType.Comma))
            {
                this.Consume();
                exprs.Add(this.Expression());
            }
            return new AstExpressionListNode(exprs);
        }

        // Rule:
        // expression -> expression_assignment
        private AstNode Expression()
        {
            return this.ExpressionAssignment();
        }

        // Rule:
        // lambda_expression -> lambda_params '=>' ( block | expression )
        private AstFuncDeclNode LambdaExpression()
        {
            AstParametersNode lambdaParams = this.LambdaParams();
            var arrow = this.Consume(TokenType.RightArrow);
            AstNode expr = this.Match(TokenType.LeftBrace) ? this.Block() : this.Expression();
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
                this.Consume();
            }

            // Consume the identifiers separated by commas
            while (this.Match(TokenType.Identifier))
            {
                parameters.Add(this.Consume(TokenType.Identifier));
                if (this.Match(TokenType.Comma))
                    this.Consume();
            }

            if (parenthesis)
                this.Consume(TokenType.RightParen);

            return new AstParametersNode(parameters);
        }

        // Rule:
        // member_access -> IDENTIFIER ( '.' IDENTIFIER | arguments )*
        private AstNode MemberAccess()
        {
            // Create an accessor node for the identifier
            AstNode accessor = new AstAccessorNode(this.Consume(TokenType.Identifier), null);

            // Try to find member accessors or callable members like:
            //  member.property
            //  member.property.property2
            //  member.property()
            //  member.property.property2()
            //  member.property.property2().property3
            while (true)
            {
                if (this.Match(TokenType.LeftParen))
                {
                    AstExpressionListNode arguments = this.Arguments();
                    accessor = new AstCallableNode(accessor, arguments);
                }
                else if (this.Match(TokenType.Dot))
                {
                    this.Consume();
                    accessor = new AstAccessorNode(this.Consume(TokenType.Identifier), accessor);
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

            if (this.Match(TokenType.LeftParen) && this.Consume(TokenType.LeftParen) != null)
                hasParenthesis = true;
            do
            {
                AstNode accessor = null;

                // Try to find member accessors or callable members
                //  member.property.property2
                if (this.Match(TokenType.Identifier))
                {
                    do
                    {
                        accessor = new AstAccessorNode(this.Consume(TokenType.Identifier), accessor);
                    } while (this.Match(TokenType.Dot) && this.Consume(TokenType.Dot) != null);
                }

                if (accessor == null)
                    accessor = new AstNoOpNode(null);

                exprs.Add(accessor);

            } while (this.Match(TokenType.Comma) && this.Consume(TokenType.Comma) != null);

            if (hasParenthesis)
                this.Consume(TokenType.RightParen);

            return new AstTupleNode(exprs);
        }

        // Rule:
        // tuple_initializer -> '(' expression_list? ')'
        private AstTupleNode TupleInitializer()
        {
            List<AstNode> args = new List<AstNode>();
            bool isValidTuple = false;

            this.Consume(TokenType.LeftParen);

            while (!this.Match(TokenType.RightParen))
            {
                args.Add(this.Expression());
                while (this.Match(TokenType.Comma))
                {
                    isValidTuple = true;
                    this.Consume();
                }
            }

            this.Consume(TokenType.RightParen);

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
            if (this.IsAssignment())
            {
                var checkpoint = this.SaveCheckpoint();

                AstNode lvalue = this.MemberAccess();

                // If we now match an assignment of any type, create an assignment node
                if (this.MatchAny(TokenType.Assignment, TokenType.IncrementAndAssign, TokenType.DecrementAndAssign, TokenType.DivideAndAssign, TokenType.MultAndAssign))
                {
                    Token assignmentop = this.Consume();
                    AstNode expression = this.Expression();

                    if (lvalue is AstCallableNode)
                        throw new ParserException($"{this.GetCurrentLineAndCol()} Left-hand side of an assignment must be a variable.");

                    return new AstVariableAssignmentNode(lvalue as AstAccessorNode, assignmentop, expression);
                }

                // If we are not seeing an assignment operator, then try again with ConditionalExpression
                this.RestoreCheckpoint(checkpoint);
                return this.ConditionalExpression();
            }

            // Try to parse a lambda expression
            if (this.MatchLambda())
                return this.LambdaExpression();

            if (this.IsDestructuring())
            {
                ParserCheckpoint checkpoint = this.SaveCheckpoint();
                // First try if it is a left-hand side expression
                try
                {
                    AstTupleNode lvalue = this.Destructuring();

                    // If we now match an assignment of any type, create an assignment node
                    if (this.MatchAny(TokenType.Assignment, TokenType.IncrementAndAssign, TokenType.DecrementAndAssign, TokenType.DivideAndAssign, TokenType.MultAndAssign))
                    {
                        Token assignmentop = this.Consume();
                        AstNode expression = this.Expression();
                        return new AstDestructuringAssignmentNode(lvalue, assignmentop, expression);
                    }
                }
                catch
                {
                }
                // If we reach this point, we restore the checkpoint
                this.RestoreCheckpoint(checkpoint);
            }

            // Finally try with a conditional expression
            return this.ConditionalExpression();
        }

        // Rule:
        // conditional_expression -> null_coalescing_expression ( '?' expression ':' expression )?
        private AstNode ConditionalExpression()
        {
            AstNode nullCoalescingExpr = this.NullCoalescingExpression();
            if (this.Match(TokenType.Question))
            {
                Token q = this.Consume();
                AstNode trueExpr = this.Expression();
                this.Consume(TokenType.Colon);
                AstNode falseExpr = this.Expression();
                return new AstIfNode(q, nullCoalescingExpr, trueExpr, falseExpr);
            }
            return nullCoalescingExpr;
        }

        // Rule:
        // null_coalescing_expression -> or_expression ( '??' null_coalescing_expression )?
        private AstNode NullCoalescingExpression()
        {
            AstNode orExpr = this.OrExpression();
            if (this.Match(TokenType.QuestionQuestion))
            {
                Token q = this.Consume();
                AstNode rightExpr = this.NullCoalescingExpression();
                return new AstNullCoalescingNode(q, orExpr, rightExpr);
            }
            return orExpr;
        }

        // Rule:
        // or_expression -> and_expression ( "||" and_expression )*
        private AstNode OrExpression()
        {
            AstNode orexpr = this.AndExpression();
            while (this.Match(TokenType.Or))
            {
                Token or = this.Consume();
                AstNode right = this.AndExpression();
                orexpr = new AstBinaryNode(or, orexpr, right);
            }
            return orexpr;
        }

        // Rule:
        // and_expression -> equality_expression ( "&&" equality_expression )*
        private AstNode AndExpression()
        {
            AstNode andexpr = this.EqualityExpression();
            while (this.Match(TokenType.And))
            {
                Token and = this.Consume();
                AstNode right = this.EqualityExpression();
                andexpr = new AstBinaryNode(and, andexpr, right);
            }
            return andexpr;
        }

        // Rule:
        // equality_expression -> comparison_expression ( ( "!=" | "==" ) comparison_expression )*
        private AstNode EqualityExpression()
        {
            AstNode compexpr = this.ComparisonExpression();
            while (this.Match(TokenType.Equal) || this.Match(TokenType.NotEqual))
            {
                Token equality = this.Consume();
                AstNode right = this.ComparisonExpression();
                compexpr = new AstBinaryNode(equality, compexpr, right);
            }
            return compexpr;
        }

        // Rule:
        // comparison_expression -> addition_expression(( ">" | ">=" | "<" | "<=" ) addition_expression )*
        private AstNode ComparisonExpression()
        {
            AstNode additionexpr = this.AdditionExpression();
            while (this.Match(TokenType.GreatThan)
                || this.Match(TokenType.GreatThanEqual)
                || this.Match(TokenType.LessThan)
                || this.Match(TokenType.LessThanEqual))
            {
                Token comp = this.Consume();
                AstNode right = this.AdditionExpression();
                additionexpr = new AstBinaryNode(comp, additionexpr, right);
            }
            return additionexpr;
        }

        // Rule:
        // addition_expression -> multiplication_expression(( "-" | "+" ) multiplication_expression )*
        private AstNode AdditionExpression()
        {
            AstNode multexpr = this.MultiplicationExpression();
            while (this.Match(TokenType.Minus) || this.Match(TokenType.Addition))
            {
                Token addition = this.Consume();
                AstNode right = this.MultiplicationExpression();
                multexpr = new AstBinaryNode(addition, multexpr, right);
            }
            return multexpr;
        }

        // Rule:
        // multiplication_expression -> unary_expression(( "/" | "*" ) unary_expression )*
        private AstNode MultiplicationExpression()
        {
            AstNode unaryexpr = this.UnaryExpression();
            while (this.Match(TokenType.Multiplication) || this.Match(TokenType.Division))
            {
                Token mult = this.Consume();
                AstNode right = this.UnaryExpression();
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
            if (this.Match(TokenType.Not) || this.Match(TokenType.Minus))
            {
                Token op = this.Consume();
                return new AstUnaryNode(op, this.UnaryExpression());
            }
            else if (this.Match(TokenType.Increment) || this.Match(TokenType.Decrement))
            {
                Token op = this.Consume();
                return new AstUnaryPrefixNode(op, this.PrimaryExpression());
            }
            var expr = this.PrimaryExpression();
            if (this.Match(TokenType.Increment) || this.Match(TokenType.Decrement))
                return new AstUnaryPostfixNode(this.Consume(), expr);
            return new AstUnaryNode(null, expr);
        }

        // Rule:
        // primary_expression -> primary ( "." IDENTIFIER | "(" arguments? ")" )*
        private AstNode PrimaryExpression()
        {
            Token newt = null;
            if (this.Match(TokenType.New))
            {
                newt = this.Consume();
            }
            AstNode primary = this.Primary();
            var checkpoint = this.SaveCheckpoint();
            while (true)
            {
                if (this.Match(TokenType.LeftParen))
                {
                    if (this.TryGetAccessor(primary) == null && this.TryGetLiteral(primary)?.Literal?.Type != TokenType.Identifier)
                        throw new ParserException($"{this.GetCurrentLineAndCol()} '{primary}' is not an invokable object");
                    AstExpressionListNode arguments = this.Arguments();
                    primary = new AstCallableNode(primary, arguments, newt);
                }
                else if (this.Match(TokenType.LeftBracket))
                {
                    primary = new AstIndexerNode(primary, this.Indexer());
                }
                else if (this.Match(TokenType.Dot))
                {
                    this.Consume();
                    primary = new AstAccessorNode(this.Consume(TokenType.Identifier), primary);
                }
                else
                {
                    if (this.pointer == checkpoint.Pointer
                        && (primary is AstLiteralNode && (primary as AstLiteralNode).Literal.Type != TokenType.Identifier)
                        && newt != null)
                        throw new ParserException($"{this.GetCurrentLineAndCol()} Type expected");
                    if (this.pointer == checkpoint.Pointer && (primary is AstAccessorNode) && newt != null)
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
            this.Consume(TokenType.LeftBracket);
            do
            {
                args.Add(this.Expression());
                while (this.Match(TokenType.Comma))
                {
                    this.Consume();
                    args.Add(this.Expression());
                }
            } while ((!this.Match(TokenType.RightBracket)));
            this.Consume(TokenType.RightBracket);
            return new AstExpressionListNode(args);
        }

        // Rule: (it is easier to check the expression_list manually here)
        // arguments -> '(' expression_list? ')'
        private AstExpressionListNode Arguments()
        {
            List<AstNode> args = new List<AstNode>();
            this.Consume(TokenType.LeftParen);
            if (!this.Match(TokenType.RightParen))
            {
                args.Add(this.Expression());
                while (this.Match(TokenType.Comma))
                {
                    this.Consume();
                    args.Add(this.Expression());
                }
            }
            this.Consume(TokenType.RightParen);
            return new AstExpressionListNode(args);
        }

        // Rule:
        // primary	-> "true" | "false" | "null" | INTEGER | DOUBLE | DECIMAL | STRING | IDENTIFIER	| tuple_initializer | "(" expression ")"
        private AstNode Primary()
        {
            if (this.Match(TokenType.LeftParen))
            {
                var checkpoint = this.SaveCheckpoint();
                // Now we can try if it is a literal tuple initializer
                try
                {
                    // This call will return null if it is not a valid tuple initializer
                    var node = this.TupleInitializer();

                    // If we match a dot, it could be a primary expression followed by a member access,
                    // it is not a literal tuple initializer
                    if (node != null && !this.MatchAny(TokenType.Dot))
                        return node;
                }
                catch
                {
                }

                // If we didn't match destructuring or tuple initializer, restore and try
                // a parenthesized expression
                this.RestoreCheckpoint(checkpoint);
                return this.ParenthesizedExpression();
            }

            bool isPrimary = this.Match(TokenType.Boolean)
                                || this.Match(TokenType.Null) 
                                || this.Match(TokenType.Char)
                                || this.Match(TokenType.Integer) 
                                || this.Match(TokenType.Float) 
                                || this.Match(TokenType.Double)
                                || this.Match(TokenType.Decimal) 
                                || this.Match(TokenType.String) 
                                || this.Match(TokenType.Identifier);

            if (!isPrimary)
                throw new ParserException($"{this.GetCurrentLineAndCol()} Expects primary but received {(this.HasInput() ? this.Peek().Type.ToString() : "end of input")}");

            return this.Match(TokenType.Identifier) ? (AstNode)new AstAccessorNode(this.Consume(), null) : new AstLiteralNode(this.Consume());
        }
        #endregion

        #region Parser lookahead helpers

        private bool IsVarDeclaration()
        {
            // 'var' identifier ( '=' expression )?
            if (this.Match(TokenType.Variable))
                return true;

            if (this.Match(TokenType.Identifier, TokenType.LeftParen, TokenType.Identifier))
            {
                // identifier (identifier, identifier, ..., identifier)
                if (this.MatchAnyFrom(3, TokenType.Identifier, TokenType.Comma))
                    return true;

                /*// identifier ( '[' ']' )+ (identifier, identifier, ..., identifier)
                int dimensions = CountRepeatedMatchesFrom(1, TokenType.LeftBracket, TokenType.RightBracket);
                return dimensions > 0 && MatchFrom(dimensions + 1, TokenType.Identifier);*/
            }

            if (this.Match(TokenType.Identifier))
            {
                // identifier identifier ( '=' expression )?
                if (this.MatchFrom(1, TokenType.Identifier))
                    return true;

                // identifier ( '[' ']' )+ identifier ( '=' expression )?
                int dimensions = this.CountRepeatedMatchesFrom(1, TokenType.LeftBracket, TokenType.RightBracket);
                return dimensions > 0 && this.MatchFrom(dimensions + 1, TokenType.Identifier);
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
            if (!this.Match(TokenType.LeftParen))
            {
                for (int i=0; i < this.tokens.Count; i++)
                {
                    var t = this.PeekFrom(i);
                    if (t == null || t.Type == TokenType.Assignment)
                        break;
                    if (t.Type == TokenType.Comma)
                        return true;
                }
            }

            // Check for parenthesized list of identifiers
            return (this.Match(TokenType.LeftParen, TokenType.Identifier) && this.MatchAnyFrom(2, TokenType.Dot, TokenType.Comma, TokenType.RightParen))
                || this.Match(TokenType.LeftParen, TokenType.Comma);
        }

        private bool IsAssignment()
        {
            return this.Match(TokenType.Identifier)
                && this.MatchAnyFrom(1, TokenType.Dot, TokenType.LeftParen, TokenType.Assignment, TokenType.IncrementAndAssign, TokenType.DecrementAndAssign, TokenType.DivideAndAssign, TokenType.MultAndAssign);
        }

        private bool MatchLambda()
        {
            // =>
            bool arrow = this.Match(TokenType.RightArrow);
            // a =>
            bool paramArrow = (!this.Match(TokenType.LeftParen) && this.Match(TokenType.Unknown, TokenType.RightArrow));
            // a,b =>
            bool paramListArrow = this.MatchFrom(this.CountRepeatedMatchesFrom(0, TokenType.Unknown, TokenType.Comma), TokenType.RightArrow);
            // () =>
            bool parentArrow = this.Match(TokenType.LeftParen, TokenType.RightParen, TokenType.RightArrow);
            // (a) =>
            bool parentParamArrow = (!this.MatchFrom(1, TokenType.LeftParen) && this.Match(TokenType.LeftParen, TokenType.Unknown, TokenType.RightParen, TokenType.RightArrow));
            // (a,b) =>
            bool parentParamListArrow = (this.Match(TokenType.LeftParen) && this.MatchFrom(1 + this.CountRepeatedMatchesFrom(1, TokenType.Unknown, TokenType.Comma), TokenType.RightParen, TokenType.RightArrow));

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
