// //AssemblerLexerTests.cs
// using NUnit.Framework;
// using System;
// using System.Collections.Generic;
// using System.Linq;
// using AssemblerLexerNamespace;
// using NUnit.Framework.Legacy;
// using lab_1.Tests;

// namespace lab_1.Tests
// {
//     [TestFixture]
//     public class AssemblerLexerTests
//     {
//         private AssemblerLexer _lexer;
//         private IPatternProvider _patternProvider;
//         private IColorTheme _colorTheme;

//         [OneTimeSetUp]
//         public void InitializeTestSuite()
//         {
//             // Виконується один раз перед усіма тестами
//             TestContext.Progress.WriteLine("Initializing test suite for AssemblerLexer");
//         }

//         [SetUp]
//         public void Setup()
//         {
//             // Ініціалізація перед кожним тестом
//             _patternProvider = new AssemblerPatternProvider();
//             _colorTheme = new AnsiColorTheme();
//             _lexer = new AssemblerLexer(_patternProvider, _colorTheme);
//         }

//         [Test]
//         public void Tokenize_EmptyString_ReturnsEmptyList()
//         {
//             // Arrange
//             string code = string.Empty;

//             // Act
//             var tokens = _lexer.Tokenize(code);

//             // Assert
//             Assert.That(tokens, Is.Empty);
//         }

//         [Test]
//         public void Tokenize_SimpleInstruction_ReturnsCorrectTokens()
//         {
//             // Arrange
//             string code = "MOV AX, BX";

//             // Act
//             var tokens = _lexer.Tokenize(code);
//             var tokenTypes = tokens.Where(t => t.Type.HasValue).Select(t => t.Type.Value).ToList();

//             // Assert
//             Assert.That(tokenTypes, Does.Contain(TokenType.INSTRUCTION));
//             Assert.That(tokenTypes, Does.Contain(TokenType.REGISTER));
//             Assert.That(tokenTypes, Does.Contain(TokenType.OPERATOR));
//         }

//         [Test]
//         public void Tokenize_Comment_ReturnsCommentToken()
//         {
//             // Arrange
//             string code = "; This is a comment";

//             // Act
//             var tokens = _lexer.Tokenize(code);
//             var commentToken = tokens.FirstOrDefault(t => t.Type == TokenType.COMMENT);

//             // Assert
//             Assert.That(commentToken, Is.Not.Null);
//             Assert.That(commentToken.Value, Is.EqualTo("; This is a comment"));
//         }

//         [Test]
//         public void FindNextToken_ValidToken_ReturnsCorrectPositionAndToken()
//         {
//             // Arrange
//             string line = "MOV AX, BX";
//             int position = 0;

//             // Act
//             var (token, newPosition) = _lexer.FindNextToken(line, position);

//             // Assert
//             Assert.Multiple(() =>
//             {
//                 Assert.That(token.Type, Is.EqualTo(TokenType.INSTRUCTION));
//                 Assert.That(token.Value, Is.EqualTo("MOV"));
//                 Assert.That(newPosition, Is.EqualTo(3));
//             });
//         }

//         [Test]
//         [TestCase("MOV", TokenType.INSTRUCTION)]
//         [TestCase("AX", TokenType.REGISTER)]
//         [TestCase("0x1234", TokenType.NUMBER)]
//         [TestCase(";comment", TokenType.COMMENT)]
//         [TestCase(".MODEL", TokenType.DIRECTIVE)]
//         public void Tokenize_DifferentTokenTypes_ReturnsCorrectType(string input, TokenType expectedType)
//         {
//             // Act
//             var tokens = _lexer.Tokenize(input);
//             var firstToken = tokens.FirstOrDefault(t => t.Type.HasValue);

//             // Assert
//             Assert.That(firstToken.Type, Is.EqualTo(expectedType));
//         }

//         [Test]
//         public void GetTokenStatistics_MixedCode_ReturnsCorrectCounts()
//         {
//             // Arrange
//             string code = @"
// LABEL: MOV AX, 10 ; Initialize accumulator
//        ADD AX, BX
//        RET";

