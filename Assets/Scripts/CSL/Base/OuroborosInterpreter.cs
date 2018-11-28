using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;


namespace OuroborosScripting {
	/// <summary>
	/// A grammer and rules specification for a Card Scripting Language
	/// </summary>
	public static class OuroborosInterpreter {
		public static readonly bool debugParseOutput = false;

		public delegate IEnumerator<object> CoroutineMethod(CoroutineWrapper coroutineWrapper);

		public delegate void Callback(object data);

		//public static Dictionary<string, object> variableDictionary;

		#region Methods
		/// <summary>
		/// Scans a string into a token stream
		/// </summary>
		/// <param name="input"></param>
		/// <returns></returns>
		public static IEnumerator<object> Scan(IOuroborosLanguage enviroment, string input, Callback callback) {
			List<ProcessedToken?> tokens = new List<ProcessedToken?>();

			List<object> TERMINALS = enviroment.GetTERMINALS();
			SymbolStringTuple[] tuples = enviroment.GetRegexPairs();

			string remainder = input;
			string text = "";
			string testingText = "";
			Regex regex;
			while (remainder.Length > 0) {
				Debug.Log("Top of While");
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
							Debug.Log("Is Match: " + regex + " => " + testingText);
							//Create the processedtoken
							token = new ProcessedToken(i, null,  text);
							//Execute the code for reading this token.
							yield return enviroment.StartLangCoroutine(tuples[i].coroutineName, new CoroutineWrapper(enviroment, (ProcessedToken)token, (object d)=> { } ));

						}
					}

					if (token != null) {
						Debug.Log("Breaking.");
						break;
					}
				}

