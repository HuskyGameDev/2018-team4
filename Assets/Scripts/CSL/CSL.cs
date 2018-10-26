using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using System.Xml;
using System.Xml.Serialization;
using System.IO;

/// <summary>
/// A grammer and rules specification for a Card Scripting Language
/// </summary>
public static class CSL {
	private static readonly bool debugParseOutput = false;
	private static List<ParserInstruction[]> ParseTable = null;
	//private static Dictionary<SymbolicToken, List<SymbolicToken>> firstSet = new Dictionary<SymbolicToken, List<SymbolicToken>>();
	//private static Dictionary<SymbolicToken, List<SymbolicToken>> followSet = new Dictionary<SymbolicToken, List<SymbolicToken>>();

	private static Dictionary<string, object> variableDictionary;

	#region Token
	/// <summary>
	/// The types of tokens interpreted by the system.
	/// </summary>
	public enum SymbolicToken {
		//NEVER REMOVE SCRIPTPRIME or EOF
		ScriptPrime, EOF, Script,


		//Non Temrinals
		StatementList, Statement,
		Assignment, IfStatement, TestStatement, Test, WhileStatement, IOStatement, DiscardStatement, CompoundStatement,
		Expr, SimpleExpr, AddExpr, MullExpr, Factor, Variable, Timing, Enforcement, NumConstant, Type, VariableDeclaration,

		//Terminals
		COMMENT, IMAGE, IMMEDIATE, STATIC, NAME, TEXT, OPTIONAL, REQUIRED, ROOM, ITEM, EVENT, ARTIFACT, WHITESP,
		NEWLN, WHILE, ELSE, IF, VAR, TRUE, FALSE, LEFTBRACE, LEFTSQR, LEFTPAREN,
		CHOICEIDEN, EFFECTIDEN, DISCARD, THIS, DELAYED,
		RIGHTBRACE, RIGHTSQR, RIGHTPAREN, AND, OR, NOT, XOR, COMMA, PERIOD, EQUAL,
		NOTEQUAL, GREATEQUAL, LESSEQUAL, GREATTHAN, LESSTHAN, ASSIGN, ADD, SUB, DIV, MUL, SEMICOLON,
		IDENTIFIER, FLOATCON, STRINGCON, INTCON, ERR, IDENERR
	}

	/// <summary>
	/// The set of Terminal SymbolicTokens
	/// </summary>
    static public List<SymbolicToken> TERMINALS = new List<SymbolicToken>(new SymbolicToken[] {
		SymbolicToken.COMMENT, SymbolicToken.IMAGE, SymbolicToken.IMMEDIATE, SymbolicToken.STATIC, SymbolicToken.NAME, SymbolicToken.TEXT, SymbolicToken.OPTIONAL, SymbolicToken.REQUIRED, SymbolicToken.ROOM, SymbolicToken.ITEM, SymbolicToken.EVENT, SymbolicToken.ARTIFACT, SymbolicToken.WHITESP,
		SymbolicToken.NEWLN, SymbolicToken.WHILE, SymbolicToken.ELSE, SymbolicToken.IF, SymbolicToken.VAR, SymbolicToken.TRUE, SymbolicToken.FALSE, SymbolicToken.LEFTBRACE, SymbolicToken.LEFTSQR, SymbolicToken.LEFTPAREN,
		SymbolicToken.DISCARD, SymbolicToken.THIS, SymbolicToken.DELAYED,
		SymbolicToken.RIGHTBRACE, SymbolicToken.RIGHTSQR, SymbolicToken.RIGHTPAREN, SymbolicToken.AND, SymbolicToken.OR, SymbolicToken.NOT, SymbolicToken.XOR, SymbolicToken.COMMA, SymbolicToken.PERIOD, SymbolicToken.EQUAL,
		SymbolicToken.NOTEQUAL, SymbolicToken.GREATEQUAL, SymbolicToken.LESSEQUAL, SymbolicToken.GREATTHAN, SymbolicToken.LESSTHAN, SymbolicToken.ASSIGN, SymbolicToken.ADD, SymbolicToken.SUB, SymbolicToken.DIV, SymbolicToken.MUL, SymbolicToken.SEMICOLON,
		SymbolicToken.IDENTIFIER, SymbolicToken.FLOATCON, SymbolicToken.STRINGCON, SymbolicToken.INTCON, SymbolicToken.ERR, SymbolicToken.IDENERR, SymbolicToken.EOF
	} );

	/// <summary>
	/// The set of NonTerminal SymbolicTokens
	/// </summary>
    static public List<SymbolicToken> nonTerminals = new List<SymbolicToken>( new SymbolicToken[] {
		SymbolicToken.ScriptPrime, SymbolicToken.VariableDeclaration,
		SymbolicToken.Script, SymbolicToken.StatementList, SymbolicToken.Statement,
		SymbolicToken.Assignment, SymbolicToken.IfStatement, SymbolicToken.TestStatement, SymbolicToken.Test, SymbolicToken.WhileStatement, SymbolicToken.IOStatement, SymbolicToken.DiscardStatement, SymbolicToken.CompoundStatement,
		SymbolicToken.Expr, SymbolicToken.SimpleExpr, SymbolicToken.AddExpr, SymbolicToken.MullExpr, SymbolicToken.Factor, SymbolicToken.Variable, SymbolicToken.Timing, SymbolicToken.Enforcement, SymbolicToken.NumConstant, SymbolicToken.Type
	} );
	#endregion

	#region Rules
	/// <summary>
	/// A mapping of terminal tokens to regex
	/// </summary>
    public static TokenRegexPair[] terminalTokenRegexPair = new TokenRegexPair[] {

		new TokenRegexPair(SymbolicToken.COMMENT,	"/*Goodbye*/",  @"^(/\*(.*)\*/)$"),
		new TokenRegexPair(SymbolicToken.NEWLN,		"\n",			@"^(\n)$"),
		new TokenRegexPair(SymbolicToken.WHITESP,	" ",			@"^\s$"),
		new TokenRegexPair(SymbolicToken.WHILE,		"while",		@"^(while)$"),
		new TokenRegexPair(SymbolicToken.ELSE,		"else",			@"^(else)$"),
		new TokenRegexPair(SymbolicToken.IF,		"if",			@"^(if)$"),
		new TokenRegexPair(SymbolicToken.TEXT,		"text",			@"^(text)$"),
		new TokenRegexPair(SymbolicToken.VAR,		"var",			@"^(var)$"),
		new TokenRegexPair(SymbolicToken.TRUE,		"true",			@"^(true)$"),
		new TokenRegexPair(SymbolicToken.FALSE,		"false",		@"^(false)$"),

		new TokenRegexPair(SymbolicToken.IMAGE,		"image",		@"^(image)$"),
		new TokenRegexPair(SymbolicToken.IMMEDIATE,	"immediate",	@"^(immediate)$"),
		new TokenRegexPair(SymbolicToken.DELAYED,	"delayed",      @"^(delayed)$"),
		new TokenRegexPair(SymbolicToken.IMMEDIATE,	"enter",		@"^(enter)$"),
		new TokenRegexPair(SymbolicToken.DELAYED,	"exit",			@"^(exit)$"),
		new TokenRegexPair(SymbolicToken.STATIC,	"static",		@"^(static)$"),
		new TokenRegexPair(SymbolicToken.NAME,		"name",         @"^(name)$"),
		new TokenRegexPair(SymbolicToken.OPTIONAL,	"optional",     @"^(optional)$"),
		new TokenRegexPair(SymbolicToken.REQUIRED,	"required",     @"^(required)$"),
		new TokenRegexPair(SymbolicToken.ROOM,		"room",         @"^(room)$"),
		new TokenRegexPair(SymbolicToken.ITEM,		"item",         @"^(item)$"),
		new TokenRegexPair(SymbolicToken.EVENT,		"event",		@"^(event)$"),
		new TokenRegexPair(SymbolicToken.ARTIFACT,	"artifact",		@"^(artifact)$"),
		new TokenRegexPair(SymbolicToken.CHOICEIDEN,"choice",		@"^(choice)$"),
		new TokenRegexPair(SymbolicToken.EFFECTIDEN,"effect",       @"^(effect)$"),
		new TokenRegexPair(SymbolicToken.DISCARD,	"discard",		@"^(discard)$"),
		new TokenRegexPair(SymbolicToken.THIS,		"this",			@"^(this)$"),

		new TokenRegexPair(SymbolicToken.LEFTBRACE, "{",			@"^\{$"),
		new TokenRegexPair(SymbolicToken.LEFTSQR,	"[",			@"^\[$"),
		new TokenRegexPair(SymbolicToken.LEFTPAREN,	"(",			@"^\($"),
		new TokenRegexPair(SymbolicToken.RIGHTBRACE,"}",			@"^\}$"),
		new TokenRegexPair(SymbolicToken.RIGHTSQR,	"]",			@"^\]$"),
		new TokenRegexPair(SymbolicToken.RIGHTPAREN,")",			@"^\)$"),

		new TokenRegexPair(SymbolicToken.AND,		"&&",			@"^(&&)$"),
		new TokenRegexPair(SymbolicToken.OR,		"||",			@"^(\|\|)$"),
		new TokenRegexPair(SymbolicToken.XOR,		"|x|",			@"^((\|)(x)(\|))$"),
		new TokenRegexPair(SymbolicToken.NOT,		"!",			@"^(\!)$"),

		new TokenRegexPair(SymbolicToken.COMMA,		",",			@"^(,)$"),
		new TokenRegexPair(SymbolicToken.PERIOD,	".",			@"^(\.)$"),
		new TokenRegexPair(SymbolicToken.SEMICOLON, ";",            @"^(;)$"),


		new TokenRegexPair(SymbolicToken.EQUAL,		"==",			@"^(==)$"),
		new TokenRegexPair(SymbolicToken.NOTEQUAL,	"!=",			@"^(!=)$"),
		new TokenRegexPair(SymbolicToken.GREATEQUAL,">=",			@"^(>=)$"),
		new TokenRegexPair(SymbolicToken.LESSEQUAL,	"<=",			@"^(<=)$"),
		new TokenRegexPair(SymbolicToken.GREATTHAN,	">",			@"^(>)$"),
		new TokenRegexPair(SymbolicToken.LESSTHAN,	"<",			@"^(<)$"),

		new TokenRegexPair(SymbolicToken.ASSIGN,	"=",			@"^(=)$"),

		new TokenRegexPair(SymbolicToken.ADD,		"+",			@"^(\+)$"),
		new TokenRegexPair(SymbolicToken.SUB,		"-",			@"^(-)$"),
		new TokenRegexPair(SymbolicToken.DIV,		"/",			@"^(/)$"),
		new TokenRegexPair(SymbolicToken.MUL,		"*",			@"^(\*)$"),

		new TokenRegexPair(SymbolicToken.IDENERR,   "7Birds",       @"^([0-9]+[a-zA-Z]([a-zA-Z]|[0-9])+)$"),

		new TokenRegexPair(SymbolicToken.IDENTIFIER,"sumdiffs",		"^([a-zA-Z]([a-zA-Z]|[0-9])*)$"),
		new TokenRegexPair(SymbolicToken.STRINGCON, "\"Hello\"",    "^(\"([^\"]*)\")$"),
		new TokenRegexPair(SymbolicToken.FLOATCON,  "-3.2314",      @"^((-?)(\d)*(\.)(\d)+)$"),
		new TokenRegexPair(SymbolicToken.INTCON,    "1337",			@"^(-?[0-9]+)$"),

		new TokenRegexPair(SymbolicToken.ERR,       "`",			@"^.*$")
	};

