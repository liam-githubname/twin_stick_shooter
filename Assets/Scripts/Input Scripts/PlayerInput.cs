using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{

  // Start is called before the first frame update
  Ship_Properties shipController;
  SpaceshipInteraction shipInteractions;
  public PlayerControls controls;
  public Vector2 move;
  public Vector2 aim;
  public bool isBraking;
  public bool isTrigger;
  public bool isBoosting;
  public bool isMining;
  public Animator animinator;
    void Awake()
  {
    controls = new PlayerControls();
    shipController = GetComponent<Ship_Properties>();
    shipInteractions = GetComponent<SpaceshipInteraction>();
  }

  void OnEnable()
  {
    controls.Gameplay.Enable();
    controls.Gameplay.Move.performed += ctx => move = ctx.ReadValue<Vector2>();
    controls.Gameplay.Move.canceled += ctx => move = Vector2.zero;
    controls.Gameplay.Aim.performed += ctx => aim = ctx.ReadValue<Vector2>();
    controls.Gameplay.Aim.canceled += ctx => aim = Vector2.zero;
    }
  void OnDisable()
  {
    controls.Gameplay.Disable();
    controls.Gameplay.Move.performed -= ctx => move = ctx.ReadValue<Vector2>();
    controls.Gameplay.Move.canceled -= ctx => move = Vector2.zero;
    controls.Gameplay.Aim.performed -= ctx => aim = ctx.ReadValue<Vector2>();
    controls.Gameplay.Aim.canceled -= ctx => aim = Vector2.zero;
  }

  // Update is called once per frame
  void Update()
  {
    animinator=GetComponent<Animator>();
    if (animinator.GetBool("isDead") == true)
        {
            controls.Disable();
        }
    isTrigger = controls.Gameplay.Trigger.IsPressed();
    isBraking = controls.Gameplay.Brake.IsPressed();
    isBoosting = controls.Gameplay.Boost.IsPressed();
    isMining = controls.Gameplay.Mine.IsPressed();
    shipController.SetInput(move, isBraking, isBoosting);
    shipInteractions.SetInput(aim, isTrigger, isMining);
  }
}
