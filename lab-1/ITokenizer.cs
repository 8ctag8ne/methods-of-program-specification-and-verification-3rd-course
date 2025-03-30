using System.Collections.Generic;

namespace AssemblerLexerNamespace
{
    public interface ITokenizer
    {
        List<Token> Tokenize(string code);
        (Token, int) FindNextToken(string line, int position);
    }
}