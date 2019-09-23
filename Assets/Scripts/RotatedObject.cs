using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotatedObject : MonoBehaviour {

    public float timeToRotate360 = 5.0f;

    // Use this for initialization
    void Start () {
       // StartCoroutine(Rotate());
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    IEnumerator Rotate()
    {
        while (true)
        {
            Quaternion startRot = transform.rotation;
            float t = 0.0f;
            while (t < timeToRotate360)
            {
                t += Time.deltaTime;
                transform.rotation = startRot * Quaternion.AngleAxis(t / timeToRotate360 * -360f, Vector3.forward);
                yield return null;
            }
            transform.rotation = startRot;
        }
    }

}
