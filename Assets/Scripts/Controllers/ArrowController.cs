using DG.Tweening;
using System.Collections;
using UnityEngine;

public class ArrowController : MonoBehaviour
{
    public GameManager m_GameManager;

    public Transform m_MiddleOfCircle;

    public GameObject m_ObjectToAffect;
    public GameObject m_Prefab;
    private GameObject m_WaveMovement;

    private Color m_IndicatorOrignalColor;
    private Color m_IndicatorPressedColor;

    private Quaternion m_TargetRotation;
    private AudioSource m_HitSound;

    private float m_TimeToRotate360 = 5.0f;
    public float m_Speed;
    public bool m_bHitCollider = false;

    void Start()
    {
        m_HitSound = GetComponent<AudioSource>();
        m_ObjectToAffect.GetComponentInChildren<LineRenderer>().enabled = false;
        StartCoroutine(Rotate());
        m_IndicatorOrignalColor = transform.parent.gameObject.GetComponent<SpriteRenderer>().color;
        m_IndicatorPressedColor = new Color(0.0f, 0.0f, 0.0f);
    }

    void Update ()
    {
       if (!m_GameManager.GetIsPaused())
        {
            FaceDirection();
            HitObject();

            if (m_bHitCollider)
            {
                m_bHitCollider = false;
                m_HitSound.PlayOneShot(m_HitSound.clip);
            }
        }
    }

    Vector3 GetDirection()
    {
        Vector3 dir = m_MiddleOfCircle.position - transform.position;
        dir.Normalize();
        return dir;
    }

    public void FaceDirection()
    {
        Vector3 dir = GetDirection();
        float fDotProduct = Vector3.Dot(dir, Vector3.up);
        float fAngle = Mathf.Acos(fDotProduct) * Mathf.Rad2Deg;

        Quaternion toRotation = Quaternion.FromToRotation(Vector3.up, dir);

        transform.rotation = toRotation;
    }

    IEnumerator Rotate()
    {
        while (true)
        {
            Quaternion startRot = transform.parent.rotation;
            float t = 0.0f;
            while (t < m_TimeToRotate360)
            {
                t += Time.deltaTime;
                transform.parent.rotation = startRot * Quaternion.AngleAxis(t / m_TimeToRotate360 * -360f, Vector3.forward);
                yield return null;
            }
            transform.parent.rotation = startRot;
        }
    }

    void HitObject()
    {
        m_ObjectToAffect.GetComponentInChildren<LineRenderer>().SetPosition(0, GetDirection() /2.0f);
        m_ObjectToAffect.GetComponentInChildren<LineRenderer>().SetPosition(1, GetDirection() * 2.5f);

        if (Input.GetKeyDown(KeyCode.Space))
        {
            m_ObjectToAffect.GetComponentInChildren<LineRenderer>().enabled = true;
            StopAllCoroutines();
            m_TimeToRotate360 = 10.0f;
            StartCoroutine(Rotate());
        }

        if (Input.GetKeyUp(KeyCode.Space))
        {
            m_ObjectToAffect.GetComponentInChildren<LineRenderer>().enabled = false;

            Animator anim = transform.parent.GetComponent<Animator>();                
            anim.SetTrigger("SpacePressed");

            CircleCollider2D mainCircleCollider = m_MiddleOfCircle.GetComponent<CircleCollider2D>();
            CircleCollider2D ballCollider = m_ObjectToAffect.GetComponent<CircleCollider2D>();
            if (ballCollider.IsTouching(mainCircleCollider))
            {
                StopAllCoroutines();
                m_TimeToRotate360 = 5.0f;
                StartCoroutine(Rotate());

                if (m_WaveMovement != null)
                {
                    m_WaveMovement.GetComponent<SpriteRenderer>().DOFade(0.0f, 0.5f);
                    Destroy(m_WaveMovement, 0.6f);
                }

                // instaniate particles
                m_WaveMovement = Instantiate(m_Prefab, transform.parent, true);
                m_WaveMovement.transform.rotation = this.transform.rotation;
                Vector3 tempPos = transform.parent.position;
                tempPos.y = 2.5f;
                m_WaveMovement.transform.localPosition = tempPos;
                m_WaveMovement.transform.parent = null;
                m_WaveMovement.transform.DOMove(-(m_WaveMovement.transform.localPosition), 1.0f);

                m_GameManager.AddToScore();
                m_HitSound.PlayOneShot(m_HitSound.clip);
                if (!m_ObjectToAffect.GetComponentInChildren<PlayerController>().GetIsAtEndGoal())
                {
                    Rigidbody2D rb = m_ObjectToAffect.GetComponent<Rigidbody2D>();
                    rb.velocity = Vector3.zero;
                    rb.AddForce(GetDirection() * m_Speed);
                }
            }
        }
     }
}
