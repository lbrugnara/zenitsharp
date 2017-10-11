// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

namespace Fl
{
    public enum TokenType
    {
        // Types
        Integer,
        Double,
        Decimal,
        Boolean,
        String,
        Identifier,
        Variable,
        Constant,
        Null,

        // Arithmetic Operators
        Addition,
        Minus,
        Multiplication,
        Division,

        // Assignment
        Assignment,

        // Equality
        Equal,
        NotEqual,

        // Comparison
        GreatThan,
        GreatThanEqual,
        LessThan,
        LessThanEqual,

        // Logical Operators
        Not,
        And,
        Or,

        // Conditional
        If,
        Else,

        // Loop
        While,
        For,

        // Scope
        Break,
        Continue,
        Return,

        LeftParen,
        RightParen,
        LeftBrace,
        RightBrace,
        Semicolon,
        Comma,
        Dot,
        Function,
        Namespace
    }
}