	/// <summary>
	/// Rules used convert tokens into higher order tokens
	/// </summary>
	public static GrammarRule[] productionRules = new GrammarRule[] {
		new GrammarRule(SymbolicToken.ScriptPrime, new SymbolicToken[] { SymbolicToken.Script }),

		new GrammarRule(SymbolicToken.Script, new SymbolicToken[] { SymbolicToken.StatementList }, 
			(List<ScannedToken> tokens) => {
				if (debugParseOutput) Debug.Log("Rule applied: (Script -> StatementList)");
				tokens[0].GetData(); //Execute the statement list
				return null;
				}
			),

		new GrammarRule(SymbolicToken.StatementList, new SymbolicToken[] { SymbolicToken.Statement },
			(List<ScannedToken> tokens) => {
				if (debugParseOutput) Debug.Log("Rule applied: (StatementList -> Statement)");
				tokens[0].GetData();//Execute the statement 
				return null;
				}
			),
		new GrammarRule(SymbolicToken.StatementList, new SymbolicToken[] { SymbolicToken.StatementList, SymbolicToken.Statement },
			(List<ScannedToken> tokens) => {
				if (debugParseOutput) Debug.Log("Rule applied: (StatementList -> StatementList  Statement)");
				tokens[0].GetData();//Execute the statement List 
				tokens[1].GetData();//Execute the statement
				return null;
				}
			),

		new GrammarRule(SymbolicToken.Statement, new SymbolicToken[] { SymbolicToken.Assignment },
			(List<ScannedToken> tokens) => {
				if (debugParseOutput) Debug.Log("Rule applied: (Statement -> Assignment)");
				return tokens[0].GetData();//Execute the statement List 
				}
			),
		new GrammarRule(SymbolicToken.Statement, new SymbolicToken[] { SymbolicToken.VariableDeclaration },
			(List<ScannedToken> tokens) => {
				if (debugParseOutput) Debug.Log("Rule applied: (Statement -> VariableDeclaration)");
				return tokens[0].GetData();//Execute the statement List 
				}
			),
		new GrammarRule(SymbolicToken.Statement, new SymbolicToken[] { SymbolicToken.IfStatement },
			(List<ScannedToken> tokens) => {
				if (debugParseOutput) Debug.Log("Rule applied: (Statement -> IfStatement)");
				return tokens[0].GetData();//Execute the statement List 
				}
			),
		new GrammarRule(SymbolicToken.Statement, new SymbolicToken[] { SymbolicToken.WhileStatement },
			(List<ScannedToken> tokens) => {
				if (debugParseOutput) Debug.Log("Rule applied: (Statement -> WhileStatement)");
				return tokens[0].GetData();//Execute the statement List 
				}
			),
		//new GrammarRule(SymbolicToken.Statement, new SymbolicToken[] { SymbolicToken.IOStatement } ),
		new GrammarRule(SymbolicToken.Statement, new SymbolicToken[] { SymbolicToken.CompoundStatement },
			(List<ScannedToken> tokens) => {
				if (debugParseOutput) Debug.Log("Rule applied: (Statement -> CompoundStatement)");
				return tokens[0].GetData();//Execute the statement List 
				}
			),
		new GrammarRule(SymbolicToken.Statement, new SymbolicToken[] { SymbolicToken.DiscardStatement },
			(List<ScannedToken> tokens) => {
				if (debugParseOutput) Debug.Log("Rule applied: (Statement -> DiscardStatement)");
				return tokens[0].GetData();//Execute the statement List 
				}
			),

		new GrammarRule(SymbolicToken.Assignment, new SymbolicToken[] { SymbolicToken.IDENTIFIER, SymbolicToken.ASSIGN, SymbolicToken.Expr, SymbolicToken.SEMICOLON },
			(List<ScannedToken> tokens) => {
				if (debugParseOutput) Debug.Log("Rule applied: (Assignment -> VariableDeclaration ASSIGN Expr SEMICOLON)");
				string name = (string)tokens[0].GetData();
				object data = tokens[2].GetData();
				variableDictionary[name] = data;
				return data;//Execute the statement List 
				}
			),

		new GrammarRule(SymbolicToken.Assignment, new SymbolicToken[] { SymbolicToken.IDENTIFIER, SymbolicToken.LEFTSQR, SymbolicToken.Factor, SymbolicToken.RIGHTSQR, SymbolicToken.ASSIGN, SymbolicToken.Expr, SymbolicToken.SEMICOLON },
			(List<ScannedToken> tokens) => {
				if (debugParseOutput) Debug.Log("Rule applied: (Assignment -> IDENTIFIER LEFTSQR Factor RIGHTSQR ASSIGN Expr SEMICOLON)");
				object data = tokens[5].GetData();
				int location = (int)tokens[2].GetData();
				string name = (string)tokens[0].GetData();
				object item = variableDictionary[name];
				((List<object>)item)[location] = data;
				return data;
				}
			),

		new GrammarRule(SymbolicToken.IfStatement, new SymbolicToken[] { SymbolicToken.IF, SymbolicToken.TestStatement, SymbolicToken.ELSE, SymbolicToken.IfStatement },
			(List<ScannedToken> tokens) => {
				if (debugParseOutput) Debug.Log("Rule applied: (IfStatement -> IF TestStatement ELSE IfStatement)");
				bool resultOfTest = (bool)tokens[1].GetData();
				if (resultOfTest == false)
					return tokens[3].GetData();
				
				return null;
				}
			),
		new GrammarRule(SymbolicToken.IfStatement, new SymbolicToken[] { SymbolicToken.IF, SymbolicToken.TestStatement, SymbolicToken.ELSE, SymbolicToken.CompoundStatement },
			(List<ScannedToken> tokens) => {
				if (debugParseOutput) Debug.Log("Rule applied: (IfStatement -> IF TestStatement ELSE CompoundStatement)");
				bool resultOfTest = (bool)tokens[1].GetData();
				if (resultOfTest == false)
					return tokens[3].GetData();
				
				return null;
				}
			),
		new GrammarRule(SymbolicToken.IfStatement, new SymbolicToken[] { SymbolicToken.IF, SymbolicToken.TestStatement, SymbolicToken.ELSE, SymbolicToken.Statement },
			(List<ScannedToken> tokens) => {
				if (debugParseOutput) Debug.Log("Rule applied: (IfStatement -> IF TestStatement ELSE Statement)");
				bool resultOfTest = (bool)tokens[1].GetData();
				if (resultOfTest == false)
					return tokens[3].GetData();

				return null;
				}
			),
		new GrammarRule(SymbolicToken.IfStatement, new SymbolicToken[] { SymbolicToken.IF, SymbolicToken.TestStatement },
			(List<ScannedToken> tokens) => {
				if (debugParseOutput) Debug.Log("Rule applied: (IfStatement -> IF TestStatement)");
					return tokens[1].GetData();
				}
			),

		new GrammarRule(SymbolicToken.TestStatement, new SymbolicToken[] { SymbolicToken.Test, SymbolicToken.CompoundStatement },
			(List<ScannedToken> tokens) => {
				if (debugParseOutput) Debug.Log("Rule applied: (TestStatement -> Test CompoundStatement)");
				bool resultOfTest = (bool)tokens[0].GetData();
				if (resultOfTest == true)
					tokens[1].GetData();
				return resultOfTest;
				}
			),

		new GrammarRule(SymbolicToken.Test, new SymbolicToken[] { SymbolicToken.LEFTPAREN, SymbolicToken.Expr, SymbolicToken.RIGHTPAREN },
			(List<ScannedToken> tokens) => {
				if (debugParseOutput) Debug.Log("Rule applied: (Test -> LEFTPAREN Expr RIGHTPAREN)");
				return tokens[1].GetData();
				}
			),

		new GrammarRule(SymbolicToken.WhileStatement, new SymbolicToken[] { SymbolicToken.WHILE, SymbolicToken.LEFTPAREN, SymbolicToken.Expr, SymbolicToken.RIGHTPAREN, SymbolicToken.CompoundStatement },
			(List<ScannedToken> tokens) => {
				if (debugParseOutput) Debug.Log("Rule applied: (WhileStatement -> WHILE LEFTPAREN Expr RIGHTPAREN CompoundStatement)");
				int safteyIter = 0;
				while (safteyIter < 10000 && ((bool)tokens[2].GetData())) {
					tokens[4].GetData();//Execute the compound statement
				}
				if (safteyIter >= 10000) {
					Debug.LogError("While statement broke due to running too long.");
				}
				return null;
				}
			),
		new GrammarRule(SymbolicToken.WhileStatement, new SymbolicToken[] { SymbolicToken.WHILE, SymbolicToken.LEFTPAREN, SymbolicToken.Expr, SymbolicToken.RIGHTPAREN, SymbolicToken.Statement },
			(List<ScannedToken> tokens) => {
				if (debugParseOutput) Debug.Log("Rule applied: (WhileStatement -> WHILE LEFTPAREN Expr RIGHTPAREN Statement)");
				int safteyIter = 0;
				while (safteyIter < 10000 && ((bool)tokens[2].GetData())) {
					tokens[4].GetData();//Execute the compound statement
				}
				if (safteyIter >= 10000) {
					Debug.LogError("While statement broke due to running too long.");
				}
				return null;
				}
			),

		//new GrammarRule(SymbolicToken.IOStatement, new SymbolicToken[] { SymbolicToken.WHILE, SymbolicToken.Expr, SymbolicToken.Statement } ),

		new GrammarRule(SymbolicToken.DiscardStatement, new SymbolicToken[] {SymbolicToken.DISCARD, SymbolicToken.IDENTIFIER, SymbolicToken.SEMICOLON },
			(List<ScannedToken> tokens) => {
				if (debugParseOutput) Debug.Log("Rule applied: (DiscardStatement -> DISCARD IDENTIFIER SEMICOLON)");
				Debug.Log("Discard was called on ("+tokens[1].GetData()+"), though it did nothing.");
				return null;
				}
			),
		new GrammarRule(SymbolicToken.DiscardStatement, new SymbolicToken[] {SymbolicToken.DISCARD, SymbolicToken.THIS, SymbolicToken.SEMICOLON },
			(List<ScannedToken> tokens) => {
				if (debugParseOutput) Debug.Log("Rule applied: (DiscardStatement -> DISCARD THIS SEMICOLON)");
				Debug.Log("Discard was called on ("+tokens[1].GetData()+"), though it did nothing.");
				return null;
				}
			),

		new GrammarRule(SymbolicToken.CompoundStatement, new SymbolicToken[] { SymbolicToken.LEFTBRACE, SymbolicToken.StatementList, SymbolicToken.RIGHTBRACE },
			(List<ScannedToken> tokens) => {
				if (debugParseOutput) Debug.Log("Rule applied: (CompoundStatement -> LEFTBRACE StatementList RIGHTBRACE)");
				tokens[1].GetData();
				return null;
				}
			),

		new GrammarRule(SymbolicToken.Expr, new SymbolicToken[] { SymbolicToken.SimpleExpr },
			(List<ScannedToken> tokens) => {
				if (debugParseOutput) Debug.Log("Rule applied: (Expr -> SimpleExpr)");
				return tokens[0].GetData();;
				}
			),
		new GrammarRule(SymbolicToken.Expr, new SymbolicToken[] { SymbolicToken.Expr, SymbolicToken.AND, SymbolicToken.SimpleExpr },
			(List<ScannedToken> tokens) => {
				if (debugParseOutput) Debug.Log("Rule applied: (Expr -> Expr AND SimpleExpr)");
				return (bool)tokens[0].GetData() && (bool)tokens[2].GetData();
				}
			),
		new GrammarRule(SymbolicToken.Expr, new SymbolicToken[] { SymbolicToken.Expr, SymbolicToken.OR, SymbolicToken.SimpleExpr },
			(List<ScannedToken> tokens) => {
				if (debugParseOutput) Debug.Log("Rule applied: (Expr -> Expr OR SimpleExpr)");
				return (bool)tokens[0].GetData() || (bool)tokens[2].GetData();
				}
			),
		new GrammarRule(SymbolicToken.Expr, new SymbolicToken[] { SymbolicToken.Expr, SymbolicToken.XOR, SymbolicToken.SimpleExpr },
			(List<ScannedToken> tokens) => {
				if (debugParseOutput) Debug.Log("Rule applied: (Expr -> Expr XOR SimpleExpr)");
				bool r1 = (bool)tokens[0].GetData();
				bool r2 = (bool)tokens[2].GetData();

				return (!r1&&r2) || (r1&&r2);
				}
			),
		new GrammarRule(SymbolicToken.Expr, new SymbolicToken[] { SymbolicToken.NOT, SymbolicToken.SimpleExpr },
			(List<ScannedToken> tokens) => {
				if (debugParseOutput) Debug.Log("Rule applied: (Expr -> NOT SimpleExpr)");
				bool r1 = (bool)tokens[1].GetData();
				return !r1;
				}
			),

		new GrammarRule(SymbolicToken.SimpleExpr, new SymbolicToken[] { SymbolicToken.AddExpr },
			(List<ScannedToken> tokens) => {
				if (debugParseOutput) Debug.Log("Rule applied: (SimpleExpr -> AddExpr)");
				return tokens[0].GetData();
				}
			),
		new GrammarRule(SymbolicToken.SimpleExpr, new SymbolicToken[] { SymbolicToken.SimpleExpr, SymbolicToken.EQUAL, SymbolicToken.AddExpr },
			(List<ScannedToken> tokens) => {
				if (debugParseOutput) Debug.Log("Rule applied: (SimpleExpr -> SimpleExpr EQUAL AddExpr)");
				object d1 = tokens[0].GetData();
				object d2 =  tokens[2].GetData();
				return d1.Equals(d2);
				}
			),
		new GrammarRule(SymbolicToken.SimpleExpr, new SymbolicToken[] { SymbolicToken.SimpleExpr, SymbolicToken.NOTEQUAL, SymbolicToken.AddExpr },
						(List<ScannedToken> tokens) => {
				if (debugParseOutput) Debug.Log("Rule applied: (SimpleExpr -> SimpleExpr NOTEQUAL AddExpr)");
				object d1 = tokens[0].GetData();
				object d2 =  tokens[2].GetData();
				return !d1.Equals(d2);
				}
			),
		new GrammarRule(SymbolicToken.SimpleExpr, new SymbolicToken[] { SymbolicToken.SimpleExpr, SymbolicToken.LESSEQUAL, SymbolicToken.AddExpr },
			(List<ScannedToken> tokens) => {
				if (debugParseOutput) Debug.Log("Rule applied: (SimpleExpr -> SimpleExpr LESSEQUAL AddExpr)");
				object d1 = tokens[0].GetData();
				object d2 = tokens[2].GetData();
				float f1 = System.Convert.ToSingle(d1);
				float f2 =  System.Convert.ToSingle(d2);
				return f1 <= f2;
				}
			),
		new GrammarRule(SymbolicToken.SimpleExpr, new SymbolicToken[] { SymbolicToken.SimpleExpr, SymbolicToken.LESSTHAN, SymbolicToken.AddExpr },
			(List<ScannedToken> tokens) => {
				if (debugParseOutput) Debug.Log("Rule applied: (SimpleExpr -> SimpleExpr LESSTHAN AddExpr)");
				object d1 = tokens[0].GetData();
				object d2 = tokens[2].GetData();
				float f1 = System.Convert.ToSingle(d1);
				float f2 =  System.Convert.ToSingle(d2);
				return f1 < f2;
				}
			),
		new GrammarRule(SymbolicToken.SimpleExpr, new SymbolicToken[] { SymbolicToken.SimpleExpr, SymbolicToken.GREATEQUAL, SymbolicToken.AddExpr },
			(List<ScannedToken> tokens) => {
				if (debugParseOutput) Debug.Log("Rule applied: (SimpleExpr -> SimpleExpr GREATEQUAL AddExpr)");
				object d1 = tokens[0].GetData();
				object d2 = tokens[2].GetData();
				float f1 = System.Convert.ToSingle(d1);
				float f2 =  System.Convert.ToSingle(d2);
				return f1 >= f2;
				}
			),
		new GrammarRule(SymbolicToken.SimpleExpr, new SymbolicToken[] { SymbolicToken.SimpleExpr, SymbolicToken.GREATTHAN, SymbolicToken.AddExpr },
			(List<ScannedToken> tokens) => {
				if (debugParseOutput) Debug.Log("Rule applied: (SimpleExpr -> SimpleExpr GREATTHAN AddExpr)");
				object d1 = tokens[0].GetData();
				object d2 = tokens[2].GetData();
				float f1 = System.Convert.ToSingle(d1);
				float f2 =  System.Convert.ToSingle(d2);
				return f1 > f2;
				}
			),

		new GrammarRule(SymbolicToken.AddExpr, new SymbolicToken[] { SymbolicToken.MullExpr },
			(List<ScannedToken> tokens) => {
				if (debugParseOutput) Debug.Log("Rule applied: (AddExpr -> MullExpr  )");
				return tokens[0].GetData();
				}
			),
		new GrammarRule(SymbolicToken.AddExpr, new SymbolicToken[] { SymbolicToken.AddExpr, SymbolicToken.ADD, SymbolicToken.MullExpr },
			(List<ScannedToken> tokens) => {
				if (debugParseOutput) Debug.Log("Rule applied: (AddExpr -> AddExpr ADD MullExpr)");
				object d1 = tokens[0].GetData();
				object d2 = tokens[2].GetData();
				float f1 = System.Convert.ToSingle(d1);
				float f2 =  System.Convert.ToSingle(d2);
				return f1 + f2;
				}
			),
		new GrammarRule(SymbolicToken.AddExpr, new SymbolicToken[] { SymbolicToken.AddExpr, SymbolicToken.SUB, SymbolicToken.MullExpr },
			(List<ScannedToken> tokens) => {
				if (debugParseOutput) Debug.Log("Rule applied: (AddExpr -> AddExpr SUB MullExpr)");
				object d1 = tokens[0].GetData();
				object d2 = tokens[2].GetData();
				float f1 = System.Convert.ToSingle(d1);
				float f2 =  System.Convert.ToSingle(d2);
				return f1 - f2;
				}
			),

		new GrammarRule(SymbolicToken.MullExpr, new SymbolicToken[] { SymbolicToken.Factor },
			(List<ScannedToken> tokens) => {
				if (debugParseOutput) Debug.Log("Rule applied: (MullExpr -> Factor)");
				return tokens[0].GetData();
				}
			),
		new GrammarRule(SymbolicToken.MullExpr, new SymbolicToken[] { SymbolicToken.MullExpr, SymbolicToken.MUL, SymbolicToken.Factor },
			(List<ScannedToken> tokens) => {
				if (debugParseOutput) Debug.Log("Rule applied: (MullExpr -> MullExpr MUL Factor)");
				object d1 = tokens[0].GetData();
				object d2 = tokens[2].GetData();
				float f1 = System.Convert.ToSingle(d1);
				float f2 =  System.Convert.ToSingle(d2);
				return f1 * f2;
				}
			),
		new GrammarRule(SymbolicToken.MullExpr, new SymbolicToken[] { SymbolicToken.MullExpr, SymbolicToken.DIV, SymbolicToken.Factor },
			(List<ScannedToken> tokens) => {
				if (debugParseOutput) Debug.Log("Rule applied: (MullExpr -> MullExpr SUB Factor)");
				object d1 = tokens[0].GetData();
				object d2 = tokens[2].GetData();
				float f1 = System.Convert.ToSingle(d1);
				float f2 =  System.Convert.ToSingle(d2);
				return f1 / f2;
				}
			),

		new GrammarRule(SymbolicToken.Factor, new SymbolicToken[] { SymbolicToken.Variable },
			(List<ScannedToken> tokens) => {
				if (debugParseOutput) Debug.Log("Rule applied: (Factor -> Variable)");
				return tokens[0].GetData();
				}
			),
		new GrammarRule(SymbolicToken.Factor, new SymbolicToken[] { SymbolicToken.NumConstant },
			(List<ScannedToken> tokens) => {
				if (debugParseOutput) Debug.Log("Rule applied: (Factor -> NumConstant)");
				return tokens[0].GetData();
				}
			),
        //new GrammarRule(SymbolicToken.Factor, new SymbolicToken[] { SymbolicToken.IDENTIFIER, SymbolicToken.LEFTPAREN, SymbolicToken.RIGHTPAREN } ), // Factor provided by exterior method call.
        new GrammarRule(SymbolicToken.Factor, new SymbolicToken[] { SymbolicToken.LEFTPAREN, SymbolicToken.Expr, SymbolicToken.RIGHTPAREN },
			(List<ScannedToken> tokens) => {
				if (debugParseOutput) Debug.Log("Rule applied: (Factor -> LEFTPAREN Expr RIGHTPAREN)");
				return tokens[1].GetData();
				}
			),

		new GrammarRule(SymbolicToken.VariableDeclaration, new SymbolicToken[] { SymbolicToken.VAR, SymbolicToken.IDENTIFIER, SymbolicToken.SEMICOLON},
			(List<ScannedToken> tokens) => {
				if (debugParseOutput) Debug.Log("Rule applied: (VariableDeclaration -> VAR IDENTIFIER)");
				string identifier = (string)tokens[1].GetData(); // Get the string from identifier
				variableDictionary.Add(identifier, null); // Log a spot for this identifier
				return identifier; // Pass this name upwards
				}
			),
		new GrammarRule(SymbolicToken.VariableDeclaration, new SymbolicToken[] { SymbolicToken.VAR, SymbolicToken.IDENTIFIER, SymbolicToken.LEFTSQR, SymbolicToken.Factor, SymbolicToken.RIGHTSQR, SymbolicToken.SEMICOLON},
			(List<ScannedToken> tokens) => {
				if (debugParseOutput) Debug.Log("Rule applied: (VariableDeclaration -> VAR IDENTIFIER LEFTSQR INTCON RIGHTSQR)");
				string identifier = (string)tokens[2].GetData(); // Get the string from identifier
				variableDictionary.Add(identifier, new List<object>((int)tokens[3].GetData())); // Log a spot for this identifier
				return identifier; // Pass this name upwards
				}
			),

		new GrammarRule(SymbolicToken.Variable, new SymbolicToken[] { SymbolicToken.IDENTIFIER },
			(List<ScannedToken> tokens) => {
				if (debugParseOutput) Debug.Log("Rule applied: (Variable -> IDENTIFIER)");
				string iden = (string)tokens[0].GetData();
				return variableDictionary[iden];
				}
			),
        new GrammarRule(SymbolicToken.Variable, new SymbolicToken[] { SymbolicToken.IDENTIFIER, SymbolicToken.LEFTSQR, SymbolicToken.Expr, SymbolicToken.RIGHTSQR },
			(List<ScannedToken> tokens) => {
				if (debugParseOutput) Debug.Log("Rule applied: (Variable -> IDENTIFIER LEFTSQR Expr RIGHTSQR)");
				string iden = (string)tokens[0].GetData();
				int loc = (int)tokens[2].GetData();
				return ((List<object>)variableDictionary[iden])[loc];
				}
			),

		new GrammarRule(SymbolicToken.NumConstant, new SymbolicToken[] { SymbolicToken.INTCON },
			(List<ScannedToken> tokens) => {
				if (debugParseOutput) Debug.Log("Rule applied: (NumConstant -> INTCON)");
				return tokens[0].GetData();
				}
			),
        new GrammarRule(SymbolicToken.NumConstant, new SymbolicToken[] { SymbolicToken.FLOATCON },
			(List<ScannedToken> tokens) => {
				if (debugParseOutput) Debug.Log("Rule applied: (NumConstant -> FLOATCON)");
				return tokens[0].GetData();
				}
			),

		new GrammarRule(SymbolicToken.Timing, new SymbolicToken[] { SymbolicToken.IMMEDIATE } ),
		new GrammarRule(SymbolicToken.Timing, new SymbolicToken[] { SymbolicToken.DELAYED } ),
		new GrammarRule(SymbolicToken.Timing, new SymbolicToken[] { SymbolicToken.STATIC } ),

		new GrammarRule(SymbolicToken.Enforcement, new SymbolicToken[] { SymbolicToken.OPTIONAL } ),
		new GrammarRule(SymbolicToken.Enforcement, new SymbolicToken[] { SymbolicToken.REQUIRED } ),

		new GrammarRule(SymbolicToken.Type, new SymbolicToken[] { SymbolicToken.ITEM }),
		new GrammarRule(SymbolicToken.Type, new SymbolicToken[] { SymbolicToken.ROOM }),
		new GrammarRule(SymbolicToken.Type, new SymbolicToken[] { SymbolicToken.EVENT }),
		new GrammarRule(SymbolicToken.Type, new SymbolicToken[] { SymbolicToken.ARTIFACT })
	};
	#endregion

