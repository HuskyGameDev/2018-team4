using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class InputManager {
	/*TODO: 
	 *		[NO] mouse position/click stuff
	 *		[X] Make into a singleton rather than static
	 * 
	 *		[X] joystick - "Axis_1h" & "Axis_1v"
	 *		[_]		button-up/down
	 *		[_]		diagonal-directions
	 *				Should input check every frame happen in the input manager? Or should input be checked every frame by things looking for input?
	 *		[_]		check happens in input-manager, sends notifications when receive input
	 *		
	 *		[?]Make sure buttonDown/Up does not trigger twice from both buttons being pressed if the first button is down when the second if pressed
	*/

	public enum Action { up, down, left, right, confirm, cancel, action_1, action_2, action_3 };    // what actions/inputs we will has keybindings for
	private Keybindings keybindings = null;  // keybindings for manager to use when checking button presses
	private Keybindings temp_keybindings = null; // potential changes to keybindings, can then be applied or discarded
	private System.Runtime.Serialization.IFormatter formatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();    // used for saving/loading keybindings from file

	// Used for deciding if joystick input was "pressed" or "released" this frame
	private bool[] joystickLastState = new bool[4];	// indicates if joystick input was considered pressed/not pressed last frame, all false when created
	private int[] joystickStateCount = new int[4];  // count of frames that joystick has been sending input, all 0 when created
	private bool[] joystickStateChange = new bool[4];	// indicates that joystick input has changes state this frame
	private const int JOYSTICKTIME = 4;    // number of frames that joystick must give/not give input in order to change state

	/*	Key notifications take following form: "inputType_inputKey"
	 * where "inputType" is one of these three options:
	 *		"Key"		: key is currently pressed down
	 *		"KeyDown"	: key was pressed down this frame
	 *		"KeyUp"		: key was released this frame
	 * 
	 * and "inputKey" is one of the following options:
	 *		"UpRight"	: direction up & right
	 *		"Up"		: direction up 
	 *		"UpLeft"	: direction up & left
	 *		"Right"		: direction right
	 *		"Left"		: direction left
	 *		"DownRight"	: direction down & right
	 *		"Down"		: direction down
	 *		"DownLeft"	: direction down & left
	 *		"Confirm"	: confirm
	 *		"Cancel"	: cancel
	 *		"Action_1"	: action #1
	 *		"Action_2"	: action #2
	 *		"Action_3"	: action #3
	 *		
	 *	Note that diagonal directions (UpRight, UpLeft, DownRight, DownLeft) only have the "Key" inputType, not "KeyUp" or "KeyDown"
	 *		
	 * Example #1 "KeyDown_Confirm" : confirm key was pressed down this frame
	 * Example #2 "KeyUp_Down" : Down direction key was released this frame
	 */

	#region Singleton Pattern
	public readonly static InputManager instance = new InputManager();	// should only be one instance of the camera manager
	private InputManager() {}	// Constuctor w/ nothing inside
	#endregion

	#region Keybinding Methods
	/// <summary>
	/// Holds keybindings
	/// </summary>
	[System.Serializable]   // this class can be saved/loaded from file
	private class Keybindings {
		public KeyCode[,] keys; // 9 tall, for 9 actions, and 2 wide, for 2 keybindings per action

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
	public void LoadKeybinds() {
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
	public void ResetKeybinds() {
		temp_keybindings = new Keybindings();
	}

	/// <summary>
	/// Sets keybindings to match temp_keybindings, and saves to file as well.
	/// </summary>
	public void ApplyKeybinds() {
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
	public void DiscardKeybinds() {
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
	public void ModifyKeybinds(Action action, bool primary, KeyCode newKey) {
		//Debug.Log("ModifyKeybind: " + action + ", primary:" + primary + ", to: " + newKey);
		int action_num = (int)action;
		int action_primary = 1;
		if (primary) action_primary = 0;

		int replace_num = -1;
		int replace_primary = -1;
		for (int i = 0; i < 9; i++) {   // find if the newKey matches any already in use.
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

		if (replace_num >= 0) { // swap newKey and replaced key is newKey was already in use.
			temp_keybindings.keys[replace_num, replace_primary] = temp_keybindings.keys[action_num, action_primary];
		}
		temp_keybindings.keys[action_num, action_primary] = newKey;
	}

	/// <summary>
	/// When called, it waits for any currently pressed buttons to be released, then gets the keyCode for the next button pressed and uses it to call ModifyKeybindingd() with the given "Action action", and "bool primary".
	/// Ends and does nothing if 5s pass with no acceptable input.
	/// </summary>
	/// <param name="action"></param>
	/// <param name="primary"></param>
	/// <returns></returns>
	public IEnumerator<object> WaitForKeybindInput(Action action, bool primary) {
		while (Input.anyKey) {  // wait for all currently pressed keys to be released.
								//Debug.Log("Waiting for key release");
			yield return null;
		}

		yield return null;  // wait a single frame after all keys are released, so the the last key released is not accidentally read as input by the next part.

		bool loop = true;
		float time = 0.0f;
		float waitTime = 5.0f;

		while ((loop) && (time < waitTime)) {   // loop until acceptable input is found/time is up.
												//Debug.Log("Waiting for key press");
			Event e = new Event();
			while ((Event.GetEventCount() > 0) && loop) {   // if there are GUI events, pop and check them
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

	#endregion

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
	/// Every frame, check for input and send any notifications
	/// </summary>
	void Update() {
		#region Joystick Data Processing
		joystickStateChange[0] = false;	// any joystick state changes (KeyDown()/KeyUp()) should only last a single frame
		joystickStateChange[1] = false;
		joystickStateChange[2] = false;
		joystickStateChange[3] = false;

		// Check the state of the "up" direction of joystick as compared to last frame
		if (Input.GetAxis("Axis_1v") < -0.5f) {	// up
			if (joystickLastState[0]) { // joystick is up, & was up
				joystickStateCount[0] = 0; // reset count
			} else {    // joystick is up, & was not up
				joystickStateCount[0] += 1; // increase count, check if greater than JOYSTICKTIME, possibly switch lastState
				if (joystickStateCount[0] > JOYSTICKTIME) {
					joystickStateChange[0] = true;
					joystickLastState[0] = true;
					joystickStateCount[0] = 0;	// reset count
				}
			}
		} else {
			if (joystickLastState[0]) { // joystick is not up, & was up
				joystickStateCount[0] += 1; // increase count, check if greater than JOYSTICKTIME, possibly switch lastState
				if (joystickStateCount[0] > JOYSTICKTIME) {
					joystickStateChange[0] = true;
					joystickLastState[0] = false;
					joystickStateCount[0] = 0;  // reset count
				}
			} else {    // joystick is not up, & was not up
				joystickStateCount[0] = 0; // reset count
			}
		}

		// Check the state of the "down" direction of joystick as compared to last frame
		if (Input.GetAxis("Axis_1v") > 0.5f) { // down
			if (joystickLastState[1]) { // joystick is up, & was up
				joystickStateCount[1] = 0; // reset count
			} else {    // joystick is up, & was not up
				joystickStateCount[1] += 1; // increase count, check if greater than JOYSTICKTIME, possibly switch lastState
				if (joystickStateCount[1] > JOYSTICKTIME) {
					joystickStateChange[1] = true;
					joystickLastState[1] = true;
					joystickStateCount[1] = 0;  // reset count
				}
			}
		} else {
			if (joystickLastState[1]) { // joystick is not up, & was up
				joystickStateCount[1] += 1; // increase count, check if greater than JOYSTICKTIME, possibly switch lastState
				if (joystickStateCount[1] > JOYSTICKTIME) {
					joystickStateChange[1] = true;
					joystickLastState[1] = false;
					joystickStateCount[1] = 0;  // reset count
				}
			} else {    // joystick is not up, & was not up
				joystickStateCount[1] = 0; // reset count
			}
		}

		// Check the state of the "left" direction of joystick as compared to last frame
		if (Input.GetAxis("Axis_1h") > 0.5f) { // left
			if (joystickLastState[2]) { // joystick is up, & was up
				joystickStateCount[2] = 0; // reset count
			} else {    // joystick is up, & was not up
				joystickStateCount[2] += 1; // increase count, check if greater than JOYSTICKTIME, possibly switch lastState
				if (joystickStateCount[2] > JOYSTICKTIME) {
					joystickStateChange[2] = true;
					joystickLastState[2] = true;
					joystickStateCount[2] = 0;  // reset count
				}
			}
		} else {
			if (joystickLastState[2]) { // joystick is not up, & was up
				joystickStateCount[2] += 1; // increase count, check if greater than JOYSTICKTIME, possibly switch lastState
				if (joystickStateCount[2] > JOYSTICKTIME) {
					joystickStateChange[2] = true;
					joystickLastState[2] = false;
					joystickStateCount[2] = 0;  // reset count
				}
			} else {    // joystick is not up, & was not up
				joystickStateCount[2] = 0; // reset count
			}
		}

		// Check the state of the "right" direction of joystick as compared to last frame
		if (Input.GetAxis("Axis_1h") < -0.5f) { // right
			if (joystickLastState[3]) { // joystick is up, & was up
				joystickStateCount[3] = 0; // reset count
			} else {    // joystick is up, & was not up
				joystickStateCount[3] += 1; // increase count, check if greater than JOYSTICKTIME, possibly switch lastState
				if (joystickStateCount[3] > JOYSTICKTIME) {
					joystickStateChange[3] = true;
					joystickLastState[3] = true;
					joystickStateCount[3] = 0;  // reset count
				}
			}
		} else {
			if (joystickLastState[3]) { // joystick is not up, & was up
				joystickStateCount[3] += 1; // increase count, check if greater than JOYSTICKTIME, possibly switch lastState
				if (joystickStateCount[3] > JOYSTICKTIME) {
					joystickStateChange[3] = true;
					joystickLastState[3] = false;
					joystickStateCount[3] = 0;  // reset count
				}
			} else {    // joystick is not up, & was not up
				joystickStateCount[3] = 0; // reset count
			}
		}

		#endregion

		#region OnKey Input
		// Send notification if a key is currently pressed down

		// probably a dumb way of doing this. YOU HEAR ME? THIS IS ALMOST CERTAINLY STUPID
		int direction = 
			  ((Input.GetKey(keybindings.keys[0, 0]) | Input.GetKey(keybindings.keys[0, 1]) | joystickLastState[0]) ? 10 : 0) 
			+ ((Input.GetKey(keybindings.keys[1, 0]) | Input.GetKey(keybindings.keys[1, 1]) | joystickLastState[1]) ? -10 : 0) 
			+ ((Input.GetKey(keybindings.keys[2, 0]) | Input.GetKey(keybindings.keys[2, 1]) | joystickLastState[2]) ? 1 : 0) 
			+ ((Input.GetKey(keybindings.keys[3, 0]) | Input.GetKey(keybindings.keys[3, 1]) | joystickLastState[3]) ? -1 : 0);

		switch (direction) {
			case 11:    // up & right
				NotificationCenter.instance.PostNotification("Key_UpRight");
				break;	
			case 10:    // up
				NotificationCenter.instance.PostNotification("Key_Up");
				break;
			case 9: // up & left
				NotificationCenter.instance.PostNotification("Key_UpLeft");
				break;
			case 1: // right
				NotificationCenter.instance.PostNotification("Key_Right");
				break;
			case -1:    // left
				NotificationCenter.instance.PostNotification("Key_Left");
				break;
			case -9:    // down & right
				NotificationCenter.instance.PostNotification("Key_DownRight");
				break;
			case -10:   // down
				NotificationCenter.instance.PostNotification("Key_Down");
				break;
			case -11:   // down & left
				NotificationCenter.instance.PostNotification("Key_DownLeft");
				break;
		}

		// confirm - 4
		if (Input.GetKey(keybindings.keys[4, 0]) | Input.GetKey(keybindings.keys[4, 1])) {
			NotificationCenter.instance.PostNotification("Key_Confirm");
		}

		// cancel - 5
		if (Input.GetKey(keybindings.keys[5, 0]) | Input.GetKey(keybindings.keys[5, 1])) {
			NotificationCenter.instance.PostNotification("Key_Cancel");
		}

		// action_1 - 6
		if (Input.GetKey(keybindings.keys[6, 0]) | Input.GetKey(keybindings.keys[6, 1])) {
			NotificationCenter.instance.PostNotification("Key_Action_1");
		}

		// action_2 - 7
		if (Input.GetKey(keybindings.keys[7, 0]) | Input.GetKey(keybindings.keys[7, 1])) {
			NotificationCenter.instance.PostNotification("Key_Action_2");
		}

		// action_3 - 8
		if (Input.GetKey(keybindings.keys[8, 0]) | Input.GetKey(keybindings.keys[8, 1])) {
			NotificationCenter.instance.PostNotification("Key_Action_3");
		}
		#endregion


		#region OnKeyDown Input
		// Send notification if a key was pressed down this frame

		// up - 0
		if (Input.GetKeyDown(keybindings.keys[0, 0]) | Input.GetKeyDown(keybindings.keys[0, 1]) | (joystickStateChange[0] & joystickLastState[0])) {
			NotificationCenter.instance.PostNotification("KeyDown_Up");
		}
		// down - 1
		if (Input.GetKeyDown(keybindings.keys[1, 0]) | Input.GetKeyDown(keybindings.keys[1, 1]) | (joystickStateChange[1] & joystickLastState[1])) {
			NotificationCenter.instance.PostNotification("KeyDown_Down");
		}
		// left - 2
		if (Input.GetKeyDown(keybindings.keys[2, 0]) | Input.GetKeyDown(keybindings.keys[2, 1]) | (joystickStateChange[2] & joystickLastState[2])) {
			NotificationCenter.instance.PostNotification("KeyDown_Left");
		}
		// right - 3
		if (Input.GetKeyDown(keybindings.keys[3, 0]) | Input.GetKeyDown(keybindings.keys[3, 1]) | (joystickStateChange[3] & joystickLastState[3])) {
			NotificationCenter.instance.PostNotification("KeyDown_Right");
		}
		// confirm - 4
		if (Input.GetKeyDown(keybindings.keys[4, 0]) | Input.GetKeyDown(keybindings.keys[4, 1])) {
			NotificationCenter.instance.PostNotification("KeyDown_Confirm");
		}
		// cancel - 5
		if (Input.GetKeyDown(keybindings.keys[5, 0]) | Input.GetKeyDown(keybindings.keys[5, 1])) {
			NotificationCenter.instance.PostNotification("KeyDown_Cancel");
		}
		// action_1 - 6
		if (Input.GetKeyDown(keybindings.keys[6, 0]) | Input.GetKeyDown(keybindings.keys[6, 1])) {
			NotificationCenter.instance.PostNotification("KeyDown_Action_1");
		}
		// action_2 - 7
		if (Input.GetKeyDown(keybindings.keys[7, 0]) | Input.GetKeyDown(keybindings.keys[7, 1])) {
			NotificationCenter.instance.PostNotification("KeyDown_Action_2");
		}
		// action_3 - 8
		if (Input.GetKeyDown(keybindings.keys[8, 0]) | Input.GetKeyDown(keybindings.keys[8, 1])) {
			NotificationCenter.instance.PostNotification("KeyDown_Action_3");
		}
		#endregion


		#region OnKeyUp Input
		// Send notification if a key was released this frame

		// up - 0
		if (Input.GetKeyUp(keybindings.keys[0, 0]) | Input.GetKeyUp(keybindings.keys[0, 1]) | (joystickStateChange[0] & !joystickLastState[0])) {
			NotificationCenter.instance.PostNotification("KeyDown_Up");
		}
		// down - 1
		if (Input.GetKeyUp(keybindings.keys[1, 0]) | Input.GetKeyUp(keybindings.keys[1, 1]) | (joystickStateChange[1] & !joystickLastState[1])) {
			NotificationCenter.instance.PostNotification("KeyDown_Down");
		}
		// left - 2
		if (Input.GetKeyUp(keybindings.keys[2, 0]) | Input.GetKeyUp(keybindings.keys[2, 1]) | (joystickStateChange[2] & !joystickLastState[2])) {
			NotificationCenter.instance.PostNotification("KeyDown_Left");
		}
		// right - 3
		if (Input.GetKeyUp(keybindings.keys[3, 0]) | Input.GetKeyUp(keybindings.keys[3, 1]) | (joystickStateChange[3] & !joystickLastState[3])) {
			NotificationCenter.instance.PostNotification("KeyDown_Right");
		}
		// confirm - 4
		if (Input.GetKeyUp(keybindings.keys[4, 0]) | Input.GetKeyUp(keybindings.keys[4, 1])) {
			NotificationCenter.instance.PostNotification("KeyDown_Confirm");
		}
		// cancel - 5
		if (Input.GetKeyUp(keybindings.keys[5, 0]) | Input.GetKeyUp(keybindings.keys[5, 1])) {
			NotificationCenter.instance.PostNotification("KeyDown_Cancel");
		}
		// action_1 - 6
		if (Input.GetKeyUp(keybindings.keys[6, 0]) | Input.GetKeyUp(keybindings.keys[6, 1])) {
			NotificationCenter.instance.PostNotification("KeyDown_Action_1");
		}
		// action_2 - 7
		if (Input.GetKeyUp(keybindings.keys[7, 0]) | Input.GetKeyUp(keybindings.keys[7, 1])) {
			NotificationCenter.instance.PostNotification("KeyDown_Action_2");
		}
		// action_3 - 8
		if (Input.GetKeyUp(keybindings.keys[8, 0]) | Input.GetKeyUp(keybindings.keys[8, 1])) {
			NotificationCenter.instance.PostNotification("KeyDown_Action_3");
		}
		#endregion
	}


	#region OLD - Direct Key Check
	
	/// <summary>
	/// Check if either of the keys bound to the action are pressed down.
	/// </summary>
	/// <param name="action"></param>
	/// <returns></returns>
	public bool OnInput(Action action) {
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
	public bool OnInputDown(Action action) {
		int action_num = (int)action;
		return (Input.GetKeyDown(keybindings.keys[action_num, 0]) | Input.GetKeyDown(keybindings.keys[action_num, 1]));
	}

	/// <summary>
	/// Check if either of the keys bound to the action have been released this frame.
	/// </summary>
	/// <param name="action"></param>
	/// <returns></returns>
	public bool OnInputUp(Action action) {
		int action_num = (int)action;
		return (Input.GetKeyUp(keybindings.keys[action_num, 0]) | Input.GetKeyUp(keybindings.keys[action_num, 1]));
	}
	#endregion

	/*public static void WaitForKeybindInput(Action action, bool primary) {
		MonoBehaviour.StartCoroutine(WaitForKeybindInput_Coroutine(action, primary));
	}*/

}

