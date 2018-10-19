using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text.RegularExpressions;

public class CSL_Scanner : MonoBehaviour {


	#region Testing
	private void Start() {
        //StartCoroutine(ScanTest(20));
        //TokenTest();
       //StartCoroutine(CSL.Build());
	}



	/// <summary>
	/// For each token, compare it to the samples of EVERY token for a match, and report the matches
	/// </summary>
	void TokenTest() {
		for (int k = 0; k < CSL.terminalTokens.Length; k++) {

			Regex reg = new Regex(CSL.terminalTokens[k].regex);
			Debug.Log("--" + System.Enum.GetName(typeof(CSL.SymbolicToken), CSL.terminalTokens[k].token) + "----------");
			for (int i = 0; i < CSL.terminalTokens.Length; i++) {
				if (reg.IsMatch(CSL.terminalTokens[i].sample))
					Debug.Log(CSL.terminalTokens[i].sample + " | " + CSL.terminalTokens[k].regex + " | " + reg.IsMatch(CSL.terminalTokens[i].sample));
			}
			//Debug.Log("");
			//Debug.Log("");

		}
	}


	/*IEnumerator ScanTest(int testSize) {
		int errCount = 0;
		List<CSL.TokenRegexPair> testingTokens = new List<CSL.TokenRegexPair>();

		for (int i = 0; i < CSL.terminalTokens.Length; i++) {
			//Don't intentionally put errors intot he test
			if (CSL.terminalTokens[i].token == CSL.SymbolicToken.ERR || CSL.terminalTokens[i].token == CSL.SymbolicToken.IDENERR)
				continue;
			testingTokens.Add(CSL.terminalTokens[i]);
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
			List<CSL.ParsedToken> tokens = Scan(testString).tokens;

			//Check to see if there is an ERR token in the scan (which should not happen since we only provided it valid samples)
			bool errFlag = false;
			string tokenString = "";

			for (int k = 0; k < tokens.Count; k++) {
				CSL.ParsedToken pToken = tokens[k];
				tokenString += System.Enum.GetName(typeof(CSL.SymbolicToken), pToken.token) + " ";
				if (pToken.token == CSL.SymbolicToken.ERR || pToken.token == CSL.SymbolicToken.IDENERR)
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
	}*/
	#endregion

	#region Methods
	/*CSL.Script Scan(string input) {
		List<CSL.ParsedToken> tokens = new List<CSL.ParsedToken>();
		string remainder = input;
		string text = "";
		int nl = 0;
		Regex regex;
		while (remainder.Length > 0) {

			CSL.SymbolicToken token = CSL.SymbolicToken.ERR;
			
			for (int i = 0; i < CSL.terminalTokens.Length; i++) {
				text = "";
				string testingText = ""; // Track the current testing string.
				regex = new Regex(CSL.terminalTokens[i].regex);
				for (int k = 0; k < remainder.Length; k++) {
					testingText += remainder[k];
					//Debug.Log(text + "<=?=>" + CSL.tokenRegexPairs[i].regex);
					//yield return new WaitForEndOfFrame();
					if (regex.IsMatch(testingText)) {
						text = testingText; // Store our string since we know that this is the valid string.
						token = CSL.terminalTokens[i].token;
						//Debug.Log("Match Found: " + text + "<=?=>" + CSL.tokenRegexPairs[i].regex + " | " + token);
						//break;
					}
				}

				if (token != CSL.SymbolicToken.ERR)
					break;
			}


			 if (token != CSL.SymbolicToken.ERR) {

				if (token == CSL.SymbolicToken.WHITESP) {
					//nothing
				}
				else if (token == CSL.SymbolicToken.NEWLN) {
					nl++;
				}
				else {
					//Debug.Log(text + " | " + System.Enum.GetName(typeof(CSL.Token), token));
					tokens.Add(new CSL.ParsedToken(token, CSL.Box(token, text)));
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

		return new CSL.Script(tokens);
	}*/
	#endregion
}
