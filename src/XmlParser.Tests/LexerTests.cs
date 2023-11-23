using Parser;

namespace XmlParser.Tests;

public class LexerTests
{
    private const string TokenDoesNotExist = "The returned TokenKind does not exist";
    
    [Theory]
    [InlineData(TokenKind.LessThanToken, "<")]
    [InlineData(TokenKind.GreaterThanToken, ">")]
    public void GetToken_LessThanAndGreaterThanToken_ReturnsCorrectToken(TokenKind expectedToken, string expectedText)
    {
        var lexer = new Lexer(expectedText);
        var actualToken = lexer.GetCurrentToken();
        
        Assert.NotNull(actualToken);
        Assert.True(Enum.IsDefined(typeof(TokenKind), actualToken.Kind), TokenDoesNotExist);
        Assert.Equal(expectedToken, actualToken.Kind);
        Assert.Equal(expectedText, actualToken.Text);
    }

    [Fact]
    public void GetToken_XmlNameToken_ReturnsCorrectToken()
    {
        const TokenKind expectedToken = TokenKind.XmlNameToken;
        const string expectedText = "HelloWorld";
        var lexer = new Lexer($"<{expectedText}>");

        _ = lexer.GetCurrentToken();
        var actualToken = lexer.GetNextToken();
        
        Assert.NotNull(actualToken);
        Assert.True(Enum.IsDefined(typeof(TokenKind), actualToken.Kind), TokenDoesNotExist);
        
        Assert.Equal(expectedToken, actualToken.Kind);
        Assert.Equal(expectedText, actualToken.Text);
    }
}