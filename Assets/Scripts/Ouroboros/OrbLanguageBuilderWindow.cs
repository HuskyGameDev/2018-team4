using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.AnimatedValues;
using LangFile = OrbScripting.OrbNonCompiledLanguage;
using TerminalChunk = OrbScripting.OrbNonCompiledLanguage.TerminalToken;
using NonTerminalChunk = OrbScripting.OrbNonCompiledLanguage.NonTerminalToken;

namespace OrbScripting {
	public class OrbLanguageBuilderWindow : EditorWindow {

		/// <summary>
		/// Enables window activation in the unity windows menu
		/// </summary>
		[MenuItem("Window/Orb Language Builder")]
		public static void ShowWindow() {
			EditorWindow.GetWindow(typeof(OrbLanguageBuilderWindow));
		}

		OrbWindowSettings windowSettings = null;




		public void OnDestroy() {
			//[TODO] Save the relevant information somewhere.
			Debug.Log("Window Destroyed");
		}

		private void OnGUI() {
			//Add A switch between different Menus
			DrawDefaultMenu();

		}

		private void DrawDefaultMenu() {

			if (windowSettings == null) {
				windowSettings = OrbWindowSettings.Load();
				//Check to see if we had a language we were working on
				if (windowSettings.languageName != "") {
					//Attempt to load it
				}
			}



			EditorGUILayout.BeginHorizontal();
			{
				if (GUILayout.Button("Load")) {
					//Attempt to load.
				}
				if (windowSettings.language != null && GUILayout.Button("Save")) {
					//Save the file.
				}
				if (GUILayout.Button("New")) {
					windowSettings.language = OrbNonCompiledLanguage.GenerateNewLanguageStructure();
					//Look for non colliding name for language
					//Create file strucuture
				}
				if (GUILayout.Button("Close")) {
					windowSettings.language = null;
					//Look for non colliding name for language
					//Create file strucuture
				}
			}
			EditorGUILayout.EndHorizontal();

			//File loading options
			//File saving options
			//New language button

			if (windowSettings.language != null) {
				//Name
				EditorGUILayout.BeginHorizontal();
				{
					windowSettings.language.name = EditorGUILayout.TextField("Name:", windowSettings.language.name);
				}
				EditorGUILayout.EndHorizontal();

				//Package Manager
				HorizontalLine(Color.grey);
				if (windowSettings.m_PackageManager = EditorGUILayout.Foldout(windowSettings.m_PackageManager, "Package manager")) {
					DrawPackageManger();
				}

				//Using Segment
				HorizontalLine(Color.grey);
				if (windowSettings.m_UsingDirectives = EditorGUILayout.Foldout(windowSettings.m_UsingDirectives, "Using Directives")) {
					DrawUsingSegment();
				}

				//Static data segment
				HorizontalLine(Color.grey);
				if (windowSettings.m_Enviroment = EditorGUILayout.Foldout(windowSettings.m_Enviroment, "Global Enviroment")) {
					DrawEnviromentManager();
				}

				//Pre execution code
				HorizontalLine(Color.grey);
				if (windowSettings.m_Preexecution = EditorGUILayout.Foldout(windowSettings.m_Preexecution, "Pre-execution")) {
					DrawPreExecution();
				}

				//Terminal Tokens and Regex
				HorizontalLine(Color.grey);
				if (windowSettings.m_Terminals = EditorGUILayout.Foldout(windowSettings.m_Terminals, "Terminal Tokens")) {
					DrawTerminalTokens();
				}

				//Non terminal Tokens and RHS
				HorizontalLine(Color.grey);
				if (windowSettings.m_Nonterminals = EditorGUILayout.Foldout(windowSettings.m_Nonterminals, "Nonterminal Tokens")) {
					DrawNonerminalTokens();
					//Display a dropdown of all other tokens
				}
			}

			OrbWindowSettings.Save(windowSettings);
		}