//             // Act
//             var stats = _lexer.GetTokenStatistics(code);

//             // Assert
//             Assert.That(stats, Contains.Key(TokenType.LABEL));
//             Assert.That(stats, Contains.Key(TokenType.INSTRUCTION));
//             Assert.That(stats, Contains.Key(TokenType.REGISTER));
//             Assert.That(stats, Contains.Key(TokenType.NUMBER));
//             Assert.That(stats, Contains.Key(TokenType.COMMENT));
            
//             // Перевіряємо кількість інструкцій
//             Assert.That(stats[TokenType.INSTRUCTION], Is.EqualTo(3)); // MOV, ADD, RET
//         }

//         [Test]
//         public void FindTokensByType_WithExistingType_ReturnsMatchingTokens()
//         {
//             // Arrange
//             string code = "MOV AX, BX\nADD CX, DX";

//             // Act
//             var registerTokens = _lexer.FindTokensByType(code, TokenType.REGISTER);

//             // Assert
//             Assert.That(registerTokens, Has.Count.EqualTo(4));
//             CollectionAssert.AreEquivalent(
//                 new[] { "AX", "BX", "CX", "DX" },
//                 registerTokens.Select(t => t.Value).ToArray()
//             );
//         }

//         [Test]
//         public void ValidateCode_WithErrorTokens_ReturnsFalseAndErrorList()
//         {
//             // Arrange
//             string code = "MOV AX, № ; Invalid character";

//             // Act
//             bool isValid = _lexer.ValidateCode(code, out var errorTokens);

//             // Assert
//             Assert.That(isValid, Is.False);
//             Assert.That(errorTokens, Is.Not.Empty);
//         }

//         [Test]
//         public void ValidateCode_ValidCode_ReturnsTrueAndEmptyErrorList()
//         {
//             // Arrange
//             string code = "MOV AX, BX";

//             // Act
//             bool isValid = _lexer.ValidateCode(code, out var errorTokens);

//             // Assert
//             Assert.That(isValid, Is.True);
//             Assert.That(errorTokens, Is.Empty);
//         }

//         [Test]
//         public void TestTokenizationException()
//         {
//             // Arrange
//             var lexerWithExceptions = new AssemblerLexer(_patternProvider, _colorTheme, true);
//             string invalidCode = "\0"; // Непідтримуваний NULL символ

//             // Act & Assert
//             var ex = Assert.Throws<TokenizationException>(() => lexerWithExceptions.Tokenize(invalidCode));
//             Assert.That(ex.LineNumber, Is.EqualTo(1));
//             Assert.That(ex.Position, Is.EqualTo(0));
//             Assert.That(ex.Message, Does.Contain("Error tokenizing"));
//         }

//         [Test]
//         public void GenerateColoredCode_AppliesCorrectColors()
//         {
//             // Arrange
//             string code = "MOV AX, 10";

//             // Act
//             string coloredCode = _lexer.GenerateColoredCode(code);

//             // Assert
//             Assert.That(coloredCode, Does.Contain(_colorTheme.GetColor(TokenType.INSTRUCTION)));
//             Assert.That(coloredCode, Does.Contain(_colorTheme.GetColor(TokenType.REGISTER)));
//             Assert.That(coloredCode, Does.Contain(_colorTheme.GetColor(TokenType.NUMBER)));
//             Assert.That(coloredCode, Does.Contain(_colorTheme.ResetColor));
//         }

//         [Test]
//         public void GetColorLegend_ReturnsLegendForUsedTokens()
//         {
//             // Arrange
//             string code = "MOV AX, 10";

//             // Act
//             string legend = _lexer.GetColorLegend(code);

//             // Assert
//             Assert.That(legend, Does.Contain("INSTRUCTION"));
//             Assert.That(legend, Does.Contain("REGISTER"));
//             Assert.That(legend, Does.Contain("NUMBER"));
//             Assert.That(legend, Does.Contain("OPERATOR"));
//         }

