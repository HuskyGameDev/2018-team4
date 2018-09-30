using UnityEngine;

/// <summary>
/// A grammer and rules specification for a Card Scripting Language
/// </summary>
public class CSL {

	#region Token
	/// <summary>
	/// The types of tokens interpreted by the system.
	/// </summary>
	public enum Token {
		//Non Temrinals
		NumConstant, Variable, Factor, MullExpr, SimpleExpr, Expr, CompoundStatement, ExitStatement, ReturnStatement,
		IOStatement, Test, WhileStatement, TestStatment, IfStatement, Assignment, Statement, StatementList, Type, 
		VarDecl, IdentifierList, DeclList, ProcedureBody, FunctionDecl, ProcedureHead, ProceadureDecl, Procedures, Script,

		//Terminals
		WHITESP, NEWLN, WHILE, ELSE, IF, PLAYER, DECK, MAP, CAMERA, ENGINE, TITLE, TEXT, UI,
		CARD, NEW, VAR, DELETE, TRUE, FALSE, RETURN, RUN, LEFTBRACE, LEFTSQR, LEFTPAREN,
		RIGHTBRACE, RIGHTSQR, RIGHTPAREN, AND, OR, NOT, XOR, COMMA, PERIOD, EQUAL,
		NOTEQUAL, GREATEQUAL, LESSEQUAL, GREATTHAN, LESSTHAN, ASSIGN, ADD, SUB, DIV, MUL, SEMICOLON,
		IDENTIFIER, FLOATCON, STRINGCON, INTCON, ERR, IDENERR
	}

	public static TokenRegexPair[] terminalTokens = new TokenRegexPair[] {
		new TokenRegexPair(Token.NEWLN,		"\n",			@"^(\n)$"),
		new TokenRegexPair(Token.WHITESP,	" ",			@"^\s$"),
		new TokenRegexPair(Token.WHILE,		"while",		@"^(while)$"),
		new TokenRegexPair(Token.ELSE,		"else",			@"^(else)$"),
		new TokenRegexPair(Token.IF,		"if",			@"^(if)$"),
		new TokenRegexPair(Token.PLAYER,	"player",		@"^(player)$"),
		new TokenRegexPair(Token.DECK,		"deck",			@"^(deck)$"),
		new TokenRegexPair(Token.MAP,		"map",			@"^(map)$"),
		new TokenRegexPair(Token.CAMERA,	"camera",		@"^(camera)$"),
		new TokenRegexPair(Token.ENGINE,	"engine",		@"^(engine)$"),
		new TokenRegexPair(Token.TITLE,		"title",		@"^(title)$"),
		new TokenRegexPair(Token.TEXT,		"text",			@"^(text)$"),
		new TokenRegexPair(Token.UI,		"UI",			@"^(UI)$"),
		new TokenRegexPair(Token.CARD,		"card",			@"^(card)$"),
		new TokenRegexPair(Token.NEW,		"new",			@"^(new)$"),
		new TokenRegexPair(Token.VAR,		"var",			@"^(var)$"),
		new TokenRegexPair(Token.DELETE,	"delete",		@"^(delete)$"),
		new TokenRegexPair(Token.TRUE,		"true",			@"^(true)$"),
		new TokenRegexPair(Token.FALSE,		"false",		@"^(false)$"),
		new TokenRegexPair(Token.RETURN,	"return",		@"^(return)$"),
		new TokenRegexPair(Token.RUN,		"run",			@"^(run)$"),
		new TokenRegexPair(Token.LEFTBRACE, "{",			@"^\{$"),
		new TokenRegexPair(Token.LEFTSQR,	"[",			@"^\[$"),
		new TokenRegexPair(Token.LEFTPAREN,	"(",			@"^\($"),
		new TokenRegexPair(Token.RIGHTBRACE,"}",			@"^\}$"),
		new TokenRegexPair(Token.RIGHTSQR,	"]",			@"^\]$"),
		new TokenRegexPair(Token.RIGHTPAREN,")",			@"^\)$"),
		new TokenRegexPair(Token.AND,		"&&",			@"^(&&)$"),
		new TokenRegexPair(Token.OR,		"||",			@"^(\|\|)$"),
		new TokenRegexPair(Token.XOR,		"|x|",			@"^((\|)(x)(\|))$"),
		new TokenRegexPair(Token.NOT,		"!",			@"^(\!)$"),
		new TokenRegexPair(Token.COMMA,		",",			@"^(,)$"),
		new TokenRegexPair(Token.PERIOD,	".",			@"^(\.)$"),
		new TokenRegexPair(Token.EQUAL,		"==",			@"^(==)$"),
		new TokenRegexPair(Token.NOTEQUAL,	"!=",			@"^(!=)$"),
		new TokenRegexPair(Token.GREATEQUAL,">=",			@"^(>=)$"),
		new TokenRegexPair(Token.LESSEQUAL,	"<=",			@"^(<=)$"),
		new TokenRegexPair(Token.GREATTHAN,	">",			@"^(>)$"),
		new TokenRegexPair(Token.LESSTHAN,	"<",			@"^(<)$"),
		new TokenRegexPair(Token.ASSIGN,	"=",			@"^(=)$"),
		new TokenRegexPair(Token.ADD,		"+",			@"^(\+)$"),
		new TokenRegexPair(Token.SUB,		"-",			@"^(-)$"),
		new TokenRegexPair(Token.DIV,		"/",			@"^(/)$"),
		new TokenRegexPair(Token.MUL,		"*",			@"^(\*)$"),
		new TokenRegexPair(Token.SEMICOLON,	";",			@"^(;)$"),
		new TokenRegexPair(Token.IDENERR,   "7Birds",       @"^([0-9]+[a-zA-Z]([a-zA-Z]|[0-9])+)$"),
		new TokenRegexPair(Token.IDENTIFIER,"sumdiffs",		@"^([a-zA-Z]([a-zA-Z]|[0-9])*)$"),
		new TokenRegexPair(Token.FLOATCON,  "-3.2314",       @"^((-?)(\d)*(\.)(\d)+)$"),
		new TokenRegexPair(Token.STRINGCON, "\"Hello\"",    "^(\"(.*)\")$"),
		new TokenRegexPair(Token.INTCON,    "1337",			@"^(-?[0-9]+)$"),
		new TokenRegexPair(Token.ERR,       "`",			@".*$")
	};
	#endregion

	#region Rules
	public static TokenMap[] rules = new TokenMap[] {
		new TokenMap()
	};

	#endregion

	#region Methods
	public static object Box(Token token, string text) {
		//Debug.Log(token + " | " + text);
		switch (token) {
			case Token.INTCON:
				return (object)System.Convert.ToInt32(text);
			case Token.STRINGCON:
				string retString = text.Substring(1,text.Length-1);
				return (object)retString;
			default:
				return (object)text;
		}
	}

	#endregion

	#region Structs
	public struct TokenMap {
		public Token nonTerminal;
		public Token[] tokens;
		public TokenMap(Token nonTerminal, Token[] tokens) {
			this.nonTerminal = nonTerminal;
			this.tokens = tokens;
		}
	}

	public struct TokenRegexPair {
		public Token token;
		public string sample;
		public string regex;
		public TokenRegexPair(Token token, string sample, string regex) {
			this.token = token;
			this.regex = regex;
			this.sample = sample;
		} 
	}
	#endregion
}
