using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeybindTest : MonoBehaviour {

	public InputManager.Action confirm = InputManager.Action.up;
	public InputManager.Action cancel = InputManager.Action.down;
	public InputManager.Action action_1 = InputManager.Action.action_1;

	// Update is called once per frame
	void Update () {

		if (InputManager.instance.OnInputDown(confirm)) {
			Debug.Log("Set/Save keybind");
			/*
			InputManager.ModifyKeybinds(InputManager.Action.up, true, KeyCode.Keypad8);
			InputManager.ModifyKeybinds(InputManager.Action.down, true, KeyCode.Keypad5);
			InputManager.ModifyKeybinds(InputManager.Action.left, true, KeyCode.Keypad4);
			InputManager.ModifyKeybinds(InputManager.Action.right, true, KeyCode.Keypad6);
			*/
			InputManager.instance.ApplyKeybinds();
		}

		if (InputManager.instance.OnInputDown(cancel)) {
			Debug.Log("Reset keybind");
			InputManager.instance.ResetKeybinds();

			InputManager.instance.ApplyKeybinds();
		}

		if (InputManager.instance.OnInputDown(action_1)) {
			Debug.Log("Starting WaitForKeybindInput Coroutine");
			StartCoroutine(InputManager.instance.WaitForKeybindInput(InputManager.Action.up, false));
		}
	}
}
