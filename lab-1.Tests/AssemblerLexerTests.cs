using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using AssemblerLexerNamespace;
using NUnit.Framework.Legacy;

namespace lab_1.Tests
{
    [TestFixture]
    public class AssemblerLexerTests
    {
        private AssemblerLexer _lexer;
        private IPatternProvider _patternProvider;
        private IColorTheme _colorTheme;

        [OneTimeSetUp]
        public void InitializeTestSuite()
        {
            // Виконується один раз перед усіма тестами
            TestContext.Progress.WriteLine("Initializing test suite for AssemblerLexer");
        }

        [SetUp]
        public void Setup()
        {
            // Ініціалізація перед кожним тестом
            _patternProvider = new AssemblerPatternProvider();
            _colorTheme = new AnsiColorTheme();
            _lexer = new AssemblerLexer(_patternProvider, _colorTheme);
        }

        [Test]
        public void Tokenize_EmptyString_ReturnsEmptyList()
        {
            // Arrange
            string code = string.Empty;

            // Act
            var tokens = _lexer.Tokenize(code);

            // Assert
            Assert.That(tokens, Is.Empty);
        }

        [Test]
        public void Tokenize_SimpleInstruction_ReturnsCorrectTokens()
        {
            // Arrange
            string code = "MOV AX, BX";

            // Act
            var tokens = _lexer.Tokenize(code);
            var tokenTypes = tokens.Where(t => t.Type.HasValue).Select(t => t.Type.Value).ToList();

            // Assert
            Assert.That(tokenTypes, Does.Contain(TokenType.INSTRUCTION));
            Assert.That(tokenTypes, Does.Contain(TokenType.REGISTER));
            Assert.That(tokenTypes, Does.Contain(TokenType.OPERATOR));
        }

        [Test]
        public void Tokenize_Comment_ReturnsCommentToken()
        {
            // Arrange
            string code = "; This is a comment";

            // Act
            var tokens = _lexer.Tokenize(code);
            var commentToken = tokens.FirstOrDefault(t => t.Type == TokenType.COMMENT);

            // Assert
            Assert.That(commentToken, Is.Not.Null);
            Assert.That(commentToken.Value, Is.EqualTo("; This is a comment"));
        }

