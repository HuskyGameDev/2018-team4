using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text.RegularExpressions;

public class CSL_TESTER : MonoBehaviour {


	#region Testing
	private void Start() {
		string testStatement = "var t; t = 0; if (1 > 2.333) { while (t<10) { discard this; t = t+1; } } else discard nothing; ";


		//CSL.PrintList(CSL.Scan(testRoom));

		StartCoroutine(CSL.Execute(CSL.Scan(testStatement)));
	}



	/// <summary>
	/// For each token, compare it to the samples of EVERY token for a match, and report the matches
	/// </summary>
	void TokenTest() {
		for (int k = 0; k < CSL.terminalTokenRegexPair.Length; k++) {

			Regex reg = new Regex(CSL.terminalTokenRegexPair[k].regex);
			Debug.Log("--" + System.Enum.GetName(typeof(CSL.SymbolicToken), CSL.terminalTokenRegexPair[k].token) + "----------");
			for (int i = 0; i < CSL.terminalTokenRegexPair.Length; i++) {
				if (reg.IsMatch(CSL.terminalTokenRegexPair[i].sample))
					Debug.Log(CSL.terminalTokenRegexPair[i].sample + " | " + CSL.terminalTokenRegexPair[k].regex + " | " + reg.IsMatch(CSL.terminalTokenRegexPair[i].sample));
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

}
