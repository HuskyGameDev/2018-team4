using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

static public class InputManager {
	/*TODO: NO---mouse position/click stuff---NO
	 * 
	 *		Make into a singleton rather than static
	 * 
	 *		joystick - "Axis_1h" & "Axis_1v"
	 *			button-up/down
	 *			diagonal-directions
	 *			Should input check every frame happen in the input manager? Or should input be checked every frame by things looking for input?
	 *				check happens in input-manager, sends notifications when receive input
	 *		
	 *		Make sure buttonDown/Up does not trigger twice from both buttons being pressed if the first button is down when the second if pressed
	*/

	public enum Action { up, down, left, right, confirm, cancel, action_1, action_2, action_3};	// what actions/inputs we will has keybindings for
	private static Keybindings keybindings = null;	// keybindings for manager to use when checking button presses
	private static Keybindings temp_keybindings = null;	// potential changes to keybindings, can then be applied or discarded
	private static System.Runtime.Serialization.IFormatter formatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();	// used for saving/loading keybindings from file

	/// <summary>
	/// Holds keybindings
	/// </summary>
	[System.Serializable]	// this class can be saved/loaded from file
	private class Keybindings {
		public KeyCode[,] keys;	// 9 tall, for 9 actions, and 2 wide, for 2 keybindings per action

		/// <summary>
		/// Constructor, with default keybindings.
		/// </summary>
		public Keybindings() {
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

		/// <summary>
		/// Copies the keybindings from one instance to another
		/// </summary>
		/// <returns></returns>
		public Keybindings Copy() {
			Keybindings copy = new Keybindings();
			for (int i = 0; i < 9; i++) {
				copy.keys[i, 0] = this.keys[i, 0];
				copy.keys[i, 1] = this.keys[i, 1];
			}

			return copy;
		}
	}

	/// <summary>
	/// Loads keybindings from file if file exist, else loads default keybindsings. 
	/// </summary>
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

	/// <summary>
	/// Sets temp_keybindings to match default keybindings.
	/// </summary>
	public static void ResetKeybinds() {
		temp_keybindings = new Keybindings();
	}

	/// <summary>
	/// Sets keybindings to match temp_keybindings, and saves to file as well.
	/// </summary>
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

	/// <summary>
	/// Discards changes to temp_keybindings. Sets temp_keybindings to match keybindings.
	/// </summary>
	public static void DiscardKeybinds() {
		if (temp_keybindings == null) {
			temp_keybindings = new Keybindings();
		} else {
			temp_keybindings = keybindings.Copy();
		}
	}

