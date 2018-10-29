using System;
using System.Collections.Generic;

namespace BoardGameScripting.CSL {
	public class Statement : GrammarElement {
		private static readonly ProductionRule pr1 = new StatementPR1();

		public static new ProductionRule[] GetRules() {
			return new ProductionRule[] { pr1 };
		}

		public override TokenType GetTokenType() {
			return TokenType.NonTerminal;
		}

		private class StatementPR1 : ProductionRule {
			public override Type GetNonTerminal() {
				return typeof(Statement);
			}

			public override List<Type> GetRHSElements() {
				return new List<Type>(new Type[] { typeof(BoardGameScripting.CSL.Assignment) });
			}
		}

		private class StatementPR2 : ProductionRule {
			public override Type GetNonTerminal() {
				return typeof(Statement);
			}

			public override List<Type> GetRHSElements() {
				return new List<Type>(new Type[] { /*typeof(BoardGameScripting.CSL.VariableDeclaration)*/ });
			}
		}

		private class StatementPR3 : ProductionRule {
			public override Type GetNonTerminal() {
				return typeof(Statement);
			}

			public override List<Type> GetRHSElements() {
				return new List<Type>(new Type[] {/* typeof(BoardGameScripting.CSL.IfStatement) */ });
			}
		}

		private class StatementPR4 : ProductionRule {
			public override Type GetNonTerminal() {
				return typeof(Statement);
			}

			public override List<Type> GetRHSElements() {
				return new List<Type>(new Type[] {/* typeof(BoardGameScripting.CSL.WhileStatement) */ });
			}
		}

		private class StatementPR5 : ProductionRule {
			public override Type GetNonTerminal() {
				return typeof(Statement);
			}

			public override List<Type> GetRHSElements() {
				return new List<Type>(new Type[] {/* typeof(BoardGameScripting.CSL.IOStatement) */ });
			}
		}

		private class StatementPR6 : ProductionRule {
			public override Type GetNonTerminal() {
				return typeof(Statement);
			}

			public override List<Type> GetRHSElements() {
				return new List<Type>(new Type[] { /*typeof(BoardGameScripting.CSL.DiscardStatement) */});
			}
		}
	}
}
