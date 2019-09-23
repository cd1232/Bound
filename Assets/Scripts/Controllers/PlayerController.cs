using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Transform m_EndGoal;
    private PortalController m_Portal;
    private bool m_bIsAtEndGoal = false;

	void Update ()
    {		
        if (m_bIsAtEndGoal)
        {
            transform.position = Vector3.Lerp(transform.position, m_EndGoal.position, 1.5f * Time.deltaTime);

            if (Vector3.Distance(transform.position, m_EndGoal.position) > Vector3.kEpsilon)
            {
                m_EndGoal.GetComponent<EndGoalController>().SetDone();
            }
        }
	}

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("EndGoal"))
        {
            m_bIsAtEndGoal = true;
            m_EndGoal = collision.transform;
            GetComponent<Rigidbody2D>().velocity = new Vector2(0.0f, 0.0f);
        }
    }

    public bool GetIsAtEndGoal()
    {
        return m_bIsAtEndGoal;
    }

    public void ResetIsAtEndGoal()
    {
        m_bIsAtEndGoal = false;
    }
}
