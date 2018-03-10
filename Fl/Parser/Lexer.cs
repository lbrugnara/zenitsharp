// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using System;
using System.Collections.Generic;

namespace Fl.Parser
{
    public class Lexer
    {
        #region Private fields

        /// <summary>
        /// Input source
        /// </summary>
        private string source;
        
        /// <summary>
        /// Keep track of the current char of source that is being pointed
        /// </summary>
        private int pointer;
        
        /// <summary>
        /// Track lines numbers
        /// </summary>
        private int line;

        /// <summary>
        /// Track column numbers
        /// </summary>
        private int col;

        /// <summary>
        /// Set of Fl's reserved words
        /// </summary>
        private static readonly Dictionary<string, TokenType> keywords = new Dictionary<string, TokenType>()
        {
            { "null", TokenType.Null },
            { "true", TokenType.Boolean },
            { "false", TokenType.Boolean },
            { "var", TokenType.Variable },
            { "const", TokenType.Constant },
            { "if", TokenType.If },
            { "else", TokenType.Else },
            { "while", TokenType.While },
            { "for", TokenType.For },
            { "break", TokenType.Break },
            { "continue", TokenType.Continue },
            { "return", TokenType.Return },
            { "fn", TokenType.Function },
            { "namespace", TokenType.Namespace },
            { "new", TokenType.New }
        };

        #endregion

        #region Constructor

        public Lexer(string source)
        {
            this.source = source;
            this.pointer = 0;
            this.line = 1;
            this.col = 0;
        }

        #endregion

        #region Scanning helpers

        /// <summary>
        /// Set the lexer back to the initial state
        /// </summary>
        private void Reset()
        {
            this.pointer = 0;
            this.line = 1;
            this.col = 0;
        }

        private bool HasInput() => this.pointer < this.source.Length;

        /// <summary>
        /// Peek the next character if available, '\0' otherwise
        /// </summary>
        /// <returns></returns>
        private char Peek() => this.pointer < this.source.Length ? this.source[this.pointer] : '\0';

        /// <summary>
        /// Peek a number of characters from the current position
        /// </summary>
        /// <param name="count">Number of characters to peek</param>
        /// <returns></returns>
        private string Peek(int count)
        {
            if (count <= 0)
                throw new ArgumentException("Amount must be greater than 0");
            return this.pointer + count <= this.source.Length ? this.source.Substring(this.pointer, count) : null;
        }

        /// <summary>
        /// Peek a char applying a positive offset to the current position
        /// </summary>
        /// <param name="offset">Offset to apply to get the character</param>
        /// <returns></returns>
        private char Lookahead(int offset)
        {
            if (offset <= 0)
                throw new ArgumentException("Amount must be greater than 0");
            return this.pointer + offset <= this.source.Length ? this.source[this.pointer + offset] : '\0';
        }

        /// <summary>
        /// Consume the next character
        /// </summary>
        /// <returns></returns>
        private char Consume()
        {
            char c = this.pointer < this.source.Length ? this.source[this.pointer] : '\0';
            this.pointer++;
            this.col++;
            return c;
        }

        /// <summary>
        /// Consume a number of characters from the current position
        /// </summary>
        /// <param name="count">Number of characters to consume</param>
        /// <returns></returns>
        private string Consume(int count)
        {
            if (count <= 0)
                throw new ArgumentException("Amount must be greater than 0");
            string str = this.pointer + count < this.source.Length ? this.source.Substring(this.pointer, count) : null;
            this.pointer += count;
            this.col += count;
            return str;
        }

        #endregion

        /// <summary>
        /// Try to consume the next Token in the stream
        /// </summary>
        public Token NextToken()
        {
            char c = this.Peek();

            // Consume new line and update line and col number
            if (c == '\n')
            {
                this.Consume();
                this.line++;
                this.col = 0;
            }

            // Consume Whitespaces and Comments
            while (this.HasInput() && (char.IsWhiteSpace(Peek()) && this.Consume() != '\0' || this.RemoveComment()));

            if (!this.HasInput())
                return null;

            // Check tokens
            Token t = this.CheckPunctuation()
                    ?? this.CheckParen()
                    ?? this.CheckBrace()
                    ?? this.CheckBracket()
                    ?? this.CheckChar()
                    ?? this.CheckNumber()
                    ?? this.CheckLogicalOperator()
                    ?? this.CheckArithmeticOperators()
                    ?? this.CheckString()
                    ?? this.CheckIdentifier();

            if (t == null && this.HasInput())
                throw new LexerException($"Unrecognized symbol '{this.source[this.pointer]}'");
            return t;
        }

