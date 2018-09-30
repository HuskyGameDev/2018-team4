using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// A grammer and rules specification for a Card Scripting Language
/// </summary>
public static class CSL {

	#region Token
	/// <summary>
	/// The types of tokens interpreted by the system.
	/// </summary>
	public enum Token {
		//Non Temrinals
		NumConstant, Variable, Factor, MullExpr, AddExpr, SimpleExpr, Expr, CompoundStatement, ExitStatement, ReturnStatement,
		IOStatement, Test, WhileStatement, TestStatment, IfStatement, Assignment, Statement, StatementList, Type, 
		VarDecl, IdentifierList, DeclList, ProcedureBody, FunctionDecl, ProcedureHead, ProceadureDecl, Procedures, Script, ScriptPrime,

		//Terminals
		WHITESP, NEWLN, WHILE, ELSE, IF, PLAYER, DECK, MAP, CAMERA, ENGINE, TITLE, TEXT, UI,
		CARD, NEW, VAR, DELETE, TRUE, FALSE, RETURN, RUN, LEFTBRACE, LEFTSQR, LEFTPAREN,
		RIGHTBRACE, RIGHTSQR, RIGHTPAREN, AND, OR, NOT, XOR, COMMA, PERIOD, EQUAL,
		NOTEQUAL, GREATEQUAL, LESSEQUAL, GREATTHAN, LESSTHAN, ASSIGN, ADD, SUB, DIV, MUL, SEMICOLON,
		IDENTIFIER, FLOATCON, STRINGCON, INTCON, ERR, IDENERR
	}


    static public Token[] terminals = new Token[] {
        Token.WHITESP, Token.NEWLN, Token.WHILE, Token.ELSE, Token.IF, Token.PLAYER, Token.DECK, Token.MAP, Token.CAMERA, Token.ENGINE, Token.TITLE, Token.TEXT, Token.UI,
        Token.CARD, Token.NEW, Token.VAR, Token.DELETE, Token.TRUE, Token.FALSE, Token.RETURN, Token.RUN, Token.LEFTBRACE, Token.LEFTSQR, Token.LEFTPAREN,
        Token.RIGHTBRACE, Token.RIGHTSQR, Token.RIGHTPAREN, Token.AND, Token.OR, Token.NOT, Token.XOR, Token.COMMA, Token.PERIOD, Token.EQUAL,
        Token.NOTEQUAL, Token.GREATEQUAL, Token.LESSEQUAL, Token.GREATTHAN, Token.LESSTHAN, Token.ASSIGN, Token.ADD, Token.SUB, Token.DIV, Token.MUL, Token.SEMICOLON,
        Token.IDENTIFIER, Token.FLOATCON, Token.STRINGCON, Token.INTCON, Token.ERR, Token.IDENERR
    };