//         [TearDown]
//         public void Cleanup()
//         {
//             // Очищення після кожного тесту
//             _lexer = null;
//             _patternProvider = null;
//             _colorTheme = null;
//         }

//         [OneTimeTearDown]
//         public void FinalizeTestSuite()
//         {
//             // Виконується один раз після всіх тестів
//             TestContext.Progress.WriteLine("All tests completed for AssemblerLexer");
//         }
//     }
// }

// //ColorThemeTests.cs
// using NUnit.Framework;
// using System.Collections.Generic;
// using AssemblerLexerNamespace;

// namespace lab_1.Tests
// {
//     [TestFixture]
//     public class ColorThemeTests
//     {
//         private IColorTheme _theme;

//         [SetUp]
//         public void Setup()
//         {
//             _theme = new AnsiColorTheme();
//         }

//         [Test]
//         public void AnsiColorTheme_HasResetColor()
//         {
//             // Assert
//             Assert.That(_theme.ResetColor, Is.EqualTo("\u001b[0m"));
//         }

//         [Test]
//         public void GetColor_ForAllTokenTypes_ReturnsNonEmptyString()
//         {
//             // Arrange
//             var tokenTypes = new[]
//             {
//                 TokenType.LABEL,
//                 TokenType.INSTRUCTION,
//                 TokenType.REGISTER,
//                 TokenType.NUMBER,
//                 TokenType.DIRECTIVE,
//                 TokenType.OPERATOR,
//                 TokenType.COMMENT,
//                 TokenType.STRING,
//                 TokenType.IDENTIFIER,
//                 TokenType.ERROR,
//                 TokenType.WHITESPACE
//             };

//             // Act & Assert
//             foreach (var tokenType in tokenTypes)
//             {
//                 string color = _theme.GetColor(tokenType);
//                 Assert.That(color, Is.Not.Null.And.Not.Empty);
//                 Assert.That(color, Does.StartWith("\u001b["));
//             }
//         }

//         [Test]
//         [TestCase(TokenType.INSTRUCTION)]
//         [TestCase(TokenType.REGISTER)]
//         [TestCase(TokenType.NUMBER)]
//         public void GetColor_ReturnsConsistentColorForSameType(TokenType tokenType)
//         {
//             // Act
//             string color1 = _theme.GetColor(tokenType);
//             string color2 = _theme.GetColor(tokenType);
            
//             // Assert
//             Assert.That(color1, Is.EqualTo(color2), $"Color for {tokenType} should be consistent");
//         }

//         [Test]
//         public void GetColor_DifferentTokenTypes_ReturnsDifferentColors()
//         {
//             // Arrange
//             var uniqueColors = new HashSet<string>();
//             var tokenTypes = new[]
//             {
//                 TokenType.INSTRUCTION,
//                 TokenType.REGISTER,
//                 TokenType.NUMBER,
//                 TokenType.DIRECTIVE,
//                 TokenType.OPERATOR
//             };
            
//             // Act
//             foreach (var type in tokenTypes)
//             {
//                 uniqueColors.Add(_theme.GetColor(type));
//             }
            
//             // Assert
//             Assert.That(uniqueColors.Count, Is.EqualTo(tokenTypes.Length),
//                 "Each token type should have a distinct color");
//         }
//     }
// }

// //IntegrationTests.cs
// using NUnit.Framework;
// using System;
// using System.Linq;
// using AssemblerLexerNamespace;

// namespace lab_1.Tests
// {
//     [TestFixture]
//     public class IntegrationTests
//     {
//         private AssemblerLexer _lexer;

//         [SetUp]
//         public void Setup()
//         {
//             _lexer = new AssemblerLexer();
//         }

