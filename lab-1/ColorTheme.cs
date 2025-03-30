using System.Collections.Generic;

namespace AssemblerLexerNamespace
{
    public interface IColorTheme
    {
        string GetColor(TokenType tokenType);
        string ResetColor { get; }
    }

    public class AnsiColorTheme : IColorTheme
    {
        private readonly Dictionary<TokenType, string> colors;
        public string ResetColor { get; } = "\u001b[0m";

        public AnsiColorTheme()
        {
            colors = new Dictionary<TokenType, string>
            {
                { TokenType.LABEL, "\u001b[95m" },       // магента
                { TokenType.INSTRUCTION, "\u001b[94m" }, // синій
                { TokenType.REGISTER, "\u001b[92m" },    // зелений
                { TokenType.NUMBER, "\u001b[93m" },      // жовтий
                { TokenType.DIRECTIVE, "\u001b[96m" },   // блакитний
                { TokenType.OPERATOR, "\u001b[91m" },    // червоний
                { TokenType.COMMENT, "\u001b[90m" },     // сірий
                { TokenType.STRING, "\u001b[93m" },      // жовтий
                { TokenType.IDENTIFIER, "\u001b[97m" },  // білий
                { TokenType.ERROR, "\u001b[41m" },       // червоний фон
                { TokenType.WHITESPACE, "\u001b[0m" }    // без кольору
            };
        }

        public string GetColor(TokenType tokenType)
        {
            return colors.TryGetValue(tokenType, out var color) ? color : ResetColor;
        }
    }
}