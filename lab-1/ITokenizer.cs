using System.Collections.Generic;

namespace AssemblerLexer
{
    public interface ITokenizer
    {
        List<Token> Tokenize(string code);
        (Token, int) FindNextToken(string line, int position);
    }
}