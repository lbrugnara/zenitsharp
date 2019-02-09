# Zenit

Zenit is an experimental project to build a high-level programming language with the following characteristics:

- Strongly typed, with full type inference support
- Immutable by default
- Multi-paradigm
- Lexical scoped
- Structural and nominal typing mixed through interfaces

## Development

Zenit is in a very very early stage, the current focus is on the syntactic and semantic analysis, both in a very volatile development process.

#### Syntactic Analysis

- *Lexer*: Handcrafted lexer, nothing special
- *Parser*: A recursive descendant parser implementation for the [Zenit grammar](https://github.com/lbrugnara/zenit/blob/master/FrontEnd/Docs/Grammar.txt). The grammar needs improvement because now for certain constructions, it heavily relies on lookahead and backtracking.

#### Semantic Analysis

The semantic analysis so far comprehends 4 passes:

- *Symbol resolution and binding*: This pass ensures all the symbols referenced in the sources are defined and ensures completeness of the different data types.
- *Type inference*: This pass checks the different unannotated expressions to get the different data types involved on them.
- *Type checker*: The pass that ensures all the operations between types are correct.
- *Mutability checker*: An initial implementation that guarantees that basic constructions do not affect the contract established at the variable definition stage.

### Next steps

The goal is to get the type inference algorithm working as expected along with the implementation of the first pass to check mutability. After that next step would be to generate an intermediate representation that would allow to improve the mutability analysis and start working on optimizations.

## How it tastes?

Zenit's syntax is based on the C family syntax, taking features from languages like C# or JavaScript, and pretends to be easy to write and read: No unnecessary type annotations, common C-like keywords, lambda functions, anonymous objects, and tuple literals:

```
// Lambda expressions
var mult1 = (a, b) => a * b;

// Parenthesis are not required
var mult2 = a, b => a * b;

// Functions
fn mult3 (a,b) => a*b;
fn multiplier (f, a, b) => f(a,b);

// Function call
var a = multiplier(mult1, 2, 2);
var b = multiplier(mult2, 3, 4);
var c = multiplier(mult3, 4, 6);

// Tuple literal
var x,y,z = (a,b,c);

// Object literal
var lang = { name: "Zenit", version: 0.0000000 };

// Loops
for (var i=0; i < 10; i++);

for var i=0; i < 10; i++ {
    // ...
}

while (/*expression*/);

while /*outer expression*/ {
    while /*inner expression*/ {
        break 2; // Break the outer while
    }
}

// Conditionals
if (/*expression*/);

if /*expression*/ {
    // ...
}
```

## License

MIT