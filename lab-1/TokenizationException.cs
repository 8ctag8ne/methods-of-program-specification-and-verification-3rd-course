using System;

namespace AssemblerLexerNamespace
{
    public class TokenizationException : Exception
    {
        public int LineNumber { get; }
        public int Position { get; }
        public string Line { get; }

        public TokenizationException(string message, int lineNumber, int position, string line)
            : base(message)
        {
            LineNumber = lineNumber;
            Position = position;
            Line = line;
        }
    }
}
