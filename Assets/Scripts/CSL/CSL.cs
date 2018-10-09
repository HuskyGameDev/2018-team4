using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// A grammer and rules specification for a Card Scripting Language
/// </summary>
public static class CSL {
	
	public enum ScriptType {Room, Item, Artifact, Event, Effect }

	#region Token
	/// <summary>
	/// The types of tokens interpreted by the system.
	/// </summary>
	public enum SymbolicToken {
		//Non Temrinals
		ScriptPrime, Script, EffectList, ChoiceList, EffectSet, ChoiceSet, Effect, Choice, StatementList, Statement,
		Assignment, IfStatement, TestStatement, Test, WhileStatement, IOStatement, DiscardStatement, CompoundStatement,
		Expr, SimpleExpr, AddExpr, MullExpr, Factor, Variable, Timing, Enforcement, NumConstant, Type,

		//Terminals
		COMMENT, IMAGE, IMMEDIATE, STATIC, NAME, TEXT, OPTIONAL, REQUIRED, ROOM, ITEM, EVENT, ARTIFACT, WHITESP,
		NEWLN, WHILE, ELSE, IF, VAR, TRUE, FALSE, LEFTBRACE, LEFTSQR, LEFTPAREN,
		CHOICEIDEN, EFFECTIDEN, DISCARD, THIS, DELAYED,
		RIGHTBRACE, RIGHTSQR, RIGHTPAREN, AND, OR, NOT, XOR, COMMA, PERIOD, EQUAL,
		NOTEQUAL, GREATEQUAL, LESSEQUAL, GREATTHAN, LESSTHAN, ASSIGN, ADD, SUB, DIV, MUL, SEMICOLON,
		IDENTIFIER, FLOATCON, STRINGCON, INTCON, ERR, IDENERR
	}


    static public SymbolicToken[] terminals = new SymbolicToken[] {
        SymbolicToken.WHITESP, SymbolicToken.NEWLN, SymbolicToken.WHILE, SymbolicToken.ELSE, SymbolicToken.IF, SymbolicToken.TEXT,
        SymbolicToken.VAR, SymbolicToken.DELAYED, SymbolicToken.TRUE, SymbolicToken.FALSE, SymbolicToken.LEFTBRACE, SymbolicToken.LEFTSQR, SymbolicToken.LEFTPAREN,
        SymbolicToken.RIGHTBRACE, SymbolicToken.RIGHTSQR, SymbolicToken.RIGHTPAREN, SymbolicToken.AND, SymbolicToken.OR, SymbolicToken.NOT, SymbolicToken.XOR, SymbolicToken.COMMA, SymbolicToken.PERIOD, SymbolicToken.EQUAL,
        SymbolicToken.NOTEQUAL, SymbolicToken.GREATEQUAL, SymbolicToken.LESSEQUAL, SymbolicToken.GREATTHAN, SymbolicToken.LESSTHAN, SymbolicToken.ASSIGN, SymbolicToken.ADD, SymbolicToken.SUB,
		SymbolicToken.DIV, SymbolicToken.MUL, SymbolicToken.SEMICOLON, SymbolicToken.IDENTIFIER, SymbolicToken.FLOATCON, SymbolicToken.STRINGCON, SymbolicToken.INTCON, SymbolicToken.ERR, SymbolicToken.IDENERR
    };

    static public SymbolicToken[] nonTerminals = new SymbolicToken[] {
		SymbolicToken.ScriptPrime, SymbolicToken.Script, SymbolicToken.EffectList, SymbolicToken.ChoiceList, SymbolicToken.EffectSet, SymbolicToken.ChoiceSet, SymbolicToken.Effect,
		SymbolicToken.Choice, SymbolicToken.StatementList, SymbolicToken.Statement, SymbolicToken.Assignment, SymbolicToken.IfStatement, SymbolicToken.Test, SymbolicToken.TestStatement,
		SymbolicToken.WhileStatement, SymbolicToken.IOStatement, SymbolicToken.DiscardStatement, SymbolicToken.CompoundStatement, SymbolicToken.Expr,
		SymbolicToken.SimpleExpr, SymbolicToken.AddExpr, SymbolicToken.MullExpr, SymbolicToken.Factor, SymbolicToken.Variable, SymbolicToken.Timing, SymbolicToken.Enforcement,
		SymbolicToken.NumConstant, SymbolicToken.Type
	};

