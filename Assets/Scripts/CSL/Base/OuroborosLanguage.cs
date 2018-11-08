using UnityEngine;
using System.IO;
using System.Collections.Generic;
using ProductionRule = OuroborosScripting.OuroborosInterpreter.ProductionRule;
using SymbolStringTuple = OuroborosScripting.OuroborosInterpreter.SymbolStringTuple;
using ParserInstruction = OuroborosScripting.OuroborosInterpreter.ParserInstruction;
using CoroutineWrapper = OuroborosScripting.OuroborosInterpreter.CoroutineWrapper;

namespace OuroborosScripting.GeneratedLanguages {

	public class OuroborosLanguage : IOuroborosLanguage {


		public ParserInstruction[][] GetParseTable() { return parseTable; }	public ProductionRule[] GetProductionRules() { return productionRules; } public SymbolStringTuple[] GetRegexPairs() { return regexPairs; }	public List<object> GetNonTerminals() {	return nonTerminals; } public List<object> GetTERMINALS() { return TERMINALS; }
		IEnumerator<object> IOuroborosLanguage.StartLangCoroutine(string name, CoroutineWrapper coroutineWrapper) { yield return GameManager._instance.StartCoroutine(name, coroutineWrapper);}


		public enum SymbolicToken {
			_WS, _COMMENT, _NEWLINE, NAMESTRING, EXTSTRING, GLOBALSTRING, USINGSTRING, PRODUCTIONSTRING, HEADERTAG, COLON, DISCARDIDEN, IDENTIFIER, REGEX, CODE,
			LanguageDefinition, IndetifierList, ProductionHeader, TokenStatementList, RegexRule, ProductionRule, ProductionRuleRHSList, RegexRuleRHSList, RegesRuleRHS, ProductionRuleRHS, Corotuine, GlobalHeader, UsingHeader, NameHeader, ExtHeader
		}

		public List<object> TERMINALS = new List<object> { SymbolicToken._WS, SymbolicToken._COMMENT, SymbolicToken._NEWLINE, SymbolicToken.NAMESTRING, SymbolicToken.EXTSTRING, SymbolicToken.GLOBALSTRING, SymbolicToken.USINGSTRING, SymbolicToken.PRODUCTIONSTRING, SymbolicToken.HEADERTAG, SymbolicToken.COLON, SymbolicToken.DISCARDIDEN, SymbolicToken.IDENTIFIER, SymbolicToken.REGEX, SymbolicToken.CODE };
		public List<object> nonTerminals = new List<object> { SymbolicToken.LanguageDefinition, SymbolicToken.IndetifierList, SymbolicToken.ProductionHeader, SymbolicToken.TokenStatementList, SymbolicToken.RegexRule, SymbolicToken.ProductionRule, SymbolicToken.ProductionRuleRHSList, SymbolicToken.RegexRuleRHSList, SymbolicToken.RegesRuleRHS, SymbolicToken.ProductionRuleRHS, SymbolicToken.Corotuine, SymbolicToken.GlobalHeader, SymbolicToken.UsingHeader, SymbolicToken.NameHeader, SymbolicToken.ExtHeader };

		public SymbolStringTuple[] regexPairs = new SymbolStringTuple[] {
			new SymbolStringTuple(0, @"(\s)", "Execution0"),
		};
		public ProductionRule[] productionRules = new ProductionRule[] { };
		public ParserInstruction[][] parseTable = new ParserInstruction[][] { };

		#region Messy
		static string fullIOPath = Application.dataPath + "/Scripts/";
		static string tokenName = "SymbolicToken";
		static string startCoroutineTarget = "GameManager._instance";
		static string interfaceCooperationSnippet = "public ParserInstruction[][] GetParseTable() { return parseTable; }	public ProductionRule[] GetProductionRules() { return productionRules; } public SymbolStringTuple[] GetRegexPairs() { return regexPairs; }	public List<object> GetNonTerminals() {	return nonTerminals; } public List<object> GetTERMINALS() { return TERMINALS; }";
		static string coroutineInputProcessingSnippet = "List<object> rhs = null;\n yield return" + startCoroutineTarget + "StartCoroutine(target.Prepare(enviroment, (object data) => { rhs = (List<object>)data; }))";
		List<string> terminalTokens = new List<string>();
		List<string> nonTerminalTokens = new List<string>();
		List<string> regexList = new List<string>();
		List<string> productionList = new List<string>();
		List<string> preparedProductionList = new List<string>();
		List<ParserInstruction[]> ParseTable;
		List<string> coroutines = new List<string>();

