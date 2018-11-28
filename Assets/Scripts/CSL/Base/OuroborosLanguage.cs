using UnityEngine;
using System.IO;
using System.Collections.Generic;
using ProductionRule = OuroborosScripting.OuroborosInterpreter.ProductionRule;
using SymbolStringTuple = OuroborosScripting.OuroborosInterpreter.SymbolStringTuple;
using ParserInstruction = OuroborosScripting.OuroborosInterpreter.ParserInstruction;
using CoroutineWrapper = OuroborosScripting.OuroborosInterpreter.CoroutineWrapper;
using CoroutineMethod = OuroborosScripting.OuroborosInterpreter.CoroutineMethod;

namespace OuroborosScripting.GeneratedLanguages {

	public class OuroborosLanguage : IOuroborosLanguage {


		public ParserInstruction[][] GetParseTable() { return parseTable; } public ProductionRule[] GetProductionRules() { return productionRules; } public SymbolStringTuple[] GetRegexPairs() { return regexPairs; } public List<object> GetNonTerminals() { return nonTerminals; } public List<object> GetTERMINALS() { return TERMINALS; }
		IEnumerator<object> IOuroborosLanguage.StartLangCoroutine(CoroutineMethod name, CoroutineWrapper coroutineWrapper) { yield return GameManager._instance.StartCoroutine(name(coroutineWrapper)); }


		public enum SymbolicToken {
			_WS, _COMMENT, _NEWLINE, NAMESTRING, EXTSTRING, GLOBALSTRING, USINGSTRING, PRODUCTIONSTRING, HEADERTAG, COLON, DISCARDIDEN, IDENTIFIER, REGEX, CODE,
			LanguageDefinition, IndetifierList, ProductionHeader, TokenStatementList, RegexRule, ProductionRule, ProductionRuleRHSList, RegexRuleRHSList, RegesRuleRHS, ProductionRuleRHS, Corotuine, GlobalHeader, UsingHeader, NameHeader, ExtHeader
		}

		public List<object> TERMINALS = new List<object> { SymbolicToken._WS, SymbolicToken._COMMENT, SymbolicToken._NEWLINE, SymbolicToken.NAMESTRING, SymbolicToken.EXTSTRING, SymbolicToken.GLOBALSTRING, SymbolicToken.USINGSTRING, SymbolicToken.PRODUCTIONSTRING, SymbolicToken.HEADERTAG, SymbolicToken.COLON, SymbolicToken.DISCARDIDEN, SymbolicToken.IDENTIFIER, SymbolicToken.REGEX, SymbolicToken.CODE };
		public List<object> nonTerminals = new List<object> { SymbolicToken.LanguageDefinition, SymbolicToken.IndetifierList, SymbolicToken.ProductionHeader, SymbolicToken.TokenStatementList, SymbolicToken.RegexRule, SymbolicToken.ProductionRule, SymbolicToken.ProductionRuleRHSList, SymbolicToken.RegexRuleRHSList, SymbolicToken.RegesRuleRHS, SymbolicToken.ProductionRuleRHS, SymbolicToken.Corotuine, SymbolicToken.GlobalHeader, SymbolicToken.UsingHeader, SymbolicToken.NameHeader, SymbolicToken.ExtHeader };

