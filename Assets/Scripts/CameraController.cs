using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {

	/*
	 Getting something together that allows dynamic camera movement and the ability to focus on some things. 
		Get it to work like stage directions, eg: look at this thing for this long, then go back to what doing before
		1 camera for scene(board), 1 camera for UI
		Smart enough to guess what zoom level to use(look-up table?)

		Camera shake		<-- later
			Random.unitcircle

		Add action Que?
	 */

	private Camera thisCamera;
	private Transform cameraTransform;

	// public Camera cameraUI;
	// public Transform cameraUITransform;

	private GameObject cameraFocus;
	private float cameraZoom;

	public Vector3 cameraOffset = new Vector3(0.0f, 0.0f, -10.0f);

	enum MoveMode {still, moving, follow};
	enum ZoomMode {still, changing};


	private MoveMode moveMode = MoveMode.still;
	private ZoomMode zoomMode = ZoomMode.still;
	private bool cameraShake = false;
	private int numMoveCoroutine = 0;
	private int numFollowCoroutine = 0;
	private int numZoomCoroutine = 0;

	public GameObject[] testObject;


	void Start () {
		thisCamera = this.GetComponent<Camera>();
		cameraTransform = this.GetComponent<Transform>();
		//set UI & scene camera heights, set to orthographic
		thisCamera.orthographic = true;
		//thisCamera.orthographicSize = FindZoom(5.0f);
		//StartCoroutine(followTest());
	}

	/* Testing
	IEnumerator<object> zoomTest() {
		yield return new WaitForSeconds(1);

		ForceZoom(2.0f);
		StartCoroutine(ChangeZoom(5.0f, 5.0f));

		yield return new WaitForSeconds(4);

		StartCoroutine(ChangeZoom(1.0f, 5.0f));

		yield return new WaitForSeconds(4);

		StartCoroutine(ChangeZoom(5.0f, 5.0f));

		yield return new WaitForSeconds(4);

		StartCoroutine(ChangeZoom(1.0f, 5.0f));

		yield break;
	}

	IEnumerator<object> moveTest() {
		ForceFocus(testObject[0]);

		StartCoroutine(MoveFocus(testObject[1], 5.0f));

		yield return new WaitForSeconds(4);

		StartCoroutine(MoveFocus(testObject[2], 5.0f));

		yield return new WaitForSeconds(4);

		StartCoroutine(MoveFocus(testObject[3], 5.0f));

		yield return new WaitForSeconds(4);

		StartCoroutine(MoveFocus(testObject[0], 5.0f));

		yield break;
	}

	IEnumerator<object> followTest() {
		yield return new WaitForSeconds(1);

		StartCoroutine(Follow(testObject[4], 5.0f));

		yield return new WaitForSeconds(7);

		StartCoroutine(Follow(testObject[4]));

		yield break;
	}
	*/

	/// <summary>
	/// Makes camera instantly look at given object.
	/// </summary>
	/// <param name="newFocus"></param>
	/// <param name="zoom"></param>
	public void ForceFocus(GameObject newFocus) {
		//Debug.Log("Forcing focus");
		moveMode = MoveMode.still;
		cameraFocus = newFocus;
		cameraTransform.position = newFocus.transform.position + cameraOffset;
	}


	/// <summary>
	/// Makes camera pan from current location to given object in given time. Time defaults to 1s.
	/// </summary>
	/// <param name="newFocus"></param>
	/// <param name="moveTime"></param>
	/// <param name="zoom"></param>
	/// <returns></returns>
	IEnumerator<object> MoveFocus(GameObject newFocus, float moveTime = 1.0f) {
		int thisCoroutine;

		if (numMoveCoroutine == 0) {
			numMoveCoroutine = 1;
			thisCoroutine = 1;
		} else {
			numMoveCoroutine++;
			thisCoroutine = numMoveCoroutine;
		}
		//Debug.Log("Starting coroutine: " + numCoroutine);

		moveMode = MoveMode.moving;

		Vector3 start = cameraTransform.position;
		cameraFocus = newFocus;
		Vector3 end = cameraFocus.transform.position + cameraOffset;

		float counter = 0.0f;
		bool loop = true;

		while (loop) {
			
			if ((counter > 1.0f) | (moveMode != MoveMode.moving)) { // if coroutine is at end location, running to long, or does not match current mode, end.
				loop = false;   // exit if: done moving, interupt
				//Debug.Log("Reached end of movement: " + thisCoroutine);
			} else if (thisCoroutine != numMoveCoroutine) {	// if this is not the newest coroutine, end
				loop = false;
				//Debug.Log("Extra coroutine: " + thisCoroutine + ", num: " + numCoroutine);
			} else {
				counter += (Time.deltaTime) / moveTime;
				cameraTransform.position = Vector3.Lerp(start, end, counter);
				yield return null; // waits until a frame has been rendered.
			}
		}
		//Debug.Log("Closing coroutine: " + thisCoroutine);

		if (thisCoroutine == numMoveCoroutine) {    // only change mode back to still if this is the newest coroutine ending
			numMoveCoroutine = 0;
			if (moveMode == MoveMode.moving) {
				moveMode = MoveMode.still;
				//Debug.Log("Last MoveFocus closed");
			}
		}

		yield break; // exit coroutine
	}

	/// <summary>
	/// Makes camera follow given object for given time, or indefinitly if given negative time. Defualts to indefinite. Indefinite can be interupted by giving camera another order.
	/// </summary>
	/// <param name="newFocus"></param>
	/// <param name="followTime"></param>
	/// <returns></returns>
	IEnumerator<object> Follow(GameObject newFocus, float followTime = -1.0f) {
		int thisCoroutine;

		if (numFollowCoroutine == 0) {
			numFollowCoroutine = 1;
			thisCoroutine = 1;
		} else {
			numFollowCoroutine++;
			thisCoroutine = numFollowCoroutine;
		}
		//Debug.Log("Starting Follow: " + thisCoroutine);

		moveMode = MoveMode.follow;

		cameraFocus = newFocus;

		bool indefinite;
		float counter = 0.0f;
		bool loop = true;
		if (followTime < 0) {
			indefinite = true;
		} else {
			indefinite = false;
		}

		while (loop) {
			if (((!indefinite) && (counter > followTime)) | (moveMode != MoveMode.follow)) { // if coroutine is at end location, running to long, or does not match current mode, end.
				loop = false;   // exit if: done moving, interupt
				//Debug.Log("Reached end of movement: " + thisCoroutine);
			} else if (thisCoroutine != numFollowCoroutine) { // if this is not the newest coroutine, end
				loop = false;
				//Debug.Log("Extra coroutine: " + thisCoroutine + ", num: " + numCoroutine);
			} else {
				counter += Time.deltaTime;
				cameraTransform.position = cameraFocus.transform.position + cameraOffset;
				yield return null; // waits until a frame has been rendered.
			}
		}
		//Debug.Log("Closing FollowCoroutine: " + thisCoroutine);

		if (thisCoroutine == numFollowCoroutine) {    // only change mode back to still if this is the newest coroutine ending
			numFollowCoroutine = 0;
			if (moveMode == MoveMode.follow) {
				moveMode = MoveMode.still;
				//Debug.Log("Last Follow closed");
			}
		}

		yield break;
	}

	/// <summary>
	/// Internal method
	/// </summary>
	/// <param name="zoom"></param>
	/// <returns></returns>
	private float FindZoom(float zoom) {
		float h = Screen.height;
		float w = Screen.width;
		if (h <= w) {
			return zoom;
		} else {
			return zoom * h / w;
		}
	}

	/// <summary>
	/// Makes camera instantly go to given zoom level
	/// </summary>
	/// <param name="newZoom"></param>
	public void ForceZoom(float newZoom) {
		//Debug.log("Forcing zoom");
		zoomMode = ZoomMode.still;
		cameraZoom = newZoom;
		thisCamera.orthographicSize = FindZoom(newZoom);
	}

	/// <summary>
	/// Makes camera zoom to given zoom level.
	/// </summary>
	/// <param name="newZoom"></param>
	/// <param name="zoomTime"></param>
	/// <returns></returns>
	IEnumerator<object> ChangeZoom(float newZoom, float zoomTime = 1.0f) {
		int thisCoroutine;

		if (numZoomCoroutine == 0) {
			numZoomCoroutine = 1;
			thisCoroutine = 1;
		} else {
			numZoomCoroutine++;
			thisCoroutine = numZoomCoroutine;
		}
		//Debug.Log("Start ChangeZoom: " + thisCoroutine);

		zoomMode = ZoomMode.changing;

		float startZoom = cameraZoom;
		float endZoom = newZoom;

		float counter = 0.0f;
		bool loop = true;

		while (loop) {

			if ((counter > 1.0f) | (zoomMode != ZoomMode.changing)) { // if coroutine is at end location, running to long, or does not match current mode, end.
				loop = false;   // exit if: done moving, interupt
			} else if (thisCoroutine != numZoomCoroutine) { // if this is not the newest coroutine, end
				loop = false;
				//Debug.Log("Extra ChangeZoom: " + thisCoroutine + ", num: " + numZoomCoroutine);
			} else {
				counter += (Time.deltaTime) / zoomTime;
				cameraZoom = Mathf.Lerp(startZoom, endZoom, counter);
				thisCamera.orthographicSize = FindZoom(cameraZoom);
				yield return null; // waits until a frame has been rendered.
			}
		}

		//Debug.Log("Closing ChangeZoom: " + thisCoroutine);
		if (thisCoroutine == numZoomCoroutine) {    // only change mode back to still if this is the newest coroutine ending
			numZoomCoroutine = 0;
			if (zoomMode == ZoomMode.changing) {
				zoomMode = ZoomMode.still;
				//Debug.Log("LastChangeZoom closed");
			}
		}

		yield break;
	}

	/*
	IEnumerator<object> LookAndReturn(GameObject tempFocus, float moveTime = 1.0f, float lookTime = 2.0f) {
		//GameObject returnFocus = focus;
		yield break;
	}*/

}
