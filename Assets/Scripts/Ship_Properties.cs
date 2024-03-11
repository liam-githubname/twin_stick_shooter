using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ship_Properties : MonoBehaviour
{

  [Header("Space Ship Settings")]
  public Rigidbody2D rb;
  private Vector2 move;
  private Vector2 controllerValues;
  public float maxVelocity;
  private float controllerDeadzone;
  public float moveSpeed;
  public float rotateSmoothing;
  public float rotationDeadzone;
  public float brakeSpeed;
  public float boostMultiplier;
  private float boostDuration;
  private bool isBraking;
  private bool isBoosting;
  private PlayerControls controls;


  void Start()
  {
    rb = GetComponent<Rigidbody2D>();
  }

  private void FixedUpdate()
  {
    if (rb.velocity.magnitude > maxVelocity && !isBoosting) rb.velocity = Vector2.ClampMagnitude(rb.velocity, maxVelocity);
    if (rb.velocity.magnitude > boostMultiplier*maxVelocity && isBoosting) rb.velocity = Vector2.ClampMagnitude(rb.velocity, boostMultiplier*maxVelocity);

    HandleRotation();
    HandleMovement();

    if (isBraking) {
      HandleBraking();
    }

    if (isBoosting) {
      HandleBoosting();
    }
  }

  public void setInput(Vector2 move, bool isBraking, bool isBoosting) {
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
    float newAngle = Mathf.Atan2(move.y, move.x) * Mathf.Rad2Deg;
    Quaternion newRotation = Quaternion.LookRotation(Vector3.forward, move);
    transform.rotation = Quaternion.RotateTowards(transform.rotation, newRotation, move.magnitude*rotateSmoothing * Time.fixedDeltaTime);
  }

  void HandleMovement()
  {
    if (Mathf.Abs(move.x) > controllerDeadzone || Mathf.Abs(move.y) > controllerDeadzone) {
      rb.AddRelativeForce(new Vector2(0, move.magnitude)* moveSpeed, ForceMode2D.Force);
    }
  }

  void HandleBraking()
  {
      rb.AddRelativeForce(new Vector2(0, -brakeSpeed), ForceMode2D.Force);
  }
}
