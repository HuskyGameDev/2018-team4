using System;
using System.Collections.Generic;

namespace BoardGameScripting.CSL {
	public class StatementList : GrammarElement {
		private static readonly ProductionRule pr1 = new StatementListPR1();
		private static readonly ProductionRule pr2 = new StatementListPR2();

		public static new ProductionRule[] GetRules() {
			return new ProductionRule[] { pr1, pr2 };
		}

		public override TokenType GetTokenType() {
			return TokenType.NonTerminal;
		}


		private class StatementListPR1 : ProductionRule {
			public override Type GetNonTerminal() {
				return typeof(StatementList);
			}

			public override List<Type> GetRHSElements() {
				return new List<Type>(new Type[] { typeof(BoardGameScripting.CSL.Statement ) });
			}
		}

		private class StatementListPR2 : ProductionRule {
			public override Type GetNonTerminal() {
				return typeof(StatementList);
			}

			public override List<Type> GetRHSElements() {
				return new List<Type>(new Type[] { typeof(BoardGameScripting.CSL.StatementList), typeof(BoardGameScripting.CSL.Statement) });
			}
		}
	}
}
