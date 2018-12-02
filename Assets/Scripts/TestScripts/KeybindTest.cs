using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeybindTest : MonoBehaviour {

	public InputManager.Action confirm_key = InputManager.Action.confirm;

	// Update is called once per frame
	void Update () {
		if (InputManager.OnInputDown(confirm_key)) {

		}

	}
}
