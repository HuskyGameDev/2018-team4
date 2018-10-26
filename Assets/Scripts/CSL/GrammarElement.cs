using UnityEngine;

namespace CSL {
  public class GrammarElement {
    /// <summary>
    /// The type of Grammar possible grammer elements
    /// </summary>
    public enum Type { Terminal, NonTerminal }


    /// <summary>
    /// The list of supported productionRules
    /// </summary>
    public abstract List<List<Type>> productionRules; 

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
    public abstract Type GetType();

    /// <summary>
    /// Returns the LHS regex string for this Terminal Grammar Element
    /// </summary>
    public virtual string GetRegex() { return ""; }

    /// <summary>
    /// Returns the LHS sequnce of tokens for this NonTerminal Grammar Element
    /// </summary>
    public virtual List<GrammarElement> GetExpression();

    public delegate Callback(object data);

    /// <summary>
    /// Execution of this rule, base definition executes the LHS and prepares it for use.
    /// </summary>
    public virtual IEnumerator<object> Execute(Script script, Callback callback) {
      //Create a list the size of tokens
      List<object> expressionResults = new List<object>(expressionElements.Count);
      for (int i = 0; i < elements.Count; i++) {
        if (elements[i].GetType() == Type.Terminal) {
          expressionResults[i] = expressionElements[i].data;
        }
        else {
          //This means is a non terminal, so we must yield to its results, and pass it a llamda expression callback that assigns this specific member
          yield return StartCoroutine(elements[i].Execute(script, (object data) => { expressionResults[i] = data; }));
        }
      }
      //expressionResults is now compilied
      
    }
  }
}
