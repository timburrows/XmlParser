namespace Parser;

public record UnicodeChars
{
    public const char NULL = (char)0x00;
    public const char TAB = (char)0x09;
    public const char LF = (char)0x0A;
    public const char CR = (char)0x0D;
    public const char SPACE = (char)0x20;
    
    public const char NBSP = (char)0xA0;
    public const char IDEOSP = (char)0x3000;
    public const char LS = (char)0x2028;
    public const char PS = (char)0x2029;
    public const char NEL = (char)0x85;
    
    public static bool IsNewLine(char c)
    {
        return CR == c || LF == c || (c >= NEL && (NEL == c || LS == c || PS == c));
    }
}