using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using ProcessedToken = OrbScripting.OrbDefinitions.ProcessedToken;
using SymbolStringTuple = OrbScripting.OrbDefinitions.SymbolStringTuple;
using Callback = OrbScripting.OrbDefinitions.Callback;
using CoroutineWrapper = OrbScripting.OrbDefinitions.CoroutineWrapper;
using ProductionRule = OrbScripting.OrbDefinitions.ProductionRule;
using CoroutineMethod = OrbScripting.OrbDefinitions.CoroutineMethod;
using ParserInstruction = OrbScripting.OrbDefinitions.ParserInstruction;
using ParseStackElement = OrbScripting.OrbDefinitions.ParseStackElement;


namespace OrbScripting {
	/// <summary>
	/// Builds the necessary data for the interpration process based on the language specification.
	/// </summary>
	public class OrbParser {
		/*static string fullIOPath = Application.persistentDataPath + "/";
		static string tokenName = "SymbolicToken";
		static string startCoroutineTarget = "GameManager._instance";
		static string interfaceCooperationSnippet = "public ParserInstruction[][] GetParseTable() { return parseTable; }	public ProductionRule[] GetProductionRules() { return productionRules; } public SymbolStringTuple[] GetRegexPairs() { return regexPairs; }	public List<object> GetNonTerminals() {	return nonTerminals; } public List<object> GetTERMINALS() { return TERMINALS; }";
		static string coroutineInputProcessingSnippet = "List<object> rhs = null;\n yield return" + startCoroutineTarget + "StartCoroutine(target.Prepare(enviroment, (object data) => { rhs = (List<object>)data; }))";
		List<string> terminalTokens = new List<string>();
		List<string> nonTerminalTokens = new List<string>();
		List<string> regexList = new List<string>();
		List<StringProductionRule> productionList = new List<StringProductionRule>();
		string ParseTableString;
		List<string> coroutines = new List<string>();

		int coroutineCount = 0;
		string GetUniqueCoroutineName() {
			return "Execution" + (coroutineCount++);
		}


		public class StringProductionRule {
			public int nonTerminal = 0;
			public string nonTerminalString = "";
			public List<string> rhs = new List<string>();
			public string coroutineName = "";
			public StringProductionRule(int nonTerminal, string nonTerminalString, List<string> rhs, string coroutineName) {
				this.nonTerminalString = nonTerminalString;
				this.nonTerminal = nonTerminal;
				this.rhs = rhs;
				this.coroutineName = coroutineName;
			}

			public override string ToString() {
				//ProductionRule(int result, int rhsSize, string coroutineName) 
				return "new ProductionRule(" + nonTerminal + "," + rhs.Count + "," + coroutineName + ")";
			}
		}

		
		/// <summary>
		/// Builds a ParseTable for the Grammar
		/// </summary>
		/// <returns></returns>
		public IEnumerator<object> BuildParseTable() {
			int stateIter = 0;
			List<ParserInstruction[]> ParseTable = new List<ParserInstruction[]>();

			List<ParserState> stateList = new List<ParserState>();
			stateList.Add(new ParserState(new ParserState.StateRule[] { new ParserState.StateRule(0, 0) }, this));
			ParserState.count++; //Count the init state.

			while (stateIter < stateList.Count) {
				//TODO, start a row for this parser state.
				//Create states for our leads.
				//Debug.Log(stateList[stateIter].ToString());
				//Debug.Log(stateList[stateIter].TransitionString());

				ParserInstruction[] parseTableRow = GenerateEmptyRow(terminalTokens.Count + nonTerminalTokens.Count);

				foreach (string key in stateList[stateIter].transitionSet.Keys) {
					//if (key == SymbolicToken.Type) PrintList(stateList[stateIter].transitionSet[key]);
					//Do not do anything if this leads nowhere.
					if (stateList[stateIter].transitionSet[key].Count == 0)
						continue;

					//Check if this state matches an already exiting one.
					ParserState newState = new ParserState(stateList[stateIter].transitionSet[key].ToArray(), this);
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

					int column = (terminalTokens.Contains(key) == true) ? terminalTokens.IndexOf(key) : terminalTokens.Count + nonTerminalTokens.IndexOf(key);
					ParserInstruction.Instruction instruction = (nonTerminalTokens.Contains(key)) ? ParserInstruction.Instruction.GOTO : ParserInstruction.Instruction.SHIFT;
					parseTableRow[column] = new ParserInstruction(newState.ID, instruction);

				}

				//For each rule that has ended, add a reduce or goto
				for (int i = 0; i < stateList[stateIter].rules.Count; i++) {
					ParserState.StateRule sRule = stateList[stateIter].rules[i];
					if (sRule.dotPosition >= productionList[sRule.rule].rhs.Count) {
						if (productionList[sRule.rule].nonTerminal == productionList[0].nonTerminal) {
							//Add Accept token at EOF
							parseTableRow[terminalTokens.Count - 1] = new ParserInstruction(0, ParserInstruction.Instruction.ACCEPT);
							continue;
						}
						for (int k = 0; k < terminalTokens.Count; k++) {
							ParserInstruction newRule = new ParserInstruction(sRule.rule, ParserInstruction.Instruction.REDUCE);
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
			//[TODO] WriteTableOut(ParseTable);
			string r = "public ParserInstruction[][] parseTable = new ParserInstruction[][] { ";
			for (int x = 0; x < ParseTable.Count; x++) {
				r += "new ParserInstruction[] {";
				for (int y = 0; y < ParseTable[0].Length; y++) {
					r += ParseTable[x][y].ToGetString();
					if (y != ParseTable[0].Length - 1)
						r += ", ";
				}
				r += "},\n";
			}
			r += "\n};";
			ParseTableString = r;

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


		/// <summary>
		/// A self constructing parsing state based on some initial rules
		/// </summary>
		private class ParserState {

			public IOrbLanguage languageContext;

			/// <summary>
			/// Internat count of the number of states.
			/// </summary>
			public static int count = 0;

			/// <summary>
			/// The ID of this state.
			/// </summary>
			public int ID;

			public Dictionary<string, List<StateRule>> transitionSet = new Dictionary<string, List<StateRule>>();
			public List<StateRule> rules = new List<StateRule>();

			public List<StringProductionRule> GetStringProductionRulesFromNonTerminal(string nonTerminal) {
				List<StringProductionRule> results = new List<StringProductionRule>();
				foreach (StringProductionRule spr in languageContext.productionList)
					if (spr.nonTerminalString == nonTerminal)
						results.Add(spr);

				return results;
			}

			public ParserState(StateRule[] initialRules, IOrbLanguage languageContext) {
				this.languageContext = languageContext;
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
					if (targetRule.dotPosition >= languageContext.GetProductionRules()[targetRule.rule].rhsSize) {
						continue;
					}

					string dotTarget = languageContext.productionList[targetRule.rule].rhs[targetRule.dotPosition];

					if (transitionSet.ContainsKey(dotTarget) == false)
						transitionSet.Add(dotTarget, new List<StateRule>());
					transitionSet[dotTarget].Add(new StateRule(targetRule.dotPosition + 1, targetRule.rule));

					//Check to see if dotPosition is a nonTerminal
					if (languageContext.nonTerminalTokens.Contains(dotTarget)) {
						//Debug.Log(System.Enum.GetName(typeof(SymbolicToken), dotTarget));

						//If it is, add the necessary production rules.
						List<StringProductionRule> tokenRules = GetStringProductionRulesFromNonTerminal(dotTarget);
						//Debug.Log(tokenRules.Count);
						foreach (StringProductionRule r in tokenRules) {
							StateRule nR = new StateRule(0, languageContext.productionList.IndexOf(r));
							if (rules.Contains(nR) == false && toExpand.Contains(nR) == false) {
								toExpand.Enqueue(nR);
							}
						}
					}
				}

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
				public int rule;

				public StateRule(int dotPosition, int rule) {
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
					else {
						return false;
					}
				}
				/*
				public override string ToString() {
					string ret = "";
					ret += productionList[rule].nonTerminal;
					ret += " -> ";
					for (int i = 0; i < rule.rhs.Count; i++) {
						if (i == dotPosition)
							ret += "•";
						ret += rule.rhs[i];
						ret += " ";
					}
					if (rule.rhs.Count == dotPosition)
						ret += "•";
					return ret;
				}/
			}
		}*/
		
	}
}