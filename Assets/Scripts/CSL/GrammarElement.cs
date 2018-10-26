using UnityEngine;
using System.Collections.Generic;

namespace CSLg {//This needs to be fixed, something to do with another script and gloabl stuff
  public abstract class GrammarElement {
    /// <summary>
    /// The type of Grammar possible grammer elements
    /// </summary>
    public enum Type { Terminal, NonTerminal }


    /// <summary>
    /// The list of supported productionRules
    /// </summary>
    public List<List<Type>> productionRules; 

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
    /// Returns the LHS regex string for this Terminal Grammar Element
    /// </summary>
    public virtual string GetRegex() { return ""; }

    /// <summary>
    /// Returns the LHS sequnce of tokens for this NonTerminal Grammar Element
    /// </summary>
    public abstract List<GrammarElement> GetExpression();

    public delegate void Callback(object data);

    /// <summary>
    /// Execution of this rule, base definition executes the LHS and prepares it for use.
    /// </summary>
    public virtual IEnumerator<object> Execute(CSL.Script script, Callback callback) {
      //Create a list the size of tokens
      List<object> expressionResults = new List<object>(expressionElements.Count);
      for (int i = 0; i < expressionElements.Count; i++) {
        if (expressionElements[i].GetTokenType() == Type.Terminal) {
          expressionResults[i] = expressionElements[i].data;
        }
        else {
          //This means is a non terminal, so we must yield to its results, and pass it a llamda expression callback that assigns this specific member
          //yield return StartCoroutine(expressionElements[i].Execute(script, (object data) => { expressionResults[i] = data; }));
        }
      }
            //expressionResults is now compilied
            yield return null;
    }
  }
}