	#region Methods
	/// <summary>
	/// Scans a string into a token stream
	/// </summary>
	/// <param name="input"></param>
	/// <returns></returns>
	public static List<ScannedToken> Scan(string input) {
		List<ScannedToken> tokens = new List<ScannedToken>();
		string remainder = input;
		string text = "";
		int nl = 0;
		Regex regex;
		while (remainder.Length > 0) {

			SymbolicToken token = SymbolicToken.ERR;

			for (int i = 0; i < terminalTokenRegexPair.Length; i++) {
				text = "";
				string testingText = ""; // Track the current testing string.
				regex = new Regex(terminalTokenRegexPair[i].regex);
				for (int k = 0; k < remainder.Length; k++) {
					testingText += remainder[k];
					//Debug.Log(testingText + "<=?=>" + CSL.terminalTokenRegexPair[i].regex);
					//yield return new WaitForEndOfFrame();
					if (regex.IsMatch(testingText)) {
						text = testingText; // Store our string since we know that this is the valid string.
						token = terminalTokenRegexPair[i].token;
						//Debug.Log("Match Found: " + testingText + "<=?=>" + terminalTokenRegexPair[i].regex + " | " + token);
						//break;
					}
				}

				if (token != SymbolicToken.ERR) {
					//Debug.Log("Breaking.");
					break;
				}
			}


			if (token != SymbolicToken.ERR) {

				if (token == SymbolicToken.WHITESP) {
					//nothing
				}
				else if (token == SymbolicToken.NEWLN) {
					nl++;
				}
				else if (token == SymbolicToken.COMMENT) {
					//Dont add the comments.
				}
				else {
					//Debug.Log(text + " | " + System.Enum.GetName(typeof(SymbolicToken), token));
					tokens.Add(new ScannedToken(token, Box(token, text), ScannedToken.Type.Terminal));
				}
				//Debug.Log("Pre<"+remainder+">");
				remainder = remainder.Substring(text.Length);
				//Debug.Log("Post<" + remainder + ">");
				text = "";
			}
			else {
				//Log ERROR
				Debug.LogError("Syntax Read Error: '" + text + "'");
			}
		}
		tokens.Add(new ScannedToken(SymbolicToken.EOF, null, ScannedToken.Type.Terminal));
		return tokens;
	}

