using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour {

	// Use this for initialization
	void Start () {
        
        StartCoroutine(MoveTo(new Vector3(4.98f, 7.8f, 0)));
        
        //StartCoroutine(Death());
    }

    private float Acceleration(float input)
    {
        float clamped = Mathf.Clamp01(input);
        return ((3.0f * clamped * clamped) - (2.0f * clamped * clamped * clamped));
    }

    public IEnumerator<Object> MoveTo(Vector3 location)
    {
        Vector3 startPos = gameObject.transform.position;
        float objectsRotation = gameObject.transform.rotation.z - (Mathf.PI / 2);
        float rotation = Mathf.Atan((location.y - startPos.y) / (location.x - startPos.x));
        for (float time = 0; time < 1; time += Time.deltaTime)
        {
            gameObject.transform.Rotate(0, 0, objectsRotation - rotation);
            yield return null;
        }

        for (float time = 0; time < 1; time += Time.deltaTime)
        {
            gameObject.transform.position = Vector3.Lerp(startPos, location, Acceleration(time));
            yield return null;
        }
    }

    public IEnumerator<Object> Death()
    {
        Vector3 startRot = new Vector3(gameObject.transform.rotation.x, gameObject.transform.rotation.y, gameObject.transform.rotation.z);
       
        for (float time = 0; time < 1; time += Time.deltaTime)
        {
            gameObject.transform.Rotate(0, 0, 730);
            yield return null;
        }
       
    }
}
