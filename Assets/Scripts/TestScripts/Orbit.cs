using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Orbit : MonoBehaviour {
	public float height = 1.0f;
	public Vector3 center = new Vector3(0.0f, 0.0f, 0.0f);
	public float orbitTime = 1.0f;
	private float counter = 0.0f;

	public GameObject target = null;
	private Vector3 pos;

	void Start() {
		if (target == null) {
			target = this.gameObject;
		}
		//pos = target.transform.position;
	}


	// Update is called once per frame
	void Update () {
		counter += (Time.deltaTime) / orbitTime;
		if (counter >= 1.0f) counter -= 1.0f;
		pos.Set(0.0f, 0.0f, 0.0f);
		pos += center;
		pos.x += Mathf.Cos(2 * Mathf.PI * counter);
		pos.y += Mathf.Sin(2 * Mathf.PI * counter);
		target.transform.position = pos;
	}
}
