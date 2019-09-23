using UnityEngine;

public class MouseMoveManager : MonoBehaviour
{
    private Vector3 m_MousePos;
    private AudioSource[] m_Sounds;

    void Start ()
    {
        m_MousePos = Input.mousePosition;
        m_Sounds = GetComponents<AudioSource>();
    }
	
	void Update ()
    {
        CheckMouseMovement();
    }

    void CheckMouseMovement()
    {
        if (m_MousePos != Input.mousePosition)
        {
            if (!m_Sounds[0].isPlaying)
            {
                m_Sounds[0].Play();
            }
        }
    }
}
