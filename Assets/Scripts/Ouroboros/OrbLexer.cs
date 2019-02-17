using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text.RegularExpressions;

using ProcessedToken = OrbScripting.OrbDefinitions.ProcessedToken;
using SymbolStringTuple = OrbScripting.OrbDefinitions.SymbolStringTuple;
using Callback = OrbScripting.OrbDefinitions.Callback;
using CoroutineWrapper = OrbScripting.OrbDefinitions.CoroutineWrapper;


namespace OrbScripting {
	/// <summary>
	/// Converts text files to symbolic tokens
	/// </summary>
	public class OrbLexer {
		public enum LexMode { String, Regex, Contained }

		/// <summary>
		/// Scans a string into a token stream
		/// </summary>
		/// <param name="input"></param>
		/// <returns></returns>
		public static IEnumerator<object> Scan(IOrbLanguage enviroment, string input, Callback callback) {
			List<ProcessedToken?> tokens = new List<ProcessedToken?>();

			List<object> TERMINALS = enviroment.GetTERMINALS();
			SymbolStringTuple[] tuples = enviroment.GetRegexPairs();

			string remainder = input;
			string text = "";
			string testingText = "";
			Regex regex;
			while (remainder.Length > 0) {
				//Debug.Log("Top of While");
				ProcessedToken? token = null;
				int i = 0;
				for (; i < TERMINALS.Count; i++) {
					//Debug.Log("For1: " + tuples[i].str);
					text = "";
					testingText = ""; // Track the current testing string.
					regex = new Regex(tuples[i].str);
					//Debug.Log(regex);
					for (int k = 0; k < remainder.Length; k++) {
						//Debug.Log("For2: " + remainder.Length + " | " + k);
						testingText += remainder[k];
						if (regex.IsMatch(testingText)) {
							text = testingText; // Store our string since we know that this is the valid string.
												//Debug.Log("Is Match: " + regex + " => " + testingText);
												//Create the processedtoken
							token = new ProcessedToken(i, null, text);
							//Execute the code for reading this token.
							yield return enviroment.StartLangCoroutine(tuples[i].coroutineName, new CoroutineWrapper(enviroment, (ProcessedToken)token, (object d) => { }));

						}
					}

					if (token != null) {
						//Debug.Log("Breaking.");
						break;
					}
				}

				if (token != null) {
					if (tuples[i].ignorable) {
						//We have ignored this section
					}
					else {
						//Debug.Log(text + " | " + token);
						tokens.Add(token);
					}
					//Debug.Log("Pre<"+remainder+">");
					remainder = remainder.Substring(text.Length);
					//Debug.Log("Post<" + remainder + ">");
					text = "";
				}
				else {
					//Log ERROR
					Debug.LogError("Syntax Read Error: '" + testingText + "'");
				}

				//pause before continuing
				yield return null;
			}

			tokens.Add(null);
			callback(tokens);
		}
	}
}
