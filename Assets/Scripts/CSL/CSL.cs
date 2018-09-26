public class CSL {
	public enum Token { WHITESP, NEWLN, WHILE, ELSE, IF, PLAYER, DECK, MAP, CAMERA, ENGINE, TITLE, TEXT, UI,
		CARD, NEW, VAR, DELETE, TRUE, FALSE, BOOLEAN, RETURN, RUN, LEFTBRACE, LEFTSQR, LEFTPAREN,
		RIGHTBRACE, RIGHTSQR, RIGHTPAREN, AND, OR, NOT, XOR, COMMA, PERIOD, EQUAL,
		NOTEQUAL, GREATEQUAL, LESSEQUAL, GREATTHAN, LESSTHAN, ASSIGN, ADD, SUB, DIV, MUL, SEMICOLON,
		IDENTIFIER, FLOATCON, STRINGCON, INTCON, ERR
	}

	public static TokenRegexPair[] tokenRegexPairs = new TokenRegexPair[] {
		new TokenRegexPair(Token.WHITESP,	@" |\t"),
		new TokenRegexPair(Token.NEWLN,		@"\n"),
		new TokenRegexPair(Token.WHILE,		@"while"),
		new TokenRegexPair(Token.ELSE,		@"else"),
		new TokenRegexPair(Token.IF,		@"if"),
		new TokenRegexPair(Token.PLAYER,	@"player"),
		new TokenRegexPair(Token.DECK,		@"deck"),
		new TokenRegexPair(Token.MAP,		@"map"),
		new TokenRegexPair(Token.CAMERA,	@"camera"),
		new TokenRegexPair(Token.ENGINE,	@"engine"),
		new TokenRegexPair(Token.TITLE,		@"title"),
		new TokenRegexPair(Token.TEXT,		@"text"),
		new TokenRegexPair(Token.UI,		@"UI"),
		new TokenRegexPair(Token.CARD,		@"card"),
		new TokenRegexPair(Token.NEW,		@"new"),
		new TokenRegexPair(Token.VAR,		@"var"),
		new TokenRegexPair(Token.DELETE,	@"delete"),
		new TokenRegexPair(Token.TRUE,		@"true"),
		new TokenRegexPair(Token.FALSE,		@"false"),
		new TokenRegexPair(Token.BOOLEAN,	@"bool"),
		new TokenRegexPair(Token.RETURN,	@"return"),
		new TokenRegexPair(Token.RUN,		@"run"),
		new TokenRegexPair(Token.LEFTBRACE, @"\{"),
		new TokenRegexPair(Token.LEFTSQR,	@"\["),
		new TokenRegexPair(Token.LEFTPAREN,	@"\("),
		new TokenRegexPair(Token.RIGHTBRACE,@"\}"),
		new TokenRegexPair(Token.RIGHTSQR,	@"\]"),
		new TokenRegexPair(Token.RIGHTPAREN,@"\)"),
		new TokenRegexPair(Token.AND,		@"&&"),
		new TokenRegexPair(Token.OR,		@"\|\|"),
		new TokenRegexPair(Token.XOR,		@"|x|"),
		new TokenRegexPair(Token.NOT,		@"!"),
		new TokenRegexPair(Token.COMMA,		@","),
		new TokenRegexPair(Token.PERIOD,	@"\."),
		new TokenRegexPair(Token.EQUAL,		@"=="),
		new TokenRegexPair(Token.NOTEQUAL,	@"!="),
		new TokenRegexPair(Token.GREATEQUAL,@">="),
		new TokenRegexPair(Token.LESSEQUAL,	@"<="),
		new TokenRegexPair(Token.GREATTHAN,	@">"),
		new TokenRegexPair(Token.LESSTHAN,	@"<"),
		new TokenRegexPair(Token.ASSIGN,	@"="),
		new TokenRegexPair(Token.ADD,		@"+"),
		new TokenRegexPair(Token.SUB,		@"-"),
		new TokenRegexPair(Token.DIV,		@"/"),
		new TokenRegexPair(Token.MUL,		@"*"),
		new TokenRegexPair(Token.SEMICOLON,	@";"),
		new TokenRegexPair(Token.IDENTIFIER,@"[a-zA-Z]([a-zA-Z]|[0-9])*"),
		new TokenRegexPair(Token.FLOATCON,  @"-?\d*\.\d+"),
		new TokenRegexPair(Token.STRINGCON,	@""),
		new TokenRegexPair(Token.INTCON,    @"-?[0-9]+"),
		new TokenRegexPair(Token.ERR,       @"\”[.]*\”")
	};


	public static object Box(Token token, string text) {
		switch (token) {
			case Token.INTCON:
				return (object)int.Parse(text);
			default:
				return (object)text;
		}
	}


	public struct TokenRegexPair {
		public Token token;
		public string regex;
		public TokenRegexPair(Token token, string regex) {
			this.token = token;
			this.regex = regex;
		} 
	}
}
