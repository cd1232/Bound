using UnityEngine;

public class BorderController : MonoBehaviour
{    
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Ball"))
        {
            Rigidbody2D rb = collision.gameObject.GetComponent<Rigidbody2D>();
            Animator anim = collision.gameObject.GetComponentInChildren<Animator>();

            Vector3 normal = (transform.position - collision.gameObject.transform.position).normalized;
            Vector3 originalDirection = rb.velocity.normalized;

            rb.velocity = Vector2.Reflect(rb.velocity, normal);
            rb.velocity *= 0.8f;

            AudioSource source = collision.gameObject.GetComponentInChildren<AudioSource>();
            source.PlayOneShot(source.clip);

            anim.SetTrigger("HitWall");
        }
    }
}
