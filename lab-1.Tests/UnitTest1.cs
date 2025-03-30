using AssemblerLexerNamespace;
namespace lab_1.Tests;

public class Tests
{
    private AssemblerLexer lexer;
    [SetUp]
    public void Setup()
    {
        var lexer = new AssemblerLexer();
    }

    [Test]
    public void Tokenize_EmptyString_ReturnsEmptyList()
    {
        // Arrange
        var lexer = new AssemblerLexer();

        // Act
        var tokens = lexer.Tokenize("");
        
        // Assert
        Assert.That(tokens.Count == 0);
    }

    [Test]
    public void Tokenize_SingleInstruction_ReturnsCorrectTokens()
    {
        // Arrange
        var lexer = new AssemblerLexer();
        
        // Act
        var tokens = lexer.Tokenize("MOV AX, BX");
        
        // Assert
        var nonWhitespaceTokens = tokens.Where(t => t.Type != TokenType.WHITESPACE && t.Type != null).ToList();
        Assert.That(4 == nonWhitespaceTokens.Count);
        Assert.That(TokenType.INSTRUCTION == nonWhitespaceTokens[0].Type);
        Assert.That(TokenType.REGISTER == nonWhitespaceTokens[1].Type);
        Assert.That(TokenType.OPERATOR == nonWhitespaceTokens[2].Type);
        Assert.That(TokenType.REGISTER == nonWhitespaceTokens[3].Type);
    }
}