	/// <summary>
	/// Operates on a Token stream using the ParsingTable
	/// </summary>
	/// <param name="tokens"></param>
	/// <param name="generateScript"></param>
	/// <returns></returns>
	public static IEnumerator<object> Execute(List<ScannedToken> tokens) {
		//If we do not have a parsing table generated, do so.
		if (ParseTable == null)
			yield return BuildParseTable();

		variableDictionary = new Dictionary<string, object>(); // Reset the variable dictionary

		//The parsing stack
		List<ParseStackElement> parsingStack = new List<ParseStackElement>();
		PrintList(tokens);

		//Place state 0 onto the stack
		parsingStack.Add(new ParseStackElement(ParseStackElement.Type.State, 0));

		//The current location in the parse
		int currencyIndicator = 0;
		
		//Begin the Looop!
		while (true) {
			yield return null;

			
			//Debug.Log(PrintList(parsingStack, false) + "|\\|/| -> " + tokens[currencyIndicator]);

			ParseStackElement top = parsingStack[parsingStack.Count -1];
			//Look at top of stack, if it is a Token, then we need to do a GoTo,
			if (top.type == ParseStackElement.Type.Token) {
				//Debug.Log("GoTo");

				//DO A GOTO
				ParseStackElement previousState = parsingStack[parsingStack.Count - 2];

				//Debug.Log(previousState);
				if (previousState.type != ParseStackElement.Type.State) {
					Debug.LogError("Stack parsing has broken due to invalid GoTo");
					yield break;
				}

				//Calculate the column for the Token that is on top of the stack.
				//Debug.Log("Top " + System.Enum.GetName(typeof(SymbolicToken), top.scannedToken.token));
				//Debug.Log("nT index " + nonTerminals.FindIndex(x => x.Equals(top.scannedToken)));
				//Debug.Log(top.scannedToken);
				int tokenColumn = TERMINALS.Count + nonTerminals.FindIndex(x => x.Equals(top.scannedToken.token));
				//Debug.Log(tokenColumn);
				//Debug.Log(ParseTable[previousState.parserStateIndex][tokenColumn].value);

				//Push onto the stack the state we are supposed to GoTo.
				parsingStack.Add(new ParseStackElement(ParseStackElement.Type.State, ParseTable[previousState.parserStateIndex][tokenColumn].value) );

			}
			else {
				//Otherwise we look at the current state and the currency indiciator
				
				//Get the input token we are looking at
				ScannedToken currentInputToken = tokens[currencyIndicator];

				//It can (should) only be a non terminal, so calculate its column on the parse table.
				int tokenColumn = TERMINALS.FindIndex(x => x.Equals(currentInputToken.token));
				//Debug.Log(currentInputToken.token);
				//Debug.Log(tokenColumn+"/"+(nonTerminals.Count+TERMINALS.Count));
				ParserInstruction targetInstruction = ParseTable[top.parserStateIndex][tokenColumn];

				if (targetInstruction.instruction == ParserInstruction.Instruction.ACCEPT) {
					Debug.Log(parsingStack[parsingStack.Count - 2].scannedToken.GetData() ?? "Execution completed with no debug output.");
					break;
				}
				else if (targetInstruction.instruction == ParserInstruction.Instruction.SHIFT) {
					//Debug.Log("Shift");
					//If it is a shift, we put this token on the stack, 
					parsingStack.Add(new ParseStackElement(ParseStackElement.Type.Token, 0, currentInputToken));

					//advance the currency token,
					currencyIndicator++;

					//then the new state on the stack.
					parsingStack.Add(new ParseStackElement(ParseStackElement.Type.State, targetInstruction.value));

				}
				else if (targetInstruction.instruction == ParserInstruction.Instruction.REDUCE) {
					//Debug.Log("Reduce");
					//If it is reduce, we... reduce
					int removalCount = productionRules[targetInstruction.value].tokens.Length * 2;
					//We pop off twice as many tokens as the rhs of the reduce instruction.
					List<ParseStackElement> poppedList = parsingStack.GetRange(parsingStack.Count - removalCount, removalCount);
					parsingStack.RemoveRange(parsingStack.Count - removalCount, removalCount);

					//Create a list of all of the ScannedTokens
					List<ScannedToken> scannedTokensList = new List<ScannedToken>();
					for (int i = 0; i < poppedList.Count; i++) {
						if (poppedList[i].type == ParseStackElement.Type.Token)
							scannedTokensList.Add(poppedList[i].scannedToken);
					}

					//Call the appropriate operation (Performi the Reduction)
					ScannedToken resultingToken = new ScannedToken(productionRules[targetInstruction.value].nonTerminal, scannedTokensList, ScannedToken.Type.NonTerminal, targetInstruction.value);
					//Debug.Log(resultingToken);
					//Put the resulting LHS token onto the stack.
					parsingStack.Add(new ParseStackElement(ParseStackElement.Type.Token, 0, resultingToken));
				}
				else {
					PrintList(parsingStack);
					Debug.Log(currentInputToken);
					tokens.RemoveRange(0, currencyIndicator);
					PrintList(tokens);
					Debug.LogError("Stack parsing has broken due to invalid Instruction: " + System.Enum.GetName(typeof(ParserInstruction.Instruction), targetInstruction.instruction));
					yield break;
				}

			}

		}

	}

