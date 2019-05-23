// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

namespace Zenit.Syntax
{
    public enum TokenType
    {
        Unknown,

        // Types
        Char,
        Integer,
        Float,
        Double,
        Decimal,
        Boolean,
        String,
        Identifier,
        Variable,
        Mutable,
        Constant,

        // Arithmetic Operators
        Addition,
        Minus,
        Multiplication,
        Division,
        Increment,
        Decrement,

        // Assignment
        Assignment,
        IncrementAndAssign,
        DecrementAndAssign,
        MultAndAssign,
        DivideAndAssign,

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
        LeftBracket,
        RightBracket,
        
        // Punctuation
        Semicolon,
        Comma,
        Dot,
        RightArrow,
        Question,
        QuestionQuestion,
        Colon,
        At,

        // More keywords
        Class,
        Function,
        Package,
        New,

        // Access modifiers
        AccessModifier
    }
}
