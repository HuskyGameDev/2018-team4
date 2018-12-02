using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputTest : MonoBehaviour {

	public InputManager.Action left = InputManager.Action.left;
	public InputManager.Action right = InputManager.Action.right;
	public InputManager.Action up = InputManager.Action.up;
	public InputManager.Action down = InputManager.Action.down;

	public GameObject target = null;
	private Vector3 pos;
	public float speed = 1.0f;

	void Start() {
		if (target == null) {
			target = this.gameObject;
		}
		//pos = target.transform.position;
	}

	// Update is called once per frame
	void Update () {
		pos = target.transform.position;
		if (InputManager.OnInput(left) && !InputManager.OnInput(right)) {
			pos.x += -speed * Time.deltaTime;
		} else if (!InputManager.OnInput(left) && InputManager.OnInput(right)) {
			pos.x += speed * Time.deltaTime;
		}

		if (InputManager.OnInput(down) && !InputManager.OnInput(up)) {
			pos.y += -speed * Time.deltaTime;
		} else if (!InputManager.OnInput(down) && InputManager.OnInput(up)) {
			pos.y += speed * Time.deltaTime;
		}
		target.transform.position = pos;
	}
}