		public SymbolStringTuple[] regexPairs = new SymbolStringTuple[] {
			new SymbolStringTuple(0, @"^(\s)$",								Execution0, true),		//_WS,					0
			new SymbolStringTuple(1, @"^(//[^\n]*\n)$",						Execution1, true),		//_COMMENT,				1 
			new SymbolStringTuple(2, @"^(\n)$",								Execution2, true),		//_NEWLINE,				2
			new SymbolStringTuple(3, @"^(Name)$",							Execution3),		//NAMESTRING			3
			new SymbolStringTuple(4, @"^(Ext)$",							Execution4),		//EXTSTRING				4
			new SymbolStringTuple(5, @"^(Global)$",							Execution5),		//GLOBALSTRING			5
			new SymbolStringTuple(6, @"^(Using)$",							Execution6),		//USINGSTRING			6
			new SymbolStringTuple(7, @"^(Prod)$",							Execution7),		//PRODUCTIONSTRING		7
			new SymbolStringTuple(8, @"^(%%)$",								Execution8),		//HEADERTAG				8
			new SymbolStringTuple(9, @"^(:)$",								Execution9),		//COLON					9
			new SymbolStringTuple(10, @"^(_)([a-zA-Z]([a-zA-Z]|[0-9])*)$",	Execution10),		//DISCARDIDEN			10
			new SymbolStringTuple(11, @"^([a-zA-Z]([a-zA-Z]|[0-9])*)$",		Execution11),		//IDENTIFIER			11
			new SymbolStringTuple(12, @"^('[^\n]*')$",						Execution12),		//REGEX				12
			new SymbolStringTuple(13, @"^(#)[^#]*(#)$$",			Execution13)		//CODE,				13
		};
		public ProductionRule[] productionRules = new ProductionRule[] {
			new ProductionRule(0, 5, Execution14),  //14 LanguageDefinition	:	NameHeader ExtHeader UsingHeader GlobalHeader ProductionHeader
			new ProductionRule(1, 1, Execution15),  //15 IdentifierList		:	IDENTIFIER
			new ProductionRule(1, 2, Execution16),  //16 						:	IdentifierList IDENTIFIER
			new ProductionRule(2, 4, Execution17),  //17 ProductionHeader 	:	HEADERTAG PRODUCTIONSTRING TokenStatementList HEADERTAG
			new ProductionRule(3, 1, Execution18),  //18 TokenStatementList	:	RegexRule
			new ProductionRule(3, 1, Execution19),  //19						:	ProductionRule
			new ProductionRule(3, 2, Execution20),  //20						:	TokenStatementList RegexRule
			new ProductionRule(3, 2, Execution21),  //21						:	TokenStatementList ProductionRule
			new ProductionRule(4, 2, Execution22),  //22 RegexRule			:	IDENTIFIER RegexRuleRHSList
			new ProductionRule(5, 2, Execution23),  //23 ProductionRule		:	IDENTIFIER ProductionRuleRHSList
			new ProductionRule(6, 1, Execution24),  //24 ProductionRuleRHSList:	ProductionRuleRHS
			new ProductionRule(6, 2, Execution25),  //25 						:	ProductionRuleRHSList ProductionRuleRHS
			new ProductionRule(7, 1, Execution26),  //26 RegexRuleRHSList		:	RegexRuleRHS
			new ProductionRule(7, 2, Execution27),  //27						:	RegexRuleRHSList RegexRuleRHS
			new ProductionRule(8, 3, Execution28),  //28 RegexRuleRHS 		:	COLON REGEX Coroutine
			new ProductionRule(9, 3, Execution29),  //29 ProductionRuleRHS	:	COLON IdentifierList Coroutine
			new ProductionRule(10, 1, Execution30), //30 Coroutine			:	CODE
			new ProductionRule(11, 3, Execution31), //31 GlobalHeader 		:	HEADERTAG GLOBALSTRING CODE
			new ProductionRule(12, 3, Execution32), //32 UsingHeader			: 	HEADERTAG USINGSTRING CODE
			new ProductionRule(13, 3, Execution33), //33 NameHeader			: 	HEADERTAG NAMESTRING CODE
			new ProductionRule(14, 3, Execution34)  //34 ExtHeader			: 	HEADERTAG EXTSTRING CODE
		};
		public ParserInstruction[][] parseTable = new ParserInstruction[][] { new ParserInstruction[] {new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(2, ParserInstruction.Instruction.SHIFT), new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(1, ParserInstruction.Instruction.GOTO), new ParserInstruction(0, ParserInstruction.Instruction.ERR)},
new ParserInstruction[] {new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(4, ParserInstruction.Instruction.SHIFT), new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(3, ParserInstruction.Instruction.GOTO)},
new ParserInstruction[] {new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(5, ParserInstruction.Instruction.SHIFT), new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(0, ParserInstruction.Instruction.ERR)},
new ParserInstruction[] {new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(7, ParserInstruction.Instruction.SHIFT), new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(6, ParserInstruction.Instruction.GOTO), new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(0, ParserInstruction.Instruction.ERR)},
new ParserInstruction[] {new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(8, ParserInstruction.Instruction.SHIFT), new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(0, ParserInstruction.Instruction.ERR)},
new ParserInstruction[] {new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(9, ParserInstruction.Instruction.SHIFT), new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(0, ParserInstruction.Instruction.ERR)},
new ParserInstruction[] {new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(11, ParserInstruction.Instruction.SHIFT), new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(10, ParserInstruction.Instruction.GOTO), new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(0, ParserInstruction.Instruction.ERR)},
new ParserInstruction[] {new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(12, ParserInstruction.Instruction.SHIFT), new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(0, ParserInstruction.Instruction.ERR)},
new ParserInstruction[] {new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(13, ParserInstruction.Instruction.SHIFT), new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(0, ParserInstruction.Instruction.ERR)},
new ParserInstruction[] {new ParserInstruction(19, ParserInstruction.Instruction.REDUCE), new ParserInstruction(19, ParserInstruction.Instruction.REDUCE), new ParserInstruction(19, ParserInstruction.Instruction.REDUCE), new ParserInstruction(19, ParserInstruction.Instruction.REDUCE), new ParserInstruction(19, ParserInstruction.Instruction.REDUCE), new ParserInstruction(19, ParserInstruction.Instruction.REDUCE), new ParserInstruction(19, ParserInstruction.Instruction.REDUCE), new ParserInstruction(19, ParserInstruction.Instruction.REDUCE), new ParserInstruction(19, ParserInstruction.Instruction.REDUCE), new ParserInstruction(19, ParserInstruction.Instruction.REDUCE), new ParserInstruction(19, ParserInstruction.Instruction.REDUCE), new ParserInstruction(19, ParserInstruction.Instruction.REDUCE), new ParserInstruction(19, ParserInstruction.Instruction.REDUCE), new ParserInstruction(19, ParserInstruction.Instruction.REDUCE), new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(0, ParserInstruction.Instruction.ERR)},
new ParserInstruction[] {new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(15, ParserInstruction.Instruction.SHIFT), new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(14, ParserInstruction.Instruction.GOTO), new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(0, ParserInstruction.Instruction.ERR)},
new ParserInstruction[] {new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(16, ParserInstruction.Instruction.SHIFT), new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(0, ParserInstruction.Instruction.ERR)},
new ParserInstruction[] {new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(17, ParserInstruction.Instruction.SHIFT), new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(0, ParserInstruction.Instruction.ERR)},
new ParserInstruction[] {new ParserInstruction(20, ParserInstruction.Instruction.REDUCE), new ParserInstruction(20, ParserInstruction.Instruction.REDUCE), new ParserInstruction(20, ParserInstruction.Instruction.REDUCE), new ParserInstruction(20, ParserInstruction.Instruction.REDUCE), new ParserInstruction(20, ParserInstruction.Instruction.REDUCE), new ParserInstruction(20, ParserInstruction.Instruction.REDUCE), new ParserInstruction(20, ParserInstruction.Instruction.REDUCE), new ParserInstruction(20, ParserInstruction.Instruction.REDUCE), new ParserInstruction(20, ParserInstruction.Instruction.REDUCE), new ParserInstruction(20, ParserInstruction.Instruction.REDUCE), new ParserInstruction(20, ParserInstruction.Instruction.REDUCE), new ParserInstruction(20, ParserInstruction.Instruction.REDUCE), new ParserInstruction(20, ParserInstruction.Instruction.REDUCE), new ParserInstruction(20, ParserInstruction.Instruction.REDUCE), new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(0, ParserInstruction.Instruction.ERR)},
new ParserInstruction[] {new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(0, ParserInstruction.Instruction.ACCEPT), new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(0, ParserInstruction.Instruction.ERR)},
new ParserInstruction[] {new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(18, ParserInstruction.Instruction.SHIFT), new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(0, ParserInstruction.Instruction.ERR)},
new ParserInstruction[] {new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(19, ParserInstruction.Instruction.SHIFT), new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(0, ParserInstruction.Instruction.ERR)},
new ParserInstruction[] {new ParserInstruction(18, ParserInstruction.Instruction.REDUCE), new ParserInstruction(18, ParserInstruction.Instruction.REDUCE), new ParserInstruction(18, ParserInstruction.Instruction.REDUCE), new ParserInstruction(18, ParserInstruction.Instruction.REDUCE), new ParserInstruction(18, ParserInstruction.Instruction.REDUCE), new ParserInstruction(18, ParserInstruction.Instruction.REDUCE), new ParserInstruction(18, ParserInstruction.Instruction.REDUCE), new ParserInstruction(18, ParserInstruction.Instruction.REDUCE), new ParserInstruction(18, ParserInstruction.Instruction.REDUCE), new ParserInstruction(18, ParserInstruction.Instruction.REDUCE), new ParserInstruction(18, ParserInstruction.Instruction.REDUCE), new ParserInstruction(18, ParserInstruction.Instruction.REDUCE), new ParserInstruction(18, ParserInstruction.Instruction.REDUCE), new ParserInstruction(18, ParserInstruction.Instruction.REDUCE), new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(0, ParserInstruction.Instruction.ERR)},
new ParserInstruction[] {new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(23, ParserInstruction.Instruction.SHIFT), new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(20, ParserInstruction.Instruction.GOTO), new ParserInstruction(21, ParserInstruction.Instruction.GOTO), new ParserInstruction(22, ParserInstruction.Instruction.GOTO), new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(0, ParserInstruction.Instruction.ERR)},
new ParserInstruction[] {new ParserInstruction(17, ParserInstruction.Instruction.REDUCE), new ParserInstruction(17, ParserInstruction.Instruction.REDUCE), new ParserInstruction(17, ParserInstruction.Instruction.REDUCE), new ParserInstruction(17, ParserInstruction.Instruction.REDUCE), new ParserInstruction(17, ParserInstruction.Instruction.REDUCE), new ParserInstruction(17, ParserInstruction.Instruction.REDUCE), new ParserInstruction(17, ParserInstruction.Instruction.REDUCE), new ParserInstruction(17, ParserInstruction.Instruction.REDUCE), new ParserInstruction(17, ParserInstruction.Instruction.REDUCE), new ParserInstruction(17, ParserInstruction.Instruction.REDUCE), new ParserInstruction(17, ParserInstruction.Instruction.REDUCE), new ParserInstruction(17, ParserInstruction.Instruction.REDUCE), new ParserInstruction(17, ParserInstruction.Instruction.REDUCE), new ParserInstruction(17, ParserInstruction.Instruction.REDUCE), new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(0, ParserInstruction.Instruction.ERR)},
new ParserInstruction[] {new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(24, ParserInstruction.Instruction.SHIFT), new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(23, ParserInstruction.Instruction.SHIFT), new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(25, ParserInstruction.Instruction.GOTO), new ParserInstruction(26, ParserInstruction.Instruction.GOTO), new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(0, ParserInstruction.Instruction.ERR)},
new ParserInstruction[] {new ParserInstruction(4, ParserInstruction.Instruction.REDUCE), new ParserInstruction(4, ParserInstruction.Instruction.REDUCE), new ParserInstruction(4, ParserInstruction.Instruction.REDUCE), new ParserInstruction(4, ParserInstruction.Instruction.REDUCE), new ParserInstruction(4, ParserInstruction.Instruction.REDUCE), new ParserInstruction(4, ParserInstruction.Instruction.REDUCE), new ParserInstruction(4, ParserInstruction.Instruction.REDUCE), new ParserInstruction(4, ParserInstruction.Instruction.REDUCE), new ParserInstruction(4, ParserInstruction.Instruction.REDUCE), new ParserInstruction(4, ParserInstruction.Instruction.REDUCE), new ParserInstruction(4, ParserInstruction.Instruction.REDUCE), new ParserInstruction(4, ParserInstruction.Instruction.REDUCE), new ParserInstruction(4, ParserInstruction.Instruction.REDUCE), new ParserInstruction(4, ParserInstruction.Instruction.REDUCE), new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(0, ParserInstruction.Instruction.ERR)},
new ParserInstruction[] {new ParserInstruction(5, ParserInstruction.Instruction.REDUCE), new ParserInstruction(5, ParserInstruction.Instruction.REDUCE), new ParserInstruction(5, ParserInstruction.Instruction.REDUCE), new ParserInstruction(5, ParserInstruction.Instruction.REDUCE), new ParserInstruction(5, ParserInstruction.Instruction.REDUCE), new ParserInstruction(5, ParserInstruction.Instruction.REDUCE), new ParserInstruction(5, ParserInstruction.Instruction.REDUCE), new ParserInstruction(5, ParserInstruction.Instruction.REDUCE), new ParserInstruction(5, ParserInstruction.Instruction.REDUCE), new ParserInstruction(5, ParserInstruction.Instruction.REDUCE), new ParserInstruction(5, ParserInstruction.Instruction.REDUCE), new ParserInstruction(5, ParserInstruction.Instruction.REDUCE), new ParserInstruction(5, ParserInstruction.Instruction.REDUCE), new ParserInstruction(5, ParserInstruction.Instruction.REDUCE), new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(0, ParserInstruction.Instruction.ERR)},
new ParserInstruction[] {new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(31, ParserInstruction.Instruction.SHIFT), new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(29, ParserInstruction.Instruction.SHIFT), new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(28, ParserInstruction.Instruction.GOTO), new ParserInstruction(27, ParserInstruction.Instruction.GOTO), new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(30, ParserInstruction.Instruction.GOTO), new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(0, ParserInstruction.Instruction.ERR)},
new ParserInstruction[] {new ParserInstruction(3, ParserInstruction.Instruction.REDUCE), new ParserInstruction(3, ParserInstruction.Instruction.REDUCE), new ParserInstruction(3, ParserInstruction.Instruction.REDUCE), new ParserInstruction(3, ParserInstruction.Instruction.REDUCE), new ParserInstruction(3, ParserInstruction.Instruction.REDUCE), new ParserInstruction(3, ParserInstruction.Instruction.REDUCE), new ParserInstruction(3, ParserInstruction.Instruction.REDUCE), new ParserInstruction(3, ParserInstruction.Instruction.REDUCE), new ParserInstruction(3, ParserInstruction.Instruction.REDUCE), new ParserInstruction(3, ParserInstruction.Instruction.REDUCE), new ParserInstruction(3, ParserInstruction.Instruction.REDUCE), new ParserInstruction(3, ParserInstruction.Instruction.REDUCE), new ParserInstruction(3, ParserInstruction.Instruction.REDUCE), new ParserInstruction(3, ParserInstruction.Instruction.REDUCE), new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(0, ParserInstruction.Instruction.ERR)},
new ParserInstruction[] {new ParserInstruction(6, ParserInstruction.Instruction.REDUCE), new ParserInstruction(6, ParserInstruction.Instruction.REDUCE), new ParserInstruction(6, ParserInstruction.Instruction.REDUCE), new ParserInstruction(6, ParserInstruction.Instruction.REDUCE), new ParserInstruction(6, ParserInstruction.Instruction.REDUCE), new ParserInstruction(6, ParserInstruction.Instruction.REDUCE), new ParserInstruction(6, ParserInstruction.Instruction.REDUCE), new ParserInstruction(6, ParserInstruction.Instruction.REDUCE), new ParserInstruction(6, ParserInstruction.Instruction.REDUCE), new ParserInstruction(6, ParserInstruction.Instruction.REDUCE), new ParserInstruction(6, ParserInstruction.Instruction.REDUCE), new ParserInstruction(6, ParserInstruction.Instruction.REDUCE), new ParserInstruction(6, ParserInstruction.Instruction.REDUCE), new ParserInstruction(6, ParserInstruction.Instruction.REDUCE), new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(0, ParserInstruction.Instruction.ERR)},
new ParserInstruction[] {new ParserInstruction(7, ParserInstruction.Instruction.REDUCE), new ParserInstruction(7, ParserInstruction.Instruction.REDUCE), new ParserInstruction(7, ParserInstruction.Instruction.REDUCE), new ParserInstruction(7, ParserInstruction.Instruction.REDUCE), new ParserInstruction(7, ParserInstruction.Instruction.REDUCE), new ParserInstruction(7, ParserInstruction.Instruction.REDUCE), new ParserInstruction(7, ParserInstruction.Instruction.REDUCE), new ParserInstruction(7, ParserInstruction.Instruction.REDUCE), new ParserInstruction(7, ParserInstruction.Instruction.REDUCE), new ParserInstruction(7, ParserInstruction.Instruction.REDUCE), new ParserInstruction(7, ParserInstruction.Instruction.REDUCE), new ParserInstruction(7, ParserInstruction.Instruction.REDUCE), new ParserInstruction(7, ParserInstruction.Instruction.REDUCE), new ParserInstruction(7, ParserInstruction.Instruction.REDUCE), new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(0, ParserInstruction.Instruction.ERR)},
new ParserInstruction[] {new ParserInstruction(8, ParserInstruction.Instruction.REDUCE), new ParserInstruction(8, ParserInstruction.Instruction.REDUCE), new ParserInstruction(8, ParserInstruction.Instruction.REDUCE), new ParserInstruction(8, ParserInstruction.Instruction.REDUCE), new ParserInstruction(8, ParserInstruction.Instruction.REDUCE), new ParserInstruction(8, ParserInstruction.Instruction.REDUCE), new ParserInstruction(8, ParserInstruction.Instruction.REDUCE), new ParserInstruction(8, ParserInstruction.Instruction.REDUCE), new ParserInstruction(8, ParserInstruction.Instruction.REDUCE), new ParserInstruction(8, ParserInstruction.Instruction.REDUCE), new ParserInstruction(8, ParserInstruction.Instruction.REDUCE), new ParserInstruction(8, ParserInstruction.Instruction.REDUCE), new ParserInstruction(8, ParserInstruction.Instruction.REDUCE), new ParserInstruction(32, ParserInstruction.Instruction.SHIFT), new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(0, ParserInstruction.Instruction.ERR)},
new ParserInstruction[] {new ParserInstruction(9, ParserInstruction.Instruction.REDUCE), new ParserInstruction(9, ParserInstruction.Instruction.REDUCE), new ParserInstruction(9, ParserInstruction.Instruction.REDUCE), new ParserInstruction(9, ParserInstruction.Instruction.REDUCE), new ParserInstruction(9, ParserInstruction.Instruction.REDUCE), new ParserInstruction(9, ParserInstruction.Instruction.REDUCE), new ParserInstruction(9, ParserInstruction.Instruction.REDUCE), new ParserInstruction(9, ParserInstruction.Instruction.REDUCE), new ParserInstruction(9, ParserInstruction.Instruction.REDUCE), new ParserInstruction(31, ParserInstruction.Instruction.SHIFT), new ParserInstruction(9, ParserInstruction.Instruction.REDUCE), new ParserInstruction(9, ParserInstruction.Instruction.REDUCE), new ParserInstruction(9, ParserInstruction.Instruction.REDUCE), new ParserInstruction(9, ParserInstruction.Instruction.REDUCE), new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(33, ParserInstruction.Instruction.GOTO), new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(0, ParserInstruction.Instruction.ERR)},
new ParserInstruction[] {new ParserInstruction(12, ParserInstruction.Instruction.REDUCE), new ParserInstruction(12, ParserInstruction.Instruction.REDUCE), new ParserInstruction(12, ParserInstruction.Instruction.REDUCE), new ParserInstruction(12, ParserInstruction.Instruction.REDUCE), new ParserInstruction(12, ParserInstruction.Instruction.REDUCE), new ParserInstruction(12, ParserInstruction.Instruction.REDUCE), new ParserInstruction(12, ParserInstruction.Instruction.REDUCE), new ParserInstruction(12, ParserInstruction.Instruction.REDUCE), new ParserInstruction(12, ParserInstruction.Instruction.REDUCE), new ParserInstruction(12, ParserInstruction.Instruction.REDUCE), new ParserInstruction(12, ParserInstruction.Instruction.REDUCE), new ParserInstruction(12, ParserInstruction.Instruction.REDUCE), new ParserInstruction(12, ParserInstruction.Instruction.REDUCE), new ParserInstruction(12, ParserInstruction.Instruction.REDUCE), new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(0, ParserInstruction.Instruction.ERR)},
new ParserInstruction[] {new ParserInstruction(10, ParserInstruction.Instruction.REDUCE), new ParserInstruction(10, ParserInstruction.Instruction.REDUCE), new ParserInstruction(10, ParserInstruction.Instruction.REDUCE), new ParserInstruction(10, ParserInstruction.Instruction.REDUCE), new ParserInstruction(10, ParserInstruction.Instruction.REDUCE), new ParserInstruction(10, ParserInstruction.Instruction.REDUCE), new ParserInstruction(10, ParserInstruction.Instruction.REDUCE), new ParserInstruction(10, ParserInstruction.Instruction.REDUCE), new ParserInstruction(10, ParserInstruction.Instruction.REDUCE), new ParserInstruction(10, ParserInstruction.Instruction.REDUCE), new ParserInstruction(10, ParserInstruction.Instruction.REDUCE), new ParserInstruction(10, ParserInstruction.Instruction.REDUCE), new ParserInstruction(10, ParserInstruction.Instruction.REDUCE), new ParserInstruction(10, ParserInstruction.Instruction.REDUCE), new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(0, ParserInstruction.Instruction.ERR)},
new ParserInstruction[] {new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(34, ParserInstruction.Instruction.SHIFT), new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(0, ParserInstruction.Instruction.ERR)},
new ParserInstruction[] {new ParserInstruction(13, ParserInstruction.Instruction.REDUCE), new ParserInstruction(13, ParserInstruction.Instruction.REDUCE), new ParserInstruction(13, ParserInstruction.Instruction.REDUCE), new ParserInstruction(13, ParserInstruction.Instruction.REDUCE), new ParserInstruction(13, ParserInstruction.Instruction.REDUCE), new ParserInstruction(13, ParserInstruction.Instruction.REDUCE), new ParserInstruction(13, ParserInstruction.Instruction.REDUCE), new ParserInstruction(13, ParserInstruction.Instruction.REDUCE), new ParserInstruction(13, ParserInstruction.Instruction.REDUCE), new ParserInstruction(13, ParserInstruction.Instruction.REDUCE), new ParserInstruction(13, ParserInstruction.Instruction.REDUCE), new ParserInstruction(13, ParserInstruction.Instruction.REDUCE), new ParserInstruction(13, ParserInstruction.Instruction.REDUCE), new ParserInstruction(13, ParserInstruction.Instruction.REDUCE), new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(0, ParserInstruction.Instruction.ERR)},
new ParserInstruction[] {new ParserInstruction(11, ParserInstruction.Instruction.REDUCE), new ParserInstruction(11, ParserInstruction.Instruction.REDUCE), new ParserInstruction(11, ParserInstruction.Instruction.REDUCE), new ParserInstruction(11, ParserInstruction.Instruction.REDUCE), new ParserInstruction(11, ParserInstruction.Instruction.REDUCE), new ParserInstruction(11, ParserInstruction.Instruction.REDUCE), new ParserInstruction(11, ParserInstruction.Instruction.REDUCE), new ParserInstruction(11, ParserInstruction.Instruction.REDUCE), new ParserInstruction(11, ParserInstruction.Instruction.REDUCE), new ParserInstruction(11, ParserInstruction.Instruction.REDUCE), new ParserInstruction(11, ParserInstruction.Instruction.REDUCE), new ParserInstruction(11, ParserInstruction.Instruction.REDUCE), new ParserInstruction(11, ParserInstruction.Instruction.REDUCE), new ParserInstruction(11, ParserInstruction.Instruction.REDUCE), new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(0, ParserInstruction.Instruction.ERR)},
new ParserInstruction[] {new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(35, ParserInstruction.Instruction.SHIFT), new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(0, ParserInstruction.Instruction.ERR)},
new ParserInstruction[] {new ParserInstruction(15, ParserInstruction.Instruction.REDUCE), new ParserInstruction(15, ParserInstruction.Instruction.REDUCE), new ParserInstruction(15, ParserInstruction.Instruction.REDUCE), new ParserInstruction(15, ParserInstruction.Instruction.REDUCE), new ParserInstruction(15, ParserInstruction.Instruction.REDUCE), new ParserInstruction(15, ParserInstruction.Instruction.REDUCE), new ParserInstruction(15, ParserInstruction.Instruction.REDUCE), new ParserInstruction(15, ParserInstruction.Instruction.REDUCE), new ParserInstruction(15, ParserInstruction.Instruction.REDUCE), new ParserInstruction(15, ParserInstruction.Instruction.REDUCE), new ParserInstruction(15, ParserInstruction.Instruction.REDUCE), new ParserInstruction(15, ParserInstruction.Instruction.REDUCE), new ParserInstruction(15, ParserInstruction.Instruction.REDUCE), new ParserInstruction(15, ParserInstruction.Instruction.REDUCE), new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(0, ParserInstruction.Instruction.ERR), new ParserInstruction(0, ParserInstruction.Instruction.ERR)},

};





















