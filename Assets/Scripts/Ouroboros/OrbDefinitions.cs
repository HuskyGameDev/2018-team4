using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OrbScripting {
	/// <summary>
	/// Holds the definition for smaller classes used in Orb Processesing
	/// </summary>
	public class OrbDefinitions {

		public delegate IEnumerator<object> CoroutineMethod(CoroutineWrapper coroutineWrapper);
		public delegate void Callback(object data);

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
				return "new ParserInstruction(" + value + ", ParserInstruction.Instruction." + System.Enum.GetName(typeof(Instruction), instruction) + ")";
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

			/*public static void PrintList(List<ProcessedToken?> list, IOuroborosLanguage context) {
				string ret = "";
				for (int i = 0; i < list.Count; i++) {
					//ret += ((list[i] != null) ? context.get.ToString() : "EOF") + " | ";
				}

				Debug.Log(ret);
			}*/

			public override string ToString() {
				return ((terminal) ? "T:" : "Nt:") + tokenSymbolLocation;
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
			public IEnumerator<object> Prepare(IOrbLanguage enviroment, Callback callback) {
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
							yield return enviroment.StartLangCoroutine(rhs[i].rule, new CoroutineWrapper(enviroment, rhs[i], (object data) => { expressionResults[i] = data; })   );
							//yield return GameManager._instance.StartCoroutine(rhs[i].rule(new CoroutineWrapper(enviroment, rhs[i], (object data) => { expressionResults[i] = data; })));
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

		/// <summary>
		/// Wraps the data for executing a generated coroutine within a single chunk
		/// </summary>
		public struct CoroutineWrapper {
			public IOrbLanguage enviroment;
			public ProcessedToken target;
			public Callback callback;
			public CoroutineWrapper(IOrbLanguage enviroment, ProcessedToken target, Callback callback) {
				this.enviroment = enviroment;
				this.target = target;
				this.callback = callback;
			}
		}

	}
}