	/// <summary>
	/// Prints a formatted list of items using its ToString method
	/// </summary>
	/// <typeparam name="T"></typeparam>
	/// <param name="list"></param>
	public static string PrintList<T>(List<T> list, bool printList = true) {
		string ret = "";
		for (int i = 0; i < list.Count; i++) {
			ret += list[i].ToString() + " | ";
		}

		if (printList) Debug.Log(ret);
		return ret;
	}

	/// <summary>
	/// Builds a ParseTable for the Grammar
	/// </summary>
	/// <returns></returns>
    public static IEnumerator<object> BuildParseTable() {
		if (System.Enum.GetNames(typeof(SymbolicToken)).Length != nonTerminals.Count + TERMINALS.Count) {
			//Debug.LogError("Count mismatch between total SymbolicTokens ("+ System.Enum.GetNames(typeof(SymbolicToken)).Length + ") and Terminals("+ TERMINALS.Count+")/NonTerminals("+ nonTerminals.Count+").");
			//yield break;
		}


		/*{
			//Follow Set storage
			//Create the first and follow set for each non termial
			foreach (SymbolicToken nT in nonTerminals) {
				//Create first set
				firstSet.Add(nT, new List<SymbolicToken>());
				//create follow set
				followSet.Add(nT, new List<SymbolicToken>());
				//for each Rule featuring this token as the NonTerminal, add the first token in tokens to the First set
				foreach (GrammarRule rule in productionRules)
					if (rule.nonTerminal == nT)
						if (firstSet[nT].Contains(rule.tokens[0]) == false) firstSet[nT].Add(rule.tokens[0]);
				//Check each rule, and for each instance of this nonTerminal, record what follows immediatly after it.
				foreach (GrammarRule rule in productionRules) {
					for (int i = rule.tokens.Length - 1; i >= 0; i--) {
						if (rule.tokens[i] == nT) {
							//Dont add EOF
							if (i == rule.tokens.Length - 1)
								continue;
							//Since we are looping backwards and skip EOF, we know that there exists a token at this spot + 1 
							if (followSet[nT].Contains(rule.tokens[i + 1]) == false) followSet[nT].Add(rule.tokens[i + 1]);
						}
					}
				}
				//Debug.Log(System.Enum.GetName(typeof(Token), nT) + " first: " + PrintList(firstSet[nT]));
				//Debug.Log(System.Enum.GetName(typeof(Token), nT) + " follow: " + PrintList(followSet[nT]));
			}


		}*/


		int stateIter = 0;
		ParseTable = new List<ParserInstruction[]>();

		List<ParserState> stateList = new List<ParserState>();
		stateList.Add(new ParserState(new ParserState.StateRule[] { new ParserState.StateRule(0, productionRules[0])}));
		ParserState.count++; //Count the init state.

		while (stateIter < stateList.Count) {
			//TODO, start a row for this parser state.
			//Create states for our leads.
			//Debug.Log(stateList[stateIter].ToString());
			//Debug.Log(stateList[stateIter].TransitionString());

			ParserInstruction[] parseTableRow = GenerateEmptyRow(TERMINALS.Count + nonTerminals.Count);

			foreach (SymbolicToken key in stateList[stateIter].transitionSet.Keys) {
				//if (key == SymbolicToken.Type) PrintList(stateList[stateIter].transitionSet[key]);
				//Do not do anything if this leads nowhere.
				if (stateList[stateIter].transitionSet[key].Count == 0)
					continue;

				//Check if this state matches an already exiting one.
				ParserState newState = new ParserState(stateList[stateIter].transitionSet[key].ToArray());
				if (stateList.Contains(newState) == false) {
					//Add it
					newState.ID = ParserState.count++;
					stateList.Add(newState);
					//TODO build row
				}
				else {
					//TODO, Make this (toExapnd) row link back to the old matching set.
					newState = stateList.Find(x => x.Equals(newState));
					//Debug.LogWarning("Discarding redundant generated state.");
				}

				int column = (TERMINALS.Contains(key) == true) ? TERMINALS.IndexOf(key) : TERMINALS.Count + nonTerminals.IndexOf(key);
				ParserInstruction.Instruction instruction = (nonTerminals.Contains(key)) ? ParserInstruction.Instruction.GOTO : ParserInstruction.Instruction.SHIFT;
				parseTableRow[column] = new ParserInstruction(newState.ID, instruction);

			}

			//For each rule that has ended, add a reduce or goto
			for (int i = 0; i < stateList[stateIter].rules.Count; i++) {
				ParserState.StateRule sRule = stateList[stateIter].rules[i];
				if (sRule.dotPosition >= sRule.rule.tokens.Length) {
					if (sRule.rule.Equals(productionRules[0])) {
						//Add Accept token at EOF
						parseTableRow[TERMINALS.Count - 1] = new ParserInstruction(0, ParserInstruction.Instruction.ACCEPT);
						continue;
					}
					for (int k = 0; k < TERMINALS.Count; k++) {
						ParserInstruction newRule = new ParserInstruction(GetRuleIndex(sRule.rule), ParserInstruction.Instruction.REDUCE);
							//Debug.LogError("Parsing Conflict. ("+ parseTableRow[k] + ")->("+ newRule + ")");
						if (parseTableRow[k].instruction == ParserInstruction.Instruction.ERR)
							parseTableRow[k] = newRule;
					}
				}
			}


			ParseTable.Add(parseTableRow);
			yield return null;
			//Debug.Log(stateIter);
			stateIter++;
		}
		/*
		Debug.Log("!!!________!!!!_________!!!");
		int sCount = 0;
		foreach (ParserState p in stateList)
			Debug.Log("S" + (sCount++) + ": " + p.ToString());

		Debug.Log(GetTableString(parseTable));*/
		WriteTableOut(ParseTable);

	}

