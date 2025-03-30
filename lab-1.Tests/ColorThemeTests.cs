using NUnit.Framework;
using System.Collections.Generic;
using AssemblerLexerNamespace;

namespace lab_1.Tests
{
    [TestFixture]
    public class ColorThemeTests
    {
        private IColorTheme _theme;

        [SetUp]
        public void Setup()
        {
            _theme = new AnsiColorTheme();
        }

        [Test]
        public void AnsiColorTheme_HasResetColor()
        {
            // Assert
            Assert.That(_theme.ResetColor, Is.EqualTo("\u001b[0m"));
        }

        [Test]
        public void GetColor_ForAllTokenTypes_ReturnsNonEmptyString()
        {
            // Arrange
            var tokenTypes = new[]
            {
                TokenType.LABEL,
                TokenType.INSTRUCTION,
                TokenType.REGISTER,
                TokenType.NUMBER,
                TokenType.DIRECTIVE,
                TokenType.OPERATOR,
                TokenType.COMMENT,
                TokenType.STRING,
                TokenType.IDENTIFIER,
                TokenType.ERROR,
                TokenType.WHITESPACE
            };

            // Act & Assert
            foreach (var tokenType in tokenTypes)
            {
                string color = _theme.GetColor(tokenType);
                Assert.That(color, Is.Not.Null.And.Not.Empty);
                Assert.That(color, Does.StartWith("\u001b["));
            }
        }

        [Test]
        [TestCase(TokenType.INSTRUCTION)]
        [TestCase(TokenType.REGISTER)]
        [TestCase(TokenType.NUMBER)]
        public void GetColor_ReturnsConsistentColorForSameType(TokenType tokenType)
        {
            // Act
            string color1 = _theme.GetColor(tokenType);
            string color2 = _theme.GetColor(tokenType);
            
            // Assert
            Assert.That(color1, Is.EqualTo(color2), $"Color for {tokenType} should be consistent");
        }

        [Test]
        public void GetColor_DifferentTokenTypes_ReturnsDifferentColors()
        {
            // Arrange
            var uniqueColors = new HashSet<string>();
            var tokenTypes = new[]
            {
                TokenType.INSTRUCTION,
                TokenType.REGISTER,
                TokenType.NUMBER,
                TokenType.DIRECTIVE,
                TokenType.OPERATOR
            };
            
            // Act
            foreach (var type in tokenTypes)
            {
                uniqueColors.Add(_theme.GetColor(type));
            }
            
            // Assert
            Assert.That(uniqueColors.Count, Is.EqualTo(tokenTypes.Length),
                "Each token type should have a distinct color");
        }
    }
}