using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EndGoalController : MonoBehaviour
{
    public List<Transform> m_ObjectsToActivate;
    public GameManager m_GameManager;
    public PlayerController m_PlayerController;
    public Text m_LevelText;


    private AudioSource m_EndGoalSound;
    private bool m_bDoneHasBeenCalled = false;

	// Use this for initialization
	void Start ()
    {
        m_EndGoalSound = GetComponent<AudioSource>();
    }

    public void SetDone()
    {
        if (!m_bDoneHasBeenCalled)
        {
            m_EndGoalSound.Play();

            foreach (Transform t in m_ObjectsToActivate)
            {
                Rigidbody2D rb = t.GetComponent<Rigidbody2D>();

                rb.bodyType = RigidbodyType2D.Dynamic;
                rb.gravityScale = 1;

                Vector3 randomForce = new Vector3(Random.Range(-1.0f, 1.0f), Random.Range(-1.0f, 1.0f), 0.0f);
                randomForce.Normalize();
                
                rb.AddForceAtPosition(transform.position + randomForce * 7.0f, transform.position, ForceMode2D.Force);

                foreach (BoxCollider2D collider in t.GetComponents<BoxCollider2D>())
                {
                    collider.isTrigger = true;
                    collider.enabled = false;
                }                
            }

            m_bDoneHasBeenCalled = true;

            // fade level out
            m_LevelText.GetComponent<Animator>().enabled = false;
            m_LevelText.DOFade(0.0f, 0.5f);
            
            StartCoroutine(SetLevel());
        }
    }

    IEnumerator SetLevel()
    {
        // fade level in
        yield return new WaitForSeconds(2.5f);
        string tempString = "Level " + (m_GameManager.GetCurrentLevel() + 1);
        m_LevelText.text = tempString;
        m_LevelText.GetComponent<Text>().DOFade(1.0f, 1.0f);
        m_PlayerController.ResetIsAtEndGoal();
        m_bDoneHasBeenCalled = false;
        m_GameManager.LoadLevel(m_GameManager.GetCurrentLevel() + 1);
    }

}
