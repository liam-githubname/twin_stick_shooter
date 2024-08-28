using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ship_Properties : MonoBehaviour
{

  [Header("Space Ship Settings")]
  public Rigidbody2D rb;
  private Vector2 move;
  public float maxVelocity;
  private float controllerDeadzone;
  public float moveSpeed;
  public float rotateSmoothing;
  public float rotationDeadzone;
  public float brakeSpeed;
  public float boostMultiplier;
  private bool isBraking;
  private bool isBoosting;
  Animator animator;
  public PolygonCollider2D coll;
  void Start()
  {
    rb = GetComponent<Rigidbody2D>();
    animator= GetComponent<Animator>();
    coll= GetComponent<PolygonCollider2D>();
  }

  private void FixedUpdate()
  {
    if (rb.velocity.magnitude > maxVelocity && !isBoosting) rb.velocity = Vector2.ClampMagnitude(rb.velocity, maxVelocity);
    if (rb.velocity.magnitude > boostMultiplier * maxVelocity && isBoosting) rb.velocity = Vector2.ClampMagnitude(rb.velocity, boostMultiplier * maxVelocity);
    if (animator.GetBool("isDead") == true) { coll.enabled = false; }

    HandleRotation();
    HandleMovement();

    if (isBraking)
    {
      HandleBraking();
    }

    if (isBoosting)
    {
      HandleBoosting();
    }
  }

  public void SetInput(Vector2 move, bool isBraking, bool isBoosting)
  {
    this.move = move;
    this.isBraking = isBraking;
    this.isBoosting = isBoosting;
  }

  void HandleBoosting()
  {
    rb.AddRelativeForce(new Vector2(0, move.magnitude) * boostMultiplier * moveSpeed, ForceMode2D.Force);
  }

  void HandleRotation()
  {
    //float newAngle = Mathf.Atan2(move.y, move.x) * Mathf.Rad2Deg;
    Quaternion newRotation = Quaternion.LookRotation(Vector3.forward, move);
    transform.rotation = Quaternion.RotateTowards(transform.rotation, newRotation, move.magnitude * rotateSmoothing * Time.fixedDeltaTime);
  }

  void HandleMovement()
  {
    if (Mathf.Abs(move.x) > controllerDeadzone || Mathf.Abs(move.y) > controllerDeadzone)
    {
      rb.AddRelativeForce(new Vector2(0, move.magnitude) * moveSpeed, ForceMode2D.Force);
    }
  }

  void HandleBraking()
  {
    rb.AddRelativeForce(new Vector2(0, -brakeSpeed), ForceMode2D.Force);
  }

}
