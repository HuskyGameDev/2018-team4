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
		//public string[,] keys;
		public KeyCode[,] keys;

		public Keybindings() {
			/*keys = new string[9, 2];
			keys[0, 0] = "up";	// up
			keys[0, 1] = "w";
			keys[1, 0] = "down";	// down
			keys[1, 1] = "s";
			keys[2, 0] = "left";	// left
			keys[2, 1] = "a";
			keys[3, 0] = "right";	// right
			keys[3, 1] = "d";
			keys[4, 0] = "enter";	// confirm
			keys[4, 1] = "b";
			keys[5, 0] = "escape";	// cancel
			keys[5, 1] = "v";
			keys[6, 0] = "left ctrl";	// action_1
			keys[6, 1] = "mouse 0";
			keys[7, 0] = "left alt";	// action_2
			keys[7, 1] = "mouse 1";
			keys[8, 0] = "left shift";	// action_3
			keys[8, 1] = "mouse 2";*/
			keys = new KeyCode[9, 2];
			keys[0, 0] = KeyCode.UpArrow;  // up
			keys[0, 1] = KeyCode.W;
			keys[1, 0] = KeyCode.DownArrow;    // down
			keys[1, 1] = KeyCode.S;
			keys[2, 0] = KeyCode.LeftArrow;    // left
			keys[2, 1] = KeyCode.A;
			keys[3, 0] = KeyCode.RightArrow;   // right
			keys[3, 1] = KeyCode.D;
			keys[4, 0] = KeyCode.Return;   // confirm
			keys[4, 1] = KeyCode.KeypadEnter;
			keys[5, 0] = KeyCode.Escape;  // cancel
			keys[5, 1] = KeyCode.Space;
			keys[6, 0] = KeyCode.LeftControl;   // action_1
			keys[6, 1] = KeyCode.Mouse0;
			keys[7, 0] = KeyCode.LeftAlt;    // action_2
			keys[7, 1] = KeyCode.Mouse0;
			keys[8, 0] = KeyCode.LeftShift;  // action_3
			keys[8, 1] = KeyCode.Mouse2; 
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

		//Debug.Log("Saving to location: \"" + Application.persistentDataPath + "\\Keybindings.txt\"");
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

	public static void ModifyKeybinds(Action action, bool primary, KeyCode newKey) {
		//Debug.Log("ModifyKeybind: " + action + ", primary:" + primary + ", to: " + newKey);
		int action_num = (int)action;
		int action_primary = 1;
		if (primary) action_primary = 0;

		int replace_num = -1;
		int replace_primary = -1;
		for (int i = 0; i < 9; i++) {
			if (action_num != i) {
				if (temp_keybindings.keys[i, 0] == newKey) {
					replace_num = i;
					replace_primary = 0;
				} else if (temp_keybindings.keys[i, 1] == newKey) {
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
		int action_num = (int)action;
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

	/*public static void WaitForKeybindInput(Action action, bool primary) {
		MonoBehaviour.StartCoroutine(WaitForKeybindInput_Coroutine(action, primary));
	}*/

	static public IEnumerator<object> WaitForKeybindInput(Action action, bool primary) {
		while (Input.anyKey) {
			//Debug.Log("Waiting for key release");
			yield return null;
		}

		yield return null;

		bool loop = true;

		while (loop) {
			//Debug.Log("Waiting for key press");
			Event e = new Event();
			while ((Event.GetEventCount() > 0) && loop) {
				Event.PopEvent(e);
				//Debug.Log("Events to test");
				if (e.isKey) {
					//Debug.Log("Detected key code: " + e.keyCode);
					ModifyKeybinds(action, primary, e.keyCode);
					loop = false;
				} else if (e.isMouse) {
					//Debug.Log("Detected mouse button: " + e.button);
					switch (e.button) {
						case 0:
							ModifyKeybinds(action, primary, KeyCode.Mouse0);
							break;
						case 1:
							ModifyKeybinds(action, primary, KeyCode.Mouse1);
							break;
						case 2:
							ModifyKeybinds(action, primary, KeyCode.Mouse2);
							break;
						case 3:
							ModifyKeybinds(action, primary, KeyCode.Mouse3);
							break;
						case 4:
							ModifyKeybinds(action, primary, KeyCode.Mouse4);
							break;
						case 5:
							ModifyKeybinds(action, primary, KeyCode.Mouse5);
							break;
						case 6:
							ModifyKeybinds(action, primary, KeyCode.Mouse6);
							break;
					}
					loop = false;
				}
			}
			
			yield return null;
		}

		yield break;
	}
}

