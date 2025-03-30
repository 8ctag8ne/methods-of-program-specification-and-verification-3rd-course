using NUnit.Framework;
using System.Text.RegularExpressions;
using AssemblerLexerNamespace;

namespace lab_1.Tests
{
    [TestFixture]
    public class TokenPatternTests
    {
        [Test]
        public void RegexTokenPattern_CreatesPatternWithCorrectType()
        {
            // Arrange & Act
            var pattern = new RegexTokenPattern(@"\b\d+\b", TokenType.NUMBER);

            // Assert
            Assert.That(pattern.TokenType, Is.EqualTo(TokenType.NUMBER));
            Assert.That(pattern.Pattern.ToString(), Does.Contain(@"\b\d+\b"));
        }

        [Test]
        public void RegexTokenPattern_MatchesExpectedText()
        {
            // Arrange
            var pattern = new RegexTokenPattern(@"\b(MOV|ADD)\b", TokenType.INSTRUCTION);
            
            // Act & Assert
            Assert.That(pattern.Pattern.IsMatch("MOV"), Is.True);
            Assert.That(pattern.Pattern.IsMatch("ADD"), Is.True);
            Assert.That(pattern.Pattern.IsMatch("SUB"), Is.False);
        }

        [TestCase(@"\b(AX|BX|CX|DX)\b", "AX", true)]
        [TestCase(@"\b(AX|BX|CX|DX)\b", "EAX", false)]
        [TestCase(@"\b\d+\b", "123", true)]
        [TestCase(@"\b\d+\b", "12a", false)]
        public void RegexTokenPattern_TestVariousPatterns(string regex, string testString, bool shouldMatch)
        {
            // Arrange
            var pattern = new RegexTokenPattern(regex, TokenType.NUMBER);
            
            // Act
            bool matches = pattern.Pattern.IsMatch(testString);
            
            // Assert
            Assert.That(matches, Is.EqualTo(shouldMatch));
        }

        [Test]
        public void RegexTokenPattern_CompileOption_CompilesFlagIsPassed()
        {
            // Arrange & Act
            var pattern = new RegexTokenPattern(@"\b\d+\b", TokenType.NUMBER, RegexOptions.Compiled | RegexOptions.IgnoreCase);
            
            // Assert
            Assert.That(pattern.Pattern.Options.HasFlag(RegexOptions.Compiled), Is.True);
            Assert.That(pattern.Pattern.Options.HasFlag(RegexOptions.IgnoreCase), Is.True);
        }
    }
}