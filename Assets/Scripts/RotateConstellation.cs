using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateConstellation : MonoBehaviour {

    public GameObject[] constellations;
    public float rotateSpeed = 85.0f;

    private bool bIsRotating;
    private int rotateDirection = 1;
    private float fTest;
    private float newAngle = 0.0f;

	// Use this for initialization
	void Start () {
        for (int i = 0; i < constellations.Length; ++i)
        {
            constellations[i].transform.position = SetPositionPointOnCircle(Vector3.zero, 4.0f, i);
        }        
       
	}
	
	// Update is called once per frame
	void Update () {
        float mouseScroll = Input.GetAxis("Mouse ScrollWheel");
        //Debug.Log(mouseScroll);
       

        if (mouseScroll >= 0.05f)
        {
            bIsRotating = true;
            newAngle += 90.0f;
           // rotateDirection = 1;
        }
        else if (mouseScroll <= -0.05f)
        {
            bIsRotating = true;
            newAngle += 90.0f;
           // rotateDirection = -1;
        }

        if (bIsRotating)
        {
            StartRotation();
        }
	}

    void StartRotation()
    {
        if (bIsRotating)
        {
            fTest += Time.deltaTime;
            
            for (int i = 0; i < constellations.Length; ++i)
            {
                constellations[i].transform.position = SetPositionOnCircleLerp(Vector3.zero, 4.0f, i, fTest * rotateSpeed);
            }

            if (fTest * rotateSpeed >= newAngle)
            {
                bIsRotating = false;
            }
        }
    }


    Vector3 SetPositionOnCircleLerp(Vector3 center, float radius, float startAng, float _fAng)
    {
        float ang = _fAng + (360 / 4 * startAng);
        Vector3 pos;
        pos.x = center.x + radius * Mathf.Sin(ang * Mathf.Deg2Rad);
        pos.y = center.y + radius * Mathf.Cos(ang * Mathf.Deg2Rad);
        pos.z = center.z;
        return pos * rotateDirection;
    }


    Vector3 SetPositionPointOnCircle(Vector3 center, float radius, int _iObjectNum)
    {
        float ang = (360 / constellations.Length) * _iObjectNum;
        Vector3 pos;
        pos.x = center.x + radius * Mathf.Sin(ang * Mathf.Deg2Rad);
        pos.y = center.y + radius * Mathf.Cos(ang * Mathf.Deg2Rad);
        pos.z = center.z;
        return pos;
    }
}