    public static TokenRegexPair[] terminalTokens = new TokenRegexPair[] {
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
		new TokenRegexPair(SymbolicToken.STRINGCON, "\"Hello\"",    "^(\"(.*)\")$"),
		new TokenRegexPair(SymbolicToken.FLOATCON,  "-3.2314",      @"^((-?)(\d)*(\.)(\d)+)$"),
		new TokenRegexPair(SymbolicToken.INTCON,    "1337",			@"^(-?[0-9]+)$"),

		new TokenRegexPair(SymbolicToken.ERR,       "`",			@"^.*$")
	};
	#endregion

	#region Rules
	public static TokenMap[] rules = new TokenMap[] {
        new TokenMap(SymbolicToken.ScriptPrime, new SymbolicToken[] { SymbolicToken.Script }),

        new TokenMap(SymbolicToken.Script, new SymbolicToken[] { SymbolicToken.Type, SymbolicToken.SEMICOLON, SymbolicToken.NAME, SymbolicToken.STRINGCON, SymbolicToken.SEMICOLON, SymbolicToken.IMAGE, SymbolicToken.STRINGCON, SymbolicToken.SEMICOLON, SymbolicToken.EffectList, SymbolicToken.ChoiceList }),

		new TokenMap(SymbolicToken.EffectList, new SymbolicToken[] { SymbolicToken.EffectSet } ),
		new TokenMap(SymbolicToken.EffectList, new SymbolicToken[] { SymbolicToken.EffectList, SymbolicToken.EffectSet } ),

		new TokenMap(SymbolicToken.ChoiceList, new SymbolicToken[] { SymbolicToken.ChoiceSet } ),
		new TokenMap(SymbolicToken.ChoiceList, new SymbolicToken[] { SymbolicToken.ChoiceList, SymbolicToken.ChoiceSet } ),

		new TokenMap(SymbolicToken.EffectSet, new SymbolicToken[] { SymbolicToken.Timing, SymbolicToken.Effect} ),
		new TokenMap(SymbolicToken.EffectSet, new SymbolicToken[] { SymbolicToken.EffectSet, SymbolicToken.Effect} ),

		new TokenMap(SymbolicToken.ChoiceSet, new SymbolicToken[] { SymbolicToken.Timing, SymbolicToken.Enforcement, SymbolicToken.Choice} ),
		new TokenMap(SymbolicToken.ChoiceSet, new SymbolicToken[] { SymbolicToken.ChoiceSet, SymbolicToken.Choice } ),

		new TokenMap(SymbolicToken.Effect, new SymbolicToken[] { SymbolicToken.EFFECTIDEN, SymbolicToken.LEFTBRACE, SymbolicToken.StatementList, SymbolicToken.RIGHTBRACE } ),

		new TokenMap(SymbolicToken.Choice, new SymbolicToken[] { SymbolicToken.CHOICEIDEN, SymbolicToken.LEFTBRACE, SymbolicToken.NAME, SymbolicToken.STRINGCON, SymbolicToken.SEMICOLON, SymbolicToken.Test, SymbolicToken.LEFTBRACE, SymbolicToken.TEXT, SymbolicToken.STRINGCON, SymbolicToken.SEMICOLON, SymbolicToken.EffectList, SymbolicToken.RIGHTBRACE, SymbolicToken.RIGHTBRACE } ),


		new TokenMap(SymbolicToken.StatementList, new SymbolicToken[] { SymbolicToken.Statement } ),
        new TokenMap(SymbolicToken.StatementList, new SymbolicToken[] { SymbolicToken.StatementList, SymbolicToken.Statement } ),

        new TokenMap(SymbolicToken.Statement, new SymbolicToken[] { SymbolicToken.Assignment } ),
        new TokenMap(SymbolicToken.Statement, new SymbolicToken[] { SymbolicToken.IfStatement } ),
        new TokenMap(SymbolicToken.Statement, new SymbolicToken[] { SymbolicToken.WhileStatement } ),
        new TokenMap(SymbolicToken.Statement, new SymbolicToken[] { SymbolicToken.IOStatement } ),
        new TokenMap(SymbolicToken.Statement, new SymbolicToken[] { SymbolicToken.CompoundStatement } ),


        new TokenMap(SymbolicToken.Assignment, new SymbolicToken[] { SymbolicToken.Variable, SymbolicToken.ASSIGN, SymbolicToken.Expr, SymbolicToken.SEMICOLON } ),

        new TokenMap(SymbolicToken.IfStatement, new SymbolicToken[] { SymbolicToken.IF, SymbolicToken.TestStatement, SymbolicToken.ELSE, SymbolicToken.IfStatement } ),
        new TokenMap(SymbolicToken.IfStatement, new SymbolicToken[] { SymbolicToken.IF, SymbolicToken.TestStatement, SymbolicToken.ELSE, SymbolicToken.CompoundStatement } ),
		new TokenMap(SymbolicToken.IfStatement, new SymbolicToken[] { SymbolicToken.IF, SymbolicToken.TestStatement } ),


		new TokenMap(SymbolicToken.TestStatement, new SymbolicToken[] { SymbolicToken.Test, SymbolicToken.CompoundStatement } ),

		new TokenMap(SymbolicToken.Test, new SymbolicToken[] { SymbolicToken.LEFTPAREN, SymbolicToken.Expr, SymbolicToken.RIGHTPAREN } ),

		new TokenMap(SymbolicToken.WhileStatement, new SymbolicToken[] { SymbolicToken.WHILE, SymbolicToken.Expr, SymbolicToken.Statement, SymbolicToken.SEMICOLON } ),

		new TokenMap(SymbolicToken.IOStatement, new SymbolicToken[] { SymbolicToken.WHILE, SymbolicToken.Expr, SymbolicToken.Statement } ),

		new TokenMap(SymbolicToken.DiscardStatement, new SymbolicToken[] {SymbolicToken.DISCARD, SymbolicToken.IDENTIFIER }),
		new TokenMap(SymbolicToken.DiscardStatement, new SymbolicToken[] {SymbolicToken.DISCARD, SymbolicToken.THIS }),

		new TokenMap(SymbolicToken.CompoundStatement, new SymbolicToken[] { SymbolicToken.LEFTBRACE, SymbolicToken.StatementList, SymbolicToken.RIGHTBRACE } ),

        new TokenMap(SymbolicToken.Expr, new SymbolicToken[] { SymbolicToken.Expr, SymbolicToken.ADD, SymbolicToken.SimpleExpr } ),
        new TokenMap(SymbolicToken.Expr, new SymbolicToken[] { SymbolicToken.Expr, SymbolicToken.OR, SymbolicToken.SimpleExpr } ),
        new TokenMap(SymbolicToken.Expr, new SymbolicToken[] { SymbolicToken.SimpleExpr } ),
        new TokenMap(SymbolicToken.Expr, new SymbolicToken[] { SymbolicToken.NOT, SymbolicToken.SimpleExpr } ),

        new TokenMap(SymbolicToken.SimpleExpr, new SymbolicToken[] { SymbolicToken.SimpleExpr, SymbolicToken.EQUAL, SymbolicToken.AddExpr } ),
        new TokenMap(SymbolicToken.SimpleExpr, new SymbolicToken[] { SymbolicToken.SimpleExpr, SymbolicToken.NOTEQUAL, SymbolicToken.AddExpr } ),
        new TokenMap(SymbolicToken.SimpleExpr, new SymbolicToken[] { SymbolicToken.SimpleExpr, SymbolicToken.LESSEQUAL, SymbolicToken.AddExpr } ),
        new TokenMap(SymbolicToken.SimpleExpr, new SymbolicToken[] { SymbolicToken.SimpleExpr, SymbolicToken.LESSTHAN, SymbolicToken.AddExpr } ),
        new TokenMap(SymbolicToken.SimpleExpr, new SymbolicToken[] { SymbolicToken.SimpleExpr, SymbolicToken.GREATEQUAL, SymbolicToken.AddExpr } ),
        new TokenMap(SymbolicToken.SimpleExpr, new SymbolicToken[] { SymbolicToken.SimpleExpr, SymbolicToken.GREATTHAN, SymbolicToken.AddExpr } ),
        new TokenMap(SymbolicToken.SimpleExpr, new SymbolicToken[] { SymbolicToken.AddExpr } ),


        new TokenMap(SymbolicToken.AddExpr, new SymbolicToken[] { SymbolicToken.AddExpr, SymbolicToken.ADD, SymbolicToken.AddExpr } ),
        new TokenMap(SymbolicToken.AddExpr, new SymbolicToken[] { SymbolicToken.AddExpr, SymbolicToken.SUB, SymbolicToken.MullExpr } ),
        new TokenMap(SymbolicToken.AddExpr, new SymbolicToken[] { SymbolicToken.MullExpr } ),

        new TokenMap(SymbolicToken.MullExpr, new SymbolicToken[] { SymbolicToken.MullExpr, SymbolicToken.MUL, SymbolicToken.Factor } ),
        new TokenMap(SymbolicToken.MullExpr, new SymbolicToken[] { SymbolicToken.MullExpr, SymbolicToken.SUB, SymbolicToken.Factor } ),
        new TokenMap(SymbolicToken.MullExpr, new SymbolicToken[] { SymbolicToken.Factor } ),

        new TokenMap(SymbolicToken.Factor, new SymbolicToken[] { SymbolicToken.Variable } ),
        new TokenMap(SymbolicToken.Factor, new SymbolicToken[] { SymbolicToken.NumConstant } ),
        new TokenMap(SymbolicToken.Factor, new SymbolicToken[] { SymbolicToken.IDENTIFIER, SymbolicToken.LEFTPAREN, SymbolicToken.RIGHTPAREN } ),
        new TokenMap(SymbolicToken.Factor, new SymbolicToken[] { SymbolicToken.LEFTPAREN, SymbolicToken.Expr, SymbolicToken.RIGHTPAREN } ),

        new TokenMap(SymbolicToken.Variable, new SymbolicToken[] { SymbolicToken.IDENTIFIER } ),
        new TokenMap(SymbolicToken.Variable, new SymbolicToken[] { SymbolicToken.IDENTIFIER, SymbolicToken.LEFTSQR, SymbolicToken.Expr, SymbolicToken.RIGHTSQR } ),

		new TokenMap(SymbolicToken.Timing, new SymbolicToken[] { SymbolicToken.IMMEDIATE } ),
		new TokenMap(SymbolicToken.Timing, new SymbolicToken[] { SymbolicToken.DELAYED } ),
		new TokenMap(SymbolicToken.Timing, new SymbolicToken[] { SymbolicToken.STATIC } ),

		new TokenMap(SymbolicToken.Enforcement, new SymbolicToken[] { SymbolicToken.OPTIONAL } ),
		new TokenMap(SymbolicToken.Enforcement, new SymbolicToken[] { SymbolicToken.REQUIRED } ),


		new TokenMap(SymbolicToken.NumConstant, new SymbolicToken[] { SymbolicToken.INTCON } ),
        new TokenMap(SymbolicToken.NumConstant, new SymbolicToken[] { SymbolicToken.FLOATCON } ),

		new TokenMap(SymbolicToken.Type, new SymbolicToken[] { SymbolicToken.ITEM }),
		new TokenMap(SymbolicToken.Type, new SymbolicToken[] { SymbolicToken.ROOM }),
		new TokenMap(SymbolicToken.Type, new SymbolicToken[] { SymbolicToken.EVENT }),
		new TokenMap(SymbolicToken.Type, new SymbolicToken[] { SymbolicToken.ARTIFACT })
	};