		int coroutineCount = 0;
		string GetUniqueCoroutineName() {
			return "Execution" + (coroutineCount++);
		}

		/*
		/// <summary>
		/// Builds a ParseTable for the Grammar
		/// </summary>
		/// <returns></returns>
		public static IEnumerator<object> BuildParseTable() {

			TERMINALS = language.GetOrderedTerminals();
			nonTerminals = language.GetOrderedNonTerminals();

			int stateIter = 0;
			ParseTable = new List<ParserInstruction[]>();

			List<ParserState> stateList = new List<ParserState>();
			stateList.Add(new ParserState(new ParserState.StateRule[] { new ParserState.StateRule(0, GrammarElement.GetRulesFromType(typeof(ScriptPrime))[0]) }));
			ParserState.count++; //Count the init state.

			while (stateIter < stateList.Count) {
				//TODO, start a row for this parser state.
				//Create states for our leads.
				//Debug.Log(stateList[stateIter].ToString());
				//Debug.Log(stateList[stateIter].TransitionString());

				ParserInstruction[] parseTableRow = GenerateEmptyRow(TERMINALS.Count + nonTerminals.Count);

				foreach (System.Type key in stateList[stateIter].transitionSet.Keys) {
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
					if (sRule.dotPosition >= sRule.rule.GetRHSElements().Count) {
						if (sRule.rule.GetType() == typeof(ScriptPrime)) {
							//Add Accept token at EOF
							parseTableRow[TERMINALS.Count - 1] = new ParserInstruction(0, ParserInstruction.Instruction.ACCEPT);
							continue;
						}
						for (int k = 0; k < TERMINALS.Count; k++) {
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
			WriteTableOut(ParseTable);
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
			/// <summary>
			/// Internat count of the number of states.
			/// </summary>
			public static int count = 0;

			/// <summary>
			/// The ID of this state.
			/// </summary>
			public int ID;

			public Dictionary<System.Type, List<StateRule>> transitionSet = new Dictionary<System.Type, List<StateRule>>();
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
					if (targetRule.dotPosition >= targetRule.rule.GetRHSElements().Count) {
						continue;
					}

					System.Type dotTarget = targetRule.rule.GetRHSElements()[targetRule.dotPosition];

					if (transitionSet.ContainsKey(dotTarget) == false)
						transitionSet.Add(dotTarget, new List<StateRule>());
					transitionSet[dotTarget].Add(new StateRule(targetRule.dotPosition + 1, targetRule.rule));

					//Check to see if dotPosition is a nonTerminal
					if (nonTerminals.Contains(dotTarget)) {
						//Debug.Log(System.Enum.GetName(typeof(SymbolicToken), dotTarget));

						//If it is, add the necessary production rules.
						List<GrammarElement.ProductionRule> tokenRules = GrammarElement.GetRulesFromType(dotTarget);
						//Debug.Log(tokenRules.Count);
						foreach (GrammarElement.ProductionRule r in tokenRules) {
							StateRule nR = new StateRule(0, r);
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
				public GrammarElement.ProductionRule rule;

				public StateRule(int dotPosition, GrammarElement.ProductionRule rule) {
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

				public override string ToString() {
					string ret = "";
					ret += rule.GetNonTerminal().Name;
					ret += " -> ";
					for (int i = 0; i < rule.GetRHSElements().Count; i++) {
						if (i == dotPosition)
							ret += "•";
						ret += rule.GetRHSElements()[i].GetType().Name;
						ret += " ";
					}
					if (rule.GetRHSElements().Count == dotPosition)
						ret += "•";
					return ret;
				}
			}
		}
		*/
		
		
		//%_WS 					:	"(\s)" 																		
		public IEnumerator<object> Execution0(CoroutineWrapper coroutineWrapper) { List<object> rhs = null; yield return GameManager._instance.StartCoroutine(coroutineWrapper.target.Prepare(coroutineWrapper.enviroment, (object data) => { rhs = (List<object>)data; })); coroutineWrapper.callback(rhs[0]); yield return null; }
		//%_COMMENT				:	"(//.\n)"																	
		public IEnumerator<object> Execution1(CoroutineWrapper coroutineWrapper) { List<object> rhs = null; yield return GameManager._instance.StartCoroutine(coroutineWrapper.target.Prepare(coroutineWrapper.enviroment, (object data) => { rhs = (List<object>)data; })); coroutineWrapper.callback(rhs[0]); yield return null; }
		//%_NEWLINE				:	"(\n)"																		
		public IEnumerator<object> Execution2(CoroutineWrapper coroutineWrapper) { List<object> rhs = null; yield return GameManager._instance.StartCoroutine(coroutineWrapper.target.Prepare(coroutineWrapper.enviroment, (object data) => { rhs = (List<object>)data; })); coroutineWrapper.callback(rhs[0]); yield return null; }
		//NAMESTRING				:	"(Name)"																	
		public IEnumerator<object> Execution3(CoroutineWrapper coroutineWrapper) { List<object> rhs = null; yield return GameManager._instance.StartCoroutine(coroutineWrapper.target.Prepare(coroutineWrapper.enviroment, (object data) => { rhs = (List<object>)data; })); coroutineWrapper.callback(rhs[0]); yield return null; }
		//EXTSTRING				:	"(Ext)"																		
		public IEnumerator<object> Execution4(CoroutineWrapper coroutineWrapper) { List<object> rhs = null; yield return GameManager._instance.StartCoroutine(coroutineWrapper.target.Prepare(coroutineWrapper.enviroment, (object data) => { rhs = (List<object>)data; })); coroutineWrapper.callback(rhs[0]); yield return null; }
		//GLOBALSTRING			:	"(Global)"																	
		public IEnumerator<object> Execution5(CoroutineWrapper coroutineWrapper) { List<object> rhs = null; yield return GameManager._instance.StartCoroutine(coroutineWrapper.target.Prepare(coroutineWrapper.enviroment, (object data) => { rhs = (List<object>)data; })); coroutineWrapper.callback(rhs[0]); yield return null; }
		//USINGSTRING				:	"(Using)"																	
		public IEnumerator<object> Execution6(CoroutineWrapper coroutineWrapper) { List<object> rhs = null; yield return GameManager._instance.StartCoroutine(coroutineWrapper.target.Prepare(coroutineWrapper.enviroment, (object data) => { rhs = (List<object>)data; })); coroutineWrapper.callback(rhs[0]); yield return null; }
		//PRODUCTIONSTRING		:	"(Prod)"																	
		public IEnumerator<object> Execution7(CoroutineWrapper coroutineWrapper) { List<object> rhs = null; yield return GameManager._instance.StartCoroutine(coroutineWrapper.target.Prepare(coroutineWrapper.enviroment, (object data) => { rhs = (List<object>)data; })); coroutineWrapper.callback(rhs[0]); yield return null; }
		//HEADERTAG				:	"(%%)"																		
		public IEnumerator<object> Execution8(CoroutineWrapper coroutineWrapper) { List<object> rhs = null; yield return GameManager._instance.StartCoroutine(coroutineWrapper.target.Prepare(coroutineWrapper.enviroment, (object data) => { rhs = (List<object>)data; })); coroutineWrapper.callback(rhs[0]); yield return null; }
		//COLON					:	"(:)"																		
		public IEnumerator<object> Execution9(CoroutineWrapper coroutineWrapper) { List<object> rhs = null; yield return GameManager._instance.StartCoroutine(coroutineWrapper.target.Prepare(coroutineWrapper.enviroment, (object data) => { rhs = (List<object>)data; })); coroutineWrapper.callback(rhs[0]); yield return null; }
		//DISCARDIDEN				:	"(%_)([a-zA-Z]([a-zA-Z]|[0-9])*)" 											
		public IEnumerator<object> Execution10(CoroutineWrapper coroutineWrapper) { List<object> rhs = null; yield return GameManager._instance.StartCoroutine(coroutineWrapper.target.Prepare(coroutineWrapper.enviroment, (object data) => { rhs = (List<object>)data; })); coroutineWrapper.callback(rhs[0]); yield return null; }     //An identifier preceeded by %_ is an identifier that is read by the scanner but is discarded from use. (will not appear in token stream)
																																				//IDENTIFIER				:	"([a-zA-Z]([a-zA-Z]|[0-9])*)"												
		public IEnumerator<object> Execution11(CoroutineWrapper coroutineWrapper) { List<object> rhs = null; yield return GameManager._instance.StartCoroutine(coroutineWrapper.target.Prepare(coroutineWrapper.enviroment, (object data) => { rhs = (List<object>)data; })); coroutineWrapper.callback(rhs[0]); yield return null; } // A token name
																																			//REGEX					:	"(\"([^\"]*)\")"															
		public IEnumerator<object> Execution12(CoroutineWrapper coroutineWrapper) { List<object> rhs = null; yield return GameManager._instance.StartCoroutine(coroutineWrapper.target.Prepare(coroutineWrapper.enviroment, (object data) => { rhs = (List<object>)data; })); coroutineWrapper.callback(rhs[0]); yield return null; }                 //String used for Regex matching
																																							//CODE					:	"(\{.\})"																	
		public IEnumerator<object> Execution13(CoroutineWrapper coroutineWrapper) { List<object> rhs = null; yield return GameManager._instance.StartCoroutine(coroutineWrapper.target.Prepare(coroutineWrapper.enviroment, (object data) => { rhs = (List<object>)data; })); string code = ((string)rhs[0]).Substring(1, ((string)rhs[0]).Length - 2); /*chopoff the brackets*/ coroutineWrapper.callback(code); yield return null; }                    //A code section to placed in the resulting language definition. //WARNING: CODE NEEDS REGEX FOR FINDING MATCHING PAIRS NOT A DIRECT COMPARISON.