    static public Token[] nonTerminals = new Token[] {
       Token.NumConstant, Token.Variable, Token.Factor, Token.MullExpr, Token.AddExpr, Token.SimpleExpr, Token.Expr, Token.CompoundStatement, Token.ExitStatement, Token.ReturnStatement,
       Token.IOStatement, Token.Test, Token.WhileStatement, Token.TestStatment, Token.IfStatement, Token.Assignment, Token.Statement, Token.StatementList, Token.Type,
       Token. VarDecl, Token.IdentifierList, Token.DeclList, Token.ProcedureBody, Token.FunctionDecl, Token.ProcedureHead, Token.ProceadureDecl, Token.Procedures, Token.Script, Token.ScriptPrime
    };

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
		new TokenRegexPair(Token.FLOATCON,  "-3.2314",      @"^((-?)(\d)*(\.)(\d)+)$"),
		new TokenRegexPair(Token.STRINGCON, "\"Hello\"",    "^(\"(.*)\")$"),
		new TokenRegexPair(Token.INTCON,    "1337",			@"^(-?[0-9]+)$"),
		new TokenRegexPair(Token.ERR,       "`",			@".*$")
	};
	#endregion

	#region Rules
	public static TokenMap[] rules = new TokenMap[] {
        new TokenMap(Token.ScriptPrime, new Token[] { Token.Script }),

		new TokenMap(Token.Script, new Token[] { Token.DeclList, Token.Procedures } ),
		new TokenMap(Token.Script, new Token[] { Token.Procedures } ),


        new TokenMap(Token.Procedures, new Token[] { Token.ProceadureDecl, Token.Procedures } ),
        new TokenMap(Token.Procedures, new Token[] { Token.ProceadureDecl } ),


        new TokenMap(Token.ProceadureDecl, new Token[] { Token.ProcedureHead, Token.ProcedureBody } ),

        new TokenMap(Token.ProcedureHead, new Token[] { Token.FunctionDecl, Token.DeclList } ),
        new TokenMap(Token.ProcedureHead, new Token[] { Token.FunctionDecl } ),

        new TokenMap(Token.FunctionDecl, new Token[] { Token.Type, Token.IDENTIFIER, Token.LEFTPAREN, Token.RIGHTPAREN, Token.LEFTBRACE } ),

        new TokenMap(Token.ProcedureBody, new Token[] { Token.StatementList, Token.RIGHTBRACE } ),

        new TokenMap(Token.DeclList, new Token[] { Token.Type, Token.IdentifierList, Token.SEMICOLON } ),
        new TokenMap(Token.DeclList, new Token[] { Token.DeclList, Token.Type, Token.IdentifierList, Token.SEMICOLON } ),


        new TokenMap(Token.IdentifierList, new Token[] { Token.VarDecl } ),
        new TokenMap(Token.IdentifierList, new Token[] { Token.IdentifierList, Token.COMMA, Token.VarDecl } ),

        new TokenMap(Token.VarDecl, new Token[] { Token.IDENTIFIER } ),
        new TokenMap(Token.VarDecl, new Token[] { Token.IDENTIFIER, Token.LEFTSQR, Token.INTCON, Token.RIGHTSQR } ),

        new TokenMap(Token.Type, new Token[] { Token.INTCON } ),
        new TokenMap(Token.Type, new Token[] { Token.FLOATCON } ),

        new TokenMap(Token.StatementList, new Token[] { Token.Statement } ),
        new TokenMap(Token.StatementList, new Token[] { Token.StatementList, Token.Statement } ),

        new TokenMap(Token.Statement, new Token[] { Token.Assignment } ),
        new TokenMap(Token.Statement, new Token[] { Token.IfStatement } ),
        new TokenMap(Token.Statement, new Token[] { Token.WhileStatement } ),
        new TokenMap(Token.Statement, new Token[] { Token.IOStatement } ),
        new TokenMap(Token.Statement, new Token[] { Token.ReturnStatement } ),
        new TokenMap(Token.Statement, new Token[] { Token.ExitStatement } ),
        new TokenMap(Token.Statement, new Token[] { Token.CompoundStatement } ),


        new TokenMap(Token.Assignment, new Token[] { Token.Variable, Token.ASSIGN, Token.Expr, Token.SEMICOLON } ),

        new TokenMap(Token.IfStatement, new Token[] { Token.IF, Token.TestStatment, Token.ELSE, Token.CompoundStatement } ),
        new TokenMap(Token.IfStatement, new Token[] { Token.IF, Token.TestStatment } ),

        new TokenMap(Token.TestStatment, new Token[] { Token.Test, Token.CompoundStatement } ),

        new TokenMap(Token.Test, new Token[] { Token.Expr } ),

        new TokenMap(Token.WhileStatement, new Token[] { Token.WHILE, Token.Expr, Token.Statement } ),

        //new TokenMap(Token.IOStatement, new Token[] { Token.READ } ),

        new TokenMap(Token.ReturnStatement, new Token[] { Token.RETURN, Token.Expr, Token.SEMICOLON } ),

        new TokenMap(Token.CompoundStatement, new Token[] { Token.LEFTBRACE, Token.StatementList, Token.RIGHTBRACE } ),

        new TokenMap(Token.Expr, new Token[] { Token.Expr, Token.ADD, Token.SimpleExpr } ),
        new TokenMap(Token.Expr, new Token[] { Token.Expr, Token.OR, Token.SimpleExpr } ),
        new TokenMap(Token.Expr, new Token[] { Token.SimpleExpr } ),
        new TokenMap(Token.Expr, new Token[] { Token.NOT, Token.SimpleExpr } ),

        new TokenMap(Token.SimpleExpr, new Token[] { Token.SimpleExpr, Token.EQUAL, Token.AddExpr } ),
        new TokenMap(Token.SimpleExpr, new Token[] { Token.SimpleExpr, Token.NOTEQUAL, Token.AddExpr } ),
        new TokenMap(Token.SimpleExpr, new Token[] { Token.SimpleExpr, Token.LESSEQUAL, Token.AddExpr } ),
        new TokenMap(Token.SimpleExpr, new Token[] { Token.SimpleExpr, Token.LESSTHAN, Token.AddExpr } ),
        new TokenMap(Token.SimpleExpr, new Token[] { Token.SimpleExpr, Token.GREATEQUAL, Token.AddExpr } ),
        new TokenMap(Token.SimpleExpr, new Token[] { Token.SimpleExpr, Token.GREATTHAN, Token.AddExpr } ),
        new TokenMap(Token.SimpleExpr, new Token[] { Token.AddExpr } ),


        new TokenMap(Token.AddExpr, new Token[] { Token.AddExpr, Token.ADD, Token.AddExpr } ),
        new TokenMap(Token.AddExpr, new Token[] { Token.AddExpr, Token.ADD, Token.MullExpr } ),
        new TokenMap(Token.AddExpr, new Token[] { Token.MullExpr} ),

        new TokenMap(Token.MullExpr, new Token[] { Token.MullExpr, Token.MUL, Token.Factor } ),
        new TokenMap(Token.MullExpr, new Token[] { Token.MullExpr, Token.SUB, Token.Factor } ),
        new TokenMap(Token.MullExpr, new Token[] { Token.Factor } ),

        new TokenMap(Token.Factor, new Token[] { Token.Variable } ),
        new TokenMap(Token.Factor, new Token[] { Token.NumConstant } ),
        new TokenMap(Token.Factor, new Token[] { Token.IDENTIFIER, Token.LEFTPAREN, Token.RIGHTPAREN } ),
        new TokenMap(Token.Factor, new Token[] { Token.LEFTPAREN, Token.Expr, Token.RIGHTPAREN } ),

        new TokenMap(Token.Variable, new Token[] { Token.IDENTIFIER } ),
        new TokenMap(Token.Variable, new Token[] { Token.IDENTIFIER, Token.LEFTSQR, Token.Expr, Token.RIGHTSQR } ),


        new TokenMap(Token.NumConstant, new Token[] { Token.INTCON } ),
        new TokenMap(Token.NumConstant, new Token[] {Token.FLOATCON } )


    };

	#endregion

	#region Methods
    public static void Build() {
        //First Set storage
        Dictionary<Token, List<Token>> firstSet = new Dictionary<Token, List<Token>>();
        //Follow Set storage
        Dictionary<Token, List<Token>> followSet = new Dictionary<Token, List<Token>>();

        //Create the first and follow set for each non termial
        foreach (Token nT in nonTerminals) {
            //Create first set
            firstSet.Add(nT, new List<Token>());
            //create follow set
            followSet.Add(nT, new List<Token>());

            //for each Rule featuring this token as the NonTerminal, add the first token in tokens to the First set
            foreach (TokenMap map in rules)
                if (map.nonTerminal == nT)
                    if (firstSet[nT].Contains(map.tokens[0]) == false) firstSet[nT].Add(map.tokens[0]);

            //Check each rule, and for each instance of this nonTerminal, record what follows immediatly after it.
            foreach (TokenMap map in rules) {
                for (int i = map.tokens.Length - 1; i >= 0; i--) {
                    if (map.tokens[i] == nT) {
                        //Dont add EOF
                        if (i == map.tokens.Length - 1)
                            continue;
                        //Since we are looping backwards and skip EOF, we know that there exists a token at this spot + 1 
                        if (followSet[nT].Contains(map.tokens[i+1]) == false) followSet[nT].Add(map.tokens[i+1]);
                    }
                }
            }

            Debug.Log(System.Enum.GetName(typeof(Token), nT) + " first: " + PrintList(firstSet[nT]));
            Debug.Log(System.Enum.GetName(typeof(Token), nT) + " follow: " + PrintList(followSet[nT]));
        }
        
        //Create the States.


    }

    private static string PrintList(List<Token> list) {
        string rt = "";
        for (int i = 0; i < list.Count; i++)
        {
            rt += System.Enum.GetName(typeof(Token), list[i]) + ",";
        }
        return rt;
    }


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