        private Token BuildToken(TokenType type, object value, int line, int col)
        {
            return new Token()
            {
                Type = type,
                Line = line,
                Col = col,
                Value = value
            };
        }

        private bool RemoveComment()
        {
            string s = this.Peek(2);
            char c = '\n';
            if (s == "//")
            {
                while (this.HasInput() && (c = this.Peek()) != '\n') this.Consume();
                this.Consume();
                return true;
            }
            else if (s == "/*")
            {
                while (this.HasInput() && (s = this.Peek(2)) != "*/") this.Consume();
                this.Consume(2);
                return true;
            }
            return false;
        }

        private Token CheckPunctuation()
        {
            char c = this.Peek();
            int line = this.line;
            int col = this.col;
            switch (c)
            {
                case ';':
                    return this.BuildToken(TokenType.Semicolon, this.Consume(), line, col);
                case ',':
                    return this.BuildToken(TokenType.Comma, this.Consume(), line, col);
                case '.':
                    return this.BuildToken(TokenType.Dot, this.Consume(), line, col);
                case '?':
                    if (this.Peek(2) == "??")
                        return this.BuildToken(TokenType.QuestionQuestion, this.Consume(2), line, col);
                    return this.BuildToken(TokenType.Question, this.Consume(), line, col);
                case ':':
                    return this.BuildToken(TokenType.Colon, this.Consume(), line, col);
            }
            return null;
        }

        private Token CheckParen()
        {
            char c = this.Peek();
            if (c != '(' && c != ')')
                return null;
            int line = this.line;
            int col = this.col;
            return this.BuildToken(c == '(' ? TokenType.LeftParen : TokenType.RightParen, this.Consume(), line, col);
        }

        private Token CheckBrace()
        {
            char c = this.Peek();
            if (c != '{' && c != '}')
                return null;
            int line = this.line;
            int col = this.col;
            return this.BuildToken(c == '{' ? TokenType.LeftBrace : TokenType.RightBrace, this.Consume(), line, col);
        }

        private Token CheckBracket()
        {
            char c = this.Peek();
            if (c != '[' && c != ']')
                return null;
            int line = this.line;
            int col = this.col;
            return this.BuildToken(c == '[' ? TokenType.LeftBracket : TokenType.RightBracket, this.Consume(), line, col);
        }

        private Token CheckNumber()
        {
            char c = this.Peek();
            if (!char.IsDigit(c))
                return null;

            int col = this.col;
            int line = this.line;
            TokenType type = TokenType.Integer;
            string val = this.Consume().ToString();
            while (this.HasInput())
            {
                c = this.Peek();
                if (char.IsDigit(c))
                {
                    val += this.Consume();
                }
                else if (c == '.' && char.IsDigit(this.Lookahead(1)))
                {
                    val += this.Consume();
                    type = TokenType.Double;
                }
                else if (c == 'F' || c == 'f')
                {
                    if (type != TokenType.Double)
                        throw new Exception($"Invalid character '{c}'");
                    // Do not add the F now, just Consume and let's try what happens
                    this.Consume();
                    type = TokenType.Float;
                }
                else if (c == 'M' ||c == 'm')
                {
                    if (type != TokenType.Double)
                        throw new Exception($"Invalid character '{c}'");
                    // Do not add the M now, just Consume and let's try what happens
                    this.Consume();
                    type = TokenType.Decimal;
                }
                else
                {
                    break;
                }
            }

            return this.BuildToken(type, val, line, col);
        }

