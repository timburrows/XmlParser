using System.Text;

namespace Parser;

public class Lexer
{
    private readonly string _buffer;
    private int _bufferOffset = 0;
    
    private char _lastChar = UnicodeChars.SPACE;

    public Lexer(string text)
    {
        _buffer = text;
    }

    private Token? _currentToken;
    private Token? _previousToken;
    private readonly List<Token> _tokens = new();

    public Token GetCurrentToken()
    {
        return _currentToken ??= GetToken();
    }

    public Token GetNextToken()
    {
        _previousToken = _currentToken;
        _currentToken = GetToken();
        return _currentToken;
    }
    
    private Token GetToken()
    {
        while (CanGetNextChar)
        {
            var peekedNextChar = PeekNextChar();
            switch (peekedNextChar)
            {
                case UnicodeChars.CR:
                case UnicodeChars.LF:
                case UnicodeChars.SPACE:
                case UnicodeChars.TAB:
                    continue; // ignore whitespace
                case '<':
                    if (CanGetCharAtOffset(1))
                    {
                        var ch = PeekAheadChar();
                        switch (ch)
                        {
                            // todo: add support for comments/instruction tokens
                            case '!':
                            case '?':
                                throw new NotSupportedException("Comments, CDATA and Processing instruction tokens are not supported yet");

                            // todo!: implement EndXmlElement tag tokens
                            case '/':
                                throw new NotImplementedException("EndXmlElement tags have not been implemented yet");
                        }
                    }
                    
                    return CreateXmlLessThanToken();
                
                case '>':
                    return CreateXmlGreaterToken();
                
                default:
                    return ScanXmlElementName();
            }
        }

        return CreateEofToken();


        // ---------------------

        // Skip through all whitespace
        // while (char.IsWhiteSpace(_lastChar))
        // {
        //     if (TryAdvanceChar(out var ch))
        //         _lastChar = ch.GetValueOrDefault();
        // }

        // if (char.IsPunctuation(_lastChar))
        // {
        //     _identifierString = _lastChar.ToString();
        //     while (char.IsPunctuation(_lastChar))
        //     {
        //         _identifierString += _lastChar;
        //     }
        //
        //     if (_identifierString == "<!--") return (int)TokenKind.LessThanExclamationDoubleDash;
        //     if (_identifierString == "<*
        // }

        // while (char.IsPunctuation(_lastChar))
        // {
        //     _identifierString += _lastChar;
        //     switch (_identifierString)
        //     {
        //         case "<":
        //             return (int)TokenKind.LessThanToken;
        //         case "<!--":
        //             return (int)TokenKind.LessThanExclamationDoubleDash;
        //         case ">":
        //             return (int)TokenKind.GreaterThan;
        //         case "/>":
        //             return (int)TokenKind.SlashGreaterThan;
        //     }
        // }
        // ---------------------
    }

    private Token ScanXmlElementName()
    {
        var currentOffset = 0;
        var text = new StringBuilder();

        var finished = false;
        do
        {
            var ch = PeekAheadChar(currentOffset);
            if (currentOffset == 0 && char.IsDigit(ch))
                throw new ApplicationException("Invalid XmlElement name. Must start with an alphabetical character");

            if (char.IsLetterOrDigit(ch))
            {
                text.Append(ch);
                currentOffset += 1;
            }
            else
            {
                finished = true;
            }
        } while (!finished && CanGetCharAtOffset(currentOffset));
        
        if (!TryAdvanceChar(out _, text.Length))
            throw new ApplicationException("Failed to advance character offset buffer to end of XmlElementName");

        return new Token(TokenKind.XmlNameToken, text.ToString());
    }

    private Token CreateXmlGreaterToken()
    {
        TryAdvanceChar(out _);
        return new Token(TokenKind.LessThanToken, ">");
    }

    private Token CreateEofToken()
    {
        throw new NotImplementedException();
    }

    private Token CreateXmlLessThanToken()
    {
        TryAdvanceChar(out _);
        return new Token(TokenKind.LessThanToken, "<");
    }

    private char PeekAheadChar(int offset = 1)
    {
        if (_bufferOffset > _buffer.Length + offset)
        {
            throw new IndexOutOfRangeException("Cannot peak further. String end");
        }

        return _buffer[_bufferOffset + offset];
    }

    private bool CanGetNextChar => _bufferOffset < _buffer.Length;
    private bool CanGetCharAtOffset(int offset) => _bufferOffset + offset < _buffer.Length; 
    
    private bool TryAdvanceChar(out char? c, int skip = 1)
    {
        _bufferOffset += skip;
        if (CanGetNextChar)
        {
            c = _buffer[_bufferOffset];
            return true;
        }

        c = null;
        return false;
    }

    private char PeekNextChar()
    {
        if (!CanGetNextChar)
            throw new IndexOutOfRangeException("Reached end of string buffer");
        
        return _buffer[_bufferOffset];
    }

}