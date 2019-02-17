using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OrbScripting {
	/// <summary>
	/// Contains utility methods for language processesing
	/// </summary>
	public class OrbUtils {
		/// <summary>
		/// Prints a formatted list of items using its ToString method
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="list"></param>
		public static string PrintList<T>(List<T> list, bool printList = true) {
			string ret = "";
			for (int i = 0; i < list.Count; i++) {
				ret += ((list[i] != null) ? list[i].ToString() : "NULL") + " | ";
			}

			if (printList) Debug.Log(ret);
			return ret;
		}
	}
}