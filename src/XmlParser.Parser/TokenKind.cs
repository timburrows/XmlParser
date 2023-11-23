namespace Parser;

public class Token
{
    public TokenKind Kind { get; private set; }
    public string Text { get; private set; }
    
    public Token(TokenKind kind, string text)
    {
        Kind = kind;
        Text = text;
    }
}

public enum TokenKind
{
    LessThanToken,
    GreaterThanToken,
    SlashGreaterThanToken,
    EndOfFileToken,
    LessThanExclamationDoubleDashToken,
    XmlNameToken,
    
    XmlElementStartTag,
}