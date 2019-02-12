using System.Collections.Generic;
using CoroutineWrapper = OuroborosScripting.OuroborosInterpreter.CoroutineWrapper;
using CoroutineMethod = OuroborosScripting.OuroborosInterpreter.CoroutineMethod;

namespace OuroborosScripting {
	public interface IOuroborosLanguage {

		List<object> GetTERMINALS();
		List<object> GetNonTerminals();
		OuroborosInterpreter.SymbolStringTuple[] GetRegexPairs();
		OuroborosInterpreter.ProductionRule[] GetProductionRules();
		OuroborosInterpreter.ParserInstruction[][] GetParseTable();
		IEnumerator<object> StartLangCoroutine(CoroutineMethod name, CoroutineWrapper coroutineWrapper);
	}
}
