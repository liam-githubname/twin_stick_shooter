// using UnityEngine;

// public class Bomb : MonoBehaviour
// {
//     public float bombDamage = 300f; // Set this to the amount of damage a bomb does

//     void OnCollisionEnter2D(Collision2D collision)
//     {
//         // Check for collision with the player ship
//         if (collision.gameObject.tag == "ship")
//         {

//             DamageSystem playerHealth = collision.gameObject.GetComponent<DamageSystem>();
//             if (playerHealth != null)
//             {
//                 // Cause damage to the player
//                 playerHealth.TakeDamage(bombDamage);
//             }

//             // Trigger bomb effects here (e.g., explosion)

//             // Destroy the bomb
//             Destroy(gameObject);
//         }
//         else
//             Physics2D.IgnoreCollision(collision.collider, gameObject.GetComponent<Collider2D>());

//     }
// }

using UnityEngine;

public class Bomb : MonoBehaviour
{
    public float bombDamage = 300f; // Set this to the amount of damage a bomb does

    void Start()
    {
        // Ignore collisions with all other objects except "ship" by setting appropriate layers or using tags
        GameObject[] allObjects = GameObject.FindObjectsOfType<GameObject>();
        foreach (GameObject obj in allObjects)
        {
            if (obj.tag != "ship" && obj.GetComponent<Collider2D>() != null)
            {
                Physics2D.IgnoreCollision(gameObject.GetComponent<Collider2D>(), obj.GetComponent<Collider2D>());
            }
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        // Check for collision with the player ship
        if (collision.gameObject.tag == "ship")
        {
            DamageSystem playerHealth = collision.gameObject.GetComponent<DamageSystem>();
            if (playerHealth != null)
            {
                // Cause damage to the player
                playerHealth.TakeDamage(bombDamage);
            }

            // Trigger bomb effects here (e.g., explosion)

            // Destroy the bomb
            Destroy(gameObject);
        }
    }
}
