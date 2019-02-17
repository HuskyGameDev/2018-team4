using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using LangFile = OrbScripting.OrbNonCompiledLanguage;
using TerminalChunk = OrbScripting.OrbNonCompiledLanguage.TerminalToken;
using NonTerminalChunk = OrbScripting.OrbNonCompiledLanguage.NonTerminalToken;


namespace OrbScripting {
	[System.Serializable]
	public class OrbWindowSettings {
		private const string fileName = "/orbWindowsSettings.json";

		//Dont save the lang file, that lis loaded elsewhere.
		[System.NonSerialized] public LangFile language = null;

		/// <summary>
		/// Name used to search for our language file
		/// </summary>
		public string languageName = "";
		public bool m_PackageManager = true;
		public bool m_UsingDirectives = true;
		public bool m_Preexecution = true;
		public bool m_Enviroment = true;
		public bool m_Terminals = true;
		public bool m_Nonterminals = true;

		public static OrbWindowSettings Load() {
			//check to see if we have a save copy, if not return a new one.
			if (File.Exists(Application.persistentDataPath + fileName)) {
				string jSonData = File.ReadAllText(Application.persistentDataPath + fileName);
				return JsonUtility.FromJson<OrbWindowSettings>(jSonData);
			}
			else {
				return new OrbWindowSettings();
			}
		}

		public static void Save(OrbWindowSettings file) {
			string jsonData = JsonUtility.ToJson(file, true);
			File.WriteAllText(Application.persistentDataPath + fileName, jsonData);
		}
	}
}