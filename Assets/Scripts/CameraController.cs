using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {

	/*
	 Getting something together that allows dynamic camera movement and the ability to focus on some things. 
		Get it to work like stage directions, eg: look at this thing for this long, then go back to what doing before
		1 camera for scene(board), 1 camera for UI
		Switch focus to this thing, 
		Smart enough to guess what zoom level to use(look-up table?)
		Camera shake
			Random.unitcircle
	 */

	private Camera thisCamera;
	private Transform cameraTransform;

	// public Camera cameraUI;
	// public Transform cameraUITransform;

	private GameObject focus;
	public Vector3 cameraOffset = new Vector3(0.0f, 0.0f, -10.0f);
	//public Vector3 position;

	enum CameraMode {still, moving, follow};
	private CameraMode mode = CameraMode.still;
	private bool closeExtraCoroutines = false;

	public GameObject[] testObject;

	void Start () {
		thisCamera = this.GetComponent<Camera>();
		cameraTransform = this.GetComponent<Transform>();
		//set UI & scene camera heights, set to orthographic
		//thisCamera.orthographicSize = FindZoom(5.0f);
		StartCoroutine(test());
	}

	IEnumerator<object> test() {
		yield return new WaitForSeconds(1);

		ForceFocus(testObject[0]);

		yield return new WaitForSeconds(1);

		StartCoroutine(MoveFocus(testObject[1], 2.0f, 1.0f));

		yield return new WaitForSeconds(2);

		StartCoroutine(MoveFocus(testObject[2], 2.0f, 1.0f));

		yield return new WaitForSeconds(1);

		StartCoroutine(MoveFocus(testObject[3], 2.0f, 1.0f));

		yield return new WaitForSeconds(1);

		ForceFocus(testObject[0]);

		yield break;
	}

	private float FindZoom(float zoom) {
		float h = Screen.height;
		float w = Screen.width;
		if (h <= w) {
			return zoom;
		} else {
			return zoom*h/w;
		}
	}

	public void ForceFocus(GameObject newFocus, float zoom = 5.0f) {
		Debug.Log("Forcing focus");
		mode = CameraMode.still;
		focus = newFocus;
		cameraTransform.position = focus.transform.position + cameraOffset;
		thisCamera.orthographicSize = FindZoom(zoom);
	}


	// needs at least 1 yield statement
	IEnumerator<object> MoveFocus(GameObject newFocus, float moveTime = 1.0f, float zoom = 5.0f) {
		//TODO: check if camera is already in moving mode
		if (mode == CameraMode.moving) {
			closeExtraCoroutines = true;

		} else {
			mode = CameraMode.moving;

		}
		


		Vector3 start = cameraTransform.position;
		float startSize = thisCamera.orthographicSize;

		focus = newFocus;
		Vector3 end = focus.transform.position + cameraOffset;
		float endSize = FindZoom(zoom);

		float counter = 0.0f;

		bool loop = true;

		while (loop) {
			
			if ((cameraTransform.position == end) | (mode != CameraMode.moving)) {
				loop = false;   // exit if: done moving, interupt
			} else {
				counter += (Time.deltaTime) / moveTime;
				cameraTransform.position = Vector3.Lerp(start, end, counter);
				thisCamera.orthographicSize = Mathf.Lerp(startSize, endSize, counter);
				yield return null; // waits until a frame has been rendered.
			}
		}

		mode = CameraMode.still;
		yield break; // exit coroutine
		//yield return new WaitForEndOfFrame(); //
	}

	IEnumerator<object> FollowFocus(GameObject newFocus, float time = -1.0f) {
		yield break;
	}

	IEnumerator<object> LookAndReturn(GameObject tempFocus, float moveTime = 1.0f, float lookTime = 2.0f, float zoom = 5.0f) {
		//GameObject returnFocus = focus;
		yield break;
	}

}
