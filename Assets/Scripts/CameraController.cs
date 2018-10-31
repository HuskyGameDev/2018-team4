using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {

	/*
	 * Add: look at for x time,
	 * Add: pan/zoom acceleration
	 * 
		Camera shake		<-- later
			Random.unitcircle
	 */

	private Camera thisCamera;
	private Transform cameraTransform;

	// public Camera cameraUI;
	// public Transform cameraUITransform;

	private GameObject cameraFocus;
	private float cameraZoom;

	public Vector3 cameraOffset = new Vector3(0.0f, 0.0f, -10.0f);

	public enum MoveMode {still, moving, follow};
	public enum ZoomMode {still, changing};
	public enum QueType {wait, forceFocus, moveFocus, followFocus, forceZoom, changeZoom};

	//private Queue<QueCard> queue;

	public class QueCard {
		public CameraController.QueType type;
		public GameObject _object;	// target object
		public float _float1;	// time, if applicable
		public float _float2;	// zoom, if applicable

		public QueCard(CameraController.QueType type, GameObject _object, float _float1, float _float2) {
			this.type = type;
			this._object = _object;
			this._float1 = _float1;
			this._float2 = _float2;
		}
	}

	public class CameraQueue {
		private Queue<QueCard> queue;

		public CameraQueue () {
			queue = new Queue<QueCard>();
		}

		/*
		private void Enqueue(CameraController.QueType type, GameObject _object, float _float1, float _float2) {
			queue.Enqueue(new QueCard(type, _object, _float1, _float2));
		}*/

		public QueCard Dequeue() {
			return queue.Dequeue();
		}

		public bool Empty() {
			return (queue.Count == 0);
		}

		//wait, lookForce, lookMove, lookFollow, zoomForce, zoomChange
		//float1: time
		//float2: zoom

		public void AddWait(float waitTime) {
			queue.Enqueue(new QueCard(QueType.wait, null, waitTime, 0.0f));
			//queue.Enqueue(new QueCard(type, _object, _float1, _float2));
			//queue.Enqueue(new QueCard(QueType., null, 0.0f, 0.0f));
		}

		public void AddForceFocus(GameObject newFocus) {
			queue.Enqueue(new QueCard(QueType.forceFocus, newFocus, 0.0f, 0.0f));
		}

		public void AddMoveFocus(GameObject newFocus, float moveTime) {
			queue.Enqueue(new QueCard(QueType.moveFocus, newFocus, moveTime, 0.0f));
		}

		public void AddFollowFocus(GameObject newFocus, float followTime) {
			queue.Enqueue(new QueCard(QueType.followFocus, newFocus, followTime, 0.0f));
		}

		public void AddForceZoom(float newZoom) {
			queue.Enqueue(new QueCard(QueType.forceZoom, null, 0.0f, newZoom));
		}

		public void AddChangeZoom(float newZoom, float zoomTime) {
			queue.Enqueue(new QueCard(QueType.changeZoom, null, zoomTime, newZoom));
		}
	}

	private MoveMode moveMode = MoveMode.still;
	private ZoomMode zoomMode = ZoomMode.still;
	private bool sequenceMode = false;
	//private bool cameraShake = false;
	private int numMoveCoroutine = 0;
	private int numFollowCoroutine = 0;
	private int numZoomCoroutine = 0;
	private int numCamSequence = 0;

	public GameObject[] testObject;


	void Start () {
		thisCamera = this.GetComponent<Camera>();
		cameraTransform = this.GetComponent<Transform>();
		//set UI & scene camera heights, set to orthographic
		thisCamera.orthographic = true;
		//thisCamera.orthographicSize = FindZoom(5.0f);
		//sequenceTest();
		StartCoroutine(TestSequence());
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

		IEnumerator<object> TestSequence() {
		sequenceTest();

		yield return new WaitForSeconds(4.0f);

		Debug.Log("Testing sequence interupt");
		StartCoroutine(MoveFocus(testObject[3], 2.0f, false));

		yield break;
	}

	public void sequenceTest() {
		CameraQueue testQueue1 = new CameraQueue();
		testQueue1.AddForceZoom(6.0f);
		/*testQueue1.AddChangeZoom(2.0f, 5.0f);
		testQueue1.AddWait(5.0f);*/

		testQueue1.AddChangeZoom(2.0f, 8.0f);
		testQueue1.AddForceFocus(testObject[0]);
		testQueue1.AddMoveFocus(testObject[1], 8.0f);
		testQueue1.AddWait(8.0f);

		/*
		testQueue1.AddMoveFocus(testObject[3], 5.0f);
		testQueue1.AddChangeZoom(2.0f, 5.0f);
		testQueue1.AddWait(2.5f);
		testQueue1.AddMoveFocus(testObject[2], 2.5f);
		testQueue1.AddWait(2.5f);*/

		/*
		testQueue1.AddFollowFocus(testObject[4], 4.0f);
		for (int i = 0; i < 4; i++) {
			testQueue1.AddForceZoom(5.0f);
			testQueue1.AddWait(0.5f);
			testQueue1.AddForceZoom(4.5f);
			testQueue1.AddWait(0.5f);
		}*/
		
		StartCoroutine(CameraSequence(testQueue1));
	}
	
	/// <summary>
	/// Makes camera instantly look at given object.
	/// </summary>
	/// <param name="newFocus"></param>
	/// <param name="zoom"></param>
	public void ForceFocus(GameObject newFocus, bool sequence = false) {
		//Debug.Log("Forcing focus");
		moveMode = MoveMode.still;
		if (!sequence && sequenceMode) {
			Stop();
			sequenceMode = false;
		}
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
	IEnumerator<object> MoveFocus(GameObject newFocus, float moveTime = 1.0f, bool sequence = false) {

		int thisCoroutine;

		if (numMoveCoroutine == 0) {
			numMoveCoroutine = 1;
			thisCoroutine = 1;
		} else {
			numMoveCoroutine++;
			thisCoroutine = numMoveCoroutine;
		}

		//Debug.Log("Starting MoveFocus: " + thisCoroutine);
		if (!sequence && sequenceMode) {
			Stop();
			sequenceMode = false;
		}

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
	IEnumerator<object> FollowFocus(GameObject newFocus, float followTime = -1.0f, bool sequence = false) {
		int thisCoroutine;

		if (numFollowCoroutine == 0) {
			numFollowCoroutine = 1;
			thisCoroutine = 1;
		} else {
			numFollowCoroutine++;
			thisCoroutine = numFollowCoroutine;
		}
		if (!sequence && sequenceMode) {
			Stop();
			sequenceMode = false;
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
	public void ForceZoom(float newZoom, bool sequence = false) {
		//Debug.log("Forcing zoom");
		zoomMode = ZoomMode.still;
		cameraZoom = newZoom;
		if (!sequence && sequenceMode) {
			Stop();
			sequenceMode = false;
		}
		thisCamera.orthographicSize = FindZoom(newZoom);
	}

	/// <summary>
	/// Makes camera zoom to given zoom level.
	/// </summary>
	/// <param name="newZoom"></param>
	/// <param name="zoomTime"></param>
	/// <returns></returns>
	IEnumerator<object> ChangeZoom(float newZoom, float zoomTime = 1.0f, bool sequence = false) {
		int thisCoroutine;

		if (numZoomCoroutine == 0) {
			numZoomCoroutine = 1;
			thisCoroutine = 1;
		} else {
			numZoomCoroutine++;
			thisCoroutine = numZoomCoroutine;
		}
		if (!sequence && sequenceMode) {
			Stop();
			sequenceMode = false;
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

	public void Stop() {
		moveMode = MoveMode.still;
		zoomMode = ZoomMode.still;
	}

	IEnumerator<object> CameraSequence(CameraQueue queue) {
		int thisCoroutine;

		if (numCamSequence == 0) {
			numCamSequence = 1;
			thisCoroutine = 1;
		} else {
			numCamSequence++;
			thisCoroutine = numCamSequence;
		}

		Debug.Log("Starting sequence: " + thisCoroutine);

		Stop();	// stops any existing stuff that might mess up the sequence

		sequenceMode = true;
		bool loop = true;

		while (loop && !queue.Empty()) {
			if (!sequenceMode) {
				loop = false;   // exit if: done moving, interupt
				Debug.Log("Not sequence mode, exiting loop");
			} else if (thisCoroutine != numCamSequence) { // if this is not the newest coroutine, end
				loop = false;
				Debug.Log("Not newest coroutine, exiting loop");
			} else {
				QueCard card = queue.Dequeue();
				switch(card.type) {
					case QueType.wait:
						yield return new WaitForSeconds(card._float1);
						Debug.Log("Wait");
						break;
					case QueType.forceFocus:
						ForceFocus(card._object, true);
						Debug.Log("ForceFocus");
						yield return null;
						break;
					case QueType.moveFocus:
						StartCoroutine(MoveFocus(card._object, card._float1, true));
						Debug.Log("MoveFocus");
						break;
					case QueType.followFocus:
						StartCoroutine(FollowFocus(card._object, card._float1, true));
						Debug.Log("FollowFocus");
						break;
					case QueType.forceZoom:
						ForceZoom(card._float2, true);
						Debug.Log("ForceZoom");
						yield return null;
						break;
					case QueType.changeZoom:
						StartCoroutine(ChangeZoom(card._float2, card._float1, true));
						Debug.Log("ChangeZoom");
						break;
					default:
						yield return null;
						break;
				}
			}
		}
		Debug.Log("Ending sequence: " + thisCoroutine);

		if (thisCoroutine == numCamSequence) {    // only change mode back to still if this is the newest coroutine ending
			numCamSequence = 0;
			if (sequenceMode == true) {
				sequenceMode = false;
				Debug.Log("Ending last sequence: " + thisCoroutine);
				//Debug.Log("Last MoveFocus closed");
			}
		}

		yield break;
	}
}