	/// <summary>
	/// Returns the index of a specific rule
	/// </summary>
	/// <param name="rule"></param>
	/// <returns></returns>
	private static int GetRuleIndex(GrammarRule rule) {
		for (int i = 0; i < productionRules.Length; i++) {
			if (productionRules[i].Equals(rule))
				return i;
		}
		return -1;
	}

	/// <summary>
	/// Builds a string representation of the ParsingTable
	/// </summary>
	/// <param name="table"></param>
	/// <returns></returns>
	private static string GetTableString(List<ParserInstruction[]> table) {
		string ret = "\t";
		for (int i = 0; i < TERMINALS.Count; i++) {
			ret += "" + System.Enum.GetName(typeof(SymbolicToken), TERMINALS[i]) + "\t";
		}

		for (int i = 0; i < nonTerminals.Count; i++) {
			ret += "" + System.Enum.GetName(typeof(SymbolicToken), nonTerminals[i]) + "\t";
		}

		ret += "\n";

		for (int i = 0; i < table.Count; i++) {
			ret += "S" + i + ":\t";
			for (int k = 0; k < table[i].Length; k++) {
				ret += table[i][k].ToString() + "\t";
			}
			ret += "\n";
		}

		return ret;
	}

	/// <summary>
	/// Writes the ParsingTable out to a file
	/// </summary>
	/// <param name="table"></param>
	private static void WriteTableOut(List<ParserInstruction[]> table) {
		string ret = ",";
		for (int i = 0; i < TERMINALS.Count; i++) {
			ret += "" + System.Enum.GetName(typeof(SymbolicToken), TERMINALS[i]) + ",";
		}

		for (int i = 0; i < nonTerminals.Count; i++) {
			ret += "" + System.Enum.GetName(typeof(SymbolicToken), nonTerminals[i]) + ",";
		}

		ret += "\n";

		for (int i = 0; i < table.Count; i++) {
			ret += "S" + i + ":,";
			for (int k = 0; k < table[i].Length; k++) {
				ret += table[i][k].ToString() + ",";
			}
			ret += "\n";
		}
		System.IO.File.WriteAllText(Application.persistentDataPath + "/BuiltTable.csv", ret);
	}

	/// <summary>
	/// Generates a row of empty instructions (ERR default)
	/// </summary>
	/// <param name="count"></param>
	/// <returns></returns>
	private static ParserInstruction[] GenerateEmptyRow(int count) {
		ParserInstruction[] instructions = new ParserInstruction[count];
		for (int i = 0; i < instructions.Length; i++) {
			instructions[i] = new ParserInstruction(0, ParserInstruction.Instruction.ERR);
		}

		return instructions;
	}

