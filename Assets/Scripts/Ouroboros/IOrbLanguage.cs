using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using ProcessedToken = OrbScripting.OrbDefinitions.ProcessedToken;
using SymbolStringTuple = OrbScripting.OrbDefinitions.SymbolStringTuple;
using Callback = OrbScripting.OrbDefinitions.Callback;
using CoroutineWrapper = OrbScripting.OrbDefinitions.CoroutineWrapper;
using ProductionRule = OrbScripting.OrbDefinitions.ProductionRule;
using CoroutineMethod = OrbScripting.OrbDefinitions.CoroutineMethod;
using ParserInstruction = OrbScripting.OrbDefinitions.ParserInstruction;

namespace OrbScripting {
	public interface IOrbLanguage {
		List<object> GetTERMINALS();
		List<object> GetNonTerminals();
		SymbolStringTuple[] GetRegexPairs();
		ProductionRule[] GetProductionRules();
		ParserInstruction[][] GetParseTable();
		IEnumerator<object> StartLangCoroutine(CoroutineMethod name, CoroutineWrapper coroutineWrapper);
	}
}