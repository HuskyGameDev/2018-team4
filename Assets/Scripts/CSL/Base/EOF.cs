using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace BoardGameScripting {
	public class EOF : GrammarElement {
		public override string GetRegex() {
			return @"";
		}

		public override TokenType GetTokenType() {
			return TokenType.NonTerminal;
		}
	}
}
