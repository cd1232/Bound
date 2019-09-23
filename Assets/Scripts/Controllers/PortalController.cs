using System.Collections;
using UnityEngine;

public class PortalController : MonoBehaviour
{
    public Transform m_ConnectedPortal;
    private bool m_BallJustTranported = false;
    private PlayerController m_Ball;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Ball") && !m_ConnectedPortal.GetComponent<PortalController>().m_BallJustTranported)
        {
            m_Ball = collision.GetComponent<PlayerController>();
            m_BallJustTranported = true;
            m_ConnectedPortal.GetComponent<PortalController>().SetBallJustTranported(true);
            m_Ball.transform.position = m_ConnectedPortal.transform.position;
            StartCoroutine(ResetPortals());
        }
    }

    IEnumerator ResetPortals()
    {
        yield return new WaitForSeconds(1.0f);
        m_BallJustTranported = false;
        m_ConnectedPortal.GetComponent<PortalController>().SetBallJustTranported(false);
    }

    public void SetBallJustTranported(bool _BallJustTranported)
    {
        m_BallJustTranported = _BallJustTranported;
    }

    public bool GetBallJustTranported()
    {
        return m_BallJustTranported;
    }
}
