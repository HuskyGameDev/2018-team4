using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

namespace OrbScripting {

	[System.Serializable]
	public class OrbNonCompiledLanguage {
		public string name;
		public List<TerminalToken> terminalTokens;
		public List<NonTerminalToken> nonTerminalTokens;

		public OrbNonCompiledLanguage() {
			  name = "NewLanguage";
			  terminalTokens = new List<TerminalToken>();
			  nonTerminalTokens = new List<NonTerminalToken>();
		}

		public static OrbNonCompiledLanguage Load(string languageName) {
			string filePath = Application.dataPath + "/Assets/Ouroboros/LanguageFiles/"+ languageName+"/" + languageName + ".json";
			//check to see if we have a save copy, if not return a new one.
			if (File.Exists(filePath)) {
				return JsonUtility.FromJson<OrbNonCompiledLanguage>(File.ReadAllText(filePath));
			}
			else { 
				return null;
			}
		}

		public static void Save(OrbNonCompiledLanguage file) {
			string filePath = Application.dataPath + "/Assets/Ouroboros/LanguageFiles/" + file.name + "/" + file.name + ".json";
			File.WriteAllText(filePath, JsonUtility.ToJson(file, true));
		}


		public class NonTerminalToken {
			public string LHS_Token;
			public List<TerminalToken> rhsTokens;
		}
		public class TerminalToken {
			public string LHS_Token;
			public OrbLexer.LexMode mode;
			public string regex;
		}

		public static OrbNonCompiledLanguage GenerateNewLanguageStructure() {
			string rootpath = Application.dataPath + "/Ouroboros/LanguageFiles";
			//Check to see if the root folder exists, create it if it doesnt
			if (System.IO.Directory.Exists(rootpath)) {
				System.IO.Directory.CreateDirectory(rootpath);
			}

			//Search the root folder for a new language number that doesnt exist yet
			int num = 0;
			while (System.IO.Directory.Exists(rootpath + "/NewLanguage" + num)) {
				num++;
			}
			string langPath = rootpath + "/NewLanguage" + num;
			//Make the new directory
			System.IO.Directory.CreateDirectory(langPath);

			System.IO.Directory.CreateDirectory(langPath + "/Terminals");
			System.IO.Directory.CreateDirectory(langPath + "/NonTerminals");

			//Create enviroment file
			System.IO.File.Create(langPath + "/enviroment.chunk");
			//Create Using File
			System.IO.File.Create(langPath + "/using.chunk");
			//Create Preexecution File
			System.IO.File.Create(langPath + "/preexecution.chunk");

			OrbNonCompiledLanguage lang = new OrbNonCompiledLanguage();
			lang.name = "NewLanguage" + num;

			//Save the new language to 

			//Refresh the asset database to allow for the Project panel to display our new folder
			UnityEditor.AssetDatabase.Refresh();
			return lang;
		}
	}
}