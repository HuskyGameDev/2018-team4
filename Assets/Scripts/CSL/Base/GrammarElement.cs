using UnityEngine;
using System.Collections.Generic;

namespace BoardGameScripting {//This needs to be fixed, something to do with another script and gloabl stuff
    public abstract class GrammarElement {
        /// <summary>
        /// The type of Grammar possible grammer elements
        /// </summary>
        public enum Type { Terminal, NonTerminal }

        /// <summary>
        /// The list of supported productionRules
        /// </summary>
        public Dictionary<List<Type>, string> productionRules;

        /// <summary>
        /// The list of Processed elements for a NonTerminal on the LHS of it's production rule
        /// </summary>
        public List<GrammarElement> expressionElements;

        /// <summary>
        /// The data that a Terminal Element Represents
        /// </summary>
        public object data;

        /// <summary>
        /// Returns the type of grammar element this is
        /// </summary>
        public abstract Type GetTokenType();

        /// <summary>
        /// Returns the type of grammar element this is
        /// </summary>
        public virtual bool Ignorable() { return false; }

        /// <summary>
        /// Returns the LHS regex string for this Terminal Grammar Element
        /// </summary>
        public virtual string GetRegex() { return ""; }

        /// <summary>
        /// Returns the LHS sequnce of tokens for this NonTerminal Grammar Element
        /// </summary>
        public abstract List<GrammarElement> GetExpression();

        /// <summary>
        /// Execution of this rule, base definition executes the LHS and prepares it for use.
        /// </summary>
        public IEnumerator<object> Prepare(BGSGrammar.Callback callback) {
            //Create a list the size of tokens
            List<object> expressionResults = new List<object>(expressionElements.Count);
            for (int i = 0; i < expressionElements.Count; i++) {
                if (expressionElements[i].GetTokenType() == Type.Terminal) {
                    expressionResults[i] = expressionElements[i].data;
                }
                else {
                    //This means it is a non terminal, so we must yield to its results, and pass it a llamda expression callback that assigns this specific member
                    yield return GameManager.instance.StartCoroutine(expressionElements[i].Execute(script, (object data) => { expressionResults[i] = data; }));
                }
            }

            callback(expressionElements);
        }

        /// <summary>
        /// Prints a formatted list of items using its ToString method
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        private static string PrintList<T>(List<T> list, bool printList = true) {
          string ret = "";
          for (int i = 0; i < list.Count; i++) {
            ret += list[i].ToString() + " | ";
          }

          if (printList) Debug.Log(ret);
          return ret;
        }

        public struct ProductionRule() {
          /// <summary>
          /// Execution of this rule, base definition executes the LHS and prepares it for use.
          /// </summary>
          public virtual IEnumerator<object> Prepare(BGSGrammar.Callback callback) {
              //Create a list the size of tokens
              List<object> expressionResults = new List<object>(expressionElements.Count);
              for (int i = 0; i < expressionElements.Count; i++) {
                  if (expressionElements[i].GetTokenType() == Type.Terminal) {
                      expressionResults[i] = expressionElements[i].data;
                  }
                  else {
                      //This means it is a non terminal, so we must yield to its results, and pass it a llamda expression callback that assigns this specific member
                      yield return GameManager.instance.StartCoroutine(expressionElements[i].Execute(script, (object data) => { expressionResults[i] = data; }));
                  }
              }
              //callback(result);
          }

        }

    }
}
