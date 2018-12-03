using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraTest : MonoBehaviour {

	public GameObject[] testObject; // used for testing
	public GameObject testCamera;

	// Use this for initialization
	void Start () {
		CameraController.CameraQueue testQueue1 = new CameraController.CameraQueue();
		testQueue1.AddForceZoom(6.0f);
		testQueue1.AddForceFocus(testObject[0]);
		testQueue1.AddChangeZoom(2.0f, 2.0f);
		testQueue1.AddWait(2.0f);

		testQueue1.AddCameraShake(1.0f, 0.2f);
		testQueue1.AddWait(2.0f);

		//testQueue1.AddChangeZoom(2.0f, 8.0f);
		testQueue1.AddChangeZoom(3.0f, 2.0f);
		testQueue1.AddMoveFocus(testObject[1], 1.0f);
		testQueue1.AddCameraShake(0.5f, 0.2f);
		testQueue1.AddWait(1.0f);

		testQueue1.AddMoveFocus(testObject[3], 1.0f);
		testQueue1.AddChangeZoom(2.0f, 1.0f);
		testQueue1.AddWait(1.0f);
		//testQueue1.AddMoveFocus(testObject[2], 1.0f);
		//testQueue1.AddWait(1.0f);

		testQueue1.AddMoveFocus(testObject[4], 8.0f);
		testQueue1.AddChangeZoom(10.0f, 8.0f);
		testQueue1.AddCameraShake(0.2f, 4.0f);
		testQueue1.AddWait(8.0f);


		testQueue1.AddFollowFocus(testObject[4], 4.0f);
		testQueue1.AddChangeZoom(4.0f, 4.0f);
		testQueue1.AddCameraShake(0.5f, 4.0f);
		testQueue1.AddWait(4.0f);

		if (testCamera != null) {
			StartCoroutine(testCamera.GetComponent<CameraController>().CameraSequence(testQueue1));
		}
	}

}
