using System.Text.RegularExpressions;

namespace AssemblerLexer
{
    public interface ITokenPattern
    {
        Regex Pattern { get; }
        TokenType TokenType { get; }
    }

    public class RegexTokenPattern : ITokenPattern
    {
        public Regex Pattern { get; }
        public TokenType TokenType { get; }

        public RegexTokenPattern(string pattern, TokenType tokenType, RegexOptions options = RegexOptions.Compiled)
        {
            Pattern = new Regex(pattern, options);
            TokenType = tokenType;
        }
    }
}