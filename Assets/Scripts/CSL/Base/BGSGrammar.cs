using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using System.Xml;
using System.Xml.Serialization;
using System.IO;
using ScriptSegment = List<GrammarElement>;

namespace BoardGameScripting {
	/// <summary>
	/// A grammer and rules specification for a Card Scripting Language
	/// </summary>
	public static class BGSGrammar {
		public delegate void Callback(object data);
		private static readonly bool debugParseOutput = false;
		private static List<ParserInstruction[]> ParseTable = null;
		private static List<Type> terminals;
		private static List<Type> nonTerminals;
		private static Dictionary<string, object> variableDictionary;

		#region Methods
		/// <summary>
		/// Scans a string into a token stream
		/// </summary>
		/// <param name="input"></param>
		/// <returns></returns>
		public static IEnumerator<object> Scan(string input, Callback callback) {
			ScriptSegment tokens = new ScriptSegment();
			string remainder = input;
			string text = "";
			Regex regex;
			while (remainder.Length > 0) {

				GrammarElement? token = null;

				for (int i = 0; i < terminalTokenRegexPair.Length; i++) {
					text = "";
					string testingText = ""; // Track the current testing string.
					regex = new Regex(terminalTokenRegexPair[i].regex);
					for (int k = 0; k < remainder.Length; k++) {
						testingText += remainder[k];
						if (regex.IsMatch(testingText)) {
							text = testingText; // Store our string since we know that this is the valid string.
							token = terminalTokenRegexPair[i].token;
						}
					}

					if (token != null) {
						//Debug.Log("Breaking.");
						break;
					}
				}

				if (token != null) {
					if (token.Ignorable()) {
						//We have ignored this section
					}
					else {
						//Debug.Log(text + " | " + System.Enum.GetName(typeof(SymbolicToken), token));
						tokens.Add(new ScannedToken(token, Box(token, text), ScannedToken.Type.Terminal));
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

				//pause before continuing
				yield return null;
			}

			//tokens.Add(new ScannedToken(SymbolicToken.EOF, null, ScannedToken.Type.Terminal));
			callback(tokens);
		}

		/// <summary>
		/// Operates on a Token stream using the ParsingTable
		/// </summary>
		/// <param name="tokens"></param>
		/// <param name="generateScript"></param>
		/// <returns></returns>
		public static IEnumerator<object> Execute(ScriptSegment tokens) {
			//If we do not have a parsing table generated, do so.
			if (ParseTable == null)
				yield return BuildParseTable();

			variableDictionary = new Dictionary<string, object>(); // Reset the variable dictionary

			//The parsing stack
			List<ParseStackElement> parsingStack = new List<ParseStackElement>();
			PrintList(tokens);

			//Place state 0 onto the stack
			parsingStack.Add(new ParseStackElement(ParseStackElement.Type.State, 0));

			//The current location in the parse
			int currencyIndicator = 0;

			//Begin the Looop!
			while (true) {
				yield return null;


				//Debug.Log(PrintList(parsingStack, false) + "|\\|/| -> " + tokens[currencyIndicator]);

				ParseStackElement top = parsingStack[parsingStack.Count -1];
				//Look at top of stack, if it is a Token, then we need to do a GoTo,
				if (top.type == ParseStackElement.Type.Token) {
					//Debug.Log("GoTo");

					//DO A GOTO
					ParseStackElement previousState = parsingStack[parsingStack.Count - 2];

					//Debug.Log(previousState);
					if (previousState.type != ParseStackElement.Type.State) {
						Debug.LogError("Stack parsing has broken due to invalid GoTo");
						yield break;
					}

					//Calculate the column for the Token that is on top of the stack.
					//Debug.Log("Top " + System.Enum.GetName(typeof(SymbolicToken), top.scannedToken.token));
					//Debug.Log("nT index " + nonTerminals.FindIndex(x => x.Equals(top.scannedToken)));
					//Debug.Log(top.scannedToken);
					int tokenColumn = TERMINALS.Count + nonTerminals.FindIndex(x => x.Equals(top.scannedToken.token));
					//Debug.Log(tokenColumn);
					//Debug.Log(ParseTable[previousState.parserStateIndex][tokenColumn].value);

					//Push onto the stack the state we are supposed to GoTo.
					parsingStack.Add(new ParseStackElement(ParseStackElement.Type.State, ParseTable[previousState.parserStateIndex][tokenColumn].value) );

				}
				else {
					//Otherwise we look at the current state and the currency indiciator

					//Get the input token we are looking at
					ScannedToken currentInputToken = tokens[currencyIndicator];

					//It can (should) only be a non terminal, so calculate its column on the parse table.
					int tokenColumn = TERMINALS.FindIndex(x => x.Equals(currentInputToken.token));
					//Debug.Log(currentInputToken.token);
					//Debug.Log(tokenColumn+"/"+(nonTerminals.Count+TERMINALS.Count));
					ParserInstruction targetInstruction = ParseTable[top.parserStateIndex][tokenColumn];

					if (targetInstruction.instruction == ParserInstruction.Instruction.ACCEPT) {
						Debug.Log(parsingStack[parsingStack.Count - 2].scannedToken.GetData() ?? "Execution completed with no debug output.");
						break;
					}
					else if (targetInstruction.instruction == ParserInstruction.Instruction.SHIFT) {
						//Debug.Log("Shift");
						//If it is a shift, we put this token on the stack,
						parsingStack.Add(new ParseStackElement(ParseStackElement.Type.Token, 0, currentInputToken));

						//advance the currency token,
						currencyIndicator++;

						//then the new state on the stack.
						parsingStack.Add(new ParseStackElement(ParseStackElement.Type.State, targetInstruction.value));

					}
					else if (targetInstruction.instruction == ParserInstruction.Instruction.REDUCE) {
						//Debug.Log("Reduce");
						//If it is reduce, we... reduce
						int removalCount = productionRules[targetInstruction.value].tokens.Length * 2;
						//We pop off twice as many tokens as the rhs of the reduce instruction.
						List<ParseStackElement> poppedList = parsingStack.GetRange(parsingStack.Count - removalCount, removalCount);
						parsingStack.RemoveRange(parsingStack.Count - removalCount, removalCount);

						//Create a list of all of the ScannedTokens
						List<ScannedToken> scannedTokensList = new List<ScannedToken>();
						for (int i = 0; i < poppedList.Count; i++) {
							if (poppedList[i].type == ParseStackElement.Type.Token)
								scannedTokensList.Add(poppedList[i].scannedToken);
						}

						//Call the appropriate operation (Performi the Reduction)
						ScannedToken resultingToken = new ScannedToken(productionRules[targetInstruction.value].nonTerminal, scannedTokensList, ScannedToken.Type.NonTerminal, targetInstruction.value);
						//Debug.Log(resultingToken);
						//Put the resulting LHS token onto the stack.
						parsingStack.Add(new ParseStackElement(ParseStackElement.Type.Token, 0, resultingToken));
					}
					else {
						PrintList(parsingStack);
						Debug.Log(currentInputToken);
						tokens.RemoveRange(0, currencyIndicator);
						PrintList(tokens);
						Debug.LogError("Stack parsing has broken due to invalid Instruction: " + System.Enum.GetName(typeof(ParserInstruction.Instruction), targetInstruction.instruction));
						yield break;
					}

				}

			}

		}

		/// <summary>
		/// Prints a formatted list of items using its ToString method
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="list"></param>
		public static string PrintList<T>(List<T> list, bool printList = true) {
			string ret = "";
			for (int i = 0; i < list.Count; i++) {
				ret += list[i].ToString() + " | ";
			}

			if (printList) Debug.Log(ret);
			return ret;
		}

		/// <summary>
		/// Builds a ParseTable for the Grammar
		/// </summary>
		/// <returns></returns>
	    public static IEnumerator<object> BuildParseTable(BGSLangauge language) {


				int stateIter = 0;
				ParseTable = new List<ParserInstruction[]>();

				List<ParserState> stateList = new List<ParserState>();
				stateList.Add(new ParserState(new ParserState.StateRule[] { new ParserState.StateRule(0, productionRules[0])}));
				ParserState.count++; //Count the init state.

				while (stateIter < stateList.Count) {
					//TODO, start a row for this parser state.
					//Create states for our leads.
					//Debug.Log(stateList[stateIter].ToString());
					//Debug.Log(stateList[stateIter].TransitionString());

					ParserInstruction[] parseTableRow = GenerateEmptyRow(TERMINALS.Count + nonTerminals.Count);

					foreach (SymbolicToken key in stateList[stateIter].transitionSet.Keys) {
						//if (key == SymbolicToken.Type) PrintList(stateList[stateIter].transitionSet[key]);
						//Do not do anything if this leads nowhere.
						if (stateList[stateIter].transitionSet[key].Count == 0)
							continue;

						//Check if this state matches an already exiting one.
						ParserState newState = new ParserState(stateList[stateIter].transitionSet[key].ToArray());
						if (stateList.Contains(newState) == false) {
							//Add it
							newState.ID = ParserState.count++;
							stateList.Add(newState);
							//TODO build row
						}
						else {
							//TODO, Make this (toExapnd) row link back to the old matching set.
							newState = stateList.Find(x => x.Equals(newState));
							//Debug.LogWarning("Discarding redundant generated state.");
						}

						int column = (TERMINALS.Contains(key) == true) ? TERMINALS.IndexOf(key) : TERMINALS.Count + nonTerminals.IndexOf(key);
						ParserInstruction.Instruction instruction = (nonTerminals.Contains(key)) ? ParserInstruction.Instruction.GOTO : ParserInstruction.Instruction.SHIFT;
						parseTableRow[column] = new ParserInstruction(newState.ID, instruction);

					}

					//For each rule that has ended, add a reduce or goto
					for (int i = 0; i < stateList[stateIter].rules.Count; i++) {
						ParserState.StateRule sRule = stateList[stateIter].rules[i];
						if (sRule.dotPosition >= sRule.rule.tokens.Length) {
							if (sRule.rule.Equals(productionRules[0])) {
								//Add Accept token at EOF
								parseTableRow[TERMINALS.Count - 1] = new ParserInstruction(0, ParserInstruction.Instruction.ACCEPT);
								continue;
							}
							for (int k = 0; k < TERMINALS.Count; k++) {
								ParserInstruction newRule = new ParserInstruction(GetRuleIndex(sRule.rule), ParserInstruction.Instruction.REDUCE);
									//Debug.LogError("Parsing Conflict. ("+ parseTableRow[k] + ")->("+ newRule + ")");
								if (parseTableRow[k].instruction == ParserInstruction.Instruction.ERR)
									parseTableRow[k] = newRule;
							}
						}
					}


					ParseTable.Add(parseTableRow);
					yield return null;
					//Debug.Log(stateIter);
					stateIter++;
				}

				//Save the Parse table for later
				WriteTableOut(ParseTable);
		}

		/// <summary>
		/// Writes the ParsingTable out to a file
		/// </summary>
		/// <param name="table"></param>
		private static void WriteTableOut(List<ParserInstruction[]> table) {
			string ret = ",";
			for (int i = 0; i < TERMINALS.Count; i++) {
				ret += "" + System.Enum.GetName(typeof(SymbolicToken), TERMINALS[i]) + ",";
			}

			for (int i = 0; i < nonTerminals.Count; i++) {
				ret += "" + System.Enum.GetName(typeof(SymbolicToken), nonTerminals[i]) + ",";
			}

			ret += "\n";

			for (int i = 0; i < table.Count; i++) {
				ret += "S" + i + ":,";
				for (int k = 0; k < table[i].Length; k++) {
					ret += table[i][k].ToString() + ",";
				}
				ret += "\n";
			}
			System.IO.File.WriteAllText(Application.persistentDataPath + "/BuiltTable.csv", ret);
		}

		/// <summary>
		/// Generates a row of empty instructions (ERR default)
		/// </summary>
		/// <param name="count"></param>
		/// <returns></returns>
		private static ParserInstruction[] GenerateEmptyRow(int count) {
			ParserInstruction[] instructions = new ParserInstruction[count];
			for (int i = 0; i < instructions.Length; i++) {
				instructions[i] = new ParserInstruction(0, ParserInstruction.Instruction.ERR);
			}
			return instructions;
		}
		#endregion

		#region Structs
		/// <summary>
		/// A struct used for tracking both Tokens and States in the parsing process
		/// </summary>
		public struct ParseStackElement {
			public enum Type { Token, State }

			public Type type;
			public int parserStateIndex;
			public ScannedToken scannedToken;


			public ParseStackElement(Type type, int parserState, ScannedToken token = default(ScannedToken)) {
				this.type = type;
				this.parserStateIndex = parserState;
				this.scannedToken = token;
			}

			public override string ToString() {
				return (type == Type.State) ? "State" + parserStateIndex : scannedToken.ToString();
			}

		}

		/// <summary>
		/// A self constructing parsing state based on some initial rules
		/// </summary>
		private class ParserState {
			/// <summary>
			/// Internat count of the number of states.
			/// </summary>
			public static int count = 0;

			/// <summary>
			/// The ID of this state.
			/// </summary>
			public int ID;

			public Dictionary<Type, List<StateRule>> transitionSet = new Dictionary<Type, List<StateRule>>();
			public List<StateRule> rules = new List<StateRule>();

			public ParserState(StateRule[] initialRules) {

				//For each InitialRules
				//Add them to the rules list
				//Based on DOT position, if this rule has a NON terminal to the right of DOT, add those rules to this state.
				//Only if they have not been added before. (Research shows that it uses obj.Equals for this, so since those are overridden in to allow for State and Grammar rules to be equal.
				Queue<StateRule> toExpand = new Queue<StateRule>(initialRules);
				//Debug.Log("Creating new state with " + toExpand.Count + " starting rules.");
				//Debug.Log(initialRules[0]);
				while (toExpand.Count > 0) {
					StateRule targetRule = toExpand.Dequeue();

					rules.Add(targetRule);
					//So on a transition we consume a token, so when adding to our transition set make sure to advance the dot one space.
					if (targetRule.dotPosition >= targetRule.rule.tokens.Length) {
						continue;
					}
					else {

					}
					SymbolicToken dotTarget = targetRule.rule.tokens[targetRule.dotPosition];

					if (transitionSet.ContainsKey(dotTarget) == false)
						transitionSet.Add(dotTarget, new List<StateRule>());
					transitionSet[dotTarget].Add(new StateRule(targetRule.dotPosition + 1, targetRule.rule));

					//Check to see if dotPosition is a nonTerminal
					if (nonTerminals.Contains(dotTarget)) {
						//Debug.Log(System.Enum.GetName(typeof(SymbolicToken), dotTarget));

						//If it is, add the necessary production rules.
						List<GrammarRule> tokenRules = GetRules(dotTarget);
						//Debug.Log(tokenRules.Count);
						foreach (GrammarRule r in tokenRules) {
							StateRule nR = new StateRule(0, r);
							if (rules.Contains(nR) == false && toExpand.Contains(nR) == false) {
								toExpand.Enqueue(nR);
							}
						}
					}
				}

			}

			public string TransitionString() {
				string ret = "";
				foreach (SymbolicToken token in transitionSet.Keys) {
					ret += System.Enum.GetName(typeof(SymbolicToken), token) + " --> \n";
					foreach (StateRule r in transitionSet[token]) {
						ret += "\t\t " + r.ToString() + "\n";
					}

					ret += "\n--------------\n";
				}

				return ret;
			}

			public override string ToString() {
				string ret = "{\n";
				for (int i = 0; i < rules.Count; i++) {
					ret += rules[i].ToString() + "\n";
				}
				ret += "}";
				return ret;
			}

			public override int GetHashCode() {
				return ToString().GetHashCode();
			}

			public override bool Equals(object obj) {
				//We need to a comparison where as long as the set is the same we are good, but the order doesnt matter
				//Sort?
				//Dynamic Comparison?
				if (obj.GetType() == typeof(ParserState)) {
					ParserState converted = (ParserState)obj;

					//We do not match if we have a different number of rules.
					if (converted.rules.Count != this.rules.Count)
						return false;

					//For each of our rules (x), there is a matching rule in the other objects. (Containment checks Equality)
					return this.rules.TrueForAll(x => converted.rules.Contains(x));
				}
				else {
					return false;
				}

			}

			/// <summary>
			/// A rule that is in the process of having a match found.
			/// </summary>
			public struct StateRule {
				/// <summary>
				/// The position in this state rule representing our progress towards finding a match for this rule.
				/// </summary>
				public int dotPosition;
				/// <summary>
				/// The rule we are attempting to make a match for.
				/// </summary>
				public GrammarElement rule;

				public StateRule(int dotPosition, GrammarRule rule) {
					this.dotPosition = dotPosition;
					this.rule = rule;
				}

				public override int GetHashCode() {
					return ToString().GetHashCode();
				}

				public override bool Equals(object obj) {
					if (obj.GetType() == typeof(StateRule)) {
						StateRule converted = (StateRule)obj;
						return converted.dotPosition == this.dotPosition && converted.rule.Equals(this.rule);
					}
					/*else if (obj.GetType() == typeof(GrammarRule)) {
						GrammarRule converted = (GrammarRule)obj;
						return converted.Equals(this.rule);
					}*/
					else {
						return false;
					}
				}

				public override string ToString() {
					string ret = "";
					ret += rule.nonTerminal.GetType().name;
					ret += " -> ";
					for (int i = 0; i < rule.tokens.Length; i++) {
						if (i == dotPosition)
							ret += "•";
						ret += rule.tokens[i].nonTerminal.GetType().name;
						ret += " ";
					}
					if (rule.tokens.Length == dotPosition)
						ret += "•";
					return ret;
				}
			}
		}

		/// <summary>
		/// An instruction for the Parse table to store.
		/// </summary>
		public struct ParserInstruction {
			public enum Instruction { SHIFT, REDUCE, GOTO, ERR, ACCEPT }
			public Instruction instruction;
			public int value;
			public ParserInstruction(int value, Instruction instruction) {
				this.value = value;
				this.instruction = instruction;
			}

			public override string ToString() {
				switch (instruction) {
					case Instruction.SHIFT:
						return "s" + value;
					case Instruction.REDUCE:
						return "r" + value;
					case Instruction.GOTO:
						return "" + value;
					case Instruction.ERR:
						return "";
					case Instruction.ACCEPT:
						return "!";
					default:
						return "";
				}
			}
		}
		#endregion
	}
}
