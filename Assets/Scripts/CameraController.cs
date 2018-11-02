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

	#region Values
	private Camera thisCamera;	// used for easy changing of zoom
	private Transform cameraTransform;	// used for easy changing of camera location

	// public Camera cameraUI;	// will need some sort of second camera for UI/mini-map
	// public Transform cameraUITransform;

	private GameObject cameraFocus;	// object camera is looking at currently/last
	private float cameraZoom;	// current zoom level

	public Vector3 cameraOffset = new Vector3(0.0f, 0.0f, -10.0f);	// z-axis offset of 10 units should be fine.

	public enum MoveMode {still, moving, follow};	// used to tell if running coroutines should exit
	public enum ZoomMode {still, changing}; // used to tell if running coroutines should exit
	public enum QueType {wait, forceFocus, moveFocus, followFocus, forceZoom, changeZoom};	// type of command, used with QueCards and CameraSequence

	/// <summary>
	/// Hold information about a command to give to the camera, used with the CameraQueue and CameraSequence
	/// </summary>
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

	/// <summary>
	/// Holds a list of commands to give to the camera
	/// </summary>
	public class CameraQueue {
		private Queue<QueCard> queue;

		/// <summary>
		/// Constructor
		/// </summary>
		public CameraQueue () {
			queue = new Queue<QueCard>();
		}

		/// <summary>
		/// Dequeues an instruction
		/// </summary>
		/// <returns></returns>
		public QueCard Dequeue() {
			return queue.Dequeue();
		}

		/// <summary>
		/// Returns true if queue is empty
		/// </summary>
		/// <returns></returns>
		public bool Empty() {
			return (queue.Count == 0);
		}

		/// <summary>
		/// Adds a time to wait before giving next command to camera to the queue
		/// </summary>
		/// <param name="waitTime"></param>
		public void AddWait(float waitTime) {
			queue.Enqueue(new QueCard(QueType.wait, null, waitTime, 0.0f));
		}

		/// <summary>
		/// Adds a ForceFocus command to the queue
		/// </summary>
		/// <param name="newFocus"></param>
		public void AddForceFocus(GameObject newFocus) {
			queue.Enqueue(new QueCard(QueType.forceFocus, newFocus, 0.0f, 0.0f));
		}

		/// <summary>
		/// Adds a MoveFocus command to the queue
		/// </summary>
		/// <param name="newFocus"></param>
		/// <param name="moveTime"></param>
		public void AddMoveFocus(GameObject newFocus, float moveTime) {
			queue.Enqueue(new QueCard(QueType.moveFocus, newFocus, moveTime, 0.0f));
		}

		/// <summary>
		/// Adds a FollowFocus command to the queue
		/// </summary>
		/// <param name="newFocus"></param>
		/// <param name="followTime"></param>
		public void AddFollowFocus(GameObject newFocus, float followTime) {
			queue.Enqueue(new QueCard(QueType.followFocus, newFocus, followTime, 0.0f));
		}

		/// <summary>
		/// Adds a ForceZoom command to the queue
		/// </summary>
		/// <param name="newZoom"></param>
		public void AddForceZoom(float newZoom) {
			queue.Enqueue(new QueCard(QueType.forceZoom, null, 0.0f, newZoom));
		}

		/// <summary>
		/// Adds a ChangeZoom command to the queue
		/// </summary>
		/// <param name="newZoom"></param>
		/// <param name="zoomTime"></param>
		public void AddChangeZoom(float newZoom, float zoomTime) {
			queue.Enqueue(new QueCard(QueType.changeZoom, null, zoomTime, newZoom));
		}
	}

	private MoveMode moveMode = MoveMode.still; // used to tell if running coroutines should exit
	private ZoomMode zoomMode = ZoomMode.still; // used to tell if running coroutines should exit
	private bool sequenceMode = false;  // used to tell if running coroutines should exit
	//private bool cameraShake = false;	// will be used if a camerashake method is added
	private int numMoveCoroutine = 0;   // number of move coroutines called since last move coroutine could finish normally, is used to know which coroutines to shut-down when more coroutines are called when one is still running.
	private int numFollowCoroutine = 0; // ditto
	private int numZoomCoroutine = 0;   //ditto
	private int numCamSequence = 0;

	//public GameObject[] testObject;	// used for testing
	#endregion



	#region Testing	// testing stuff
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
	*/

		/*
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
	*/

		/*
	IEnumerator<object> followTest() {
		yield return new WaitForSeconds(1);

		StartCoroutine(Follow(testObject[4], 5.0f));

		yield return new WaitForSeconds(7);

		StartCoroutine(Follow(testObject[4]));

		yield break;
	}
	*/

	/*
	IEnumerator<object> TestSequence() {
		sequenceTest();

		yield return new WaitForSeconds(4.0f);

		Debug.Log("Testing sequence interupt");
		StartCoroutine(MoveFocus(testObject[3], 2.0f, false));

		yield break;
	}
	/*

		/*
	public void sequenceTest() {
		CameraQueue testQueue1 = new CameraQueue();
		testQueue1.AddForceZoom(6.0f);
		testQueue1.AddChangeZoom(2.0f, 5.0f);
		testQueue1.AddWait(5.0f);

		testQueue1.AddChangeZoom(2.0f, 8.0f);
		testQueue1.AddForceFocus(testObject[0]);
		testQueue1.AddMoveFocus(testObject[1], 8.0f);
		testQueue1.AddWait(8.0f);

		
		testQueue1.AddMoveFocus(testObject[3], 5.0f);
		testQueue1.AddChangeZoom(2.0f, 5.0f);
		testQueue1.AddWait(2.5f);
		testQueue1.AddMoveFocus(testObject[2], 2.5f);
		testQueue1.AddWait(2.5f);

		
		testQueue1.AddFollowFocus(testObject[4], 4.0f);
		for (int i = 0; i < 4; i++) {
			testQueue1.AddForceZoom(5.0f);
			testQueue1.AddWait(0.5f);
			testQueue1.AddForceZoom(4.5f);
			testQueue1.AddWait(0.5f);
		}
		
		StartCoroutine(CameraSequence(testQueue1));
	}
	*/

		/*
	public void lookReturnTest() {
		ForceFocus(testObject[0]);
		ForceZoom(6.0f);
		StartCoroutine(LookAndReturn(testObject[1], 2.0f, 2.0f, 4.0f, 2.0f));
	}
*/
	#endregion

	#region Methods
	void Start() {
		thisCamera = this.GetComponent<Camera>();	// set these if they haven't been set
		cameraTransform = this.GetComponent<Transform>();
		thisCamera.orthographic = true; // camera must be in orthographic mode
	}

	/// <summary>
	/// Makes camera instantly look at given object.
	/// </summary>
	/// <param name="newFocus"></param>
	/// <param name="zoom"></param>
	public void ForceFocus(GameObject newFocus, bool sequence = false) {
		//Debug.Log("Forcing focus");
		moveMode = MoveMode.still;
		if (!sequence && sequenceMode) {	// if this wasn't called by a sequence, it should interupt all active sequences
			Stop();
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

		int thisCoroutine;  // what number this move coroutine is since the last normal move coroutine end

		if (numMoveCoroutine == 0) {	// set the number of move coroutines since the last normal coroutine end, and the number of this coroutine in particular
			numMoveCoroutine = 1;
			thisCoroutine = 1;
		} else {
			numMoveCoroutine++;
			thisCoroutine = numMoveCoroutine;
		}

		//Debug.Log("Starting MoveFocus: " + thisCoroutine);
		if (!sequence && sequenceMode) {    // if this wasn't called by a sequence, it should interupt all active sequences
			Stop();
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
		int thisCoroutine;  // what number this follow coroutine is since the last normal follow coroutine end

		if (numFollowCoroutine == 0) {  // set the number of follow coroutines since the last normal follow coroutine end, and the number of this coroutine in particular
			numFollowCoroutine = 1;
			thisCoroutine = 1;
		} else {
			numFollowCoroutine++;
			thisCoroutine = numFollowCoroutine;
		}
		if (!sequence && sequenceMode) {    // if this wasn't called by a sequence, it should interupt all active sequences
			Stop();
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
		if (!sequence && sequenceMode) {    // if this wasn't called by a sequence, it should interupt all active sequences
			Stop();
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
		int thisCoroutine;  // what number this zoom coroutine is since the last normal zoom coroutine end

		if (numZoomCoroutine == 0) {    // set the number of zoom coroutines since the last normal zoom coroutine end, and the number of this coroutine in particular
			numZoomCoroutine = 1;
			thisCoroutine = 1;
		} else {
			numZoomCoroutine++;
			thisCoroutine = numZoomCoroutine;
		}
		if (!sequence && sequenceMode) {    // if this wasn't called by a sequence, it should interupt all active sequences
			Stop();
		}
		//Debug.Log("Start ChangeZoom: " + thisCoroutine);

		zoomMode = ZoomMode.changing;

		float startZoom = cameraZoom;
		float endZoom = newZoom;

		float counter = 0.0f;
		bool loop = true;

		while (loop) {

			if ((counter > 1.0f) | (zoomMode != ZoomMode.changing)) { // if coroutine is running to long, or does not match current mode, end.
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
			if (zoomMode == ZoomMode.changing) {    // only change mode back to still if 
				zoomMode = ZoomMode.still;
				//Debug.Log("LastChangeZoom closed");
			}
		}

		yield break;
	}

	/// <summary>
	/// Stops whatever coroutines may be running
	/// </summary>
	public void Stop() {
		moveMode = MoveMode.still;
		zoomMode = ZoomMode.still;
		sequenceMode = false;
	}

	/// <summary>
	/// Accepts a queue of commands to give to the camera
	/// </summary>
	/// <param name="queue"></param>
	/// <returns></returns>
	IEnumerator<object> CameraSequence(CameraQueue queue) {
		int thisCoroutine;  // what number this sequence coroutine is since the last normal sequence coroutine end

		if (numCamSequence == 0) {  // set the number of sequence coroutines since the last normal sequence coroutine end, and the number of this coroutine in particular
			numCamSequence = 1;
			thisCoroutine = 1;
		} else {
			numCamSequence++;
			thisCoroutine = numCamSequence;
		}

		//Debug.Log("Starting sequence: " + thisCoroutine);

		Stop();	// stops any existing stuff that might mess up the sequence

		sequenceMode = true;
		bool loop = true;

		while (loop && !queue.Empty()) {	// only loop while other exit conditions are not met, and queue still has commands to give
			if (!sequenceMode) {
				loop = false; 
				//Debug.Log("Not sequence mode, exiting loop");
			} else if (thisCoroutine != numCamSequence) { // if this is not the newest coroutine, end
				loop = false;
				//Debug.Log("Not newest coroutine, exiting loop");
			} else {
				QueCard card = queue.Dequeue();	// get next command
				switch(card.type) {	// give next command
					case QueType.wait:
						yield return new WaitForSeconds(card._float1);
						//Debug.Log("Wait");
						break;
					case QueType.forceFocus:
						ForceFocus(card._object, true);
						//Debug.Log("ForceFocus");
						yield return null;
						break;
					case QueType.moveFocus:
						StartCoroutine(MoveFocus(card._object, card._float1, true));
						//Debug.Log("MoveFocus");
						break;
					case QueType.followFocus:
						StartCoroutine(FollowFocus(card._object, card._float1, true));
						//Debug.Log("FollowFocus");
						break;
					case QueType.forceZoom:
						ForceZoom(card._float2, true);
						//Debug.Log("ForceZoom");
						yield return null;
						break;
					case QueType.changeZoom:
						StartCoroutine(ChangeZoom(card._float2, card._float1, true));
						//Debug.Log("ChangeZoom");
						break;
					default:
						yield return null;
						break;
				}
			}
		}
		//Debug.Log("Ending sequence: " + thisCoroutine);

		if (thisCoroutine == numCamSequence) {    // only change mode back to still if this is the newest coroutine ending
			numCamSequence = 0;
			if (sequenceMode == true) {
				sequenceMode = false;
				//Debug.Log("Ending last sequence: " + thisCoroutine);
				//Debug.Log("Last MoveFocus closed");
			}
		}

		yield break;
	}

	/// <summary>
	/// A prewritten sequence of commands to give to the camera. Looks at a given object for a given amount of time, then goes back to what it was looking at before.
	/// </summary>
	/// <param name="lookAtObject"></param>
	/// <param name="panTime"></param>
	/// <param name="lookZoom"></param>
	/// <param name="lookTime"></param>
	/// <param name="returnTime"></param>
	/// <returns></returns>
	IEnumerator<object> LookAndReturn(GameObject lookAtObject, float panTime, float lookZoom, float lookTime, float returnTime) {

		CameraQueue lookAtQueue = new CameraQueue();

		GameObject originalFocus = cameraFocus;
		float originalZoom = cameraZoom;

		lookAtQueue.AddChangeZoom(lookZoom, panTime);	// pan to given object with given zoom, in given time
		lookAtQueue.AddMoveFocus(lookAtObject, panTime);
		lookAtQueue.AddWait(panTime);

		lookAtQueue.AddFollowFocus(lookAtObject, lookTime);	// look at object for given time
		lookAtQueue.AddWait(lookTime);

		lookAtQueue.AddChangeZoom(originalZoom, returnTime);	// pan back to original object with original zoom in given time
		lookAtQueue.AddMoveFocus(originalFocus, returnTime);

		StartCoroutine(CameraSequence(lookAtQueue));
		yield break;
	}

	#endregion
}