		//LanguageDefinition		:	NameHeader ExtHeader UsingHeader GlobalHeader ProductionHeader
		public IEnumerator<object> Execution14(CoroutineWrapper coroutineWrapper) {
			List<object> rhs = null; yield return GameManager._instance.StartCoroutine(coroutineWrapper.target.Prepare(coroutineWrapper.enviroment, (object data) => { rhs = (List<object>)data; }));
			using (StreamWriter sw = File.CreateText(fullIOPath + rhs[0] +"."+ rhs[1])) {
				//Print Using Header
				sw.Write((string) rhs[2]);
				//Print Namespace/class Declaration
				sw.Write("namespace OuroborosScripting.GeneratedLanguages {\n\npublic class " + rhs[0] + " : IOuroborosLanguage {\n\n");
				//Print the interface code
				sw.Write(interfaceCooperationSnippet);
				//Print Global Header
				sw.Write((string) rhs[3]);
				//Print Production Header
				sw.Write((string) rhs[4]);
				//Close Namespace/Class Declaration
				sw.Write("\n}\n}\n\n");
			}
			yield return null;
		}

		//IdentifierList			:	IDENTIFIER
		public IEnumerator<object> Execution15(CoroutineWrapper coroutineWrapper) { List<object> rhs = null; yield return GameManager._instance.StartCoroutine(coroutineWrapper.target.Prepare(coroutineWrapper.enviroment, (object data) => { rhs = (List<object>)data; })); coroutineWrapper.callback("new SymbolicToken[] { " + tokenName + "." + (string)rhs[0]); yield return null; }
		//							:	IdentifierList IDENTIFIER 
		public IEnumerator<object> Execution16(CoroutineWrapper coroutineWrapper) { List<object> rhs = null; yield return GameManager._instance.StartCoroutine(coroutineWrapper.target.Prepare(coroutineWrapper.enviroment, (object data) => { rhs = (List<object>)data; })); coroutineWrapper.callback((string)rhs[0] + ", " +tokenName + "." + (string)rhs[1]); yield return null;}

