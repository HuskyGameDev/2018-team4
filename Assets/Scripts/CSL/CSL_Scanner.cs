using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text.RegularExpressions;

public class CSL_Scanner : MonoBehaviour {


	#region Testing
	private void Start() {
		//StartCoroutine(ScanTest(1000));
		//TokenTest();
	}



	/// <summary>
	/// For each token, compare it to the samples of EVERY token for a match, and report the matches
	/// </summary>
	void TokenTest() {
		for (int k = 0; k < CSL.tokenRegexPairs.Length; k++) {

			Regex reg = new Regex(CSL.tokenRegexPairs[k].regex);
			Debug.Log("--" + System.Enum.GetName(typeof(CSL.Token), CSL.tokenRegexPairs[k].token) + "----------");
			for (int i = 0; i < CSL.tokenRegexPairs.Length; i++) {
				if (reg.IsMatch(CSL.tokenRegexPairs[i].sample))
					Debug.Log(CSL.tokenRegexPairs[i].sample + " | " + CSL.tokenRegexPairs[k].regex + " | " + reg.IsMatch(CSL.tokenRegexPairs[i].sample));
			}
			//Debug.Log("");
			//Debug.Log("");

		}
	}


	IEnumerator ScanTest(int testSize) {
		int errCount = 0;
		List<CSL.TokenRegexPair> testingTokens = new List<CSL.TokenRegexPair>();

		for (int i = 0; i < CSL.tokenRegexPairs.Length; i++) {
			//Don't intentionally put errors intot he test
			if (CSL.tokenRegexPairs[i].token == CSL.Token.ERR || CSL.tokenRegexPairs[i].token == CSL.Token.IDENERR)
				continue;
			testingTokens.Add(CSL.tokenRegexPairs[i]);
		}

		for (int i = 0; i < testSize; i++) {
			Debug.Log("---------------------------------------------------------");
			testingTokens.Shuffle();
			string testString = "";
			for (int k = 0; k < testingTokens.Count; k++) {
				testString += testingTokens[k].sample;
				testString += " ";
			}
			//testString = "|x|";

			Debug.Log(testString);
			//Perform the scan
			List<ParsedToken> tokens = Scan(testString);

			//Check to see if there is an ERR token in the scan (which should not happen since we only provided it valid samples)
			bool errFlag = false;
			string tokenString = "";

			for (int k = 0; k < tokens.Count; k++) {
				ParsedToken pToken = tokens[k];
				tokenString += System.Enum.GetName(typeof(CSL.Token), pToken.token) + " ";
				if (pToken.token == CSL.Token.ERR || pToken.token == CSL.Token.IDENERR)
					errFlag = true;
			}
			Debug.Log(tokenString);

			if (errFlag) {
				errCount++;
				//Log this error
			}

			yield return null;
		}
		Debug.Log("---------------------------------------------------------");
		Debug.Log("---------------------------------------------------------");
		Debug.Log("---------------------------------------------------------");
		Debug.Log("Testing completed with " + errCount + " errors.");
	}
	#endregion

	#region Methods
	List<ParsedToken> Scan(string input) {
		List<ParsedToken> tokens = new List<ParsedToken>();
		string remainder = input;
		string text = "";
		int nl = 0;
		Regex regex;
		while (remainder.Length > 0) {

			CSL.Token token = CSL.Token.ERR;
			
			for (int i = 0; i < CSL.tokenRegexPairs.Length; i++) {
				text = "";
				regex = new Regex(CSL.tokenRegexPairs[i].regex);
				for (int k = 0; k < remainder.Length; k++) {
					text += remainder[k];
					//Debug.Log(text + "<=?=>" + CSL.tokenRegexPairs[i].regex);
					//yield return new WaitForEndOfFrame();
					if (regex.IsMatch(text)) {
						token = CSL.tokenRegexPairs[i].token;
						//Debug.Log("Match Found: " + text + "<=?=>" + CSL.tokenRegexPairs[i].regex + " | " + token);
						break;
					}
				}

				if (token != CSL.Token.ERR)
					break;
			}


			 if (token != CSL.Token.ERR) {

				if (token == CSL.Token.WHITESP) {
					//nothing
				}
				else if (token == CSL.Token.NEWLN) {
					nl++;
				}
				else {
					//Debug.Log(text + " | " + System.Enum.GetName(typeof(CSL.Token), token));
					tokens.Add(new ParsedToken(token, CSL.Box(token, text)));
				}
				//Debug.Log("Pre<"+remainder+">");
				remainder = remainder.Substring(text.Length);
				//Debug.Log("Post<" + remainder + ">");
				text = "";
			}
			else {
				//Log ERROR
				Debug.LogError("Syntax Read Error: '" + text + "'");
			}
		}

		return tokens;
	}
	#endregion

	#region Structs
	public struct ParsedToken {
		public CSL.Token token;
		public object data;
		public ParsedToken(CSL.Token token, object data) {
			this.token = token;
			this.data = data;
		}
	}
	#endregion
}
