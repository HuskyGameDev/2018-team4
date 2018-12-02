using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

static public class InputManager {
	/*TODO: mouse position/click stuff
	 *		joystick mouse-control?
	 *		
	 *		keybindings
	 *			Save keybinds to file for later
	 *			retrieve keybinds from file
	 *			let player modify keybinds
	*/

	public enum Action { up, down, left, right, confirm, cancel, action_1, action_2, action_3};
	private static Keybindings keybindings = null;
	private static Keybindings temp_keybindings = null;
	private static System.Runtime.Serialization.IFormatter formatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();

	[System.Serializable]
	private class Keybindings {
		public string[,] keys;
		/*
		public string up_1;	//1
		public string up_2;
		public string down_1;	//2
		public string down_2;
		public string left_1;	//3
		public string left_2;
		public string right_1;	//4
		public string right_2;
		public string confirm_1;	//5
		public string confirm_2;
		public string cancel_1;	//6
		public string cancel_2;
		public string action1_1;	//7
		public string action1_2;
		public string action2_1;	//8
		public string action2_2;
		public string action3_1;	//9
		public string action3_2;*/

		public Keybindings() {
			keys = new string[9, 2];
			keys[0, 0] = "up";
			keys[0, 1] = "w";
			keys[1, 0] = "down";
			keys[1, 1] = "s";
			keys[2, 0] = "left";
			keys[2, 1] = "a";
			keys[3, 0] = "right";
			keys[3, 1] = "d";
			keys[4, 0] = "enter";
			keys[4, 1] = "joystick button 0";
			keys[5, 0] = "escape";
			keys[5, 1] = "joystick button 1";
			keys[6, 0] = "left ctrl";
			keys[6, 1] = "mouse 0";
			keys[7, 0] = "left alt";
			keys[7, 1] = "mouse 1";
			keys[8, 0] = "left shift";
			keys[8, 1] = "mouse 2";
		}

		public Keybindings Copy() {
			Keybindings copy = new Keybindings();
			for (int i = 0; i < 9; i++) {
				copy.keys[i, 0] = this.keys[i, 0];
				copy.keys[i, 1] = this.keys[i, 1];
			}

			return copy;
		}
	}

	public static void LoadKeybinds() {
		if (File.Exists(Application.persistentDataPath + "\\Keybindings.txt")) {
			//Debug.Log("Loading keybinds from location: \"" + Application.persistentDataPath + "\\Keybindings.txt\"");
			Stream stream = new FileStream(Application.persistentDataPath + "\\Keybindings.txt", FileMode.Open, FileAccess.Read);
			keybindings = (Keybindings)formatter.Deserialize(stream);
			temp_keybindings = keybindings.Copy();
		} else {
			//Debug.Log("Loading default keybinds");
			keybindings = new Keybindings();
			temp_keybindings = keybindings.Copy();
		}
	}

	public static void ResetKeybinds() {
		temp_keybindings = new Keybindings();
	}

	public static void ApplyKeybinds() {
		if (temp_keybindings == null) {
			temp_keybindings = new Keybindings();
		}
		keybindings = temp_keybindings.Copy();

		Debug.Log("Saving to location: \"" + Application.persistentDataPath + "\\Keybindings.txt\"");
		Stream stream = new FileStream(Application.persistentDataPath + "\\Keybindings.txt", FileMode.Create, FileAccess.Write);
		formatter.Serialize(stream, keybindings);
		stream.Close();
	}

	public static void DiscardKeybinds() {
		if (temp_keybindings == null) {
			temp_keybindings = new Keybindings();
		} else {
			temp_keybindings = keybindings.Copy();
		}
	}

	public static void ModifyKeybinds(Action action, bool primary, string newKey) {
		int action_num = (int)action;
		int action_primary = 1;
		if (primary) action_primary = 0;

		int replace_num = -1;
		int replace_primary = -1;
		for (int i = 0; i < 9; i++) {
			if (action_num != i) {
				if (temp_keybindings.keys[i, 0].Equals(newKey)) {
					replace_num = i;
					replace_primary = 0;
				} else if (temp_keybindings.keys[i, 1].Equals(newKey)) {
					replace_num = i;
					replace_primary = 1;
				}
			}
		}

		if (replace_num >= 0) {
			temp_keybindings.keys[replace_num, replace_primary] = temp_keybindings.keys[action_num, action_primary];
		}
		temp_keybindings.keys[action_num, action_primary] = newKey;
	}

	/*
	public Vector2 GetMousePos() {
		return new Vector2(2.0f, 2.0f);
	}
	*/

	public static bool OnInput(Action action) {
		//Debug.Log("Checking \"" + action + "\"");
		int action_num = (int)action;
		//Debug.Log("Action \"" + action + "\" is: " + action_num);
		return (Input.GetKey(keybindings.keys[action_num,0]) | Input.GetKey(keybindings.keys[action_num, 1]));
	}

	public static bool OnInputDown(Action action) {
		int action_num = (int)action;
		return (Input.GetKeyDown(keybindings.keys[action_num, 0]) | Input.GetKeyDown(keybindings.keys[action_num, 1]));
	}

	public static bool OnInputUp(Action action) {
		int action_num = (int)action;
		return (Input.GetKeyUp(keybindings.keys[action_num, 0]) | Input.GetKeyUp(keybindings.keys[action_num, 1]));
	}

}
