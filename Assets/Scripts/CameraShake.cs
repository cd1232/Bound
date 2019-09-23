using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour {

    public bool m_bShouldShake = false;

    private float m_fShakeY = 0.8f;
    private float m_fShakeYSpeed = 0.8f;
    private float m_fOriginalShakeY;
    public void setShake(float _fSomeY)
    {       
        m_fShakeY = _fSomeY;
        m_fOriginalShakeY = m_fShakeY;
    }

	// Use this for initialization
	void Start () {
        m_fOriginalShakeY = m_fShakeY;
    }
	
	// Update is called once per frame
	void Update () {
        if (m_bShouldShake)
        {
            Vector2 newPos = new Vector2(0, m_fShakeY);
            if (m_fShakeY < 0)
            {
                m_fShakeY *= m_fShakeYSpeed;
            }
            m_fShakeY = -m_fShakeY;
            transform.Translate(newPos, Space.Self);

            if (Mathf.Abs(m_fShakeY) < 0.001f)
            {
                m_bShouldShake = false;
                m_fShakeY = m_fOriginalShakeY;
            }
        }
    }
}
