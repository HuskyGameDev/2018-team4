using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour {

	// Use this for initialization
	void Start () {
        
        //StartCoroutine(MoveTo(new Vector3(20, 0, 20)));
        StartCoroutine(Death());
    }

    private float Acceleration(float input)
    {
        float clamped = Mathf.Clamp01(input);
        return ((3.0f * clamped * clamped) - (2.0f * clamped * clamped * clamped));
    }

    public IEnumerator<Object> MoveTo(Vector3 location)
    {
        Vector3 startPos = gameObject.transform.position;
        for (float time = 0; time < 1; time += Time.deltaTime)
        {
            gameObject.transform.position = Vector3.Lerp(startPos, location, Acceleration(time));
            yield return null;
        }
    }

    //Quaternions are confusing...
    public IEnumerator<Object> Death()
    {
        Vector3 startRot = new Vector3(gameObject.transform.rotation.x, gameObject.transform.rotation.y, gameObject.transform.rotation.z);
       
        for (float time = 0; time < 1; time += Time.deltaTime)
        {
            Vector3 newRot = new Vector3(0, 0, 90);
            Vector3 tempRot = Vector3.Lerp(startRot, newRot, Acceleration(time));
            gameObject.transform.rotation.z = tempRot.z;
            yield return null;
        }
       
    }
}
