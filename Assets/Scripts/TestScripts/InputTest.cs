using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputTest : MonoBehaviour {
	/*
	public InputManager.Action left = InputManager.Action.left;
	public InputManager.Action right = InputManager.Action.right;
	public InputManager.Action up = InputManager.Action.up;
	public InputManager.Action down = InputManager.Action.down;
	*/

	public GameObject target = null;
	private Vector3 pos;
	public float speed = 1.0f;

	void Start() {
		if (target == null) {
			target = this.gameObject;
		}
		pos = target.transform.position;

		//MoveInput m1 = new MoveInput(this.MoveUp);
		this.AddObserver(MoveUp, "Key_Up");
		this.AddObserver(MoveDown, "Key_Down");
		this.AddObserver(MoveLeft, "Key_Left");
		this.AddObserver(MoveRight, "Key_Right");
	}

	//public delegate void MoveInput(object sender, object args);

	public void MoveUp(object sender, object args) {
		pos.y += speed * Time.deltaTime;
		target.transform.position = pos;
		//Debug.Log("Received up input, moving");
	}
	
	public void MoveDown(object sender, object args) {
		pos.y -= speed * Time.deltaTime;
		target.transform.position = pos;
		//Debug.Log("Received down input, moving");
	}

	public void MoveLeft(object sender, object args) {
		pos.x -= speed * Time.deltaTime;
		target.transform.position = pos;
		//Debug.Log("Received left input, moving");
	}

	public void MoveRight(object sender, object args) {
		pos.x += speed * Time.deltaTime;
		target.transform.position = pos;
		//Debug.Log("Received right input, moving");
	}
	

	/*
	// Update is called once per frame
	void Update () {
		pos = target.transform.position;
		if (InputManager.instance.OnInput(left) && !InputManager.instance.OnInput(right)) {
			pos.x += -speed * Time.deltaTime;
		} else if (!InputManager.instance.OnInput(left) && InputManager.instance.OnInput(right)) {
			pos.x += speed * Time.deltaTime;
		}

		if (InputManager.instance.OnInput(down) && !InputManager.instance.OnInput(up)) {
			pos.y += -speed * Time.deltaTime;
		} else if (!InputManager.instance.OnInput(down) && InputManager.instance.OnInput(up)) {
			pos.y += speed * Time.deltaTime;
		}
		target.transform.position = pos;
	}
	*/
}
