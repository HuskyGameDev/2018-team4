using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OrbScripting {
	/// <summary>
	/// Validates a token stream or language defintion.
	/// </summary>
	public class OrbLanguageValidator {

		/// <summary>
		/// Confirms that there are no naming collisions or errors within a language.
		/// </summary>
		/// <returns></returns>
		public bool ValidateLanguageDefintion() {
			return true;
		}

		/// <summary>
		/// Validates that a language token stream can be interpereted by the interpreter (analog for compile time checking of a script)
		/// </summary>
		/// <returns></returns>
		public bool ValidateTokenStream() {
			return true;
		}

	}
}