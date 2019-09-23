using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GradientScroll : MonoBehaviour {

    public Transform m_Gradient;
    public Transform m_Gradient2;
    public float m_fGradientSpeed;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        ScrollGradient();
    }

    void ScrollGradient()
    {
        m_Gradient.transform.position =
            new Vector3(m_Gradient.transform.position.x, m_Gradient.transform.position.y + (m_fGradientSpeed * Time.smoothDeltaTime), m_Gradient.transform.position.z);
        m_Gradient2.transform.position =
            new Vector3(m_Gradient2.transform.position.x, m_Gradient2.transform.position.y + (m_fGradientSpeed * Time.smoothDeltaTime), m_Gradient2.transform.position.z);

        if (m_Gradient.transform.position.y >= 340.0f)
        {
            m_Gradient.transform.position = new Vector3(m_Gradient.transform.position.x, -307.0f, m_Gradient.transform.position.z);
        }

        if (m_Gradient2.transform.position.y >= 340.0f)
        {
            m_Gradient2.transform.position = new Vector3(m_Gradient2.transform.position.x, -307.0f, m_Gradient2.transform.position.z);
        }
    }
}
