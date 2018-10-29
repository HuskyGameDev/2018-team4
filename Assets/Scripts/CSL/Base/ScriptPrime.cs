using System;
using System.Collections.Generic;

namespace BoardGameScripting {
	public class ScriptPrime : GrammarElement {
		private static readonly ProductionRule pr1 = new ScriptPrimePR1();

		public static new ProductionRule[] GetRules() {
			return new ProductionRule[] { pr1 };
		}

		public override TokenType GetTokenType() {
			return TokenType.NonTerminal;
		}


		private class ScriptPrimePR1 : ProductionRule {
			public override Type GetNonTerminal() {
				return typeof(ScriptPrime);
			}

			public override List<Type> GetRHSElements() {
				return new List<Type>(new Type[] { typeof(BoardGameScripting.CSL.StatementList) });
			}
		}
	}
}