	#endregion

	#region Methods
    static public IEnumerator<object> Build() {
        //First Set storage
        Dictionary<SymbolicToken, List<SymbolicToken>> firstSet = new Dictionary<SymbolicToken, List<SymbolicToken>>();
        //Follow Set storage
        Dictionary<SymbolicToken, List<SymbolicToken>> followSet = new Dictionary<SymbolicToken, List<SymbolicToken>>();

        //Create the first and follow set for each non termial
        foreach (SymbolicToken nT in nonTerminals) {
            //Create first set
            firstSet.Add(nT, new List<SymbolicToken>());
            //create follow set
            followSet.Add(nT, new List<SymbolicToken>());

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

            //Debug.Log(System.Enum.GetName(typeof(Token), nT) + " first: " + PrintList(firstSet[nT]));
            //Debug.Log(System.Enum.GetName(typeof(Token), nT) + " follow: " + PrintList(followSet[nT]));
        }

		//Create the States.


		List<SymbolicToken> unexapndedList = new List<SymbolicToken>();
		List<SymbolicToken> seenList = new List<SymbolicToken>();
		List<TokenMap> stateRules = new List<TokenMap>();
		unexapndedList.Add(SymbolicToken.ScriptPrime);
		seenList.Add(SymbolicToken.ScriptPrime);

		while (unexapndedList.Count > 0) {
			yield return null;

			SymbolicToken target = unexapndedList[0];
			//Debug.Log(PrintList(seenList));
			//Debug.Log(System.Enum.GetName(typeof(Token), target));
			unexapndedList.RemoveAt(0);

			//For each token, add this rule to the state.
			for (int i = 0; i < rules.Length; i++) {
				//If this token has the rule 
				if (rules[i].nonTerminal == target)
					stateRules.Add(rules[i]);
				yield return null;
			}
			//Go through the first set for this token
			if (firstSet.ContainsKey(target) == false)
				continue;
			foreach (SymbolicToken t in firstSet[target]) {
				yield return null;

				//If we have not seen this token before.
				if (seenList.Contains(t) == false) {
					unexapndedList.Add(t);
					seenList.Add(t);
				}
			}
		}

		string toPrint = "";
		foreach (TokenMap map in stateRules) {
			toPrint += map.ToString();
			toPrint += "\n";
		}
		Debug.Log(toPrint);
    }