		//ProductionHeader 		:	HEADERTAG PRODUCTIONSTRING TokenStatementList HEADERTAG 
		public IEnumerator<object> Execution17(CoroutineWrapper coroutineWrapper) {
			List<object> rhs = null; yield return GameManager._instance.StartCoroutine(coroutineWrapper.target.Prepare(coroutineWrapper.enviroment, (object data) => { rhs = (List<object>)data; }));
			//Create the tokens ENUMs
			string enums = "public enum Symbolictoken {\n";
			foreach (string s in terminalTokens) enums += s+",";
			enums += "\n";
			foreach (string s in nonTerminalTokens) enums += s+",";
			enums = enums.Substring(0,enums.Length - 2); // Remove the extra comma
			enums += "\n}\n";
																											
			//Create the list of terminals
			string terminalDcl = "public SymbolicToken[] TERMINALS = new SymbolicToken[] {";
			foreach (string s in terminalTokens) terminalDcl += tokenName +"."+s+",";
			terminalDcl = terminalDcl.Substring(0,terminalDcl.Length - 2); // Remove the extra comma
			terminalDcl += "}\n";
																											
			//Create the list of non terminals
			string nonTerminalDcl = "public SymbolicToken[] nonTerminals = new SymbolicToken[] {";
			foreach (string s in nonTerminalTokens) nonTerminalDcl += tokenName +"."+s+",";
			nonTerminalDcl = nonTerminalDcl.Substring(0,nonTerminalDcl.Length - 2); // Remove the extra comma
			nonTerminalDcl += "}\n";



			string regexPairsDCL = "public SymbolStringTuple[] regexPairs = new SymbolStringTuple[] {\n";
			foreach (string s in regexList) regexPairsDCL += s+",\n";
			regexPairsDCL = regexPairsDCL.Substring(0, regexPairsDCL.Length - 3); // Remove the extra comma and new line
			regexPairsDCL += "\n};\n";

			string productionRulesDCL = "public ProductionRule[] productionRules = new ProductionRule[] {\n";
			foreach (string s in productionList) productionRulesDCL += s + ",\n";
			productionRulesDCL = productionRulesDCL.Substring(0, productionRulesDCL.Length - 3); // Remove the extra comma and new line
			productionRulesDCL += "\n};\n";

			string parseTableDCL = "public ParserInstruction[][] parseTable = new ParserInstruction[][] {\n";
			parseTableDCL += "\n};\n";

			string coroutinesDcl = "";
			foreach (string s in coroutines) coroutinesDcl += s+"\n";

			coroutineWrapper.callback(enums + "\n" + terminalDcl + "\n" + nonTerminalDcl + "\n"+ regexPairsDCL +"\n" + productionRulesDCL + "\n" + coroutinesDcl + "\n" + parseTableDCL); yield return null;
		}
		//TokenStatementList		:	RegexRule																	
		public IEnumerator<object> Execution18(CoroutineWrapper coroutineWrapper) { yield return null;}
		//						:	ProductionRule																
		public IEnumerator<object> Execution19(CoroutineWrapper coroutineWrapper) { yield return null;}
		//						:	TokenStatementList RegexRule 
		public IEnumerator<object> Execution20(CoroutineWrapper coroutineWrapper) { yield return null;}
		//						:	TokenStatementList ProductionRule 
		public IEnumerator<object> Execution21(CoroutineWrapper coroutineWrapper) { yield return null;}
		//RegexRule				:	IDENTIFIER RegexRuleRHSList 
		public IEnumerator<object> Execution22(CoroutineWrapper coroutineWrapper) { List<object> rhs = null; yield return GameManager._instance.StartCoroutine(coroutineWrapper.target.Prepare(coroutineWrapper.enviroment, (object data) => { rhs = (List<object>)data; })); int c = terminalTokens.Count; terminalTokens.Add((string)rhs [0]); foreach(string v in (List<string>)rhs [1]) { regexList.Add("new SymbolStringTuple("+c+v); } yield return null;}
		//ProductionRule			:	IDENTIFIER ProductionRuleRHSList 
		public IEnumerator<object> Execution23(CoroutineWrapper coroutineWrapper) { List<object> rhs = null; yield return GameManager._instance.StartCoroutine(coroutineWrapper.target.Prepare(coroutineWrapper.enviroment, (object data) => { rhs = (List<object>)data; })); int c = nonTerminalTokens.Count; nonTerminalTokens.Add((string)rhs [0]); foreach(string v in (List<string>)rhs [1]) { productionList.Add("new ProductionRule("+c+v); } yield return null;}

