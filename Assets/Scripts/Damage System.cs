using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageSystem : MonoBehaviour
{
    public Ship_Properties ship;
    // Start is called before the first frame update
    [Header("Damage Settings")]
    private float DamageFactor=1.0f;
    private float minVelocity=0.0f;
    private float currVelocity=0.0f;
    public SpaceshipInteraction rock;
    private float currMass;
    public float damage=0.0f;

    void OnCollisionEnter2D(Collision2D col){
        currVelocity=ship.rb.velocity.x;
        currMass=ship.rb.mass;
        Debug.Log(currVelocity); //This sometimes comes out as a negative. Gotta fix it.
        if (minVelocity<=currVelocity){
            damage=currVelocity*currMass*DamageFactor+(rock.asteroid.velocity.x);
            Debug.Log(damage);
        }


    }

    void Start()
    {
        ship=gameObject.GetComponent<Ship_Properties>();
        rock=gameObject.GetComponent<SpaceshipInteraction>();
    }

    // Update is called once per frame
    void Update()
    {

    }
}
