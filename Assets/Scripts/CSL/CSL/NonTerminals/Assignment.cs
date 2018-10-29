using System;
using System.Collections.Generic;

namespace BoardGameScripting.CSL {
	public class Assignment : GrammarElement {
		private static readonly ProductionRule pr1 = new PR1();

		public static new ProductionRule[] GetRules() {
			return new ProductionRule[] { pr1 };
		}

		public override TokenType GetTokenType() {
			return TokenType.NonTerminal;
		}


		private class PR1 : ProductionRule {
			public override Type GetNonTerminal() {
				return typeof(StatementList);
			}

			public override List<Type> GetRHSElements() {
				return new List<Type>(new Type[] { typeof(Terminals.IDENTIFIER), typeof(Terminals.ASSIGN), /*typeof(Expr),*/ typeof(Terminals.SEMICOLON) });
			}

			public override IEnumerator<object> Execute(GrammarElement element, BGSGrammar.Callback callback) {
				List<object> expressionResults = new List<object>();
				yield return GameManager._instance.StartCoroutine(element.Prepare((object data) => { expressionResults = (List<object>)data; }));
				//[TODO] Stuff
			}
		}

		private class PR2 : ProductionRule {
			public override Type GetNonTerminal() {
				return typeof(StatementList);
			}

			public override List<Type> GetRHSElements() {
				return new List<Type>(new Type[] { typeof(Terminals.IDENTIFIER), typeof(Terminals.LEFTSQR), typeof(Terminals.RIGHTSQR), typeof(Terminals.ASSIGN), /*typeof(Expr),*/ typeof(Terminals.SEMICOLON) });
			}
		}

	}
}
