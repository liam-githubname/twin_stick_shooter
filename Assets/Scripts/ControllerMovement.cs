using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ControllerMovement : MonoBehaviour
{

  private Rigidbody2D rb;
  private Vector3 move;
  private Vector2 controllerValues;
  public float maxVelocity;
  private float controllerDeadzone = 0.075f;
  public float moveSpeed;
  public float rotateSmoothing;
  private float previousAngle;
  public float rotationDeadzone;
  PlayerControls controls;

  void Awake()
  {
    rb = GetComponent<Rigidbody2D>();
    controls = new PlayerControls();
  }

  void OnEnable()
  {
    controls.Gameplay.Enable();
    controls.Gameplay.Move.performed += ctx => move = ctx.ReadValue<Vector2>();
    controls.Gameplay.Move.canceled += ctx => move = Vector2.zero;
  }

  void OnDisable()
  {
    controls.Gameplay.Disable();
    controls.Gameplay.Move.performed -= ctx => move = ctx.ReadValue<Vector2>();
    controls.Gameplay.Move.canceled -= ctx => move = Vector2.zero;
  }

  void FixedUpdate()
  {
    HandleRotation();
    HandleMovement();
  }

  void HandleRotation()
  {
    rb.velocity = Vector2.ClampMagnitude(rb.velocity, maxVelocity);
    float newAngle = Mathf.Atan2(move.y, move.x) * Mathf.Rad2Deg;
    previousAngle = Mathf.Abs(newAngle - previousAngle) > rotationDeadzone ? newAngle : previousAngle;
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
}
