using UnityEngine;

public class Bullet : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "asteroid" || collision.gameObject.tag == "thrown_asteroid")
        {
            Destroy(gameObject); // Destroy the bullet
        }
        else if (collision.gameObject.tag != "ship")
            Physics2D.IgnoreCollision(collision.collider, gameObject.GetComponent<Collider2D>());
    }
}