//         [Test]
//         public void FullParse_SimpleAssemblyProgram_CorrectlyTokenizes()
//         {
//             // Arrange
//             string code = @"
// MAIN:
//     MOV AX, 10      ; Ініціалізація AX
//     MOV BX, 20      ; Ініціалізація BX
//     ADD AX, BX      ; AX = AX + BX
//     RET             ; Повернення з процедури
// ";

//             // Act
//             var tokens = _lexer.Tokenize(code);
//             var filteredTokens = tokens.Where(t => 
//                 t.Type.HasValue && 
//                 t.Type != TokenType.WHITESPACE &&
//                 t.Type != TokenType.ERROR).ToList();
            
//             // Assert
//             Assert.That(filteredTokens.Count, Is.GreaterThan(10));
            
//             // Перевіряємо наявність різних типів токенів
//             var tokenTypes = filteredTokens.Select(t => t.Type.Value).Distinct().ToList();
//             Assert.That(tokenTypes, Does.Contain(TokenType.LABEL));
//             Assert.That(tokenTypes, Does.Contain(TokenType.INSTRUCTION));
//             Assert.That(tokenTypes, Does.Contain(TokenType.REGISTER));
//             Assert.That(tokenTypes, Does.Contain(TokenType.NUMBER));
//             Assert.That(tokenTypes, Does.Contain(TokenType.COMMENT));
//             Assert.That(tokenTypes, Does.Contain(TokenType.OPERATOR));
//         }

//         [Test]
//         public void ColoredOutput_GeneratesAnsiColorCodes()
//         {
//             // Arrange
//             string code = "MOV AX, 10";
            
//             // Act
//             string coloredCode = _lexer.GenerateColoredCode(code);
            
//             // Assert
//             Assert.That(coloredCode, Does.Contain("\u001b["));  // Має містити ANSI коди кольорів
//             Assert.That(coloredCode, Does.Contain("MOV"));
//             Assert.That(coloredCode, Does.Contain("AX"));
//             Assert.That(coloredCode, Does.Contain("10"));
//         }
        
//         [Test]
//         public void TokenStatistics_CountsEachTokenType()
//         {
//             // Arrange
//             string code = @"
// LABEL1: MOV AX, 10
// LABEL2: MOV BX, 20
//         ADD AX, BX
// ";
            
//             // Act
//             var stats = _lexer.GetTokenStatistics(code);
            
//             // Assert
//             Assert.That(stats[TokenType.LABEL], Is.EqualTo(2));
//             Assert.That(stats[TokenType.INSTRUCTION], Is.EqualTo(3));
//             Assert.That(stats[TokenType.REGISTER], Is.EqualTo(4));
//             Assert.That(stats[TokenType.NUMBER], Is.EqualTo(2));
//             Assert.That(stats[TokenType.OPERATOR], Is.EqualTo(3));  // Коми
//         }

//         [Test]
//         public void FindTokensByType_InvalidType_ReturnsEmptyList()
//         {
//             // Arrange
//             string code = "MOV AX, 10";
            
//             // Act
//             var tokens = _lexer.FindTokensByType(code, TokenType.ERROR);
            
//             // Assert
//             Assert.That(tokens, Is.Empty);
//         }
//     }
// }

// //TokenPatternTests.cs
// using NUnit.Framework;
// using System.Text.RegularExpressions;
// using AssemblerLexerNamespace;

// namespace lab_1.Tests
// {
//     [TestFixture]
//     public class TokenPatternTests
//     {
//         [Test]
//         public void RegexTokenPattern_CreatesPatternWithCorrectType()
//         {
//             // Arrange & Act
//             var pattern = new RegexTokenPattern(@"\b\d+\b", TokenType.NUMBER);

//             // Assert
//             Assert.That(pattern.TokenType, Is.EqualTo(TokenType.NUMBER));
//             Assert.That(pattern.Pattern.ToString(), Does.Contain(@"\b\d+\b"));
//         }

//         [Test]
//         public void RegexTokenPattern_MatchesExpectedText()
//         {
//             // Arrange
//             var pattern = new RegexTokenPattern(@"\b(MOV|ADD)\b", TokenType.INSTRUCTION);
            