        private Token CheckLogicalOperator()
        {
            int col = this.col;
            int line = this.line;
            TokenType? type = null;
            string val = null;
            
            char c = this.Peek();
            switch (c)
            {
                case '>':
                    type = TokenType.GreatThan;
                    val = this.Consume().ToString();
                    if (this.Peek() == '=')
                    {
                        type = TokenType.GreatThanEqual;
                        val += this.Consume();
                    }
                    break;
                case '<':
                    type = TokenType.LessThan;
                    val = this.Consume().ToString();
                    if (this.Peek() == '=')
                    {
                        type = TokenType.LessThanEqual;
                        val += this.Consume().ToString();
                    }
                    break;
                case '!':
                    type = TokenType.Not;
                    val = this.Consume().ToString();                    
                    if (this.Peek() == '=')
                    {
                        type = TokenType.NotEqual;
                        val += this.Consume();
                    }
                    break;
                case '=':
                    type = TokenType.Assignment;
                    val = this.Consume().ToString();
                    if (this.Peek() == '=')
                    {
                        type = TokenType.Equal;
                        val += this.Consume();
                    }
                    else if (this.Peek() == '>')
                    {
                        type = TokenType.RightArrow;
                        val += this.Consume();
                    }
                    break;
                case '&':
                    if (this.Peek(2) == "&&")
                    {
                        type = TokenType.And;
                        val = this.Consume(2);
                    }
                    break;
                case '|':
                    if (this.Peek(2) == "||")
                    {
                        type = TokenType.Or;
                        val = this.Consume(2);
                    }
                    break;
                default:
                    return null;
        }

            if (!type.HasValue)
                return null;

            return this.BuildToken(type.Value, val, line, col);
        }

        private Token CheckArithmeticOperators()
        {
            char c = this.Peek();

            int col = this.col;
            int line = this.line;
            string val = null;
            TokenType? type = null;
            switch (c)
            {
                case '+':
                    type = TokenType.Addition;
                    val = this.Consume().ToString();
                    if (this.Peek() == '+')
                    {
                        type = TokenType.Increment;
                        val += this.Consume().ToString();
                    }
                    else if (this.Peek() == '=')
                    {
                        type = TokenType.IncrementAndAssign;
                        val += this.Consume().ToString();
                    }
                    break;
                case '-':
                    type = TokenType.Minus;
                    val = this.Consume().ToString();
                    if (this.Peek() == '-')
                    {
                        type = TokenType.Decrement;
                        val += this.Consume().ToString();
                    }
                    else if (this.Peek() == '=')
                    {
                        type = TokenType.DecrementAndAssign;
                        val += this.Consume().ToString();
                    }
                    break;
                case '*':
                    type = TokenType.Multiplication;
                    val = this.Consume().ToString();
                    if (this.Peek() == '=')
                    {
                        type = TokenType.MultAndAssign;
                        val += this.Consume().ToString();
                    }
                    break;
                case '/':
                    type = TokenType.Division;
                    val = this.Consume().ToString();
                    if (this.Peek() == '=')
                    {
                        type = TokenType.DivideAndAssign;
                        val += this.Consume().ToString();
                    }
                    break;
                default:
                    return null;
            }
            return this.BuildToken(type.Value, val, line, col);
        }

        private Token CheckIdentifier()
        {
            char c = this.Peek();

            if (!char.IsLetter(c) && c != '_' && c != '$')
                return null;

            int line = this.line;
            int col = this.col;
            string val = this.Consume().ToString();
            while (this.HasInput())
            {
                c = this.Peek();
                if (!char.IsLetterOrDigit(c) && c != '_')
                    break;
                val += this.Consume();
            }

            return this.BuildToken(keywords.ContainsKey(val) ? keywords[val] : TokenType.Identifier, val, line, col);
        }

        private Token CheckChar()
        {
            char c = this.Peek();

            if (c != '\'')
                return null;

            int line = this.line;
            int col = this.col;
            this.Consume(); // First '
            string val = "";
            // Escaped char
            if (this.Peek() == '\\')
            {
                this.Consume(); // Consume the backslash
                var ec = this.Consume(); // Consume the escaped char
                switch (ec)
                {
                    case 'n':
                        val += '\n';
                        break;
                    case 't':
                        val += '\t';
                        break;
                    case '0':
                        val += '\0';
                        break;
                    default:
                        throw new LexerException($"Invalid escaped char \\{ec}");
                }
            }
            else
            {
                // Consume char
                val += this.Consume();
            }

            if (c != '\'')
                throw new LexerException("Bad char literal");

            this.Consume(); // Last '

            return this.BuildToken(TokenType.Char, val, line, col);
        }

        private Token CheckString()
        {
            char c = this.Peek();

            if (c != '"')
                return null;

            int line = this.line;
            int col = this.col;
            this.Consume(); // First "
            string val = "";
            c = '\0';
            while (this.HasInput())
            {
                c = this.Peek();
                if (c == '"')
                    break;
                val += this.Consume();
            }

            if (c != '"')
                throw new LexerException("Bad string literal");

            this.Consume(); // Last "

            return this.BuildToken(TokenType.String, val, line, col);
        }
    }
}
