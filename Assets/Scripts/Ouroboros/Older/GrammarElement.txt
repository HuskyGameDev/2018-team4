using UnityEngine;
using System.Collections.Generic;

namespace BoardGameScripting {

	/// <summary>
	/// A token, or GrammarElement. It either contians a Regex string to match or a list of possible Production Rules.
	/// </summary>
    public abstract class GrammarElement {

		#region TypeElements
		/// <summary>
		/// The type of Grammar possible grammer elements
		/// </summary>
		public enum TokenType { Terminal, NonTerminal }

        /// <summary>
        /// Returns the type of grammar element this is
        /// </summary>
        public abstract TokenType GetTokenType();

		#region NonTerminal
		/// <summary>
		/// The list of supported productionRules for this type.
		/// </summary>
		public static ProductionRule[] GetRules() { return new ProductionRule[] { }; }
		#endregion

		#region Terminal
		/// <summary>
		/// Returns the type of grammar element this is
		/// </summary>
		public virtual bool Ignorable() { return false; }

        /// <summary>
        /// Returns the LHS regex string for this Terminal Grammar Element
        /// </summary>
        public virtual string GetRegex() { return ""; }
		#endregion

		#endregion

		#region InstanceElements

		#region NonTerminal
		/// <summary>
		/// The production rule for this Instance
		/// </summary>
		public ProductionRule productionRule;

        /// <summary>
        /// The list of Processed elements for a NonTerminal instance of this Grammar Element (used for the production rule)
        /// </summary>
        public List<GrammarElement> expressionElements;
		#endregion

		#region Terminal
		/// <summary>
		/// The data that a Terminal Element Represents
		/// </summary>
		public object data;
		#endregion

		#endregion

		public static List<ProductionRule> GetRulesFromType(System.Type type)  {
			return (List<ProductionRule>)type.GetMethod("GetRules").Invoke(null, null);
		}

		public static string GetRegexFromType(System.Type type) {
			return (string)type.GetMethod("GetRegex").Invoke(null, null);
		}

		public static GrammarElement GetInstanceFromType(System.Type type) {
			return (GrammarElement)type.GetMethod("GetRegex").Invoke(null, null);
		}


		/// <summary>
		/// Execution of this rule, base definition executes the LHS and prepares it for use.
		/// </summary>
		public IEnumerator<object> Prepare(BGSGrammar.Callback callback) {
            //Create a list the size of tokens
            List<object> expressionResults = new List<object>(expressionElements.Count);
            for (int i = 0; i < expressionElements.Count; i++) {
                if (expressionElements[i].GetTokenType() == TokenType.Terminal) {
                    expressionResults[i] = expressionElements[i].data;
                }
                else {
                    //This means it is a non terminal, so we must yield to its results, and pass it a llamda expression callback that assigns this specific member
                    yield return GameManager._instance.StartCoroutine(expressionElements[i].productionRule.Execute(this, (object data) => { expressionResults[i] = data; }));
                }
            }

            callback(expressionElements);
        }


        public abstract class ProductionRule {

			/// <summary>
			/// The left hand side of this production rule.
			/// </summary>
			/// <returns></returns>
			public abstract System.Type GetNonTerminal();

			/// <summary>
			/// The right hand side of the production rule
			/// </summary>
			public abstract List<System.Type> GetRHSElements();

			/// <summary>
			/// Execution of this rule, base definition executes the LHS and prepares it for use.
			/// </summary>
			public virtual IEnumerator<object> Execute(GrammarElement element, BGSGrammar.Callback callback) {
				//Execute the lower order rules
				List<object> expressionResults = new List<object>();
				yield return GameManager._instance.StartCoroutine(element.Prepare( (object data) => { expressionResults = (List<object>)data; } ));


				if (BGSGrammar.debugParseOutput) {
					//Print this rule out by default.
					string rhs = "";
					for (int i = 0; i < element.expressionElements.Count; i++) {
						rhs += " " + element.expressionElements[i].GetType().Name + "(" + (expressionResults[i] == null ? "null" : expressionResults[i].ToString()) + ")";
					}

					Debug.Log(element.GetType().Name + " ->" + rhs);
				}

				callback(null);
			}
        }

    }
}