//             // Act & Assert
//             Assert.That(pattern.Pattern.IsMatch("MOV"), Is.True);
//             Assert.That(pattern.Pattern.IsMatch("ADD"), Is.True);
//             Assert.That(pattern.Pattern.IsMatch("SUB"), Is.False);
//         }

//         [TestCase(@"\b(AX|BX|CX|DX)\b", "AX", true)]
//         [TestCase(@"\b(AX|BX|CX|DX)\b", "EAX", false)]
//         [TestCase(@"\b\d+\b", "123", true)]
//         [TestCase(@"\b\d+\b", "12a", false)]
//         public void RegexTokenPattern_TestVariousPatterns(string regex, string testString, bool shouldMatch)
//         {
//             // Arrange
//             var pattern = new RegexTokenPattern(regex, TokenType.NUMBER);
            
//             // Act
//             bool matches = pattern.Pattern.IsMatch(testString);
            
//             // Assert
//             Assert.That(matches, Is.EqualTo(shouldMatch));
//         }

//         [Test]
//         public void RegexTokenPattern_CompileOption_CompilesFlagIsPassed()
//         {
//             // Arrange & Act
//             var pattern = new RegexTokenPattern(@"\b\d+\b", TokenType.NUMBER, RegexOptions.Compiled | RegexOptions.IgnoreCase);
            
//             // Assert
//             Assert.That(pattern.Pattern.Options.HasFlag(RegexOptions.Compiled), Is.True);
//             Assert.That(pattern.Pattern.Options.HasFlag(RegexOptions.IgnoreCase), Is.True);
//         }
//     }
// }

// //TokenTests.cs
// using NUnit.Framework;
// using AssemblerLexerNamespace;

// namespace lab_1.Tests
// {
//     [TestFixture]
//     public class TokenTests
//     {
//         [Test]
//         public void Token_Constructor_SetsProperties()
//         {
//             // Arrange & Act
//             var token = new Token(TokenType.INSTRUCTION, "MOV");
            
//             // Assert
//             Assert.Multiple(() =>
//             {
//                 Assert.That(token.Type, Is.EqualTo(TokenType.INSTRUCTION));
//                 Assert.That(token.Value, Is.EqualTo("MOV"));
//             });
//         }

//         [Test]
//         public void Token_Constructor_AllowsNullType()
//         {
//             // Arrange & Act
//             var token = new Token(null, "NewLine");
            
//             // Assert
//             Assert.That(token.Type, Is.Null);
//             Assert.That(token.Value, Is.EqualTo("NewLine"));
//         }

//         [Test]
//         public void Token_ToString_ReturnsFormattedString()
//         {
//             // Arrange
//             var token = new Token(TokenType.REGISTER, "AX");
            
//             // Act
//             string result = token.ToString();
            
//             // Assert
//             Assert.That(result, Is.EqualTo("<AX, REGISTER>"));
//         }

//         [Test]
//         public void Token_ToString_HandlesNullType()
//         {
//             // Arrange
//             var token = new Token(null, "\n");
            
//             // Act
//             string result = token.ToString();
            
//             // Assert
//             Assert.That(result, Is.EqualTo("<\n, null>"));
//         }

//         [TestCase(TokenType.INSTRUCTION, "MOV", "<MOV, INSTRUCTION>")]
//         [TestCase(TokenType.REGISTER, "AX", "<AX, REGISTER>")]
//         [TestCase(TokenType.NUMBER, "123", "<123, NUMBER>")]
//         [TestCase(null, "\n", "<\n, null>")]
//         public void Token_ToString_FormatsDifferentTypes(TokenType? type, string value, string expected)
//         {
//             // Arrange
//             var token = new Token(type, value);
            
//             // Act
//             string result = token.ToString();
            
//             // Assert
//             Assert.That(result, Is.EqualTo(expected));
//         }
//     }
// }