	/// <summary>
	/// Used to convert a string of text to the appropriate data based on the token type
	/// </summary>
	/// <param name="token"></param>
	/// <param name="text"></param>
	/// <returns></returns>
	public static object Box(SymbolicToken token, string text) {
		//Debug.Log(token + " | " + text);
		//switch (token) {
		//	case SymbolicToken.INTCON:
		//		return (object)System.Convert.ToInt32(text);
		//	case SymbolicToken.STRINGCON:
		//		string retString = text.Substring(1,text.Length-1);
		//		return (object)retString;
		//	default:
				return (object)text;
		//}
	}

	/// <summary>
	/// Gets all of the production rules with the NonTerminal token on the LHS
	/// </summary>
	/// <param name="token"></param>
	/// <returns></returns>
	private static List<GrammarRule> GetRules(SymbolicToken token) {
		List<GrammarRule> rules = new List<GrammarRule>();
		for (int i = 0; i < productionRules.Length; i++) {
			if (productionRules[i].nonTerminal == token)
				rules.Add(productionRules[i]);
		}

		return rules;
	}
	#endregion

	#region Structs
	/// <summary>
	/// A struct used for tracking both Tokens and States in the parsing process
	/// </summary>
	public struct ParseStackElement {
		public enum Type { Token, State }

		public Type type;
		public int parserStateIndex;
		public ScannedToken scannedToken;


		public ParseStackElement(Type type, int parserState, ScannedToken token = default(ScannedToken)) {
			this.type = type;
			this.parserStateIndex = parserState;
			this.scannedToken = token;
		}

		public override string ToString() {
			return (type == Type.State) ? "State" + parserStateIndex : scannedToken.ToString();
		}

	}

	/// <summary>
	/// A self constructing parsing state based on some initial rules
	/// </summary>
	private class ParserState {
		/// <summary>
		/// Internat count of the number of states. 
		/// </summary>
		public static int count = 0;

		/// <summary>
		/// The ID of this state.
		/// </summary>
		public int ID;

		public Dictionary<SymbolicToken, List<StateRule>> transitionSet = new Dictionary<SymbolicToken, List<StateRule>>();
		public List<StateRule> rules = new List<StateRule>();

		public ParserState(StateRule[] initialRules) {


			//For each InitialRules
			//Add them to the rules list
			//Based on DOT position, if this rule has a NON terminal to the right of DOT, add those rules to this state.
			//Only if they have not been added before. (Research shows that it uses obj.Equals for this, so since those are overridden in to allow for State and Grammar rules to be equal.
			Queue<StateRule> toExpand = new Queue<StateRule>(initialRules);
			//Debug.Log("Creating new state with " + toExpand.Count + " starting rules.");
			//Debug.Log(initialRules[0]);
			while (toExpand.Count > 0) { 
				StateRule targetRule = toExpand.Dequeue();

				rules.Add(targetRule);
				//So on a transition we consume a token, so when adding to our transition set make sure to advance the dot one space.
				if (targetRule.dotPosition >= targetRule.rule.tokens.Length) {
					continue;
				}
				else {

				}
				SymbolicToken dotTarget = targetRule.rule.tokens[targetRule.dotPosition];

				if (transitionSet.ContainsKey(dotTarget) == false)
					transitionSet.Add(dotTarget, new List<StateRule>());
				transitionSet[dotTarget].Add(new StateRule(targetRule.dotPosition + 1, targetRule.rule));





				//Check to see if dotPosition is a nonTerminal
				if (nonTerminals.Contains(dotTarget)) {
					//Debug.Log(System.Enum.GetName(typeof(SymbolicToken), dotTarget));

					//If it is, add the necessary production rules.
					List<GrammarRule> tokenRules = GetRules(dotTarget);
					//Debug.Log(tokenRules.Count);
					foreach (GrammarRule r in tokenRules) {
						StateRule nR = new StateRule(0, r);
						if (rules.Contains(nR) == false && toExpand.Contains(nR) == false) {
							toExpand.Enqueue(nR);
						}
					}
				}
			}
				
		}

		public string TransitionString() {
			string ret = "";
			foreach (SymbolicToken token in transitionSet.Keys) {
				ret += System.Enum.GetName(typeof(SymbolicToken), token) + " --> \n";
				foreach (StateRule r in transitionSet[token]) {
					ret += "\t\t " + r.ToString() + "\n";
				}

				ret += "\n--------------\n";
			}

			return ret;
		}

		public override string ToString() {
			string ret = "{\n";
			for (int i = 0; i < rules.Count; i++) {
				ret += rules[i].ToString() + "\n";
			}
			ret += "}";
			return ret;
		}

		public override int GetHashCode() {
			return ToString().GetHashCode();
		}

		public override bool Equals(object obj) {
			//We need to a comparison where as long as the set is the same we are good, but the order doesnt matter
			//Sort?
			//Dynamic Comparison?
			if (obj.GetType() == typeof(ParserState)) {
				ParserState converted = (ParserState)obj;

				//We do not match if we have a different number of rules.
				if (converted.rules.Count != this.rules.Count)
					return false;

				//For each of our rules (x), there is a matching rule in the other objects. (Containment checks Equality)
				return this.rules.TrueForAll(x => converted.rules.Contains(x));
			}
			else {
				return false;
			}

		}

		/// <summary>
		/// A rule that is in the process of having a match found.
		/// </summary>
		public struct StateRule {
			/// <summary>
			/// The position in this state rule representing our progress towards finding a match for this rule.
			/// </summary>
			public int dotPosition;
			/// <summary>
			/// The rule we are attempting to make a match for.
			/// </summary>
			public GrammarRule rule;
			
			public StateRule(int dotPosition, GrammarRule rule) {
				this.dotPosition = dotPosition;
				this.rule = rule;
			}

			public override int GetHashCode() {
				return ToString().GetHashCode();
			}

			public override bool Equals(object obj) {
				if (obj.GetType() == typeof(StateRule)) {
					StateRule converted = (StateRule)obj;
					return converted.dotPosition == this.dotPosition && converted.rule.Equals(this.rule);
				}
				/*else if (obj.GetType() == typeof(GrammarRule)) {
					GrammarRule converted = (GrammarRule)obj;
					return converted.Equals(this.rule);
				}*/
				else {
					return false;
				}
			}

			public override string ToString() {
				string ret = "";
				ret += System.Enum.GetName(typeof(SymbolicToken), rule.nonTerminal);
				ret += " -> ";
				for (int i = 0; i < rule.tokens.Length; i++) {
					if (i == dotPosition)
						ret += "•";
					ret += System.Enum.GetName(typeof(SymbolicToken), rule.tokens[i]);
					ret += " ";
				}
				if (rule.tokens.Length == dotPosition)
					ret += "•";
				return ret;
			}
		}
	}

	/// <summary>
	/// A grammar production rule. Maps a NonTerminal SymbolicToken to a sequence of other SymbolicTokens.
	/// </summary>
	public struct GrammarRule {
		/// <summary>
		/// The LeftHandSide NonTerminal that is produced by this rule.
		/// </summary>
		public SymbolicToken nonTerminal;

		/// <summary>
		/// The list of RightHandSide SymbolicTokens that are required to produce the NonTerminal
		/// </summary>
		public SymbolicToken[] tokens;

		/// <summary>
		/// A frame for Rule Reduction Operations.
		/// </summary>
		/// <param name="parsedTokens"></param>
		/// <param name="script"></param>
		/// <returns></returns>
		public delegate object Operate(List<ScannedToken> parsedTokens);

		/// <summary>
		/// The operation to perfrom when this rule is used.
		/// </summary>
		public Operate operate;

		public GrammarRule(SymbolicToken nonTerminal, SymbolicToken[] tokens, Operate operate = null) {
			this.nonTerminal = nonTerminal;
			this.tokens = tokens;
			//Set it to the passed operation otherwise use the default statement. the Default statement mentions the default rule was used and shows the form for the rule
			this.operate = operate ?? 
				((List<ScannedToken> parsedTokens) => {
					string ret = "";
					ret += System.Enum.GetName(typeof(SymbolicToken), nonTerminal);
					ret += " -> ";
					for (int i = 0; i < tokens.Length; i++) {
						ret += System.Enum.GetName(typeof(SymbolicToken), tokens[i]);
						ret += " ";
					}
					Debug.Log("Default rule applied for (" + ret + ")");
					return null;
				});
		}

		public override int GetHashCode() {
			return ToString().GetHashCode();
		}

		public override bool Equals(object obj) {
			if (obj.GetType() == typeof(GrammarRule)) {
				GrammarRule converted = (GrammarRule)obj;
				//We already know that this cannot be equal if the lhs is different or if they do not have the same length of rhs tokens 
				if (converted.nonTerminal != this.nonTerminal || converted.tokens.Length != this.tokens.Length)
					return false;

				//Assume true until we find a mismatch
				bool sameFlag = true;
				for (int i = 0; i < this.tokens.Length; i++) {
					if (this.tokens[i] != converted.tokens[i]) {
						sameFlag = false;
						break;
					}
				}

				return sameFlag;
			}
			else {
				return false;
			}
		}