    private static string PrintList(List<SymbolicToken> list) {
        string rt = "";
        for (int i = 0; i < list.Count; i++)
        {
            rt += System.Enum.GetName(typeof(SymbolicToken), list[i]) + ", ";
        }
        return rt;
    }


	public static object Box(SymbolicToken token, string text) {
		//Debug.Log(token + " | " + text);
		switch (token) {
			case SymbolicToken.INTCON:
				return (object)System.Convert.ToInt32(text);
			case SymbolicToken.STRINGCON:
				string retString = text.Substring(1,text.Length-1);
				return (object)retString;
			default:
				return (object)text;
		}
	}

	#endregion

	#region Structs
	public struct TokenMap {
		public SymbolicToken nonTerminal;
		public SymbolicToken[] tokens;
		public TokenMap(SymbolicToken nonTerminal, SymbolicToken[] tokens) {
			this.nonTerminal = nonTerminal;
			this.tokens = tokens;
		}

		public string ToString() {
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


	public struct Script {
		public List<ParsedToken> tokens;
		public Script(List<ParsedToken> tokens) {
			this.tokens = tokens;
		}
	}

	public struct ParsedToken {
		public SymbolicToken token;
		public object data;
		public ParsedToken(SymbolicToken token, object data) {
			this.token = token;
			this.data = data;
		}
	}
	#endregion
}
