using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

/// <summary>
/// A grammer and rules specification for a Card Scripting Language
/// </summary>
public static class CSL {

	public enum ScriptType {Room, Item, Artifact, Event, Effect }
	public enum Timing {Immediate, Delayed, Static }
	public enum Enforcement { Optional, Required }

	private static List<ParserInstruction[]> ParseTable = null;

	#region Token
	/// <summary>
	/// The types of tokens interpreted by the system.
	/// </summary>
	public enum SymbolicToken {
		//NEVER REMOVE SCRIPTPRIME or EOF
		ScriptPrime, EOF, 


		//Non Temrinals
		 Script, EffectList, ChoiceList, EffectSet, ChoiceSet, Effect, Choice, StatementList, Statement,
		Assignment, IfStatement, TestStatement, Test, WhileStatement, IOStatement, DiscardStatement, CompoundStatement,
		Expr, SimpleExpr, AddExpr, MullExpr, Factor, Variable, Timing, Enforcement, NumConstant, Type, CompleteEffectSet, CompleteChoiceSet, ScriptHeading,

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
		SymbolicToken.CHOICEIDEN, SymbolicToken.EFFECTIDEN, SymbolicToken.DISCARD, SymbolicToken.THIS, SymbolicToken.DELAYED,
		SymbolicToken.RIGHTBRACE, SymbolicToken.RIGHTSQR, SymbolicToken.RIGHTPAREN, SymbolicToken.AND, SymbolicToken.OR, SymbolicToken.NOT, SymbolicToken.XOR, SymbolicToken.COMMA, SymbolicToken.PERIOD, SymbolicToken.EQUAL,
		SymbolicToken.NOTEQUAL, SymbolicToken.GREATEQUAL, SymbolicToken.LESSEQUAL, SymbolicToken.GREATTHAN, SymbolicToken.LESSTHAN, SymbolicToken.ASSIGN, SymbolicToken.ADD, SymbolicToken.SUB, SymbolicToken.DIV, SymbolicToken.MUL, SymbolicToken.SEMICOLON,
		SymbolicToken.IDENTIFIER, SymbolicToken.FLOATCON, SymbolicToken.STRINGCON, SymbolicToken.INTCON, SymbolicToken.ERR, SymbolicToken.IDENERR, SymbolicToken.EOF
	} );

	/// <summary>
	/// The set of NonTerminal SymbolicTokens
	/// </summary>
    static public List<SymbolicToken> nonTerminals = new List<SymbolicToken>( new SymbolicToken[] {
		SymbolicToken.ScriptPrime,
		SymbolicToken.Script, SymbolicToken.EffectList, SymbolicToken.ChoiceList, SymbolicToken.EffectSet, SymbolicToken.ChoiceSet, SymbolicToken.Effect, SymbolicToken.Choice, SymbolicToken.StatementList, SymbolicToken.Statement,
		SymbolicToken.Assignment, SymbolicToken.IfStatement, SymbolicToken.TestStatement, SymbolicToken.Test, SymbolicToken.WhileStatement, SymbolicToken.IOStatement, SymbolicToken.DiscardStatement, SymbolicToken.CompoundStatement,
		SymbolicToken.Expr, SymbolicToken.SimpleExpr, SymbolicToken.AddExpr, SymbolicToken.MullExpr, SymbolicToken.Factor, SymbolicToken.Variable, SymbolicToken.Timing, SymbolicToken.Enforcement, SymbolicToken.NumConstant, SymbolicToken.Type, SymbolicToken.CompleteEffectSet, SymbolicToken.CompleteChoiceSet, SymbolicToken.ScriptHeading
	} );
	#endregion

	#region Rules
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
	public static GrammarRule[] productionRules = new GrammarRule[] {
		new GrammarRule(SymbolicToken.ScriptPrime, new SymbolicToken[] { SymbolicToken.Script }),

        new GrammarRule(SymbolicToken.Script, new SymbolicToken[] { SymbolicToken.ScriptHeading }),
        new GrammarRule(SymbolicToken.Script, new SymbolicToken[] { SymbolicToken.ScriptHeading, SymbolicToken.EffectList }),
        new GrammarRule(SymbolicToken.Script, new SymbolicToken[] { SymbolicToken.ScriptHeading, SymbolicToken.ChoiceList }),
        new GrammarRule(SymbolicToken.Script, new SymbolicToken[] { SymbolicToken.ScriptHeading, SymbolicToken.EffectList, SymbolicToken.ChoiceList }),

		new GrammarRule(SymbolicToken.ScriptHeading, new SymbolicToken[] { SymbolicToken.Type, SymbolicToken.SEMICOLON, SymbolicToken.NAME, SymbolicToken.STRINGCON, SymbolicToken.SEMICOLON, SymbolicToken.IMAGE, SymbolicToken.SEMICOLON, SymbolicToken.TEXT, SymbolicToken.STRINGCON, SymbolicToken.SEMICOLON }),
		new GrammarRule(SymbolicToken.ScriptHeading, new SymbolicToken[] { SymbolicToken.Type, SymbolicToken.SEMICOLON, SymbolicToken.NAME, SymbolicToken.STRINGCON, SymbolicToken.SEMICOLON, SymbolicToken.IMAGE, SymbolicToken.STRINGCON, SymbolicToken.SEMICOLON, SymbolicToken.TEXT, SymbolicToken.STRINGCON, SymbolicToken.SEMICOLON }),

		new GrammarRule(SymbolicToken.EffectList, new SymbolicToken[] { SymbolicToken.CompleteEffectSet } ),
		new GrammarRule(SymbolicToken.EffectList, new SymbolicToken[] { SymbolicToken.EffectList, SymbolicToken.CompleteEffectSet } ),

		new GrammarRule(SymbolicToken.ChoiceList, new SymbolicToken[] { SymbolicToken.CompleteChoiceSet } ),
		new GrammarRule(SymbolicToken.ChoiceList, new SymbolicToken[] { SymbolicToken.ChoiceList, SymbolicToken.CompleteChoiceSet } ),

		new GrammarRule(SymbolicToken.CompleteEffectSet, new SymbolicToken[] { SymbolicToken.Timing, SymbolicToken.LEFTBRACE, SymbolicToken.EffectSet, SymbolicToken.RIGHTBRACE } ),


		new GrammarRule(SymbolicToken.EffectSet, new SymbolicToken[] { SymbolicToken.Effect} ),
		new GrammarRule(SymbolicToken.EffectSet, new SymbolicToken[] { SymbolicToken.EffectSet, SymbolicToken.Effect} ),

		new GrammarRule(SymbolicToken.CompleteChoiceSet, new SymbolicToken[] { SymbolicToken.Timing, SymbolicToken.Enforcement, SymbolicToken.LEFTBRACE, SymbolicToken.ChoiceSet, SymbolicToken.RIGHTBRACE } ),

		new GrammarRule(SymbolicToken.ChoiceSet, new SymbolicToken[] {  SymbolicToken.Choice } ),
		new GrammarRule(SymbolicToken.ChoiceSet, new SymbolicToken[] { SymbolicToken.ChoiceSet, SymbolicToken.Choice } ),

		new GrammarRule(SymbolicToken.Effect, new SymbolicToken[] { SymbolicToken.EFFECTIDEN, SymbolicToken.LEFTBRACE, SymbolicToken.StatementList, SymbolicToken.RIGHTBRACE } ),

		new GrammarRule(SymbolicToken.Choice, new SymbolicToken[] { SymbolicToken.CHOICEIDEN, SymbolicToken.LEFTBRACE, SymbolicToken.NAME, SymbolicToken.STRINGCON, SymbolicToken.SEMICOLON, SymbolicToken.Test, SymbolicToken.LEFTBRACE, SymbolicToken.TEXT, SymbolicToken.STRINGCON, SymbolicToken.SEMICOLON, SymbolicToken.EffectList, SymbolicToken.RIGHTBRACE, SymbolicToken.RIGHTBRACE } ),


		new GrammarRule(SymbolicToken.StatementList, new SymbolicToken[] { SymbolicToken.Statement } ),
        new GrammarRule(SymbolicToken.StatementList, new SymbolicToken[] { SymbolicToken.StatementList, SymbolicToken.Statement } ),

        new GrammarRule(SymbolicToken.Statement, new SymbolicToken[] { SymbolicToken.Assignment } ),
        new GrammarRule(SymbolicToken.Statement, new SymbolicToken[] { SymbolicToken.IfStatement } ),
        new GrammarRule(SymbolicToken.Statement, new SymbolicToken[] { SymbolicToken.WhileStatement } ),
        new GrammarRule(SymbolicToken.Statement, new SymbolicToken[] { SymbolicToken.IOStatement } ),
        new GrammarRule(SymbolicToken.Statement, new SymbolicToken[] { SymbolicToken.CompoundStatement } ),


        new GrammarRule(SymbolicToken.Assignment, new SymbolicToken[] { SymbolicToken.Variable, SymbolicToken.ASSIGN, SymbolicToken.Expr, SymbolicToken.SEMICOLON } ),

        new GrammarRule(SymbolicToken.IfStatement, new SymbolicToken[] { SymbolicToken.IF, SymbolicToken.TestStatement, SymbolicToken.ELSE, SymbolicToken.IfStatement } ),
        new GrammarRule(SymbolicToken.IfStatement, new SymbolicToken[] { SymbolicToken.IF, SymbolicToken.TestStatement, SymbolicToken.ELSE, SymbolicToken.CompoundStatement } ),
		new GrammarRule(SymbolicToken.IfStatement, new SymbolicToken[] { SymbolicToken.IF, SymbolicToken.TestStatement } ),


		new GrammarRule(SymbolicToken.TestStatement, new SymbolicToken[] { SymbolicToken.Test, SymbolicToken.CompoundStatement } ),

		new GrammarRule(SymbolicToken.Test, new SymbolicToken[] { SymbolicToken.LEFTPAREN, SymbolicToken.Expr, SymbolicToken.RIGHTPAREN } ),

		new GrammarRule(SymbolicToken.WhileStatement, new SymbolicToken[] { SymbolicToken.WHILE, SymbolicToken.Expr, SymbolicToken.Statement, SymbolicToken.SEMICOLON } ),

		new GrammarRule(SymbolicToken.IOStatement, new SymbolicToken[] { SymbolicToken.WHILE, SymbolicToken.Expr, SymbolicToken.Statement } ),

		new GrammarRule(SymbolicToken.DiscardStatement, new SymbolicToken[] {SymbolicToken.DISCARD, SymbolicToken.IDENTIFIER }),
		new GrammarRule(SymbolicToken.DiscardStatement, new SymbolicToken[] {SymbolicToken.DISCARD, SymbolicToken.THIS }),

		new GrammarRule(SymbolicToken.CompoundStatement, new SymbolicToken[] { SymbolicToken.LEFTBRACE, SymbolicToken.StatementList, SymbolicToken.RIGHTBRACE } ),

        new GrammarRule(SymbolicToken.Expr, new SymbolicToken[] { SymbolicToken.Expr, SymbolicToken.ADD, SymbolicToken.SimpleExpr } ),
        new GrammarRule(SymbolicToken.Expr, new SymbolicToken[] { SymbolicToken.Expr, SymbolicToken.OR, SymbolicToken.SimpleExpr } ),
        new GrammarRule(SymbolicToken.Expr, new SymbolicToken[] { SymbolicToken.SimpleExpr } ),
        new GrammarRule(SymbolicToken.Expr, new SymbolicToken[] { SymbolicToken.NOT, SymbolicToken.SimpleExpr } ),

        new GrammarRule(SymbolicToken.SimpleExpr, new SymbolicToken[] { SymbolicToken.SimpleExpr, SymbolicToken.EQUAL, SymbolicToken.AddExpr } ),
        new GrammarRule(SymbolicToken.SimpleExpr, new SymbolicToken[] { SymbolicToken.SimpleExpr, SymbolicToken.NOTEQUAL, SymbolicToken.AddExpr } ),
        new GrammarRule(SymbolicToken.SimpleExpr, new SymbolicToken[] { SymbolicToken.SimpleExpr, SymbolicToken.LESSEQUAL, SymbolicToken.AddExpr } ),
        new GrammarRule(SymbolicToken.SimpleExpr, new SymbolicToken[] { SymbolicToken.SimpleExpr, SymbolicToken.LESSTHAN, SymbolicToken.AddExpr } ),
        new GrammarRule(SymbolicToken.SimpleExpr, new SymbolicToken[] { SymbolicToken.SimpleExpr, SymbolicToken.GREATEQUAL, SymbolicToken.AddExpr } ),
        new GrammarRule(SymbolicToken.SimpleExpr, new SymbolicToken[] { SymbolicToken.SimpleExpr, SymbolicToken.GREATTHAN, SymbolicToken.AddExpr } ),
        new GrammarRule(SymbolicToken.SimpleExpr, new SymbolicToken[] { SymbolicToken.AddExpr } ),


        new GrammarRule(SymbolicToken.AddExpr, new SymbolicToken[] { SymbolicToken.AddExpr, SymbolicToken.ADD, SymbolicToken.AddExpr } ),
        new GrammarRule(SymbolicToken.AddExpr, new SymbolicToken[] { SymbolicToken.AddExpr, SymbolicToken.SUB, SymbolicToken.MullExpr } ),
        new GrammarRule(SymbolicToken.AddExpr, new SymbolicToken[] { SymbolicToken.MullExpr } ),

        new GrammarRule(SymbolicToken.MullExpr, new SymbolicToken[] { SymbolicToken.MullExpr, SymbolicToken.MUL, SymbolicToken.Factor } ),
        new GrammarRule(SymbolicToken.MullExpr, new SymbolicToken[] { SymbolicToken.MullExpr, SymbolicToken.SUB, SymbolicToken.Factor } ),
        new GrammarRule(SymbolicToken.MullExpr, new SymbolicToken[] { SymbolicToken.Factor } ),

        new GrammarRule(SymbolicToken.Factor, new SymbolicToken[] { SymbolicToken.Variable } ),
        new GrammarRule(SymbolicToken.Factor, new SymbolicToken[] { SymbolicToken.NumConstant } ),
        new GrammarRule(SymbolicToken.Factor, new SymbolicToken[] { SymbolicToken.IDENTIFIER, SymbolicToken.LEFTPAREN, SymbolicToken.RIGHTPAREN } ),
        new GrammarRule(SymbolicToken.Factor, new SymbolicToken[] { SymbolicToken.LEFTPAREN, SymbolicToken.Expr, SymbolicToken.RIGHTPAREN } ),

        new GrammarRule(SymbolicToken.Variable, new SymbolicToken[] { SymbolicToken.IDENTIFIER } ),
        new GrammarRule(SymbolicToken.Variable, new SymbolicToken[] { SymbolicToken.IDENTIFIER, SymbolicToken.LEFTSQR, SymbolicToken.Expr, SymbolicToken.RIGHTSQR } ),

		new GrammarRule(SymbolicToken.Timing, new SymbolicToken[] { SymbolicToken.IMMEDIATE } ),
		new GrammarRule(SymbolicToken.Timing, new SymbolicToken[] { SymbolicToken.DELAYED } ),
		new GrammarRule(SymbolicToken.Timing, new SymbolicToken[] { SymbolicToken.STATIC } ),

		new GrammarRule(SymbolicToken.Enforcement, new SymbolicToken[] { SymbolicToken.OPTIONAL } ),
		new GrammarRule(SymbolicToken.Enforcement, new SymbolicToken[] { SymbolicToken.REQUIRED } ),


		new GrammarRule(SymbolicToken.NumConstant, new SymbolicToken[] { SymbolicToken.INTCON } ),
        new GrammarRule(SymbolicToken.NumConstant, new SymbolicToken[] { SymbolicToken.FLOATCON } ),

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
				else {
					//Debug.Log(text + " | " + System.Enum.GetName(typeof(SymbolicToken), token));
					tokens.Add(new ScannedToken(token, Box(token, text)));
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
		tokens.Add(new ScannedToken(SymbolicToken.EOF, null));
		return tokens;
	}

	/// <summary>
	/// Operates on a Token stream using the ParsingTable
	/// </summary>
	/// <param name="tokens"></param>
	/// <param name="generateScript"></param>
	/// <returns></returns>
	public static IEnumerator<object> Parse(List<ScannedToken> tokens, bool generateScript = false) {
		//If we do not have a parsing table generated, do so.
		if (ParseTable == null)
			yield return BuildParseTable();

		//The parsing stack
		List<ParseStackElement> parsingStack = new List<ParseStackElement>();
		//PrintList(tokens);

		//Place state 0 onto the stack
		parsingStack.Add(new ParseStackElement(ParseStackElement.Type.State, 0));

		Script script = (generateScript) ? new Script() : null;

		//The current location in the parse
		int currencyIndicator = 0;
		
		//Begin the Looop!
		while (true) {
			yield return null;

			//PrintList(parsingStack);
			//Debug.Log(""+ tokens[currencyIndicator]);

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
					//Debug.Log("Parsing Accepted.");
					//Do something with the script here?
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

					//Call the appropriate operation (Perform the Reduction)
					ScannedToken resultingToken = new ScannedToken(productionRules[targetInstruction.value].nonTerminal, productionRules[targetInstruction.value].operate(scannedTokensList, script));

					//Put the resulting LHS token onto the stack.
					parsingStack.Add(new ParseStackElement(ParseStackElement.Type.Token, 0, resultingToken));
				}
				else {
					Debug.LogError("Stack parsing has broken due to invalid Instruction: " + System.Enum.GetName(typeof(ParserInstruction.Instruction), targetInstruction.instruction));
					yield break;
				}

			}

		}

	}


	public static void PrintList<T>(List<T> list) {
		string ret = "";
		for (int i = 0; i < list.Count; i++) {
			ret += list[i].ToString() + " | ";
		}

		Debug.Log(ret);
	}

	/// <summary>
	/// Builds a ParseTable for the Grammar
	/// </summary>
	/// <returns></returns>
    public static IEnumerator<object> BuildParseTable() {
		if (System.Enum.GetNames(typeof(SymbolicToken)).Length != nonTerminals.Count + TERMINALS.Count) {
			Debug.LogError("Count mismatch between total SymbolicTokens ("+ System.Enum.GetNames(typeof(SymbolicToken)).Length + ") and Terminals("+ TERMINALS.Count+")/NonTerminals("+ nonTerminals.Count+").");
			//yield break;
		}


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
						parseTableRow[k] = new ParserInstruction(GetRuleIndex(sRule.rule), ParserInstruction.Instruction.REDUCE);
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
	/// Prints a list of Symbolic tokens
	/// </summary>
	/// <param name="list"></param>
	/// <returns></returns>
    private static string PrintList(List<SymbolicToken> list) {
        string rt = "";
        for (int i = 0; i < list.Count; i++)
        {
            rt += System.Enum.GetName(typeof(SymbolicToken), list[i]) + ", ";
        }
        return rt;
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
		public delegate object Operate(List<ScannedToken> parsedTokens, Script script);

		/// <summary>
		/// The operation to perfrom when this rule is used.
		/// </summary>
		public Operate operate;

		public GrammarRule(SymbolicToken nonTerminal, SymbolicToken[] tokens, Operate operate = null) {
			this.nonTerminal = nonTerminal;
			this.tokens = tokens;
			//Set it to the passed operation otherwise use the default statement. the Default statement mentions the default rule was used and shows the form for the rule
			this.operate = operate ?? 
				((List<ScannedToken> parsedTokens, Script script) => {
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
		public ScriptType type;
		public string name;
		public Sprite image;
		public List<EffectSet> effectList;
		public List<ChoiceSet> choiceList;

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
		public SymbolicToken token;
		public object data;
		public ScannedToken(SymbolicToken token, object data) {
			this.token = token;
			this.data = data;
		}

		public override string ToString() {
			return System.Enum.GetName(typeof(SymbolicToken), token);
		}
	}
	#endregion
}