		public override string ToString() {
			string ret = "";
			ret += System.Enum.GetName(typeof(SymbolicToken), nonTerminal);
			ret += " -> ";
			for (int i = 0; i < tokens.Length; i++) {
				ret += System.Enum.GetName(typeof(SymbolicToken), tokens[i]);
				ret += " ";
			}
			return ret;
		}
	}

	/// <summary>
	/// An instruction for the Parse table to store.
	/// </summary>
	public struct ParserInstruction {
		public enum Instruction { SHIFT, REDUCE, GOTO, ERR, ACCEPT }
		public Instruction instruction;
		public int value;
		public ParserInstruction(int value, Instruction instruction) {
			this.value = value;
			this.instruction = instruction;
		}

		public override string ToString() {
			switch (instruction) {
				case Instruction.SHIFT:
					return "s" + value;
				case Instruction.REDUCE:
					return "r" + value;
				case Instruction.GOTO:
					return "" + value;
				case Instruction.ERR:
					return "";
				case Instruction.ACCEPT:
					return "!";
				default:
					return "";
			}
		}
	}

	/// <summary>
	/// A SymbolicToken and its related Regex String
	/// </summary>
	public struct TokenRegexPair {
		public SymbolicToken token;
		public string sample;
		public string regex;
		public TokenRegexPair(SymbolicToken token, string sample, string regex) {
			this.token = token;
			this.regex = regex;
			this.sample = sample;
		} 
	}

	/// <summary>
	/// A parsed Script
	/// </summary>
	public class Script {

		private static string defaultSavePath = Application.persistentDataPath + "/Scripts/";
		private static string fileExtension = ".xml";

		/// <summary>
		/// Types of supported scripts
		/// </summary>
		public enum ScriptType { Room, Item, Artifact, Event, Effect }

		/// <summary>
		/// Timings for choices and effects
		/// </summary>
		public enum Timing { Immediate, Delayed, Static }

		/// <summary>
		/// The enforcement level for a choice
		/// </summary>
		public enum Enforcement { Optional, Required }

		/// <summary>
		/// The type of card this is.
		/// </summary>
		public ScriptType type;

		/// <summary>
		/// The name for this card
		/// </summary>
		public string name;

		/// <summary>
		/// The base image for this card
		/// </summary>
		public Sprite image;

		/// <summary>
		/// Aditional icons to place on the card. Will get assumed based on CSL statements
		/// </summary>
		public List<Sprite> additionalIcons;

		/// <summary>
		/// List of effects that this room applies
		/// </summary>
		public List<EffectSet> effectList;

		/// <summary>
		/// List of choices that can be made in this room
		/// </summary>
		public List<ChoiceSet> choiceList;

		/// <summary>
		/// Creates a unique UID for the filename
		/// </summary>
		/// <returns></returns>
		public bool TestForUniqueFilename(string filename, out string UID) {
			string filePath;
			int num = 0;
			do {
				num++;
				filePath = defaultSavePath + filename + ((num == 1) ? "" : "" + num) + fileExtension;
			} while (System.IO.File.Exists(filePath));
			UID = "";
			return num == 1;
		}

		/// <summary>
		/// Saves a card to the file base.
		/// </summary>
		public static void SaveCard(Script cardToSave, string filename) {
			XmlSerializer serializer = new XmlSerializer(typeof(Script));
			string filePath = defaultSavePath + filename + fileExtension;
			System.IO.TextWriter textWriter = new System.IO.StreamWriter(filePath);
			serializer.Serialize(textWriter, cardToSave);
		}

		/// <summary>
		/// Loads all cards from a using a filepath
		/// </summary>
		/// <param name="filename"></param>
		/// <returns></returns>
		public static Script LoadCard(string filePath) {
			XmlSerializer serializer = new XmlSerializer(typeof(Script));
			FileStream fs = new FileStream(filePath, FileMode.Open);
			Script script = (Script)serializer.Deserialize(fs);
			return script;
		}

		/// <summary>
		/// Loads all scripts from the scripts folder.
		/// </summary>
		/// <param name="dirPath"></param>
		/// <returns></returns>
		public static List<Script> LoadAllScripts(string dirPath) {
			List<string> filePaths = new List<string>(Directory.GetFiles(dirPath));
			List<Script> scripts = new List<Script>();

			for (int i = filePaths.Count - 1; i >= 0; i--) {
				string filePath = filePaths[i];
				if (filePath.Contains(fileExtension)) {
					scripts.Add(LoadCard(filePath));
					filePaths.RemoveAt(i);
				}
			}

			return scripts;
		}

		/// <summary>
		/// A player set of choices, with a level of Enforcemment.
		/// </summary>
		public struct ChoiceSet {
			/// <summary>
			/// Enfocement level, such as optional or required
			/// </summary>
			public Enforcement enforcement;
			
			/// <summary>
			/// The list of options to choose between
			/// </summary>
			public List<Choice> choices;

			public ChoiceSet(Enforcement enforcement, List<Choice> choices) {
				this.enforcement = enforcement;
				this.choices = choices;
			}
		}

		/// <summary>
		/// A set of effects that is optional to the player, with some requirements.
		/// </summary>
		public struct Choice {
			/// <summary>
			/// The name, or summary of this Choice. Ex: 'Run Away'
			/// </summary>
			public string name;
			
			/// <summary>
			/// The Statement that must result in true for this to be a valid choice for the player
			/// </summary>
			public List<ScannedToken> requirementTestStatement;

			/// <summary>
			/// The text displayed when this choice is chosen/completed.
			/// </summary>
			public string resultText;

			/// <summary>
			/// The effects of this choice upon selection.
			/// </summary>
			public List<EffectSet> resultEffectList;

			public Choice(string name, List<ScannedToken> requirementTestStatement, string resultText, List<EffectSet> resultEffectList) {
				this.name = name;
				this.requirementTestStatement = requirementTestStatement;
				this.resultText = resultText;
				this.resultEffectList = resultEffectList;
			}
		}

		/// <summary>
		/// A set of Effects, with a timing on when they happen
		/// </summary>
		public struct EffectSet {
			/// <summary>
			/// The timing on when this effect happens, such as immediate or recurring.
			/// </summary>
			public Timing timing;

			/// <summary>
			/// The list of effects to be applied.
			/// </summary>
			public List<Effect> effects;

			public EffectSet(Timing timing, List<Effect> effects) {
				this.timing = timing;
				this.effects = effects;
			}
		}

		/// <summary>
		/// A set of statements that have an effect on the game.
		/// </summary>
		public struct Effect {

			/// <summary>
			/// The tokens that generate an effect when parsed.
			/// </summary>
			public List<ScannedToken> tokens;
			public Effect(List<ScannedToken> tokens) {
				this.tokens = tokens;
			}
		}
	}

	/// <summary>
	/// A token that has been read by the scanner. Contains its SymbolicToken and it's parsed data.
	/// </summary>
	public struct ScannedToken {
		public enum Type { Terminal, NonTerminal }
		public List<ScannedToken> scannedTokens;
		public Type type;
		public SymbolicToken token;
		public int rule;
		private readonly object data;
		public ScannedToken(SymbolicToken token, object data, Type type, int rule = -1) {
			this.token = token;
			this.type = type;
			this.rule = rule;
			if (type == Type.NonTerminal) {
				this.data = null;
				this.scannedTokens = (List<ScannedToken>)data;
			}
			else {
				this.data = data;
				this.scannedTokens = null;
			}
			//Debug.Log("New Token:(" + System.Enum.GetName(typeof(SymbolicToken), token) + ") type:(" + System.Enum.GetName(typeof(Type), type) + ") rule:(" + rule + ") data:(" + ((type == Type.Terminal) ? data : PrintList(scannedTokens, false)) + ")");
		}

		public object GetData() {
			if (type == Type.Terminal) {
				return data;
			}
			else {
				return productionRules[rule].operate(scannedTokens);
			}
		}

		public string ToStringVerbose() {
			return "Token:(" + System.Enum.GetName(typeof(SymbolicToken), token)+ ") type:(" +System.Enum.GetName(typeof(Type),type)+") rule:("+rule+ ") data:("+ ((type == Type.Terminal) ? data : PrintList(scannedTokens, false)) + ")";
		}

		public override string ToString() {
			return type+"."+System.Enum.GetName(typeof(SymbolicToken), token);// "Token:(" + System.Enum.GetName(typeof(SymbolicToken), token)+ ") type:(" +System.Enum.GetName(typeof(Type),type)+") rule:("+rule+ ") data:("+ ((type == Type.Terminal) ? data : PrintList(scannedTokens, false)) + ")";
		}
	}
	#endregion
}