		#region Messy
		static string fullIOPath = Application.persistentDataPath + "/";
		static string tokenName = "SymbolicToken";
		static string startCoroutineTarget = "GameManager._instance";
		static string interfaceCooperationSnippet = "public ParserInstruction[][] GetParseTable() { return parseTable; }	public ProductionRule[] GetProductionRules() { return productionRules; } public SymbolStringTuple[] GetRegexPairs() { return regexPairs; }	public List<object> GetNonTerminals() {	return nonTerminals; } public List<object> GetTERMINALS() { return TERMINALS; }";
		static string coroutineInputProcessingSnippet = "List<object> rhs = null;\n yield return" + startCoroutineTarget + "StartCoroutine(target.Prepare(enviroment, (object data) => { rhs = (List<object>)data; }))";
		List<string> terminalTokens = new List<string>();
		List<string> nonTerminalTokens = new List<string>();
		List<string> regexList = new List<string>();
		List<StringProductionRule> productionList = new List<StringProductionRule>();
		string ParseTableString;
		List<string> coroutines = new List<string>();

		int coroutineCount = 0;
		string GetUniqueCoroutineName() {
			return "Execution" + (coroutineCount++);
		}


		public class StringProductionRule {
			public int nonTerminal = 0;
			public string nonTerminalString = "";
			public List<string> rhs = new List<string>();
			public string coroutineName = "";
			public StringProductionRule(int nonTerminal, string nonTerminalString, List<string> rhs, string coroutineName) {
				this.nonTerminalString = nonTerminalString;
				this.nonTerminal = nonTerminal;
				this.rhs = rhs;
				this.coroutineName = coroutineName;
			}