				if (token != null) {
					if (tuples[i].ignorable) {
						//We have ignored this section
					}
					else {
						Debug.Log(text + " | " + token);
						tokens.Add(token);
					}
					Debug.Log("Pre<"+remainder+">");
					remainder = remainder.Substring(text.Length);
					Debug.Log("Post<" + remainder + ">");
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

		/// <summary>
		/// Operates on a Token stream using the ParsingTable
		/// </summary>
		/// <param name="tokens"></param>
		/// <param name="generateScript"></param>
		/// <returns></returns>
		public static IEnumerator<object> Execute<T>(string inputString) where T : IOuroborosLanguage {
			Debug.Log("Execute");
			//Create an instance of the language we are going to work with.
			IOuroborosLanguage language = (IOuroborosLanguage)System.Activator.CreateInstance(typeof(T));
			//Create a place to store out input tokens (nullable so null = EOF when processing)
			List<ProcessedToken?> inputTokens = null;
			//Call scan, to get our list of beginning tokens.
			yield return Scan(language, inputString, (object d) => { inputTokens = (List<ProcessedToken?>)d; });
			Debug.Log("Scanning Finished");

			PrintList<ProcessedToken?>(inputTokens);

			List<object> TERMINALS = language.GetTERMINALS();
			List<object> nonTerminals = language.GetNonTerminals();
			ParserInstruction[][] ParseTable = language.GetParseTable();

			//The parsing stack
			List<ParseStackElement> parsingStack = new List<ParseStackElement>();

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
					int tokenColumn = TERMINALS.Count + top.scannedToken.tokenSymbolLocation;

					//Push onto the stack the state we are supposed to GoTo.
					parsingStack.Add(new ParseStackElement(ParseStackElement.Type.State, (int)ParseTable[previousState.parserStateIndex][tokenColumn].value));


				}
				else {
					//Otherwise we look at the current state and the currency indiciator

					//Get the input token we are looking at
					ProcessedToken currentInputToken = (ProcessedToken)inputTokens[currencyIndicator];

					//It can (should) only be a non terminal, so calculate its column on the parse table.
					int tokenColumn = currentInputToken.tokenSymbolLocation;

					//Debug.Log(currentInputToken.token);
					//Debug.Log(tokenColumn+"/"+(nonTerminals.Count+TERMINALS.Count));
					ParserInstruction targetInstruction = ParseTable[top.parserStateIndex][tokenColumn];

					if (targetInstruction.instruction == ParserInstruction.Instruction.ACCEPT) {

						ProcessedToken finalToken = (ProcessedToken)parsingStack[parsingStack.Count - 2].scannedToken;
						//Execute the final rule.
						language.StartLangCoroutine(finalToken.rule, new CoroutineWrapper(language, finalToken, (object data) => { } ));
						break;
					}
					else if (targetInstruction.instruction == ParserInstruction.Instruction.SHIFT) {
						//Debug.Log("Shift");
						//If it is a shift, we put this token on the stack,
						parsingStack.Add(new ParseStackElement(ParseStackElement.Type.Token, 0, currentInputToken));

						//advance the currency token,
						currencyIndicator++;

						//then the new state on the stack.
						parsingStack.Add(new ParseStackElement(ParseStackElement.Type.State, (int)targetInstruction.value));

					}
					else if (targetInstruction.instruction == ParserInstruction.Instruction.REDUCE) {
						//If it is reduce, we... reduce

						ProductionRule productionRule = language.GetProductionRules()[targetInstruction.value];
						int removalCount = productionRule.rhsSize * 2;
						//We pop off twice as many tokens as the rhs of the reduce instruction.
						List<ParseStackElement> poppedList = parsingStack.GetRange(parsingStack.Count - removalCount, removalCount);
						parsingStack.RemoveRange(parsingStack.Count - removalCount, removalCount);

						//Create a list of all of the ScannedTokens
						List<ProcessedToken> scannedTokensList = new List<ProcessedToken>();
						for (int i = 0; i < poppedList.Count; i++) {
							if (poppedList[i].type == ParseStackElement.Type.Token)
								scannedTokensList.Add(poppedList[i].scannedToken);
						}

						//Call the appropriate operation (Performi the Reduction)
						ProcessedToken resultingToken = new ProcessedToken(productionRule.result, productionRule.coroutineName, scannedTokensList);

						//Put the resulting LHS token onto the stack.
						parsingStack.Add(new ParseStackElement(ParseStackElement.Type.Token, 0, resultingToken));
					}
					else {
						PrintList(parsingStack);
						Debug.Log(currentInputToken);
						inputTokens.RemoveRange(0, currencyIndicator);
						//PrintList(inputTokens);
						Debug.LogError("Stack parsing has broken due to invalid Instruction: " + System.Enum.GetName(typeof(ParserInstruction.Instruction), targetInstruction.instruction));
						yield break;
					}

				}

			}
			Debug.Log("EndExecute");
		}//-------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------END EXECUTE

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

		#endregion

		#region Structs
		/// <summary>
		/// A struct used for tracking both Tokens and States in the parsing process
		/// </summary>
		public struct ParseStackElement {
			public enum Type { Token, State }

			public Type type;
			public int parserStateIndex;
			public ProcessedToken scannedToken;

			public ParseStackElement(Type type, int parserState, ProcessedToken token = default(ProcessedToken)) {
				this.type = type;
				this.parserStateIndex = parserState;
				this.scannedToken = token;
			}

			public override string ToString() {
				return (type == Type.State) ? "State" + parserStateIndex : scannedToken.ToString();
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

			public string ToGetString() {
				return "new ParserInstruction(" + value + ", ParserInstruction.Instruction." + System.Enum.GetName(typeof(Instruction), instruction) +")";
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

		/// <summary>
		/// A token that has been processed during execution
		/// </summary>
		public struct ProcessedToken {
			public List<ProcessedToken> rhs;
			public bool terminal;
			public int tokenSymbolLocation;
			public CoroutineMethod rule;
			public object data;

			public override string ToString() {
				return (data != null) ? data.ToString() : "EOF";
			}

			/// <summary>
			/// Create a processed token for a NonTerminal
			/// </summary>
			/// <param name="tokenSymbol"></param>
			/// <param name="rule"></param>
			/// <param name="rhs"></param>
			public ProcessedToken(int tokenSymbolLocation, CoroutineMethod rule, List<ProcessedToken> rhs) {
				this.rhs = rhs;
				this.terminal = false;
				this.tokenSymbolLocation = tokenSymbolLocation;
				this.rule = rule;
				this.data = null;
			}

			/// <summary>
			/// Create a processed token for a Terminal
			/// </summary>
			/// <param name="tokenSymbol"></param>
			/// <param name="rule"></param>
			/// <param name="rhs"></param>
			public ProcessedToken(int tokenSymbolLocation, CoroutineMethod rule, object data) {
				this.rhs = null;
				this.terminal = true;
				this.tokenSymbolLocation = tokenSymbolLocation;
				this.rule = rule;
				this.data = data;
			}

			/// <summary>
			/// Execution of this rule, base definition executes the LHS and prepares it for use.
			/// </summary>
			public IEnumerator<object> Prepare(IOuroborosLanguage enviroment, Callback callback) {
				//If this is a terminal, return the data found for the RHS


				//Create a list the size of tokens
				List<object> expressionResults = new List<object>();

				if (!terminal) { 

					for (int i = 0; i < rhs.Count; i++) {
						if (rhs[i].terminal) {
							expressionResults[i] = rhs[i].data;
						}
						else {
							//This means it is a non terminal, so we must yield to its results, and pass it a llamda expression callback that assigns this specific member
							yield return GameManager._instance.StartCoroutine(rhs[i].rule(new CoroutineWrapper(enviroment, rhs[i], (object data) => { expressionResults[i] = data; })));
						}
					}
				}
				else {
					expressionResults.Add(data);
				}

				callback(expressionResults);
			}
		}

		/// <summary>
		/// A tuple for storing a string regex for a token symbol
		/// </summary>
		public struct SymbolStringTuple {
			public bool ignorable;
			public int result;
			public string str;
			public CoroutineMethod coroutineName;
			public SymbolStringTuple(int result, string str, CoroutineMethod coroutineName, bool ignorable = false) {
				this.result = result;
				this.str = str;
				this.coroutineName = coroutineName;
				this.ignorable = ignorable;
			}
		}

		/// <summary>
		/// A production rule for execution.
		/// </summary>
		public struct ProductionRule {
			public int result;
			public int rhsSize;
			public CoroutineMethod coroutineName;

			public ProductionRule(int result, int rhsSize, CoroutineMethod coroutineName) {
				this.result = result;
				this.rhsSize = rhsSize;
				this.coroutineName = coroutineName;
			}
		}

		public struct CoroutineWrapper {
			public IOuroborosLanguage enviroment;
			public ProcessedToken target;
			public Callback callback;
			public CoroutineWrapper(IOuroborosLanguage enviroment, ProcessedToken target, Callback callback) {
				this.enviroment = enviroment;
				this.target = target;
				this.callback = callback;
			}
		}
		#endregion
	}
}