        [Test]
        public void FindNextToken_ValidToken_ReturnsCorrectPositionAndToken()
        {
            // Arrange
            string line = "MOV AX, BX";
            int position = 0;

            // Act
            var (token, newPosition) = _lexer.FindNextToken(line, position);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(token.Type, Is.EqualTo(TokenType.INSTRUCTION));
                Assert.That(token.Value, Is.EqualTo("MOV"));
                Assert.That(newPosition, Is.EqualTo(3));
            });
        }

        [Test]
        [TestCase("MOV", TokenType.INSTRUCTION)]
        [TestCase("AX", TokenType.REGISTER)]
        [TestCase("0x1234", TokenType.NUMBER)]
        [TestCase(";comment", TokenType.COMMENT)]
        [TestCase(".MODEL", TokenType.DIRECTIVE)]
        public void Tokenize_DifferentTokenTypes_ReturnsCorrectType(string input, TokenType expectedType)
        {
            // Act
            var tokens = _lexer.Tokenize(input);
            var firstToken = tokens.FirstOrDefault(t => t.Type.HasValue);

            // Assert
            Assert.That(firstToken.Type, Is.EqualTo(expectedType));
        }

        [Test]
        public void GetTokenStatistics_MixedCode_ReturnsCorrectCounts()
        {
            // Arrange
            string code = @"
LABEL: MOV AX, 10 ; Initialize accumulator
       ADD AX, BX
       RET";

            // Act
            var stats = _lexer.GetTokenStatistics(code);

            // Assert
            Assert.That(stats, Contains.Key(TokenType.LABEL));
            Assert.That(stats, Contains.Key(TokenType.INSTRUCTION));
            Assert.That(stats, Contains.Key(TokenType.REGISTER));
            Assert.That(stats, Contains.Key(TokenType.NUMBER));
            Assert.That(stats, Contains.Key(TokenType.COMMENT));
            
            // Перевіряємо кількість інструкцій
            Assert.That(stats[TokenType.INSTRUCTION], Is.EqualTo(3)); // MOV, ADD, RET
        }

        [Test]
        public void FindTokensByType_WithExistingType_ReturnsMatchingTokens()
        {
            // Arrange
            string code = "MOV AX, BX\nADD CX, DX";

            // Act
            var registerTokens = _lexer.FindTokensByType(code, TokenType.REGISTER);

            // Assert
            Assert.That(registerTokens, Has.Count.EqualTo(4));
            CollectionAssert.AreEquivalent(
                new[] { "AX", "BX", "CX", "DX" },
                registerTokens.Select(t => t.Value).ToArray()
            );
        }

        [Test]
        public void ValidateCode_WithErrorTokens_ReturnsFalseAndErrorList()
        {
            // Arrange
            string code = "MOV AX, № ; Invalid character";

            // Act
            bool isValid = _lexer.ValidateCode(code, out var errorTokens);

            // Assert
            Assert.That(isValid, Is.False);
            Assert.That(errorTokens, Is.Not.Empty);
        }

        [Test]
        public void ValidateCode_ValidCode_ReturnsTrueAndEmptyErrorList()
        {
            // Arrange
            string code = "MOV AX, BX";

            // Act
            bool isValid = _lexer.ValidateCode(code, out var errorTokens);

            // Assert
            Assert.That(isValid, Is.True);
            Assert.That(errorTokens, Is.Empty);
        }

        [Test]
        public void TestTokenizationException()
        {
            // Arrange
            var lexerWithExceptions = new AssemblerLexer(_patternProvider, _colorTheme, true);
            string invalidCode = "\0"; // Непідтримуваний NULL символ

            // Act & Assert
            var ex = Assert.Throws<TokenizationException>(() => lexerWithExceptions.Tokenize(invalidCode));
            Assert.That(ex.LineNumber, Is.EqualTo(1));
            Assert.That(ex.Position, Is.EqualTo(0));
            Assert.That(ex.Message, Does.Contain("Error tokenizing"));
        }

        [Test]
        public void GenerateColoredCode_AppliesCorrectColors()
        {
            // Arrange
            string code = "MOV AX, 10";

            // Act
            string coloredCode = _lexer.GenerateColoredCode(code);

            // Assert
            Assert.That(coloredCode, Does.Contain(_colorTheme.GetColor(TokenType.INSTRUCTION)));
            Assert.That(coloredCode, Does.Contain(_colorTheme.GetColor(TokenType.REGISTER)));
            Assert.That(coloredCode, Does.Contain(_colorTheme.GetColor(TokenType.NUMBER)));
            Assert.That(coloredCode, Does.Contain(_colorTheme.ResetColor));
        }

        [Test]
        public void GetColorLegend_ReturnsLegendForUsedTokens()
        {
            // Arrange
            string code = "MOV AX, 10";

            // Act
            string legend = _lexer.GetColorLegend(code);

            // Assert
            Assert.That(legend, Does.Contain("INSTRUCTION"));
            Assert.That(legend, Does.Contain("REGISTER"));
            Assert.That(legend, Does.Contain("NUMBER"));
            Assert.That(legend, Does.Contain("OPERATOR"));
        }

        [TearDown]
        public void Cleanup()
        {
            // Очищення після кожного тесту
            _lexer = null;
            _patternProvider = null;
            _colorTheme = null;
        }

        [OneTimeTearDown]
        public void FinalizeTestSuite()
        {
            // Виконується один раз після всіх тестів
            TestContext.Progress.WriteLine("All tests completed for AssemblerLexer");
        }
    }
}