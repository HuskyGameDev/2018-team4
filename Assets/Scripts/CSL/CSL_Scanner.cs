using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text.RegularExpressions;

public class CSL_Scanner : MonoBehaviour {

	private void Start() {
		List<ParsedToken> tokens = Scan("while (true) { \nplayer.deck; \n}");
		string ret = "";
		for (int i = 0; i < tokens.Count; i++) {
			ret += System.Enum.GetName(typeof(CSL.Token), tokens[i].token);
			ret += " ";
		}
		Debug.Log(ret);
	}


	public List<ParsedToken> Scan(string input) {
		string text = "";
		int nl = 0;
		Regex regex;
		List<ParsedToken> tokens = new List<ParsedToken>();
		while (input.Length > 0) {
			text += input[0];
			input = input.Substring(1);

			CSL.Token token = CSL.Token.ERR;
			
			for (int i = 0; i < CSL.tokenRegexPairs.Length; i++) {
				regex = new Regex(CSL.tokenRegexPairs[i].regex);
				Debug.Log(text + "<=?=>" + CSL.tokenRegexPairs[i].regex);
				if (regex.IsMatch(text)) {
					token = (CSL.Token)i;
					Debug.Log("Match Found");
					break;
				}
			}

			if (token == CSL.Token.WHITESP) {
				//nothing
			}
			else if (token == CSL.Token.NEWLN) {
				nl++;
			}
			else if (token != CSL.Token.ERR) {
				Debug.Log(text + " | " + System.Enum.GetName(typeof(CSL.Token), token));
				tokens.Add(new ParsedToken(token, CSL.Box(token, text)));
				text = "";
			}
			else {
				//Log ERROR
				Debug.LogError("Syntax Read Error: '" + text + "'");
			}
		}

		Debug.Log("Parsing Successfully completed, reading " + nl + " lines.");
		return tokens;
	}

	public struct ParsedToken {
		public CSL.Token token;
		public object data;
		public ParsedToken(CSL.Token token, object data) {
			this.token = token;
			this.data = data;
		}
	}
}
