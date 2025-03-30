using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace AssemblerLexer
{
    public class AssemblerLexer : ITokenizer
    {
        private readonly IEnumerable<ITokenPattern> patterns;
        private readonly IColorTheme colorTheme;
        private readonly bool throwOnError;

        public AssemblerLexer() 
            : this(new AssemblerPatternProvider(), new AnsiColorTheme(), false)
        {
        }

        public AssemblerLexer(IPatternProvider patternProvider, IColorTheme colorTheme, bool throwOnError = false)
        {
            this.patterns = patternProvider.GetTokenPatterns();
            this.colorTheme = colorTheme;
            this.throwOnError = throwOnError;
        }

        public List<Token> Tokenize(string code)
        {
            if (string.IsNullOrEmpty(code))
            {
                return new List<Token>();
            }

            List<Token> tokens = new List<Token>();
            string[] lines = code.Split('\n');

            for (int lineNumber = 0; lineNumber < lines.Length; lineNumber++)
            {
                string line = lines[lineNumber];
                int position = 0;

                while (position < line.Length)
                {
                    try
                    {
                        (Token token, int newPosition) = FindNextToken(line, position);
                        tokens.Add(token);
                        position = newPosition;
                    }
                    catch (Exception ex) when (throwOnError)
                    {
                        throw new TokenizationException(
                            $"Error tokenizing at line {lineNumber + 1}, position {position}: {ex.Message}",
                            lineNumber + 1, position, line);
                    }
                }

                tokens.Add(new Token(null, Environment.NewLine));
            }

            return tokens;
        }

        public (Token, int) FindNextToken(string line, int position)
        {
            // Try to match a pattern
            foreach (var pattern in patterns)
            {
                Match match = pattern.Pattern.Match(line, position);
                if (match.Success && match.Index == position)
                {
                    string value = match.Value;
                    return (new Token(pattern.TokenType, value), position + value.Length);
                }
            }

            // Handle whitespace
            if (position < line.Length && char.IsWhiteSpace(line[position]))
            {
                return (new Token(TokenType.WHITESPACE, " "), position + 1);
            }

            // Handle unrecognized character as error
            if (position < line.Length)
            {
                return (new Token(TokenType.ERROR, line[position].ToString()), position + 1);
            }

            throw new InvalidOperationException("Unexpected end of line");
        }

        public string GenerateColoredCode(string code)
        {
            List<Token> tokens = Tokenize(code);
            StringBuilder sb = new StringBuilder();

            foreach (Token token in tokens)
            {
                if (token.Type is null)  // Line break
                {
                    sb.AppendLine();
                }
                else if (token.Type is TokenType type)
                {
                    sb.Append($"{colorTheme.GetColor(type)}{token.Value}{colorTheme.ResetColor}");
                }
            }

            return sb.ToString();
        }

        public string GenerateTokenPairs(string code)
        {
            List<Token> tokens = Tokenize(code);
            StringBuilder sb = new StringBuilder();

            foreach (Token token in tokens)
            {
                if (token.Type != null && token.Type != TokenType.WHITESPACE)
                {
                    sb.AppendLine($"<{token.Value}, {token.Type}>");
                }
            }

            return sb.ToString();
        }

        public Dictionary<TokenType, int> GetTokenStatistics(string code)
        {
            List<Token> tokens = Tokenize(code);
            return tokens
                .Where(t => t.Type.HasValue && t.Type != TokenType.WHITESPACE)
                .GroupBy(t => t.Type.Value)
                .ToDictionary(g => g.Key, g => g.Count());
        }

        public List<Token> FindTokensByType(string code, TokenType tokenType)
        {
            return Tokenize(code)
                .Where(t => t.Type == tokenType)
                .ToList();
        }

        public bool ValidateCode(string code, out List<Token> errorTokens)
        {
            errorTokens = FindTokensByType(code, TokenType.ERROR);
            return !errorTokens.Any();
        }

        public string GetColorLegend(string code)
        {
            List<Token> tokens = Tokenize(code);
            HashSet<TokenType> usedTypes = new HashSet<TokenType>();
            
            foreach (Token token in tokens)
            {
                if (token.Type.HasValue)
                {
                    usedTypes.Add(token.Type.Value);
                }
            }

            StringBuilder sb = new StringBuilder();
            sb.AppendLine("\nЛегенда:");

            Dictionary<TokenType, string> allTypes = new Dictionary<TokenType, string>
            {
                { TokenType.LABEL, "LABEL" },
                { TokenType.INSTRUCTION, "INSTRUCTION" },
                { TokenType.REGISTER, "REGISTER" },
                { TokenType.NUMBER, "NUMBER" },
                { TokenType.DIRECTIVE, "DIRECTIVE" },
                { TokenType.OPERATOR, "OPERATOR" },
                { TokenType.COMMENT, "COMMENT" },
                { TokenType.STRING, "STRING" },
                { TokenType.IDENTIFIER, "IDENTIFIER" },
                { TokenType.ERROR, "ERROR" }
            };

            foreach (TokenType tokenType in Enum.GetValues(typeof(TokenType)))
            {
                if (usedTypes.Contains(tokenType) && tokenType != TokenType.WHITESPACE)
                {
                    string exampleText = allTypes[tokenType];
                    sb.AppendLine($"{colorTheme.GetColor(tokenType)}{exampleText}{colorTheme.ResetColor}");
                }
            }

            return sb.ToString();
        }

        // Original methods kept for backward compatibility
        public void PrintColoredCode(string code)
        {
            Console.Write(GenerateColoredCode(code));
            Console.WriteLine(GetColorLegend(code));
        }

        public void PrintTokenPairs(string code)
        {
            Console.Write(GenerateTokenPairs(code));
        }
    }
}
