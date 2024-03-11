using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsteroidMovement : MonoBehaviour
{
    public Rigidbody2D rb;
    [SerializeField] float asteroidSize;
    [SerializeField] float moveSpeed = 18f;
    public Vector3 moveVector;
    public float damageAmount = 1f; 

    void Start()
    {
        Vector3 currPosition = transform.position;
        transform.position = new Vector3(currPosition.x, currPosition.y, 0);
        rb = GetComponent<Rigidbody2D>();
        rb.velocity = moveVector * moveSpeed;
    }
     private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("enemy"))
        {
            EnemyOne enemy = collision.gameObject.GetComponent<EnemyOne>();
            if (enemy != null)
            {
                enemy.TakeDamage(damageAmount);
            }
            // Optionally destroy the asteroid upon collision
            Destroy(gameObject);
        }
    }
}