		//ProductionRuleRHSList	:	ProductionRuleRHS															
		public IEnumerator<object> Execution24(CoroutineWrapper coroutineWrapper) { List<object> rhs = null; yield return GameManager._instance.StartCoroutine(coroutineWrapper.target.Prepare(coroutineWrapper.enviroment, (object data) => { rhs = (List<object>)data; })); List<string> u = new List<string>(); u.Add((string) rhs[0]); coroutineWrapper.callback(u); yield return null;}
		//						:	ProductionRuleRHSList ProductionRuleRHS 
		public IEnumerator<object> Execution25(CoroutineWrapper coroutineWrapper) { List<object> rhs = null; yield return GameManager._instance.StartCoroutine(coroutineWrapper.target.Prepare(coroutineWrapper.enviroment, (object data) => { rhs = (List<object>)data; })); ((List<string>)rhs [0]).Add((string)rhs [1]); coroutineWrapper.callback(rhs [0]); yield return null;}

		//RegexRuleRHSList		:	RegexRuleRHS																
		public IEnumerator<object> Execution26(CoroutineWrapper coroutineWrapper) { List<object> rhs = null; yield return GameManager._instance.StartCoroutine(coroutineWrapper.target.Prepare(coroutineWrapper.enviroment, (object data) => { rhs = (List<object>)data; })); List<string> u = new List<string>(); u.Add((string) rhs[0]); coroutineWrapper.callback(u); yield return null;}
		//						:	RegexRuleRHSList RegexRuleRHS 
		public IEnumerator<object> Execution27(CoroutineWrapper coroutineWrapper) { List<object> rhs = null; yield return GameManager._instance.StartCoroutine(coroutineWrapper.target.Prepare(coroutineWrapper.enviroment, (object data) => { rhs = (List<object>)data; })); ((List<string>)rhs [0]).Add((string)rhs [1]); coroutineWrapper.callback(rhs [0]); yield return null;}

