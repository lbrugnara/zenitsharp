// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Fl.Ast;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace Fl.Syntax
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

        public Node Parse(List<Token> tokens)
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
        private DeclarationNode Program()
        {            
            List<Node> statements = new List<Node>();
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
            return new DeclarationNode(statements);
        }

        // Rule:
        // declaration	-> func_declaration
        //               | variable_declaration
        //	             | constant_declaration
        //	             | statement
        //
        private Node Declaration()
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
            else if (this.Match(TokenType.Class))
            {
                return this.ClassDeclaration();
            }
            return this.Statement();
        }

        // Rule:
        // variable_declaration -> 'mut'? ( implicit_var_declaration | typed_var_declaration ) ';'
        private VariableNode VarDeclaration()
        {
            VariableNode variable = null;
            if (this.Match(TokenType.Variable) || this.Match(TokenType.Mutable, TokenType.Variable))
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
        private VariableNode ImplicitVarDeclaration()
        {
            // Get the symbol information
            Token mutability = this.Match(TokenType.Mutable) ? this.Consume() : null;
            Token type = this.Consume(TokenType.Variable);

            SymbolInformation variableType = new SymbolInformation(type, mutability, null);

            // If there is a left parent present, it is a destructuring declaration
            if (this.IsDestructuring())
                return this.VarDestructuring(variableType);
            
            // If not it is a common var declaration
            Token identifier = this.Consume(TokenType.Identifier);
            this.Consume(TokenType.Assignment, "Implicitly typed variables must be initialized");
            Node expression = this.Expression();
            return new VariableDefinitionNode(variableType, new List<SymbolDefinition>() { new SymbolDefinition(identifier, expression) });
        }

        // Rule:
        // typed_var_declaration -> IDENTIFIER ( '[' ']' )* ( typed_var_definition | var_destructuring ) ';'
        private VariableNode TypedVarDeclaration()
        {
            // Get the symbol information
            Token mutability = this.Match(TokenType.Mutable) ? this.Consume() : null;
            Token type = this.Consume(TokenType.Identifier);

            SymbolInformation variableType = new SymbolInformation(type, mutability, null);

            // If it contains a left bracket, it is an array variable
            if (this.Match(TokenType.LeftBracket))
            {
                List<Token> dimensions = new List<Token>();
                while (this.Match(TokenType.LeftBracket))
                {
                    dimensions.Add(this.Consume(TokenType.LeftBracket));
                    dimensions.Add(this.Consume(TokenType.RightBracket));
                }
                variableType = new SymbolInformation(type, mutability, null, dimensions);
            }

            var state = this.SaveCheckpoint();

            try
            {
                // If there is a left parent present, it is a destructuring declaration
                if (this.IsDestructuring())
                    return this.VarDestructuring(variableType);
            }
            catch (Exception)
            {
                this.RestoreCheckpoint(state);
            }

            // If not, it is a simple typed var definition
            return new VariableDefinitionNode(variableType, this.TypedVarDefinition());
        }

        // Rule: 
        // typed_var_definition -> IDENTIFIER ( '=' expression )? ( ',' typed_var_definition )*
        private List<SymbolDefinition> TypedVarDefinition()
        {
            var vars = new List<SymbolDefinition>();
            do
            {
                // There could be multiple declarations and definitions, so consume the
                // identifier and then check if it is a definition or just a declaration
                var id = this.Consume(TokenType.Identifier);

                if (this.Match(TokenType.Assignment) && this.Consume(TokenType.Assignment) != null)
                    vars.Add(new SymbolDefinition(id, this.Expression()));
                else
                    vars.Add(new SymbolDefinition(id, null));

            } while (this.Match(TokenType.Comma) && this.Consume(TokenType.Comma) != null);

            return vars;
        }

        // Rule: 
        // var_destructuring -> '(' ( ',' | IDENTIFIER )+ ')' '=' expression
        private VariableDestructuringNode VarDestructuring(SymbolInformation varType)
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
            return new VariableDestructuringNode(varType, tokens, this.Expression());
        }

        // Rule:
        // constant_declaration -> 'const' IDENTIFIER? IDENTIFIER '=' expression ( ',' IDENTIFIER '=' expression )* ) ';'
        private ConstantNode ConstDeclaration()
        {
            // Consume the keyword
            this.Consume(TokenType.Constant);

            // Get the constant type if present
            Token type = this.Match(TokenType.Identifier, TokenType.Identifier) ? this.Consume(TokenType.Identifier) : null;

            // Consume multiple constants declarations and definitions
            List<SymbolDefinition> constdefs = new List<SymbolDefinition>();
            do
            {
                Token identifier = this.Consume(TokenType.Identifier);
                this.Consume(TokenType.Assignment, "A constant value needs to be defined when declared.");
                Node expression = this.Expression();

                constdefs.Add(new SymbolDefinition(identifier, expression));

            } while (this.Match(TokenType.Comma) && this.Consume(TokenType.Comma) != null);

            this.Consume(TokenType.Semicolon);

            return new ConstantNode(type, constdefs);
        }

        // Rule:
        // func_declaration -> 'fn' IDENTIFIER '(' func_params? ')' ( '{' declaration* '}' | '=>' expression )
        private FunctionNode FuncDeclaration()
        {
            this.Consume(TokenType.Function);

            Token name = this.Consume(TokenType.Identifier);

            List<ParameterNode> parameters = null;

            this.Consume(TokenType.LeftParen);
            parameters = this.FuncParameters();
            this.Consume(TokenType.RightParen);

            if (this.Peek().Type == TokenType.RightArrow)
            {
                // RightArrow followed by brace doesn't make sense here, expression is the only accepted node
                this.Consume(TokenType.RightArrow);

                var f = new FunctionNode(name, parameters ?? new List<ParameterNode>(), new List<Node>() { this.Expression() }, false, true);

                if (this.Match(TokenType.Semicolon))
                    this.Consume(TokenType.Semicolon);

                return f;
            }

            List<Node> decls = new List<Node>();
            this.Consume(TokenType.LeftBrace);
            while (!this.Match(TokenType.RightBrace))
            {
                decls.Add(this.Declaration());
            }
            this.Consume(TokenType.RightBrace);

            return new FunctionNode(name, parameters, decls, false, false);
        }

        // Rule:
        // func_params -> func_param_declaration ( ',' func_param_declaration )*
        private List<ParameterNode> FuncParameters()
        {
            var parameters = new List<ParameterNode>();
            while (this.MatchAny(TokenType.Mutable, TokenType.Identifier))
            {
                parameters.Add(this.FuncParameter());
                if (this.Match(TokenType.Comma))
                    this.Consume();
            }
            return parameters;
        }

        // Rule:
        // func_param_declaration -> 'mut'? IDENTIFIER? IDENTIFIER
        //private 
        private ParameterNode FuncParameter()
        {
            Token mutability = this.Match(TokenType.Mutable) ? this.Consume() : null;
            // The type is present if we find IDENTIFIER IDENTIFIER
            Token type = this.PeekFrom(1).Type != TokenType.Identifier ? null : this.Consume();
            Token name = this.Consume(TokenType.Identifier);

            return new ParameterNode(name, new SymbolInformation(type, mutability, null));
        }

        // Rule:
        // class_declaration -> 'class' IDENTIFIER '{' class_body? '}'
        //
        // class_body   -> class_property
        //	            | class_constant
        //	            | class_method
        //
        // access_modifier -> ( 'public' | 'protected' | 'private' )
        private ClassNode ClassDeclaration()
        {
            var classToken = this.Consume(TokenType.Class);
            var className = this.Consume(TokenType.Identifier);

            this.Consume(TokenType.LeftBrace);

            var properties = new List<ClassPropertyNode>();
            var constants = new List<ClassConstantNode>();
            var methods = new List<ClassMethodNode>();

            while (!this.Match(TokenType.RightBrace))
            {
                if (this.IsClassPropertyDeclaration())
                {
                    properties.Add(this.ClassProperty());
                }
                else if (this.IsClassConstantDeclaration())
                {
                    constants.Add(this.ClassConstant());
                }
                else if (this.IsClassMethodDeclaration())
                {
                    methods.Add(this.ClassMethod());
                }
                else throw new ParserException($"Unexpected '{this.Peek().Value}' in class declaration");
            }

            this.Consume(TokenType.RightBrace);

            return new ClassNode(className, properties, constants, methods);
        }

        // Rule: (TODO: getter and setter for class_property, because of that the class_field indirection)
        // class_property -> class_field
        //
        // class_field -> access_modifier? 'mut' IDENTIFIER ( '[' ']' )* IDENTIFIER ( '=' expression )? ';'
        private ClassPropertyNode ClassProperty()
        {
            // Get symbol information
            Token accessModifier = this.Match(TokenType.AccessModifier) ? this.Consume() : null;
            Token mutability = this.Match(TokenType.Mutable) ? this.Consume() : null;
            Token type = this.Consume(TokenType.Identifier);

            SymbolInformation variableType = new SymbolInformation(type, mutability, accessModifier);

            // If it contains a left bracket, it is an array variable
            if (this.Match(TokenType.LeftBracket))
            {
                List<Token> dimensions = new List<Token>();
                while (this.Match(TokenType.LeftBracket))
                {
                    dimensions.Add(this.Consume(TokenType.LeftBracket));
                    dimensions.Add(this.Consume(TokenType.RightBracket));
                }
                variableType = new SymbolInformation(type, mutability, accessModifier, dimensions);
            }

            var name = this.Consume(TokenType.Identifier);

            Node definition = null;

            if (this.Match(TokenType.Assignment) && this.Consume(TokenType.Assignment) != null)
                definition = this.Expression();

            this.Consume(TokenType.Semicolon);

            // If not, it is a simple typed var definition
            return new ClassPropertyNode(name, variableType, definition);
        }

        // Rule:
        // class_constant -> access_modifier? 'const' IDENTIFIER IDENTIFIER '=' expression ';'
        private ClassConstantNode ClassConstant()
        {
            // Check access modifier
            Token accessModifier = this.Match(TokenType.AccessModifier) ? this.Consume() : null;

            // Consume the keyword
            this.Consume(TokenType.Constant);

            // Get the constant type if present
            Token type = this.Consume(TokenType.Identifier);

            var modifiers = new SymbolInformation(type, null, accessModifier);

            Token identifier = this.Consume(TokenType.Identifier);
            this.Consume(TokenType.Assignment, "A constant value needs to be defined when declared.");
            Node expression = this.Expression();

            this.Consume(TokenType.Semicolon);

            return new ClassConstantNode(identifier, modifiers, expression);
        }

        // Rule:
        // class_method -> access_modifier? func_declaration
        private ClassMethodNode ClassMethod()
        {
            // Check access modifier
            Token accessModifier = this.Match(TokenType.AccessModifier) ? this.Consume() : null;
            Token type = this.Consume(TokenType.Function);

            var modifiers = new SymbolInformation(type, null, accessModifier);

            Token name = this.Consume(TokenType.Identifier);

            this.Consume(TokenType.LeftParen);

            List<ParameterNode> parameters = this.FuncParameters();

            this.Consume(TokenType.RightParen);

            if (this.Peek().Type == TokenType.RightArrow)
            {
                // RightArrow followed by brace doesn't make sense here, expression is the only accepted node
                this.Consume(TokenType.RightArrow);

                var f = new ClassMethodNode(name, modifiers, parameters, new List<Node>() { this.Expression() }, true);

                if (this.Match(TokenType.Semicolon))
                    this.Consume(TokenType.Semicolon);

                return f;
            }

            List<Node> decls = new List<Node>();
            this.Consume(TokenType.LeftBrace);
            while (!this.Match(TokenType.RightBrace))
            {
                decls.Add(this.Declaration());
            }
            this.Consume(TokenType.RightBrace);

            return new ClassMethodNode(name, modifiers, parameters, decls, false);
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
        private Node Statement()
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
        private ReturnNode ReturnStatement()
        {
            Token kw = this.Consume(TokenType.Return);

            TupleNode expr = null;
            if (!this.Match(TokenType.Semicolon))
                expr = new TupleNode(this.ExpressionList());

            this.Consume(TokenType.Semicolon);

            return new ReturnNode(kw, expr);
        }

        // Rule:
        // continue_statement -> "continue" ";"
        private ContinueNode ContinueStatement()
        {
            var cont = new ContinueNode(this.Consume(TokenType.Continue));
            this.Consume(TokenType.Semicolon);
            return cont;
        }

        // Rule:
        // break_statement -> "break" INTEGER? ";"
        private BreakNode BreakStatement()
        {
            Node nbreaks = null;
            Token kw = this.Consume(TokenType.Break);
            if (this.Match(TokenType.Integer))
                nbreaks = new LiteralNode(this.Consume(TokenType.Integer));
            this.Consume(TokenType.Semicolon);
            return new BreakNode(kw, nbreaks);
        }

        // Rule:
        // parenthesized_expr -> "(" expression ")" ( statement | ";" )
        private (Node, Node) ParenthesizedStatement()
        {
            Node expression = this.ParenthesizedExpression();
            Node stmt = this.Match(TokenType.Semicolon) ? new NoOpNode(this.Consume(TokenType.Semicolon)) : this.Statement();
            return (expression, stmt);
        }

        private Node ParenthesizedExpression()
        {
            this.Consume(TokenType.LeftParen);
            Node expression = this.Expression();
            this.Consume(TokenType.RightParen);
            return expression;
        }

        // Rule:
        // braced_expr -> expression block
        private (Node, Node) BracedStatement()
        {
            Node expression = this.Expression();
            Node block = this.Block();
            return (expression, block);
        }

        // Rule:
        // while_statement -> "while" ( parenthesized_expr | braced_expr )
        private Node WhileStatement()
        {
            Token kw = this.Consume(TokenType.While);

            Node condition = null;
            Node body = null;
            (condition, body) = this.Match(TokenType.LeftParen) ? this.ParenthesizedStatement() : this.BracedStatement();
            return new WhileNode(kw, condition, body);
        }

        #region for_statement

        // Rule:
        // for_statement -> "for" "(" for_initializer? ";" expression? ";" for_iterator? ")" statement
        // 			      | "for" for_initializer? ";" expression? ";" for_iterator? block
        private ForNode ForStatement()
        {
            if (this.Match(TokenType.For, TokenType.LeftParen))
                return this.ParenthesizedForStatemet();

            Token kw = this.Consume(TokenType.For);
            Node forInitializer = null;
            Node expression = null;
            Node forIterator = null;
            Node body = null;

            // Initializer
            if (this.Match(TokenType.Semicolon))
            {
                forInitializer = new NoOpNode(this.Consume());
            }
            else
            {
                forInitializer = this.ForInitializer();
                this.Consume(TokenType.Semicolon);
            }
            // Expression
            if (this.Match(TokenType.Semicolon))
            {
                expression = new NoOpNode(this.Consume());
            }
            else
            {
                expression = this.Expression();
                this.Consume(TokenType.Semicolon);
            }

            // Iterator
            if (this.Match(TokenType.LeftBrace))
            {
                forIterator = new NoOpNode(this.Peek()); // Get a reference of the line/col
            }
            else
            {
                forIterator = this.ForIterator();
            }

            // Body
            body = this.Block();
            return new ForNode(kw, forInitializer, expression, forIterator, body);
        }

        // Rule: (continuation)
        // for_statement -> "for" "(" for_initializer? ";" expression? ";" for_iterator? ")" statement
        private ForNode ParenthesizedForStatemet()
        {
            Token kw = this.Consume(TokenType.For);
            this.Consume(TokenType.LeftParen);

            Node forInitializer = null;
            Node expression = null;
            Node forIterator = null;
            Node body = null;

            // Initializer
            if (this.Match(TokenType.Semicolon))
            {
                forInitializer = new NoOpNode(this.Consume());
            }
            else
            {
                forInitializer = this.ForInitializer();
                this.Consume(TokenType.Semicolon);
            }

            // Expression
            if (this.Match(TokenType.Semicolon))
            {
                expression = new NoOpNode(this.Consume());
            }
            else
            {
                expression = this.Expression();
                this.Consume(TokenType.Semicolon);
            }

            // Iterator
            if (this.Match(TokenType.RightParen))
            {
                forIterator = new NoOpNode(this.Consume());
            }
            else
            {
                forIterator = this.ForIterator();
                this.Consume(TokenType.RightParen);
            }

            // Body
            body = this.Match(TokenType.Semicolon) ? new NoOpNode(this.Consume(TokenType.Semicolon)) : this.Statement();
            return new ForNode(kw, forInitializer, expression, forIterator, body);
        }

        // Rule:
        // for_initializer -> for_declaration
        // 				    | expression_list
        private Node ForInitializer()
        {
            if (this.IsVarDeclaration())
            {
                return this.ForDeclaration();
            }
            return this.ExpressionList();
        }

        // Rule:
        // for_declaration -> ( implicit_var_declaration | typed_var_declaration )
        private DeclarationNode ForDeclaration()
        {
            Node variable = null;
            if (this.Match(TokenType.Variable))
            {
                variable = this.ImplicitVarDeclaration();
            }
            else
            {
                variable = this.TypedVarDeclaration();
            }
            return new DeclarationNode(new List<Node>() { variable });
        }

        // Rule:
        // for_iterator -> expression ( "," expression )*
        private DeclarationNode ForIterator()
        {
            List<Node> exprs = new List<Node> {
                this.Expression()
            };
            while (this.Match(TokenType.Comma))
            {
                this.Consume();
                exprs.Add(this.Expression());
            }
            // TODO: Check if DeclarationNode is correct
            return new DeclarationNode(exprs);
        }
        #endregion

        // Rule:
        // if_statement -> "if" (parenthesized_expr | braced_expr ) ( "else" (statement | ";" ) )?
        // parenthesized_expr -> "(" expression ")" ( statement | ";" )
        // braced_expr -> expression block
        private IfNode IfStatement()
        {
            Token kw = this.Consume(TokenType.If);
            Node condition = null;
            Node thenbranch = null;
            Node elsebranch = null;

            (condition, thenbranch) = this.Match(TokenType.LeftParen) ? this.ParenthesizedStatement() : this.BracedStatement();

            // Parse the else branch if present
            if (this.Match(TokenType.Else))
            {
                this.Consume(TokenType.Else);
                elsebranch = this.Match(TokenType.Semicolon) ? new NoOpNode(this.Consume(TokenType.Semicolon)) : this.Statement();
            }
            return new IfNode(kw, condition, thenbranch, elsebranch);
        }

        // Rule:
        // block -> "{" declaration* "}"
        private BlockNode Block()
        {
            List<Node> statements = new List<Node>();
            this.Consume(TokenType.LeftBrace);
            while (!this.Match(TokenType.RightBrace))
            {
                statements.Add(this.Declaration());
            }
            this.Consume(TokenType.RightBrace);
            return new BlockNode(statements);
        }

        // Rule:
        // expression_statement	-> expression ";"
        private Node ExpressionStatement()
        {
            Node expr = this.Expression();
            this.Consume(TokenType.Semicolon);
            return expr;
        }

        // Rule:
        // expression_list -> expression ( ',' expression )*
        private ExpressionListNode ExpressionList()
        {
            List<Node> exprs = new List<Node> {
                this.Expression()
            };
            while (this.Match(TokenType.Comma))
            {
                this.Consume();
                exprs.Add(this.Expression());
            }
            return new ExpressionListNode(exprs);
        }

        // Rule:
        // expression -> expression_assignment
        private Node Expression()
        {
            return this.ExpressionAssignment();
        }

        // Rule:
        // object_expression -> '{' object_property ( ',' object_property )* ','? '}'
        private ObjectNode ObjectExpression()
        {
            var properties = new List<ObjectPropertyNode>();

            this.Consume(TokenType.LeftBrace);

            while (this.Match(TokenType.Identifier) || this.Match(TokenType.Mutable, TokenType.Identifier))
            {
                // Create a dummy token for the var token
                var curTok = this.Peek();
                var varToken = new Token
                {
                    Col = curTok.Col-1,
                    Line = curTok.Line,
                    Type = TokenType.Variable,
                    Value = "var"
                };

                var info = new SymbolInformation(varToken, this.Match(TokenType.Mutable) ? this.Consume(TokenType.Mutable) : null, null);
                var name = this.Consume(TokenType.Identifier);

                this.Consume(TokenType.Colon);

                var value = this.Expression();

                var property = new ObjectPropertyNode
                {
                    Name = name,
                    Information = info,
                    Value = value
                };

                properties.Add(property);

                // Consume trailing comma
                if (this.Match(TokenType.Comma))
                    this.Consume(TokenType.Comma);
            }

            this.Consume(TokenType.RightBrace);

            return new ObjectNode(properties);
        }

        // Rule:
        // lambda_expression -> lambda_params '=>' ( block | expression )
        private FunctionNode LambdaExpression()
        {
            List<ParameterNode> lambdaParams = this.LambdaParams();
            var arrow = this.Consume(TokenType.RightArrow);
            var isBlock = this.Match(TokenType.LeftBrace);
            Node expr = isBlock ? this.Block() : this.Expression();
            return new FunctionNode(arrow, lambdaParams, new List<Node>() { expr }, true, !isBlock);
        }

        // Rule:
        // lambda_params -> '(' func_params ')' | func_params
        private List<ParameterNode> LambdaParams()
        {
            var parameters = new List<ParameterNode>();

            // Lambda params could be wrapped between parenthesis
            bool parenthesis = false;

            if (Match(TokenType.LeftParen))
            {
                parenthesis = true;
                this.Consume();
            }

            // Consume the identifiers separated by commas
            while (this.MatchAny(TokenType.Mutable, TokenType.Identifier))
            {
                parameters.Add(this.FuncParameter());
                if (this.Match(TokenType.Comma))
                    this.Consume();
            }

            if (parenthesis)
                this.Consume(TokenType.RightParen);

            return parameters;
        }

        // Rule:
        // member_access -> IDENTIFIER ( '.' IDENTIFIER | arguments )*
        private Node MemberAccess()
        {
            // Create an accessor node for the identifier
            Node accessor = new AccessorNode(this.Consume(TokenType.Identifier), null);

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
                    ExpressionListNode arguments = this.Arguments();
                    accessor = new CallableNode(accessor, arguments);
                }
                else if (this.Match(TokenType.Dot))
                {
                    this.Consume();
                    accessor = new AccessorNode(this.Consume(TokenType.Identifier), accessor, this.Match(TokenType.LeftParen));
                }
                else break;
            }
            return accessor;
        }

        // Rule:
        // destructuring -> '(' IDENTIFIER ( '.' IDENTIFIER )* ( ',' destructuring )* ')'
        private TupleNode Destructuring()
        {
            List<Node> exprs = new List<Node>();

            bool hasParenthesis = false;

            if (this.Match(TokenType.LeftParen) && this.Consume(TokenType.LeftParen) != null)
                hasParenthesis = true;
            do
            {
                Node accessor = null;

                // Try to find member accessors or callable members
                //  member.property.property2
                if (this.Match(TokenType.Identifier))
                {
                    do
                    {
                        accessor = new AccessorNode(this.Consume(TokenType.Identifier), accessor);
                    } while (this.Match(TokenType.Dot) && this.Consume(TokenType.Dot) != null);
                }

                if (accessor == null)
                    accessor = new NoOpNode(null);

                exprs.Add(accessor);

            } while (this.Match(TokenType.Comma) && this.Consume(TokenType.Comma) != null);

            if (hasParenthesis)
                this.Consume(TokenType.RightParen);

            return new TupleNode(exprs);
        }

        // Rule:
        // tuple_initializer -> '(' expression_list? ')'
        private TupleNode TupleInitializer()
        {
            List<Node> args = new List<Node>();
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

            return isValidTuple ? new TupleNode(args) : null;
        }

        // Rule:
        // expression_assignment	-> ( destructuring | member_access ) ( ( '=' | '+=' | '-=' | '/=' | '*=' )  expression_assignment )?
        // 						     | lambda_expression
        // 						     | conditional_expression
        private Node ExpressionAssignment()
        {
            // Check for:
            //  identifier ( = | += | -= | *= | /= )
            //  identifier.
            //  identifier(
            if (this.IsAssignment())
            {
                var checkpoint = this.SaveCheckpoint();

                Node lvalue = this.MemberAccess();

                // If we now match an assignment of any type, create an assignment node
                if (this.MatchAny(TokenType.Assignment, TokenType.IncrementAndAssign, TokenType.DecrementAndAssign, TokenType.DivideAndAssign, TokenType.MultAndAssign))
                {
                    Token assignmentop = this.Consume();
                    Node expression = this.Expression();

                    if (lvalue is CallableNode)
                        throw new ParserException($"{this.GetCurrentLineAndCol()} Left-hand side of an assignment must be a variable.");

                    return new VariableAssignmentNode(lvalue as AccessorNode, assignmentop, expression);
                }

                // If we are not seeing an assignment operator, then try again with ConditionalExpression
                this.RestoreCheckpoint(checkpoint);
                return this.ConditionalExpression();
            }

            // Try to parse a lambda expression
            if (this.IsLambdaExpression())
                return this.LambdaExpression();

            if (this.IsDestructuring())
            {
                ParserCheckpoint checkpoint = this.SaveCheckpoint();
                // First try if it is a left-hand side expression
                try
                {
                    TupleNode lvalue = this.Destructuring();

                    // If we now match an assignment of any type, create an assignment node
                    if (this.MatchAny(TokenType.Assignment, TokenType.IncrementAndAssign, TokenType.DecrementAndAssign, TokenType.DivideAndAssign, TokenType.MultAndAssign))
                    {
                        Token assignmentop = this.Consume();
                        Node expression = this.TupleInitializer();
                        return new DestructuringAssignmentNode(lvalue, assignmentop, expression as TupleNode);
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
        private Node ConditionalExpression()
        {
            Node nullCoalescingExpr = this.NullCoalescingExpression();
            if (this.Match(TokenType.Question))
            {
                Token q = this.Consume();
                Node trueExpr = this.Expression();
                this.Consume(TokenType.Colon);
                Node falseExpr = this.Expression();
                return new IfNode(q, nullCoalescingExpr, trueExpr, falseExpr);
            }
            return nullCoalescingExpr;
        }

        // Rule:
        // null_coalescing_expression -> or_expression ( '??' null_coalescing_expression )?
        private Node NullCoalescingExpression()
        {
            Node orExpr = this.OrExpression();
            if (this.Match(TokenType.QuestionQuestion))
            {
                Token q = this.Consume();
                Node rightExpr = this.NullCoalescingExpression();
                return new NullCoalescingNode(q, orExpr, rightExpr);
            }
            return orExpr;
        }

        // Rule:
        // or_expression -> and_expression ( "||" and_expression )*
        private Node OrExpression()
        {
            Node orexpr = this.AndExpression();
            while (this.Match(TokenType.Or))
            {
                Token or = this.Consume();
                Node right = this.AndExpression();
                orexpr = new BinaryNode(or, orexpr, right);
            }
            return orexpr;
        }

        // Rule:
        // and_expression -> equality_expression ( "&&" equality_expression )*
        private Node AndExpression()
        {
            Node andexpr = this.EqualityExpression();
            while (this.Match(TokenType.And))
            {
                Token and = this.Consume();
                Node right = this.EqualityExpression();
                andexpr = new BinaryNode(and, andexpr, right);
            }
            return andexpr;
        }

        // Rule:
        // equality_expression -> comparison_expression ( ( "!=" | "==" ) comparison_expression )*
        private Node EqualityExpression()
        {
            Node compexpr = this.ComparisonExpression();
            while (this.Match(TokenType.Equal) || this.Match(TokenType.NotEqual))
            {
                Token equality = this.Consume();
                Node right = this.ComparisonExpression();
                compexpr = new BinaryNode(equality, compexpr, right);
            }
            return compexpr;
        }

        // Rule:
        // comparison_expression -> addition_expression(( ">" | ">=" | "<" | "<=" ) addition_expression )*
        private Node ComparisonExpression()
        {
            Node additionexpr = this.AdditionExpression();
            while (this.Match(TokenType.GreatThan)
                || this.Match(TokenType.GreatThanEqual)
                || this.Match(TokenType.LessThan)
                || this.Match(TokenType.LessThanEqual))
            {
                Token comp = this.Consume();
                Node right = this.AdditionExpression();
                additionexpr = new BinaryNode(comp, additionexpr, right);
            }
            return additionexpr;
        }

        // Rule:
        // addition_expression -> multiplication_expression(( "-" | "+" ) multiplication_expression )*
        private Node AdditionExpression()
        {
            Node multexpr = this.MultiplicationExpression();
            while (this.Match(TokenType.Minus) || this.Match(TokenType.Addition))
            {
                Token addition = this.Consume();
                Node right = this.MultiplicationExpression();
                multexpr = new BinaryNode(addition, multexpr, right);
            }
            return multexpr;
        }

        // Rule:
        // multiplication_expression -> unary_expression(( "/" | "*" ) unary_expression )*
        private Node MultiplicationExpression()
        {
            Node unaryexpr = this.UnaryExpression();
            while (this.Match(TokenType.Multiplication) || this.Match(TokenType.Division))
            {
                Token mult = this.Consume();
                Node right = this.UnaryExpression();
                unaryexpr = new BinaryNode(mult, unaryexpr, right);
            }
            return unaryexpr;
        }

        // Rule:
        // unary_expression	-> ( "!" | "-" ) unary_expression
        // 					| ( "++" | "--" ) primary_expression
        // 					| primary_expression ( "++" | "--" )?
        private UnaryNode UnaryExpression()
        {
            if (this.Match(TokenType.Not) || this.Match(TokenType.Minus))
            {
                Token op = this.Consume();
                return new UnaryNode(op, this.UnaryExpression());
            }
            else if (this.Match(TokenType.Increment) || this.Match(TokenType.Decrement))
            {
                Token op = this.Consume();
                return new UnaryPrefixNode(op, this.PrimaryExpression());
            }
            var expr = this.PrimaryExpression();
            if (this.Match(TokenType.Increment) || this.Match(TokenType.Decrement))
                return new UnaryPostfixNode(this.Consume(), expr);
            return new UnaryNode(null, expr);
        }

        // Rule:
        // primary_expression -> primary ( "." IDENTIFIER | "(" arguments? ")" )*
        private Node PrimaryExpression()
        {
            if (this.IsObjectExpression())
                return this.ObjectExpression();

            Token newt = null;
            if (this.Match(TokenType.New))
            {
                newt = this.Consume();
            }
            Node primary = this.Primary();
            var checkpoint = this.SaveCheckpoint();
            while (true)
            {
                if (this.Match(TokenType.LeftParen))
                {
                    if (this.TryGetAccessor(primary) == null && this.TryGetLiteral(primary)?.Literal?.Type != TokenType.Identifier)
                        throw new ParserException($"{this.GetCurrentLineAndCol()} '{primary}' is not an invokable object");
                    ExpressionListNode arguments = this.Arguments();
                    primary = new CallableNode(primary, arguments, newt);
                }
                else if (this.Match(TokenType.LeftBracket))
                {
                    primary = new IndexerNode(primary, this.Indexer());
                }
                else if (this.Match(TokenType.Dot))
                {
                    this.Consume();
                    primary = new AccessorNode(this.Consume(TokenType.Identifier), primary, this.Match(TokenType.LeftParen));
                }
                else
                {
                    if (this.pointer == checkpoint.Pointer
                        && (primary is LiteralNode && (primary as LiteralNode).Literal.Type != TokenType.Identifier)
                        && newt != null)
                        throw new ParserException($"{this.GetCurrentLineAndCol()} Type expected");
                    if (this.pointer == checkpoint.Pointer && (primary is AccessorNode) && newt != null)
                        primary = new CallableNode(primary, new ExpressionListNode(new List<Node>()), newt);
                    break;
                }

            }
            return primary;
        }

        // Rule: (it is easier to check the expression_list manually here)
        // indexer -> '[' expression_list ']'
        private ExpressionListNode Indexer()
        {
            List<Node> args = new List<Node>();
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
            return new ExpressionListNode(args);
        }

        // Rule: (it is easier to check the expression_list manually here)
        // arguments -> '(' expression_list? ')'
        private ExpressionListNode Arguments()
        {
            List<Node> args = new List<Node>();
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
            return new ExpressionListNode(args);
        }

        // Rule:
        // primary	-> "true" | "false" | "null" | INTEGER | DOUBLE | DECIMAL | STRING | IDENTIFIER	| tuple_initializer | "(" expression ")"
        private Node Primary()
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
                                || this.Match(TokenType.Char)
                                || this.Match(TokenType.Integer) 
                                || this.Match(TokenType.Float) 
                                || this.Match(TokenType.Double)
                                || this.Match(TokenType.Decimal) 
                                || this.Match(TokenType.String) 
                                || this.Match(TokenType.Identifier);

            if (!isPrimary)
                throw new ParserException($"{this.GetCurrentLineAndCol()} Expects primary but received {(this.HasInput() ? this.Peek().Type.ToString() : "end of input")}");

            return this.Match(TokenType.Identifier) ? (Node)new AccessorNode(this.Consume(), null) : new LiteralNode(this.Consume());
        }
        #endregion

        #region Parser lookahead helpers

        private bool IsClassPropertyDeclaration()
        {
            int offset = 0;

            if (this.Match(TokenType.AccessModifier))
                offset++;

            if (this.MatchFrom(offset ,TokenType.Mutable))
                offset++;

            if (this.MatchFrom(offset, TokenType.Identifier))
            {
                // identifier identifier ( '=' expression )?
                if (this.MatchFrom(offset+1, TokenType.Identifier))
                    return true;

                // identifier ( '[' ']' )+ identifier ( '=' expression )?
                int dimensions = this.CountRepeatedMatchesFrom(offset + 1, TokenType.LeftBracket, TokenType.RightBracket);
                return dimensions > 0 && this.MatchFrom(dimensions + offset + 1, TokenType.Identifier);
            }

            return false;
        }

        private bool IsClassConstantDeclaration()
        {
            int offset = 0;

            if (this.Match(TokenType.AccessModifier))
                offset++;

            return this.MatchFrom(offset, TokenType.Constant);
        }

        private bool IsClassMethodDeclaration()
        {
            int offset = 0;

            if (this.Match(TokenType.AccessModifier))
                offset++;

            return this.MatchFrom(offset, TokenType.Function);
        }

        private bool IsVarDeclaration()
        {
            var offset = this.Match(TokenType.Mutable) ? 1 : 0;

            // 'var' identifier ( '=' expression )?
            if (this.MatchFrom(offset, TokenType.Variable))
                return true;

            if (this.MatchFrom(offset, TokenType.Identifier, TokenType.LeftParen, TokenType.Identifier))
            {
                // identifier (identifier, identifier, ..., identifier) '='
                int decls = this.CountRepeatedMatchesFrom(offset + 2, TokenType.Identifier, TokenType.Comma);
                if (this.MatchFrom(offset + decls + 1, TokenType.Identifier, TokenType.RightParen, TokenType.Assignment))
                    return true;

                /*// identifier ( '[' ']' )+ (identifier, identifier, ..., identifier)
                int dimensions = CountRepeatedMatchesFrom(1, TokenType.LeftBracket, TokenType.RightBracket);
                return dimensions > 0 && MatchFrom(dimensions + 1, TokenType.Identifier);*/
            }

            if (this.MatchFrom(offset, TokenType.Identifier))
            {
                // identifier identifier ( '=' expression )?
                if (this.MatchFrom(offset + 1, TokenType.Identifier) 
                    && (this.MatchAnyFrom(offset + 2, TokenType.Assignment, TokenType.Semicolon) || this.MatchAnyFrom(offset + 2, TokenType.Comma, TokenType.Identifier)))
                    return true;

                // identifier ( '[' ']' )+ identifier ( '=' expression )?
                int dimensions = this.CountRepeatedMatchesFrom(offset + 1, TokenType.LeftBracket, TokenType.RightBracket);
                return dimensions > 0 && this.MatchFrom(offset + dimensions + 1, TokenType.Identifier);
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
                    if (t == null || t.Type == TokenType.Assignment || t.Type == TokenType.Semicolon)
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

        private bool IsObjectExpression()
        {
            return this.Match(TokenType.LeftBrace) 
                && (this.MatchFrom(1, TokenType.RightBrace) || this.MatchFrom(1, TokenType.Mutable, TokenType.Identifier, TokenType.Colon)
                    || this.MatchFrom(1, TokenType.Identifier, TokenType.Colon));
        }

        private bool IsLambdaExpression()
        {
            if (this.Match(TokenType.RightArrow))
                return true;

            bool needsParent = this.Match(TokenType.LeftParen);
            int offset = needsParent ? 1 : 0;

            while (this.MatchAnyFrom(offset, TokenType.Mutable, TokenType.Identifier, TokenType.Comma))
                offset++;

            if (needsParent && this.MatchFrom(offset, TokenType.RightParen))
                offset++;

            return this.MatchFrom(offset, TokenType.RightArrow);

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

        private AccessorNode TryGetAccessor(Node primary)
        {
            Node tmp = primary;
            while (tmp != null)
            {
                if (tmp is AccessorNode)
                    return tmp as AccessorNode;

                if (tmp is CallableNode)
                {
                    tmp = (tmp as CallableNode).Target;
                    continue;
                }
            }
            return null;
        }

        private LiteralNode TryGetLiteral(Node primary)
        {
            Node tmp = primary;
            while (tmp != null)
            {
                if (tmp is LiteralNode)
                    return tmp as LiteralNode;

                if (tmp is AccessorNode)
                {
                    tmp = (tmp as AccessorNode).Parent;
                    continue;
                }
                else if (tmp is CallableNode)
                {
                    tmp = (tmp as CallableNode).Target;
                    continue;
                }
            }
            return null;
        }

        #endregion
    }
}
