using System;

namespace AssemblerLexerNamespace
{
    public enum TokenType
    {
        LABEL,
        INSTRUCTION,
        REGISTER,
        NUMBER,
        DIRECTIVE,
        OPERATOR,
        COMMENT,
        STRING,
        IDENTIFIER,
        ERROR,
        WHITESPACE
    }

    public class Token
    {
        public TokenType? Type { get; set; }
        public string Value { get; set; }

        public Token(TokenType? type, string value)
        {
            Type = type;
            Value = value;
        }

        public override string ToString()
        {
            return $"<{Value}, {(Type.HasValue ? Type.Value.ToString() : "null")}>";
        }
    }
}