		private void DrawPackageManger() {
			EditorGUILayout.LabelField("Unsupported");
		}
		private void DrawUsingSegment() {
			EditorGUILayout.LabelField("Unfinished 2");
		}
		private void DrawEnviromentManager() {
			EditorGUILayout.LabelField("Unfinished 3");
		}
		private void DrawPreExecution() {
			EditorGUILayout.LabelField("Unfinished 4");
		}
		private void DrawTerminalTokens() {
			EditorGUILayout.LabelField("Unfinished 5");
			//DrawTerminal(null, null);
			if (GUILayout.Button("New Terminal")) {
				//Create a new terminal data
				TerminalChunk chunk = new TerminalChunk();
				//[TODO] make this search for a non-taken name
				chunk.LHS_Token = "NEWTERMINAL";
				chunk.regex = "";
				chunk.mode = OrbLexer.LexMode.Regex;

				windowSettings.language.terminalTokens.Add(chunk);
			}
			//Draw UI for all tokens
			for (int i = 0; i < windowSettings.language.terminalTokens.Count; i++) {
				DrawTerminal(windowSettings.language, windowSettings.language.terminalTokens[i]);
			}
		}
		private void DrawNonerminalTokens() {
			EditorGUILayout.LabelField("Unfinished 6");
		}

		static GUIStyle horizontalLine = new GUIStyle();

		static void DrawEditButton(string target) {
			if (GUILayout.Button("Edit Code")) {
				System.Diagnostics.Process.Start(target);
			}
		}

		static void DrawTerminal(LangFile file, TerminalChunk chunk) {
			EditorGUILayout.BeginHorizontal(EditorStyles.helpBox);
			{
				EditorGUILayout.BeginVertical();
				{
					//Move Up Precedence
					if (GUILayout.Button("Up")) {
						//Search for this terminal in the list

						for (int i = 0; i < file.terminalTokens.Count; i++) {
							if (file.terminalTokens[i] == chunk) {
								//If it isnt the first one
								if (i != 0) {
									TerminalChunk temp;
									//Perform a swap with the one after it
									temp = file.terminalTokens[i];
									file.terminalTokens[i] = file.terminalTokens[i - 1];
									file.terminalTokens[i - 1] = temp;
								}
								break;
							}
						}
					}
					//Move Down Precedence
					if (GUILayout.Button("Down")) {
						//Search for this terminal in the list
						for (int i = 0; i < file.terminalTokens.Count; i++) {
							if (file.terminalTokens[i] == chunk) {
								//If it isnt the last one
								if (i != file.terminalTokens.Count - 1) {
									//Perform a swap with the one after it
									TerminalChunk temp;
									temp = file.terminalTokens[i];
									file.terminalTokens[i] = file.terminalTokens[i + 1];
									file.terminalTokens[i + 1] = temp;
								}
								break;
							}
						}
					}
				}
				EditorGUILayout.EndVertical();
				//Name
				chunk.LHS_Token = EditorGUILayout.TextField("Identifier:", chunk.LHS_Token);
				//Mode
				//EditorGUILayout.TextField("Mode:", "Enum");
				chunk.mode = (OrbLexer.LexMode)EditorGUILayout.EnumPopup(chunk.mode);
				//Regex/string match
				chunk.regex = EditorGUILayout.TextField("Regex/String:", chunk.regex);

				string path = Application.dataPath + "/Assets/Ouroboros/LanguageFiles" + "/" + file.name + "/" + chunk.LHS_Token + ".cs";
				//Create the file if it doesnt exist, otherwise leave it alone.
				System.IO.File.AppendText(path).Close();
				//Edit Code
				DrawEditButton(path);
			}
			EditorGUILayout.EndHorizontal();
		}

		static void HorizontalLine(Color color) {
			horizontalLine.normal.background = EditorGUIUtility.whiteTexture;
			horizontalLine.margin = new RectOffset(0, 0, 4, 4);
			horizontalLine.fixedHeight = 1;
			var c = GUI.color;
			GUI.color = color;
			GUILayout.Box(GUIContent.none, horizontalLine);
			GUI.color = c;
		}
	}
}
