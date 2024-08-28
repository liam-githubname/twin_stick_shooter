using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrosshairAim : MonoBehaviour
{
    public GameObject ship;
    public GameObject crosshairObj;
    public Vector2 aim = Vector2.zero;
    public Rigidbody2D crosshairRb;
    public float distanceFromShip = 2.5f;
    private bool hasAsteroid;
    private bool isTrigger;

    void Start()
    {
        ship = gameObject;
        crosshairObj = GameObject.Find("Crosshair");
        crosshairRb = GameObject.Find("Crosshair").GetComponent<Rigidbody2D>();
        crosshairRb.freezeRotation = true;
    }

    void FixedUpdate()
    {
        hasAsteroid = ship.GetComponent<SpaceshipInteraction>().hasAsteroid;
        isTrigger = ship.GetComponent<SpaceshipInteraction>().isTrigger;

        if (isTrigger && !hasAsteroid)
        {
            crosshairObj.SetActive(true);
            RotateAim();
        }
        else
            crosshairObj.SetActive(false);
    }

    void RotateAim()
    {
        aim = ship.GetComponent<SpaceshipInteraction>().GetAim().normalized;

        if ((aim.x > 0 || aim.x < 0) && (aim.y > 0 || aim.y < 0))
        {
            crosshairRb.position = ship.transform.position + (Vector3)aim * distanceFromShip;
        }
        else
            crosshairRb.position = ship.transform.position;

        Vector3 relativePos = ship.transform.position;
    }
}