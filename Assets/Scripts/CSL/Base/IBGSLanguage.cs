using System.Collections.Generic;

namespace BoardGameScripting {
  public interface IBGSLangauge {

    /// <summary>
    /// Returns an ordered list of Terminals to use in the language
    /// </sumamry>
    List<System.Type> GetOrderedTerminals();

    /// <summary>
    /// Returns an (bottom-up) ordered list of NonTerminals to use in the language
    /// </sumamry>
    List<System.Type> GetOrderedNonTerminals();

  }
}
