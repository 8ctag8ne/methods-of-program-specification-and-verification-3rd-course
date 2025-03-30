using NUnit.Framework;
using System;
using System.Linq;
using AssemblerLexerNamespace;

namespace lab_1.Tests
{
    [TestFixture]
    public class IntegrationTests
    {
        private AssemblerLexer _lexer;

        [SetUp]
        public void Setup()
        {
            _lexer = new AssemblerLexer();
        }

        [Test]
        public void FullParse_SimpleAssemblyProgram_CorrectlyTokenizes()
        {
            // Arrange
            string code = @"
MAIN:
    MOV AX, 10      ; Ініціалізація AX
    MOV BX, 20      ; Ініціалізація BX
    ADD AX, BX      ; AX = AX + BX
    RET             ; Повернення з процедури
";

            // Act
            var tokens = _lexer.Tokenize(code);
            var filteredTokens = tokens.Where(t => 
                t.Type.HasValue && 
                t.Type != TokenType.WHITESPACE &&
                t.Type != TokenType.ERROR).ToList();
            
            // Assert
            Assert.That(filteredTokens.Count, Is.GreaterThan(10));
            
            // Перевіряємо наявність різних типів токенів
            var tokenTypes = filteredTokens.Select(t => t.Type.Value).Distinct().ToList();
            Assert.That(tokenTypes, Does.Contain(TokenType.LABEL));
            Assert.That(tokenTypes, Does.Contain(TokenType.INSTRUCTION));
            Assert.That(tokenTypes, Does.Contain(TokenType.REGISTER));
            Assert.That(tokenTypes, Does.Contain(TokenType.NUMBER));
            Assert.That(tokenTypes, Does.Contain(TokenType.COMMENT));
            Assert.That(tokenTypes, Does.Contain(TokenType.OPERATOR));
        }

        [Test]
        public void ColoredOutput_GeneratesAnsiColorCodes()
        {
            // Arrange
            string code = "MOV AX, 10";
            
            // Act
            string coloredCode = _lexer.GenerateColoredCode(code);
            
            // Assert
            Assert.That(coloredCode, Does.Contain("\u001b["));  // Має містити ANSI коди кольорів
            Assert.That(coloredCode, Does.Contain("MOV"));
            Assert.That(coloredCode, Does.Contain("AX"));
            Assert.That(coloredCode, Does.Contain("10"));
        }
        
        [Test]
        public void TokenStatistics_CountsEachTokenType()
        {
            // Arrange
            string code = @"
LABEL1: MOV AX, 10
LABEL2: MOV BX, 20
        ADD AX, BX
";
            
            // Act
            var stats = _lexer.GetTokenStatistics(code);
            
            // Assert
            Assert.That(stats[TokenType.LABEL], Is.EqualTo(2));
            Assert.That(stats[TokenType.INSTRUCTION], Is.EqualTo(3));
            Assert.That(stats[TokenType.REGISTER], Is.EqualTo(4));
            Assert.That(stats[TokenType.NUMBER], Is.EqualTo(2));
            Assert.That(stats[TokenType.OPERATOR], Is.EqualTo(3));  // Коми
        }

        [Test]
        public void FindTokensByType_InvalidType_ReturnsEmptyList()
        {
            // Arrange
            string code = "MOV AX, 10";
            
            // Act
            var tokens = _lexer.FindTokensByType(code, TokenType.ERROR);
            
            // Assert
            Assert.That(tokens, Is.Empty);
        }
    }
}