	/// <summary>
	/// Modifys a bindings in temp_keybindings. If given key matches one already in use elsewhere, swaps the keybinding that is being replaced and the one that was already in use.
	/// </summary>
	/// <param name="action">Action to change keybinding of</param>
	/// <param name="primary">Should the primary or secondary keybinding be changed</param>
	/// <param name="newKey">New key to set the keybinding to</param>
	public static void ModifyKeybinds(Action action, bool primary, KeyCode newKey) {
		//Debug.Log("ModifyKeybind: " + action + ", primary:" + primary + ", to: " + newKey);
		int action_num = (int)action;
		int action_primary = 1;
		if (primary) action_primary = 0;

		int replace_num = -1;
		int replace_primary = -1;
		for (int i = 0; i < 9; i++) {	// find if the newKey matches any already in use.
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

		if (replace_num >= 0) {	// swap newKey and replaced key is newKey was already in use.
			temp_keybindings.keys[replace_num, replace_primary] = temp_keybindings.keys[action_num, action_primary];
		}
		temp_keybindings.keys[action_num, action_primary] = newKey;
	}

	/*
	/// <summary>
	/// returns the mouse position in pixels on the screen, not in world-coordinates
	/// (0,0) is bottom left corner, top right is (pixelWidth,pixelHeight)
	/// </summary>
	/// <returns></returns>
	public static Vector3 GetMousePos() {
		//Vector3 mousePos = Camera.ScreenToWorldPoint(Input.mousePosition);
		return Input.mousePosition;
		//return new Vector2(2.0f, 2.0f);
	}*/
	
	/// <summary>
	/// 
	/// </summary>
	void Update() {

	}

	/// <summary>
	/// Check if either of the keys bound to the action are pressed down.
	/// </summary>
	/// <param name="action"></param>
	/// <returns></returns>
	public static bool OnInput(Action action) {
		int action_num = (int)action;
		
		if (action_num >= 0 & action_num <= 3) {
			bool joyStick = false;
			switch (action_num) {
				case 0: // up
					joyStick = (Input.GetAxis("Axis_1v") < 0);
					break;
				case 1: // down
					joyStick = (Input.GetAxis("Axis_1v") > 0);
					break;
				case 2: // left
					joyStick = (Input.GetAxis("Axis_1h") < 0);
					break;
				case 3: // right
					joyStick = (Input.GetAxis("Axis_1h") > 0);
					break;
			}
			return (Input.GetKey(keybindings.keys[action_num, 0]) | Input.GetKey(keybindings.keys[action_num, 1]) | joyStick);
		} else {
			return (Input.GetKey(keybindings.keys[action_num, 0]) | Input.GetKey(keybindings.keys[action_num, 1]));
		}

		//return (Input.GetKey(keybindings.keys[action_num,0]) | Input.GetKey(keybindings.keys[action_num, 1]));
	}

	/// <summary>
	/// Check if either of the keys bound to the action have been pressed down this frame.
	/// </summary>
	/// <param name="action"></param>
	/// <returns></returns>
	public static bool OnInputDown(Action action) {
		int action_num = (int)action;
		return (Input.GetKeyDown(keybindings.keys[action_num, 0]) | Input.GetKeyDown(keybindings.keys[action_num, 1]));
	}

	/// <summary>
	/// Check if either of the keys bound to the action have been released this frame.
	/// </summary>
	/// <param name="action"></param>
	/// <returns></returns>
	public static bool OnInputUp(Action action) {
		int action_num = (int)action;
		return (Input.GetKeyUp(keybindings.keys[action_num, 0]) | Input.GetKeyUp(keybindings.keys[action_num, 1]));
	}

	/*public static void WaitForKeybindInput(Action action, bool primary) {
		MonoBehaviour.StartCoroutine(WaitForKeybindInput_Coroutine(action, primary));
	}*/

	/// <summary>
	/// When called, it waits for any currently pressed buttons to be released, then gets the keyCode for the next button pressed and uses it to call ModifyKeybindingd() with the given "Action action", and "bool primary".
	/// Ends and does nothing if 5s pass with no acceptable input.
	/// </summary>
	/// <param name="action"></param>
	/// <param name="primary"></param>
	/// <returns></returns>
	static public IEnumerator<object> WaitForKeybindInput(Action action, bool primary) {
		while (Input.anyKey) {	// wait for all currently pressed keys to be released.
			//Debug.Log("Waiting for key release");
			yield return null;
		}

		yield return null;	// wait a single frame after all keys are released, so the the last key released is not accidentally read as input by the next part.

		bool loop = true;
		float time = 0.0f;
		float waitTime = 5.0f;

		while ((loop) && (time < waitTime)) {	// loop until acceptable input is found/time is up.
			//Debug.Log("Waiting for key press");
			Event e = new Event();
			while ((Event.GetEventCount() > 0) && loop) {	// if there are GUI events, pop and check them
				Event.PopEvent(e);
				//Debug.Log("Events to test");
				if (e.isKey) {  // if event was a key press, get its key code, enter it into ModifyKeybinds(), and exit loop.
								//Debug.Log("Detected key code: " + e.keyCode);
					ModifyKeybinds(action, primary, e.keyCode);
					loop = false;
				} else if (e.isMouse) { // if event was a mouse button press, use case statement to get it's keycode, enter it into ModifyKeybinds(), and exit loop.
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
			waitTime += Time.deltaTime;
			yield return null;
		}

		yield break;
	}
}

