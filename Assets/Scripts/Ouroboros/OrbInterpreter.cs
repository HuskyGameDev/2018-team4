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
	/// Executes a token stream using a language definition.
	/// </summary>
	public class OrbInterpreter {
		public static readonly bool debugParseOutput = false;

		/// <summary>
		/// Operates on a Token stream using the ParsingTable
		/// </summary>
		/// <param name="tokens"></param>
		/// <param name="generateScript"></param>
		/// <returns></returns>
		public static IEnumerator<object> Execute<T>(string inputString) where T : IOrbLanguage {
			Debug.Log("Execute");
			//Create an instance of the language we are going to work with.
			IOrbLanguage language = (IOrbLanguage)System.Activator.CreateInstance(typeof(T));
			//Create a place to store out input tokens (nullable so null = EOF when processing)
			List<ProcessedToken?> inputTokens = null;
			//Call scan, to get our list of beginning tokens.
			yield return OrbLexer.Scan(language, inputString, (object d) => { inputTokens = (List<ProcessedToken?>)d; });
			Debug.Log("Scanning Finished");

			OrbUtils.PrintList<ProcessedToken?>(inputTokens);

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


				Debug.Log(OrbUtils.PrintList(parsingStack, false) + "|\\|/| -> " + inputTokens[currencyIndicator]);

				ParseStackElement top = parsingStack[parsingStack.Count - 1];
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

					Debug.Log(currentInputToken.data);
					Debug.Log(tokenColumn + "/" + (nonTerminals.Count + TERMINALS.Count));
					ParserInstruction targetInstruction = ParseTable[top.parserStateIndex][tokenColumn];

					if (targetInstruction.instruction == ParserInstruction.Instruction.ACCEPT) {

						ProcessedToken finalToken = (ProcessedToken)parsingStack[parsingStack.Count - 2].scannedToken;
						//Execute the final rule.
						language.StartLangCoroutine(finalToken.rule, new CoroutineWrapper(language, finalToken, (object data) => { }));
						break;
					}
					else if (targetInstruction.instruction == ParserInstruction.Instruction.SHIFT) {
						Debug.Log("Shift");
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
						OrbUtils.PrintList(parsingStack);
						Debug.Log(currentInputToken);
						inputTokens.RemoveRange(0, currencyIndicator);
						//PrintList(inputTokens);
						Debug.LogError("Stack parsing has broken due to invalid Instruction: " + System.Enum.GetName(typeof(ParserInstruction.Instruction), targetInstruction.instruction));
						yield break;
					}

				}

			}
			Debug.Log("EndExecute");
		}
	}
}