			public override string ToString() {
				//ProductionRule(int result, int rhsSize, string coroutineName) 
				return "new ProductionRule(" + nonTerminal+","+ rhs.Count + "," + coroutineName + ")";
			}
		}
		
		/// <summary>
		/// Builds a ParseTable for the Grammar
		/// </summary>
		/// <returns></returns>
		public IEnumerator<object> BuildParseTable() {
			terminalTokens = new List<string> { "_WS", "_COMMENT", "_NEWLINE", "NAMESTRING", "EXTSTRING", "GLOBALSTRING", "USINGSTRING", "PRODUCTIONSTRING", "HEADERTAG", "COLON", "DISCARDIDEN", "IDENTIFIER", "REGEX", "CODE" };
			nonTerminalTokens = new List<string> { "LanguageDefinition", "IndetifierList", "ProductionHeader", "TokenStatementList", "RegexRule", "ProductionRule", "ProductionRuleRHSList", "RegexRuleRHSList", "RegesRuleRHS", "ProductionRuleRHS", "Corotuine", "GlobalHeader", "UsingHeader", "NameHeader", "ExtHeader" };


			//Manually place productionRules for 
			productionList.Add(new StringProductionRule(0,  "LanguageDefinition",	new List<string> { "NameHeader", "ExtHeader", "UsingHeader", "GlobalHeader", "ProductionHeader" }, "Execution14"));
			productionList.Add(new StringProductionRule(1,  "IdentifierList",		new List<string> { "IDENTIFIER" }, "Execution15"));
			productionList.Add(new StringProductionRule(1,  "IdentifierList",		new List<string> { "IdentifierList", "IDENTIFIER" }, "Execution16"));
			productionList.Add(new StringProductionRule(2,  "ProductionHeader",		new List<string> { "HEADERTAG", "PRODUCTIONSTRING", "TokenStatementList", "HEADERTAG" }, "Execution17"));
			productionList.Add(new StringProductionRule(3,  "TokenStatementList",	new List<string> { "RegexRule" }, "Execution18"));
			productionList.Add(new StringProductionRule(3,  "TokenStatementList",	new List<string> { "ProductionRule" }, "Execution19"));
			productionList.Add(new StringProductionRule(3,  "TokenStatementList",	new List<string> { "TokenStatementList", "RegexRule" }, "Execution20"));
			productionList.Add(new StringProductionRule(3,  "TokenStatementList",	new List<string> { "TokenStatementList", "ProductionRule" }, "Execution21"));
			productionList.Add(new StringProductionRule(4,  "RegexRule",			new List<string> { "IDENTIFIER", "RegexRuleRHSList" }, "Execution22"));
			productionList.Add(new StringProductionRule(5,  "ProductionRule",		new List<string> { "IDENTIFIER", "ProductionRuleRHSList" }, "Execution23"));
			productionList.Add(new StringProductionRule(6,  "ProductionRuleRHSList",new List<string> { "ProductionRuleRHS" }, "Execution24"));
			productionList.Add(new StringProductionRule(6,  "ProductionRuleRHSList",new List<string> { "ProductionRuleRHSList", "ProductionRuleRHS" }, "Execution25"));
			productionList.Add(new StringProductionRule(7,  "RegexRuleRHSList",		new List<string> { "RegexRuleRHS" }, "Execution26"));
			productionList.Add(new StringProductionRule(7,  "RegexRuleRHSList",		new List<string> { "RegexRuleRHSList", "RegexRuleRHS" }, "Execution27"));
			productionList.Add(new StringProductionRule(8,	"RegexRuleRHS",			new List<string> { "COLON", "REGEX", "Coroutine" }, "Execution28"));
			productionList.Add(new StringProductionRule(9,  "ProductionRuleRHS",	new List<string> { "COLON", "IdentifierList", "Coroutine" }, "Execution29"));
			productionList.Add(new StringProductionRule(10, "Coroutine",			new List<string> { "CODE" }, "Execution30"));
			productionList.Add(new StringProductionRule(11, "GlobalHeader",			new List<string> { "HEADERTAG", "GLOBALSTRING", "CODE" }, "Execution31"));
			productionList.Add(new StringProductionRule(12, "UsingHeader",			new List<string> { "HEADERTAG", "USINGSTRING", "CODE" }, "Execution32"));
			productionList.Add(new StringProductionRule(13, "NameHeader",			new List<string> { "HEADERTAG", "NAMESTRING", "CODE" }, "Execution33"));
			productionList.Add(new StringProductionRule(14, "ExtHeader",			new List<string> { "HEADERTAG", "EXTSTRING", "CODE" }, "Execution34"));

			int stateIter = 0;
			List<ParserInstruction[]> ParseTable = new List<ParserInstruction[]>();

			List<ParserState> stateList = new List<ParserState>();
			stateList.Add(new ParserState(new ParserState.StateRule[] { new ParserState.StateRule(0, 0) }, this));
			ParserState.count++; //Count the init state.

			while (stateIter < stateList.Count) {
				//TODO, start a row for this parser state.
				//Create states for our leads.
				//Debug.Log(stateList[stateIter].ToString());
				//Debug.Log(stateList[stateIter].TransitionString());

				ParserInstruction[] parseTableRow = GenerateEmptyRow(terminalTokens.Count + nonTerminalTokens.Count);

				foreach (string key in stateList[stateIter].transitionSet.Keys) {
					//if (key == SymbolicToken.Type) PrintList(stateList[stateIter].transitionSet[key]);
					//Do not do anything if this leads nowhere.
					if (stateList[stateIter].transitionSet[key].Count == 0)
						continue;

					//Check if this state matches an already exiting one.
					ParserState newState = new ParserState(stateList[stateIter].transitionSet[key].ToArray(), this);
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

					int column = (terminalTokens.Contains(key) == true) ? terminalTokens.IndexOf(key) : terminalTokens.Count + nonTerminalTokens.IndexOf(key);
					ParserInstruction.Instruction instruction = (nonTerminalTokens.Contains(key)) ? ParserInstruction.Instruction.GOTO : ParserInstruction.Instruction.SHIFT;
					parseTableRow[column] = new ParserInstruction(newState.ID, instruction);

				}

				//For each rule that has ended, add a reduce or goto
				for (int i = 0; i < stateList[stateIter].rules.Count; i++) {
					ParserState.StateRule sRule = stateList[stateIter].rules[i];
					if (sRule.dotPosition >= productionList[sRule.rule].rhs.Count) {
						if (productionList[sRule.rule].nonTerminal == productionList[0].nonTerminal) {
							//Add Accept token at EOF
							parseTableRow[terminalTokens.Count - 1] = new ParserInstruction(0, ParserInstruction.Instruction.ACCEPT);
							continue;
						}
						for (int k = 0; k < terminalTokens.Count; k++) {
							ParserInstruction newRule = new ParserInstruction(sRule.rule, ParserInstruction.Instruction.REDUCE);
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

			//Save the Parse table for later
			//[TODO] WriteTableOut(ParseTable);
			string r = "public ParserInstruction[][] parseTable = new ParserInstruction[][] { ";
			for (int x = 0; x < ParseTable.Count; x++) {
				r += "new ParserInstruction[] {";
				for (int y = 0; y < ParseTable[0].Length; y++) {
					r += ParseTable[x][y].ToGetString();
					if (y != ParseTable[0].Length - 1)
						r += ", ";
				}
				r += "},\n";
			}
			r += "\n};";
			ParseTableString = r;

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
		/// A self constructing parsing state based on some initial rules
		/// </summary>
		private class ParserState {

			public OuroborosLanguage languageContext;

			/// <summary>
			/// Internat count of the number of states.
			/// </summary>
			public static int count = 0;

			/// <summary>
			/// The ID of this state.
			/// </summary>
			public int ID;

			public Dictionary<string, List<StateRule>> transitionSet = new Dictionary<string, List<StateRule>>();
			public List<StateRule> rules = new List<StateRule>();

			public List<StringProductionRule> GetStringProductionRulesFromNonTerminal(string nonTerminal) {
				List<StringProductionRule> results = new List<StringProductionRule>();
				foreach (StringProductionRule spr in languageContext.productionList)
					if (spr.nonTerminalString == nonTerminal)
						results.Add(spr);

				return results;
			}

			public ParserState(StateRule[] initialRules, OuroborosLanguage languageContext) {
				this.languageContext = languageContext;
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
					if (targetRule.dotPosition >= languageContext.productionList[targetRule.rule].rhs.Count) {
						continue;
					}

					string dotTarget = languageContext.productionList[targetRule.rule].rhs[targetRule.dotPosition];

					if (transitionSet.ContainsKey(dotTarget) == false)
						transitionSet.Add(dotTarget, new List<StateRule>());
					transitionSet[dotTarget].Add(new StateRule(targetRule.dotPosition + 1, targetRule.rule));

					//Check to see if dotPosition is a nonTerminal
					if (languageContext.nonTerminalTokens.Contains(dotTarget)) {
						//Debug.Log(System.Enum.GetName(typeof(SymbolicToken), dotTarget));

						//If it is, add the necessary production rules.
						List<StringProductionRule> tokenRules = GetStringProductionRulesFromNonTerminal(dotTarget);
						//Debug.Log(tokenRules.Count);
						foreach (StringProductionRule r in tokenRules) {
							StateRule nR = new StateRule(0, languageContext.productionList.IndexOf(r));
							if (rules.Contains(nR) == false && toExpand.Contains(nR) == false) {
								toExpand.Enqueue(nR);
							}
						}
					}
				}

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
				public int rule;

				public StateRule(int dotPosition, int rule) {
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
					else {
						return false;
					}
				}
				/*
				public override string ToString() {
					string ret = "";
					ret += productionList[rule].nonTerminal;
					ret += " -> ";
					for (int i = 0; i < rule.rhs.Count; i++) {
						if (i == dotPosition)
							ret += "•";
						ret += rule.rhs[i];
						ret += " ";
					}
					if (rule.rhs.Count == dotPosition)
						ret += "•";
					return ret;
				}*/
			}
		}
		
		
		
		//%_WS 					:	"(\s)" 																		
		public static IEnumerator<object> Execution0(CoroutineWrapper coroutineWrapper) { List<object> rhs = null; yield return GameManager._instance.StartCoroutine(coroutineWrapper.target.Prepare(coroutineWrapper.enviroment, (object data) => { rhs = (List<object>)data; })); coroutineWrapper.callback(rhs[0]); yield return null; }
		//%_COMMENT				:	"(//.\n)"																	
		public static IEnumerator<object> Execution1(CoroutineWrapper coroutineWrapper) { List<object> rhs = null; yield return GameManager._instance.StartCoroutine(coroutineWrapper.target.Prepare(coroutineWrapper.enviroment, (object data) => { rhs = (List<object>)data; })); coroutineWrapper.callback(rhs[0]); yield return null; }
		//%_NEWLINE				:	"(\n)"																		
		public static IEnumerator<object> Execution2(CoroutineWrapper coroutineWrapper) { List<object> rhs = null; yield return GameManager._instance.StartCoroutine(coroutineWrapper.target.Prepare(coroutineWrapper.enviroment, (object data) => { rhs = (List<object>)data; })); coroutineWrapper.callback(rhs[0]); yield return null; }
		//NAMESTRING				:	"(Name)"																	
		public static IEnumerator<object> Execution3(CoroutineWrapper coroutineWrapper) { List<object> rhs = null; yield return GameManager._instance.StartCoroutine(coroutineWrapper.target.Prepare(coroutineWrapper.enviroment, (object data) => { rhs = (List<object>)data; })); coroutineWrapper.callback(rhs[0]); yield return null; }
		//EXTSTRING				:	"(Ext)"																		
		public static IEnumerator<object> Execution4(CoroutineWrapper coroutineWrapper) { List<object> rhs = null; yield return GameManager._instance.StartCoroutine(coroutineWrapper.target.Prepare(coroutineWrapper.enviroment, (object data) => { rhs = (List<object>)data; })); coroutineWrapper.callback(rhs[0]); yield return null; }
		//GLOBALSTRING			:	"(Global)"																	
		public static IEnumerator<object> Execution5(CoroutineWrapper coroutineWrapper) { List<object> rhs = null; yield return GameManager._instance.StartCoroutine(coroutineWrapper.target.Prepare(coroutineWrapper.enviroment, (object data) => { rhs = (List<object>)data; })); coroutineWrapper.callback(rhs[0]); yield return null; }
		//USINGSTRING				:	"(Using)"																	
		public static IEnumerator<object> Execution6(CoroutineWrapper coroutineWrapper) { List<object> rhs = null; yield return GameManager._instance.StartCoroutine(coroutineWrapper.target.Prepare(coroutineWrapper.enviroment, (object data) => { rhs = (List<object>)data; })); coroutineWrapper.callback(rhs[0]); yield return null; }
		//PRODUCTIONSTRING		:	"(Prod)"																	
		public static IEnumerator<object> Execution7(CoroutineWrapper coroutineWrapper) { List<object> rhs = null; yield return GameManager._instance.StartCoroutine(coroutineWrapper.target.Prepare(coroutineWrapper.enviroment, (object data) => { rhs = (List<object>)data; })); coroutineWrapper.callback(rhs[0]); yield return null; }
		//HEADERTAG				:	"(%%)"																		
		public static IEnumerator<object> Execution8(CoroutineWrapper coroutineWrapper) { List<object> rhs = null; yield return GameManager._instance.StartCoroutine(coroutineWrapper.target.Prepare(coroutineWrapper.enviroment, (object data) => { rhs = (List<object>)data; })); coroutineWrapper.callback(rhs[0]); yield return null; }
		//COLON					:	"(:)"																		
		public static IEnumerator<object> Execution9(CoroutineWrapper coroutineWrapper) { List<object> rhs = null; yield return GameManager._instance.StartCoroutine(coroutineWrapper.target.Prepare(coroutineWrapper.enviroment, (object data) => { rhs = (List<object>)data; })); coroutineWrapper.callback(rhs[0]); yield return null; }
		//DISCARDIDEN				:	"(%_)([a-zA-Z]([a-zA-Z]|[0-9])*)" 											
		public static IEnumerator<object> Execution10(CoroutineWrapper coroutineWrapper) { List<object> rhs = null; yield return GameManager._instance.StartCoroutine(coroutineWrapper.target.Prepare(coroutineWrapper.enviroment, (object data) => { rhs = (List<object>)data; })); coroutineWrapper.callback(rhs[0]); yield return null; }     //An identifier preceeded by %_ is an identifier that is read by the scanner but is discarded from use. (will not appear in token stream)
																																				//IDENTIFIER				:	"([a-zA-Z]([a-zA-Z]|[0-9])*)"												
		public static IEnumerator<object> Execution11(CoroutineWrapper coroutineWrapper) { List<object> rhs = null; yield return GameManager._instance.StartCoroutine(coroutineWrapper.target.Prepare(coroutineWrapper.enviroment, (object data) => { rhs = (List<object>)data; })); coroutineWrapper.callback(rhs[0]); yield return null; } // A token name
																																			//REGEX					:	"(\"([^\"]*)\")"															
		public static IEnumerator<object> Execution12(CoroutineWrapper coroutineWrapper) { List<object> rhs = null; yield return GameManager._instance.StartCoroutine(coroutineWrapper.target.Prepare(coroutineWrapper.enviroment, (object data) => { rhs = (List<object>)data; })); coroutineWrapper.callback(rhs[0]); yield return null; }                 //String used for Regex matching
																																							//CODE					:	"(\{.\})"																	
		public static IEnumerator<object> Execution13(CoroutineWrapper coroutineWrapper) { List<object> rhs = null; yield return GameManager._instance.StartCoroutine(coroutineWrapper.target.Prepare(coroutineWrapper.enviroment, (object data) => { rhs = (List<object>)data; })); string code = ((string)rhs[0]).Substring(1, ((string)rhs[0]).Length - 2); /*chopoff the brackets*/ coroutineWrapper.callback(code); yield return null; }                    //A code section to placed in the resulting language definition. //WARNING: CODE NEEDS REGEX FOR FINDING MATCHING PAIRS NOT A DIRECT COMPARISON.


		//LanguageDefinition		:	NameHeader ExtHeader UsingHeader GlobalHeader ProductionHeader
		public static IEnumerator<object> Execution14(CoroutineWrapper coroutineWrapper) {
			List<object> rhs = null; yield return GameManager._instance.StartCoroutine(coroutineWrapper.target.Prepare(coroutineWrapper.enviroment, (object data) => { rhs = (List<object>)data; }));
			using (StreamWriter sw = File.CreateText(fullIOPath + rhs[0] +"."+ rhs[1])) { 
				//Print Using Header
				sw.Write((string) rhs[2]);
				//Print Namespace/class Declaration
				sw.Write("namespace OuroborosScripting.GeneratedLanguages {\n\npublic class " + rhs[0] + " : IOuroborosLanguage {\n\n");
				//Print the interface code
				sw.Write(interfaceCooperationSnippet);
				//Print Global Header
				sw.Write((string) rhs[3]);
				//Print Production Header
				sw.Write((string) rhs[4]);

				//Print insctruction table
				sw.Write(((OuroborosLanguage)coroutineWrapper.enviroment).ParseTableString);

				//Close Namespace/Class Declaration
				sw.Write("\n}\n}\n\n");
			}
			yield return null;
		}

		//IdentifierList			:	IDENTIFIER
		public static IEnumerator<object> Execution15(CoroutineWrapper coroutineWrapper) { List<object> rhs = null; yield return GameManager._instance.StartCoroutine(coroutineWrapper.target.Prepare(coroutineWrapper.enviroment, (object data) => { rhs = (List<object>)data; })); List<string> u = new List<string>(); u.Add((string)rhs[0]); coroutineWrapper.callback(u); yield return null; }
		//							:	IdentifierList IDENTIFIER 
		public static IEnumerator<object> Execution16(CoroutineWrapper coroutineWrapper) { List<object> rhs = null; yield return GameManager._instance.StartCoroutine(coroutineWrapper.target.Prepare(coroutineWrapper.enviroment, (object data) => { rhs = (List<object>)data; })); ((List<string>)rhs[0]).Add((string)rhs[1]); coroutineWrapper.callback(rhs[0]); yield return null;}

		//ProductionHeader 		:	HEADERTAG PRODUCTIONSTRING TokenStatementList HEADERTAG 
		public static IEnumerator<object> Execution17(CoroutineWrapper coroutineWrapper) {
			List<object> rhs = null; yield return GameManager._instance.StartCoroutine(coroutineWrapper.target.Prepare(coroutineWrapper.enviroment, (object data) => { rhs = (List<object>)data; }));
			//Create the tokens ENUMs
			string enums = "public enum Symbolictoken {\n";
			foreach (string s in ((OuroborosLanguage)coroutineWrapper.enviroment).terminalTokens) enums += s+",";
			enums += "\n";
			foreach (string s in ((OuroborosLanguage)coroutineWrapper.enviroment).nonTerminalTokens) enums += s+",";
			enums = enums.Substring(0,enums.Length - 2); // Remove the extra comma
			enums += "\n}\n";
																											
			//Create the list of terminals
			string terminalDcl = "public SymbolicToken[] TERMINALS = new SymbolicToken[] {";
			foreach (string s in ((OuroborosLanguage)coroutineWrapper.enviroment).terminalTokens) terminalDcl += tokenName +"."+s+",";
			terminalDcl = terminalDcl.Substring(0,terminalDcl.Length - 2); // Remove the extra comma
			terminalDcl += "}\n";
																											
			//Create the list of non terminals
			string nonTerminalDcl = "public SymbolicToken[] nonTerminals = new SymbolicToken[] {";
			foreach (string s in ((OuroborosLanguage)coroutineWrapper.enviroment).nonTerminalTokens) nonTerminalDcl += tokenName +"."+s+",";
			nonTerminalDcl = nonTerminalDcl.Substring(0,nonTerminalDcl.Length - 2); // Remove the extra comma
			nonTerminalDcl += "}\n";



			string regexPairsDCL = "public SymbolStringTuple[] regexPairs = new SymbolStringTuple[] {\n";
			foreach (string s in ((OuroborosLanguage)coroutineWrapper.enviroment).regexList) regexPairsDCL += s+",\n";
			regexPairsDCL = regexPairsDCL.Substring(0, regexPairsDCL.Length - 3); // Remove the extra comma and new line
			regexPairsDCL += "\n};\n";

			string productionRulesDCL = "public ProductionRule[] productionRules = new ProductionRule[] {\n";


			foreach (StringProductionRule s in ((OuroborosLanguage)coroutineWrapper.enviroment).productionList)
				productionRulesDCL += s + ",\n";


			productionRulesDCL = productionRulesDCL.Substring(0, productionRulesDCL.Length - 3); // Remove the extra comma and new line
			productionRulesDCL += "\n};\n";

			string parseTableDCL = "public ParserInstruction[][] parseTable = new ParserInstruction[][] {\n";
			parseTableDCL += "\n};\n";

			string coroutinesDcl = "";
			foreach (string s in ((OuroborosLanguage)coroutineWrapper.enviroment).coroutines) coroutinesDcl += s+"\n";

			coroutineWrapper.callback(enums + "\n" + terminalDcl + "\n" + nonTerminalDcl + "\n"+ regexPairsDCL +"\n" + productionRulesDCL + "\n" + coroutinesDcl + "\n" + parseTableDCL); yield return null;
		}
		//TokenStatementList	:	RegexRule																	
		public static IEnumerator<object> Execution18(CoroutineWrapper coroutineWrapper) { yield return null;}
		//						:	ProductionRule																
		public static IEnumerator<object> Execution19(CoroutineWrapper coroutineWrapper) { yield return null;}
		//						:	TokenStatementList RegexRule 
		public static IEnumerator<object> Execution20(CoroutineWrapper coroutineWrapper) { yield return null;}
		//						:	TokenStatementList ProductionRule 
		public static IEnumerator<object> Execution21(CoroutineWrapper coroutineWrapper) { yield return null;}


		//RegexRule				:	IDENTIFIER RegexRuleRHSList 
		public static IEnumerator<object> Execution22(CoroutineWrapper coroutineWrapper) {
			List<object> rhs = null; yield return GameManager._instance.StartCoroutine(coroutineWrapper.target.Prepare(coroutineWrapper.enviroment, (object data) => { rhs = (List<object>)data; }));
			int c = ((OuroborosLanguage)coroutineWrapper.enviroment).terminalTokens.Count;
			((OuroborosLanguage)coroutineWrapper.enviroment).terminalTokens.Add((string)rhs [0]);
			foreach (object[] v in (List<object[]>)rhs[1]) 
				{ ((OuroborosLanguage)coroutineWrapper.enviroment).regexList.Add("new SymbolStringTuple("+ c + ", " + ((string)v[0]) + ", " + ((string)v[1])); }
			yield return null;
		}

		//ProductionRule		:	IDENTIFIER ProductionRuleRHSList 
		public static IEnumerator<object> Execution23(CoroutineWrapper coroutineWrapper) {
			List<object> rhs = null; yield return GameManager._instance.StartCoroutine(coroutineWrapper.target.Prepare(coroutineWrapper.enviroment, (object data) => { rhs = (List<object>)data; }));
			int c = ((OuroborosLanguage)coroutineWrapper.enviroment).nonTerminalTokens.Count;
			((OuroborosLanguage)coroutineWrapper.enviroment).nonTerminalTokens.Add((string)rhs[0]);
			foreach (object[] v in (List<object[]>)rhs[1]) 
				{ ((OuroborosLanguage)coroutineWrapper.enviroment).productionList.Add(new StringProductionRule(c, (string)rhs[0], (List<string>)v[0], (string)v[1]));  }
			yield return null;
		}

		//ProductionRuleRHSList	:	ProductionRuleRHS															
		public static IEnumerator<object> Execution24(CoroutineWrapper coroutineWrapper) {
			List<object> rhs = null; yield return GameManager._instance.StartCoroutine(coroutineWrapper.target.Prepare(coroutineWrapper.enviroment, (object data) => { rhs = (List<object>)data; }));
			List<object[]> u = new List<object[]>();
			u.Add((object[]) rhs[0]);
			coroutineWrapper.callback(u);
			yield return null;
		}
		//						:	ProductionRuleRHSList ProductionRuleRHS 
		public static IEnumerator<object> Execution25(CoroutineWrapper coroutineWrapper) {
			List<object> rhs = null; yield return GameManager._instance.StartCoroutine(coroutineWrapper.target.Prepare(coroutineWrapper.enviroment, (object data) => { rhs = (List<object>)data; }));
			((List<object[]>)rhs [0]).Add((object[])rhs[1]); coroutineWrapper.callback(rhs[0]); yield return null;
		}

		//RegexRuleRHSList		:	RegexRuleRHS																
		public static IEnumerator<object> Execution26(CoroutineWrapper coroutineWrapper) {
			List<object> rhs = null; yield return GameManager._instance.StartCoroutine(coroutineWrapper.target.Prepare(coroutineWrapper.enviroment, (object data) => { rhs = (List<object>)data; }));
			List<object[]> u = new List<object[]>();
			u.Add((object[]) rhs[0]);
			coroutineWrapper.callback(u);
			yield return null;
		}
		//						:	RegexRuleRHSList RegexRuleRHS 
		public static IEnumerator<object> Execution27(CoroutineWrapper coroutineWrapper) {
			List<object> rhs = null; yield return GameManager._instance.StartCoroutine(coroutineWrapper.target.Prepare(coroutineWrapper.enviroment, (object data) => { rhs = (List<object>)data; }));
			((List<object[]>)rhs[0]).Add((object[])rhs[1]);
			coroutineWrapper.callback(rhs[0]);
			yield return null;
		}

		//RegexRuleRHS 			:	COLON REGEX Coroutine														
		public static IEnumerator<object> Execution28(CoroutineWrapper coroutineWrapper) {
			List<object> rhs = null; yield return GameManager._instance.StartCoroutine(coroutineWrapper.target.Prepare(coroutineWrapper.enviroment, (object data) => { rhs = (List<object>)data; }));
			coroutineWrapper.callback(new object[] { rhs[1], rhs[2] });
			yield return null;
		}
		//ProductionRuleRHS		:	COLON IdentifierList Coroutine												
		public static IEnumerator<object> Execution29(CoroutineWrapper coroutineWrapper) {
			List<object> rhs = null; yield return GameManager._instance.StartCoroutine(coroutineWrapper.target.Prepare(coroutineWrapper.enviroment, (object data) => { rhs = (List<object>)data; }));
			coroutineWrapper.callback(new object[] { rhs[1], rhs[2] });
			yield return null;
		}
		//Coroutine				:	CODE																		
		public static IEnumerator<object> Execution30(CoroutineWrapper coroutineWrapper) {

			List<object> rhs = null; yield return GameManager._instance.StartCoroutine(coroutineWrapper.target.Prepare(coroutineWrapper.enviroment, (object data) => { rhs = (List<object>)data; }));
			/*Prepare a coroutine stub*/
			string name = ((OuroborosLanguage)coroutineWrapper.enviroment).GetUniqueCoroutineName();
			string coroutineCode = "";
			coroutineCode += "public static IEnumerator<object> " + name + "(CoroutineWrapper coroutineWrapper) {\n";
			coroutineCode += coroutineInputProcessingSnippet;
			coroutineCode += rhs[0];
			coroutineCode += "\n}\n";
			((OuroborosLanguage)coroutineWrapper.enviroment).coroutines.Add(coroutineCode);
			coroutineWrapper.callback(name); yield return null;
		}
		//GlobalHeader 			:	HEADERTAG GLOBALSTRING CODE													
		public static IEnumerator<object> Execution31(CoroutineWrapper coroutineWrapper) { List<object> rhs = null; yield return GameManager._instance.StartCoroutine(coroutineWrapper.target.Prepare(coroutineWrapper.enviroment, (object data) => { rhs = (List<object>)data; })); coroutineWrapper.callback(rhs[2]); yield return null;}
		//UsingHeader				: 	HEADERTAG USINGSTRING CODE 													
		public static IEnumerator<object> Execution32(CoroutineWrapper coroutineWrapper) { List<object> rhs = null; yield return GameManager._instance.StartCoroutine(coroutineWrapper.target.Prepare(coroutineWrapper.enviroment, (object data) => { rhs = (List<object>)data; })); coroutineWrapper.callback(rhs[2]); yield return null;}
		//NameHeader				: 	HEADERTAG NAMESTRING CODE 													
		public static IEnumerator<object> Execution33(CoroutineWrapper coroutineWrapper) { List<object> rhs = null; yield return GameManager._instance.StartCoroutine(coroutineWrapper.target.Prepare(coroutineWrapper.enviroment, (object data) => { rhs = (List<object>)data; })); coroutineWrapper.callback(rhs[2]); yield return null;}
		//ExtHeader				: 	HEADERTAG EXTSTRING CODE 													
		public static IEnumerator<object> Execution34(CoroutineWrapper coroutineWrapper) { List<object> rhs = null; yield return GameManager._instance.StartCoroutine(coroutineWrapper.target.Prepare(coroutineWrapper.enviroment, (object data) => { rhs = (List<object>)data; })); coroutineWrapper.callback(rhs[2]); yield return null;} 

		#endregion














	}
}