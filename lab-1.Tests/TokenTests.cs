using NUnit.Framework;
using AssemblerLexerNamespace;

namespace lab_1.Tests
{
    [TestFixture]
    public class TokenTests
    {
        [Test]
        public void Token_Constructor_SetsProperties()
        {
            // Arrange & Act
            var token = new Token(TokenType.INSTRUCTION, "MOV");
            
            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(token.Type, Is.EqualTo(TokenType.INSTRUCTION));
                Assert.That(token.Value, Is.EqualTo("MOV"));
            });
        }

        [Test]
        public void Token_Constructor_AllowsNullType()
        {
            // Arrange & Act
            var token = new Token(null, "NewLine");
            
            // Assert
            Assert.That(token.Type, Is.Null);
            Assert.That(token.Value, Is.EqualTo("NewLine"));
        }

        [Test]
        public void Token_ToString_ReturnsFormattedString()
        {
            // Arrange
            var token = new Token(TokenType.REGISTER, "AX");
            
            // Act
            string result = token.ToString();
            
            // Assert
            Assert.That(result, Is.EqualTo("<AX, REGISTER>"));
        }

        [Test]
        public void Token_ToString_HandlesNullType()
        {
            // Arrange
            var token = new Token(null, "\n");
            
            // Act
            string result = token.ToString();
            
            // Assert
            Assert.That(result, Is.EqualTo("<\n, null>"));
        }

        [TestCase(TokenType.INSTRUCTION, "MOV", "<MOV, INSTRUCTION>")]
        [TestCase(TokenType.REGISTER, "AX", "<AX, REGISTER>")]
        [TestCase(TokenType.NUMBER, "123", "<123, NUMBER>")]
        [TestCase(null, "\n", "<\n, null>")]
        public void Token_ToString_FormatsDifferentTypes(TokenType? type, string value, string expected)
        {
            // Arrange
            var token = new Token(type, value);
            
            // Act
            string result = token.ToString();
            
            // Assert
            Assert.That(result, Is.EqualTo(expected));
        }
    }
}