		//RegexRuleRHS 			:	COLON REGEX Coroutine														
		public IEnumerator<object> Execution28(CoroutineWrapper coroutineWrapper) { List<object> rhs = null; yield return GameManager._instance.StartCoroutine(coroutineWrapper.target.Prepare(coroutineWrapper.enviroment, (object data) => { rhs = (List<object>)data; })); coroutineWrapper.callback(", "+rhs[1]+", "+rhs[2]+")"); yield return null;}
		//ProductionRuleRHS		:	COLON IdentifierList Coroutine												
		public IEnumerator<object> Execution29(CoroutineWrapper coroutineWrapper) { List<object> rhs = null; yield return GameManager._instance.StartCoroutine(coroutineWrapper.target.Prepare(coroutineWrapper.enviroment, (object data) => { rhs = (List<object>)data; })); coroutineWrapper.callback(", "+rhs[1]+" }, "+rhs[2]+")"); yield return null;}
		//Coroutine				:	CODE																		
		public IEnumerator<object> Execution30(CoroutineWrapper coroutineWrapper) {

			List<object> rhs = null; yield return GameManager._instance.StartCoroutine(coroutineWrapper.target.Prepare(coroutineWrapper.enviroment, (object data) => { rhs = (List<object>)data; }));
			/*Prepare a coroutine stub*/
			string name = GetUniqueCoroutineName();
			string coroutineCode = "";
			coroutineCode += "public IEnumerator<object> " + name + "(CoroutineWrapper coroutineWrapper) {\n";
			coroutineCode += coroutineInputProcessingSnippet;
			coroutineCode += rhs[0];
			coroutineCode += "\n}\n";
			coroutines.Add(coroutineCode);
			coroutineWrapper.callback(name); yield return null;
		}
		//GlobalHeader 			:	HEADERTAG GLOBALSTRING CODE													
		public IEnumerator<object> Execution31(CoroutineWrapper coroutineWrapper) { List<object> rhs = null; yield return GameManager._instance.StartCoroutine(coroutineWrapper.target.Prepare(coroutineWrapper.enviroment, (object data) => { rhs = (List<object>)data; })); coroutineWrapper.callback(rhs[2]); yield return null;}
		//UsingHeader				: 	HEADERTAG USINGSTRING CODE 													
		public IEnumerator<object> Execution32(CoroutineWrapper coroutineWrapper) { List<object> rhs = null; yield return GameManager._instance.StartCoroutine(coroutineWrapper.target.Prepare(coroutineWrapper.enviroment, (object data) => { rhs = (List<object>)data; })); coroutineWrapper.callback(rhs[2]); yield return null;}
		//NameHeader				: 	HEADERTAG NAMESTRING CODE 													
		public IEnumerator<object> Execution33(CoroutineWrapper coroutineWrapper) { List<object> rhs = null; yield return GameManager._instance.StartCoroutine(coroutineWrapper.target.Prepare(coroutineWrapper.enviroment, (object data) => { rhs = (List<object>)data; })); coroutineWrapper.callback(rhs[2]); yield return null;}
		//ExtHeader				: 	HEADERTAG EXTSTRING CODE 													
		public IEnumerator<object> Execution34(CoroutineWrapper coroutineWrapper) { List<object> rhs = null; yield return GameManager._instance.StartCoroutine(coroutineWrapper.target.Prepare(coroutineWrapper.enviroment, (object data) => { rhs = (List<object>)data; })); coroutineWrapper.callback(rhs[2]); yield return null;} 

		#endregion














	}
}