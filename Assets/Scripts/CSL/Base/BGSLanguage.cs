using UnityEngine;
using System.Collections.Generic;

namespace BoardGameScripting {
  public interface BGSLangauge {

    /// <summary>
    /// Returns an ordered list of Terminals to use in the language
    /// </sumamry>
    List<Type> GetOrderedTerminals();

    /// <summary>
    /// Returns an (bottom-up) ordered list of NonTerminals to use in the language
    /// </sumamry>
    List<Type> GetOrderedNonTerminals();

  }
}
