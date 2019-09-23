using UnityEngine;

public class ObstacleController : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ball"))
        { 
            AudioSource source = collision.gameObject.GetComponentInChildren<AudioSource>();
            source.PlayOneShot(source.clip);

            Animator anim = collision.gameObject.GetComponentInChildren<Animator>();
            anim.SetTrigger("HitWall");

            Rigidbody2D rb = collision.gameObject.GetComponent<Rigidbody2D>();
            rb.velocity *= 0.8f;
        }
    }
}
